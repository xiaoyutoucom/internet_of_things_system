
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using DevExpress.Mvvm.Native;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Filtering.Templates;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraTreeList.Nodes;
using log4net;
using Microsoft.Extensions.Configuration;
using ServiceStack;
using SmartKylinApp.Common;
using SmartKylinData.BaseModel;
using SmartKylinData.IOTModel;

namespace SmartKylinApp.View.BaseConfig
{
    public partial class deviceinfo : UserControl
    {
        private ILog _log = LogManager.GetLogger("deviceinfo");
        private int CurrentId { get; set; }
        private bool Isedit { get; set; } = false;

        private List<DeviceRecord> list;
        private List<DeviceRecord> DeviceList;
        private List<string> types = new List<string>();
        private List<TreeListModel> tagdata;
        private bool ifscbm = true;
        private string oldccbh;

        public deviceinfo()
        {
            InitializeComponent();
            //禁止右键菜单
            layoutControl1.AllowCustomization = false;
            layoutControl2.AllowCustomization = false;
            layoutControl3.AllowCustomization = false;
            bar2.Manager.AllowShowToolbarsPopup = false;
            bar2.OptionsBar.AllowQuickCustomization = false;
            gridView1.OptionsMenu.EnableColumnMenu = false;
            treeListleft.OptionsMenu.EnableColumnMenu = false;
            tree_city.Properties.TreeList.OptionsMenu.EnableColumnMenu = false;
            tree_mstype.Properties.TreeList.OptionsMenu.EnableColumnMenu = false;
        }
        private void deviceinfo_Load(object sender, EventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            splashScreenManager1.SetWaitFormCaption("请稍后,数据加载中....");     // 标题
            //splashScreenManager1.SetWaitFormDescription("正在更新.....");　　　　　// 信息
            BindTree();
            treeListleft.BeforeCheckNode += TreeList1_BeforeCheckNode;
            treeListleft.AfterCheckNode += TreeList1_AfterCheckNode;
            treeListleft.NodeChanged += TreeList1_NodeChanged;
            layoutControl1.Visible = false;
            BindAgreement();
            //layoutControl2.Visible = false;
            //GetData();
            BindCityInfo();
            //左侧树默认勾选第一项,并初始化数据

            try {
                treeListleft.Nodes[0].CheckAll();
                var list = treeListleft.GetAllCheckedNodes();
            types.Clear();
            foreach (var item in list)
            {
                if (item.GetValue("ID").ToString().Length == 6)
                {
                    types.Add(item.GetValue("ID").ToString());
                }
            }
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show("初始化左侧树数据出错！");
                _log.Error("初始化左侧树数据出错，出错提示：" + ex.ToString());
            }
            GetData(types);
            splashScreenManager1.CloseWaitForm();
        }
        private void BindCityInfo()
        {
            try { 
            //绑定去换信息
            var code = ConfigHelp.Config["Application:Config:City"];
            if (code != null)
            {
                if (code.Contains("0000")) code = code.Substring(0, 2);
                if (code.Contains("00")) code = code.Substring(0, 4);
                var list = new List<CityModel>();
                list = GlobalHandler.CityInfo.Where(a => a.CITYCODE.StartsWith(code)).ToList();
                var gl = new List<TreeListModel>();
                list.Each(a => gl.Add(new TreeListModel() { ParentID = a.PID, Name = a.CITYNAME, ID = a.CITYCODE }));
                tree_city.Properties.DataSource = gl;
                tree_city.EditValue = gl.Where(q => q.ID.Substring(q.ID.Length - 2, 2) != "00").First().ID;
            }
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取区划数据出错");
                _log.Error("获取区划数据出错，出错提示：" + e.ToString());
            }
}

        private void BindAgreement()
        {
            try
            {
                var list = GlobalHandler.agreeresp.GetAllList();
                List<string> lstName = new List<string>();
                ListItem item = null;
                ComboBoxItemCollection coll = this.com_txxy.Properties.Items;
                coll.BeginUpdate();
                try
                {
                    lstName.Clear();
                    foreach (AgreementRecord ar in list)
                    {
                        if (lstName.Contains(ar.Devicecj) == false)
                        {
                            item = new ListItem(ar.Devicecj, ar.Id.ToString());
                            coll.Add(item);
                            lstName.Add(ar.Devicecj);
                        }
                    }
                }
                finally
                {
                    coll.EndUpdate();
                }
                com_txxy.SelectedIndex = 0;
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取通信协议信息数据出错");
                _log.Error("获取通信协议信息数据出错，出错提示：" + e.ToString());
            }
        }

