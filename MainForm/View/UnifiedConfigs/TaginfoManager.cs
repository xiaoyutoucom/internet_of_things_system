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
using ServiceStack;
using SmartKylinApp.Common;
using SmartKylinData.IOTModel;
using log4net;
using SmartKylinApp.View.BaseConfig;

namespace SmartKylinApp.View.UnifiedConfigs
{
    public partial class TaginfoManager : DevExpress.XtraEditors.XtraUserControl
    {
        private ILog _log = LogManager.GetLogger("TaginfoManager");
        public TaginfoManager()
        {
            InitializeComponent();
            //禁止右键菜单
            layoutControl1.AllowCustomization = false;
            layoutControl2.AllowCustomization = false;
            layoutControl3.AllowCustomization = false;
            bar2.Manager.AllowShowToolbarsPopup = false;
            bar2.OptionsBar.AllowQuickCustomization = false;
            gridView1.OptionsMenu.EnableColumnMenu = false;
            treeList1.OptionsMenu.EnableColumnMenu = false;
        }

        private List<TagInfoRecord> tagList;
        private void TaginfoManager_Load(object sender, EventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            splashScreenManager1.SetWaitFormCaption("请稍后,数据加载中....");     // 标题
            //splashScreenManager1.SetWaitFormDescription("正在更新.....");　　　　　// 信息
            layoutControl1.Visible = false;
            searchControl1.Properties.NullText =@"请输入关键字";
            treeList1.BeforeCheckNode += TreeList1_BeforeCheckNode;
            treeList1.AfterCheckNode += TreeList1_AfterCheckNode;
            treeList1.NodeChanged += TreeList1_NodeChanged;
            BindTree();
            GetData();
            //左侧树默认勾选第一项,并初始化数据
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
            GetData(types);
            splashScreenManager1.CloseWaitForm();
        }
        private void BindAgreementTree()
        {
            try { 
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
                XtraMessageBox.Show("获取监测点类型数据出错");
                _log.Error("获取监测点类型数据出错，出错提示：" + e.ToString());
            }
        }
        private void GetData()
        {
            try { 
            var list = GlobalHandler.tagresp.GetAllList();
            tagList = list;
            gridControl1.DataSource = list;
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

           GetData(types);
           BindAgreementTree();
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
            try { 
            if (mstype == null) return;
            tagList = GlobalHandler.tagresp.GetAllList();
            var list = new List<TagInfoRecord>();
            foreach (var item in mstype)
            {
                if (item.Length < 6) continue;
                var aaList = tagList.Where(a => a.TAG_KEY.StartsWith(item)).ToList();
                list.AddRange(aaList);
            }
            gridControl1.DataSource = list;
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取数据出错");
                _log.Error("获取数据出错，出错提示：" + e.ToString());
            }
        }

        private bool isEdit = false;
        private List<string> types = new List<string>();
        private List<TreeListModel> tagdata;

        private void btn_add_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //添加
            layoutControl1.Visible = !layoutControl1.Visible;
            isEdit = false;
            CleanData();
            if (layoutControl1.Visible)
            {
                BindAgreementTree();
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //编辑
            layoutControl1.Visible = !layoutControl1.Visible;
            isEdit = true;

            if (layoutControl1.Visible)
            {
                BindAgreementTree();
            }
            EditData();
        }

        private void btn_delete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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
                //var Idd = int.Parse(gridView1.GetRowCellValue(gridView1.GetSelectedRows()[0], "Id").ToString());
                //GlobalHandler.tagresp.Delete(Idd);
                var index = gridView1.GetSelectedRows();
                index.Each(a => GlobalHandler.tagresp.Delete((int)(gridView1.GetRowCellValue(a, "Id"))));
                GetData(types);
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show("删除失败");
                _log.Error("删除失败，出错提示：" + exception.ToString());
            }
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (types.Count > 0)
                GetData(types);
        }

