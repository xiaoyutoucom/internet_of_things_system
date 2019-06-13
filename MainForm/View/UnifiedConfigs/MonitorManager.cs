using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using SmartKylinData.IOTModel;
using SmartKylinApp.Common;
using ServiceStack;
using SmartKylinData.BaseModel;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraLayout.Utils;
using SmartKylinApp.View.BaseConfig;
using log4net;
using System.Web.UI.WebControls;

namespace SmartKylinApp.View.UnifiedConfigs{
    public partial class MonitorManager : DevExpress.XtraEditors.XtraUserControl
    {
        private ILog _log = LogManager.GetLogger("MonitorManager");
        public MonitorManager()
        {
            InitializeComponent();
            //禁止右键菜单
            layoutControl1.AllowCustomization = false;
            layoutControl2.AllowCustomization = false;
            bar2.Manager.AllowShowToolbarsPopup = false;
            bar2.OptionsBar.AllowQuickCustomization = false;
            gridView1.OptionsMenu.EnableColumnMenu = false;
            treeList1.OptionsMenu.EnableColumnMenu = false;
        }
        private bool ifscbm = true;
        private List<BasicMonitorRecord> list;
        private List<BasicMonitorRecord> tagList;
        private void MonitorManager_Load(object sender, EventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            splashScreenManager1.SetWaitFormCaption("请稍后,数据加载中....");     // 标题
            //splashScreenManager1.SetWaitFormDescription("正在更新.....");　　　　　// 信息
            layoutControl10.Visible = false;
            searchControl1.Properties.NullText = @"请输入关键字";
            treeList1.BeforeCheckNode += TreeList1_BeforeCheckNode;
            treeList1.AfterCheckNode += TreeList1_AfterCheckNode;
            treeList1.NodeChanged += TreeList1_NodeChanged;
            BindTXFS();
            BindTree();
            //GetData();
            BindCityInfo();
            //BindAgreementTree();
            layoutControl1.Visible = true;
            //左侧树默认勾选第一项,并初始化数据
            try
            { 
            treeList1.Nodes[0].CheckAll();
            var list = treeList1.GetAllCheckedNodes();
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
                _log.Error("初始化左侧树数据出错，出错提示：" + ex.ToString());
                XtraMessageBox.Show("初始化左侧树数据出错！"); 
            }
            GetData(types);
            splashScreenManager1.CloseWaitForm();
        }

