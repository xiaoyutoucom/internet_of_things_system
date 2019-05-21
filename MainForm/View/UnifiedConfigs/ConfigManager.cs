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
using SmartKylinData.IOTModel;
using SmartKylinApp.Common;
using FluentNHibernate.Utils;
using FluentNHibernate.Conventions;
using DevExpress.XtraLayout.Utils;
using SmartKylinApp.View.BaseConfig;
using DevExpress.XtraTreeList.Nodes;
using System.Web.UI.WebControls;
using log4net;

namespace SmartKylinApp.View.UnifiedConfigs
{
    public partial class ConfigManager : DevExpress.XtraEditors.XtraUserControl
    {
        private ILog _log = LogManager.GetLogger("ConfigManager");
        public ConfigManager()
        {
            InitializeComponent();
            //禁止右键菜单
            layoutControl1.AllowCustomization = false;
            layoutControl2.AllowCustomization = false;
            layoutControl3.AllowCustomization = false;
            bar2.Manager.AllowShowToolbarsPopup = false;
            bar2.OptionsBar.AllowQuickCustomization = false;
            gridView1.OptionsMenu.EnableColumnMenu = false;
            gridView2.OptionsMenu.EnableColumnMenu = false;
            gridView3.OptionsMenu.EnableColumnMenu = false;
            tree_mstype.Properties.TreeList.OptionsMenu.EnableColumnMenu = false;
            tree_jcxlx.Properties.TreeList.OptionsMenu.EnableColumnMenu = false;
            treeList1.OptionsMenu.EnableColumnMenu = false;
        }
        private bool ifscbm = true;
        private string getdata;
        private string hytype;
        private SensorRecord srmodel;
        private List<ConfigRecord> tagList;
        private void ConfigManager_Load(object sender, EventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            splashScreenManager1.SetWaitFormCaption("请稍后,数据加载中....");     // 标题
            //splashScreenManager1.SetWaitFormDescription("正在更新.....");　　　　　// 信息
            layoutControl3.Visible = false;
            BindTree();
            //GetData();
            //BindAgreementTree();
            //初始化监测点选择条件
            //var blist = new List<BasicMonitorRecord>();
            //BasicMonitorRecord model = new BasicMonitorRecord();
            //model.BMMC = "请选择过滤条件！";
            //model.Id = -1;
            //blist.Add(model);
            //gridControl2.DataSource = blist;
            BindReportType();
            BindPermissionType();
            com_PermissionType.SelectedIndex = 0;
            com_ReportType.SelectedIndex = 0;
            splashScreenManager1.CloseWaitForm();
        }
        //数据集绑定
        private void BindPermissionType()
        {
            listPermissionType = new List<TreeListModel>();
            listPermissionType.Add(new TreeListModel() { ID = "1", Name = "公有权限" });
            listPermissionType.Add(new TreeListModel() { ID = "2", Name = "私有权限" });
            ListItem it = null;
            foreach (TreeListModel item in listPermissionType)
            {
                it = new ListItem(item.Name, item.ID);
                this.com_PermissionType.Properties.Items.Add(it);
            }
        }
        //数据集绑定
        private void BindReportType()
        {
            listReportType = new List<TreeListModel>();
            listReportType.Add(new TreeListModel() { ID = "1", Name = "通用报表" });
            listReportType.Add(new TreeListModel() { ID = "2", Name = "定制报表" });
            listReportType.Add(new TreeListModel() { ID = "3", Name = "其他" });
            ListItem it = null;
            foreach (TreeListModel item in listReportType)
            {
                it = new ListItem(item.Name, item.ID);
                this.com_ReportType.Properties.Items.Add(it);
            }
        }
        private void BindAgreementTree(string mstype)
        {
            try { 
            if (layoutControl3.Visible == false)
            {
                return;
            }
            if (!string.IsNullOrEmpty(mstype))
            {
                var datas2 = GlobalHandler.tagresp.GetAllList().ToList();
                if (datas2 == null) return;
                var list = new List<TreeListModel>();
                var dt4 = datas2.Where(a => a.TAG_KEY.StartsWith(mstype.Substring(6, 6)));
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
        private void BindControl(string mstype)
        {
            try { 
            aList = GlobalHandler.monitorresp.GetAllList(a => a.BMID.Substring(6, 6).StartsWith(mstype)).ToList();
            if (aList.Count < 1)
            {
                BasicMonitorRecord model = new BasicMonitorRecord();
                model.Id = -1;
                model.BMMC = "没有匹配监测点！";
                aList.Add(model);
                gridControl2.DataSource = aList;
                return;
            }
            else
            {
                gridControl2.DataSource = aList;

            }
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取监测点数据出错");
                _log.Error("获取监测点数据出错，出错提示：" + e.ToString());
            }
        }
        private void GetData(string mstype)
        {
            try { 
            if (!(string.IsNullOrEmpty(mstype)|| mstype=="-1"))
            {
                var aList = GlobalHandler.configresp.GetAllList(a => a.CONFIG_CODE.StartsWith(mstype)).ToList();
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
        private void BindTree()
        {
            try { 
            var datas = GlobalHandler.mstyperesp.GetAllList();
            if (datas == null) return;
            var list = new List<TreeListModel>();
            var dt1 = datas.Where(a => a.TYPE_KEY.ToString().Length == 2);
            dt1.Each(a => list.Add(new TreeListModel() { ID = a.TYPE_KEY, ParentID = "1", Name = a.TYPE_NAME}));
            var dt2 = datas.Where(a => a.TYPE_KEY.ToString().Length == 4);
            dt2.Each(a => list.Add(new TreeListModel() { ID = a.TYPE_KEY, ParentID = a.TYPE_KEY.Substring(0, 2), Name = a.TYPE_NAME }));
            var dt3 = datas.Where(a => a.TYPE_KEY.ToString().Length == 6);
            dt3.Each(a => list.Add(new TreeListModel() { ID = a.TYPE_KEY, ParentID = a.TYPE_KEY.Substring(0, 4), Name = a.TYPE_NAME }));
            tree_mstype.Properties.DataSource = list;
            tree_mstype.Properties.DisplayMember = "Name";
            tree_mstype.Properties.ValueMember = "ID";
            tree_mstype.Properties.TreeList.CollapseAll();
            tree_mstype.EditValue = "03";
                //foreach (TreeListNode node in tree_mstype.Properties.TreeList.Nodes)
                //{
                //    if(node.Level==0)
                //    node.Expanded = false;
                //}
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取行业类型数据出错");
                _log.Error("获取行业类型数据出错，出错提示：" + e.ToString());
            }
        }
        private void GetData()
        {
            try { 
            var list = GlobalHandler.configresp.GetAllList();
            tagList = list;
            gridControl1.DataSource = list;
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取数据出错");
                _log.Error("获取数据出错，出错提示：" + e.ToString());
            }
        }

        //获取数据
        private void GetData(IReadOnlyCollection<string> mstype)
        {

            if (mstype == null) return;
            var list = new List<ConfigRecord>();
            foreach (var item in mstype)
            {
                if (item.Length < 6) continue;
                var aaList = tagList.Where(a => a.CONFIG_CODE.Substring(6, 6).StartsWith(item)).ToList();
                list.AddRange(aaList);
            }
            gridControl1.DataSource = list;
        }



        private void gridControl2_Click(object sender, EventArgs e)
        {
            var Id = int.Parse(gridView2.GetRowCellValue(gridView2.GetSelectedRows()[0], "Id").ToString());
            if (Id == -1) return;
            var model = GlobalHandler.monitorresp.Get(Id);
            getdata = model.BMID;
            RefreshDate();
            EditData();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //隐藏左边面板
            layoutControl1.Visible = !layoutControl1.Visible;
        }
        private bool isEdit = false;
        private object bmid;
        private List<TreeListModel> listPermissionType;
        private List<TreeListModel> listReportType;
        private List<BasicMonitorRecord> aList;

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //添加
           
            var id = int.Parse(gridView2.GetRowCellValue(gridView2.GetSelectedRows()[0], "Id").ToString());
            if (id==-1)
            {
                XtraMessageBox.Show("请选择监测点！");
                return;
            }
            layoutControlGroup3.Visibility = LayoutVisibility.Always;
            layoutControl3.Controls.Remove(layoutControl3.Controls["import"]);
            layoutControl3.Visible = !layoutControl3.Visible;
            isEdit = false;
            CleanData();
            if (layoutControl1.Visible)
            {
                bmid = gridView2.GetRowCellValue(gridView2.GetSelectedRows()[0], "BMID");
                BindAgreementTree(bmid?.ToString());
            }
        }

        private void CleanData()
        {
            //清空添加的数据
            tree_jcxlx.EditValue = "";
            txt_bmid.Text = "";
            txt_bmms.Text = "";
            txt_blms.Text = "";
            txt_bmx.Text = "";
            txt_bmy.Text = "";
            txt_MAX_VALUE.Text = "";
            txt_MIN_VALUE.Text = "";
            txt_MAX_MAX_VALUE.Text = "";
            txt_MIN_MIN_VALUE.Text = "";
            txt_UNITS.Text = "";
            txt_ORDER_NUM.Text = "";
            bet_cgqbm.Tag = "";
            bet_cgqbm.Text = "";
            txt_NBSS.Text = "";
            txt_CGROUP.Text = "";
            txt_TEMPLATE.Text = "";
            txt_COLOR_VALUE.Text = "";
            txt_PRECISION.Text = "";
            txt_ALERTRATE.Text = "";
            rdo_yj.EditValue = "1";
            rdo_ts.EditValue = "0";
            txt_L1_START.Text = "";
            txt_L1_END.Text = "";
            txt_L1_RETURNVALUE.Text = "";
            txt_L1_COLOR_VALUE.Text = "";
            txt_L2_START.Text = "";
            txt_L2_END.Text = "";
            txt_L2_RETURNVALUE.Text = "";
            txt_L2_COLOR_VALUE.Text = "";
            txt_L3_START.Text = "";
            txt_L3_END.Text = "";
            txt_L3_RETURNVALUE.Text = "";
            txt_L3_COLOR_VALUE.Text = "";
            //txt_MIN_IDEALVALUE.Text = "";
            //txt_MAX_IDEALVALUE.Text = "";
            rdo_xjyj.EditValue = "0";
            txt_L1_STARTDOWN.Text = "";
            txt_L1_ENDDOWN.Text = "";
            txt_L1_DOWNRETURNVALUE.Text = "";
            txt_L1_COLOR_VALUEDOWN.Text = "";
            txt_L2_STARTDOWN.Text = "";
            txt_L2_ENDDOWN.Text = "";
            txt_L2_DOWNRETURNVALUE.Text = "";
            txt_L2_COLOR_VALUEDOWN.Text = "";
            txt_L3_STARTDOWN.Text = "";
            txt_L3_ENDDOWN.Text = "";
            txt_L3_DOWNRETURNVALUE.Text = "";
            txt_L3_COLOR_VALUEDOWN.Text = "";
            txt_REMARK.Text = "";
            com_PermissionType.SelectedIndex=0;
            com_ReportType.SelectedIndex=0;
            //com_PermissionType.Tag =null;
            //com_ReportType.Tag=null;
            //com_PermissionType.Text = null;
            //com_ReportType.Text = null;
            EXTENDCODE2.Text = "";
            EXTENDCODE3.Text = "";
            EXTENDCODE4.Text = "";
            EXTENDCODE5.Text = "";
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //编辑
            var id = int.Parse(gridView2.GetRowCellValue(gridView2.GetSelectedRows()[0], "Id").ToString());
            if (id == -1)
            {
                XtraMessageBox.Show("请选择分组！");
                return;
            }
            layoutControlGroup3.Visibility = LayoutVisibility.Always;
            layoutControl3.Controls.Remove(layoutControl3.Controls["import"]);
            layoutControl3.Visible = !layoutControl3.Visible;
            isEdit = true;

            if (layoutControl1.Visible)
            {
                bmid = gridView2.GetRowCellValue(gridView2.GetSelectedRows()[0], "BMID");
                BindAgreementTree(bmid?.ToString());
            }
            EditData();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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
                //var Id = int.Parse(gridView1.GetRowCellValue(gridView1.GetSelectedRows()[0], "Id").ToString());
                //GlobalHandler.configresp.Delete(Id);
                var index = gridView1.GetSelectedRows();
                index.Each(a => GlobalHandler.configresp.Delete((int)(gridView1.GetRowCellValue(a, "Id"))));
                //XtraMessageBox.Show("删除成功");
                RefreshDate();

            }
            catch (Exception exception)
            {
                XtraMessageBox.Show("删除失败");
                _log.Error("删除失败，出错提示：" + e.ToString());
            }
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            RefreshDate();
        }

        private void RefreshDate()
        {
                bmid=gridView2.GetRowCellValue(gridView2.GetSelectedRows()[0], "BMID");
                GetData(bmid?.ToString());
                BindAgreementTree(bmid?.ToString());
        }

        private void tree_mstype_EditValueChanged(object sender, EventArgs e)
        {
            //行业类型选择
            var code = (tree_mstype.GetSelectedDataRow() as TreeListModel)?.ID;
            hytype = code;
            BindControl(code);
            RefreshDate();
            EditData();
            //GenerateNumber();
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {
            EditData();
        }

        private void EditData()
        {
            try { 
            //点击选择
            if (layoutControl1.Visible == false)
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
                var Id = int.Parse(gridView1.GetRowCellValue(gridView1.GetSelectedRows()[0], "Id").ToString());
                var model = GlobalHandler.configresp.Get(Id);
                ifscbm = false;
                var jcxlx = model.CONFIG_CODE.Substring(6, 6) + model.CONFIG_CODE.Substring(19, 4);
                tree_jcxlx.EditValue = jcxlx;
                ifscbm = true;
                txt_bmid.Text = model.CONFIG_CODE;
                txt_bmms.Text = model.CONFIG_DESC;
                txt_blms.Text = model.VARIABLE_NAME;
                txt_bmx.Text = model.PAGE_X.ToString();
                txt_bmy.Text = model.PAGE_Y.ToString();
                txt_MAX_VALUE.Text = model.MAX_VALUE.ToString();
                txt_MIN_VALUE.Text = model.MIN_VALUE.ToString();
                txt_MAX_MAX_VALUE.Text = model.MAX_MAX_VALUE.ToString();
                txt_MIN_MIN_VALUE.Text = model.MIN_VALUE.ToString(); txt_UNITS.Text = model.UNITS;
                txt_ORDER_NUM.Text = model.ORDER_NUM.ToString();
                srmodel = model.SENSORID;
                bet_cgqbm.Tag = model.SENSORID?.CGQBM;
                bet_cgqbm.Text = model.SENSORID?.CGQMC;
                txt_NBSS.Text = model.NBSS;
                txt_CGROUP.Text = model.CGROUP;
                txt_TEMPLATE.Text = model.TEMPLATE;
                txt_COLOR_VALUE.Text = model.COLOR_VALUE;
                txt_PRECISION.Text = model.PRECISION.ToString();
                txt_ALERTRATE.Text = model.ALERTRATE.ToString();
                rdo_yj.EditValue = model.ENABLE;
                txt_L1_START.Text = model.L1_START.ToString();
                txt_L1_END.Text = model.L1_END.ToString();
                txt_L1_RETURNVALUE.Text = model.L1_RETURNVALUE.ToString();
                txt_L1_COLOR_VALUE.Text = model.L1_COLOR_VALUE;
                txt_L2_START.Text = model.L2_START.ToString();
                txt_L2_END.Text = model.L2_END.ToString();
                txt_L2_RETURNVALUE.Text = model.L2_RETURNVALUE.ToString();
                txt_L2_COLOR_VALUE.Text = model.L2_COLOR_VALUE;
                txt_L3_START.Text = model.L3_START.ToString();
                txt_L3_END.Text = model.L3_END.ToString();
                txt_L3_RETURNVALUE.Text = model.L3_RETURNVALUE.ToString();
                txt_L3_COLOR_VALUE.Text = model.L3_COLOR_VALUE;
                //txt_MIN_IDEALVALUE.Text = model.MIN_IDEALVALUE.ToString();
                //txt_MAX_IDEALVALUE.Text = model.MAX_IDEALVALUE.ToString();
                rdo_xjyj.EditValue = model.ENABLEDOWN;
                txt_L1_STARTDOWN.Text = model.L1_STARTDOWN.ToString();
                txt_L1_ENDDOWN.Text = model.L1_ENDDOWN.ToString();
                txt_L1_DOWNRETURNVALUE.Text = model.L1_DOWNRETURNVALUE.ToString();
                txt_L1_COLOR_VALUEDOWN.Text = model.L1_COLOR_VALUEDOWN;
                txt_L2_STARTDOWN.Text = model.L2_STARTDOWN.ToString();
                txt_L2_ENDDOWN.Text = model.L2_ENDDOWN.ToString();
                txt_L2_DOWNRETURNVALUE.Text = model.L2_DOWNRETURNVALUE.ToString();
                txt_L2_COLOR_VALUEDOWN.Text = model.L2_COLOR_VALUEDOWN;
                txt_L3_STARTDOWN.Text = model.L3_STARTDOWN.ToString();
                txt_L3_ENDDOWN.Text = model.L3_ENDDOWN.ToString();
                txt_L3_DOWNRETURNVALUE.Text = model.L3_DOWNRETURNVALUE.ToString();
                txt_L3_COLOR_VALUEDOWN.Text = model.L3_COLOR_VALUEDOWN;
                txt_REMARK.Text = model.REMARK;
                rdo_ts.EditValue= model.ISPUSH == null ? "0" : model.ISPUSH;
                    for (int i=0;i < listPermissionType.Count;i++)
                    {
                        if (listPermissionType[i].ID.Equals ( model.PermissionType == null ? "1" : model.PermissionType))
                        {
                            
                            com_PermissionType.SelectedIndex = i;
                        }
                    }
                    
                    for (int i = 0; i < listReportType.Count; i++)
                    {
                        if (listReportType[i].ID.Equals( model.ReportType == null ? "1" : model.ReportType))
                        {

                            com_ReportType.SelectedIndex = i;
                        }
                    }
                    EXTENDCODE2.Text = model.EXTENDCODE2;
                    EXTENDCODE3.Text = model.EXTENDCODE3;
                    EXTENDCODE4.Text = model.EXTENDCODE4;
                    EXTENDCODE5.Text = model.EXTENDCODE5;
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

        private void tree_jcxlx_EditValueChanged(object sender, EventArgs e)
        {

            if (tree_jcxlx.EditValue != null && tree_jcxlx.EditValue.ToString() != "" && ifscbm)
            {
                if (tree_jcxlx.EditValue.ToString().Length < 7 )
                {
                    XtraMessageBox.Show("只能选择监测项，请重新选择！");
                    tree_jcxlx.EditValue = "";
                    txt_bmid.Text = "";
                }
                else 
                {
                    GenerateNumber();//生成编号
                    GetPretermission();//获取监测类型默认数据
                }
            }
        }
        private void GetPretermission()
        {
            try { 
            TagInfoRecord model = GlobalHandler.tagresp.FirstOrDefault(a=>a.TAG_KEY == tree_jcxlx.EditValue.ToString());
            txt_MAX_VALUE.Text = model.NORMAL_END.ToString();
            txt_MIN_VALUE.Text = model.NORMAL_START.ToString();
            txt_COLOR_VALUE.Text = model.COLOR_VALUE;
            txt_COLOR_VALUE.Text = model.COLOR_VALUE;
            txt_L1_START.Text = model.L1_START.ToString();
            txt_L1_END.Text = model.L1_END.ToString();
            txt_L1_COLOR_VALUE.Text = model.L1_COLOR_VALUE;
            txt_L2_START.Text = model.L2_START.ToString();
            txt_L2_END.Text = model.L2_END.ToString();
            txt_L2_COLOR_VALUE.Text = model.L2_COLOR_VALUE;
            txt_L3_START.Text = model.L3_START.ToString();
            txt_L3_END.Text = model.L3_END.ToString();
            txt_L3_COLOR_VALUE.Text = model.L3_COLOR_VALUE;

            txt_UNITS.Text = model.UNITS;
            txt_ALERTRATE.Text = model.ALERTRATE.ToString();
            txt_PRECISION.Text = model.PRECISION.ToString();
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("初始化数据出错");
                _log.Error("初始化数据出错，出错提示：" + e.ToString());
            }
        }
        private void GenerateNumber()
        {
            try { 
            //生成编号
            if (gridView2.GetRowCellValue(gridView2.GetSelectedRows()[0], "BMID")!=null) { 
            decimal num = 1;
            var jcdbh = gridView2.GetRowCellValue(gridView2.GetSelectedRows()[0], "BMID").ToString();

            string jcxlx = tree_jcxlx.EditValue.ToString();
           //string nub = num.ToString().PadLeft(7, '0');
            string bh = jcdbh + "_" + jcxlx.Substring(jcxlx.Length - 3, 3);
            var count = GlobalHandler.configresp.Count(s => s.CONFIG_CODE.StartsWith(bh));
            //var count = GlobalHandler.monitorresp.GetAllList().Where(s => s.BMMC == bh).ToList().Count;
            if (count > 0)
            {               
                var str = (long.Parse(GlobalHandler.configresp.GetAllList(s => s.CONFIG_CODE.StartsWith(bh))
                                         .OrderByDescending(q => q.CONFIG_CODE)
                                         .FirstOrDefault().CONFIG_CODE.Split('_')[2]
                                         ) + 1).ToString();
                bh = bh+ "_"+str;
            }
            else
            {
                bh = bh +"_"+ num.ToString();
            }
            txt_bmid.Text = bh;
            }
            }
            catch (Exception e)
            {
                _log.Error("生成编号出错，出错提示：" + e.ToString());
                XtraMessageBox.Show("生成编号出错");
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //保存
            try
            {
                //验证必填项
                bool validate = false;
                StringBuilder st = new StringBuilder();
                if (string.IsNullOrEmpty(txt_bmid.Text))
                {
                    txt_bmid.Properties.Appearance.BorderColor = Color.Red;
                    validate = true;
                    st.Append("监测项编码不能为空！\n\r");
                }
                else
                {
                    txt_bmid.Properties.Appearance.BorderColor = Color.White;
                }
                if (string.IsNullOrEmpty(txt_bmms.Text))
                {
                    txt_bmms.Properties.Appearance.BorderColor = Color.Red;
                    validate = true;
                    st.Append("监测项描述不能为空！\n\r");
                }
                else
                {
                    txt_bmms.Properties.Appearance.BorderColor = Color.White;
                }
                //if (string.IsNullOrEmpty(bet_cgqbm.Text))
                //{
                //    bet_cgqbm.Properties.Appearance.BorderColor = Color.Red;
                //    validate = true;
                //    st.Append("传感器编码不能为空！\n\r");
                //}
                //else
                //{
                //    bet_cgqbm.Properties.Appearance.BorderColor = Color.White;
                //}              
                if (validate)
                {
                    XtraMessageBox.Show(st.ToString());
                    return;
                }
                if (isEdit)
                {
                    
                    if (gridView1.GetSelectedRows().Length == 0) return;
                    var Id = int.Parse(gridView1.GetRowCellValue(gridView1.GetSelectedRows()[0], "Id").ToString());
                    var model = GlobalHandler.configresp.Get(Id);
                    var Idd = int.Parse(gridView2.GetRowCellValue(gridView2.GetSelectedRows()[0], "Id").ToString());
                    BasicMonitorRecord bmrmodel = GlobalHandler.monitorresp.Get(Idd);
                    model.STATIONID = bmrmodel;
                    var da = GlobalHandler.tagresp.FirstOrDefault(a => a.TAG_KEY == tree_jcxlx.EditValue.ToString());
                    model.TAGID = da;

                    model.CONFIG_CODE = txt_bmid.Text;
                    model.CONFIG_DESC = txt_bmms.Text;
                    model.VARIABLE_NAME = txt_blms.Text;
                    model.PAGE_X = decimal.Parse(txt_bmx.Text == "" ? "0" : txt_bmx.Text);
                    model.PAGE_Y = double.Parse(txt_bmy.Text == "" ? "0" : txt_bmy.Text);
                    model.MAX_VALUE = double.Parse(txt_MAX_VALUE.Text == "" ? "0" : txt_MAX_VALUE.Text);
                    model.MIN_VALUE = double.Parse(txt_MIN_VALUE.Text == "" ? "0" : txt_MIN_VALUE.Text); 
                    model.MAX_MAX_VALUE = double.Parse(txt_MAX_MAX_VALUE.Text == "" ? "0" : txt_MAX_MAX_VALUE.Text); 
                    model.MIN_MIN_VALUE = double.Parse(txt_MIN_MIN_VALUE.Text == "" ? "0" : txt_MIN_MIN_VALUE.Text); 
                    model.UNITS = txt_UNITS.Text;
                    model.ORDER_NUM = int.Parse(txt_ORDER_NUM.Text == "" ? "0" : txt_ORDER_NUM.Text); 
                    model.SENSORID = srmodel;//传感器编码
                    model.NBSS = txt_NBSS.Text;
                    model.CGROUP = txt_CGROUP.Text;
                    model.TEMPLATE = txt_TEMPLATE.Text;
                    model.COLOR_VALUE = txt_COLOR_VALUE.Text;
                    model.PRECISION = double.Parse(txt_PRECISION.Text == "" ? "0" : txt_PRECISION.Text);
                    model.ALERTRATE = double.Parse(txt_ALERTRATE.Text == "" ? "0" : txt_ALERTRATE.Text);
                    model.ENABLE = rdo_yj.EditValue.ToString();
                    model.L1_START = double.Parse(txt_L1_START.Text == "" ? "0" : txt_L1_START.Text);
                    model.L1_END = double.Parse(txt_L1_END.Text == "" ? "0" : txt_L1_END.Text);
                    model.L1_RETURNVALUE = double.Parse(txt_L1_RETURNVALUE.Text == "" ? "0" : txt_L1_RETURNVALUE.Text);
                    model.L1_COLOR_VALUE = txt_L1_COLOR_VALUE.Text;
                    model.L2_START = double.Parse(txt_L2_START.Text == "" ? "0" : txt_L2_START.Text);
                    model.L2_END = double.Parse(txt_L2_END.Text == "" ? "0" : txt_L2_END.Text);
                    model.L2_RETURNVALUE = double.Parse(txt_L2_RETURNVALUE.Text == "" ? "0" : txt_L2_RETURNVALUE.Text);
                    model.L2_COLOR_VALUE = txt_L2_COLOR_VALUE.Text;
                    model.L3_START = double.Parse(txt_L3_START.Text == "" ? "0" : txt_L3_START.Text);
                    model.L3_END = double.Parse(txt_L3_END.Text == "" ? "0" : txt_L3_END.Text);
                    model.L3_RETURNVALUE = double.Parse(txt_L3_RETURNVALUE.Text == "" ? "0" : txt_L3_RETURNVALUE.Text);
                    model.L3_COLOR_VALUE = txt_L3_COLOR_VALUE.Text;
                    //model.MIN_IDEALVALUE = double.Parse(txt_MIN_IDEALVALUE.Text == "" ? "0" : txt_MIN_IDEALVALUE.Text);
                    //model.MAX_IDEALVALUE = double.Parse(txt_MAX_IDEALVALUE.Text == "" ? "0" : txt_MAX_IDEALVALUE.Text);
                    model.ENABLEDOWN = rdo_xjyj.EditValue.ToString();
                    model.L1_STARTDOWN = double.Parse(txt_L1_STARTDOWN.Text == "" ? "0" : txt_L1_STARTDOWN.Text);
                    model.L1_ENDDOWN = double.Parse(txt_L1_ENDDOWN.Text == "" ? "0" : txt_L1_ENDDOWN.Text);
                    model.L1_DOWNRETURNVALUE = double.Parse(txt_L1_DOWNRETURNVALUE.Text == "" ? "0" : txt_L1_DOWNRETURNVALUE.Text);
                    model.L1_COLOR_VALUEDOWN = txt_L1_COLOR_VALUEDOWN.Text;
                    model.L2_STARTDOWN = double.Parse(txt_L2_STARTDOWN.Text == "" ? "0" : txt_L2_STARTDOWN.Text);
                    model.L2_ENDDOWN = double.Parse(txt_L2_ENDDOWN.Text == "" ? "0" : txt_L2_ENDDOWN.Text);
                    model.L2_DOWNRETURNVALUE = double.Parse(txt_L2_DOWNRETURNVALUE.Text == "" ? "0" : txt_L2_DOWNRETURNVALUE.Text);
                    model.L2_COLOR_VALUEDOWN = txt_L2_COLOR_VALUEDOWN.Text;
                    model.L3_STARTDOWN = double.Parse(txt_L3_STARTDOWN.Text == "" ? "0" : txt_L3_STARTDOWN.Text);
                    model.L3_ENDDOWN = double.Parse(txt_L3_ENDDOWN.Text == "" ? "0" : txt_L3_ENDDOWN.Text);
                    model.L3_DOWNRETURNVALUE = double.Parse(txt_L3_DOWNRETURNVALUE.Text == "" ? "0" : txt_L3_DOWNRETURNVALUE.Text);
                    model.L3_COLOR_VALUEDOWN = txt_L3_COLOR_VALUEDOWN.Text;
                    model.REMARK = txt_REMARK.Text;
                    model.ISPUSH = rdo_ts.EditValue.ToString();
                    model.PermissionType = (com_PermissionType.SelectedItem as ListItem).Value.Trim();
                    model.ReportType = (com_ReportType.SelectedItem as ListItem).Value.Trim();
                   // model.ADDTIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    model.EXTENDCODE2 = EXTENDCODE2.Text;
                    model.EXTENDCODE3 = EXTENDCODE3.Text;
                    model.EXTENDCODE4 = EXTENDCODE4.Text;
                    model.EXTENDCODE5 = EXTENDCODE5.Text;
                    GlobalHandler.configresp.Update(model);
                    EditData();
                }
                else
                {
                   

                    var model = new ConfigRecord();
                    var Idd = int.Parse(gridView2.GetRowCellValue(gridView2.GetSelectedRows()[0], "Id").ToString());
                    BasicMonitorRecord bmrmodel = GlobalHandler.monitorresp.Get(Idd);
                    model.STATIONID = bmrmodel;
                    var da = GlobalHandler.tagresp.FirstOrDefault(a => a.TAG_KEY == tree_jcxlx.EditValue.ToString());
                    model.TAGID = da;
                    //model.SENSORID = srmodel;//传感器编码

                    model.CONFIG_CODE = txt_bmid.Text;
                    model.CONFIG_DESC = txt_bmms.Text;
                    model.VARIABLE_NAME = txt_blms.Text;
                    model.PAGE_X = decimal.Parse(txt_bmx.Text == "" ? "0" : txt_bmx.Text);
                    model.PAGE_Y = double.Parse(txt_bmy.Text == "" ? "0" : txt_bmy.Text);
                    model.MAX_VALUE = double.Parse(txt_MAX_VALUE.Text == "" ? "0" : txt_MAX_VALUE.Text);
                    model.MIN_VALUE = double.Parse(txt_MIN_VALUE.Text == "" ? "0" : txt_MIN_VALUE.Text);
                    model.MAX_MAX_VALUE = double.Parse(txt_MAX_MAX_VALUE.Text == "" ? "0" : txt_MAX_MAX_VALUE.Text);
                    model.MIN_VALUE = double.Parse(txt_MIN_MIN_VALUE.Text == "" ? "0" : txt_MIN_MIN_VALUE.Text);
                    model.UNITS = txt_UNITS.Text;
                    model.ORDER_NUM = int.Parse(txt_ORDER_NUM.Text == "" ? "0" : txt_ORDER_NUM.Text);
                    model.SENSORID = srmodel;//传感器编码
                    model.NBSS = txt_NBSS.Text;
                    model.CGROUP = txt_CGROUP.Text;
                    model.TEMPLATE = txt_TEMPLATE.Text;
                    model.COLOR_VALUE = txt_COLOR_VALUE.Text;
                    model.PRECISION = double.Parse(txt_PRECISION.Text == "" ? "0" : txt_PRECISION.Text);
                    model.ALERTRATE = double.Parse(txt_ALERTRATE.Text == "" ? "0" : txt_ALERTRATE.Text);
                    model.ENABLE = rdo_yj.EditValue.ToString();
                    model.L1_START = double.Parse(txt_L1_START.Text == "" ? "0" : txt_L1_START.Text);
                    model.L1_END = double.Parse(txt_L1_END.Text == "" ? "0" : txt_L1_END.Text);
                    model.L1_RETURNVALUE = double.Parse(txt_L1_RETURNVALUE.Text == "" ? "0" : txt_L1_RETURNVALUE.Text);
                    model.L1_COLOR_VALUE = txt_L1_COLOR_VALUE.Text;
                    model.L2_START = double.Parse(txt_L2_START.Text == "" ? "0" : txt_L2_START.Text);
                    model.L2_END = double.Parse(txt_L2_END.Text == "" ? "0" : txt_L2_END.Text);
                    model.L2_RETURNVALUE = double.Parse(txt_L2_RETURNVALUE.Text == "" ? "0" : txt_L2_RETURNVALUE.Text);
                    model.L2_COLOR_VALUE = txt_L2_COLOR_VALUE.Text;
                    model.L3_START = double.Parse(txt_L3_START.Text == "" ? "0" : txt_L3_START.Text);
                    model.L3_END = double.Parse(txt_L3_END.Text == "" ? "0" : txt_L3_END.Text);
                    model.L3_RETURNVALUE = double.Parse(txt_L3_RETURNVALUE.Text == "" ? "0" : txt_L3_RETURNVALUE.Text);
                    model.L3_COLOR_VALUE = txt_L3_COLOR_VALUE.Text;
                    //model.MIN_IDEALVALUE = double.Parse(txt_MIN_IDEALVALUE.Text == "" ? "0" : txt_MIN_IDEALVALUE.Text);
                    //model.MAX_IDEALVALUE = double.Parse(txt_MAX_IDEALVALUE.Text == "" ? "0" : txt_MAX_IDEALVALUE.Text);
                    model.ENABLEDOWN = rdo_xjyj.EditValue.ToString();
                    model.L1_STARTDOWN = double.Parse(txt_L1_STARTDOWN.Text == "" ? "0" : txt_L1_STARTDOWN.Text);
                    model.L1_ENDDOWN = double.Parse(txt_L1_ENDDOWN.Text == "" ? "0" : txt_L1_ENDDOWN.Text);
                    model.L1_DOWNRETURNVALUE = double.Parse(txt_L1_DOWNRETURNVALUE.Text == "" ? "0" : txt_L1_DOWNRETURNVALUE.Text);
                    model.L1_COLOR_VALUEDOWN = txt_L1_COLOR_VALUEDOWN.Text;
                    model.L2_STARTDOWN = double.Parse(txt_L2_STARTDOWN.Text == "" ? "0" : txt_L2_STARTDOWN.Text);
                    model.L2_ENDDOWN = double.Parse(txt_L2_ENDDOWN.Text == "" ? "0" : txt_L2_ENDDOWN.Text);
                    model.L2_DOWNRETURNVALUE = double.Parse(txt_L2_DOWNRETURNVALUE.Text == "" ? "0" : txt_L2_DOWNRETURNVALUE.Text);
                    model.L2_COLOR_VALUEDOWN = txt_L2_COLOR_VALUEDOWN.Text;
                    model.L3_STARTDOWN = double.Parse(txt_L3_STARTDOWN.Text == "" ? "0" : txt_L3_STARTDOWN.Text);
                    model.L3_ENDDOWN = double.Parse(txt_L3_ENDDOWN.Text == "" ? "0" : txt_L3_ENDDOWN.Text);
                    model.L3_DOWNRETURNVALUE = double.Parse(txt_L3_DOWNRETURNVALUE.Text == "" ? "0" : txt_L3_DOWNRETURNVALUE.Text);
                    model.L3_COLOR_VALUEDOWN = txt_L3_COLOR_VALUEDOWN.Text;
                    model.REMARK = txt_REMARK.Text;
                    model.PermissionType = (com_PermissionType.SelectedItem as ListItem).Value.Trim();
                    model.ReportType = (com_ReportType.SelectedItem as ListItem).Value.Trim();
                    model.ISPUSH = rdo_ts.EditValue.ToString();
                    //model.ADDTIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    model.EXTENDCODE2 = EXTENDCODE2.Text;
                    model.EXTENDCODE3 = EXTENDCODE3.Text;
                    model.EXTENDCODE4 = EXTENDCODE4.Text;
                    model.EXTENDCODE5 = EXTENDCODE5.Text;
                    GlobalHandler.configresp.Insert(model);
                    CleanData();
                }
                XtraMessageBox.Show("保存成功");
                RefreshDate();
                layoutControl3.Visible = !layoutControl3.Visible;
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show("数据保存失败");
                _log.Error("数据保存失败，出错提示：" + exception.ToString());
            }
        }

        private void buttonEdit1_Click(object sender, EventArgs e)
        {
            SensoridManager from = new SensoridManager(srmodel);
            from.StartPosition = FormStartPosition.CenterScreen;
            from.hytype = hytype;
            from.ShowDialog();
            srmodel = from.model;
            if (srmodel==null) return;
            bet_cgqbm.Tag = srmodel.CGQBM;
            bet_cgqbm.Text = srmodel.CGQMC;
            //var a = bet_cgqbm.Tag;
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //导入
            if (!layoutControl3.Visible)
            {
                layoutControlGroup3.Visibility = LayoutVisibility.Never;
                foreach (Control item in layoutControl3.Controls)
                {
                    item.Visible = false;
                }
                var c = new Configimport();
                c.Name = "import";
                c.Dock = DockStyle.Fill;
                layoutControl3.Controls.Add(c);
                layoutControl3.Padding = new System.Windows.Forms.Padding(0, 0, 30, 0);
                layoutControl3.Visible = true;
            }
            else
            {
                foreach (Control item in layoutControl3.Controls)
                {
                    item.Visible = true;
                }
                layoutControl3.Controls.Remove(layoutControl3.Controls["import"]);
                layoutControl3.Visible = false;
                layoutControlGroup3.Visibility = LayoutVisibility.Always;

            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            layoutControl3.Visible = !layoutControl3.Visible;
            CleanData();
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ConfigEdit from = new ConfigEdit();
            from.StartPosition = FormStartPosition.CenterScreen;
            from.ShowDialog();
        }

        private void rdo_xxyj_EditValueChanged(object sender, EventArgs e)
        {
            if (rdo_xjyj.EditValue.ToString() == "0")
            {
                la_L1_STARTDOWN.Visibility = LayoutVisibility.Never;
                la_L1_ENDDOWN.Visibility = LayoutVisibility.Never;

                la_L1_COLOR_VALUEDOWN.Visibility = LayoutVisibility.Never;
                la_L2_STARTDOWN.Visibility = LayoutVisibility.Never;
                la_L2_ENDDOWN.Visibility = LayoutVisibility.Never;

                la_L2_COLOR_VALUEDOWN.Visibility = LayoutVisibility.Never;
                la_L3_STARTDOWN.Visibility = LayoutVisibility.Never;
                la_L3_ENDDOWN.Visibility = LayoutVisibility.Never;

                la_L3_COLOR_VALUEDOWN.Visibility = LayoutVisibility.Never;

                layoutX.Visibility = LayoutVisibility.Never;
                layoutY.Visibility = LayoutVisibility.Never;
            }
            else
            {
                la_L1_STARTDOWN.Visibility = LayoutVisibility.Always;
                la_L1_ENDDOWN.Visibility = LayoutVisibility.Always;

                la_L1_COLOR_VALUEDOWN.Visibility = LayoutVisibility.Always;
                la_L2_STARTDOWN.Visibility = LayoutVisibility.Always;
                la_L2_ENDDOWN.Visibility = LayoutVisibility.Always;

                la_L2_COLOR_VALUEDOWN.Visibility = LayoutVisibility.Always;
                la_L3_STARTDOWN.Visibility = LayoutVisibility.Always;
                la_L3_ENDDOWN.Visibility = LayoutVisibility.Always;

                la_L3_COLOR_VALUEDOWN.Visibility = LayoutVisibility.Always;
                layoutX.Visibility = LayoutVisibility.Always;
                layoutY.Visibility = LayoutVisibility.Always;
            }
        }

        private void searchControl1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //菜单查询
            var listwhere = aList.Where(a => a.BMMC.Contains(searchControl1.Text)).ToList();
            if (listwhere.Count<1)
            {
                BasicMonitorRecord model = new BasicMonitorRecord();
                model.Id = -1;
                model.BMMC = "没有匹配监测点！";
                listwhere.Add(model);
            }
            gridControl2.DataSource = listwhere;
            RefreshDate();
            EditData();
        }
    }
}