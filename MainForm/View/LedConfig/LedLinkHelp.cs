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
using FluentNHibernate.Conventions;
using log4net;

namespace SmartKylinApp.View.BaseConfig
{
    public partial class LedLinkHelp : DevExpress.XtraEditors.XtraForm
    {
        private ILog _log = LogManager.GetLogger("LedLinkHelp");
        public LedLinkHelp()
        {
            InitializeComponent();
            //禁止右键菜单
            layoutControl1.AllowCustomization = false;
            layoutControl2.AllowCustomization = false;
            gridView1.OptionsMenu.EnableColumnMenu = false;
            gridView2.OptionsMenu.EnableColumnMenu = false;
            tree_mstype.Properties.TreeList.OptionsMenu.EnableColumnMenu = false;
        }
        private bool ifscbm = true;
        private List<Contact> tagList;
        private int Id;
        public Contact model;
        private string Idd;
        private string code;

        private void LedLinkHelp_Load(object sender, EventArgs e)
        {
            BindMstypeTree();
            GetData();
        }

        private void GetData()
        {
            try { 
            var datas = GlobalHandler.ledresp.GetAllList();
            gridControl2.DataSource = datas;
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取数据出错");
                _log.Error("获取数据出错，出错提示：" + e.ToString());
            }
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
                XtraMessageBox.Show("获取数据出错");
                _log.Error("获取数据出错，出错提示：" + e.ToString());
            }
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //保存
            try
            {
                //Idd = treeList1.FocusedNode.GetValue("ID")?.ToString();
                if (Idd == null)
                {
                    XtraMessageBox.Show("请选择Led信息！");
                    return;
                }

                int idd = int.Parse(Idd);
                LedModel cgmodel = GlobalHandler.ledresp.Get(idd);
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
                    var wlist = GlobalHandler.ledLinkresp.GetAllList(a => a.Led == cgmodel).ToList();
                    if (wlist.Count > 0)
                    {
                        for (int j = 0; j < wlist.Count; j++)
                        {
                            GlobalHandler.ledLinkresp.Delete(int.Parse(wlist[j].Id.ToString()));
                        }
                    }
                }
                else {

                    //选中用户关联数据删除

                    var list = GlobalHandler.ledLinkresp.GetAllList(a => a.Led == cgmodel).Where(b => b.MonitorRecord.BMID.Substring(6, 6).StartsWith(code)).ToList();
                    if (list.Count > 0)
                    {
                        for (int j = 0; j < list.Count; j++)
                        {
                            GlobalHandler.ledLinkresp.Delete(int.Parse(list[j].Id.ToString()));
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
                    LedLinkMonitor wlmodel = new LedLinkMonitor();
                    wlmodel.MonitorRecord = bmmodel;
                    wlmodel.Led= cgmodel;
                    GlobalHandler.ledLinkresp.Insert(wlmodel);
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
            //初始化选择的监测点
            try { 
            var list = GlobalHandler.ledLinkresp.GetAllList(a=>a.Led.Id.ToString() == Idd).Where(b => b.MonitorRecord.BMID.Substring(6, 6).StartsWith(code)).ToList();
            ArrayList _arr = new ArrayList();
            for (int i = 0; i < list.Count; i++)
            {
                _arr.Add(list[i].MonitorRecord.Id.ToString());
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
                XtraMessageBox.Show("获取初始化数据出错");
                _log.Error("获取初始化数据出错，出错提示：" + e.ToString());
            }

        }
        private void GetAllCheck()
        {
            //获取用户全部监测点
            var list = GlobalHandler.ledLinkresp.GetAllList(a => a.Led.Id.ToString() == Idd).ToList();
            List< BasicMonitorRecord > bmlist = new List < BasicMonitorRecord > ();
            for (int i = 0; i < list.Count; i++)
            {
                bmlist.Add(list[i].MonitorRecord);
            }
            gridControl1.DataSource = bmlist;
            gridView1.SelectAll();
        }
        private void BindControl(string mstype)
        {
            try { 
            var aList = GlobalHandler.monitorresp.GetAllList(a => a.BMID.Substring(6, 6).StartsWith(mstype)).ToList();
            if (aList.Count < 1)
            {
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

        private void treeList1_GetSelectImage(object sender, DevExpress.XtraTreeList.GetSelectImageEventArgs e)
        {
            //树图片
            if (e.Node == null) return;
            TreeListNode node = e.Node;
            if (!node.HasChildren) return;
            string ID = node.GetValue("ID").ToString();
            if (ID == "1")
            {
                e.NodeImageIndex = 1;
            }
            else if (node.HasChildren)
            {
                e.NodeImageIndex = 2;
            }
        }

        private void gridControl2_Click(object sender, EventArgs e)
        {
            //点击事件
        }

        private void gridView2_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            //获取选中状态
            if (gridView2.GetSelectedRows().IsNotEmpty())
            {
                Idd = gridView2.GetRowCellValue(gridView2.GetSelectedRows()[0], "Id").ToString();
                tree_mstype.EditValue = "";
                if (tree_mstype.EditValue.ToString() != "")
                {
                    BindControl(code);
                    BindCheck();
                }
                else
                {
                    GetAllCheck();
                }
            }
        }
    }
}