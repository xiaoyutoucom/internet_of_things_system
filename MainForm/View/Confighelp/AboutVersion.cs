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
    public partial class AboutVersion : DevExpress.XtraEditors.XtraForm
    {
        public AboutVersion()
        {
            InitializeComponent();
            //禁止右键菜单
            //layoutControl1.AllowCustomization = false;
        }

        private void AboutVersion_Load(object sender, EventArgs e)
        {
            lbv3.Text = "系统版本:V1.1";
            lbt3.Text = "更新时间:2018-12-28";       
            lbu3.Text = "更新内容:1、修改数据总览界面显示样式\n\n2、增加总览界面现实的数据，可对显示的监测类型进行配置\n\n3、可手动控制数据刷新也可对系统自动刷新进行开关和配置\n\n4、增加弹窗预警功能\n\n5、总览数据详细数据增加监测点类型和监测项编码数据列";
            lbv2.Text = "系统版本:V1.2";
            lbt2.Text = "更新时间:2019-02-16";
            lbu2.Text = "更新内容:1、增加短信模版配置、短信配置、短信发送和短信记录功能\n\n2、修改更新缓存出现的问题3、导入监测项和倒入设备导入功能问题解决";
            lbv1.Text = "系统版本:V1.3";
            lbt1.Text = "更新时间:2019-05-19";
            lbu1.Text = "更新内容:1、启动速度和更新速度进行优化\n\n2、修改数据总览数据不匹配的问题\n\n3、总览数据井盖数据合并规则修改";
            lbv4.Text = "当前版本:V1.4";
            lbt4.Text = "更新时间:2019-06-11";
            lbu4.Text = "更新内容:1、对版本更新页面进行修改\n\n2、主要数据的删除功能增加校验功能\n\n3、更新缓存的更新和保存按钮增加校验功能\n\n3、更新缓存类别下拉框动态获取\n\n4、更新和保存错误提示修改并记入日志";

        }
    }
}