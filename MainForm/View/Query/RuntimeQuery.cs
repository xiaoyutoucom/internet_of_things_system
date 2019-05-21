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

namespace SmartKylinApp.View.Query
{
    public partial class RuntimeQuery : DevExpress.XtraEditors.XtraUserControl
    {
        private string hytype;
        private string bmid;
        private string codewhere;
        private List<Smart_Kylin_Runtime> list;
        private ILog _log = LogManager.GetLogger("HistoryQuery");
        private List<BasicMonitorRecord> alist;
        private List<DataListModel> dllist;
        private Timer timea;

        public RuntimeQuery()
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

        private void RuntimeQuery_Load(object sender, EventArgs e)
        {
            BindTree();
            timea = new Timer();
            int t = int.Parse(ConfigHelp.Config["Application:Config:Time"]);
            barTime.Caption = "刷新频率：" + t.ToString();
            timea.Interval = 60000 * t;
            timea.Enabled = true;
            timea.Tick += Time_Tick;
        }

        private void Time_Tick(object sender, EventArgs e)
        {
            GetComType();
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
        private void BindControl(string mstype)
        {
            try { 
            //监测点数据绑定
            alist = GlobalHandler.monitorresp.GetAllList(a => a.BMID.Substring(6, 6).StartsWith(mstype)).ToList();
            if (alist.Count < 1)
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
                gridControl1.DataSource = alist;

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
            try
            {
                barDate.Caption = "最后更新时间：" + DateTime.Now.ToLongTimeString().ToString();
                //监测项类型
                var Id = int.Parse(gridView2.GetRowCellValue(gridView2.GetSelectedRows()[0], "Id").ToString());
            if (Id == -1) return;
            bmid = gridView2.GetRowCellValue(gridView2.GetSelectedRows()[0], "BMID").ToString();

                //splashScreenManager1.SetWaitFormDescription("正在更新.....");　　　　　// 信息
                if (tree_mstype.EditValue.ToString() == "" && barEditItem1.EditValue == null)
                {
                    tree_mstype.Focus();
                    XtraMessageBox.Show("请选择行业类型");
                    return;
                }
                //if (com_Type.SelectedIndex == -1)
                //{
                //    tree_mstype.Focus();
                //    XtraMessageBox.Show("请选择监测项类型");
                //    return;
                //}
                //查询
                splashScreenManager1.ShowWaitForm();
                splashScreenManager1.SetWaitFormCaption("请稍后,数据加载中....");     // 标题
                //list = GlobalHandler.historyResp.FindAll().OrderBy(a => a.SAVE_DATE).ToList();//全部数据
                Smart_Kylin_History model = new Smart_Kylin_History();
                 list = GlobalHandler.runtimeResp.Find(a => a.CONFIG_CODE.Contains(bmid))
                .OrderBy(a => a.SAVE_DATE).ToList();
                //根据条件查询数据，开始时间与结束时间闭区间

                if (list.Count < 1)
                {
                    //XtraMessageBox.Show("查询数据为空!");
                    gridControl3.DataSource = null;
                }
                else
                {
                    dllist = new List<DataListModel>();
                    ConfigRecord cr = new ConfigRecord();
                    foreach (Smart_Kylin_Runtime dtm in list)
                    {
                        cr = GlobalHandler.configresp.FirstOrDefault(a => a.CONFIG_CODE == dtm.CONFIG_CODE);
                        if (cr == null)
                        {
                            dllist.Add(new DataListModel() { CONFIG_CODE = dtm.CONFIG_CODE, CONFIG_VALUE = dtm.CONFIG_VALUE, SAVE_DATE = dtm.SAVE_DATE.ToString(), CONFIGMC = "", MONITORMC = "", LEVEL = "" });
                        }
                        else
                        {
                            dllist.Add(new DataListModel() { CONFIG_CODE = dtm.CONFIG_CODE, CONFIG_VALUE = dtm.CONFIG_VALUE, SAVE_DATE = dtm.SAVE_DATE.ToString(), CONFIGMC = cr.CONFIG_DESC, MONITORMC = cr.STATIONID.BMMC, LEVEL = dtm.LEVEL=="1"?"是":"否" });
                        }
                    }
                    gridControl3.DataSource = dllist;
                }
                splashScreenManager1.CloseWaitForm();

            }
            catch (Exception ex)
            {
                splashScreenManager1.CloseWaitForm();
                XtraMessageBox.Show("显示数据出错");
                _log.Error("显示数据出错，出错提示：" + ex.ToString());
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
           
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
                var cList = alist.Where(a => a.BMMC.Contains(where)).ToList();
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
                var saveFileName = "实时数据.xls";
                fs = new FileStream(saveFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                var workbook = new HSSFWorkbook();
                var sheet = workbook.CreateSheet();
                string[] names = new string[] { "监测点名称", "监测项描述", "监测项编码", "监测值","报警级别", "监测时间" };
                var headerRow = sheet.CreateRow(0);
                for (int i = 0; i < names.Length; i++)
                {
                    sheet.SetColumnWidth(i, 30 * 256);
                    headerRow.CreateCell(i).SetCellValue(names[i]);
                }
                for (var i = 0; i < dllist.Count; i++)
                {
                    var row = sheet.CreateRow(i + 1);
                    row.CreateCell(0).SetCellValue(gridView2.GetRowCellValue(gridView2.GetSelectedRows()[0], "BMMC").ToString());
                    row.CreateCell(1).SetCellValue(dllist[i].CONFIGMC);
                    row.CreateCell(2).SetCellValue(dllist[i].CONFIG_CODE);
                    row.CreateCell(3).SetCellValue(dllist[i].CONFIG_VALUE);
                    row.CreateCell(4).SetCellValue(dllist[i].LEVEL);                    
                    row.CreateCell(5).SetCellValue(dllist[i].SAVE_DATE.ToString());
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

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GetComType();
        }

        private void RuntimeQuery_VisibleChanged(object sender, EventArgs e)
        {
            timea.Stop();
        }
    }
}
