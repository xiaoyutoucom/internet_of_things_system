using System;
using Robin.Domain.Entities;
using Robin.NHibernate.EntityMappings;

/**
* 命名空间: Smart.Core.Model
* 功 能： 传感器实体
* 类 名： SensorRecord
* 作 者:  张保东
* 时 间： 2018/1/23 10:40:27 
*/
namespace SmartKylinData.IOTModel
{

    public class SensorRecordMap : EntityMap<SensorRecord>
    {
        public SensorRecordMap()
            : base("smart_kylin_sensor")
        {
            Id(x => x.Id).GeneratedBy.Increment();
            //Map(x => x.Device);
            Map(x => x.CGQMC);
            Map(x => x.CGQBM);
            Map(x => x.CGQXH);
            Map(x => x.CGQLXBM);
            Map(x => x.CGQLXMC);
            Map(x => x.CCRQ);
            Map(x => x.SYSM);
            Map(x => x.BZ);
            Map(x => x.ADDTIME);
            Map(x => x.EXTENDCODE);
            Map(x => x.EXTENDCODE2);
            Map(x => x.EXTENDCODE3);
            Map(x => x.EXTENDCODE4);
            Map(x => x.EXTENDCODE5);
            References<DeviceRecord>(o => o.Device).Not.LazyLoad().Column("device_id");
        }
    }

    /// <summary>
    /// 传感器表
    /// </summary>
    public class SensorRecord : Entity<int>
    {
        /// <summary>
        /// 主键
        /// </summary>
        //public virtual int Id { get; set; }
        /// <summary>
        /// 关联的设备信息
        /// </summary>
        public virtual DeviceRecord Device { get; set; }
        /// <summary>
        /// 传感器编码
        /// </summary>
        public virtual string CGQBM { get; set; }
        /// <summary>
        /// 传感器名称
        /// </summary>
        public virtual string CGQMC { get; set; }
        /// <summary>
        /// 传感器型号
        /// </summary>
        public virtual string CGQXH { get; set; }
        /// <summary>
        /// 传感器类型编码（不同于监测项类型）
        /// </summary>
        public virtual string CGQLXBM { get; set; }
        /// <summary>
        /// 传感器类型名称（不同于监测项类型）分超声波式、压力式、投放式、光敏式、电磁式、电流式、激光式、力矩式、测距式、图像式
        /// </summary>
        public virtual string CGQLXMC { get; set; }
        /// <summary>
        /// 出厂日期
        /// </summary>
        public virtual DateTime CCRQ { get; set; }
        /// <summary>
        /// 使用寿命
        /// </summary>
        public virtual double SYSM { get; set; }
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
