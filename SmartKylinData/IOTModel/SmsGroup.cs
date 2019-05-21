/*********************************************
* 命名空间: Smart.Core.Model
* 功 能： 通讯录分组实体
* 类 名： SmsGroup
* 作 者:  东腾
* 时 间： 2018-06-05 11:26:21 
**********************************************
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robin.Domain.Entities;
using Robin.NHibernate.EntityMappings;

namespace SmartKylinData.IOTModel
{
    public class ContactsGroupMap : EntityMap<ContactsGroup>
    {
        public ContactsGroupMap()
            : base("smart_sms_contactsgroup")
        {
            Id(x => x.Id).GeneratedBy.Increment();
            Map(x => x.PARENTID);
            Map(x => x.GROUPNAME);
            Map(x => x.GROUPNUM);
            Map(x => x.REMARK);
            Map(x => x.EXTENDCODE);// References<AgreementRecord>(o => o.Agreement).Not.LazyLoad().Column("agreement_id");
        }
    }

    /// <summary>
    /// 通讯录分组
    /// </summary>
    public class ContactsGroup: Entity<int>
    {
        /// <summary>
        /// 分组Id
        /// </summary>
        //public virtual int ID { get; set; }
        /// <summary>
        /// 父节点ID </summary>
        public virtual int PARENTID { get; set; }
        /// <summary>
        /// 分组名称
        /// </summary>
        public virtual string GROUPNAME { get; set; }
        /// <summary>
        /// 分组编号，系统唯一Guid
        /// </summary>
        public virtual string GROUPNUM { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string REMARK { get; set; }
        /// <summary>
        /// 备用字段1
        /// </summary>
        public virtual string EXTENDCODE { get; set; }
        
    }
}
