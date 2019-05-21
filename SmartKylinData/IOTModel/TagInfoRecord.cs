using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robin.Domain.Entities;
using Robin.NHibernate.EntityMappings;

/**
* 命名空间:Smart.Core.Model
* 功 能： 监测项配置表
* 类 名： TagInfoRecord
* 作 者:  张保东
* 时 间： 2018/1/24 11:24:26 
*/
namespace SmartKylinData.IOTModel
{
    public class TagInfoRecordMap : EntityMap<TagInfoRecord>
    {
        public TagInfoRecordMap()
            : base("smart_kylin_taginfo")
        {
            Id(x => x.Id).GeneratedBy.Increment();
            Map(x => x.TAG_KEY);
            Map(x => x.TAG_NAME);
            Map(x => x.EXPLAIN);
            Map(x => x.NORMAL_START);
            Map(x => x.NORMAL_END);
            Map(x => x.COLOR_VALUE);
            Map(x => x.L1_START);
            Map(x => x.L1_END);
            Map(x => x.L1_COLOR_VALUE);
            Map(x => x.L2_START);
            Map(x => x.L2_END);
            Map(x => x.L2_COLOR_VALUE);
            Map(x => x.L3_START);
            Map(x => x.L3_END);
            Map(x => x.L3_COLOR_VALUE);
            Map(x => x.EXTENDCODE);
            Map(x => x.EXTENDCODE2);
            Map(x => x.EXTENDCODE3);
            Map(x => x.EXTENDCODE4);
            Map(x => x.EXTENDCODE5);
            Map(x => x.UNITS);
            Map(x => x.PRECISION);
            Map(x => x.ALERTRATE);
        }
    }
    public class TagInfoRecord:Entity
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public virtual int Id { get; set; }
        /// <summary>
        /// 监测项编号 
        /// </summary>
        /// 
        public virtual string TAG_KEY { get; set; }
        /// <summary>
        ///     单位
        /// </summary>

        public virtual string UNITS { get; set; }
        /// <summary>
        ///     上报频率
        /// </summary>
        public virtual double? ALERTRATE { get; set; }
        /// <summary>
        ///     显示精度
        /// </summary>
        public virtual double? PRECISION { get; set; }
        /// <summary>
        /// 检测项名称 
        /// </summary>
        public virtual string TAG_NAME { get; set; }
        /// <summary>
        /// 描述 
        /// </summary>
        public virtual string EXPLAIN { get; set; }

        /// <summary>
        /// 正常范围起始
        /// </summary>
        public virtual double NORMAL_START { get; set; }
        /// <summary>
        /// 正常范围终止
        /// </summary>
        public virtual double NORMAL_END { get; set; }
        /// <summary>
        /// 正常彩色值 
        /// </summary>
        public virtual string COLOR_VALUE { get; set; }
        /// <summary>
        /// 一级预警范围起始
        /// </summary>
        public virtual double L1_START { get; set; }
        /// <summary>
        /// 一级预警范围终止
        /// </summary>
        public virtual double L1_END { get; set; }
        /// <summary>
        /// 一级预警彩色值 
        /// </summary>
        public virtual string L1_COLOR_VALUE { get; set; }
        /// <summary>
        /// 二级预警范围起始
        /// </summary>
        public virtual double L2_START { get; set; }
        /// <summary>
        /// 二级预警范围终止
        /// </summary>
        public virtual double L2_END { get; set; }
        /// <summary>
        /// 二级预警彩色值 
        /// </summary>
        public virtual string L2_COLOR_VALUE { get; set; }
        /// <summary>
        /// 三级预警范围起始
        /// </summary>
        public virtual double L3_START { get; set; }
        /// <summary>
        /// 三级预警范围终止
        /// </summary>
        public virtual double L3_END { get; set; }
        /// <summary>
        /// 三级预警彩色值 
        /// </summary>
        public virtual string L3_COLOR_VALUE { get; set; }
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