        private void TreeList1_NodeChanged(object sender, DevExpress.XtraTreeList.NodeChangedEventArgs e)
        {
            var node = e.Node.Nodes;
            if (node.Count <= 0) return;
            var a = node[0].CheckState;
        }

        private void TreeList1_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            //设置子节点状态
            SetCheckedChildNodes(e.Node, e.Node.CheckState);
            SetCheckedParentNodes(e.Node, e.Node.CheckState);
            //获取选中状态
            var list = treeListleft.GetAllCheckedNodes();
            types.Clear();
            foreach (var item in list)
            {
                if (item.GetValue("ID").ToString().Length == 6)
                {
                    types.Add(item.GetValue("ID").ToString());
                }
            }
            BindAgreement();
            GetData(types);
            EditData();
            BindTreeMstype();
        }
        private void SetCheckedChildNodes(DevExpress.XtraTreeList.Nodes.TreeListNode node, CheckState check)
        {
            for (var i = 0; i < node.Nodes.Count; i++)
            {
                node.Nodes[i].CheckState = check;
                SetCheckedChildNodes(node.Nodes[i], check);
            }
        }

        private void SetCheckedParentNodes(DevExpress.XtraTreeList.Nodes.TreeListNode node, CheckState check)
        {
            while (true)
            {
                if (node.ParentNode == null) return;
                var b = false;
                for (var i = 0; i < node.ParentNode.Nodes.Count; i++)
                {
                    var state = (CheckState)node.ParentNode.Nodes[i].CheckState;
                    if (check.Equals(state)) continue;
                    b = true;
                    break;
                }
                node.ParentNode.CheckState = b ? CheckState.Indeterminate : check;
                node = node.ParentNode;
            }
        }

        private void TreeList1_BeforeCheckNode(object sender, DevExpress.XtraTreeList.CheckNodeEventArgs e)
        {
            //节点选中前
            e.State = e.PrevState == CheckState.Checked ? CheckState.Unchecked : CheckState.Checked;
        }

        //绑定行业信息
        private void BindTreeMstype()
        {
            try { 
            if (layoutControl1.Visible == false)
            {
                return;
            }
            var list = new List<TreeListModel>();
            var listcheck = treeListleft.GetAllCheckedNodes();
            if (listcheck.Count > 0)
            {
                foreach (var item in listcheck)
                {
                    var ID = item["ID"].ToString();
                    list.Add(new TreeListModel() { ID = item["ID"].ToString(), ParentID = item["ParentID"].ToString(), Name = item["Name"].ToString() });
                }
            }
            else
            {
                var datas = GlobalHandler.mstyperesp.GetAllList();
                if (datas == null) return;

                var dt1 = datas.Where(a => a.TYPE_KEY.ToString().Length == 2);
                dt1.Each(a => list.Add(new TreeListModel() { ID = a.TYPE_KEY, ParentID = "1", Name = a.TYPE_NAME }));
                var dt2 = datas.Where(a => a.TYPE_KEY.ToString().Length == 4);
                dt2.Each(a => list.Add(new TreeListModel() { ID = a.TYPE_KEY, ParentID = a.TYPE_KEY.Substring(0, 2), Name = a.TYPE_NAME }));
                var dt3 = datas.Where(a => a.TYPE_KEY.ToString().Length == 6);
                dt3.Each(a => list.Add(new TreeListModel() { ID = a.TYPE_KEY, ParentID = a.TYPE_KEY.Substring(0, 4), Name = a.TYPE_NAME }));
            }

            tree_mstype.Properties.DataSource = list;
            tree_mstype.Properties.DisplayMember = "Name";
            tree_mstype.Properties.ValueMember = "ID";
                if (list.Count(a => a.ID.ToString().Length == 6) < 1) return;
                tree_mstype.EditValue = list.Where(a => a.ID.ToString().Length == 6).First().ID;
                //tree_mstype.EditValue = ;
            }
            catch(Exception e)
            {
                XtraMessageBox.Show("初始化添加行业类型数据出错");
                _log.Error("初始化添加行业类型数据出错，出错提示：" + e.ToString());
            }
        }
        //绑定行业信息
        private void BindTree()
        {
            try
            { 
            var list = new List<TreeListModel>();
            var datas = GlobalHandler.mstyperesp.GetAllList();
            if (datas == null) return;

            var dt1 = datas.Where(a => a.TYPE_KEY.ToString().Length == 2);
            dt1.Each(a => list.Add(new TreeListModel() { ID = a.TYPE_KEY, ParentID = "1", Name = a.TYPE_NAME }));
            var dt2 = datas.Where(a => a.TYPE_KEY.ToString().Length == 4);
            dt2.Each(a => list.Add(new TreeListModel() { ID = a.TYPE_KEY, ParentID = a.TYPE_KEY.Substring(0, 2), Name = a.TYPE_NAME }));
            var dt3 = datas.Where(a => a.TYPE_KEY.ToString().Length == 6);
            dt3.Each(a => list.Add(new TreeListModel() { ID = a.TYPE_KEY, ParentID = a.TYPE_KEY.Substring(0, 4), Name = a.TYPE_NAME }));
            treeListleft.DataSource = list;
            tagdata = list;
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取行业类型数据出错");
                _log.Error("获取行业类型数据出错，出错提示：" + e.ToString());
            }
        }
        private void btn_add_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            layoutControlGroup1.Visibility = LayoutVisibility.Always;
            layoutControl1.Controls.Remove(layoutControl1.Controls["import"]);
            layoutControl1.Visible = !layoutControl1.Visible;
            //清除表单
            Isedit = false;
            ClearForms();
            if (layoutControl1.Visible)
            {
                BindTreeMstype();
                if (tree_mstype.EditValue != null && tree_mstype.EditValue.ToString() != "")
                {
                    GenerateNumber();
                }
                
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            layoutControlGroup1.Visibility = LayoutVisibility.Always;
            layoutControl1.Controls.Remove(layoutControl1.Controls["import"]);
            layoutControl1.Visible = !layoutControl1.Visible;
            Isedit = true;
          
            if (layoutControl1.Visible)
            {
                BindTreeMstype();
            }
            EditData();
        }

