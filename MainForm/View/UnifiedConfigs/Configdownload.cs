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
using log4net;
using NPOI.HSSF.Util;

namespace SmartKylinApp.View.BaseConfig
{
    public partial class Configdownload : DevExpress.XtraEditors.XtraForm
    {
        private ILog _log = LogManager.GetLogger("Configdownload");
        public Configdownload(){
            InitializeComponent();
            //禁止右键菜单
            layoutControl1.AllowCustomization = false;
            gridView1.OptionsMenu.EnableColumnMenu = false;
            treeList1.OptionsMenu.EnableColumnMenu = false;
        }

        private List<DeviceRecord> DeviceList;
        private string mstype;

        private void treeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {

        }

        private void sensortemdownload_Load(object sender, EventArgs e)
        {
            DeviceList = GlobalHandler.deviceresp.GetAllList();
            BindTree();

            gridView1.SelectionChanged += GridView1_SelectionChanged;
            mstype = treeList1.FocusedNode.GetValue("ID").ToString(); ;
            GetData(mstype);
        }

        private void GridView1_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            lab_selectnum.Text = gridView1.GetSelectedRows().Length.ToString();
        }


        private void GetData(string mstype)
        {
            var list = GlobalHandler.monitorresp.GetAllList();
            //var list = new List<BasicMonitorRecord>();
            var aaList = list.Where(a => a.BMID.Substring(6,6) == mstype).ToList();
            BindView(aaList);
        }
        private void BindView(object list)
        {
            gridControl1.DataSource = list;
        }

