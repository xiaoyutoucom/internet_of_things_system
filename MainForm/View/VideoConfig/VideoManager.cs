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
using SmartKylinApp.Common;
using SmartKylinData.IOTModel;
using DevExpress.XtraTreeList;
using SmartKylinApp.View.BaseConfig;
using DevExpress.XtraTreeList.Nodes;
using ServiceStack;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraBars;
using System.Web.UI.WebControls;
using log4net;

namespace SmartKylinApp.View.MsgConfig
{ 
    public partial class VideoManager : DevExpress.XtraEditors.XtraUserControl
    {
        private ILog _log = LogManager.GetLogger("VideoManager");
        private int Id;
        private bool isEdit;
        private bool sftop;
        private List<TreeListModel> listSPJXLX;
        private List<TreeListModel> listSPPP;

        public VideoManager()
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

        private void VideoManager_Load(object sender, EventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            splashScreenManager1.SetWaitFormCaption("请稍后,数据加载中....");     // 标题
            //splashScreenManager1.SetWaitFormDescription("正在更新.....");　　　　　// 信息
            BindTree();
            layoutControl2.Visible = false;
            BindSPPP();
            BindSPJXLX();
            splashScreenManager1.CloseWaitForm();
        }
        private void BindSPJXLX()
        {
            //数据集绑定
            listSPJXLX = new List<TreeListModel>();
            listSPJXLX.Add(new TreeListModel() { ID = "hls", Name = "hls" });
            listSPJXLX.Add(new TreeListModel() { ID = "rtsp", Name = "rtsp" });
            listSPJXLX.Add(new TreeListModel() { ID = "rtspstream", Name = "rtsp取流" });
            listSPJXLX.Add(new TreeListModel() { ID = "hk8700", Name = "海康8700" });
            listSPJXLX.Add(new TreeListModel() { ID = "hkdev", Name = "海康8700二次开发" });
            ListItem it = null;
            foreach (TreeListModel item in listSPJXLX)     
            {
                it = new ListItem(item.Name, item.ID);
                this.com_SPJXLX.Properties.Items.Add(it);
            }
            com_SPJXLX.SelectedIndex = 0;


            //数据集绑定
            foreach (TreeListModel item in listSPJXLX)
            {
                it = new ListItem(item.Name, item.ID);
                this.com_SPJXLX2.Properties.Items.Add(it);
            }
            com_SPJXLX2.SelectedIndex = 1;
        }
        private void BindSPPP()
        {
            //数据集绑定
            listSPPP = new List<TreeListModel>();
            listSPPP.Add(new TreeListModel() { ID = "海康", Name = "海康" });
            listSPPP.Add(new TreeListModel() { ID = "大华", Name = "大华" });
            ListItem it = null;
            foreach (TreeListModel item in listSPPP)
            {
                it = new ListItem(item.Name, item.ID);
                this.com_SPPP.Properties.Items.Add(it);
            }
            com_SPJXLX.SelectedIndex = 0;
        }
        private void BindTree()
        {
            try { 
            var datas = GlobalHandler.videogroupresp.GetAllList().OrderBy(a=>a.XH).ToList();//排序
            treeList1.DataSource = datas;
            treeList1.ExpandAll();
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取分组数据出错");
                _log.Error("获取分组数据出错，出错提示：" + e.ToString());
            }
        }

