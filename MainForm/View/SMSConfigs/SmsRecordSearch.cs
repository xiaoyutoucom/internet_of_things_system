using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SmartKylinApp.Common;
using ServiceStack;
using DevExpress.XtraEditors;
using SmartKylinData.IOTModel;
using SmartKylinApp.View.UnifiedConfigs;
using SmartKylinApp.View.BaseConfig;
using log4net;

namespace SmartKylinApp.View.MsgConfig
{
    public partial class SmsRecordSearch : UserControl
    { 
        private bool isEdit;
        BasicMonitorRecord bmmodel = new BasicMonitorRecord();
        private List<SmsRecorder> list;
        private ILog _log = LogManager.GetLogger("SmsRecordSearch");
        public SmsRecordSearch()
        {
            InitializeComponent();
            //禁止右键菜单
            layoutControl2.AllowCustomization = false;
            layoutControl1.AllowCustomization = false;
            bar2.Manager.AllowShowToolbarsPopup = false;
            bar2.OptionsBar.AllowQuickCustomization = false;
            gridView1.OptionsMenu.EnableColumnMenu = false;
        }

        private void TemplateManager_Load(object sender, EventArgs e)
        {
            splashScreenManager3.ShowWaitForm();
            splashScreenManager3.SetWaitFormCaption("请稍后,数据加载中....");     // 标题
            //splashScreenManager1.SetWaitFormDescription("正在更新.....");　　　　　// 信息
            layoutControl2.Visible = false;
            GetData();
            splashScreenManager3.CloseWaitForm();

            //cmbParam.Properties.Items.Add(new ComModel()
            //{
            //    Text = "全部",
            //    Value = "00"
            //});
            cmbTYPE.Properties.Items.Add("");
            cmbTYPE.Properties.Items.Add("通知");
            //cmbState.Properties.Items.Add("MonitorName");
            //cmbState.Properties.Items.Add("TagDesc");
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {            
            layoutControl2.Visible = !layoutControl2.Visible;
            isEdit = false;
            CleanData();  
        }

        private void CleanData()
        {
            txtSysCode.Text = "";
            txtSmsNo.Text = "";
            cmbTYPE.EditValue = "";
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
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
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
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
                //GlobalHandler.msgtempleteresp.Delete(Idd);
                var index = gridView1.GetSelectedRows();
                index.Each(a => GlobalHandler.msgSmsConfigtresp.Delete((int)(gridView1.GetRowCellValue(a, "Id"))));
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
            //获取数据
            try { 
            var aList = GlobalHandler.msgSmsRecordresp.GetAllList();
                foreach (var v in aList)
                {
                    if (v.STATUS.Trim() == "1")
                    {
                        v.STATUS = "发送成功";
                    }
                    else if (v.STATUS.Trim() == "2")
                    {
                        v.STATUS = "发送失败";
                    }
                }
            list = aList;
            gridControl1.DataSource = list;
            barStaticItem3.Caption = list.Count.ToString();
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取数据出错");
                _log.Error("获取数据出错，出错提示：" + e.ToString());
            }
        }
        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {            
            GetData();
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            layoutControl2.Visible = !layoutControl2.Visible;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //layoutControl2.Visible = !layoutControl2.Visible;
            try
            {
                var listwhere = list;// GlobalHandler.msgSmsRecordresp.GetAllList();
                if (txtSysCode.Text.ToString() == "" && txtSmsNo.Text.ToString() == "" && cmbTYPE.EditValue == null && txtSmsContent.Text.ToString() == "" && cmbState.EditValue==null && smsSendTimeBegin.EditValue==null && smsSendTimeEnd.EditValue==null)
                {
                    XtraMessageBox.Show("请输入查询内容");
                    return;
                }
                if (txtSysCode.Text.ToString() != "")
                {
                    listwhere = listwhere.Where(p => p.SYSTEMCODE.Contains(txtSysCode.Text.ToString())).ToList();
                }
                if (txtSmsNo.Text.ToString() != "")
                {
                    listwhere = listwhere.Where(p => p.PHONENUM.Contains(txtSmsNo.Text.ToString())).ToList();
                }
                if (cmbTYPE.EditValue  != null)
                {
                    listwhere = listwhere.Where(p => p.TYPE.Contains(cmbTYPE.EditValue.ToString())).ToList();
                }
                if (txtSmsContent.Text.ToString() != "")
                {
                    listwhere = listwhere.Where(p => p.CONTENT.Contains(txtSmsContent.Text.ToString())).ToList();
                }
                if (cmbState.EditValue != null)
                {
                    //1、发送成功  2、发送失败
                    //listwhere = listwhere.Where(p => p.STATUS.Trim()==cmbState.SelectedIndex.ToString()).ToList();
                    listwhere = listwhere.Where(p => p.STATUS == cmbState.EditValue.ToString()).ToList();
                }
                if (smsSendTimeBegin.EditValue != null)
                {
                    //listwhere = listwhere.Where(p => DateTime.Parse(p.TIME) >= DateTime.Parse(smsSendTimeBegin.EditValue.ToString())).ToList();
                    listwhere = listwhere.Where(p => p.TIME >= decimal.Parse(DateTime.Parse(barEditItem6.EditValue.ToString()).ToString("yyyyMMddhhmmss"))).ToList();
                }
                if (smsSendTimeEnd.EditValue != null)
                {
                    //listwhere = listwhere.Where(p => DateTime.Parse(p.TIME) <= DateTime.Parse(smsSendTimeEnd.EditValue.ToString())).ToList();
                    listwhere = listwhere.Where(p => p.TIME <= decimal.Parse(DateTime.Parse(barEditItem8.EditValue.ToString()).ToString("yyyyMMddhhmmss"))).ToList();
                }
                gridControl1.DataSource = listwhere;
                barStaticItem3.Caption = listwhere.Count.ToString();
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show("查询失败");
                _log.Error("出错提示：" + e.ToString());
            }
        }

        private void layoutControlItem64_Click(object sender, EventArgs e)
        {
            //关联监测点

        }

        private void bet_stationtype_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //MonitorHelp from = new MonitorHelp();
            //from.StartPosition = FormStartPosition.CenterScreen;
            //from.ShowDialog();

            //bmmodel = from.model;
            //if (bmmodel == null) return;
            //bet_stationtype.Tag = bmmodel.BMID;
            //bet_stationtype.Text = bmmodel.BMMC;
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {
            EditData();
            //点击选择
            
        }

        private void EditData()
        {
            try
            {
                if (layoutControl2.Visible == false)
                {
                    return;
                }
                if (isEdit)
                {
                    if (gridView1.GetSelectedRows().Length == 0) return;
                    var Id = int.Parse(gridView1.GetRowCellValue(gridView1.GetSelectedRows()[0], "Id").ToString());
                    var model = GlobalHandler.msgSmsConfigtresp.Get(Id);
                    txtSysCode.Text = model.CODE;
                    txtSmsNo.Text = model.DISORDER.ToString();
                    cmbTYPE.EditValue = model.PARAMNAME;
                }
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取行数据出错");
                _log.Error("获取行数据出错，出错提示：" + e.ToString());
            }
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //layoutControl2.Visible = !layoutControl2.Visible;

            var listwhere = list;

            string status = barEditItem13.EditValue.ToString() == "1" ? "发送成功" : "发送失败";
            if (barEditItem13.EditValue == "1")
            {
                listwhere = listwhere.Where(p => p.STATUS.Trim() == "发送成功").ToList();
            }
            else
            {
                listwhere = listwhere.Where(p => p.STATUS.Trim() == "发送失败").ToList();
            }
            if (barEditItem6.EditValue != null)
            {
                //日期类型
                //listwhere = listwhere.Where(p => DateTime.Parse(p.TIME) >= DateTime.Parse(barEditItem6.EditValue.ToString())).ToList();
                //Decimal类型
                listwhere = listwhere.Where(p => p.TIME >= decimal.Parse(DateTime.Parse(barEditItem6.EditValue.ToString()).ToString("yyyyMMddhhmmss"))).ToList();
            }
            if (barEditItem8.EditValue != null) 
            {
                //listwhere = listwhere.Where(p => DateTime.Parse(p.TIME) <= DateTime.Parse(barEditItem8.EditValue.ToString())).ToList();
                listwhere = listwhere.Where(p => p.TIME <= decimal.Parse(DateTime.Parse(barEditItem8.EditValue.ToString()).ToString("yyyyMMddhhmmss"))).ToList();
            }
            gridControl1.DataSource = listwhere;
            barStaticItem3.Caption = listwhere.Count.ToString();
        }
    }
}
