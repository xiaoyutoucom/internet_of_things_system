using System.Reflection;
using Robin.Configuration.Startup;
using Robin.NHibernate;
using Robin.Modules;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using System;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace SmartKylinData
{
    [DependsOn(typeof(RobinNHibernateModule))]
    public class DataMoudle : RobinModule
    {
        public override void PreInitialize()
        {
            var json = Environment.CurrentDirectory + "\\appsettings.json";
            var build = new ConfigurationBuilder()
                .AddJsonFile(json)
                .Build();
            //Config = build;
            // 获取数据库类型
            var type = build["Application:Config:DbType"];
            var conn = build["Application:Config:DbConn"];
            FluentConfiguration config = null;
            switch (type)
            {
                case "Oracle":
                    config = Configuration.Modules.RobinNHibernate().FluentConfiguration
                        .Database(OracleClientConfiguration.Oracle10.ConnectionString(conn));
                    break;
                case "PgSQL":
                    config = Configuration.Modules.RobinNHibernate().FluentConfiguration
                        .Database(PostgreSQLConfiguration.Standard.ConnectionString(conn));
                    break;
                case "MySQL":
                    config = Configuration.Modules.RobinNHibernate().FluentConfiguration
                        .Database(MySQLConfiguration.Standard.ConnectionString(conn));
                    break;
            }

            //映射配置
            config.Mappings(a => a.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()));
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
       
    }
}
