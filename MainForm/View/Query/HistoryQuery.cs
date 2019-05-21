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
using ServiceStack;
using SmartKylinData.IOTModel;
using System.Web.UI.WebControls;
using SmartKylinData.BaseModel;
using System.IO;
using NPOI.HSSF.UserModel;
using log4net;
using MongoDB.Bson;
using MongoDB.Driver;
using DevExpress.XtraCharts;
using DevExpress.Utils;

namespace SmartKylinApp.View.Query
{
    public partial class HistoryQuery : DevExpress.XtraEditors.XtraUserControl
    {
        private string hytype;
        private string bmid;
        private string codewhere;
        private List<Smart_Kylin_History> list;
        private ILog _log = LogManager.GetLogger("HistoryQuery");
        private List<ConfigRecord> datas2;
        private List<BasicMonitorRecord> aList;
        private List<Smart_Kylin_History> listw;

        public HistoryQuery()
        {
            InitializeComponent();
            //禁止右键菜单
            layoutControl2.AllowCustomization = false;
            layoutControl1.AllowCustomization = false;
            bar2.Manager.AllowShowToolbarsPopup = false;
            bar2.OptionsBar.AllowQuickCustomization = false;
            gridView2.OptionsMenu.EnableColumnMenu = false;
            gridView3.OptionsMenu.EnableColumnMenu = false;
            tree_mstype.Properties.TreeList.OptionsMenu.EnableColumnMenu = false;
        }

        private void HistoryQuery_Load(object sender, EventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            splashScreenManager1.SetWaitFormCaption("请稍后,数据加载中....");     // 标题
            splashScreenManager1.SetWaitFormDescription("正在更新数据.....");
            BindTree();
            date_ksrq.DateTime = DateTime.Now.AddDays(-1);
            date_jsrq.DateTime = DateTime.Now;
            splashScreenManager1.CloseWaitForm();
        }
        private void BindTree()
        {
            try { 
            var datas = GlobalHandler.mstyperesp.GetAllList();
            if (datas == null) return;
            var List = new List<TreeListModel>();
            var dt1 = datas.Where(a => a.TYPE_KEY.ToString().Length == 2);
            dt1.Each(a => List.Add(new TreeListModel() { ID = a.TYPE_KEY, ParentID = "1", Name = a.TYPE_NAME }));
            var dt2 = datas.Where(a => a.TYPE_KEY.ToString().Length == 4);
            dt2.Each(a => List.Add(new TreeListModel() { ID = a.TYPE_KEY, ParentID = a.TYPE_KEY.Substring(0, 2), Name = a.TYPE_NAME }));
            var dt3 = datas.Where(a => a.TYPE_KEY.ToString().Length == 6);
            dt3.Each(a => List.Add(new TreeListModel() { ID = a.TYPE_KEY, ParentID = a.TYPE_KEY.Substring(0, 4), Name = a.TYPE_NAME }));
            tree_mstype.Properties.DataSource = List;
            tree_mstype.Properties.DisplayMember = "Name";
            tree_mstype.Properties.ValueMember = "ID";
            tree_mstype.EditValue = "03";
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取行业类型数据出错");
                _log.Error("获取行业类型数据出错，出错提示：" + e.ToString());
            }
        }

