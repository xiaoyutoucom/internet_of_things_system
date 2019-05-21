using System;
using Robin.Domain.Entities;
using Robin.NHibernate.EntityMappings;

/**
* 命名空间: Smart.Core.Model
* 功 能： 设备表实体
* 类 名： DevicedRecord
* 作 者:  张保东
* 时 间： 2018/1/23 10:41:45 
*/
namespace SmartKylinData.IOTModel
{
    public class DeviceRecordMap : EntityMap<DeviceRecord>
    {
        public DeviceRecordMap()
            : base("smart_kylin_device")
        {
            Id(x => x.Id).GeneratedBy.Increment();
            Map(x => x.SBBM);
            Map(x => x.SBMC);
            Map(x => x.SBTYPE);
            Map(x => x.CITYNAME);
            Map(x => x.CITYCODE);
            Map(x => x.SBXH);
            Map(x => x.SCCJ);
            Map(x => x.CCBH);
            Map(x => x.AZRQ);
            Map(x => x.GZRQ);
            Map(x => x.AZDW);
            Map(x => x.GLDW);
            Map(x => x.SYSM);
            Map(x => x.DCSM);
            Map(x => x.DCGHRQ);
            Map(x => x.BZ);
            Map(x => x.PIC);
            Map(x => x.FREQUENCY);
            Map(x => x.ADDTIME);
            Map(x => x.EXTENDCODE);
            Map(x => x.EXTENDCODE2);
            Map(x => x.EXTENDCODE3);
            Map(x => x.EXTENDCODE4);
            Map(x => x.EXTENDCODE5);

            Map(x => x.Agreement_Id);            
            // References<AgreementRecord>(o => o.Agreement).Not.LazyLoad().Column("agreement_id");
        }
    }
    public class DeviceRecord : Entity<int>
    {
        /// <summary>
        /// 主键id
        /// </summary>
       // public virtual int Id { get; set; }
        /// <summary>
        /// 设备编码，存储正元统一编码，行业+5位数字
        /// </summary>
        public virtual string SBBM { get; set; }

        //新添加字段，设备关联协议信息

        // public virtual AgreementRecord Agreement { get; set; }

        public virtual int Agreement_Id { get; set; }
        /// <summary>
        /// 行业分类
        /// </summary>
        public virtual string SBTYPE { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public virtual string SBMC { get; set; }
        /// <summary>
        /// 区划编号
        /// </summary>
        public virtual string CITYCODE { get; set; }
        /// <summary>
        /// 区划名称
        /// </summary>
        public virtual string CITYNAME { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public virtual string PIC { get; set; }
        /// <summary>
        /// 设备型号
        /// </summary>
        public virtual string SBXH { get; set; }
        /// <summary>
        /// 生产厂家
        /// </summary>
        public virtual string SCCJ { get; set; }
        /// <summary>
        /// 出厂编号，设备通信编码，原来的设备编号
        /// </summary>
        public virtual string CCBH { get; set; }
        /// <summary>
        /// 购买日期
        /// </summary>

        public virtual DateTime GZRQ { get; set; }
        /// <summary>
        /// 安装日期
        /// </summary>
        public virtual DateTime AZRQ { get; set; }
        /// <summary>
        /// 安装单位
        /// </summary>
        public virtual string AZDW { get; set; }
        /// <summary>
        /// 管理单位
        /// </summary>
        public virtual string GLDW { get; set; }
        /// <summary>
        /// 使用寿命
        /// </summary>
        public virtual decimal SYSM { get; set; }
        /// <summary>
        /// 电池寿命
        /// </summary>
        public virtual double DCSM { get; set; }

        /// <summary>
        /// 设备数据上报频率
        /// </summary>
        public virtual double FREQUENCY { get; set; }
        /// <summary>
        /// 电池更换日期
        /// </summary>
        public virtual DateTime DCGHRQ { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string BZ { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public virtual DateTime ADDTIME { get; set; }
        /// <summary>
        ///     EXTENDCODE
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
