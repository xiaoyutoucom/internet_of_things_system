/*********************************************
* 命名空间: Smart.Core.Model
* 功 能： 设备协议表实体
* 类 名： AgreementRecord
* 作 者:  东腾
* 时 间： 2018/4/12 14:24:48 
**********************************************
*/
using Robin.Domain.Entities;
using Robin.NHibernate.EntityMappings;

namespace SmartKylinData.IOTModel
{
    public class AgreementRecordMap : EntityMap<AgreementRecord>
    {
        public AgreementRecordMap()
            : base("smart_kylin_agreement")
        {
            Id(x => x.Id).GeneratedBy.Increment();
            Map(x => x.Devicecj);
            Map(x => x.Enable);
            Map(x => x.Frequency);
            Map(x => x.Command);
            Map(x => x.Acode);
        }
    }
    public class AgreementRecord:Entity
    {
        /// <summary>
        /// 设备厂家
        /// </summary>
        public virtual string Devicecj { get; set; }
        /// <summary>
        /// 是否可用,0不可用，1可用
        /// </summary>
        public virtual int Enable { get; set; }
        /// <summary>
        /// 上传频率
        /// </summary>
        public virtual string Frequency { get; set; }
        /// <summary>
        /// 下发命令
        /// </summary>
        public virtual string Command { get; set; }
        /// <summary>
        /// 协议编号
        /// </summary>
        public virtual string Acode { get; set; }
    }
}
