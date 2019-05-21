using Robin.Domain.Entities;
using Robin.NHibernate.EntityMappings;
using System;

/**
* 命名空间: Smart.Core.Model
* 功 能： 监测点实体类
* 类 名： BasicMonitorRecord
* 作 者:  张保东
* 时 间： 2018/1/24 11:22:18 
*/
namespace SmartKylinData.IOTModel
{


    public class BasicMonitorRecordMap : EntityMap<BasicMonitorRecord>
    {
        public BasicMonitorRecordMap()
            : base("smart_kylin_basicmonitor")
        {
            Id(x => x.Id).GeneratedBy.Increment();
            Map(x => x.BMID);
            Map(x => x.BMMC);
            Map(x => x.BJBM);
            Map(x => x.BMMS);
            Map(x => x.BMX);
            Map(x => x.BMY);
            Map(x => x.Template);
            Map(x => x.TXFS);
            Map(x => x.WebUrl);
            Map(x => x.IMGURL);
            Map(x => x.STATIONTYPE);
            Map(x => x.MLEVEL);
            Map(x => x.ADDTIME);
            Map(x => x.EXTENDCODE);
            Map(x => x.EXTENDCODE2);
            Map(x => x.EXTENDCODE3);
            Map(x => x.EXTENDCODE4);
            Map(x => x.EXTENDCODE5);
            // References<BasicMonitorRecord>(o => o.Device).Not.LazyLoad().Column("device_id");
        }
    }

    public class BasicMonitorRecord : Entity<int>
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        //public virtual int Id { get; set; }
        /// <summary>
        /// 监测点编码
        /// </summary>
        public virtual string BMID { get; set; }
        /// <summary>
        /// 监测点名称
        /// </summary>
        public virtual string BMMC { get; set; }
        /// <summary>
        /// 部件编码
        /// </summary>
        public virtual string BJBM { get; set; }
        /// <summary>
        /// 中心坐标X
        /// </summary>
        public virtual decimal BMX { get; set; }
        /// <summary>
        /// 中心坐标Y
        /// </summary>
        public virtual decimal BMY { get; set; }

        /// <summary>
        ///消息模板 用户短信发送时的模板
        /// </summary>
        public virtual string Template { get; set; }

        /// <summary>
        /// 通讯方式
        /// </summary>
        public virtual string TXFS { get; set; }
        /// <summary>
        /// OPC的url
        /// </summary>
        public virtual string WebUrl { get; set; }
        /// <summary>
        /// 图片URL
        /// </summary>
        public virtual string IMGURL { get; set; }
        /// <summary>
        /// 监测点类型
        /// </summary>
        public virtual string STATIONTYPE { get; set; }
        /// <summary>
        /// 监测点级别 重要监测点
        /// </summary>
        public virtual string MLEVEL { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public virtual string BMMS { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public virtual DateTime ADDTIME { get; set; }
        /// <summary>
        ///     EXTENDCODE
        /// </summary>
        public virtual string EXTENDCODE { get; set; }
        /// <summary>
        /// 备用字段2
        /// </summary>
        /// 
        public virtual string EXTENDCODE2 { get; set; }
        /// <summary>
        /// 备用字段3
        /// </summary>
        public virtual string EXTENDCODE3 { get; set; }
        /// <summary>
        /// 备用字段4
        /// </summary>
        public virtual string EXTENDCODE4 { get; set; }
        /// <summary>
        /// 备用字段5
        /// </summary>
        public virtual string EXTENDCODE5 { get; set; }
    }
}
