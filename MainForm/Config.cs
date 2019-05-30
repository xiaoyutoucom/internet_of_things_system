using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Microsoft.Extensions.Configuration;
using SmartKylinApp.Common;

namespace SmartKylinApp
{
    public partial class Config : XtraForm
    {
        public Config()
        {
            InitializeComponent();
            config = ConfigHelp.Config;
            pro_verif.Location = new Point(Width / 2 - pro_verif.Width / 2, Height / 3);
            pro_verif.Visible = false;
        }

        private readonly IConfigurationRoot config;

        private void btn_check_Click(object sender, EventArgs e)
        {
            pro_verif.Visible = true;
            //执行验证
            var verif = new Verification
            {
                DbName = cob_db.Text,
                DbConn = txt_dbstring.Text,
                //SignalR = txt_signalr.Text,
                Redis = txt_redis.Text,
                MongoDb = txt_mongodb.Text
            };
            verif.Execute();
            error_provider.ClearErrors();
            //数据库验证
            state_db.Visible = verif.DBConnState == Verification.DbState.Connected;
            if (!state_db.Visible)
            {
                error_provider.SetError(txt_dbstring, "数据库连接错误");
            }

            //socket验证
            state_signalr.Visible = verif.SignaleState;
            if (!state_signalr.Visible)
            {

                error_provider.SetError(txt_signalr, "数据接收服务连接错误");
            }

            ////redis验证
            //state_redis.Visible = verif.RedisState;
            //if (!state_redis.Visible)
            //{

            //    error_provider.SetError(txt_redis, "redis数据库连接错误");
            //}

            ////mongo验证
            //state_mongodb.Visible = verif.MongoState;
            //if (!state_mongodb.Visible)
            //{
       
            //    error_provider.SetError(txt_mongodb, "MongoDB数据库连接错误");
            //}
            //|| !state_signalr.Visible
            if (!state_db.Visible)//|| !state_redis.Visible|| !state_mongodb.Visible
            {
                btn_ok.Enabled = false;
            }
            else
            {
                btn_ok.Enabled = true;
            }

            //区划验证
            state_city.Visible = !search_city.EditValue.ToString().Contains("0000");
            if (!state_city.Visible) error_provider.SetError(txt_city, "区划至少要选到市");
            pro_verif.Visible = false;
            ConfigHelp.WriteCity(search_city.EditValue.ToString());

            GlobalHandler.LocalInfo = txt_city.Text;

        }

        private void Config_Load(object sender, EventArgs e)
        {
            DbSetting();

            txt_signalr.Text = config["Application:Config:Signalr"] != ""
                ? config["Application:Config:Signalr"]
                : @"http://localhost:805";

            txt_redis.Text = config["Application:Config:Redis"] != ""
                ? config["Application:Config:Redis"]
                : @"localhost:6379,password=123";

            txt_mongodb.Text = config["Application:Config:MongoDb"] != ""
                ? config["Application:Config:MongoDb"]
                : @"220.179.52.231,zyCoredb";

            CityTimeSetting();
        }

        /// <summary>
        ///     数据库配置
        /// </summary>
        private void DbSetting()
        {
            var list = new List<DbModel>
            {
                new DbModel
                {
                    DbName = "Oracle",
                    DbString =
                        "Data Source=172.30.16.248:1521/orcl;User Id=ZYGIS;Password=ZYGIS;Integrated Security=no;"
                },
                new DbModel
                {
                    DbName = "PgSQL",
                    DbString = "Server=114.115.134.36;Port=5432;Database=ZYGIS;User Id=ZYGIS;Password=ZYGIS;"
                },
                new DbModel
                {
                    DbName = "MySQL",
                    DbString =
                        "Server=localhost;Database=drivetop_base; User=otnp80;Password=123;Use Procedure Bodies=false;"
                }
            };
            cob_db.DataSource = list;
            cob_db.SelectedIndex = 0;
            txt_dbstring.Text = cob_db.SelectedValue.ToString();
            cob_db.SelectedIndexChanged += Cob_db_SelectedIndexChanged;

            //判断配置信息
            if (config["Application:Config:DbType"] != null)
            {
                cob_db.Text = "";
                cob_db.SelectedText = config["Application:Config:DbType"];
                txt_dbstring.Text = config["Application:Config:DbConn"];
            }
        }

        /// <summary>
        ///     数据库选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cob_db_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_dbstring.Text = cob_db.SelectedValue.ToString();
        }


        /// <summary>     区划刷新时间配置
        /// </summary>
        private void CityTimeSetting()
        {
            var data = GlobalHandler.CityInfo;
            search_city.Properties.TreeList.DataSource = data;
            search_city.EditValueChanged += Search_city_EditValueChanged;
            search_city.EditValue = ConfigHelp.Config["Application:Config:City"] == ""
                ? "620422"
                : ConfigHelp.Config["Application:Config:City"];
        }

        /// <summary>
        ///     区划选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_city_EditValueChanged(object sender, EventArgs e)
        {
            var code = search_city.EditValue.ToString();
            var list = GlobalHandler.CityInfo;
            if(list==null) return;
            error_provider.ClearErrors();
            if (!code.Contains("0000"))
            {
                var aa = list.Find(a => a.CITYCODE == code);
                var bb = list.Find(a => a.CITYCODE == aa.PID);
                var cc = "";
                if (bb.PID.Contains("0000")) cc = list.Find(a => a.CITYCODE == bb.PID).CITYNAME;
                txt_city.Text = (cc + "," + bb.CITYNAME + "," + aa.CITYNAME).TrimStart(',');
                txt_city.Tag = aa;
            }
            else
            {
                txt_city.Text = list.Find(a => a.CITYCODE == code).CITYNAME;
                txt_city.Tag = code;
                error_provider.SetError(txt_city, "区划至少要选到市");
            }
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            //保存数据
            ConfigHelp.WriteDb(cob_db.Text, txt_dbstring.Text);
            //cwq,2019-04-15
            //ConfigHelp.WriteSignalr(txt_signalr.Text);
            ConfigHelp.WriteRedis(txt_redis.Text);
            ConfigHelp.WriteMongo(txt_mongodb.Text);
           
            XtraMessageBox.Show("修改完成，重新启动后生效");
            Application.Exit();
            System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //使用txt打开配置文件
            System.Diagnostics.Process.Start("notepad.exe",Directory.GetCurrentDirectory()+$"/appsettings.json");
        }
    }
    internal class DbModel
    {
        public string DbName { get; set; }
        public string DbString { get; set; }
    }
}