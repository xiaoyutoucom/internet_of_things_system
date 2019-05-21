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
using ServiceStack;
using System.Web.UI.WebControls;
using log4net;

namespace SmartKylinApp.View.LedConfig
{
    public partial class LedManager : DevExpress.XtraEditors.XtraUserControl
    {
        private bool isEdit;
        private List<LedModel> list;
        private List<TreeListModel> listTXFS;
        private ILog _log = LogManager.GetLogger("LedManager");
        public LedManager()
        {
            InitializeComponent();
            //禁止右键菜单
            layoutControl2.AllowCustomization = false;
            layoutControl1.AllowCustomization = false;
            bar2.Manager.AllowShowToolbarsPopup = false;
            bar2.OptionsBar.AllowQuickCustomization = false;
            gridView1.OptionsMenu.EnableColumnMenu = false;
        }
        //数据集绑定
        private void BindTXFSCODE()
        {
            listTXFS = new List<TreeListModel>();
            listTXFS.Add(new TreeListModel() { ID = "1", Name = "网络通讯" });
            listTXFS.Add(new TreeListModel() { ID = "2", Name = "串口通讯" });
            listTXFS.Add(new TreeListModel() { ID = "3", Name = "短信" });
            ListItem it = null;
            foreach (TreeListModel item in listTXFS)
            {
                it = new ListItem(item.Name, item.ID);
                this.com_TXFSCODE.Properties.Items.Add(it);
            }
            com_TXFSCODE.SelectedIndex = 0;
        }

        private void LedManager_Load(object sender, EventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            splashScreenManager1.SetWaitFormCaption("请稍后,数据加载中....");     // 标题
            //splashScreenManager1.SetWaitFormDescription("正在更新.....");　　　　　// 信息
            layoutControl2.Visible = false;
            BindTXFSCODE();
            GetData();
            splashScreenManager1.CloseWaitForm();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //新增
            layoutControl2.Visible = !layoutControl2.Visible;
            isEdit = false;
            CleanData();
        }

        private void CleanData()
        {
            //清空文本
            txt_MC.Text="";
            com_TXFSCODE.SelectedIndex = 0;
            txt_PHONE.Text = "";
            txt_CKH.Text = "";
            txt_BTL.Text = "";
            txt_KZKDZ.Text = "";
            txt_KZKIP.Text = "";
            txt_BDDK.Text = "";
            txt_MESSAGEFOMAT.Text = "";
            txt_ZBX.Text = "";
            txt_ZBY.Text = "";
            txt_BZ.Text = "";
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //编辑
            if (gridView1.GetSelectedRows().Length == 0) return;
            var idd = int.Parse(gridView1.GetRowCellValue(gridView1.GetSelectedRows()[0], "Id").ToString());
            if (idd == -1)
            {
                XtraMessageBox.Show("请选择数据！");
                return;
            }
            layoutControl2.Visible = !layoutControl2.Visible;
            isEdit = true;
            EditData();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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
                //GlobalHandler.ledresp.Delete(Idd);
                var index = gridView1.GetSelectedRows();
                index.Each(a => GlobalHandler.ledresp.Delete((int)(gridView1.GetRowCellValue(a, "Id"))));
                //XtraMessageBox.Show("删除成功");
                GetData();

            }
            catch (Exception exception)
            {
                XtraMessageBox.Show("删除失败");
                _log.Error("删除失败，出错提示：" + e.ToString());
            }
        
        }

