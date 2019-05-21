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
using DevExpress.XtraTreeList.Nodes;
using System.Collections;
using log4net;

namespace SmartKylinApp.View.BaseConfig
{
    public partial class VidieoLinkHelp : DevExpress.XtraEditors.XtraForm
    {
        private ILog _log = LogManager.GetLogger("VidieoLinkHelp");
        public VidieoLinkHelp()
        {
            InitializeComponent();
            //禁止右键菜单
            layoutControl1.AllowCustomization = false;
            layoutControl2.AllowCustomization = false;
            gridView1.OptionsMenu.EnableColumnMenu = false;
            tree_mstype.Properties.TreeList.OptionsMenu.EnableColumnMenu = false;
            treeList1.OptionsMenu.EnableColumnMenu = false;
        }
        private bool ifscbm = true;
        private List<Contact> tagList;
        private int Id;
        public Contact model;
        private string Idd;
        private string code;

        private void VidieoLinkHelp_Load(object sender, EventArgs e)
        {
            BindTree();
            BindMstypeTree();
            layoutControl1.Visible = true;
        }
        private void BindMstypeTree()
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
            tree_mstype.Properties.DataSource = list;
            tree_mstype.Properties.DisplayMember = "Name";
            tree_mstype.Properties.ValueMember = "ID";
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取行业类型数据出错");
                _log.Error("获取行业类型数据出错，出错提示：" + e.ToString());
            }
        }

        private void BindTree()
        {
            var datas = GlobalHandler.videogroupresp.GetAllList();
            var datas2 = GlobalHandler.videoresp.GetAllList();
            if (datas == null || datas2 == null) return;
            var list = new List<TreeListModel>();
            datas.Each(a => list.Add(new TreeListModel() { ID = a.Id.ToString(), ParentID = a.FID.ToString(), Name = a.FZMC }));

            datas2.Each(a => list.Add(new TreeListModel() { ID = "a" + a.Id.ToString(), ParentID = a.FZID.ToString(), Name = a.SPDMC }));
            treeList1.DataSource = list;
            treeList1.ExpandAll();


        }


        private void BindTree(string where)
        {
            try { 
            var datas = GlobalHandler.videogroupresp.GetAllList(a => a.FZMC.Contains(where));
            treeList1.DataSource = datas;
            treeList1.ExpandAll();
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取分组数据出错");
                _log.Error("获取分组数据出错，出错提示：" + e.ToString());
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //保存
            try
            {
                Idd = treeList1.FocusedNode.GetValue("ID")?.ToString();
                if (Idd == null || Idd.Substring(0, 1) != "a")
                {
                    XtraMessageBox.Show("请选择视频点！");
                    return;
                }

                int idd = int.Parse(Idd.Substring(1, Idd.Length - 1));
                VideoRecord vmodel = GlobalHandler.videoresp.Get(idd);
                int[] rownumber = this.gridView1.GetSelectedRows();//获取选中行号；
                if (rownumber.Length < 1 && gridView1.RowCount == 0)
                {
                    XtraMessageBox.Show("请勾选监测点！");
                    return;
                }
                if (tree_mstype.EditValue.ToString() == "" || tree_mstype.EditValue == null)
                {
                    //XtraMessageBox.Show("请选择行业类型！");
                    //选中用户关联数据全部删除
                    var wlist = GlobalHandler.monitorlinkvideoresp.GetAllList(a => a.VIDEO == vmodel).ToList();
                    if (wlist.Count > 0)
                    {
                        for (int j = 0; j < wlist.Count; j++)
                        {
                            GlobalHandler.monitorlinkvideoresp.Delete(int.Parse(wlist[j].Id.ToString()));
                        }
                    }
                }
                else {

                    //选中用户关联数据删除

                    var list = GlobalHandler.monitorlinkvideoresp.GetAllList(a => a.VIDEO == vmodel).Where(b => b.MONITOR.BMID.Substring(6, 6).StartsWith(code)).ToList();
                    if (list.Count > 0)
                    {
                        for (int j = 0; j < list.Count; j++)
                        {
                            GlobalHandler.monitorlinkvideoresp.Delete(int.Parse(list[j].Id.ToString()));
                        }
                    }
                }
                for (int i = 0; i < rownumber.Length; i++)
                {
                    //foreach (int i in selectRows)
                    //{
                    //    custcode.Add(int.Parse(this.gridView1.GetDataRow(i)["PEBID"].ToString()));
                    //}
                    Id = int.Parse(gridView1.GetRowCellValue(rownumber[i], "Id").ToString());
                    BasicMonitorRecord bmmodel = GlobalHandler.monitorresp.Get(Id);
                    MonitorVideoRecord mvmodel = new MonitorVideoRecord();
                    mvmodel.MONITOR = bmmodel;
                    mvmodel.VIDEO = vmodel;
                    GlobalHandler.monitorlinkvideoresp.Insert(mvmodel);
                }              
                XtraMessageBox.Show("保存成功");
                this.Close();
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show("保存失败");
                _log.Error("保存失败，出错提示：" + e.ToString());
            }
        } 

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tree_mstype_EditValueChanged(object sender, EventArgs e)
        {
            //行业类型选择
            if (tree_mstype.EditValue.ToString()!="") { 
            code = (tree_mstype.GetSelectedDataRow() as TreeListModel)?.ID;
            BindControl(code);
            BindCheck();
           }
        }

        private void BindCheck()
        {
            try { 
            //初始化选择的监测点
            var list = GlobalHandler.monitorlinkvideoresp.GetAllList(a=>a.VIDEO.Id.ToString() == Idd.Substring(1, Idd.Length-1)).Where(b => b.MONITOR.BMID.Substring(6, 6).StartsWith(code)).ToList();
            ArrayList _arr = new ArrayList();
            for (int i = 0; i < list.Count; i++)
            {
                _arr.Add(list[i].MONITOR.Id.ToString());
            }
            for (int j = 0; j < gridView1.RowCount; j++)
            {
                var Id = gridView1.GetRowCellValue(j, "Id").ToString();
                if(_arr.Contains(Id))
                {
                    gridView1.SelectRow(j);
                }
            }
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("初始化数据出错");
                _log.Error("初始化数据出错，出错提示：" + e.ToString());
            }
        }
        private void GetAllCheck()
        {
            //获取用户全部监测点
            try { 
            var list = GlobalHandler.monitorlinkvideoresp.GetAllList(a => a.VIDEO.Id.ToString() == Idd.Substring(1, Idd.Length - 1)).ToList();
            List< BasicMonitorRecord > bmlist = new List < BasicMonitorRecord > ();
            for (int i = 0; i < list.Count; i++)
            {
                bmlist.Add(list[i].MONITOR);
            }
            gridControl1.DataSource = bmlist;
            gridView1.SelectAll();
                //for (int i = 0; i < list.Count; i++)
                //{
                //    _arr.Add(list[i].BaseMonitor.Id.ToString());
                //}
                //for (int j = 0; j < gridView1.RowCount; j++)
                //{
                //    var Id = gridView1.GetRowCellValue(j, "Id").ToString();
                //    if (_arr.Contains(Id))
                //    {
                //        gridView1.SelectRow(j);
                //    }
                //}
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("初始化数据出错");
                _log.Error("初始化数据出错，出错提示：" + e.ToString());
            }
        }
        private void BindControl(string mstype)
        {
            try { 
            var aList = GlobalHandler.monitorresp.GetAllList(a => a.BMID.Substring(6, 6).StartsWith(mstype)).ToList();
            if (aList.Count < 1)
            {
                //var blist = new List<BasicMonitorRecord>();
                //BasicMonitorRecord model = new BasicMonitorRecord();
                //model.Id = -1;
                //model.BMMC = "没有匹配监测点！";
                //blist.Add(model);
                //gridControl1.DataSource = blist;
                return;
            }
            else
            {
                gridControl1.DataSource = aList;
            }
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取数据出错");
                _log.Error("获取数据出错，出错提示：" + e.ToString());
            }
        }

        private void treeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            tree_mstype.EditValue = "";
            Idd = treeList1.FocusedNode.GetValue("ID")?.ToString();
            if (tree_mstype.EditValue.ToString()!="")
            {
                BindControl(code);
                BindCheck();
            }
            else
            {
                GetAllCheck();
            }
            //gridControl1.DataSource = null;
        }

        private void treeList1_GetSelectImage(object sender, DevExpress.XtraTreeList.GetSelectImageEventArgs e)
        {
            //树图片
            if (e.Node == null) return;
            TreeListNode node = e.Node;
            //if (!node.HasChildren) return;
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                var ID = node.GetValue("ID");
                if (!(node.Nodes[i].GetValue("ID").ToString().Substring(0, 1) == "a"))
                {
                    node.Nodes[i].ImageIndex = 3;
                }
                else
                {
                    node.Nodes[i].ImageIndex = 1;
                }
            }
            var FID = node.GetValue("ParentID");
            if (FID.Equals("-1"))
            {
                e.NodeImageIndex = 2;
            }
            //else
            //{
            //    e.NodeImageIndex = 1;
            //}
         
        }
    }
}