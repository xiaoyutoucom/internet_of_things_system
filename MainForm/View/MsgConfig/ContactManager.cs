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
using DevExpress.XtraBars;
using log4net;

namespace SmartKylinApp.View.MsgConfig
{ 
    public partial class ContactManager : DevExpress.XtraEditors.XtraUserControl
    {
        private int Id;
        private bool isEdit;
        private ILog _log = LogManager.GetLogger("ContactManager");
        public ContactManager()
        {
            InitializeComponent();
            //禁止右键菜单
            layoutControl3.AllowCustomization = false;
            layoutControl2.AllowCustomization = false;
            layoutControl1.AllowCustomization = false;
            bar2.Manager.AllowShowToolbarsPopup = false;
            bar2.OptionsBar.AllowQuickCustomization = false;
            gridView1.OptionsMenu.EnableColumnMenu = false;
            treeList1.OptionsMenu.EnableColumnMenu = false;
        }

        private void ContactManager_Load(object sender, EventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            splashScreenManager1.SetWaitFormCaption("请稍后,数据加载中....");     // 标题
            //splashScreenManager1.SetWaitFormDescription("正在更新.....");　　　　　// 信息
            BindTree();
            layoutControl2.Visible = false;
            splashScreenManager1.CloseWaitForm();
        }
        private void BindTree()
        {
            try { 
            var datas = GlobalHandler.contactgroupresp.GetAllList();
            treeList1.DataSource = datas;
            treeList1.ExpandAll();
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取数据出错");
                _log.Error("获取数据出错，出错提示：" + e.ToString());
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
            var dow = new ContactAddManager();
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
            var dow = new ContactAddManager();
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
            var box = new XtraMessageBoxArgs();
            box.Caption = "提示";
            box.Text = "确定要删除吗？";
            box.Buttons = new DialogResult[] { DialogResult.OK, DialogResult.Cancel };
            box.Showing += ShowButton.Box_Showing;
            if (XtraMessageBox.Show(box) != DialogResult.OK)
            {
                return;
            }
            GlobalHandler.contactgroupresp.Delete(Id);
            BindTree();
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
                //验证必填项
                bool validate = false;
                StringBuilder st = new StringBuilder();
                if (string.IsNullOrEmpty(txt_NAME.Text))
                {
                    txt_NAME.Properties.Appearance.BorderColor = Color.Red;
                    validate = true;
                    st.Append("姓名不能为空！\n\r");
                }
                else
                {
                    txt_NAME.Properties.Appearance.BorderColor = Color.White;
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
                    var model = GlobalHandler.contactresp.Get(Idd);
                    model.NAME = txt_NAME.Text;
                    model.GENDER = rdo_GENDER.EditValue.ToString();
                    model.BIRTHDAY = date_BIRTHDAY.DateTime;
                    model.MAJOB = txt_MAJOB.Text;
                    model.PHONE = txt_PHONE.Text;
                    model.DEPARTMENT = txt_DEPARTMENT.Text;
                    model.ADDRESS = txt_ADDRESS.Text;
                    model.JOB = txt_JOB.Text;
                    model.DUTY = txt_DUTY.Text;
                    model.SECTION = txt_SECTION.Text;
                    model.EXTENDCODE = textEdit2.Text;

                    //ContactsGroup modelGroup = GlobalHandler.contactgroupresp.Get(Id);
                    //model.CONTACTSGROUP = modelGroup;
                    GlobalHandler.contactresp.Update(model);
                    EditData();
                }
                else
                {

                    var model = new Contact();
                    model.NAME = txt_NAME.Text;
                    model.GENDER = rdo_GENDER.EditValue.ToString();
                    model.BIRTHDAY = date_BIRTHDAY.DateTime;
                    model.MAJOB = txt_MAJOB.Text;
                    model.PHONE = txt_PHONE.Text;
                    model.DEPARTMENT = txt_DEPARTMENT.Text;
                    model.ADDRESS = txt_ADDRESS.Text;
                    model.JOB = txt_JOB.Text;
                    model.DUTY = txt_DUTY.Text;
                    model.SECTION = txt_SECTION.Text;
                    model.EXTENDCODE = textEdit2.Text;

                    ContactsGroup modelGroup = GlobalHandler.contactgroupresp.Get(Id);
                    model.CONTACTSGROUP = modelGroup;
                    GlobalHandler.contactresp.Insert(model);
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
                var aList = GlobalHandler.contactresp.GetAllList(a => a.CONTACTSGROUP.Id==(mstype)).ToList();
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
        }

        private void CleanData()
        {
            //清空数据
            txt_NAME.Text="";
            rdo_GENDER.EditValue="1";
            date_BIRTHDAY.DateTime=DateTime.Now;
            txt_MAJOB.Text="";
            txt_PHONE.Text="";
            txt_DEPARTMENT.Text="";
            txt_ADDRESS.Text="";
            txt_JOB.Text="";
            txt_DUTY.Text="";
            txt_SECTION.Text="";
            textEdit2.Text="";

        }

        private void treeList1_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            try { 
            Id = int.Parse(treeList1.FocusedNode.GetValue("Id").ToString());
            if (!string.IsNullOrEmpty(Id.ToString()))
            {
                var aList = GlobalHandler.contactresp.GetAllList(a => a.CONTACTSGROUP.Id==Id).ToList();
                gridControl1.DataSource = aList;
                
            }
            else
            {
                gridControl1.DataSource = null;
            }
            EditData();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("获取数据出错");
                _log.Error("获取数据出错，出错提示：" + ex.ToString());
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
                //GlobalHandler.contactresp.Delete(Idd);
                var index = gridView1.GetSelectedRows();
                index.Each(a => GlobalHandler.contactresp.Delete((int)(gridView1.GetRowCellValue(a, "Id"))));
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
            if (layoutControl2.Visible == false)
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
                int Idd = int.Parse(gridView1.GetRowCellValue(gridView1.GetSelectedRows()[0], "Id").ToString());
                var model = GlobalHandler.contactresp.Get(Idd);
                txt_NAME.Text = model.NAME;
                rdo_GENDER.EditValue = model.GENDER;
                date_BIRTHDAY.DateTime = model.BIRTHDAY;
                txt_MAJOB.Text = model.MAJOB;
                txt_PHONE.Text = model.PHONE;
                txt_DEPARTMENT.Text = model.DEPARTMENT;
                txt_ADDRESS.Text = model.ADDRESS;
                txt_JOB.Text = model.JOB;
                txt_DUTY.Text = model.DUTY;
                txt_SECTION.Text = model.SECTION;
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
            
             if (node.GetValue("PARENTID") == null) return;
            int FID = (int)node.GetValue("PARENTID");
            if (FID == -1||FID == 0)
            {
                e.NodeImageIndex = 1;
            }
            else
            {
                e.NodeImageIndex = 0;

            }
        }
    }
}
