using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using NPOI.HSSF.UserModel;
using ServiceStack;
using SmartKylinApp.Common;
using SmartKylinData.IOTModel;
using NPOI.SS.UserModel;
using CefSharp.Structs;
using NPOI.SS.Util;
using SmartKylinData.BaseModel;
using log4net;
using DevExpress.XtraTreeList.Nodes;

namespace SmartKylinApp.View.BaseConfig
{
    public partial class MonitorHelp : DevExpress.XtraEditors.XtraForm
    {
        List<TreeListModel> list = new List<TreeListModel>();
        private ILog _log = LogManager.GetLogger("MonitorHelp");
        //监测点
        private List<BasicMonitorRecord> tagList;
        public BasicMonitorRecord model;
        //检测项
        private List<ConfigRecord> crtagList;
        public ConfigRecord crModel;
        public MonitorHelp(BasicMonitorRecord _bmmodel, ConfigRecord _crModel)
        {
            InitializeComponent();
            //禁止右键菜单
            layoutControl2.AllowCustomization = false;
            layoutControl1.AllowCustomization = false;
            gridView1.OptionsMenu.EnableColumnMenu = false;
            model = _bmmodel;
            crModel = _crModel;
        }
        private bool ifscbm = true;
        private void MonitorHelp_Load(object sender, EventArgs e)
        {
            searchControl1.Properties.NullText = @"请输入关键字";
            treeList1.BeforeCheckNode += TreeList1_BeforeCheckNode;
            treeList1.AfterCheckNode += TreeList1_AfterCheckNode;
            treeList1.NodeChanged += TreeList1_NodeChanged;
            BindTree();
            GetData();
            layoutControl1.Visible = true;

            //gridView1.Click
            gridControl1.Click += GridControl1_Click;

            gridView1.CustomDrawCell += GridView1_CustomDrawCell;
            gridView2.CustomDrawCell += GridView2_CustomDrawCell;

            CheckSelect();
        }

