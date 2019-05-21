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
using SmartKylinApp.View.BaseConfig;
using SmartKylinData.IOTModel;
using SmartKylinApp.Common;
using ServiceStack;
using log4net;

namespace SmartKylinApp.View.MsgConfig
{
    public partial class ContactLinkMonitor : DevExpress.XtraEditors.XtraUserControl
    {
        private List<WorkerLinkMonitor> list;
        private ILog _log = LogManager.GetLogger("ContactLinkMonitor");
        public ContactLinkMonitor()
        {
            InitializeComponent();
            //禁止右键菜单
            layoutControl2.AllowCustomization = false;
            bar2.Manager.AllowShowToolbarsPopup = false;
            bar2.OptionsBar.AllowQuickCustomization = false;
            gridView1.OptionsMenu.EnableColumnMenu = false;
        }
        private void ContactLinkMonitor_Load(object sender, EventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            splashScreenManager1.SetWaitFormCaption("请稍后,数据加载中....");     // 标题
            //splashScreenManager1.SetWaitFormDescription("正在更新.....");　　　　　// 信息
            GetData();
            splashScreenManager1.CloseWaitForm();
        }
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //新增
            ContactLinkHelp from = new ContactLinkHelp();
            from.StartPosition = FormStartPosition.CenterScreen;
            from.ShowDialog();
            GetData();
        }
        private void GetData()
        {
            //获取数据
            try { 
            list = GlobalHandler.wlinkmresp.GetAllList();
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

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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
                var index = gridView1.GetSelectedRows();
                index.Each(a => GlobalHandler.wlinkmresp.Delete((int)(gridView1.GetRowCellValue(a, "Id"))));
                //XtraMessageBox.Show("删除成功");
                GetData();

            }
            catch (Exception exception)
            {
                XtraMessageBox.Show("删除失败");
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barEdit_mc.EditValue == null || barEdit_mc.EditValue.ToString() == "")
            {
                XtraMessageBox.Show("请输入查询内容");
                return;
            }
            //查找
            var sbbm = barEdit_mc.EditValue.ToString();

            if (string.IsNullOrEmpty(sbbm)) return;
            var listwhere = list.Where(a => a.BaseMonitor.BMMC.Contains(sbbm)).ToList();
            gridControl1.DataSource = listwhere;
            barStaticItem3.Caption = listwhere.Count.ToString();
        }
    }
}
