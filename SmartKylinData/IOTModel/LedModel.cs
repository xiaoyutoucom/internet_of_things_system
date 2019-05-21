/*********************************************
* 命名空间:Smart.Core.Model
* 功 能： Led 实体
* 类 名： LedModel
* 作 者:  东腾
* 时 间： 2018/6/22 19:00:24 
**********************************************
*/using System;
using Robin.Domain.Entities;
using Robin.NHibernate.EntityMappings;

namespace SmartKylinData.IOTModel
{
    public class LedModelMap : EntityMap<LedModel>
    {
        public LedModelMap()
            : base("smart_kylin_led")
        {
            Id(x => x.Id).GeneratedBy.Increment();
            Map(x => x.TXFSCODE);
            Map(x => x.MC);
            Map(x => x.TXFS);
            Map(x => x.PHONE);
            Map(x => x.CKH);
            Map(x => x.BTL);
            Map(x => x.KZKIP);
            Map(x => x.KZKDZ);
            Map(x => x.BDDK);
            Map(x => x.ZBX);
            Map(x => x.ZBY);
            Map(x => x.BZ);
            Map(x => x.MESSAGEFOMAT);
            Map(x => x.EXTENDCODE);
            Map(x => x.EXTENDCODE2);
        }
    }
    public class LedModel:Entity<int>
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        //public virtual int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string MC { get; set; }
        /// <summary>
        /// 通讯方式：1网络通讯、2串口通讯、3短信
        /// </summary>
        public virtual string TXFSCODE { get; set; }
        /// <summary>
        /// 通讯方式：1网络通讯、2串口通讯
        /// </summary>
        public virtual string TXFS { get; set; }
        /// <summary>
        /// 通讯号码
        /// </summary>
        public virtual string PHONE { get; set; }
        /// <summary>
        /// 串口号
        /// </summary>
        public virtual string CKH { get; set; }
        /// <summary>
        /// 波特率
        /// </summary>
        public virtual string BTL { get; set; }
        /// <summary>
        /// 控制卡地址
        /// </summary>
        public virtual string KZKDZ { get; set; }
        /// <summary>
        /// 控制卡IP
        /// </summary>
        public virtual string KZKIP { get; set; }
        /// <summary>
        /// 本地端口
        /// </summary>
        public virtual int BDDK { get; set; }
        /// <summary>
        /// 坐标X
        /// </summary>
        public virtual double ZBX { get; set; }
        /// <summary>
        /// 坐标Y
        /// </summary>
        public virtual double ZBY { get; set; }
        /// <summary>
        /// 短信发送格式
        /// </summary>
        public virtual string MESSAGEFOMAT { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string BZ { get; set; }
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
