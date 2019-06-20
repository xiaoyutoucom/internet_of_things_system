using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraPrinting.Native;
using Newtonsoft.Json;
using Robin.Json;
using SmartKylinApp.Common;
using SmartKylinData.IOTModel;
using StackExchange.Redis;
using SmartKylinApp.View.BaseConfig;
using log4net;
using RestSharp.Extensions;
using Newtonsoft.Json.Linq;

namespace SmartKylinApp.View.Cache
{
    public partial class CacheManger : DevExpress.XtraEditors.XtraForm
    {


        /**
         *功能说明：
         * 1、实现物联网数据缓存
         * 2、修改并更新单个数据
         * k-v说明：
         * Default:Kylin:Config ：针对于泵站信息，opc处理
         * Default:Kylin:ConfigSensor:100  ：监测项传感器数据，数值为传感器id
         * Default:Kylin:Device:0020161217  :设备信息缓存，保存设备的出厂编号
         * 时间：2018-12-17
         * 东腾
         * *
         */

        public CacheManger()
        {
            InitializeComponent();

            label1.Visible = false;
            comboBox2.Visible = false;
            updatatype = true;

            //禁止右键菜单
            layoutControl2.AllowCustomization = false;
            layoutControl1.AllowCustomization = false;
        }

        private IDatabase _db;
        private bool isConn = false;
        private ILog _log = LogManager.GetLogger("CacheManger");
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //连接缓存服务器
            try
            {
                var conn = GlobalHandler.RedisConnectionstring;
                var config = ConfigurationOptions.Parse(conn);
                var redis = ConnectionMultiplexer.Connect(config);
                _db = redis.GetDatabase(0);
                labelControl1.Text = @"连接成功";
                labelControl1.ForeColor = Color.MediumSeaGreen;
                isConn = true;
            }
            catch (Exception exception)
            {
                isConn = false;
                labelControl1.Text = @"连接失败";
                labelControl1.ForeColor = Color.Red;
            }
        }

        private bool updatatype;

        private void richEditControl1_Click(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label1.Visible = radioButton2.Checked;
            comboBox2.Visible = radioButton2.Checked;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label1.Visible = !radioButton1.Checked;
            comboBox2.Visible = !radioButton1.Checked;
            updatatype = radioButton1.Checked;
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (!isConn)
            {
                XtraMessageBox.Show("服务未连接，请点击连接按钮");
                return;
            }
            //删除数据
            DelectBox dbox = new DelectBox();
            dbox.StartPosition = FormStartPosition.CenterScreen;
            dbox.ShowDialog();
            bool IfDelect = dbox.IfDelect;
            if (!IfDelect)
            {
                return;
            }
            splashScreenManager1.ShowWaitForm();
            splashScreenManager1.SetWaitFormCaption("请稍后,缓存更新中....");     // 标题
            splashScreenManager1.SetWaitFormDescription("正在更新.....");　　　　　// 信息

            try
            {
                Application.DoEvents();
                //更新缓存
                if (updatatype)
                {
                    //更新全部缓存
                    UpdataAllCache();
                }
                else
                {
                    //按照行业类型更新
                    var type = comboBox2.SelectedValue.ToString();
                    UpdataCacheBytype(type);
                }

                splashScreenManager1.CloseWaitForm();

                XtraMessageBox.Show("缓存更新成功");
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show($"缓存更新失败");
                _log.Error("缓存更新失败，出错提示：" + exception.ToString());
            }


        }
        private void UpOPC(string type)
        {
            try
            {
                List<ConfigRecord> configlist;
                configlist = string.IsNullOrEmpty(type) ? GlobalHandler.configresp.GetAllList(a => a.VARIABLE_NAME != null && a.VARIABLE_NAME != "") : GlobalHandler.configresp.GetAllList(a => a.VARIABLE_NAME != null && a.VARIABLE_NAME != "" && a.STATIONID.STATIONTYPE.StartsWith(type));

                configlist.ForEach(a =>
                {
                    try
                    {
                        a.TAGID = null;
                        a.STATIONID = null;
                        a.SENSORID = null;
                        _db.KeyDelete($"Default:Kylin:Opc:{a.VARIABLE_NAME}");
                        _db.StringSet($"Default:Kylin:Opc:{a.VARIABLE_NAME}", JsonConvert.SerializeObject(a));
                    }
                    catch { }
                });
            }
            catch (Exception e)
            {
                _log.Error("opc缓存更新失败，出错提示：" + e.ToString());
            }
        }

