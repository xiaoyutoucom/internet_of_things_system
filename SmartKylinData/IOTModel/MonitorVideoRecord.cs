using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Robin.Domain.Entities;
using Robin.NHibernate.EntityMappings;

namespace SmartKylinData.IOTModel
{
    public class MonitorVideoRecordMap : EntityMap<MonitorVideoRecord>
    {
        public MonitorVideoRecordMap() : base("smart_kylin_monitorvideo")
        {
            Id(x => x.Id).GeneratedBy.Increment();
            References<BasicMonitorRecord>(o => o.MONITOR).Not.LazyLoad().Column("monitor_id");
            References<VideoRecord>(o => o.VIDEO).Not.LazyLoad().Column("video_id");
        }
    }
    /// <summary>
    /// 监测点关联视频表
    /// </summary>

    public class MonitorVideoRecord : Entity<int>
    {
        /// <summary>
        /// 监测点ID
        /// </summary>
        public virtual BasicMonitorRecord MONITOR { get; set; }
        /// <summary>
        /// 视频ID
        /// </summary>
        public virtual VideoRecord VIDEO { get; set; }
    }
}