        private void dateEdit1_EditValueChanged(object sender, System.EventArgs e)
        {

        }
        private void GetData(IReadOnlyCollection<string> mstype)
        {
            try {
            if (mstype == null) return;
            list = new List<DeviceRecord>();
            DeviceList = GlobalHandler.deviceresp.GetAllList().ToList();
            foreach (var item in mstype) {
                if (item.Length < 6) continue;
                var aaList = DeviceList.Where(a => a.SBTYPE == item).ToList();
                list.AddRange(aaList);
            }
            BindView(list);
            barStaticItem3.Caption = list.Count.ToString();
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取设备信息数据出错");
                _log.Error("获取设备信息数据出错：" + e.ToString());
            }
        }
        private void GetData()
        {
            var aaList = GlobalHandler.deviceresp.GetAllList().ToList();
            BindView(aaList);
            DeviceList = aaList;
            //让树节点都处于选中状态
            //foreach (TreeListNode item in treeList1.Nodes)
            //{
            //    item.CheckState = CheckState.Checked;

            //}
        }

        private void BindView(object list)
        {
            grid_agreement.DataSource = list;
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!layoutControl1.Visible)
            {
                layoutControlGroup1.Visibility = LayoutVisibility.Never;
                foreach (Control item in layoutControl1.Controls)
                {
                    item.Visible = false;
                }
                var c = new deviceimport();
                c.Name = "import";
                c.Dock = DockStyle.Fill;
                layoutControl1.Controls.Add(c);
                layoutControl1.Padding = new System.Windows.Forms.Padding(0, 0, 30, 0);
                layoutControl1.Visible = true;
            }
            else
            {
                foreach (Control item in layoutControl1.Controls)
                {
                    item.Visible = true;
                }
                layoutControl1.Controls.Remove(layoutControl1.Controls["import"]);
                layoutControl1.Visible = false;
                layoutControlGroup1.Visibility = LayoutVisibility.Always;

            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            layoutControl2.Visible = !layoutControl2.Visible;
        }