        private void BindTXFS()
        {
            //数据集绑定
            List<TreeListModel> list = new List<TreeListModel>();
            list.Add(new TreeListModel() { ID = "GPRS", Name = "GPRS" });
            list.Add(new TreeListModel() { ID = "OPC", Name = "OPC" });
            list.Add(new TreeListModel() { ID = "混合通讯", Name = "混合通讯" });
            ListItem it = null;
            foreach (TreeListModel item in list)
            {
                it = new ListItem(item.Name, item.ID);
                this.com_txfs.Properties.Items.Add(it);
            }
            com_txfs.SelectedIndex = 0;
        }
        private void BindAgreementTree()
        {
            try { 
            if (layoutControl10.Visible == false)
            {
                return;
            }
            var listcheck = treeList1.GetAllCheckedNodes();
            var list = new List<TreeListModel>();
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
            tree_stationtype.Properties.DataSource = list;
                if (list.Count(a => a.ID.ToString().Length == 6) < 1) return;
                tree_stationtype.EditValue = list.Where(a => a.ID.ToString().Length == 6).First().ID;
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取监测项类型数据出错");
                _log.Error("获取监测项类型数据出错，出错提示：" + e.ToString());
            }
        }
        private void BindCityInfo()
        {
            try { 
            //绑定区划信息
            var code = GlobalHandler.CityCode;
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
        private void treeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            //行业类型能够选择
        }
        private void BindTree()
        {
            try { 
            var datas = GlobalHandler.mstyperesp.GetAllList();
            if (datas == null) return;
            var list = new List<TreeListModel>();
            var dt1 = datas.Where(a => a.TYPE_KEY.ToString().Length == 2);
            dt1.Each(a => list.Add(new TreeListModel() { ID = a.TYPE_KEY, ParentID = "1", Name = a.TYPE_NAME }));
            var dt2 = datas.Where(a => a.TYPE_KEY.ToString().Length == 4);
            dt2.Each(a => list.Add(new TreeListModel() { ID = a.TYPE_KEY, ParentID = a.TYPE_KEY.Substring(0, 2), Name = a.TYPE_NAME }));
            var dt3 = datas.Where(a => a.TYPE_KEY.ToString().Length == 6);
            dt3.Each(a => list.Add(new TreeListModel() { ID = a.TYPE_KEY, ParentID = a.TYPE_KEY.Substring(0, 4), Name = a.TYPE_NAME }));
            treeList1.DataSource = list;
            tagdata = list;
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取行业类型数据出错");
                _log.Error("获取行业类型数据出错，出错提示：" + e.ToString());
            }
        }
        private void GetData()
        {
            try { 
            var list = GlobalHandler.monitorresp.GetAllList();
            tagList = list;
            gridControl1.DataSource = list;
            barLinkContainerItem4.Caption = list.Count.ToString();
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取数据出错");
                _log.Error("获取数据出错，出错提示：" + e.ToString());
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
            var list = treeList1.GetAllCheckedNodes();
            types.Clear();
            foreach (var item in list)
            {
                if (item.GetValue("ID").ToString().Length == 6)
                {
                    types.Add(item.GetValue("ID").ToString());
                }
            }
            BindAgreementTree();
            GetData(types);
            EditData();
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
        //获取数据
        private void GetData(IReadOnlyCollection<string> mstype)
        {
            try
            {
                if (mstype == null) return;
                list = new List<BasicMonitorRecord>();
                tagList = GlobalHandler.monitorresp.GetAllList();
                foreach (var item in mstype)
                {
                    if (item.Length < 6) continue;
                    var aaList = tagList.Where(a => a.BMID.Substring(6, 6).StartsWith(item)).ToList();
                    list.AddRange(aaList);
                }
                gridControl1.DataSource = list;
                barLinkContainerItem4.Caption = list.Count.ToString();
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show("获取数据失败");
                _log.Error("获取数据失败，出错提示：" + exception.ToString());
            }
        }

        private bool isEdit = false;
        private List<string> types = new List<string>();
        private List<TreeListModel> tagdata;

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //添加、
            Root.Visibility = LayoutVisibility.Always;
            layoutControl10.Controls.Remove(layoutControl10.Controls["import"]);
            layoutControl10.Visible = !layoutControl10.Visible;
            isEdit = false;
            CleanData();
            if (layoutControl1.Visible)
            {
                BindCityInfo();
                BindAgreementTree();
            }

        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //编辑
            BindAgreementTree();
            Root.Visibility = LayoutVisibility.Always;
            layoutControl10.Controls.Remove(layoutControl10.Controls["import"]);
            layoutControl10.Visible = !layoutControl10.Visible;
            isEdit = true;

            if (layoutControl1.Visible)
            {
                BindAgreementTree();
            }
            EditData();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //删除
            try
            {
                if (gridView1.GetSelectedRows().Length == 0) return;
                //删除数据
                DelectBox dbox = new DelectBox();
                dbox.StartPosition = FormStartPosition.CenterScreen;
                dbox.ShowDialog();
                bool IfDelect = dbox.IfDelect;
                if (!IfDelect)
                {
                    return;
                }
                //var Id = int.Parse(gridView1.GetRowCellValue(gridView1.GetSelectedRows()[0], "Id").ToString());
                //GlobalHandler.monitorresp.Delete(Id);

                var index = gridView1.GetSelectedRows();
                index.Each(a => GlobalHandler.monitorresp.Delete((int)(gridView1.GetRowCellValue(a, "Id"))));
                //XtraMessageBox.Show("删除成功");
                GetData(types);

            }
            catch (Exception exception)
            {
                XtraMessageBox.Show("删除失败");
                _log.Error("删除失败，出错提示：" + exception.ToString());
            }
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (types.Count > 0)
                GetData(types);
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            //保存
            try
            { //验证必填项
                bool validate = false;
                StringBuilder st = new StringBuilder();
                if (string.IsNullOrEmpty(txt_bmid.Text))
                {
                    txt_bmid.Properties.Appearance.BorderColor = Color.Red;
                    validate = true;
                    st.Append("监测点编码不能为空！\n\r");
                }
                else
                {
                    txt_bmid.Properties.Appearance.BorderColor = Color.White;
                }
                if (string.IsNullOrEmpty(txt_bmmc.Text))
                {
                    txt_bmmc.Properties.Appearance.BorderColor = Color.Red;
                    validate = true;
                    st.Append("监测名称码不能为空！\n\r");
                }
                else
                {
                    txt_bmmc.Properties.Appearance.BorderColor = Color.White;
                }
                if (validate)
                {
                    XtraMessageBox.Show(st.ToString());
                    return;
                }
                if (isEdit)
                {

                    if (gridView1.GetSelectedRows().Length == 0) return;
                    var Id = int.Parse(gridView1.GetRowCellValue(gridView1.GetSelectedRows()[0], "Id").ToString());
                    var model = GlobalHandler.monitorresp.Get(Id);
                    model.BMID = txt_bmid.Text;
                    model.BMMC = txt_bmmc.Text;
                    model.BMMS = txt_bmms.Text;
                    model.BMX= decimal.Parse(txt_bmx.Text == "" ? "0" : txt_bmx.Text);
                    model.BMY = decimal.Parse(txt_bmy.Text == "" ? "0" : txt_bmy.Text);
                    model.IMGURL = txt_imgurl.Text;
                    model.MLEVEL = txt_mlevel.Text;
                    model.STATIONTYPE = tree_stationtype.EditValue.ToString();
                    model.Template = txt_template.Text;
                    model.TXFS = com_txfs.Text;
                    model.WebUrl = txt_weburl.Text;
                    model.BJBM = txt_bjbm.Text;
                    model.EXTENDCODE = EXTENDCODE1.Text.Trim();
                    model.EXTENDCODE2 = EXTENDCODE2.Text.Trim();
                    //model.ADDTIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    GlobalHandler.monitorresp.Update(model);
                    EditData();
                }
                else
                {

                    var model = new BasicMonitorRecord();
                    model.BMID = txt_bmid.Text;
                    model.BMMC = txt_bmmc.Text;
                    model.BMMS = txt_bmms.Text;
                    model.BMX = decimal.Parse(txt_bmx.Text == "" ? "0" : txt_bmx.Text);
                    model.BMY = decimal.Parse(txt_bmy.Text == "" ? "0" : txt_bmy.Text);
                    model.IMGURL = txt_imgurl.Text;
                    model.MLEVEL = txt_mlevel.Text;
                    model.STATIONTYPE = tree_stationtype.EditValue.ToString();
                    model.Template = txt_template.Text;
                    model.TXFS = com_txfs.Text;
                    model.WebUrl = txt_weburl.Text;
                    model.BJBM = txt_bjbm.Text;
                    model.EXTENDCODE = EXTENDCODE1.Text.Trim();
                    model.EXTENDCODE2 = EXTENDCODE2.Text.Trim();
                    // model.ADDTIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    GlobalHandler.monitorresp.Insert(model);
                    CleanData();
                }
                XtraMessageBox.Show("保存成功");
                GetData(types);
                layoutControl10.Visible = !layoutControl10.Visible;
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show("数据保存失败");
                _log.Error("数据保存失败，出错提示：" + exception.ToString());
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            //取消
            layoutControl10.Visible = false;
            CleanData();
        }
        private void CleanData()
        {
            //清空
            //var list = GlobalHandler.monitorresp.GetAllList();
            tree_city.EditValue = "";
            txt_bmid.Text = "";
            txt_bmmc.Text = "";
            txt_bmms.Text = "";
            txt_bmx.Text = "";
            txt_bmy.Text = "";
            txt_imgurl.Text = "";
            txt_mlevel.Text = "";
            tree_stationtype.EditValue = "";
            txt_template.Text = "";
            com_txfs.Text = "";
            txt_weburl.Text = "";
            txt_bjbm.Text = "";
        }
        private void tree_city_EditValueChanged(object sender, EventArgs e)
        {
            //区域修改
    
            if (tree_city.EditValue != null && tree_city.EditValue.ToString()!=""&& ifscbm) { 
            if (tree_city.EditValue.ToString().Substring(4, 2) == "00")
            {
                XtraMessageBox.Show( "请选择最下级节点");
                tree_city.EditValue = "";
                txt_bmid.Text = "";
            }
            else if (tree_stationtype.Text!= "")
            {
                GenerateNumber();//生成编号
            }
            }
        }
        private void tree_stationtype_EditValueChanged(object sender, EventArgs e)
        {
            //监测点类型修改
            if (tree_stationtype.EditValue!= null && tree_stationtype.EditValue.ToString() != "" && ifscbm)
            {
                if (tree_stationtype.EditValue.ToString().Length != 6)
                {
                    XtraMessageBox.Show("请选择最下级节点");
                    tree_stationtype.EditValue = "";
                    txt_bmid.Text = "";
                }
                else if (tree_city.Text != "" )
                {
                    GenerateNumber();//生成编号
                }
            }
        }
        private void GenerateNumber()
        {
            try { 
            //生成编号
            var datas = GlobalHandler.monitorresp.GetAllList();
            decimal num = 1;
            string nub = num.ToString().PadLeft(7, '0');
            string bh = tree_city.EditValue.ToString() + tree_stationtype.EditValue.ToString();
            var count = GlobalHandler.monitorresp.Count(s => s.BMID.StartsWith(bh));
            //var count = GlobalHandler.monitorresp.GetAllList().Where(s => s.BMMC == bh).ToList().Count;
            if (count > 0)
            {

                var str = (long.Parse(GlobalHandler.monitorresp.GetAllList(s => s.BMID.StartsWith(bh))
                                         .OrderByDescending(q => q.BMID)
                                         .FirstOrDefault().BMID.ToString()
                                         ) + 1).ToString();
                bh = str;
            }
            else
            {
                bh = bh + nub;
            }
            txt_bmid.Text = bh;
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("生成编号出错");
                _log.Error("生成编号出错，出错提示：" + e.ToString());
            }
        }
        private void gridControl1_Click(object sender, EventArgs e)
        {
            EditData();
            
        }

        private void EditData()
        {
            try
            {
                //点击选择
                if (layoutControl10.Visible == false)
                {
                    return;
                }
                if (isEdit)
                {
                    if (gridView1.GetSelectedRows().Length == 0)
                    {
                        CleanData();
                        return;
                    }
                    var Id = int.Parse(gridView1.GetRowCellValue(gridView1.GetSelectedRows()[0], "Id").ToString());
                    var model = GlobalHandler.monitorresp.Get(Id);
                    ifscbm = false;
                    tree_city.EditValue = model.BMID.Substring(0, 6).ToString();
                    if (model.STATIONTYPE != null)
                    {
                        tree_stationtype.EditValue = model.STATIONTYPE;
                    }
                    else
                    {
                        tree_stationtype.EditValue = "";
                    }
                    ifscbm = true;
                    txt_bmid.Text = model.BMID;
                    txt_bmmc.Text = model.BMMC;
                    txt_bmms.Text = model.BMMS;
                    txt_bmx.Text = model.BMX.ToString();
                    txt_bmy.Text = model.BMY.ToString();
                    txt_imgurl.Text = model.IMGURL;
                    txt_mlevel.Text = model.MLEVEL;

                    txt_template.Text = model.Template;
                    com_txfs.Text = model.TXFS;
                    txt_weburl.Text = model.WebUrl;
                    txt_bjbm.Text = model.BJBM;

                    EXTENDCODE1.Text = model.EXTENDCODE;
                    EXTENDCODE2.Text = model.EXTENDCODE2;
                }
                else
                {
                    //CleanData();
                }
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取数据出错");
                _log.Error("获取数据出错，出错提示：" + e.ToString());
            }
        }

        private void barButtonItem24_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            if (barEdit_mc.EditValue == null|| barEdit_mc.EditValue.ToString()=="")
            {
                XtraMessageBox.Show("请输入查询内容");
                return;
            }
            //查找
            var sbbm = barEdit_mc.EditValue.ToString();

            if (string.IsNullOrEmpty(sbbm)) return;
            var listwhere = list.Where(a => a.BMMC.Contains(sbbm)).ToList();
            gridControl1.DataSource = listwhere;
            barLinkContainerItem4.Caption = listwhere.Count.ToString();
        }

        private void barButtonItem25_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //隐藏左边面板
            layoutControl1.Visible = !layoutControl1.Visible;
        }

        private void barButtonItem26_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //导入
            if (!layoutControl10.Visible)
            {
                Root.Visibility = LayoutVisibility.Never;
                foreach (Control item in layoutControl10.Controls)
                {
                    item.Visible = false;
                }
                var c = new Monitorimport();
                c.Name = "import";
                c.Dock = DockStyle.Fill;
                layoutControl10.Controls.Add(c);
                layoutControl10.Padding = new System.Windows.Forms.Padding(0, 0, 30, 0);
                layoutControl10.Visible = true;
            }
            else
            {
                foreach (Control item in layoutControl10.Controls)
                {
                    item.Visible = true;
                }
                layoutControl10.Controls.Remove(layoutControl10.Controls["import"]);
                layoutControl10.Visible = false;
                Root.Visibility = LayoutVisibility.Always;

            }
        }

        private void searchControl1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //菜单查询
            treeList1.DataSource = tagdata.Where(a => a.Name.Contains(searchControl1.Text)).ToList();
        }
    }

}

