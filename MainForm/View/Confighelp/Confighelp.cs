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
using log4net;
using System.Collections;

namespace SmartKylinApp.View.BaseConfig
{
    public partial class Confighelp : DevExpress.XtraEditors.XtraForm
    {
        public Confighelp()
        {
            InitializeComponent();
            //禁止右键菜单
            layoutControl1.AllowCustomization = false;
            
        }
        private ILog _log = LogManager.GetLogger("Confighelp");
        private void Confighelp_Load(object sender, EventArgs e)
        {
            try
            {
                var list = GlobalHandler.mstyperesp.GetAllList(a => a.TYPE_KEY.Length == 2);
                gridControl1.DataSource = list;
                txt_time.Text = ConfigHelp.Config["Application:Config:Time"];
                BindCheck();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("初始化数据出错");
                _log.Error("初始化数据出错，出错提示：" + ex.ToString());
            }
        }
        private void BindCheck()
        {
            try
            {
                //初始化选择的监测点
                var Type = ConfigHelp.Config["Application:Config:Type"];
                
                ArrayList _arr = new ArrayList();
                string[] str = Type.Split(',');
                for (int i = 0; i < str.Length; i++)
                {
                    _arr.Add(str[i].Split(':')[1]);
                }
                for (int j = 0; j < gridView1.RowCount; j++)
                {
                    var TYPE_KEY = gridView1.GetRowCellValue(j, "TYPE_KEY").ToString();
                    if (_arr.Contains(TYPE_KEY))
                    {
                        gridView1.SelectRow(j);
                    }
                }
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("初始化默认选中数据出错");
                _log.Error("初始化默认选中数据出错，出错提示：" + e.ToString());
            }
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                //保存
                int[] rownumber = this.gridView1.GetSelectedRows();//获取选中行号；
                if (rownumber.Length < 1 && gridView1.RowCount == 0)
                {
                    XtraMessageBox.Show("请勾选监测项类型！");
                    return;
                }
                if (rownumber.Length > 10)
                {
                    XtraMessageBox.Show("选择类型不能大于10个！");
                    return;
                }
                string TYPE_KEY = string.Empty, TYPE_NAME = string.Empty, Type = string.Empty;
                for (int i=0;i< rownumber.Length;i++)
                {
                    TYPE_KEY = gridView1.GetRowCellValue(rownumber[i], "TYPE_KEY").ToString();
                    TYPE_NAME = gridView1.GetRowCellValue(rownumber[i], "TYPE_NAME").ToString();
                    Type += TYPE_NAME + ":" + TYPE_KEY;
                    if(i != rownumber.Length-1)
                    {
                        Type += ",";
                    }
                }
                ConfigHelp.WriteType(Type);
                ConfigHelp.WriteTime(txt_time.Text);
                XtraMessageBox.Show("修改完成，重新启动后生效");
                Application.Exit();
                System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
            } catch (Exception ex)
            {
                XtraMessageBox.Show("保存数据出错");
                _log.Error("保存数据出错，出错提示：" + ex.ToString());
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //取消
            this.Close();
        }
    }
}