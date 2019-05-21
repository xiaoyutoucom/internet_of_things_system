/*********************************************
* 命名空间:SmartKylinApp.Common
* 功 能： 配置帮助类
* 类 名： ConfigHelp
* 作 者:  东腾
* 时 间： 2018-08-16 17:39:17 
**********************************************
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SmartKylinApp.Common
{
    public class ConfigHelp
    {
        static ConfigHelp()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
            Config = config;
        }

        public static IConfigurationRoot Config { get; set; }

        public static IConfigurationRoot Info { get; set; }

        /// <summary>
        /// 当前数据库类型
        /// </summary>
        public static string DbType => Config["Application:Config:DbType"];

        /// <summary>
        /// 当前数据库连接字符串
        /// </summary>
        public static string DbConn=>Config["Application:Config:DbConn"];

        /// <summary>
        /// Redis缓存地址
        /// </summary>
        public static string RedisConnectionstring => Config["Application:Config:Redis"];

        /// <summary>
        /// MongoDB数据库地址
        /// </summary>
        public static string MongodbConnection => Config["Application:Config:MongoDB"];

        /// <summary>
        /// websocket地址
        /// </summary>
        public static string SignalrHost => Config["Application:Config:Signalr"];
        

        //写入JSON
        public static void WriteDb(string type,string conn)
        {
            var filePath = Directory.GetCurrentDirectory() + "\\appsettings.json";
            dynamic jObject = JObject.Parse(File.ReadAllText(filePath), new JsonLoadSettings() { CommentHandling = CommentHandling.Load });
            var config = jObject;
            config.Application.Config.DbType = type;
            config.Application.Config.DbConn = conn;
            var ob = JsonConvert.SerializeObject(config, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
            File.WriteAllText(filePath, ob);
        }

        public static void WriteRedis(string conn)
        {
            var filePath = Directory.GetCurrentDirectory() + "\\appsettings.json";
            dynamic jObject = JObject.Parse(File.ReadAllText(filePath), new JsonLoadSettings() { CommentHandling = CommentHandling.Load });
            var config = jObject;
            config.Application.Config.Redis = conn;
            var ob = JsonConvert.SerializeObject(config, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
            File.WriteAllText(filePath, ob);
        }

        public static void WriteMongo(string conn)
        {
            var filePath = Directory.GetCurrentDirectory() + "\\appsettings.json";
            dynamic jObject = JObject.Parse(File.ReadAllText(filePath), new JsonLoadSettings() { CommentHandling = CommentHandling.Load });
            var config = jObject;
            config.Application.Config.MongoDB = conn;
            var ob = JsonConvert.SerializeObject(config, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
            File.WriteAllText(filePath, ob);
        }

        public static void WriteSignalr(string conn)
        {
            var filePath = Directory.GetCurrentDirectory() + "\\appsettings.json";
            dynamic jObject = JObject.Parse(File.ReadAllText(filePath), new JsonLoadSettings() { CommentHandling = CommentHandling.Load });
            var config = jObject;
            config.Application.Config.Signalr = conn;
            var ob = JsonConvert.SerializeObject(config, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
            File.WriteAllText(filePath, ob);
        }

        public static void WriteCity(string conn)
        {
            var filePath = Directory.GetCurrentDirectory() + "\\appsettings.json";
            dynamic jObject = JObject.Parse(File.ReadAllText(filePath), new JsonLoadSettings() { CommentHandling = CommentHandling.Load });
            var config = jObject;
            config.Application.Config.City = conn;
            var ob = JsonConvert.SerializeObject(config, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
            File.WriteAllText(filePath, ob);
        }

        public static void WriteTime(string time)
        {
            var filePath = Directory.GetCurrentDirectory() + "\\appsettings.json";
            dynamic jObject = JObject.Parse(File.ReadAllText(filePath), new JsonLoadSettings() { CommentHandling = CommentHandling.Load });
            var config = jObject;
            config.Application.Config.Time = time;
            var ob = JsonConvert.SerializeObject(config, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
            File.WriteAllText(filePath, ob);
        }
        public static void WriteType(string Type)
        {
            var filePath = Directory.GetCurrentDirectory() + "\\appsettings.json";
            dynamic jObject = JObject.Parse(File.ReadAllText(filePath), new JsonLoadSettings() { CommentHandling = CommentHandling.Load });
            var config = jObject;
            config.Application.Config.Type = Type;
            var ob = JsonConvert.SerializeObject(config, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
            File.WriteAllText(filePath, ob);
        }
    }
}