        private void simpleButton1_Click(object sender, System.EventArgs e)
        {
            try
            {
                bool validate = false;
                StringBuilder st = new StringBuilder();
                if (string.IsNullOrEmpty(txt_sbmc.Text))
                {
                    txt_sbmc.Properties.Appearance.BorderColor = Color.Red;
                    validate = true;
                    st.Append("设备名称不能为空！\n\r");
                }
                else
                {
                    txt_sbmc.Properties.Appearance.BorderColor = Color.White;
                }
                
                if (string.IsNullOrEmpty(txt_ccbh.Text))
                {
                    txt_ccbh.Properties.Appearance.BorderColor = Color.Red;
                    validate = true;
                    st.Append("出厂编号不能为空！\n\r");
                }
                else
                {
                    txt_ccbh.Properties.Appearance.BorderColor = Color.White;
                }
                if (string.IsNullOrEmpty(tree_mstype.EditValue.ToString()))
                {
                    tree_mstype.Properties.Appearance.BorderColor = Color.Red;
                    validate = true;
                    st.Append("监测项类型不能为空！\n\r");
                }
                else
                {
                    tree_mstype.Properties.Appearance.BorderColor = Color.White;
                }
                if (string.IsNullOrEmpty(tree_city.EditValue.ToString()))
                {
                    tree_city.Properties.Appearance.BorderColor = Color.Red;
                    validate = true;
                    st.Append("区划信息不能为空！\n\r");
                }
                else
                {
                    tree_city.Properties.Appearance.BorderColor = Color.White;
                }
                if (validate)
                {
                    XtraMessageBox.Show(st.ToString());
                    return;
                }
                //添加数据
                if (Isedit)
                {
                    //编辑
                    if (gridView1.GetSelectedRows().Length <= 0) return;
                    if (string.IsNullOrEmpty(txt_sbmc.Text) || string.IsNullOrEmpty(txt_sbbh.Text))
                    {
                        XtraMessageBox.Show("设备名称和设备编码不能为空");
                        return;
                    }
                    int count = GlobalHandler.deviceresp.Count(a => a.CCBH == txt_ccbh.Text);
                    if (count > 0&& oldccbh!=txt_ccbh.Text&& txt_ccbh.Text!="")
                    {
                        XtraMessageBox.Show("出厂编号已存在！");
                        return;
                    }
                    var index = gridView1.GetSelectedRows()[0];
                    var Id = gridView1.GetRowCellValue(index, "Id");
                    var model = GlobalHandler.deviceresp.Get((int)Id);
                    model.AZDW = txt_azdw.Text;
                    model.SBTYPE = tree_mstype.EditValue.ToString();
                    model.SBMC = txt_sbmc.Text;
                    model.SBBM = txt_sbbh.Text;
                    model.CITYNAME = tree_city.Text;
                    model.CITYCODE = tree_city.EditValue.ToString();
                    model.SBXH = txt_sbxh.Text;
                    model.SCCJ = txt_sbcj.Text;
                    model.CCBH = txt_ccbh.Text;
                    model.GZRQ = date_gmrq.DateTime;
                    model.AZRQ = date_azrq.DateTime;
                    model.AZDW = txt_azdw.Text;
                    model.GLDW = txt_gldw.Text;
                    model.DCSM = double.Parse(txt_dcsm.Text == "" ? "0" : txt_dcsm.Text);
                    model.DCGHRQ = date_gmrq.DateTime;
                    model.BZ = txt_bz.Text;
                    //model.EXTENDCODE = txt_EXTENDCODE.Text;
                    model.EXTENDCODE2 = txt_EXTENDCODE2.Text;
                    model.EXTENDCODE3 = txt_EXTENDCODE3.Text;
                    model.EXTENDCODE4 = txt_EXTENDCODE4.Text;
                    model.EXTENDCODE5 = txt_EXTENDCODE5.Text;
                    //model.ADDTIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    //model.Agreement = GlobalHandler.agreeresp.Get((int) com_txxy.SelectedValue);
                    model.Agreement_Id = int.Parse((com_txxy.EditValue as ListItem).Value);
                    GlobalHandler.deviceresp.Update(model);
                    EditData();
                }
                else
                {
                    var model = new DeviceRecord();
                    if (string.IsNullOrEmpty(txt_sbmc.Text) || string.IsNullOrEmpty(txt_sbbh.Text))
                    {
                        XtraMessageBox.Show("设备名称和设备编码不能为空");
                        return;
                    }
                    int count = GlobalHandler.deviceresp.Count(a => a.CCBH == txt_ccbh.Text);
                    if (count > 0 && txt_ccbh.Text != "")
                    {
                        XtraMessageBox.Show("出厂编号已存在！");
                        return;
                    }
                    model.AZDW = txt_azdw.Text;
                    model.SBTYPE = tree_mstype.EditValue.ToString();
                    model.SBMC = txt_sbmc.Text;
                    model.SBBM = txt_sbbh.Text;
                    model.CITYCODE = tree_city.EditValue.ToString();
                    model.CITYNAME = tree_city.Text.ToString();
                    model.SBXH = txt_sbxh.Text;
                    model.SCCJ = txt_sbcj.Text;
                    model.CCBH = txt_ccbh.Text;
                    model.GZRQ = date_gmrq.DateTime;
                    model.AZRQ = date_azrq.DateTime;
                    model.AZDW = txt_azdw.Text;
                    model.GLDW = txt_gldw.Text;
                    model.DCSM = double.Parse(txt_dcsm.Text == "" ? "0" : txt_dcsm.Text);
                    model.DCGHRQ = date_gmrq.DateTime;
                    model.BZ = txt_bz.Text;
                    //model.EXTENDCODE = txt_EXTENDCODE.Text;
                    model.EXTENDCODE2 = txt_EXTENDCODE2.Text;
                    model.EXTENDCODE3 = txt_EXTENDCODE3.Text;
                    model.EXTENDCODE4 = txt_EXTENDCODE4.Text;
                    model.EXTENDCODE5 = txt_EXTENDCODE5.Text;
                    //model.ADDTIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    model.Agreement_Id = int.Parse((com_txxy.EditValue as ListItem).Value);
                    GlobalHandler.deviceresp.Insert(model);
                    ClearForms();
                }
                GetData(types);
                XtraMessageBox.Show("保存成功");
                layoutControl1.Visible = !layoutControl1.Visible;
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show("数据保存失败");
                _log.Error("获取设备信息数据出错，出错提示：" + exception.ToString());
            
        }

        }

