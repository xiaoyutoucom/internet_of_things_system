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
    public partial class TemplateManager : UserControl
    { 
        private bool isEdit;
        BasicMonitorRecord bmmodel = new BasicMonitorRecord();
        public ConfigRecord crModel;
        private List<SmsConfigs> list;
        private ILog _log = LogManager.GetLogger("TemplateManager");
        public TemplateManager()
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
            splashScreenManager1.ShowWaitForm();
            splashScreenManager1.SetWaitFormCaption("请稍后,数据加载中....");     // 标题
            //splashScreenManager1.SetWaitFormDescription("正在更新.....");　　　　　// 信息
            layoutControl2.Visible = false;
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
            txt_INTERVAL.Text = "";
            txt_CHANGEDIFF.Text = "";
            txt_MSGTYPE.Text = "";
            txt_TEMPLATEID.Text = "";
            date_LASTTIME.DateTime = DateTime.Now;
            txt_LASTVALUE.Text = "";
            rdo_ISENABLED.EditValue = "1";
            textEdit2.Text = "";
            bet_stationtype.Tag = "";
            bet_stationtype.Text = "";
            crModel = null;
            bmmodel = null;
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
                //GlobalHandler.msgtempleteresp.Delete(Idd);
                var index = gridView1.GetSelectedRows();
                index.Each(a => GlobalHandler.msgtempleteresp.Delete((int)(gridView1.GetRowCellValue(a, "Id"))));
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
            try
            {
                var aList = GlobalHandler.msgtempleteresp.GetAllList();
                for (int i = 0; i < aList.Count; i++)
                {
                    if (aList[i].ISENABLED == "1")
                    {
                        aList[i].ISENABLED = "是";
                    }
                    else
                    {
                        aList[i].ISENABLED = "否";
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

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //刷新
            GetData();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //取消
            layoutControl2.Visible = !layoutControl2.Visible;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //保存
            try
            {
                //验证必填项
                bool validate = false;
                StringBuilder st = new StringBuilder();
                if (string.IsNullOrEmpty(bet_stationtype.Text))
                {
                    bet_stationtype.Properties.Appearance.BorderColor = Color.Red;
                    validate = true;
                    st.Append("关联监测点不能为空！\n\r");
                }
                else
                {
                    bet_stationtype.Properties.Appearance.BorderColor = Color.White;
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
                    var model = GlobalHandler.msgtempleteresp.Get(Idd);
                    model.INTERVAL = decimal.Parse(txt_INTERVAL.Text==""?"0": txt_INTERVAL.Text);
                    model.CHANGEDIFF = decimal.Parse(txt_CHANGEDIFF.Text == "" ? "0" : txt_CHANGEDIFF.Text);
                    model.MSGTYPE = txt_MSGTYPE.Text == "" ? "0" : txt_MSGTYPE.Text;
                    model.TEMPLATEID = txt_TEMPLATEID.Text;                   
                    model.LASTTIME = date_LASTTIME.DateTime;
                    model.LASTVALUE = decimal.Parse(txt_LASTVALUE.Text == "" ? "0" : txt_LASTVALUE.Text);
                    model.ISENABLED = rdo_ISENABLED.EditValue.ToString();
                    model.EXTENDCODE = textEdit2.Text;
                    //model.CONFIGCODE = bet_stationtype.Tag.ToString();
                    model.ConfigItem = crModel;
                    if (bet_stationtype.Text == "")
                    {
                        XtraMessageBox.Show("关联监测点不能为空");
                        return;
                    }
                    if (bmmodel.Id != 0)
                    {
                        model.Monitor = bmmodel;
                    }
                    GlobalHandler.msgtempleteresp.Update(model);
                    EditData();
                }
                else
                {
                    var model = new SmsConfigs();
                    model.INTERVAL = decimal.Parse(txt_INTERVAL.Text == "" ? "0" : txt_INTERVAL.Text);
                    model.CHANGEDIFF = decimal.Parse(txt_CHANGEDIFF.Text == "" ? "0" : txt_CHANGEDIFF.Text);
                    model.MSGTYPE = txt_MSGTYPE.Text == "" ? "0" : txt_MSGTYPE.Text;
                    model.TEMPLATEID = txt_TEMPLATEID.Text;
                    model.LASTTIME = date_LASTTIME.DateTime;
                    model.LASTVALUE = decimal.Parse(txt_LASTVALUE.Text == "" ? "0" : txt_LASTVALUE.Text);
                    model.ISENABLED = rdo_ISENABLED.EditValue.ToString();
                    model.EXTENDCODE = textEdit2.Text;
                    //model.CONFIGCODE = bet_stationtype.Tag.ToString();
                    model.ConfigItem = crModel;
                    if (bet_stationtype.Text=="")
                    {
                        XtraMessageBox.Show("关联监测点不能为空");
                        return;
                    }
                    model.Monitor = bmmodel;
                    GlobalHandler.msgtempleteresp.Insert(model);
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

        private void layoutControlItem64_Click(object sender, EventArgs e)
        {
            //关联监测点

        }

        private void bet_stationtype_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            MonitorHelp from = new MonitorHelp(bmmodel, crModel);
            from.StartPosition = FormStartPosition.CenterScreen;
            from.ShowDialog();
            bmmodel = from.model;
            crModel = from.crModel;
            if (bmmodel == null||crModel==null) return;
            bet_stationtype.Tag = crModel.CONFIG_CODE;
            bet_stationtype.Text = crModel.CONFIG_DESC;
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
                    var model = GlobalHandler.msgtempleteresp.Get(Id);
                    txt_INTERVAL.Text = model.INTERVAL.ToString();
                    txt_CHANGEDIFF.Text = model.CHANGEDIFF.ToString();
                    txt_MSGTYPE.Text = model.MSGTYPE.ToString();
                    txt_TEMPLATEID.Text = model.TEMPLATEID.ToString();
                    date_LASTTIME.DateTime = model.LASTTIME;
                    txt_LASTVALUE.Text = model.LASTVALUE.ToString();
                    rdo_ISENABLED.EditValue = model.ISENABLED.ToString();
                    textEdit2.Text = model.EXTENDCODE == null ? "" : model.EXTENDCODE.ToString();
                    //if (string.IsNullOrEmpty(model.CONFIGCODE) == false)

                    bmmodel = model.Monitor;
                    crModel = model.ConfigItem;
                    if (model.ConfigItem != null)
                    {
                        var _crmodel = GlobalHandler.configresp.GetAllList(p => p.CONFIG_CODE == model.ConfigItem.CONFIG_CODE).FirstOrDefault();
                        bet_stationtype.Tag = _crmodel.CONFIG_CODE;
                        bet_stationtype.Text = _crmodel.CONFIG_DESC;
                    }
                    else
                    {
                        bet_stationtype.Tag = null;
                        bet_stationtype.Text = "";
                    }
                }
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取行数据出错");
                _log.Error("获取行数据出错，出错提示：" + e.ToString());
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
            //var listwhere = list.Where(a => a.Monitor.BMMC.Contains(sbbm)).ToList();
            var listwhere = list.Where(a => a.ConfigItem.CONFIG_DESC.Contains(sbbm)).ToList();
            gridControl1.DataSource = listwhere;
            barStaticItem3.Caption = listwhere.Count.ToString();
        }
    }
}