        private void GetData()
        {
            try { 
                list = GlobalHandler.ledresp.GetAllList();
                gridControl1.DataSource = list;
                barStaticItem3.Caption = list.Count.ToString();
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取数据出错");
                _log.Error("获取数据出错，出错提示：" + e.ToString());
            }
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GetData();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //取消
            layoutControl2.Visible = !layoutControl2.Visible;
            CleanData();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //保存
            try
            {//验证必填项
                bool validate = false;
                StringBuilder st = new StringBuilder();
                if (string.IsNullOrEmpty(txt_MC.Text))
                {
                    txt_MC.Properties.Appearance.BorderColor = Color.Red;
                    validate = true;
                    st.Append("LED信息名称不能为空！\n\r");
                }
                else
                {
                    txt_MC.Properties.Appearance.BorderColor = Color.White;
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
                    var model = GlobalHandler.ledresp.Get(Idd);
                    model.MC = txt_MC.Text;
                    model.TXFSCODE = (com_TXFSCODE.SelectedItem as ListItem).Value;
                    model.TXFS = com_TXFSCODE.Text;
                    model.PHONE = txt_PHONE.Text;
                    model.CKH = txt_CKH.Text;
                    model.BTL = txt_BTL.Text;

                    model.KZKIP = txt_KZKIP.Text;
                    model.BDDK = int.Parse(txt_BDDK.Text == "" ? "0" : txt_BDDK.Text);
                    model.MESSAGEFOMAT = txt_MESSAGEFOMAT.Text;
                    model.ZBX = double.Parse(txt_ZBX.Text == "" ? "0" : txt_ZBX.Text);
                    model.ZBY = double.Parse(txt_ZBY.Text == "" ? "0" : txt_ZBY.Text);
                    model.BZ = txt_BZ.Text;
                    model.KZKDZ = txt_KZKDZ.Text;
                    GlobalHandler.ledresp.Update(model);
                    EditData();
                }
                else
                {
                    var model = new LedModel();
                    model.MC = txt_MC.Text;
                    var aaa = com_TXFSCODE.EditValue.ToString();
                    model.TXFSCODE = (com_TXFSCODE.SelectedItem as ListItem).Value;
                    model.TXFS = com_TXFSCODE.Text;
                    model.PHONE = txt_PHONE.Text;
                    model.CKH = txt_CKH.Text;
                    model.BTL = txt_BTL.Text;

                    model.KZKIP = txt_KZKIP.Text;
                    model.BDDK = int.Parse(txt_BDDK.Text == "" ? "0" : txt_BDDK.Text);
                    model.MESSAGEFOMAT = txt_MESSAGEFOMAT.Text;
                    model.ZBX = double.Parse(txt_ZBX.Text == "" ? "0" : txt_ZBX.Text);
                    model.ZBY = double.Parse(txt_ZBY.Text == "" ? "0" : txt_ZBY.Text);
                    model.BZ = txt_BZ.Text;
                    model.KZKDZ = txt_KZKDZ.Text;
                    GlobalHandler.ledresp.Insert(model);
                    CleanData();
                }

                GetData();
                XtraMessageBox.Show("保存成功");
                layoutControl2.Visible = !layoutControl2.Visible;
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show("数据保存失败");
                _log.Error("数据保存失败，出错提示：" + e.ToString());
            }

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
                if (gridView1.GetSelectedRows().Length == 0) return;
                var Id = int.Parse(gridView1.GetRowCellValue(gridView1.GetSelectedRows()[0], "Id").ToString());
                var model = GlobalHandler.ledresp.Get(Id);
                txt_MC.Text = model.MC;
                    for (int i = 0; i < listTXFS.Count; i++)
                    {
                        if (listTXFS[i].ID.Equals(model.TXFSCODE == null ? "1" : model.TXFSCODE))
                        {

                            com_TXFSCODE.SelectedIndex = i;
                        }
                    }
                    txt_PHONE.Text = model.PHONE;
                txt_CKH.Text = model.CKH;
                txt_BTL.Text = model.BTL;
                txt_KZKDZ.Text = model.KZKDZ;
                txt_KZKIP.Text = model.KZKIP;
                txt_BDDK.Text = model.BDDK.ToString();
                txt_MESSAGEFOMAT.Text = model.MESSAGEFOMAT;
                txt_ZBX.Text = model.ZBX.ToString();
                txt_ZBY.Text = model.ZBY.ToString();
                txt_BZ.Text = model.BZ;
            }
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取数据出错");
                _log.Error("获取数据出错，出错提示：" + e.ToString());
            }
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barEdit_mc.EditValue == null || barEdit_mc.EditValue.ToString() == "")
            {
                XtraMessageBox.Show("请输入查询内容");
                return;
            }
            //查找
            var sbbm = barEdit_mc.EditValue.ToString();

            if (string.IsNullOrEmpty(sbbm)) return;
            var listwhere = list.Where(a => a.MC.Contains(sbbm)).ToList();
            gridControl1.DataSource = listwhere;
            barStaticItem3.Caption = listwhere.Count.ToString();
        }
    }
}
