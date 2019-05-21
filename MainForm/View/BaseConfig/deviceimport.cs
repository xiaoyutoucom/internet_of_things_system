using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using NPOI.HSSF.UserModel;
using SmartKylinApp.Common;
using log4net;
using NPOI.SS.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.Util;
using System.Web;
using System.Net;

namespace SmartKylinApp.View.BaseConfig
{
    public partial class deviceimport : UserControl
    {
        private ILog _log = LogManager.GetLogger("deviceimport");
        public deviceimport()
        {
            InitializeComponent();
            btn_check.Enabled = false;
            btn_close.Enabled = false;
        }

        private bool isCheck = false;

        private void btn_mb_Click(object sender, EventArgs e)
        {
            //设备导入模板下载
            FileStream fs = null;
            try
            {
                var saveFileName = "设备信息导入模板.xls";

                fs = new FileStream(saveFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                var workbook = new HSSFWorkbook();
                var sheet = workbook.CreateSheet();
                //(Optional) set the width of the columns
                sheet.SetColumnWidth(0, 40 * 256);
                sheet.SetColumnWidth(1, 40 * 256);
                sheet.SetColumnWidth(2, 30 * 256);
                sheet.SetColumnWidth(3, 30 * 256);
                sheet.SetColumnWidth(4, 30 * 256);
                sheet.SetColumnWidth(5, 30 * 256);
                sheet.SetColumnWidth(6, 30 * 256);
                sheet.SetColumnWidth(7, 30 * 256);
                sheet.SetColumnWidth(8, 30 * 256);

                var headerRow = sheet.CreateRow(0);

                //Set the column names in the header row
                headerRow.CreateCell(0).SetCellValue("行业类型（必填）");
                headerRow.CreateCell(1).SetCellValue("设备名称（必填）");
                headerRow.CreateCell(2).SetCellValue("区划编号（必填）");
                headerRow.CreateCell(3).SetCellValue("区划名称");
                headerRow.CreateCell(4).SetCellValue("出厂编号（必填）");
                headerRow.CreateCell(5).SetCellValue("生产厂家");
                headerRow.CreateCell(6).SetCellValue("上传频率");
                headerRow.CreateCell(7).SetCellValue("操作人");
                headerRow.CreateCell(8).SetCellValue("安装时间");
                headerRow.CreateCell(9).SetCellValue("管理单位");
                headerRow.CreateCell(10).SetCellValue("备注");

                sheet.CreateFreezePane(0, 1, 0, 1);
                var headerRow2 = sheet.CreateRow(1);
                //Set the column names in the header row
                headerRow2.CreateCell(0).SetCellValue("（示例数据请手动删除）990498");
                headerRow2.CreateCell(1).SetCellValue("4路灯后井盖设备");
                headerRow2.CreateCell(2).SetCellValue("341302");
                headerRow2.CreateCell(3).SetCellValue("埇桥区");
                headerRow2.CreateCell(4).SetCellValue("C00071");
                headerRow2.CreateCell(5).SetCellValue("生产厂家名称");
                headerRow2.CreateCell(6).SetCellValue("360");
                headerRow2.CreateCell(7).SetCellValue("操作人");
                headerRow2.CreateCell(8).SetCellValue("2018-01-01");
                headerRow2.CreateCell(9).SetCellValue("宿州市排水公司");
                headerRow2.CreateCell(10).SetCellValue("填写备注信息");
                var rowNumber = 1;

                var saveDialog = new SaveFileDialog();
                saveDialog.DefaultExt = "xls"; saveDialog.Filter = @"Excel文件|*.xls;*.xlsx";
                saveDialog.FileName = saveFileName;
                saveDialog.ShowDialog();saveFileName = saveDialog.FileName;
                if (saveFileName.IndexOf(":", StringComparison.Ordinal) < 0) return; //被点了取消

                if (saveFileName == "") return;

                try
                {
                    fs = File.OpenWrite(saveDialog.FileName);
                    workbook.Write(fs);
                    XtraMessageBox.Show(@"下载成功（第一条数据为示例数据，请手动删除）！");
                }
                catch (Exception ex)
                {
                    _log.Error("导出文件时出错，文件可能正被打开，出错提示：" + ex.ToString());
                   XtraMessageBox.Show(@"导出文件时出错,文件可能正被打开！\n" + ex.Message);
                }
            }catch (Exception exception)
            {
                //fs?.Close();
                GC.SuppressFinalize(this);
               XtraMessageBox.Show(@"下载失败！\n" + exception.Message);
                _log.Error("下载失败，出错提示：" + exception.ToString());
            }
            finally
            {
                fs?.Close();
            }
        }

        private void btn_ck_Click(object sender, EventArgs e)
        {
            //下载参考文档
            try { 
            string fileName = "设备信息导入说明.xls";
            WebClient client = new WebClient();
            var Dialog = new FolderBrowserDialog();
            Dialog.Description = "请选择文件下载路径";
          
            DialogResult result = Dialog.ShowDialog(); if (result == System.Windows.Forms.DialogResult.Cancel) { return; }
            string folderPath = Dialog.SelectedPath.Trim();
            DirectoryInfo theFolder = new DirectoryInfo(folderPath);
            if (theFolder.Exists)
            {
                    //操作  
                    //folderPath + "//参考文档" +
                    //     client.DownloadFile("../../../Data//" + fileName, folderPath + "//" + fileName);
                    client.DownloadFile(fileName, folderPath + "//" + fileName);
                    XtraMessageBox.Show(@"下载成功！");
                    return;
            }
            else
            {
                XtraMessageBox.Show(@"目录不正确！");
            }
            }
            catch(Exception exception)
            {
                XtraMessageBox.Show(@"下载失败！");
                _log.Error("下载失败，出错提示：" + exception.ToString());
            }
            #region 代码生成
            //    //设备导入信息参考：
            //    FileStream fs = null;
            //try
            //{
            //    var saveFileName = "设备信息导入说明.xls";
            //    fs = new FileStream(saveFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            //    var workbook = new HSSFWorkbook();

            //    IFont font = workbook.CreateFont();
            //    font.FontHeightInPoints = 14;//设置字体大小
            //    font.Color = HSSFColor.Red.Index;
            //    ICellStyle style = workbook.CreateCellStyle();
            //    style.SetFont(font);
            //    //获取行业类型，填充数据
            //    var mstypelist = GlobalHandler.mstyperesp.GetAllList();
            //    if (mstypelist == null) return;
            //    var mlist = mstypelist.Where(a=>a.TYPE_KEY.Length==2).ToList();

            //    for (var i = 0; i < mlist.Count; i++)
            //    {
            //        var mslist = mstypelist.Where(a => a.TYPE_KEY.StartsWith(mlist[i].TYPE_KEY)).Where(a => a.TYPE_KEY.Length == 6).ToList();
            //        var sheet = workbook.CreateSheet(mlist[i].TYPE_NAME);
            //        sheet.SetColumnWidth(0, 20 * 256);
            //        sheet.SetColumnWidth(1, 40 * 256);
            //        sheet.SetColumnWidth(2, 30 * 256);
            //        var headerRowTop = sheet.CreateRow(0);
            //        headerRowTop.CreateCell(0).SetCellValue($"设备编码: 信息填写不能为空,格式： 行业类型编号+五位数字 示例：03010100001");
            //        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 6));
            //        var headerRow = sheet.CreateRow(1);
            //        headerRow.CreateCell(0).SetCellValue("行业类型名称");
            //        headerRow.CreateCell(1).SetCellValue("行业类型编号");
            //        sheet.CreateFreezePane(0, 2, 0, 2);

            //        headerRowTop.GetCell(0).CellStyle = style;
            //        for (var j = 0; j < mslist.Count; j++)
            //        { 
            //        var row = sheet.CreateRow(j + 2);
            //        row.CreateCell(0).SetCellValue($"{mslist[j].TYPE_NAME}");
            //        row.CreateCell(1).SetCellValue($"{mslist[j].TYPE_KEY}");
            //        }
            //    }
            //    var sheet1 = workbook.CreateSheet("区划信息");
            //    sheet1.SetColumnWidth(0, 20 * 256);
            //    sheet1.SetColumnWidth(1, 40 * 256);
            //    sheet1.SetColumnWidth(2, 30 * 256);
            //    var headerRowTop1 = sheet1.CreateRow(0);
            //    headerRowTop1.CreateCell(0).SetCellValue($"设备编码: 信息填写不能为空,格式： 区划号码+监测点类型编号+七位数字 示例：3705020111960000001");
            //    sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, 6));
            //    var headerRow1 = sheet1.CreateRow(1);
            //    headerRow1.CreateCell(0).SetCellValue("当前区划编号");
            //    headerRow1.CreateCell(1).SetCellValue("当前区划名称");
            //    sheet1.CreateFreezePane(0, 2, 0, 2);
            //    headerRowTop1.GetCell(0).CellStyle = style;
            //    //获取区划信息
            //    var code = ConfigHelp.Config["Application:Config:City"];
            //    if (code.Contains("0000")) code = code.Substring(0, 2);
            //    if (code.Contains("00")) code = code.Substring(0, 4);
            //    var list = GlobalHandler.CityInfo.Where(a => a.CITYCODE.StartsWith(code)).ToList();
            //    for (var i = 0; i <list.Count; i++)
            //    {
            //        var row = sheet1.CreateRow(i+2);
            //        row.CreateCell(0).SetCellValue($"{list[i].CITYNAME}");
            //        row.CreateCell(1).SetCellValue($"{list[i].CITYCODE}");

            //    }

            //    var saveDialog = new SaveFileDialog
            //    {
            //        DefaultExt = "xls",
            //        Filter = @"Excel文件|*.xls;*.xlsx",
            //        FileName = saveFileName
            //    };
            //    saveDialog.ShowDialog(); saveFileName = saveDialog.FileName;
            //    if (saveFileName.IndexOf(":", StringComparison.Ordinal) < 0) return; //被点了取消

            //    if (saveFileName == "") return;

            //    try
            //    {
            //        fs = File.OpenWrite(saveDialog.FileName);
            //        workbook.Write(fs);
            //        XtraMessageBox.Show(@"下载成功！");
            //    }
            //    catch (Exception ex)
            //    {
            //       XtraMessageBox.Show(@"导出文件时出错,文件可能正被打开！\n" + ex.Message);
            //        _log.Error("导出文件时出错，文件可能正被打开，出错提示：" + ex.ToString());
            //    }
            //}
            //catch (Exception exception)
            //{
            //    //fs?.Close();
            //    GC.SuppressFinalize(this);
            //   XtraMessageBox.Show(@"下载失败！\n" + exception.Message);
            //    _log.Error("下载失败，出错提示：" + exception.ToString());
            //}
            //finally
            //{
            //    fs?.Close();

            //}
            #endregion
        }

        private void btn_open_Click(object sender, EventArgs e)
        {
            btn_close.Enabled = false;
            try
            {
                var f = new OpenFileDialog { Filter = @"Excel|*.xlsx;*.xls;" };
                if (f.ShowDialog() != DialogResult.OK)
                    return;
                txt_filepath.Text = f.FileName;
                btn_check.Enabled = true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                _log.Error("打开文件出错，出错提示：" + exception.ToString());
               XtraMessageBox.Show(@"打开文件出错！\n" + exception.Message);
                throw;
            }
        }

        private DataTable datatable;
        private void btn_check_Click(object sender, EventArgs e)
        {
            try
            {
                rich_result.Text = @"";
                //验证指定文件是否有效
                var str_error ="11";
                var ds = OutputFile.LoadDataFromExcel(txt_filepath.Text, ref str_error);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    rich_result.Text= $@"未发现有效数据，错误{str_error}提示";
                    return;
                }
                //循环读取table，判断是否有重复的选项

                var dt = ds.Tables[0];
                int row;
                for(int i=0;i< dt.Rows.Count;i++)
                {
                    row = i + 2;
                    if (dt.Rows[i][0].ToString()=="")
                    {
                        rich_result.Text += @"第"+ row + "行业类型列不能为空" + "\n";
                    }
                    if (dt.Rows[i][1].ToString() == "")
                    {
                        rich_result.Text += @"第" + row + "行设备名称列不能为空" + "\n";
                    }
                    if (dt.Rows[i][2].ToString() == "")
                    {
                        rich_result.Text += @"第" + row + "行区划编号列不能为空" + "\n";
                    }
                    if (dt.Rows[i][4].ToString() == "")
                    {
                        rich_result.Text += @"第" + row + "行出厂编号列不能为空" + "\n";
                    }
                }
                if (dt.DefaultView.ToTable(true, dt.Columns[4].ToString()).Rows.Count < dt.Rows.Count)
                {
                    rich_result.Text += @"出厂编号列存在重复项，请检查导入数据的正确性\n";
                    //可能是科学计数法导致，数字前加半角'
                }
                if (rich_result.Text=="")
                { 
                isCheck = true;
                btn_close.Enabled = true;
                datatable = dt;
                rich_result.Text += @"数据正常，可以执行导入" + "\n";
                }

            }
            catch (Exception exception)
            {
                //rich_result.Text += @"出现异常"+ exception;
                rich_result.Text += @"校验出错，请检查导入文件是否正确";
                _log.Error("校验出错，请检查导入文件是否正确，出错提示：" + exception.ToString());
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            var box = new XtraMessageBoxArgs();
            box.Caption = "提示";
            box.Text = "确定要导入吗？";
            box.Buttons = new DialogResult[] { DialogResult.OK, DialogResult.Cancel };
            box.Showing += ShowButton.Box_Showing;
            if (XtraMessageBox.Show(box) != DialogResult.OK)
            {
                return;
            }
            //批量导入数据

            var pro = new BaseConfigProgress
            {
                Topic = "device",
                datatable = datatable
            };
            if(pro.ShowDialog()!=DialogResult.OK)
                return;

            rich_result.Text += pro.logbuild.ToString();

            if(pro.logbuild==null)
            { 
            this.Parent.Visible = false;
            this.Parent.Controls.Remove(this.Parent.Controls["import"]);
            }
        }
    }
}