        private void ClearForms()
        {
            //tree_mstype.Text = "";
            txt_sbbh.Text = "";
            //com_txxy.Text = "";
            txt_sbmc.Text = "";
            txt_sbbh.Text = "";
            //tree_city.Text = "";
            txt_sbxh.Text = "";
            txt_sbcj.Text = "";
            txt_ccbh.Text = "";
            txt_gldw.Text = "";
            txt_sbpl.Text = "";
            date_gmrq.DateTime = DateTime.Now;
            date_azrq.DateTime = DateTime.Now;
            date_dcghrq.DateTime = DateTime.Now;
            txt_dcsm.Text = "";
            txt_azdw.Text = "";
            txt_bz.Text = "";
            txt_gldw.Text = "";
            txt_sbpl.Text = "";
            txt_EXTENDCODE.Text = "";
            txt_EXTENDCODE2.Text = "";
            txt_EXTENDCODE3.Text = "";
            txt_EXTENDCODE4.Text = "";
            txt_EXTENDCODE5.Text = "";
        }

        private void grid_agreement_Click(object sender, EventArgs e)
        {
            EditData();

        }

        private void EditData()
        {
            try
            {
               
                if (layoutControl1.Visible == false)
                {
                    return;
                }
                if (!Isedit)
                {
                   //ClearForms();
                   return;
                }
                if (gridView1.GetSelectedRows().Length <= 0)
                {
                    ClearForms();
                    return;
                }
                var index = gridView1.GetSelectedRows()[0];
                var Id = gridView1.GetRowCellValue(index, "Id");
                var model = GlobalHandler.deviceresp.Get((int)Id);
                ifscbm = false;
                //绑定值
                txt_sbbh.Text = model.SBBM;
                //com_txxy.Text = model.Agreement?.Command ?? "无";

                AgreementRecord ar = null;
                try
                {
                    ar= GlobalHandler.agreeresp.Get(model.Agreement_Id);
                    com_txxy.EditValue = new ListItem(ar.Devicecj, ar.Id.ToString());
                }
                catch (Exception )
                {
                    ar = null;
                    com_txxy.EditValue = null;
                }

                //com_txxy.EditValue = ar == null ? "" : ar.Devicecj;

                tree_mstype.Text = model.SBTYPE;
                txt_sbmc.Text = model.SBMC;
                //tree_city.Text = model.CITYNAME;
                tree_city.EditValue = model.CITYCODE;
                txt_sbxh.Text = model.SBXH;
                txt_sbcj.Text = model.SCCJ;
                txt_ccbh.Text = model.CCBH;
                oldccbh = model.CCBH;
                date_gmrq.DateTime = model.GZRQ;
                date_azrq.DateTime = model.AZRQ;
                date_dcghrq.DateTime = model.DCGHRQ;

                txt_dcsm.Text = model.DCSM.ToString();
                txt_azdw.Text = model.AZDW;
                txt_bz.Text = model.BZ;
                txt_gldw.Text = model.GLDW;
                txt_sbpl.Text = model.FREQUENCY.ToString();

                txt_EXTENDCODE.Text = model.EXTENDCODE;
                txt_EXTENDCODE2.Text = model.EXTENDCODE2;
                txt_EXTENDCODE3.Text = model.EXTENDCODE3;
                txt_EXTENDCODE4.Text = model.EXTENDCODE4;
                txt_EXTENDCODE5.Text = model.EXTENDCODE5;
                ifscbm = true;
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取行数据出错");
                _log.Error("获取行数据出错，出错提示：" + e.ToString());
            }
        }

