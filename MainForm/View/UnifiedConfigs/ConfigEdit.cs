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
using System.Web.UI.WebControls;
using log4net;

namespace SmartKylinApp.View.BaseConfig
{
    public partial class ConfigEdit : DevExpress.XtraEditors.XtraForm
    {
        private ILog _log = LogManager.GetLogger("ConfigEdit");
        public ConfigEdit()
        {
            InitializeComponent();
            //禁止右键菜单
            layoutControl1.AllowCustomization = false;
            layoutControl2.AllowCustomization = false;
            gridView1.OptionsMenu.EnableColumnMenu = false;
            tree_mstype.Properties.TreeList.OptionsMenu.EnableColumnMenu = false;
            tree_jcxlx.Properties.TreeList.OptionsMenu.EnableColumnMenu = false;
           
        }
        private string hytype;
        private void ConfigEdit_Load(object sender, EventArgs e)
        {
            BindTree();
            BindReportType();
            BindPermissionType();
            //com_PermissionType.SelectedIndex = 0;
            //com_ReportType.SelectedIndex = 0;
        }
        //数据集绑定
        private void BindPermissionType()
        {
            var list = new List<TreeListModel>();
            list.Add(new TreeListModel() { ID = "1", Name = "公有权限" });
            list.Add(new TreeListModel() { ID = "2", Name = "私有权限" });
            ListItem it = null;
            foreach (TreeListModel item in list)
            {
                it = new ListItem(item.Name, item.ID);
                this.com_PermissionType.Properties.Items.Add(it);
            }
        }
        //数据集绑定
        private void BindReportType()
        {
            var list = new List<TreeListModel>();
            list.Add(new TreeListModel() { ID = "1", Name = "通用报表" });
            list.Add(new TreeListModel() { ID = "2", Name = "定制报表" });
            list.Add(new TreeListModel() { ID = "3", Name = "其他" });
            ListItem it = null;
            foreach (TreeListModel item in list)
            {
                it = new ListItem(item.Name, item.ID);
                this.com_ReportType.Properties.Items.Add(it);
            }
        }
        private void BindTree()
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

        private void tree_mstype_EditValueChanged(object sender, EventArgs e)
        {
            //行业类型选择
            var code = (tree_mstype.GetSelectedDataRow() as TreeListModel)?.ID;
            hytype = code;
            BindControl(code);
            var bmid = code;
            BindAgreementTree(bmid?.ToString());
        }

        private void BindControl(string mstype)
        {
            try { 
            var aList = GlobalHandler.monitorresp.GetAllList(a => a.BMID.Substring(6, 6).StartsWith(mstype)).ToList();
            if (aList.Count < 1)
            {
                var blist = new List<BasicMonitorRecord>();
                BasicMonitorRecord model = new BasicMonitorRecord();
                model.Id = -1;
                model.BMMC = "没有匹配监测点！";
                blist.Add(model);
                gridControl1.DataSource = blist;
                return;
            }
            else
            {
                gridControl1.DataSource = aList;

            }
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取监测点数据出错");
                _log.Error("获取监测点数据出错，出错提示：" + e.ToString());
            }
        }