        //更新缓存
        private void UpdataAllCache()
        {
            try
            {
                //获取所有的数据
                var configlist = GlobalHandler.configresp.GetAllList(a => (a.VARIABLE_NAME == "" || a.VARIABLE_NAME == null) && a.SENSORID != null);
                configlist?.ForEach(b =>
                {
                    try
                    {
                        var key = b.SENSORID.Id;
                        b.TAGID = null;
                        b.STATIONID = null;
                        b.SENSORID = null;
                        _db.KeyDelete($"Default:Kylin:ConfigSensor:{key}");
                        _db.StringSet($"Default:Kylin:ConfigSensor:{key}", JsonConvert.SerializeObject(b));
                    }
                    catch { }

                });

                //更新设备传感器
                var devicelist = GlobalHandler.deviceresp.GetAllList();
                var sensorlist = GlobalHandler.sensorresp.GetAllList();

                devicelist?.ForEach(a =>
                {
                    try
                    {
                        var sens = sensorlist.Where(b => b.Device?.Id == a.Id).Select(c => new SensorRecord()
                        {
                            Id = c.Id,
                            CCRQ = c.CCRQ,
                            CGQBM = c.CGQBM,
                            CGQLXBM = c.Device.CCBH,
                            BZ = c.BZ,
                            CGQLXMC = c.CGQLXMC,
                            CGQMC = c.CGQMC,
                            CGQXH = c.CGQXH
                        });
                        _db.KeyDelete($"Default:Kylin:Device:{a.CCBH}");
                        _db.StringSet($"Default:Kylin:Device:{a.CCBH}", JsonConvert.SerializeObject(sens));
                    }
                    catch { }
                });
            }
            catch (Exception e)
            {
                _log.Error("缓存更新失败，出错提示：" + e);
            }
            UpOPC("");
        }


        private void UpdataCacheBytype(string type)
        {
            try
            {
                //获取所有的数据
                var configlist = GlobalHandler.configresp.GetAllList(a => (a.VARIABLE_NAME == "" || a.VARIABLE_NAME == null) && a.SENSORID != null && a.STATIONID.STATIONTYPE.StartsWith(type));
                configlist?.ForEach(b =>
                {
                    try
                    {
                        var key = b.SENSORID.Id;
                        b.TAGID = null;
                        b.STATIONID = null;
                        b.SENSORID = null;
                        _db.KeyDelete($"Default:Kylin:ConfigSensor:{key}");
                        _db.StringSet($"Default:Kylin:ConfigSensor:{key}", JsonConvert.SerializeObject(b));
                    }
                    catch { }

                });

                //更新设备传感器
                var devicelist = GlobalHandler.deviceresp.GetAllList(a => a.SBTYPE.StartsWith(type));
                var sensorlist = GlobalHandler.sensorresp.GetAllList(a => a.Device.SBTYPE.StartsWith(type));
                devicelist?.ForEach(a =>
                {
                    try
                    {
                        var sens = sensorlist.Where(b => b.Device?.Id == a.Id).Select(c => new SensorRecord()
                        {
                            Id = c.Id,
                            CCRQ = c.CCRQ,
                            CGQBM = c.CGQBM,
                            CGQLXBM = c.Device.CCBH,
                            BZ = c.BZ,
                            CGQLXMC = c.CGQLXMC,
                            CGQMC = c.CGQMC,
                            CGQXH = c.CGQXH
                        });
                        _db.KeyDelete($"Default:Kylin:Device:{a.CCBH}");
                        _db.StringSet($"Default:Kylin:Device:{a.CCBH}", JsonConvert.SerializeObject(sens));
                    }
                    catch { }
                });

            }
            catch (Exception e)
            {
                _log.Error("分类更新，出错提示：" + e);
            }

            //更新opc
            UpOPC(type);
        }

