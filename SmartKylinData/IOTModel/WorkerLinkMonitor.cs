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
    public class WorkerLinkMonitorMap : EntityMap<WorkerLinkMonitor>
    {
        public WorkerLinkMonitorMap()
            : base("smart_sms_workerlinkmonitor")
        {
            Id(x => x.Id).GeneratedBy.Increment();
            Map(x => x.EXTENDCODE);
            References<BasicMonitorRecord>(o => o.BaseMonitor).Not.LazyLoad().Column("monitorid");
            References<Contact>(o => o.Contact).Not.LazyLoad().Column("workerid");
        }
    }
    public class WorkerLinkMonitor : Entity<int>
    {
        /// <summary>
        /// 监测点
        /// </summary>
        public virtual BasicMonitorRecord BaseMonitor { get; set; }
        /// <summary>
        /// 通讯录
        /// </summary>
        public virtual Contact Contact { get; set; }
        /// <summary>
        /// 备用字段
        /// </summary>
        public virtual string EXTENDCODE { get; set; }


    }
}
