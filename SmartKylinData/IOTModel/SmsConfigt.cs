/*********************************************
* 命名空间: Smart.Core.Model
* 功 能： 短信配置实体
* 类 名： SmsConfigt
* 作 者:  CWQ
* 时 间： 2019年3月18日14:46:50
**********************************************
*/using System;
using Robin.Domain.Entities;
using Robin.NHibernate.EntityMappings;
namespace SmartKylinData.IOTModel
{
    public class SmsConfigtMap : EntityMap<SmsConfigt>
    {
        public SmsConfigtMap()
            : base("smart_sms_templateparam")
        {
            Id(x => x.Id).GeneratedBy.Increment();
            Map(x => x.CODE);
            Map(x => x.NAME);
            Map(x => x.PARAMNAME);
            Map(x => x.DISORDER);
        }
    }
    public class SmsConfigt : Entity<int>
    {
        /// <summary>
        /// 模板编号
        /// </summary>
        public virtual string CODE { get; set; }
        /// <summary>
        /// 模板名称
        /// </summary>
        public virtual string NAME { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public virtual string PARAMNAME { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public virtual int DISORDER { get; set; }
    }
}