        private void tree_mstype_EditValueChanged(object sender, EventArgs e)
        {
            var code = (tree_mstype.GetSelectedDataRow() as TreeListModel)?.ID;
            hytype = code;
            BindControl(code);

            GetComType();
        }
        private void BindAgreementTree(string mstype)
        {
            try{
                com_Type.Properties.Items.Clear();
                //监测项类型数据绑定
                if (!string.IsNullOrEmpty(mstype))
            {
                    //if (datas2 == null)
                    //{
                    //    datas2= GlobalHandler.configresp.GetAllList().ToList();
                    //} 
                var list = new List<TreeListModel>();
                var dt1 = GlobalHandler.configresp.GetAllList(a => a.CONFIG_CODE.StartsWith(mstype));
                    if (dt1 == null) return;
                    dt1.Each(a => list.Add(new TreeListModel() { ID = a.CONFIG_CODE, Name = a.CONFIG_DESC }));
                    ListItem it = null;
                    foreach (TreeListModel item in list)
                    {
                        it = new ListItem(item.Name, item.ID);
                        this.com_Type.Properties.Items.Add(it);
                    }
                    com_Type.SelectedIndex = 0;
            }
            else
            {
                    com_Type.Properties.Items.Clear();
            }
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取监测项类型数据出错");
                _log.Error("获取监测项描述数据出错，出错提示：" + e.ToString());
            }
        }
        private void BindControl(string mstype)
        {
            try { 
            //监测点数据绑定
            aList = GlobalHandler.monitorresp.GetAllList(a => a.BMID.Substring(6, 6).StartsWith(mstype)).ToList();
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
                XtraMessageBox.Show("获取数据出错");
                _log.Error("获取数据出错，出错提示：" + e.ToString());
            }
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {
            GetComType();         
        }

        private void GetComType()
        {
            //监测项类型
            var Id = int.Parse(gridView2.GetRowCellValue(gridView2.GetSelectedRows()[0], "Id").ToString());
            if (Id == -1) return;
            bmid = gridView2.GetRowCellValue(gridView2.GetSelectedRows()[0], "BMID").ToString();
            BindAgreementTree(bmid);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                chartControl1.Series.Clear();
                //splashScreenManager1.SetWaitFormDescription("正在更新.....");　　　　　// 信息
                if (tree_mstype.EditValue.ToString() == "" && barEditItem1.EditValue == null)
                {
                    tree_mstype.Focus();
                    XtraMessageBox.Show("请选择行业类型");
                    return;
                }
                if (com_Type.SelectedIndex == -1)
                {
                    tree_mstype.Focus();
                    XtraMessageBox.Show("请选择监测项类型");
                    return;
                }
                if (date_ksrq.DateTime > date_jsrq.DateTime)
                {
                    XtraMessageBox.Show("开始时间不能大于结束时间!");
                    return;
                }
                //查询
                splashScreenManager1.ShowWaitForm();
                splashScreenManager1.SetWaitFormCaption("请稍后,数据加载中....");     // 标题
                //list = GlobalHandler.historyResp.FindAll().OrderBy(a => a.SAVE_DATE).ToList();//全部数据
                Smart_Kylin_History model = new Smart_Kylin_History();
                var type = (com_Type.SelectedItem as ListItem).Value;
                codewhere = type;
                list = GlobalHandler.historyResp.Find(a => a.CONFIG_CODE.Contains(bmid) && a.CONFIG_CODE.Contains(type) && a.SAVE_DATE > DateTime.Parse(date_ksrq.Text) && a.SAVE_DATE < DateTime.Parse(date_jsrq.Text + " 23:59:59"))
                .OrderBy(a => a.SAVE_DATE).ToList();
                //根据条件查询数据，开始时间与结束时间闭区间

                if (list.Count < 1)
                {
                    XtraMessageBox.Show("查询数据为空!");
                    gridControl3.DataSource = null;
                }
                else
                {
                    BuilderDevChart(codewhere);
                    gridControl3.DataSource = list;
                }
                splashScreenManager1.CloseWaitForm();

            }
            catch (Exception ex)
            {
                splashScreenManager1.CloseWaitForm();
                XtraMessageBox.Show("查询数据出错");
                _log.Error("查询数据出错，出错提示：" + ex.ToString());
            }
        }
        private DataTable CreateChartData(string where)
        {
            //整理折线图数据
            DataTable table = new DataTable("Table1");
            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("Value", typeof(double));
            listw=list.Where(a=>a.CONFIG_CODE == where).ToList();
            for (int i=0;i < listw.Count;i++)
            {
                table.Rows.Add(new object[] { listw[i].SAVE_DATE, listw[i].CONFIG_VALUE });
            }
            return table;
        }
        private DataTable CreateChartMax()
        {
            //准备最大正常值数据
            DataTable table = new DataTable("Table1");
            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("Value", typeof(double));
            //TagInfoRecord model = GlobalHandler.tagresp.FirstOrDefault(a => a.TAG_KEY == (com_Type.SelectedItem as ListItem).Value);
            ConfigRecord model = GlobalHandler.configresp.FirstOrDefault(a => a.CONFIG_CODE == (com_Type.SelectedItem as ListItem).Value);
            //list.Max(a=>a.SAVE_DATE);
            if (model != null && model.MAX_VALUE != 0d)
            {
                for (int i = 0; i < listw.Count; i++)
                {
                    table.Rows.Add(new object[] { listw[i].SAVE_DATE, model.MAX_VALUE });
                }
            }
            //if (model != null&& model.MAX_VALUE!=0d)
            //{ 
            //table.Rows.Add(new object[] { list.Min(a => a.SAVE_DATE), model.MAX_VALUE });
            //table.Rows.Add(new object[] { list.Max(a => a.SAVE_DATE), model.MAX_VALUE });
            //}
            return table;
        }
        private DataTable CreateChartMin()
        {
            //准备最小正常值数据
            DataTable table = new DataTable("Table1");
            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("Value", typeof(double));
            //TagInfoRecord model = GlobalHandler.tagresp.FirstOrDefault(a => a.TAG_KEY == (com_Type.SelectedItem as ListItem).Value);
            ConfigRecord model = GlobalHandler.configresp.FirstOrDefault(a => a.CONFIG_CODE == (com_Type.SelectedItem as ListItem).Value);
            //list.Max(a => a.SAVE_DATE);
            for (int i = 0; i < listw.Count; i++)
            {
                table.Rows.Add(new object[] { listw[i].SAVE_DATE, model.MIN_VALUE });
            }
            //if (model != null)
            //{
            //    table.Rows.Add(new object[] { list.Min(a => a.SAVE_DATE), model.MIN_VALUE });
            //    table.Rows.Add(new object[] { list.Max(a => a.SAVE_DATE), model.MIN_VALUE });
            //}
            return table;
        }
        private void BuilderDevChart(string codewhere)
        {
            try { 
            //绑定折线图数据
           
            //绘制折线图
            //var listConfig =GlobalHandler.configresp.GetAllList(a => a.CONFIG_CODE.Contains(codewhere));
            //for (int i=0; i< listConfig.Count;i++)
            //{ 
            Series _lineSeries = new Series("监测值", ViewType.Line);
            _lineSeries.ArgumentScaleType = ScaleType.DateTime;
            //_lineSeries.ArgumentScaleType = ScaleType.Qualitative;
            _lineSeries.ArgumentDataMember = "Date";
            _lineSeries.ValueDataMembers[0] = "Value";
            _lineSeries.DataSource = CreateChartData(codewhere);
            chartControl1.Series.Add(_lineSeries);
            //}
            //绘制最大正常值
            Series _lineMax = new Series("最大正常值", ViewType.Line);
            _lineMax.ArgumentScaleType = ScaleType.DateTime;
            _lineMax.ArgumentDataMember = "Date";
            _lineMax.ValueDataMembers[0] = "Value";
            _lineMax.DataSource = CreateChartMax();
            //绘制最小正常值
            Series _lineMin = new Series("最小正常值", ViewType.Line);
            _lineMin.ArgumentScaleType = ScaleType.DateTime;
            _lineMin.ArgumentDataMember = "Date";
            _lineMin.ValueDataMembers[0] = "Value";
            _lineMin.DataSource = CreateChartMin();


            chartControl1.Series.Add(_lineMax);
            chartControl1.Series.Add(_lineMin);
            XYDiagram _diagram = (XYDiagram)chartControl1.Diagram;
            if (_diagram != null)
            {
                _diagram.AxisX.DateTimeMeasureUnit = DateTimeMeasurementUnit.Millisecond;//X轴刻度单位
                _diagram.AxisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Day;//X轴刻度间距
                _diagram.AxisX.DateTimeOptions.Format = DateTimeFormat.Custom;
               // _diagram.AxisX.DateTimeOptions.Format = DateTimeFormat.LongTime;
                _diagram.AxisX.DateTimeOptions.FormatString = "yyyy/MM/dd";
                    //_diagram.AxisX.Range.Auto = false;//要开启滚动条必须将其设置为false
                    //_diagram.AxisX.Range.MaxValueInternal = 500;//在不拉滚动条的时候，X轴显示多个值，既固定的X轴长度。
                    //_diagram.EnableAxisXScrolling = true;//启用X轴滚动条
                }

            }
            catch (Exception exception)
            {
                XtraMessageBox.Show("折线图显示失败");
                _log.Error("折线图显示失败，出错提示：" + exception.ToString());
            }
        }
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //工具栏监测点查询
            //tree_mstype.Text = "";
            if (barEditItem1.EditValue == null || barEditItem1.EditValue.ToString() == "")
            {
                XtraMessageBox.Show("请输入查询内容");
                return;
            }
            string where = barEditItem1.EditValue.ToString();
                var cList = aList.Where(a => a.BMMC.Contains(where)).ToList();
                if (cList.Count < 1)
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
                    gridControl1.DataSource = cList;

                }
            GetComType();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (list == null||list.Count < 1)
            {
                XtraMessageBox.Show("导出数据为空！");
                return;
            }
            //导出
            FileStream fs = null;
            try
            {
                var saveFileName = "历史数据.xls";
                fs = new FileStream(saveFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                var workbook = new HSSFWorkbook();
                var sheet = workbook.CreateSheet();
                string[] names = new string[] { "监测点名称", "监测项描述", "监测项编码", "监测值","监测时间"};
                var headerRow = sheet.CreateRow(0);
                for (int i = 0; i < names.Length; i++)
                {
                    sheet.SetColumnWidth(i, 30 * 256);
                    headerRow.CreateCell(i).SetCellValue(names[i]);
                }
                for (var i = 0; i < list.Count; i++)
                {
                    var row = sheet.CreateRow(i + 1);
                    row.CreateCell(0).SetCellValue(gridView2.GetRowCellValue(gridView2.GetSelectedRows()[0], "BMMC").ToString());
                    row.CreateCell(1).SetCellValue(com_Type.Text);
                    row.CreateCell(2).SetCellValue(list[i].CONFIG_CODE);
                    row.CreateCell(3).SetCellValue(list[i].CONFIG_VALUE);
                    row.CreateCell(4).SetCellValue(list[i].SAVE_DATE.ToString());
                }
                sheet.CreateFreezePane(0, 1, 0, 1);
                var saveDialog = new SaveFileDialog
                {
                    DefaultExt = "xls",
                    Filter = @"Excel文件|*.xls;*.xlsx",
                    FileName = saveFileName
                };
                saveDialog.ShowDialog();
                saveFileName = saveDialog.FileName;
                if (saveFileName.IndexOf(":", StringComparison.Ordinal) < 0) return; //被点了取消

                if (saveFileName == "") return;
                try
                {
                    fs = File.OpenWrite(saveDialog.FileName);
                    workbook.Write(fs);
                    XtraMessageBox.Show(@"导出成功！");
                }
                catch (Exception ex)
                {
                   XtraMessageBox.Show(@"导出文件时出错,文件可能正被打开！\n" + ex.Message);
                }
            }
            catch (Exception exception)
            {
                //fs?.Close();
                _log.Error("导出数据出错，出错提示：" + exception.ToString());
            }
            finally
            {
                fs?.Close();

            }
        }

