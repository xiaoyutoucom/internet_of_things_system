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
    public partial class AboutSystem : DevExpress.XtraEditors.XtraForm
    {
        public AboutSystem()
        {
            InitializeComponent();
            //禁止右键菜单
            layoutControl1.AllowCustomization = false;
        }

        private void AboutSystem_Load(object sender, EventArgs e)
        {
            label3.Text = "系统介绍：本系统可以实现数据统一配置、数据查询、系统配置、更新缓存、皮肤风格修改等功\n\n能，方便快捷的录入和查询设备、传感器等基础信息。";
            var Version = ConfigHelp.Config["Application:Setting:Version"];
            label5.Text += Version;
            label2.Text = "警告：本计算机程序受著作权法和国际条约保护，未经授权而擅自复制或传播本程序，将受到严\n\n厉的民事制裁，并将在法律许可范围内受到最大程度的起诉。";
            var VersionDate = ConfigHelp.Config["Application:Setting:VersionDate"];
            label4.Text += VersionDate;

        }
    }
}