using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraSplashScreen;
using SmartKylinApp;
using SmartKylinApp.Common;
using SmartKylinApp.View;
using SmartKylinApp.View.BaseConfig;
using SmartKylinApp.View.Cache;
using SmartKylinApp.View.ComprehensiveDocument;
using SmartKylinApp.View.Query;

namespace MainForm
{
    public sealed partial class MainForm1 : RibbonForm
    {
        public MainForm1()
        {
            InitializeComponent();        
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            GlobalHandler.ControlContainer = MainContainer;
            Width = 1180;
            Height = 700;
            var time = new Timer();
            time.Interval = 1000;
            time.Enabled = true;
            time.Tick += Time_Tick;
            Load += MainForm_Load;
            var frm = new Login();
            if (frm.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            else
            {
                var pfrm = new Progressing();
                if (pfrm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

            }
            Text = @"正元物联网信息配置平台";
            StartPosition = FormStartPosition.CenterScreen;
            // ribbon_unifiedConfig.Visible = GlobalHandler.Auth;
            //显示实时时间，时分秒
            GlobalHandler.AddControl(new ComprehensiveDisplay());
            barItem_cityInfo.Caption = GlobalHandler.LocalInfo;
            GlobalHandler.CurrentSkin = defaultLookAndFeel1.LookAndFeel.SkinName;

        }
        private void Time_Tick(object sender, EventArgs e)
        {
            barItem_runingtime.Caption = @"当前时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void barBtn_DataView_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalHandler.AddControl(new ComprehensiveDisplay());
        }

        private void barBtn_dataMonitor_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MainContainer.Controls.Clear();
        }

        private void ribbonControl1_SelectedPageChanged(object sender, EventArgs e)
        {
            var page = (RibbonControl) sender;

            if (page.SelectedPage.Name == "ribbon_unifiedConfig")
            {
               // ribbonControl1.Minimized = true;
                GlobalHandler.AddControl(new UnifiedConfig());
            }
            else
            {
                ribbonControl1.Minimized = false;
            }
        }

        private void skinRibbonGalleryBarItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
        }

        private void skinRibbonGalleryBarItem2_GalleryItemClick(object sender, GalleryItemClickEventArgs e)
        {
            GlobalHandler.CurrentSkin = skinRibbonGalleryBarItem2.Gallery.GetCheckedItem().Caption;
        }

        private void btn_updatacache_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //更新缓存CacheManger
            var frm = new CacheManger();
            if (frm.ShowDialog() != DialogResult.OK) return;
        }

        private void btn_confighelp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //打开帮助文档
            Confighelp frm = new Confighelp();
            frm.StartPosition = FormStartPosition.CenterScreen;
            if (frm.ShowDialog() != DialogResult.OK) return;
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //打开配置
            Config frm = new Config();
            frm.StartPosition = FormStartPosition.CenterScreen;
            if (frm.ShowDialog() != DialogResult.OK) return;
        }

        private void barBtn_Classify_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalHandler.AddControl(new HistoryQuery());
        }
    }
    
}
