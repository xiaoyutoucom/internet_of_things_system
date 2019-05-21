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
using Org.BouncyCastle.Math.EC;
using SmartKylinApp.Common;
using SmartKylinApp.View.BaseConfig;
using SmartKylinApp.View.UnifiedConfigs;
using SmartKylinApp.View.MsgConfig;
using SmartKylinApp.View.LedConfig;

namespace SmartKylinApp.View
{
    public partial class UnifiedConfig : DevExpress.XtraEditors.XtraUserControl
    {
        public UnifiedConfig()
        {
            InitializeComponent();

            navBarControl.PaintStyleName =(string) GlobalHandler.CurrentSkin;
        }
        void navBarControl_ActiveGroupChanged(object sender, DevExpress.XtraNavBar.NavBarGroupEventArgs e)
        {
            navigationFrame.SelectedPageIndex = navBarControl.Groups.IndexOf(e.Group);
        }

        void barButtonNavigation_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
        }

        private void nav_agreement_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            var control = new agreement();
            configPanel.Controls.Clear();
            configPanel.Controls.Add(control);
            control.Dock = DockStyle.Fill;
        }

        private void navBarItem2_ItemChanged(object sender, EventArgs e)
        {
          
        }

        private void navBarItem2_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            var control = new deviceinfo();
            configPanel.Controls.Clear();
            configPanel.Controls.Add(control);
            control.Dock = DockStyle.Fill;
        }

        private void navBarItem3_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            var control = new sensormanager();
            configPanel.Controls.Clear();
            configPanel.Controls.Add(control);
            control.Dock = DockStyle.Fill;
        }

        private void navBarItem4_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //监测点类型
            var control=new MstypeManager();
            configPanel.Controls.Clear();
            configPanel.Controls.Add(control);
            control.Dock = DockStyle.Fill;
        }

        private void navBarItem5_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //监测项类型
            var control = new TaginfoManager();
            configPanel.Controls.Clear();
            configPanel.Controls.Add(control);
            control.Dock = DockStyle.Fill;
        }

        private void navBarItem6_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //监测点
            var control = new MonitorManager();
            configPanel.Controls.Clear();
            configPanel.Controls.Add(control);
            control.Dock = DockStyle.Fill;
        }
        private void navBarItem7_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //监测项
            var control = new ConfigManager();
            configPanel.Controls.Clear();
            configPanel.Controls.Add(control);
            control.Dock = DockStyle.Fill;
        }

        private void navBarItem8_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //联系人
            var control = new ContactManager();
            configPanel.Controls.Clear();
            configPanel.Controls.Add(control);
            control.Dock = DockStyle.Fill;
        }

        private void navBarItem9_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //短信模板
            var control = new TemplateManager();
            configPanel.Controls.Clear();
            configPanel.Controls.Add(control);
            control.Dock = DockStyle.Fill;
        }

        private void navBarItem1_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //关联监测点
            var control = new ContactLinkMonitor();
            configPanel.Controls.Clear();
            configPanel.Controls.Add(control);
            control.Dock = DockStyle.Fill;
        }

        private void navBarItem11_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //关联LED信息
            var control = new LedManager();
            configPanel.Controls.Clear();
            configPanel.Controls.Add(control);
            control.Dock = DockStyle.Fill;
        }

        private void navBarItem10_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //关联字典
            var control = new FontLibrary();
            configPanel.Controls.Clear();
            configPanel.Controls.Add(control);
            control.Dock = DockStyle.Fill;
        }

        private void navBarItem12_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //关联视频点
            var control = new VideoManager();
            configPanel.Controls.Clear();
            configPanel.Controls.Add(control);
            control.Dock = DockStyle.Fill;
        }

        private void navBarItem13_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //LES关联检测点
            var control = new LedLinkMonitor();
            configPanel.Controls.Clear();
            configPanel.Controls.Add(control);
            control.Dock = DockStyle.Fill;
        }

        private void navBarItem14_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //视频关联检测点
            var control = new VidieoLinkMonitor();
            configPanel.Controls.Clear();
            configPanel.Controls.Add(control);
            control.Dock = DockStyle.Fill;
        }

        private void UnifiedConfig_Load(object sender, EventArgs e)
        {
            var control = new deviceinfo();
            configPanel.Controls.Clear();
            configPanel.Controls.Add(control);
            control.Dock = DockStyle.Fill;
        }

       /// <summary>
       /// 积水点关联泵站
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void navBarItem15_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            var control = new WaterLink();
            configPanel.Controls.Clear();
            configPanel.Controls.Add(control);
            control.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// 雨量站关联积水点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void navBarItem16_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            var control = new RainWaterForm();
            configPanel.Controls.Clear();
            configPanel.Controls.Add(control);
            control.Dock = DockStyle.Fill;
        }
    }
}