        private void treeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            //行业类型选择
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
            tagdata = list;
            treeList1.DataSource = list;
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取行业类型数据出错");
                _log.Error("获取行业类型数据出错，出错提示：" + e.ToString());
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            //保存
            try
            {
                //验证必填项
                bool validate = false;
                StringBuilder st = new StringBuilder();
                if (string.IsNullOrEmpty(txt_bm.Text))
                {
                    txt_bm.Properties.Appearance.BorderColor = Color.Red;
                    validate = true;
                    st.Append("类型编码不能为空！\n\r");
                }
                else
                {
                    txt_bm.Properties.Appearance.BorderColor = Color.White;
                }
                if (string.IsNullOrEmpty(txt_name.Text))
                {
                    txt_name.Properties.Appearance.BorderColor = Color.Red;
                    validate = true;
                    st.Append("类型名称不能为空！\n\r");
                }
                else
                {
                    txt_name.Properties.Appearance.BorderColor = Color.White;
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
                    var model = GlobalHandler.tagresp.Get(Id);
                    model.TAG_KEY = txt_bm.Text.ToString();
                    model.TAG_NAME = txt_name.Text;
                    model.EXPLAIN = txt_desc.Text;
                    model.NORMAL_END = double.Parse(txt_normale.Text == "" ? "0" : txt_normale.Text);
                    model.NORMAL_START = double.Parse(txt_normals.Text == "" ? "0" : txt_normals.Text);
                    model.COLOR_VALUE = txt_COLOR_VALUE.Text;
                    model.L1_START = double.Parse(txt_L1_START.Text == "" ? "0" : txt_L1_START.Text);
                    model.L1_END = double.Parse(txt_L1_END.Text == "" ? "0" : txt_L1_END.Text);
                    
                    model.L1_COLOR_VALUE = txt_L1_COLOR_VALUE.Text;
                    model.L2_START = double.Parse(txt_L2_START.Text == "" ? "0" : txt_L2_START.Text);
                    model.L2_END = double.Parse(txt_L2_END.Text == "" ? "0" : txt_L2_END.Text);

                    model.L2_COLOR_VALUE = txt_L2_COLOR_VALUE.Text;
                    model.L3_START = double.Parse(txt_L3_START.Text == "" ? "0" : txt_L3_START.Text);
                    model.L3_END = double.Parse(txt_L3_END.Text == "" ? "0" : txt_L3_END.Text);
                   
                    model.L3_COLOR_VALUE = txt_L3_COLOR_VALUE.Text;
                    model.UNITS = txt_UNITS.Text;
                    model.ALERTRATE = double.Parse(txt_ALERTRATE.Text == "" ? "0" : txt_ALERTRATE.Text);
                    model.PRECISION = double.Parse(txt_PRECISION.Text == "" ? "0" : txt_PRECISION.Text);
                    GlobalHandler.tagresp.Update(model);
                    EditData();
                }
                else
                {

                    var model=new TagInfoRecord();
                    model.TAG_KEY = txt_bm.Text.ToString();
                    model.TAG_NAME = txt_name.Text;
                    model.EXPLAIN = txt_desc.Text;
                    model.NORMAL_END = double.Parse(txt_normale.Text == "" ? "0" : txt_normale.Text);
                    model.NORMAL_START =  double.Parse(txt_normals.Text == "" ? "0" : txt_normals.Text);
                    model.COLOR_VALUE = txt_COLOR_VALUE.Text;
                    model.L1_START = double.Parse(txt_L1_START.Text == "" ? "0" : txt_L1_START.Text);
                    model.L1_END = double.Parse(txt_L1_END.Text == "" ? "0" : txt_L1_END.Text);

                    model.L1_COLOR_VALUE = txt_L1_COLOR_VALUE.Text;
                    model.L2_START = double.Parse(txt_L2_START.Text == "" ? "0" : txt_L2_START.Text);
                    model.L2_END = double.Parse(txt_L2_END.Text == "" ? "0" : txt_L2_END.Text);

                    model.L2_COLOR_VALUE = txt_L2_COLOR_VALUE.Text;
                    model.L3_START = double.Parse(txt_L3_START.Text == "" ? "0" : txt_L3_START.Text);
                    model.L3_END = double.Parse(txt_L3_END.Text == "" ? "0" : txt_L3_END.Text);

                    model.L3_COLOR_VALUE = txt_L3_COLOR_VALUE.Text;
                    model.UNITS = txt_UNITS.Text;
                    model.ALERTRATE = double.Parse(txt_ALERTRATE.Text == "" ? "0" : txt_ALERTRATE.Text);
                    model.PRECISION = double.Parse(txt_PRECISION.Text == "" ? "0" : txt_PRECISION.Text);

                    GlobalHandler.tagresp.Insert(model);
                    CleanData();
                }
                XtraMessageBox.Show("保存成功");
                GetData(types);
                layoutControl1.Visible = !layoutControl1.Visible;
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show("数据保存失败");
                _log.Error("数据保存失败，出错提示：" + exception.ToString());
            }
        }
        private void GenerateNumber()
        {
            //生成编号
            string jcxlx = tree_stationtype.EditValue.ToString();
            string bh = jcxlx;
            var count = GlobalHandler.tagresp.Count(s => s.TAG_KEY.StartsWith(bh));
            //var count = GlobalHandler.monitorresp.GetAllList().Where(s => s.BMMC == bh).ToList().Count;
            if (count > 0)
            {
                var str = (long.Parse(GlobalHandler.tagresp.GetAllList(s => s.TAG_KEY.StartsWith(bh))
                                         .OrderByDescending(q => q.TAG_KEY)
                                         .FirstOrDefault().TAG_KEY.Split('_')[1]
                                         ) + 1).ToString();
                bh = bh + "_" + str.PadLeft(3, '0');
            }
            else
            {
                bh = bh + "_001";
            }
            txt_bm.Text = bh;
        }
        private void CleanData()
        {
            tree_stationtype.EditValue = "";
            txt_bm.Text = "";
            txt_name.Text = "";
           
            txt_desc.Text = "";
            txt_normale.Text = "";
            txt_normals.Text = "";
            txt_COLOR_VALUE.Text = "";
            txt_L1_START.Text = "";
            txt_L1_END.Text = "";
            txt_L1_COLOR_VALUE.Text = "";
            txt_L2_START.Text = "";
            txt_L2_END.Text = "";
            txt_L2_COLOR_VALUE.Text = "";
            txt_L3_START.Text = "";
            txt_L3_END.Text = "";
            txt_L3_COLOR_VALUE.Text = "";

            txt_UNITS.Text = "";
            txt_ALERTRATE.Text = "";
            txt_PRECISION.Text = "";
        }

        private void btn_cancle_Click(object sender, EventArgs e)
        {
            //取消
            layoutControl1.Visible = false;
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {
            EditData();
           
        }

        private void EditData()
        {
            //点击选择
            if (layoutControl1.Visible == false)
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
                var model = GlobalHandler.tagresp.Get(Id);
                txt_bm.Text= model.TAG_KEY;
                tree_stationtype.EditValue = model.TAG_KEY.Substring(0, 6);
                txt_name.Text = model.TAG_NAME;
                
                txt_desc.Text = model.EXPLAIN;
                txt_normale.Text = model.NORMAL_END.ToString();
                txt_normals.Text = model.NORMAL_START.ToString();
                txt_COLOR_VALUE.Text = model.COLOR_VALUE;
                txt_COLOR_VALUE.Text= model.COLOR_VALUE;
                txt_L1_START.Text = model.L1_START.ToString();
                txt_L1_END.Text = model.L1_END.ToString();
                txt_L1_COLOR_VALUE.Text = model.L1_COLOR_VALUE;
                txt_L2_START.Text = model.L2_START.ToString();
                txt_L2_END.Text = model.L2_END.ToString();
                txt_L2_COLOR_VALUE.Text = model.L2_COLOR_VALUE;
                txt_L3_START.Text = model.L3_START.ToString();
                txt_L3_END.Text = model.L3_END.ToString();
                txt_L3_COLOR_VALUE.Text = model.L3_COLOR_VALUE;

                txt_UNITS.Text = model.UNITS  ;
                txt_ALERTRATE.Text = model.ALERTRATE.ToString() ;
                txt_PRECISION.Text = model.PRECISION.ToString();
            }
            else
            {
                //CleanData();
            }
        }

        private void tree_stationtype_EditValueChanged(object sender, EventArgs e)
        {
            if (tree_stationtype.EditValue != null && tree_stationtype.EditValue.ToString() != "")
            {
                if (tree_stationtype.EditValue.ToString().Length != 6)
                {
                    XtraMessageBox.Show("只能选择最下级节点!");
                    tree_stationtype.EditValue = "";
                    txt_bm.Text = "";
                    return;
                }
                if (!isEdit)
                { 
                GenerateNumber();
                }
            }
        }

        private void searchControl1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //菜单查询
            treeList1.DataSource = tagdata.Where(a=> a.Name.Contains(searchControl1.Text)).ToList();
        }
    }
}
