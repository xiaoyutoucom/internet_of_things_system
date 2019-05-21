/*********************************************
* 命名空间: Smart.Core.Model
* 功 能： Modbus数据实体
* 类 名： ModbusRecord
* 作 者:  东腾
* 时 间： 2018/3/29 13:28:21 
**********************************************
*/using System;
using Robin.Domain.Entities;
using Robin.NHibernate.EntityMappings;
namespace SmartKylinData.IOTModel
{
    public class ModbusRecordMap : EntityMap<ModbusRecord>
    {
        public ModbusRecordMap()
            : base("smart_kylin_modbus")
        {
            Id(x => x.Id).GeneratedBy.Increment();
            Map(x => x.DZW);
            Map(x => x.JCNAME);
            Map(x => x.STARTBYTE);
            Map(x => x.SLENGTH);
            Map(x => x.DTYPE);
            Map(x => x.UNIT);
            Map(x => x.ISFOMULA);
            Map(x => x.SFOMULA);
            Map(x => x.BZ);
            Map(x => x.BZ2);
            Map(x => x.BZ3);
            Map(x => x.BZ4);
            References<SensorRecord>(a=>a.SENSOR).Not.LazyLoad().Column("sensor_id");
        }
    }
    public class ModbusRecord:Entity<int>
    {

        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }
        /// <summary>
        /// 传感器ID
        /// </summary>
        public virtual SensorRecord SENSOR { get; set; }
        /// <summary>
        /// Moudbus地址位
        /// </summary>
        public virtual string DZW { get; set; }
        /// <summary>
        /// 监测项名称
        /// </summary>
        public virtual string JCNAME { get; set; }
        /// <summary>
        /// 开始字节
        /// </summary>
        public virtual double STARTBYTE { get; set; }
        /// <summary>
        /// 数据长度
        /// </summary>
        public virtual double SLENGTH { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public virtual string DTYPE { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public virtual string UNIT { get; set; }
        /// <summary>
        ///  是否使用公式 1使用 0不使用 默认0
        /// </summary>
        public virtual string ISFOMULA { get; set; }
        /// <summary>
        ///  计算公式
        /// </summary>
        public virtual string SFOMULA { get; set; }
        /// <summary>
        ///  备注
        /// </summary>
        public virtual string BZ { get; set; }
        /// <summary>
        ///  备注2
        /// </summary>
        public virtual string BZ2 { get; set; }
        /// <summary>
        ///  备注3
        /// </summary>
        public virtual string BZ3 { get; set; }
        /// <summary>
        ///  备注4
        /// </summary>
        public virtual string BZ4 { get; set; }
        /// <summary>
        ///  备注5
        /// </summary>
        public virtual string BZ5 { get; set; }
    }

}
