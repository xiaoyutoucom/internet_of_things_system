/*********************************************
* 命名空间:SmartKylinApp.Common
* 功 能： 全局帮助类
* 类 名： GlobalHandler
* 作 者:  东腾
* 时 间： 2018-08-08 22:15:28 
**********************************************
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Microsoft.Extensions.Configuration;
using Mongodb;
using Mongodb.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Robin;
using SmartKylinData.BaseModel;
using SmartKylinData.IOTModel;

namespace SmartKylinApp.Common
{
    public class GlobalHandler : ConfigHelp
    {
        //监测加密狗是否存在
        public static string UserName;

        public static bool Auth;

        public static bool Checked;

        public static List<CityModel> CityInfo { get; set; }

        public static RobinBootstrapper Bootstrapper;

        public static IEnumerable<BasicMonitorRecord> monitors;

        public static IEnumerable<ConfigRecord> config;

        public static BindingList<TableModel> resourList;

        public static object ControlContainer;

        public static string LocalInfo { get; set; }

        public static string CityCode { get; set; }

        public static object CurrentSkin { get; set; }

        /// <summary>
        ///     加密狗信息验证
        /// </summary>
        public static void GetKeyInfo()
        {
            //初始化我们的操作加密锁的类
            var ytsoftkey = new SoftKeyPWD();
            var KeyPath = string.Empty;
            //
            //这个用于判断系统中是否存在着加密锁。不需要是指定的加密锁,
            Checked = ytsoftkey.FindPort(0, ref KeyPath) == 0;

            if (Checked)
            {
                var pwd = "";
                int id1 = 0, id2 = 0;
                if (ytsoftkey.GetID(ref id1, ref id2, KeyPath) == 0) pwd = id1.ToString();
                //查找用户名
                var buf = new byte[1];
                var outstring = "";
                short addr = 0; //要读取的地址
                //先从地址0读到以前写入的字符串的长度
                var ret = ytsoftkey.YReadEx(buf, addr, 1, pwd, pwd, KeyPath);
                short nlen = buf[0];
                if (ret != 0)
                {
                    //MessageBox.Show("读取字符串长度错误。错误码：" + ret.ToString()); return;
                }

                //再读取相应长度的字符串
                ret = ytsoftkey.YReadString(ref outstring, addr + 1, nlen, pwd, pwd, KeyPath);
                UserName = ret != 0 ? "用户名获取失败" : outstring;
                Auth = true;
            }
            else
            {
                UserName = "未检测到加密狗";
                Auth = false;
            }
        }

        public static void ApplicationStart()
        {
            //获取系统配置文件
            var json = Environment.CurrentDirectory + "\\appsettings.json";
            var build = new ConfigurationBuilder()
                .AddJsonFile(json)
                .Build();
            Config = build;
            //获取区划信息
            var assembly = Assembly.GetExecutingAssembly();
            const string name = "SmartKylinApp.Data.city.json";
            var str = assembly.GetManifestResourceStream(name);
            var reader = new StreamReader(str);
            var cityJson = (JArray) JsonConvert.DeserializeObject(reader.ReadToEnd());
            var cityList = cityJson.ToObject<List<CityModel>>();
            CityInfo = cityList;
            CityCode = ConfigHelp.Config["Application:Config:City"];
        }

        public static void AddControl(object control)
        {
            var panel = (PanelControl) ControlContainer;
            ((Control) control).Dock = DockStyle.Fill;
            panel.Controls.Clear();
            panel.Controls.Add((Control) control);
        }


        //设备协议
        public static Robin.Domain.Repositories.IRepository<AgreementRecord> agreeresp =>
            Bootstrapper.IocManager.Resolve<Robin.Domain.Repositories.IRepository<AgreementRecord>>();

        //行业信息
        public static Robin.Domain.Repositories.IRepository<MsTypeRecord> mstyperesp =>
            Bootstrapper.IocManager.Resolve<Robin.Domain.Repositories.IRepository<MsTypeRecord>>();

        //行业信息
        public static Robin.Domain.Repositories.IRepository<DeviceRecord> deviceresp =>
            Bootstrapper.IocManager.Resolve<Robin.Domain.Repositories.IRepository<DeviceRecord>>();

        //区划信息
        public static Robin.Domain.Repositories.IRepository<CityRecord> cityresp =>
            Bootstrapper.IocManager.Resolve<Robin.Domain.Repositories.IRepository<CityRecord>>();

        //传感器信息
        public static Robin.Domain.Repositories.IRepository<SensorRecord> sensorresp =>
            Bootstrapper.IocManager.Resolve<Robin.Domain.Repositories.IRepository<SensorRecord>>();

        public static Robin.Domain.Repositories.IRepository<TagInfoRecord> tagresp =>
            Bootstrapper.IocManager.Resolve<Robin.Domain.Repositories.IRepository<TagInfoRecord>>();

        //监测点
        public static Robin.Domain.Repositories.IRepository<BasicMonitorRecord> monitorresp =>
            Bootstrapper.IocManager.Resolve<Robin.Domain.Repositories.IRepository<BasicMonitorRecord>>();
        //监测项
        public static Robin.Domain.Repositories.IRepository<ConfigRecord> configresp =>
            Bootstrapper.IocManager.Resolve<Robin.Domain.Repositories.IRepository<ConfigRecord>>();

        //通讯录分组
        public static Robin.Domain.Repositories.IRepository<ContactsGroup> contactgroupresp =>
            Bootstrapper.IocManager.Resolve<Robin.Domain.Repositories.IRepository<ContactsGroup>>();
        //通讯录
        public static Robin.Domain.Repositories.IRepository<Contact> contactresp =>
            Bootstrapper.IocManager.Resolve<Robin.Domain.Repositories.IRepository<Contact>>();
        //短信模板
        public static Robin.Domain.Repositories.IRepository<SmsConfigs> msgtempleteresp =>
            Bootstrapper.IocManager.Resolve<Robin.Domain.Repositories.IRepository<SmsConfigs>>();

        //新增短信模板
        public static Robin.Domain.Repositories.IRepository<SmsConfigt> msgSmsConfigtresp =>
            Bootstrapper.IocManager.Resolve<Robin.Domain.Repositories.IRepository<SmsConfigt>>();
        //新增短信发送记录
        public static Robin.Domain.Repositories.IRepository<SmsRecorder,string> msgSmsRecordresp =>
            Bootstrapper.IocManager.Resolve<Robin.Domain.Repositories.IRepository<SmsRecorder,string>>();

        //联系人关联监测点
        public static Robin.Domain.Repositories.IRepository<WorkerLinkMonitor> wlinkmresp =>
            Bootstrapper.IocManager.Resolve<Robin.Domain.Repositories.IRepository<WorkerLinkMonitor>>();

        //视频分组信息
        public static Robin.Domain.Repositories.IRepository<VideoGroupRecord> videogroupresp =>
            Bootstrapper.IocManager.Resolve<Robin.Domain.Repositories.IRepository<VideoGroupRecord>>();
        //视频信息
        public static Robin.Domain.Repositories.IRepository<VideoRecord> videoresp =>
            Bootstrapper.IocManager.Resolve<Robin.Domain.Repositories.IRepository<VideoRecord>>();
        //视频监测点关联
        public static Robin.Domain.Repositories.IRepository<MonitorVideoRecord> monitorlinkvideoresp =>
            Bootstrapper.IocManager.Resolve<Robin.Domain.Repositories.IRepository<MonitorVideoRecord>>();
        //LED信息
        public static Robin.Domain.Repositories.IRepository<LedModel> ledresp =>
            Bootstrapper.IocManager.Resolve<Robin.Domain.Repositories.IRepository<LedModel>>();
        //LED关联检测带点
        public static Robin.Domain.Repositories.IRepository<LedLinkMonitor> ledLinkresp =>
            Bootstrapper.IocManager.Resolve<Robin.Domain.Repositories.IRepository<LedLinkMonitor>>();
        //积水点关联泵站
        public static Robin.Domain.Repositories.IRepository<WaterLink> waterLinkresp =>
            Bootstrapper.IocManager.Resolve<Robin.Domain.Repositories.IRepository<WaterLink>>();
        //雨量站关联积水点
        public static Robin.Domain.Repositories.IRepository<RainWaterRecord> rainLinkWaterresp =>
            Bootstrapper.IocManager.Resolve<Robin.Domain.Repositories.IRepository<RainWaterRecord>>();
        //字典信息
        public static Robin.Domain.Repositories.IRepository<LedFontLibrary> ledFontresp =>
            Bootstrapper.IocManager.Resolve<Robin.Domain.Repositories.IRepository<LedFontLibrary>>();
        //Modbus信息
        public static Robin.Domain.Repositories.IRepository<ModbusRecord> modbusresp =>
            Bootstrapper.IocManager.Resolve<Robin.Domain.Repositories.IRepository<ModbusRecord>>();
        //历史数据查询仓储
        public static IRepository<Smart_Kylin_History> historyResp = new MongoDBRepository<Smart_Kylin_History>(MongodbConnection);

        //报警记录查询仓储
        public static IRepository<Smart_Kylin_Alert> alertResp = new MongoDBRepository<Smart_Kylin_Alert>(MongodbConnection);
        //实时数据查询仓储
        public static IRepository<Smart_Kylin_Runtime> runtimeResp = new MongoDBRepository<Smart_Kylin_Runtime>(MongodbConnection);
    }
}