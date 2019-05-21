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
    public partial class TemplateConfig : UserControl
    { 
        private bool isEdit;
        BasicMonitorRecord bmmodel = new BasicMonitorRecord();
        private List<SmsConfigt> list;
        private ILog _log = LogManager.GetLogger("TemplateConfig");
        public TemplateConfig()
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
            splashScreenManager2.ShowWaitForm();
            splashScreenManager2.SetWaitFormCaption("请稍后,数据加载中....");     // 标题
            //splashScreenManager1.SetWaitFormDescription("正在更新.....");　　　　　// 信息
            layoutControl2.Visible = false;
            GetData();
            splashScreenManager2.CloseWaitForm();

            //cmbParam.Properties.Items.Add(new ComModel()
            //{
            //    Text = "全部",
            //    Value = "00"
            //});
            cmbParam.Properties.Items.Add("");
            cmbParam.Properties.Items.Add("MonitorName");
            cmbParam.Properties.Items.Add("TagDesc");
            cmbParam.Properties.Items.Add("Unit");
            cmbParam.Properties.Items.Add("Time");
            cmbParam.Properties.Items.Add("TagValue");

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
            txtCode.Text = "";
            txtDisorder.Text = "";
            cmbParam.EditValue = "";
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
            var aList = GlobalHandler.msgSmsConfigtresp.GetAllList();
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
                if (string.IsNullOrEmpty(txtCode.Text))
                {
                    //txtCode.Properties.Appearance.BorderColor = Color.Red;
                    txtCode.BackColor = Color.Red;
                    validate = true;
                    st.Append("模板编号不能为空！\n\r");
                }
                else
                {
                    txtCode.Properties.Appearance.BorderColor = Color.White;
                    txtCode.BackColor = Color.White;
                }
                if (string.IsNullOrEmpty(cmbParam.EditValue.ToString()))
                {
                    //txtCode.Properties.Appearance.BorderColor = Color.Red;
                    cmbParam.BackColor = Color.Red;
                    validate = true;
                    st.Append("模板编号不能为空！\n\r");
                }
                else
                {
                    cmbParam.Properties.Appearance.BorderColor = Color.White;
                    cmbParam.BackColor = Color.White;
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
                    var model = GlobalHandler.msgSmsConfigtresp.Get(Idd);
                    model.CODE = txtCode.Text.Trim();
                    model.NAME = txtName.Text.Trim();
                    model.PARAMNAME = cmbParam.EditValue.ToString();
                    model.DISORDER = int.Parse(txtDisorder.Text.Trim() == "" ? "0":txtDisorder.Text.Trim());
                    GlobalHandler.msgSmsConfigtresp.Update(model);
                    EditData();
                }
                else
                {
                    var model = new SmsConfigt();
                    model.CODE = txtCode.Text.Trim();
                    model.NAME = txtName.Text.Trim();
                    model.PARAMNAME = cmbParam.EditValue.ToString();
                    model.DISORDER = int.Parse(txtDisorder.Text.Trim() == "" ? "0" : txtDisorder.Text.Trim());
                    GlobalHandler.msgSmsConfigtresp.Insert(model);
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
                    txtCode.Text = model.CODE;
                    txtName.Text = model.NAME;
                    txtDisorder.Text = model.DISORDER.ToString();
                    cmbParam.EditValue = model.PARAMNAME;
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
            if (barEdit_mc.EditValue == null || barEdit_mc.EditValue.ToString() == "")
            {
                XtraMessageBox.Show("请输入查询内容");
                return;
            }            
            var sbbm = barEdit_mc.EditValue.ToString();
            if (string.IsNullOrEmpty(sbbm)) return;
            var listwhere = list.Where(a => a.CODE.Contains(sbbm)).ToList();
            gridControl1.DataSource = listwhere;
            barStaticItem3.Caption = listwhere.Count.ToString();
        }
    }
}