        private void chartControl1_CustomDrawCrosshair(object sender, CustomDrawCrosshairEventArgs e)
        {
            //foreach (CrosshairElement element in e.CrosshairElements)
            //{
            //    SeriesPoint point = element.SeriesPoint;
            //    element.LabelElement.Text = point.Values[0].ToString();//显示要显示的文字
            //    element.LabelElement.Text = string.Format("监测值:{0} ", point.Values[0].ToString());//显示要显示的文字
            //}
            foreach (CrosshairElementGroup a in e.CrosshairElementGroups)
            {
                foreach (CrosshairElement element in e.CrosshairElements)
                {
                    SeriesPoint point = element.SeriesPoint;
                    a.HeaderElement.Text = "监测时间:" + point.Argument;//显示要显示的文字
                }
            }
        }
        /// <summary>
        /// 自定义ChartControl的Tooltip
        /// </summary>
        //public void CustomToolTip( MouseEventArgs e, ToolTipController tooltip, string tooltipTitle, System.Func<string, double[], string> paramter)
        //{
        //    ChartHitInfo _hitInfo = chartControl1.CalcHitInfo(e.X, e.Y);
        //    SeriesPoint _point = _hitInfo.SeriesPoint;
        //    if (_point != null)
        //    {
        //        string _msg = paramter(_point.Argument, _point.Values);
        //        tooltip.ShowHint(_msg, tooltipTitle);
        //    }
        //    else
        //    {
        //        tooltip.HideHint();
        //    }
        //}

        private void chartControl1_MouseMove(object sender, MouseEventArgs e)
        {
            //CustomToolTip( e, toolTipController1, "交易详情", (agr, values) =>
            //{
            //    return string.Format("时间：{0}\r\n金额:{1}", agr, values[0]);
            //});
        }
    }
}