        private void treeList1_MouseUp(object sender, MouseEventArgs e)
        {
            
            //分组右击菜单显示
            if ((e.Button == MouseButtons.Right) && (ModifierKeys == Keys.None)
                )
            {
                TreeList tree = sender as TreeList;
                Point p = new Point(Cursor.Position.X, Cursor.Position.Y);
                TreeListHitInfo hitInfo = tree.CalcHitInfo(e.Location);
                TreeListNode node = hitInfo.Node;
                if (node == null)
                {
                    Id = -1;
                    barButtonItem3.Visibility = BarItemVisibility.Never;
                    barButtonItem2.Visibility = BarItemVisibility.Never;
                    //if (hitInfo.HitInfoType == HitInfoType.Cell)
                    {
                        tree.SetFocusedNode(hitInfo.Node);
                        popupMenu1.ShowPopup(p);
                    }

                }
                else
                {
                    node.TreeList.FocusedNode = node;
                Id = int.Parse(treeList1.FocusedNode.GetValue("Id").ToString());
                barButtonItem3.Visibility = BarItemVisibility.Always;
                barButtonItem2.Visibility = BarItemVisibility.Always;
                if (hitInfo.HitInfoType == HitInfoType.Cell)
                {
                    tree.SetFocusedNode(hitInfo.Node);
                    popupMenu1.ShowPopup(p);
                }
                }
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //分组新增
            var dow = new VideoAddManager();
            dow.Id = Id;
            dow.isEdit = true;
            if (dow.ShowDialog() != DialogResult.OK)
            {
                BindTree();
                return;
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //分组修改
            var dow = new VideoAddManager();
            dow.Id = Id;
            dow.isEdit = false;
            if (dow.ShowDialog() != DialogResult.OK)
            {
                BindTree();
                return;
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //分组删除
            try { 
            var box = new XtraMessageBoxArgs();
            box.Caption = "提示";
            box.Text = "确定要删除吗？";
            box.Buttons = new DialogResult[] { DialogResult.OK, DialogResult.Cancel };
            box.Showing += ShowButton.Box_Showing;
            if (XtraMessageBox.Show(box) != DialogResult.OK)
            {
                return;
            }
            GlobalHandler.videogroupresp.Delete(Id);
            BindTree();
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show("分组删除失败");
                _log.Error("分组删除失败，出错提示：" + exception.ToString());
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //取消
            layoutControl2.Visible = !layoutControl2.Visible;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //保存
            try
            {
                Id = int.Parse(treeList1.FocusedNode.GetValue("Id").ToString());
                //验证必填项
                bool validate = false;
                StringBuilder st = new StringBuilder();
                if (string.IsNullOrEmpty(txt_SPDBM.Text))
                {
                    txt_SPDBM.Properties.Appearance.BorderColor = Color.Red;
                    validate = true;
                    st.Append("视频点编码不能为空！\n\r");
                }
                else
                {
                    txt_SPDBM.Properties.Appearance.BorderColor = Color.White;
                }
                if ( string.IsNullOrEmpty(txt_SPDMC.Text))
                {
                    txt_SPDMC.Properties.Appearance.BorderColor = Color.Red;                  
                    validate = true;
                    st.Append("视频点名称不能为空！\n\r");
                }
                else
                {
                    txt_SPDMC.Properties.Appearance.BorderColor = Color.White;
                }
                if (validate)
                {
                    XtraMessageBox.Show(st.ToString());
                    return;
                }
                if (isEdit)
                {
                    if (gridView1.GetSelectedRows().Length == 0) return;
                    var Idd = int.Parse(gridView1.GetRowCellValue(gridView1.GetSelectedRows()[0], "Id").ToString());
                    var model = GlobalHandler.videoresp.Get(Idd);
                    model.SPDMC = txt_SPDMC.Text;
                    model.SPPP = com_SPPP.EditValue.ToString();
                    model.SPJXLX = com_SPJXLX2.Text;   //获取选中项的值
                    model.XSQS = long.Parse(txt_XSQS.Text == "" ? "0" : txt_XSQS.Text);
                    model.IPLX = txt_IPLX.Text;
                    model.IP = txt_IP.Text;
                    model.DKH = txt_DKH.Text;
                    model.XH = decimal.Parse(txt_XH.Text == "" ? "0" : txt_XH.Text);
                    model.X = decimal.Parse(txt_X.Text == "" ? "0" : txt_X.Text);
                    model.Y = decimal.Parse(txt_Y.Text == "" ? "0" : txt_Y.Text);
                    model.YHM = txt_YHM.Text;
                    model.MM = txt_MM.Text;
                    model.JXCS = txt_JXCS.Text;
                    model.SPJXLX = com_SPJXLX.Text;
                    //model.SPJXLXBM = com_SPJXLX.Text;
                    model.EXTENDCODE = textEdit2.Text;
                    model.JXCS2 = txt_JXCS2.Text;
                    model.SPJXLX2 = com_SPJXLX2.Text;
                    //model.SPJXLXBM2 = com_SPJXLX2.Text;
                    if (com_SPJXLX.SelectedItem as ListItem != null)
                    {
                        model.SPJXLXBM = (com_SPJXLX.SelectedItem as ListItem).Value.Trim();
                    }
                    else
                    {
                        model.SPJXLXBM = com_SPJXLX.Text;
                    }
                    if (com_SPJXLX.SelectedItem as ListItem != null)
                    {
                        model.SPJXLXBM2 = (com_SPJXLX2.SelectedItem as ListItem).Value.Trim();
                    }
                    else
                    {
                        model.SPJXLXBM2 = com_SPJXLX2.Text;
                    }
                    model.SPDBM = txt_SPDBM.Text.Trim();
                    GlobalHandler.videoresp.Update(model);
                    EditData();
                }
                else
                {
                    var model = new VideoRecord();
                    //model.SPDBM = Guid.NewGuid().ToString();
                    model.SPDBM = txt_SPDBM.Text.Trim();
                    model.SPDMC = txt_SPDMC.Text;
                    model.XSQS = long.Parse(txt_XSQS.Text == "" ? "0" : txt_XSQS.Text);
                    model.IPLX = txt_IPLX.Text;
                    model.IP = txt_IP.Text;
                    model.DKH = txt_DKH.Text;
                    model.XH = decimal.Parse(txt_XH.Text == "" ? "0" : txt_XH.Text);
                    model.X = decimal.Parse(txt_X.Text == "" ? "0" : txt_X.Text);
                    model.Y = decimal.Parse(txt_Y.Text == "" ? "0" : txt_Y.Text);
                    model.YHM = txt_YHM.Text;
                    model.MM = txt_MM.Text;
                    model.JXCS = txt_JXCS.Text;
                    model.SPPP = com_SPPP.EditValue.ToString();
                    model.SPJXLX = com_SPJXLX.Text;
                    
                    model.EXTENDCODE = textEdit2.Text;
                    model.FZID = Id;
                    model.JXCS2 = txt_JXCS2.Text;
                    model.SPJXLX2 = com_SPJXLX2.Text;
                    if (com_SPJXLX.SelectedItem as ListItem != null)
                    { 
                        model.SPJXLXBM = (com_SPJXLX.SelectedItem as ListItem).Value.Trim();
                    }
                    else
                    {
                        model.SPJXLXBM = com_SPJXLX.Text;
                    }
                    if (com_SPJXLX.SelectedItem as ListItem != null)
                    {
                        model.SPJXLXBM2 = (com_SPJXLX2.SelectedItem as ListItem).Value.Trim();
                    }
                    else
                    {
                        model.SPJXLXBM2 = com_SPJXLX2.Text;
                    }
                    GlobalHandler.videoresp.Insert(model);
                    CleanData();
                }

                GetAllData(Id);
                XtraMessageBox.Show("保存成功");
                layoutControl2.Visible = !layoutControl2.Visible;

            }
            catch (Exception exception)
            {
                XtraMessageBox.Show("数据保存失败");
                _log.Error("数据保存失败，出错提示：" + exception.ToString());
            }
        }

        private void GetAllData(int mstype)
        {
            try { 
            if (!string.IsNullOrEmpty(mstype.ToString()))
            {
                var aList = GlobalHandler.videoresp.GetAllList(a => a.FZID==(mstype)).ToList();
                gridControl1.DataSource = aList;
            }
            else
            {
                gridControl1.DataSource = null;
            }
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取数据出错");
                _log.Error("获取数据出错，出错提示：" + e.ToString());
            }
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //新增
            layoutControl2.Visible = !layoutControl2.Visible;
            isEdit = false;
            CleanData();
            //txt_SPDBM.Text = Guid.NewGuid().ToString();
            txt_SPDBM.Text = "";

        }

        private void CleanData()
        {
            //清空数据
            txt_SPDBM.Text = "";
            txt_SPDMC.Text = "";
            com_SPPP.SelectedIndex = 0;
            txt_XSQS.Text = "";
            txt_IPLX.Text = "";
            txt_IP.Text = "";
            txt_DKH.Text = "";
            txt_XH.Text = "";
            txt_X.Text = "";
            txt_Y.Text = "";
            txt_YHM.Text = "";
            txt_MM.Text = "";
            txt_JXCS.Text = "";
            com_SPJXLX.SelectedIndex = 0;
            textEdit2.Text = "";
            txt_JXCS2.Text = "";
            com_SPJXLX2.SelectedIndex = 1;

        }

        private void treeList1_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            try
            {
                Id = int.Parse(treeList1.FocusedNode.GetValue("Id").ToString());
            if (!string.IsNullOrEmpty(Id.ToString()))
            {
               var aList = GlobalHandler.videoresp.GetAllList(a => a.FZID==Id).ToList();
                gridControl1.DataSource = aList;
            }
            else
            {
                gridControl1.DataSource = null;
            }
            EditData();
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show("获取详细信息失败");
                _log.Error("获取详细信息失败，出错提示：" + exception.ToString());
            }
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //删除
            try
            {
                if (gridView1.GetSelectedRows().Length == 0) return;
                var box = new XtraMessageBoxArgs();
                box.Caption = "提示";
                box.Text = "确定要删除吗？";
                box.Buttons = new DialogResult[] { DialogResult.OK, DialogResult.Cancel };
                box.Showing += ShowButton.Box_Showing;
                if (XtraMessageBox.Show(box) != DialogResult.OK)
                {
                    return;
                }
                //var Idd = int.Parse(gridView1.GetRowCellValue(gridView1.GetSelectedRows()[0], "Id").ToString());
                //GlobalHandler.videoresp.Delete(Idd);
                var index = gridView1.GetSelectedRows();
                index.Each(a => GlobalHandler.videoresp.Delete((int)(gridView1.GetRowCellValue(a, "Id"))));
                //XtraMessageBox.Show("删除成功");
                GetAllData(Id);

            }
            catch (Exception exception)
            {
                XtraMessageBox.Show("删除失败");
                _log.Error("删除失败，出错提示：" + exception.ToString());
            }
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //刷新
            GetAllData(Id);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //编辑
            //if (layoutControl2.Visible == false)
            //{
            //    return;
            //}
            if (gridView1.GetSelectedRows().Length == 0) return;
            
            var idd = int.Parse(gridView1.GetRowCellValue(gridView1.GetSelectedRows()[0], "Id").ToString());
            if (idd == -1)
            {
                XtraMessageBox.Show("请选择设备！");
                return;
            }
            layoutControl2.Visible = !layoutControl2.Visible;
            isEdit = true;
            EditData();
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {
            EditData();
            
        }

        private void EditData()
        {
            try { 
            if (isEdit)
            {
                if (gridView1.GetSelectedRows().Length == 0)
                {
                    CleanData();
                    return;
                }
                int Idd = int.Parse(gridView1.GetRowCellValue(gridView1.GetSelectedRows()[0], "Id").ToString());
                var model = GlobalHandler.videoresp.Get(Idd);
                txt_SPDBM.Text = model.SPDBM;
                txt_SPDMC.Text = model.SPDMC;
                   
                txt_XSQS.Text = model.XSQS.ToString();
                txt_IPLX.Text = model.IPLX;
                txt_IP.Text = model.IP;
                txt_DKH.Text = model.DKH;
                txt_XH.Text = model.XH.ToString();
                txt_X.Text = model.X.ToString();
                txt_Y.Text = model.Y.ToString();
                txt_YHM.Text = model.YHM;
                txt_MM.Text = model.MM;
                txt_JXCS.Text = model.JXCS;
                    com_SPJXLX.Text = model.SPJXLXBM;
                    com_SPJXLX2.Text = model.SPJXLXBM2;
                    for (int i = 0; i < listSPJXLX.Count; i++)
                    {
                        if (listSPJXLX[i].ID.Equals(model.SPJXLXBM))
                        {

                            com_SPJXLX.SelectedIndex = i;
                        }
                    }
                    for (int i = 0; i < listSPJXLX.Count; i++)
                    {
                        if (listSPJXLX[i].ID.Equals(model.SPJXLXBM2))
                        {

                            com_SPJXLX2.SelectedIndex = i;
                        }
                    }
                    //com_SPJXLX.Text = model.SPJXLXBM.Trim();
                    //com_SPJXLX2.Text = model.SPJXLXBM2.Trim();
                    txt_JXCS2.Text = model.JXCS2;
                    com_SPPP.EditValue = model.SPPP;
                    textEdit2.Text = model.EXTENDCODE;
                }
            else
            {
                //CleanData();
            }
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取行数据出错");
                _log.Error("获取行数据出错，出错提示：" + e.ToString());
            }
        }

        private void treeList1_GetSelectImage(object sender, GetSelectImageEventArgs e)
        {
            //树图片
            if(e.Node == null) return;
            TreeListNode node = e.Node;
            if (node.GetValue("FID")==null) return;
            int FID = (int)node.GetValue("FID");
            if (FID == -1|| FID == 0)
            {
                e.NodeImageIndex = 1;
            }
            else
            {
                e.NodeImageIndex = 0;

            }
            //else 
            //if (node.HasChildren)
            //{
            //    e.NodeImageIndex = 1;
            //}
        }
    }
}
