/*********************************************
* 命名空间: SmartKylinData.IOTModel
* 功 能： 雨量站关联积水点
* 类 名： RainWaterRecord
* 作 者:  cwq
* 时 间： 2019年4月28日14:27:23 
**********************************************
*/
using System;
using System.Linq;
using Robin.Domain.Entities;
using Robin.NHibernate.EntityMappings;

namespace SmartKylinData.IOTModel
{
    public class RainWaterRecordMap : EntityMap<RainWaterRecord>
    {
        public RainWaterRecordMap()
            : base("smart_kylin_rainlinkwater")
        {
            Id(x => x.Id).GeneratedBy.Increment();
            References<BasicMonitorRecord>(o => o.rain_id).Not.LazyLoad().Column("rain_id");
            References<BasicMonitorRecord>(o => o.water_id).Not.LazyLoad().Column("water_id");
        }
    }
    public class RainWaterRecord : Entity<int>
    {
        /// <summary>
        /// 雨量站
        /// </summary>
        public virtual BasicMonitorRecord rain_id { get; set; }
        /// <summary>
        /// 积水点
        /// </summary>
        public virtual BasicMonitorRecord water_id { get; set; }
    }
}
