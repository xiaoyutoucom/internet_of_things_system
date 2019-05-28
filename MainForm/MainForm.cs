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
using SmartKylinApp.View.Cache;
using SmartKylinApp.View;
using SmartKylinApp.View.BaseConfig;
using SmartKylinApp.View.Query;
using SmartKylinApp;
using DevExpress.XtraBars;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace MainForm
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        private ComprehensiveDisplay comprehensive;

        public MainForm()
        {
           
            InitializeComponent();
           
            Load += MainForm_Load;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            GlobalHandler.ControlContainer = MainContainer;
            Width = 1180;
            Height = 700;

            var frm = new Login();
            if (frm.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            //splashScreenManager1.ShowWaitForm();
            //splashScreenManager1.SetWaitFormCaption("请稍后,数据加载中....");     // 标题
            //splashScreenManager1.SetWaitFormDescription("正在更新.....");　　
            barStatictime.Caption = @"当前时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// 
            var time = new Timer();
            time.Interval = 1000;
            time.Enabled = true;
            time.Tick += Time_Tick;

            //初始化皮肤
            var Skin = ConfigHelp.Config["Application:Setting:Skin"];
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(Skin);
            Text = @"正元物联网数据配置平台";
            StartPosition = FormStartPosition.CenterScreen;
            //ribbon_unifiedConfig.Visible = GlobalHandler.Auth;
            //显示实时时间，时分秒        
            //GlobalHandler.AddControl(new ComprehensiveDisplay());
            //barItem_cityInfo.Caption = GlobalHandler.LocalInfo;
            //splashScreenManager1.CloseWaitForm();
            GlobalHandler.CurrentSkin = defaultLookAndFeel1.LookAndFeel.SkinName;
            SkinLoad();
            //GlobalHandler.AddControl(new ComprehensiveDisplay());
            comprehensive = new ComprehensiveDisplay();
            comprehensive.Dock = DockStyle.Fill;
            GlobalHandler.AddControl(comprehensive);

        }

        private void SkinLoad()
        {
            foreach (DevExpress.Skins.SkinContainer skin in DevExpress.Skins.SkinManager.Default.Skins)
            {
                //barSubItem3.Properties.Items.Add(skin.SkinName);
                BarButtonItem bt = new BarButtonItem();
                Bitmap b1 = new Bitmap(12, 12); //新建位图b1
                Graphics g1 = Graphics.FromImage(b1); //创建b1的Graphics

                if (skin.SkinName == "DevExpress Style")
                {
                    bt.Caption = "默认风格";
                    g1.FillRectangle(Brushes.White, new Rectangle(0, 0, 12, 12)); //把b1涂成红色
                    bt.ImageOptions.Image = b1;
                    
                }
                else if (skin.SkinName == "DevExpress Dark Style")
                {
                    bt.Caption = "黑色风格";
                    g1.FillRectangle(Brushes.Black, new Rectangle(0, 0, 12, 12)); //把b1涂成红色
                    bt.ImageOptions.Image = b1;
                }
                else if (skin.SkinName == "Office 2016 Dark")
                {
                    bt.Caption = "灰色风格";
                    g1.FillRectangle(Brushes.Gray, new Rectangle(0, 0, 12, 12)); //把b1涂成红色
                    bt.ImageOptions.Image = b1;
                }
                else if (skin.SkinName == "Office 2010 Blue")
                {
                    bt.Caption = "淡蓝风格";
                    g1.FillRectangle(Brushes.LightBlue, new Rectangle(0, 0, 12, 12)); //把b1涂成红色
                    bt.ImageOptions.Image = b1;
                }
                else if (skin.SkinName == "Office 2007 Blue")
                {
                    bt.Caption = "蓝色风格";
                    g1.FillRectangle(Brushes.Blue, new Rectangle(0, 0, 12, 12)); //把b1涂成红色
                    bt.ImageOptions.Image = b1;
                }
                else if (skin.SkinName == "Office 2007 Silver")
                {
                    bt.Caption = "银色风格";
                    g1.FillRectangle(Brushes.Silver, new Rectangle(0, 0, 12, 12)); //把b1涂成红色
                    bt.ImageOptions.Image = b1;
                }
                else if (skin.SkinName == "Office 2007 Green")
                {
                    bt.Caption = "绿色风格";
                    g1.FillRectangle(Brushes.Green, new Rectangle(0, 0, 12, 12)); //把b1涂成红色
                    bt.ImageOptions.Image = b1;
                }
                else if (skin.SkinName == "Office 2007 Pink")
                {
                    bt.Caption = "粉色风格";
                    g1.FillRectangle(Brushes.Pink, new Rectangle(0, 0, 12, 12)); //把b1涂成红色
                    bt.ImageOptions.Image = b1;
                }
                else if (skin.SkinName == "Darkroom")
                {
                    bt.Caption = "暗室风格";
                    g1.FillRectangle(Brushes.DarkGray, new Rectangle(0, 0, 12, 12)); //把b1涂成红色
                    bt.ImageOptions.Image = b1;
                }
                else
                {
                    continue;
                }
                bt.Name = skin.SkinName;
                bt.AllowRightClickInMenu = false;
                barSkin.AddItem(bt);
                bt.ItemClick += barSkin_ItemClick;
            }
        }
        private void barSkin_ItemClick(object sender, ItemClickEventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            splashScreenManager1.SetWaitFormCaption("请稍后,正在更换皮肤....");     // 标题
            //splashScreenManager1.SetWaitFormDescription("正在更新.....");　　
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(e.Item.Name);
            //保存皮肤修改
            var filePath = Directory.GetCurrentDirectory() + "\\appsettings.json";
            dynamic jObject = JObject.Parse(File.ReadAllText(filePath), new JsonLoadSettings() { CommentHandling = CommentHandling.Load });
            var config = jObject;
            config.Application.Setting.Skin = e.Item.Name;
            var ob = JsonConvert.SerializeObject(config, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
            File.WriteAllText(filePath, ob);
            splashScreenManager1.CloseWaitForm();

        }
        private void Time_Tick(object sender, EventArgs e)
        {
            barStatictime.Caption = @"当前时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
           // barHeaderItem1.Caption = @"当前时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalHandler.AddControl(new ComprehensiveDisplay());
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MainContainer.Controls.Clear();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalHandler.AddControl(new HistoryQuery());
        }
        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalHandler.AddControl(new AlertQuery());
        }
        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalHandler.AddControl(new RuntimeQuery());
        }
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //打开帮助文档
            //Confighelp frm = new Confighelp();
            //frm.StartPosition = FormStartPosition.CenterScreen;
            //if (frm.ShowDialog() != DialogResult.OK) return;
            string filePath = "帮助文档.chm"; //martKylinApp.Data.city.json"
            //string filePath = "../../../Data/帮助文档.chm"; //martKylinApp.Data.city.json"
            if (!File.Exists(filePath))
            {
                XtraMessageBox.Show("帮助文档不存在!");
                return;
            }
            Help.ShowHelp(this, "帮助文档.chm");//打开debug文件夹下的chm文件
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //打开配置
            Config frm = new Config();
            frm.StartPosition = FormStartPosition.CenterScreen;
            if (frm.ShowDialog() != DialogResult.OK) return;
        }
        private void barButtonItem16_ItemClick(object sender, ItemClickEventArgs e)
        {
            //打开配置
            Confighelp frm = new Confighelp();
            frm.StartPosition = FormStartPosition.CenterScreen;
            if (frm.ShowDialog() != DialogResult.OK) return;
        }
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //更新缓存CacheManger
            var frm = new CacheManger();
            if (frm.ShowDialog() != DialogResult.OK) return;
        }
        //同一配置
        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalHandler.AddControl(new UnifiedConfig());
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            //MessageBox.Show("公司名称：正元地理信息股份有限公司\n\n公司地址：北京市顺义区机场东路2号国门商务区正元地理信息大厦\n\n联系方式：4006016696\n\n传真：+86 - 010 - 53296200\n版本号:v1\n\n");
            AboutSystem from = new AboutSystem();
            from.StartPosition = FormStartPosition.CenterScreen;
            from.ShowDialog();
        }

        private void toolTipController1_BeforeShow(object sender, DevExpress.Utils.ToolTipControllerShowEventArgs e)
        {
            //禁用鼠标悬浮title
            e.Show = false;   
        }
        //数据总览
        private void barButtonItem17_ItemClick(object sender, ItemClickEventArgs e)
        {
            
            GlobalHandler.AddControl(comprehensive);
        }
        /// <summary>
        /// 短息管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem18_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalHandler.AddControl(new SMSConfig());
        }
    }
}