        private void GridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.RowHandle == gridView1.FocusedRowHandle)
            {
                e.Appearance.BackColor = Color.LightCyan;
            }
        }
        private void GridView2_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.RowHandle == gridView2.FocusedRowHandle)
            {
                e.Appearance.BackColor = Color.LightCyan;
            }
        }

        /// <summary>
        /// 实现反选
        /// </summary>
        private void CheckSelect()
        {
            if (model != null && crModel != null)
            {
                //行业类型
                treeList1.FocusedNode = treeList1.Nodes.TreeList.FindNodeByID(list.IndexOf(list.Where(p => p.ID == model.STATIONTYPE).FirstOrDefault()));
                ParentNodeExpend(treeList1.FocusedNode);
                //监测点
                var list1 = GlobalHandler.monitorresp.GetAllList(p => p.STATIONTYPE==model.STATIONTYPE).ToList();
                gridControl1.DataSource = list1;
                gridView1.FocusedRowHandle = list1.IndexOf(model);
                //检测项
                var list2 = GlobalHandler.configresp.GetAllList(p => p.CONFIG_CODE.Contains(crModel.CONFIG_CODE.Substring(0,19))).ToList();
                gridControl2.DataSource = list2;
                gridView2.FocusedRowHandle = list2.IndexOf(crModel);
            }
        }
        /// <summary>
        /// 展开当前节点及父节点
        /// </summary>
        /// <param name="_node"></param>
        public void ParentNodeExpend(TreeListNode node)
        {
            node.Checked = true;
            TreeListNode _cNode = node;
            treeList1.Nodes.TreeList.FindNodeByID(_cNode.Id).Expanded = true;
            while (_cNode.ParentNode != null)
            {
                treeList1.Nodes.TreeList.FindNodeByID(_cNode.ParentNode.Id).Expanded = true;
                _cNode = _cNode.ParentNode;
            }
        }
        private void GridControl1_Click(object sender, EventArgs e)
        {
            int focusedhandle = gridView1.FocusedRowHandle;
            if (focusedhandle < 0)
            {
                return;
            }
            object rowIdObj = gridView1.GetRowCellValue(focusedhandle, "BMID");

            try
            {
                var list = GlobalHandler.configresp.GetAllList(p => p.CONFIG_CODE.Contains(rowIdObj.ToString())).ToList();
                //list = list.Where(p => p.CONFIG_CODE.Contains(rowIdObj.ToString())).ToList();
                crtagList = list;
                gridControl2.DataSource = list;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("获取数据出错");
                _log.Error("获取数据出错，出错提示：" + e.ToString());
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
            var dt1 = datas.Where(a => a.TYPE_KEY.ToString().Length == 2);
            dt1.Each(a => list.Add(new TreeListModel() { ID = a.TYPE_KEY, ParentID = "1", Name = a.TYPE_NAME }));
            var dt2 = datas.Where(a => a.TYPE_KEY.ToString().Length == 4);
            dt2.Each(a => list.Add(new TreeListModel() { ID = a.TYPE_KEY, ParentID = a.TYPE_KEY.Substring(0, 2), Name = a.TYPE_NAME }));
            var dt3 = datas.Where(a => a.TYPE_KEY.ToString().Length == 6);
            dt3.Each(a => list.Add(new TreeListModel() { ID = a.TYPE_KEY, ParentID = a.TYPE_KEY.Substring(0, 4), Name = a.TYPE_NAME }));
            treeList1.DataSource = list;
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
            var types = new List<string>();
            foreach (var item in list)
            {
                if (item.GetValue("ID").ToString().Length == 6)
                {
                    types.Add(item.GetValue("ID").ToString());
                }
            }

            GetData(types);
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
            var list = new List<BasicMonitorRecord>();
            foreach (var item in mstype)
            {
                if (item.Length < 6) continue;
                var aaList = tagList.Where(a => a.BMID.Substring(6, 6).StartsWith(item)).ToList();
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

        private void searchControl1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            var where = searchControl1.Text;
            BindTree(where);
        }
        private void BindTree(string where)
        {
            try { 
            var datas = GlobalHandler.mstyperesp.GetAllList(a => a.TYPE_NAME.Contains(where));
            if (datas == null) return;
            var list = new List<TreeListModel>();
            var dt1 = datas.Where(a => a.TYPE_KEY.ToString().Length == 2);
            dt1.Each(a => list.Add(new TreeListModel() { ID = a.TYPE_KEY, ParentID = "1", Name = a.TYPE_NAME }));
            var dt2 = datas.Where(a => a.TYPE_KEY.ToString().Length == 4);
            dt2.Each(a => list.Add(new TreeListModel() { ID = a.TYPE_KEY, ParentID = a.TYPE_KEY.Substring(0, 2), Name = a.TYPE_NAME }));
            var dt3 = datas.Where(a => a.TYPE_KEY.ToString().Length == 6);
            dt3.Each(a => list.Add(new TreeListModel() { ID = a.TYPE_KEY, ParentID = a.TYPE_KEY.Substring(0, 4), Name = a.TYPE_NAME }));
            treeList1.DataSource = list;
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取行业类型数据出错");
                _log.Error("获取行业类型数据出错，出错提示：" + e.ToString());
            }
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            //双击选择
            if (gridView1.GetSelectedRows().Length >0)
            {
                var Id = int.Parse(gridView1.GetRowCellValue(gridView1.GetSelectedRows()[0], "Id").ToString());
                model = GlobalHandler.monitorresp.Get(Id);
                this.Close();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (gridView1.GetSelectedRows().Length > 0)
            {
                //监测点
                var Id = int.Parse(gridView1.GetRowCellValue(gridView1.GetSelectedRows()[0], "Id").ToString());
                model = GlobalHandler.monitorresp.Get(Id);
                //检测项
                Id = int.Parse(gridView2.GetRowCellValue(gridView2.GetSelectedRows()[0], "Id").ToString());
                crModel = GlobalHandler.configresp.Get(Id);
                this.Close();
            }
            else
            {
                XtraMessageBox.Show("请先选择数据！");
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}