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
using System.Text.RegularExpressions;
using ServiceStack;
using System.Web.UI.WebControls;
using log4net;

namespace SmartKylinApp.View.LedConfig
{
    public partial class FontLibrary : DevExpress.XtraEditors.XtraUserControl
    {
        private bool isEdit;
        private List<LedFontLibrary> list;
        private List<TreeListModel> listTQ;
        private ILog _log = LogManager.GetLogger("FontLibrary");
        public FontLibrary()
        {
            InitializeComponent();
            //禁止右键菜单
            layoutControl2.AllowCustomization = false;
            layoutControl1.AllowCustomization = false;
            bar2.Manager.AllowShowToolbarsPopup = false;
            bar2.OptionsBar.AllowQuickCustomization = false;
            gridView1.OptionsMenu.EnableColumnMenu = false;
        }
        private void FontLibrary_Load(object sender, EventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            splashScreenManager1.SetWaitFormCaption("请稍后,数据加载中....");     // 标题
            //splashScreenManager1.SetWaitFormDescription("正在更新.....");　　　　　// 信息
            layoutControl1.Visible = false;
            GetData();
            BindTQ();
            splashScreenManager1.CloseWaitForm();
        }
        private void BindTQ()
        {
            //数据集绑定
            listTQ = new List<TreeListModel>();
            listTQ.Add(new TreeListModel() { ID = "0", Name = "正常" });
            listTQ.Add(new TreeListModel() { ID = "1", Name = "雨天" });
            listTQ.Add(new TreeListModel() { ID = "2", Name = "雪天" });
            ListItem it = null;
            foreach (TreeListModel item in listTQ)
            {
                it = new ListItem(item.Name, item.ID);
                this.com_TQ.Properties.Items.Add(it);
            }
            com_TQ.SelectedIndex = 0;
        }
        private void GetData()
        {
            try{ 
            list = GlobalHandler.ledFontresp.GetAllList();
            gridControl1.DataSource = list;
            barStaticItem3.Caption = list.Count.ToString();
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
                if (isEdit)
                {
                    if (gridView1.GetSelectedRows().Length == 0) return;
                    if (me_XXNR.Text == "")
                    {
                        XtraMessageBox.Show("信息内容不能为空");
                        return;
                    }
                    var Idd = int.Parse(gridView1.GetRowCellValue(gridView1.GetSelectedRows()[0], "Id").ToString());
                    var model = GlobalHandler.ledFontresp.Get(Idd);
                    model.WEATHERTYPE = int.Parse((com_TQ.SelectedItem as ListItem).Value);
                    model.WEATHERDESC = com_TQ.Text.ToString();
                    model.WATERLEVEL = txt_FWJB.Text;
                    model.CONTENT = me_XXNR.Text;
                    model.EXTEND = txt_BZ.Text;
                    GlobalHandler.ledFontresp.Update(model);

                }
                else
                {
                    var model = new LedFontLibrary();
                    if (me_XXNR.Text == "")
                    {
                        XtraMessageBox.Show("信息内容不能为空");
                        return;
                    }
                    model.WEATHERTYPE = int.Parse((com_TQ.SelectedItem as ListItem).Value);
                    model.WEATHERDESC = com_TQ.Text.ToString();
                    model.WATERLEVEL = txt_FWJB.Text;
                    model.CONTENT = me_XXNR.Text;
                    model.EXTEND = txt_BZ.Text;
                    
                    GlobalHandler.ledFontresp.Insert(model);
                    CleanData();
                }

                GetData();
                XtraMessageBox.Show("保存成功");
                layoutControl1.Visible = !layoutControl1.Visible;
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show("数据保存失败");
                _log.Error("数据保存失败，出错提示：" + e.ToString());
            }
        }

        private void CleanData()
        {
            com_TQ.SelectedIndex = 0;
            //com_TQ.SelectedText=model.WEATHERDESC;
            txt_FWJB.Text ="";
            me_XXNR.Text = "";
            txt_BZ.Text = "";
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {
            EditDate();
        }

        private void EditDate()
        {
            try { 
            if (layoutControl1.Visible == false)
            {
                return;
            }
            if (isEdit)
            {
                if (gridView1.GetSelectedRows().Length == 0) return;
                var Id = int.Parse(gridView1.GetRowCellValue(gridView1.GetSelectedRows()[0], "Id").ToString());
                var model = GlobalHandler.ledFontresp.Get(Id);
          
                    for (int i = 0; i < listTQ.Count; i++)
                    {
                        if (listTQ[i].ID.Equals(model.WEATHERTYPE== null? "0" : model.WEATHERTYPE.ToString()))
                        {

                            com_TQ.SelectedIndex = i;
                        }
                    }
                    //com_TQ.SelectedText=model.WEATHERDESC;
                    txt_FWJB.Text = model.WATERLEVEL;
                me_XXNR.Text = model.CONTENT;
                txt_BZ.Text = model.EXTEND;
            }
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取行数据出错");
                _log.Error("获取行数据出错，出错提示：" + e.ToString());
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //新增
            layoutControl1.Visible = !layoutControl1.Visible;
            isEdit = false;
            CleanData();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //取消
            layoutControl1.Visible = !layoutControl1.Visible;
            CleanData();
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
            layoutControl1.Visible = !layoutControl1.Visible;
            isEdit = true;
            EditDate();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // 删除
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
                //GlobalHandler.ledFontresp.Delete(Idd);
                var index = gridView1.GetSelectedRows();
                index.Each(a => GlobalHandler.ledFontresp.Delete((int)(gridView1.GetRowCellValue(a, "Id"))));
                //XtraMessageBox.Show("删除成功");
                GetData();

            }
            catch (Exception exception)
            {
                XtraMessageBox.Show("删除失败");
                _log.Error("删除失败，出错提示：" + e.ToString());
            }
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GetData();
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
            var listwhere = list.Where(a => a.CONTENT.Contains(sbbm)).ToList();
            gridControl1.DataSource = listwhere;
            barStaticItem3.Caption = listwhere.Count.ToString();
        }
    }
}
