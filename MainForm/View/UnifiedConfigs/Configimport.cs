using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using NPOI.HSSF.UserModel;
using SmartKylinApp.Common;
using log4net;

namespace SmartKylinApp.View.BaseConfig
{
    public partial class Configimport : DevExpress.XtraEditors.XtraUserControl
    {
        private ILog _log = LogManager.GetLogger("Configimport");
        public Configimport()
        {
            InitializeComponent();
        }

        private void btn_mb_Click(object sender, EventArgs e)
        {
            var dow=new Configdownload();
            if (dow.ShowDialog() != DialogResult.OK) return;}

        private void btn_ck_Click(object sender, EventArgs e)
        {
            //传感器填写说明
            FileStream fs = null;
            try
            {
                var saveFileName = "监测项信息填写说明.xls";
                fs = new FileStream(saveFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                var workbook = new HSSFWorkbook();
                var sheet = workbook.CreateSheet();

                sheet.SetColumnWidth(0, 20 * 256);
                var headerRow = sheet.CreateRow(0);
                headerRow.Height = 300;
                headerRow.CreateCell(0).SetCellValue($"1、传感器编码不可为空。");
                sheet.CreateFreezePane(0, 1, 0, 1);

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
                    XtraMessageBox.Show(@"下载成功（第一条数据为示例数据，请手动删除）！");
                }
                catch (Exception ex)
                {
                   XtraMessageBox.Show(@"导出文件时出错,文件可能正被打开！\n" + ex.Message);
                    _log.Error("导出文件时出错，文件可能正被打开，出错提示：" + ex.ToString());
                }
            }
            catch (Exception exception)
            {
               XtraMessageBox.Show(@"下载失败！\n" + exception.Message);
                _log.Error("下载失败，出错提示：" + exception.ToString());
            }
            finally
            {
                fs?.Close();
              
            }
        }

        private void btn_open_Click(object sender, EventArgs e)
        {
            //数据导入
            btn_close.Enabled = false;
            btn_check.Enabled = false;
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
               XtraMessageBox.Show(@"打开文件出错！\n" + exception.Message);
                _log.Error("打开文件出错，出错提示：" + exception.ToString());
                Console.WriteLine(exception);
                throw;
            }
            
        }

        private bool isCheck = false;
        private DataTable datatable;
        private void btn_check_Click(object sender, EventArgs e)
        {
            //检查导入的文件是够正确
            try
            {
                rich_result.Text = "";
                var str_error = "11";
                var ds = OutputFile.LoadDataFromExcel(txt_filepath.Text, ref str_error);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    rich_result.Text = $@"未发现有效数据，错误{str_error}提示";
                }

                var dt = ds.Tables[0];
                if (dt.Columns.Count != 44)
                {
                    XtraMessageBox.Show("打开的文本格式不正确");
                    return;
                }
                else
                {
                    int row;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        row = i + 2;
                        if (dt.Rows[i][1].ToString() == "")
                        {
                            rich_result.Text += @"第" + row + "行监测点编码列不能为空" + "\n";
                        }
                        if (dt.Rows[i][2].ToString() == "")
                        {
                            rich_result.Text += @"第" + row + "行监测项编码列不能为空" + "\n";
                        }
                        if (dt.Rows[i][3].ToString() == "")
                        {
                            rich_result.Text += @"第" + row + "行监测项描述列不能为空" + "\n";
                        }
                    }
                    //if (dt.DefaultView.ToTable(true, dt.Columns[2].ToString()).Rows.Count < dt.Rows.Count)
                    //{
                    //    {
                    //        rich_result.Text += @"监测项编码列存在重复项，请检查导入数据的正确性" + "\n";
                    //    }
                    //}
                    if (rich_result.Text == "")
                    {
                        isCheck = true;
                        btn_close.Enabled = true;
                        datatable = dt;
                        rich_result.Text += @"数据正常，可以执行导入" + "\n";
                    }
                }
            }
            catch (Exception exception)
            {
                //rich_result.Text += $"传感器信息校验出错{exception}行";
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
            rich_result.Text = "";
            var pro = new BaseConfigProgress
            {
                Topic = "config",
                datatable = datatable
            };
            if (pro.ShowDialog() != DialogResult.OK)
                return;

            rich_result.Text += pro.logbuild.ToString();

            if (pro.logbuild == null)
            {
                this.Parent.Visible = false;
                this.Parent.Controls.Remove(this.Parent.Controls["import"]);
            }
        }
    }
}