        //绑定行业信息
        private void BindTree()
        {
            var datas = GlobalHandler.mstyperesp.GetAllList();
            if (datas == null) return;
            var list = new List<TreeListModel>();
            var dt1 = datas.Where(a => a.TYPE_KEY.ToString().Length == 2);
            dt1.Each(a => list.Add(new TreeListModel() { ID = a.TYPE_KEY, ParentID = "1", Name = a.TYPE_NAME }));
            var dt2 = datas.Where(a => a.TYPE_KEY.ToString().Length == 4);
            dt2.Each(a => list.Add(new TreeListModel() { ID = a.TYPE_KEY, ParentID = a.TYPE_KEY.Substring(0, 2), Name = a.TYPE_NAME }));
            var dt3 = datas.Where(a => a.TYPE_KEY.ToString().Length == 6);
            dt3.Each(a => list.Add(new TreeListModel() { ID = a.TYPE_KEY, ParentID = a.TYPE_KEY.Substring(0, 4), Name = a.TYPE_NAME }));
            treeList1.DataSource = list;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            FileStream fs = null;
            try
            {
                var index = gridView1.GetSelectedRows();
                if (index.Length<1)
                {
                   XtraMessageBox.Show(@"请选择监测点");
                    return;
                }
                var saveFileName = "监测项信息导入模板.xls";
                fs = new FileStream(saveFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                var workbook = new HSSFWorkbook();
                var sheet = workbook.CreateSheet();
                //(Optional) set the width of the columns
                //"监测项类型名称", "监测项类型编码",
                string[] names = new string[] { "监测点名称", "监测点编码（必填）", "监测项类型编码（必填）", "监测项描述（必填）", "变量名称", "页面相对位置X", "页面相对位置Y", "单位", "显示精度", "传感器编码", "内部设施", "上报频率", "是否推送", "是否预警", "一级预警范围起始" ,"一级预警范围终止", "一级预警彩色值", "二级预警范围起始", "二级预警范围终止", "二级预警彩色值", "三级预警范围起始", "三级预警范围终止",  "三级预警彩色值","是否下行预警", "下行一级预警范围起始", "下行一级预警范围终止", "下行一级预警彩色值", "下行二级预警范围起始", "下行二级预警范围终止", "下行二级预警彩色值", "下行三级预警范围起始", "下行三级预警范围终止", "下行三级预警彩色值", "最大正常值", "最小正常值", "量程最大值", "量程最小值", "排序号", "分组", "彩色值", "消息模板", "备注","权限类型","报表类型" };
                var headerRow = sheet.CreateRow(0);
                for (int i = 0; i < names.Length; i++)
                {
                    sheet.SetColumnWidth(i, 30 * 256);
                    headerRow.CreateCell(i).SetCellValue(names[i]);
                }
                sheet.CreateFreezePane(0, 1, 0, 1);

                string[] names1 = new string[] { "（示例数据请手动删除）微商银行门口东",   "3413029904980000001",    "990498_001",  "井盖状态", "",   "",   "",   "",  "" ,   "C00003_1",  "" ,   "360",   "" ,   "是",  "0.5",    "1.5",    "",   "0.5",    "3.5",   "" ,   "0.5",    "3.5"};
                var headerRow2 = sheet.CreateRow(1);
                for (int i = 0; i < names1.Length; i++)
                {
                    sheet.SetColumnWidth(i, 30 * 256);
                    headerRow2.CreateCell(i).SetCellValue(names1[i]);
                }
                //获取选中的监测点信息


                for (var i = 0; i < index.Length; i++)
                {
                    var row = sheet.CreateRow(i + 2);
                    row.CreateCell(0).SetCellValue(gridView1.GetRowCellValue(index[i], "BMMC").ToString());
                    row.CreateCell(1).SetCellValue(gridView1.GetRowCellValue(index[i], "BMID").ToString());
                }
                var saveDialog = new SaveFileDialog
                {
                    DefaultExt = "xls",
                    Filter = @"Excel文件|*.xls;*.xlsx",
                    FileName = saveFileName
                };
                saveDialog.ShowDialog(); saveFileName = saveDialog.FileName;
                if (saveFileName.IndexOf(":", StringComparison.Ordinal) < 0) return; //被点了取消

                if (saveFileName == "") return;

                try
                {
                    fs = File.OpenWrite(saveDialog.FileName);
                    workbook.Write(fs);
                    fs?.Close();
                    try
                    {
                        var saveFileName1 = "监测项信息填写说明.xls";
                        var fs1 = new FileStream(saveFileName1, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        var workbook1 = new HSSFWorkbook();
                        var sheet1 = workbook1.CreateSheet();
                        IFont font = workbook1.CreateFont();
                        font.FontHeightInPoints = 14;//设置字体大小
                        font.Color = HSSFColor.Red.Index;
                        ICellStyle style = workbook1.CreateCellStyle();
                        style.SetFont(font);
                        sheet1.SetColumnWidth(0, 20 * 256);
                        var headerRowTop = sheet1.CreateRow(0);
                        headerRowTop.CreateCell(0).SetCellValue($"监测项类型编码、传感器编码: 信息填写不能为空");
                        sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, 6));
                        headerRowTop.GetCell(0).CellStyle = style;
                        var headerRow1 = sheet1.CreateRow(1);
                        headerRow1.Height = 300;
                        string[] names2 = new string[] { "监测项类型名称 ", "监测项类型编码", "设备编码", "设备名称", "传感器名称", "传感器编码" };
                        for (int i = 0; i < names2.Length; i++)
                        {
                            sheet1.SetColumnWidth(i, 30 * 256);
                            headerRow1.CreateCell(i).SetCellValue(names2[i]);
                        }
                     
                        sheet1.CreateFreezePane(0, 2, 0, 2);

                        //获取选中的监测点信息
                        //var index1 = gridView1.GetSelectedRows();
                        
                        var datas = GlobalHandler.tagresp.GetAllList(a => a.TAG_KEY.ToString().StartsWith(mstype)).ToList();

                        for (var i = 1; i < datas.Count+1; i++)
                        {
                            var row = sheet1.CreateRow(i + 1);
                            row.CreateCell(0).SetCellValue(datas[i-1].TAG_NAME);
                            row.CreateCell(1).SetCellValue(datas[i - 1].TAG_KEY);
                            //var TAG_KEY = datas[i - 1].TAG_KEY;
                            //row.CreateCell(1).SetCellValue(TAG_KEY.Substring(TAG_KEY.Length-3,3));
                        }
                        var datas1 = GlobalHandler.deviceresp.GetAllList(a => a.SBBM.ToString().StartsWith(mstype)).ToList();
                        int cot = 0;
                        int num = 0;
                        var d = GlobalHandler.sensorresp.GetAllList();
                        for (var i = 0; i < datas1.Count; i++)
                        {
                            //var row = sheet1.GetRow(i+1);
                            //row.CreateCell(3).SetCellValue(datas1[i].SBBM);
                            //row.CreateCell(4).SetCellValue(datas1[i].SBMC);
                            var da = d.Where(a => a.CGQBM.ToString().StartsWith(datas1[i].CCBH)).ToList();
                            if (da.Count == 0|| datas1[i].CCBH.ToString()=="")
                            {
                                IRow row2;
                                if (sheet1.GetRow( 2 + cot) != null)
                                {
                                    row2 = sheet1.GetRow(2 + cot);
                                }
                                else
                                {
                                    row2 = sheet1.CreateRow( cot + 2);
                                }
                                row2.CreateCell(3).SetCellValue(datas1[i].SBBM);
                                row2.CreateCell(2).SetCellValue(datas1[i].SBMC);
                            }

                            if (da.Count>0)
                            {
                                for (var j = 1; j < da.Count+1; j++)
                                {
                                        IRow row2;
                                        if (sheet1.GetRow(j + 1 + cot) != null)
                                        {
                                            row2 = sheet1.GetRow(j + 1 + cot);
                                        }
                                        else
                                        {
                                            row2 = sheet1.CreateRow(j + cot + 1);
                                        }
                                        row2.CreateCell(3).SetCellValue(datas1[i].SBBM);
                                        row2.CreateCell(2).SetCellValue(datas1[i].SBMC);
                                        row2.CreateCell(5).SetCellValue(da[j-1].CGQBM);
                                        row2.CreateCell(4).SetCellValue(da[j-1].CGQMC);
                                        num++;
                                }
                                if (da.Count > 1) { 
                                sheet1.AddMergedRegion(new CellRangeAddress(num-da.Count+2, num+1 , 2, 2));
                                sheet1.AddMergedRegion(new CellRangeAddress(num-da.Count+2, num+1, 3, 3));
                                }
                                cot = cot + da.Count;
                            }
                        }
                       

                        var saveDialog1 = new SaveFileDialog
                        {
                            DefaultExt = "xls",
                            Filter = @"Excel文件|*.xls;*.xlsx",
                            FileName = saveFileName1
                        };
                        saveDialog1.ShowDialog(); saveFileName1 = saveDialog1.FileName;

                        if (saveFileName1.IndexOf(":", StringComparison.Ordinal) < 0) return; //被点了取消

                        if (saveFileName1 == "") return;

                        try
                        {
                            fs1 = File.OpenWrite(saveDialog1.FileName);
                            workbook1.Write(fs1);
                            XtraMessageBox.Show(@"下载成功！");
                            fs1?.Close();
                        }
                        catch (Exception ex)
                        {
                           XtraMessageBox.Show(@"导出文件时出错,文件可能正被打开！\n" + ex.Message);
                            fs1?.Close();
                            _log.Error("导出文件时出错,文件可能正被打开，出错提示：" + e.ToString());
                        }
                    }
                    catch (Exception exception)
                    {
                        XtraMessageBox.Show(@"导出失败！\n" + exception.Message);
                        _log.Error("导出失败，出错提示：" + exception.ToString());
                        fs?.Close();
                    }
                }
                catch (Exception ex)
                {
                   XtraMessageBox.Show(@"导出文件时出错,文件可能正被打开！\n" + ex.Message);
                    fs?.Close();
                    _log.Error("导出文件时出错,文件可能正被打开，出错提示：" + e.ToString());
                }



            }
            catch (Exception exception)
            {
                fs?.Close();
                _log.Error("下载出错，出错提示：" + exception.ToString());
                XtraMessageBox.Show(@"下载出错！\n" + exception.Message);
                GC.SuppressFinalize(this);
            }
            finally
            {
                fs?.Close();

            }
            this.DialogResult = DialogResult.OK;
        }

        private void treeList1_Click(object sender, EventArgs e)
        {
            mstype = treeList1.FocusedNode.GetValue("ID").ToString(); 
            if (mstype.Length != 6)
            {
                return;
            }
            
            GetData(mstype);
        }
    }
}