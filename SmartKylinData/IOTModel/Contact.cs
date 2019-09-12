/*********************************************
* 命名空间: Smart.Core.Model
* 功 能： 通讯录实体
* 类 名： contact
* 作 者:  东腾
* 时 间： 2018-06-05 15:52:08 
**********************************************
*/

using System;
using Robin.Domain.Entities;
using Robin.NHibernate.EntityMappings;

namespace SmartKylinData.IOTModel
{
    public class ContactMap : EntityMap<Contact>
    {
        public ContactMap()
            : base("smart_sms_contacts")
        {
            Id(x => x.Id).GeneratedBy.Increment();
            Map(x => x.NAME);
            Map(x => x.BIRTHDAY);
            Map(x => x.GENDER);
            Map(x => x.MAJOB);
            Map(x => x.PHONE);
            Map(x => x.DEPARTMENT); Map(x => x.ADDRESS);
            Map(x => x.JOB);
            Map(x => x.DUTY); Map(x => x.SECTION);
            Map(x => x.EXTENDCODE);
            References<ContactsGroup>(o => o.CONTACTSGROUP).Not.LazyLoad().Column("group_id");
        }
    }
    public class Contact : Entity<int>
    {
        public virtual string NAME { get; set; }
        public virtual ContactsGroup CONTACTSGROUP { get; set; }
        //性别
        public virtual string GENDER { get; set; }
        //生日
        public virtual DateTime BIRTHDAY { get; set; }
        //专业
        public virtual string MAJOB { get; set; }
        public virtual string PHONE { get; set; }
        //
        public virtual string DEPARTMENT { get; set; }
        public virtual string ADDRESS { get; set; }
        //职位
        public virtual string JOB { get; set; }
        //职责
        public virtual string DUTY { get; set; }
        //责任路段
        public virtual string SECTION { get; set; }
        public virtual string EXTENDCODE { get; set; }
    }
}
