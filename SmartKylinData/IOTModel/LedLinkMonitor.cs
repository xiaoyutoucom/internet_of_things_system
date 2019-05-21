/*********************************************
* 命名空间:Smart.Core.Model
* 功 能： redis 模块
* 类 名： LedLinkMonitor
* 作 者:  东腾
* 时 间： 2018/6/24 16:50:10 
**********************************************
*/using System;
using Robin.Domain.Entities;
using Robin.NHibernate.EntityMappings;
namespace SmartKylinData.IOTModel
{
    public class LedLinkMonitorMap : EntityMap<LedLinkMonitor>
    {
        public LedLinkMonitorMap()
            : base("smart_kylin_monitorled")
        {
            Id(x => x.Id).GeneratedBy.Increment();
            References<LedModel>(o => o.Led).Not.LazyLoad().Column("ledrecord_id");
            References<BasicMonitorRecord>(o => o.MonitorRecord).Not.LazyLoad().Column("monitorrecord_id");
        }
    }
    public class LedLinkMonitor:Entity<int>
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }
        /// <summary>
        /// LED Id
        /// </summary>
        public virtual LedModel Led { get; set; }
        /// <summary>
        ///监测点ID
        /// </summary>
        public virtual BasicMonitorRecord MonitorRecord { get; set; }
    }
}