        private void btn_delete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (gridView1.GetSelectedRows().Length <= 0) return;
                //删除数据
                DelectBox dbox = new DelectBox();
                dbox.StartPosition = FormStartPosition.CenterScreen;
                dbox.ShowDialog();
                bool IfDelect = dbox.IfDelect;
                if (!IfDelect)
                {
                    return;
                }
                var index = gridView1.GetSelectedRows();
                index.Each(a => GlobalHandler.deviceresp.Delete((int)(gridView1.GetRowCellValue(a, "Id"))));
                GetData(types);
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show("删除数据失败");
                _log.Error("删除数据失败，出错提示：" + exception.ToString());
            }

        }

        private void btn_query_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txt_code.EditValue == null)
            {
                XtraMessageBox.Show("请输入查询内容");
                return;
            }
            //查找
            var sbmc = txt_code.EditValue.ToString();

            if (string.IsNullOrEmpty(sbmc)) return;
            var listwhere = list.Where(a => a.SBMC.Contains(sbmc)).ToList();
            grid_agreement.DataSource = listwhere;
            barStaticItem3.Caption = listwhere.Count.ToString();
        }


        private void btn_newcode_Click(object sender, EventArgs e)
        {

        }
        private void GenerateNumber()
        {
             //启动生成设备编号
            if (string.IsNullOrEmpty(tree_mstype.Text))
            {
                XtraMessageBox.Show("请选择行业类型");
                return;
            }
            else
            {
                var mstypecode = (tree_mstype.GetSelectedDataRow() as TreeListModel)?.ID;
    var count = GlobalHandler.deviceresp.Count(s => s.SBBM.StartsWith(mstypecode));
                if (count > 0)
                {
                    var str = (int.Parse(GlobalHandler.deviceresp.GetAllList(s => s.SBBM.StartsWith(mstypecode))
                                             .OrderByDescending(q => q.SBBM.Substring(6))
                                             .FirstOrDefault()
                                             ?.SBBM.Substring(6) ?? throw new InvalidOperationException()) + 1)
                        .ToString();
    var hl = "";
                    for (var i = 0; i< 5 - str.Length; i++)
                        hl += "0";
                    txt_sbbh.Text = mstypecode+ hl + str;
                }
                else
                {
                    txt_sbbh.Text = mstypecode+ @"00001";
                }
            }

        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if(types.Count>0)
            GetData(types);
        }

        private void searchControl1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //菜单查询
            treeListleft.DataSource = tagdata.Where(a => a.Name.Contains(searchControl1.Text)).ToList();
        }

        private void tree_mstype_EditValueChanged_1(object sender, EventArgs e)
        {
            if (tree_mstype.EditValue.ToString().Length < 6)
            {
                XtraMessageBox.Show("只能选择最下级节点，请重新选择！");
                this.tree_mstype.EditValueChanged -= new EventHandler(tree_mstype_EditValueChanged_1);
                this.tree_mstype.EditValue = "";
                this.tree_mstype.EditValueChanged += new EventHandler(tree_mstype_EditValueChanged_1);
                return;
            }
            if ( tree_mstype.EditValue != null && tree_mstype.EditValue.ToString() != "" && ifscbm)
            {
                    GenerateNumber();//生成编号
            }
        }

        private void tree_city_EditValueChanged(object sender, EventArgs e)
        {
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            layoutControl1.Visible = false;
        }
    }
}
