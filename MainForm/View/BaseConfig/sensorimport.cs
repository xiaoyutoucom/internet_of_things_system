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
    public partial class sensorimport : DevExpress.XtraEditors.XtraUserControl
    {
        private ILog _log = LogManager.GetLogger("sensorimport");
        public sensorimport()
        {
            InitializeComponent();
        }

        private void btn_mb_Click(object sender, EventArgs e)
        {
            var dow=new sensortemdownload();
            if (dow.ShowDialog() != DialogResult.OK) return;}

        private void btn_ck_Click(object sender, EventArgs e)
        {
            XtraMessageBox.Show("1、一个设备有一个或者多个传感器。\n2、新增一条传感器信息需要复制一条设备编码信息。\n3、进行严格的检验，尽量避免出现重复数据。\n4、传感器的编号根据实际的传感器采集服务进行配置。");
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
                Console.WriteLine(exception);
                XtraMessageBox.Show("打开文件出错");
                _log.Error("打开文件出错，出错提示：" + exception.ToString());
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
                rich_result.Text = @"";
                var str_error = "11";
                var ds = OutputFile.LoadDataFromExcel(txt_filepath.Text, ref str_error);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    rich_result.Text = $@"未发现有效数据，错误{str_error}提示";
                    return;
                }

                var dt = ds.Tables[0];
                if (dt.Columns.Count != 8)
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
                            rich_result.Text += @"第" + row + "行设备编码列不能为空" + "\n";
                        }
                        if (dt.Rows[i][2].ToString() == "")
                        {
                            rich_result.Text += @"第" + row + "行传感器编码列不能为空" + "\n";
                        }
                        if (dt.Rows[i][3].ToString() == "")
                        {
                            rich_result.Text += @"第" + row + "行传感器名称列不能为空" + "\n";
                        }
                    }
                    //判断同一设备变化是够有两个相同的传感器编号
                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //    if (i == dt.Rows.Count - 1) continue;
                    //    if (dt.Rows[i].ItemArray.GetValue(1) + "_" + dt.Rows[i].ItemArray.GetValue(3) ==
                    //        dt.Rows[i + 1].ItemArray.GetValue(1) + "_" + dt.Rows[i + 1].ItemArray.GetValue(3))
                    //    {
                    //        //存在重复的传感器编号
                    //        rich_result.Text += $"存在重复的传感器信息{i}行";
                    //        //return;
                    //    }
                    //    else
                    //    {
                    //        isCheck = true;
                    //    }
                    //}
                    if(rich_result.Text=="")
                    { 
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
            var pro = new BaseConfigProgress
            {
                Topic = "sensor",
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
