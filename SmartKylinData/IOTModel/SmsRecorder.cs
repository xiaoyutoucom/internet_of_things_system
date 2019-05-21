/*********************************************
* 命名空间: Smart.Core.Model
* 功 能： 短信配置实体
* 类 名： SmsRecorder
* 作 者:  CWQ
* 时 间： 2019年3月18日14:45:47
**********************************************
*/using System;
using Robin.Domain.Entities;
using Robin.NHibernate.EntityMappings;
namespace SmartKylinData.IOTModel
{
    public class SmsRecorderMap : EntityMap<SmsRecorder,string>
    {
        public SmsRecorderMap()
            : base("smart_sms_history")
        {
            Id(x => x.Id);
            Map(x => x.SYSTEMCODE);
            Map(x => x.PHONENUM);
            Map(x => x.CONTENT);
            Map(x => x.TYPE);
            Map(x => x.TIME);
            Map(x => x.STATUS);
            Map(x => x.EXTENDCODE);
            Map(x => x.EXTENDCODE2);
            Map(x => x.EXTENDCODE3);
            Map(x => x.EXTENDCODE4);
            Map(x => x.EXTENDCODE5);
        }
    }
    public class SmsRecorder : Entity<string>
    {
        /// <summary>
        /// 系统编码
        /// </summary>
        public virtual string SYSTEMCODE { get; set; }
        /// <summary>
        /// 发送号码
        /// </summary>
        public virtual string PHONENUM { get; set; }
        /// <summary>
        /// 短信内容
        /// </summary>
        public virtual string CONTENT { get; set; }
        /// <summary>
        /// 短信类型
        /// </summary>
        public virtual string TYPE { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public virtual Decimal TIME { get; set; }
        /// <summary>
        /// 发送状态,1、发送成功  2、发送失败
        /// </summary>
        public virtual string STATUS { get; set; }
        /// <summary>
        /// 备用字段
        /// </summary>
        public virtual string EXTENDCODE { get; set; }
        /// <summary>
        /// 备用字段2
        /// </summary>
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