        private void BindAgreementTree(string mstype)
        {
            try { 
            if (!string.IsNullOrEmpty(mstype))
            {
                var datas2 = GlobalHandler.tagresp.GetAllList().ToList();
                if (datas2 == null) return;
                var list = new List<TreeListModel>();
                var dt4 = datas2.Where(a => a.TAG_KEY.StartsWith(mstype));
                dt4.Each(a => list.Add(new TreeListModel() { ID = a.TAG_KEY, ParentID = "1", Name = a.TAG_NAME }));
                tree_jcxlx.Properties.DataSource = list;
                    if (list.Count < 1) return;
                    tree_jcxlx.EditValue = list.First().ID;
            }
            else
            {
                tree_jcxlx.Properties.DataSource = null;
            }
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取监测项类型数据出错");
                _log.Error("获取监测项类型数据出错，出错提示：" + e.ToString());
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //保存
            try
            {

                int[] rownumber = this.gridView2.GetSelectedRows();//获取选中行号；
                if (rownumber.Length == 0)
                {
                    XtraMessageBox.Show("请勾选监测点！");
                    return;
                }
                if (tree_jcxlx.EditValue.ToString() == "")
                {
                    XtraMessageBox.Show("请选择监测项类型名称！");
                    return;
                }
                var box = new XtraMessageBoxArgs();
                box.Caption = "提示";
                box.Text = "确定要更新吗？";
                box.Buttons = new DialogResult[] { DialogResult.OK, DialogResult.Cancel };
                box.Showing += ShowButton.Box_Showing;
                if (XtraMessageBox.Show(box) != DialogResult.OK)
                {
                    return;
                }
                string BMID, jcxlx;
                for (int i = 0; i < rownumber.Length; i++)
                {
                    BMID = gridView2.GetRowCellValue(rownumber[i], "BMID").ToString();
                    jcxlx = tree_jcxlx.EditValue.ToString();
                    var list = GlobalHandler.configresp.GetAllList(a => a.CONFIG_CODE.StartsWith(BMID + "_" + jcxlx.Substring(jcxlx.Length - 3, 3)));
                    if (list.Count > 0)
                    {
                        for (int j = 0; j < list.Count; j++)
                        {
                            var model = GlobalHandler.configresp.Get(list[j].Id);
                            if (txt_UNITS.Text != "")
                            {
                                model.UNITS = (txt_UNITS.Text);
                            }
                            if (txt_PRECISION.Text != "")
                            {
                                model.PRECISION = double.Parse(txt_PRECISION.Text);
                            }
                            if (txt_ALERTRATE.Text != "")
                            { 
                            model.ALERTRATE = double.Parse(txt_ALERTRATE.Text);
                            }
                            if (rdo_ts.EditValue != null)
                            {
                                model.ISPUSH = rdo_ts.EditValue.ToString();
                            }
                            if (rdo_yj.EditValue != null)
                            {
                                model.ENABLE = rdo_yj.EditValue.ToString();
                            }
                            if (txt_L1_START.Text != "")
                            {
                                model.L1_START = double.Parse(txt_L1_START.Text);
                            }
                            if (txt_L1_END.Text != "")
                            {
                                model.L1_END = double.Parse( txt_L1_END.Text);
                            }
                            if (txt_L1_RETURNVALUE.Text != "")
                            {
                                model.L1_RETURNVALUE = double.Parse(txt_L1_RETURNVALUE.Text);
                            }
                            if (txt_L1_COLOR_VALUE.Text != "")
                            {
                                model.L1_COLOR_VALUE = txt_L1_COLOR_VALUE.Text;
                            }
                            if (txt_L2_START.Text != "")
                            {
                                model.L2_START = double.Parse( txt_L2_START.Text);
                            }
                            if (txt_L2_END.Text != "")
                            {
                                model.L2_END = double.Parse(txt_L2_END.Text);
                            }
                            if (txt_L2_RETURNVALUE.Text != "")
                            {
                                model.L2_RETURNVALUE = double.Parse(txt_L2_RETURNVALUE.Text);
                            }
                            if (txt_L2_COLOR_VALUE.Text != "")
                            {
                                model.L2_COLOR_VALUE = txt_L2_COLOR_VALUE.Text;
                            }
                            if (txt_L3_START.Text != "")
                            {
                                model.L3_START = double.Parse(txt_L3_START.Text);
                            }
                            if (txt_L3_END.Text != "")
                            {
                                model.L3_END = double.Parse( txt_L3_END.Text);
                            }
                            if (txt_L3_RETURNVALUE.Text != "")
                            {
                                model.L3_RETURNVALUE = double.Parse(txt_L3_RETURNVALUE.Text);
                            }
                            if (txt_L3_COLOR_VALUE.Text != "")
                            {
                                model.L3_COLOR_VALUE = txt_L3_COLOR_VALUE.Text;
                            }
                            if(com_PermissionType.EditValue != null)
                            {
                                model.PermissionType = (com_PermissionType.SelectedItem as ListItem).Value.Trim();
                            }
                            if (com_ReportType.EditValue!= null)
                            {
                                
                                model.ReportType = (com_ReportType.SelectedItem as ListItem).Value.Trim();
                            }
                            GlobalHandler.configresp.Update(model);
                        }

                    }
                }
                XtraMessageBox.Show("保存成功");
                this.Close();
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show("数据保存失败");
                _log.Error("数据保存失败，出错提示：" + e.ToString());
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}