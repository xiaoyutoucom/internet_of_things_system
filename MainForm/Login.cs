using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using log4net;
using Microsoft.Extensions.Configuration;
using Robin;
using SmartKylinApp.Common;
using SmartKylinApp.Module;
using SmartKylinData.BaseModel;
using SmartKylinData.Interface;

namespace SmartKylinApp
{
    public partial class Login : XtraForm
    {
        private readonly ILog _log = LogManager.GetLogger("SmartKylinApp");

        //[DllImport("user32.dll")]
        //public static extern bool ReleaseCapture();

        //[DllImport("user32.dll")]
        //public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private static readonly IConfigurationRoot config = ConfigHelp.Config;

        private readonly Verification verif = new Verification
        {
            DbName = config["Application:Config:DbType"],
            DbConn = config["Application:Config:DbConn"],
            SignalR = config["Application:Config:Signalr"],
            Redis = config["Application:Config:Redis"],
            MongoDb = config["Application:Config:MongoDb"]
        };

        private readonly string PassWord = ConfigHelp.Config["Application:Config:PassWord"];
        private readonly string UserName = ConfigHelp.Config["Application:Config:UserName"];

        public Login()
        {
            InitializeComponent();
        }

        //更新UI
        private void UpdataUIStatus(int step)
        {
            progressBarControl1.Position += step;
        }

        //完成任务时需要调用
        private void Accomplish()
        {
            Hide();
        }

        //初始化数据
        private void loadData()
        {
            var boot = RobinBootstrapper.Create<BootstrapMoudle>();
            boot.Initialize();
            GlobalHandler.Bootstrapper = boot;

            var msType = GlobalHandler.Bootstrapper.IocManager.Resolve<IMsType>();
            var bindlist = new BindingList<TableModel>();

            GlobalHandler.resourList = bindlist;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {


            try
            {
                //验证用户名密码是否正确

                if (UserName != txt_name.Text || PassWord != txt_password.Text)
                {
                    XtraMessageBox.Show("用户名或密码错误!");
                    return;
                }
                progressBarControl1.Visible = true;
                UpdataUIStatus(30);
                //首先执行数据库等验证，验证不通过则取消登录
                VerifExt();
                //&& verif.SignaleState
                if (verif.DBConnState == Verification.DbState.Connected && verif.RedisState  && verif.MongoState)
                {
                    //验证通过
                    loadData();
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    //MessageBox.Show("数据验证不通过，请检查系统配置");
                    var box = new XtraMessageBoxArgs();
                    box.Caption = "温馨提示";
                    box.Text = "数据验证失败，请重试或者检查系统配置";
                    box.Buttons = new[] {DialogResult.OK, DialogResult.Retry};
                    box.Showing += Box_Showing;
                    if (XtraMessageBox.Show(box) != DialogResult.OK)
                    {
                        VerifExt();
                        if (verif.DBConnState == Verification.DbState.Connected && verif.RedisState &&
                            verif.SignaleState && verif.MongoState)
                        {
                            //验证通过
                            loadData();
                            UpdataUIStatus(100);
                            DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            progressBarControl1.Position = 0;
                            progressBarControl1.Visible = false;
                            XtraMessageBox.Show("验证失败，请修检查配置!");
                            var frm = new Config();
                            if (frm.ShowDialog() != DialogResult.OK) return;
                        }
                    }
                    else
                    {
                        progressBarControl1.Position = 0;
                        progressBarControl1.Visible = false;
                        var frm = new Config();
                        if (frm.ShowDialog() != DialogResult.OK) return;
                    }
                }
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show("失败");
                _log.Error("登陆失败：" + exception);
                progressBarControl1.Position = 0;
                progressBarControl1.Visible = false;
            }
        }

        private void VerifExt()
        {
            verif.upProgress += UpdataUIStatus;
            verif.Execute();
        }

        private void Box_Showing(object sender, XtraMessageShowingArgs e)
        {
            foreach (var control in e.Form.Controls)
            {
                var button = control as SimpleButton;
                if (button != null)
                    switch (button.DialogResult.ToString())
                    {
                        case "OK":
                            button.Text = "配置";
                            break;
                        case "Retry":
                            button.Text = "重试";
                            break;
                    }
            }
        }
        private void btn_close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void link_config_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var frm = new Config();
            if (frm.ShowDialog() != DialogResult.OK) return;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            //记住密码ui显示，未更新到配置文档
            if (checkBox1.Checked)
            {
                txt_name.Text = UserName;
                txt_password.Text = PassWord;
            }

            //水平进度
            //最大 最小值
            progressBarControl1.Properties.Maximum = 100;
            progressBarControl1.Properties.Minimum = 0;
            progressBarControl1.Position = 0; //当前值
            progressBarControl1.Properties.ShowTitle = true; //是否显示进度数据
            //是否显示百分比
            progressBarControl1.Properties.PercentView = false;
          
            txt_password.ForeColor = Color.FromArgb(135, 177, 253);
            txt_password.BackColor = Color.FromArgb(35, 32, 183);
            txt_password.Properties.Appearance.BorderColor = Color.FromArgb(135, 177, 253);
            txt_password.Properties.AppearanceDisabled.BorderColor = Color.FromArgb(135, 177, 253);
            txt_password.Properties.AppearanceFocused.BorderColor = Color.FromArgb(135, 177, 253);
            txt_password.Properties.AppearanceReadOnly.BorderColor = Color.FromArgb(135, 177, 253);
            txt_name.ForeColor = Color.FromArgb(135, 177, 253);
            txt_name.BackColor = Color.FromArgb(35, 32, 183);
            txt_name.Properties.Appearance.BorderColor = Color.FromArgb(135, 177, 253);
            txt_name.Properties.AppearanceDisabled.BorderColor = Color.FromArgb(135, 177, 253);
            txt_name.Properties.AppearanceFocused.BorderColor = Color.FromArgb(135, 177, 253);
            txt_name.Properties.AppearanceReadOnly.BorderColor = Color.FromArgb(135, 177, 253);
        }

        private void link_config_LinkClicked(object sender, EventArgs e)
        {
            var frm = new Config();
            if (frm.ShowDialog() != DialogResult.OK) return;
        }


        private void txt_name_Click(object sender, EventArgs e)
        {
            if (txt_name.Text == "请输入用户名") txt_name.Text = "";
        }

        private void txt_password_Click(object sender, EventArgs e)
        {
            if (txt_password.Text == "请输入用户名") txt_password.Text = "";
        }
    }
}