        private void CacheManger_Load(object sender, EventArgs e)
        {

            var list = GlobalHandler.mstyperesp.GetAllList(a => a.TYPE_KEY.Length == 2).GroupBy(a => a.TYPE_KEY).Select(a =>
                   new MstypeModel()
                   {
                       Key = a.Key,
                       Value = a.FirstOrDefault()?.TYPE_NAME

                   }).ToList();

            comboBox2.ValueMember = "Key";
            comboBox2.DisplayMember = "Value";
            comboBox2.DataSource = list;
            comboBox2.SelectedIndex = 0;

            //绑定查询调价
            var klist = new List<MstypeModel>
            {
                new MstypeModel()
                {
                    Id="1",
                    Key = "Opc缓存",
                    Value = "Default:Kylin:Opc"
                },
                new MstypeModel()
                {
                    Id="2",
                    Key = "监测项传感器缓存",
                    Value = "Default:Kylin:ConfigSensor"
                },
                new MstypeModel()
                {
                    Id="3",
                    Key = "设备信息缓存",
                    Value = "Default:Kylin:Device"
                }
            };
            comboBox1.ValueMember = "Value";
            comboBox1.DisplayMember = "Key";
            comboBox1.DataSource = klist;
            comboBox1.SelectedIndex = 0;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //根据条件查询
            try
            {
                if (!isConn)
                {
                    XtraMessageBox.Show("请连接redis");
                    return;
                }

                switch ((comboBox1.SelectedItem as MstypeModel)?.Id)
                {
                    case "1":
                        //查询符合条件的opc缓存
                        var key1 = comboBox1.SelectedValue.ToString() + ":" + textEdit1.Text;
                        QueryCache(key1);
                        break;
                    case "2":
                        //查询符合条件的监测项，传感器信息
                        var key2 = comboBox1.SelectedValue.ToString() + ":" + textEdit1.Text;
                        QueryCache(key2);
                        break;
                    case "3":
                        //查询设备缓存信息
                        var key3 = comboBox1.SelectedValue.ToString() + ":" + textEdit1.Text;
                        QueryCache(key3);
                        break;
                }

            }
            catch (Exception exception)
            {
                XtraMessageBox.Show("查询失败");
            }
        }

        private void QueryCache(string key)
        {
            var data = _db.StringGet(key);

            //处理成json 格式

            if (!data.IsNull)
            {
                var strs = data.ToString();

                var arr = (JObject)JsonConvert.DeserializeObject<dynamic>(strs);
                richTextBox1.AppendText(arr.ToString());
            }
        }
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            //根据条件查询需要的键值

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            //保存修改后缓存
            //确认权限
            DelectBox dbox = new DelectBox();
            dbox.StartPosition = FormStartPosition.CenterScreen;
            dbox.ShowDialog();
            bool IfDelect = dbox.IfDelect;
            if (!IfDelect)
            {
                return;
            }
            if (!isConn)
            {
                XtraMessageBox.Show("缓存服务器连接失败，请检查连接字符串是否正确");
                return;
            }

            switch ((comboBox1.SelectedItem as MstypeModel)?.Id)
            {
                case "1":
                    //查询符合条件的opc缓存
                    var key1 = comboBox1.SelectedValue.ToString();
                    SavaCache(key1);
                    break;
                case "2":
                    //查询符合条件的监测项，传感器信息
                    var key2 = comboBox1.SelectedValue.ToString() + ":" + textEdit1.Text;
                    SavaCache(key2);
                    break;
                case "3":
                    //查询设备缓存信息
                    var key3 = comboBox1.SelectedValue.ToString() + ":" + textEdit1.Text;
                    SavaCache(key3);
                    break;
            }
        }

        private void SavaCache(string key)
        {
            try
            {
                var data = string.Empty;

                var lines = richTextBox1.Lines;

                lines.ForEach(a => data += a);

                _db.StringSet(key, data);
                XtraMessageBox.Show("更新成功");
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("更新失败");
                _log.Error("更新失败，出错提示：" + e.ToString());
            }
        }
    }

    partial class MstypeModel
    {
        public string Id { get; set; }
        public string Key { get; set; }

        public string Value { get; set; }
    }
}