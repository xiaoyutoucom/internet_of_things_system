/*********************************************
* 命名空间: Smart.Core.Model
* 功 能： 人员关联监测点实体
* 类 名： WorkerLinkMonitor
* 作 者:  东腾
* 时 间： 2018-06-05 16:47:17 
**********************************************
*/
using System;
using System.Linq;
using Robin.Domain.Entities;
using Robin.NHibernate.EntityMappings;

namespace SmartKylinData.IOTModel
{
    public class WaterLinkMap : EntityMap<WaterLink>
    {
        public WaterLinkMap()
            : base("smart_kylin_waterlinkpump")
        {
            Id(x => x.Id).GeneratedBy.Increment();
            Map(x => x.EXTENDCODE);
            Map(x => x.EXTENDCODE2);
            Map(x => x.EXTENDCODE3);
            Map(x => x.EXTENDCODE4);
            Map(x => x.EXTENDCODE5);
            References<BasicMonitorRecord>(o => o.pmonitorrecord_id).Not.LazyLoad().Column("pmonitorrecord_id");
            References<BasicMonitorRecord>(o => o.water_id).Not.LazyLoad().Column("water_id");
        }
    }
    public class WaterLink : Entity<int>
    {
        /// <summary>
        /// 积水点
        /// </summary>
        public virtual BasicMonitorRecord water_id { get; set; }
        /// <summary>
        /// 泵站
        /// </summary>
        public virtual BasicMonitorRecord pmonitorrecord_id { get; set; }
        /// <summary>
        /// 备用字段
        /// </summary>
        public virtual string EXTENDCODE { get; set; }
        public virtual string EXTENDCODE2 { get; set; }
        public virtual string EXTENDCODE3 { get; set; }
        public virtual string EXTENDCODE4 { get; set; }
        public virtual string EXTENDCODE5 { get; set; }

    }
}
