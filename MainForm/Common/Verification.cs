/*********************************************
* 命名空间:SmartKylinApp.Common
* 功 能： 配置信息验证类
* 类 名： Verification
* 作 者:  东腾
* 时 间： 2018-08-17 14:09:42 
**********************************************
*/

using System;
using System.Configuration;
using System.Data.OracleClient;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using log4net;
using MongoDB.Bson;
using MongoDB.Driver;
using Npgsql;
using NPOI.SS.Formula.Functions;
using ServiceStack.Redis;

namespace SmartKylinApp.Common
{
    public class Verification
    {
        private ILog _log = LogManager.GetLogger("Verification");
        /// <summary>
        ///     数据库类型
        /// </summary>
        public string DbName { get; set; }

        /// <summary>
        ///     数据库连接字符串
        /// </summary>
        public string DbConn { get; set; }

        /// <summary>
        ///     数据库连接状态
        /// </summary>
        public DbState DBConnState { get; set; }

        /// <summary>
        ///     redis信息
        /// </summary>
        public string Redis { get; set; }

        public bool RedisState { get; set; }

        /// <summary>
        ///     MongoDB信息
        /// </summary>
        public string MongoDb { get; set; }

        public bool MongoState { get; set; }

        /// <summary>
        ///     socket信息
        /// </summary>
        public string SignalR { get; set; }

        public bool SignaleState { get; set; }

        public delegate void UpProgress(int x);
        public UpProgress upProgress;
        /// <summary>
        ///     执行验证
        /// </summary>
        public void Execute()
        {
            VerifDb();
            if (upProgress != null) upProgress(30);
            //cwq,2019-04-15,推送服务注释
            //VerifSignalr();
            //if (upProgress != null) upProgress(20);
            //VerifMongo();
            if (upProgress != null) upProgress(20);
            //VerifRedis();

            MongoState = true;
            RedisState = true;
        }

        public enum DbState
        {
            Connected,
            DisConnection
        }

        /// <summary>
        ///     验证数据库
        /// </summary>
        private void VerifDb()
        {
            if (DbName != null)
                try
                {
                    switch (DbName)
                    {
                        case "Oracle":
                            {
                                var conn = new OracleConnection(DbConn);
                                conn.Open();
                                DBConnState = DbState.Connected;
                                conn.Close();
                               // ConfigHelp.WriteDb(DbName, DbConn);
                            }
                            break;
                        case "PgSQL":
                            var pgconn = new NpgsqlConnection(DbConn);
                            pgconn.Open();
                            DBConnState = DbState.Connected;
                            pgconn.Close();
                            ConfigHelp.WriteDb(DbName, DbConn);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    DBConnState = DbState.DisConnection;
                    _log.Error("验证数据库出错，出错提示：" + ex.ToString());
                }
        }

        private void VerifSignalr()
        {
            try
            {
                var str = SignalR.Split(':');
                var ip = str[1].Substring(2);
                if (ip == "localhost")
                    ip = "127.0.0.1";
                var point = new IPEndPoint(IPAddress.Parse(ip), int.Parse(str[2]));
                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(point);
                SignaleState = true;
                ConfigHelp.WriteSignalr(SignalR);
            }
            catch (Exception ex)
            {
                //SignaleState = false;
                SignaleState = true;
                _log.Error("验证socket信息出错，出错提示：" + ex.ToString());
            }
        }

        private void VerifRedis()
        {
            try
            {
                var arr = Redis.Split(',');
                var pwd = arr[1].Substring(arr[1].IndexOf('=') + 1);
                var redisManager = new RedisManagerPool(pwd + "@" + arr[0]);
                redisManager.GetClientPoolActiveStates();
                redisManager.GetClient();
                RedisState = true;
                ConfigHelp.WriteRedis(Redis);
            }
            catch (Exception ex)
            {
                _log.Error("验证Redis信息出错，出错提示：" + ex.ToString());
                RedisState = false;
                //RedisState = true;
            }
        }

        public void VerifMongo()
        {
            try
            {
                var aa = MongoDb.Split('/');
                MongoServerSettings s = new MongoServerSettings();
                s.Server = new MongoServerAddress(aa[2]);
                MongoServer a = new MongoServer(s);
                s.ConnectTimeout = TimeSpan.FromMinutes(1);
                MongoDatabase database = a.GetDatabase(aa[3]);
                var state = a.State;
                var collection = database.GetCollection<T>("Smart_Kylin_History");
                var filter = new BsonDocument();
                var Stats = collection.GetStats();
                MongoState = true;
                //if (state == MongoServerState.Connected)
                //{
                //    MongoState = true;
                //}

                //var str = MongoDb.Replace("//", "--");
                //var aa = str.Split('/');
                //var client = new MongoClient(aa[0].Replace("--", "//"));
                //var _database = client.GetDatabase(aa[1]);
                //var collection = _database.GetCollection<BsonDocument>("smart_kylin_history");
                //var filter = new BsonDocument();
                //var list = Task.Run(async () => await collection.Find(filter).ToListAsync()).Result;//连接测试是否成功
                //MongoState = true;
                ConfigHelp.WriteMongo(MongoDb);

            }
            catch (Exception ex)
            {
                _log.Error("验证Mongo数据库信息出错，出错提示：" + ex.ToString());
                MongoState = false;
                //MongoState = true;
            }
        }
    }
}