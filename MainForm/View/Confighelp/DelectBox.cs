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

namespace SmartKylinApp.View.BaseConfig
{
    public partial class DelectBox : DevExpress.XtraEditors.XtraForm
    {
        private bool ifdelect=false;

        public DelectBox()
        {
            InitializeComponent();
            //禁止右键菜单
            //layoutControl1.AllowCustomization = false;
        }

        public bool IfDelect {
            get { return ifdelect; }
            set { ifdelect = value; } }

        private void DelectBox_Load(object sender, EventArgs e)
        {
           

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string pass=ConfigHelp.Config["Application:Config:Power"];
            if (tex_pass.Text=="")
            {
                XtraMessageBox.Show("请先输入密码！");
                return;
            }
            if(pass == tex_pass.Text)
            {
                ifdelect = true;
                this.Close();
            }
            else
            {
                ifdelect = false;
                XtraMessageBox.Show("密码错误请重试！");
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}