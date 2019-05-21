using System;
using Robin.Domain.Entities;
using Robin.NHibernate.EntityMappings;

/**
* 命名空间: Smart.Core.Model
* 功 能： 监测项实体类
* 类 名： ConfigRecord
* 作 者:  张保东
* 时 间： 2018/1/24 11:17:46 
*/
namespace SmartKylinData.IOTModel
{
    public class ConfigRecordMap : EntityMap<ConfigRecord>
    {
        public ConfigRecordMap()
            : base("smart_kylin_config")
        {
            Id(x => x.Id).GeneratedBy.Increment();
            Map(x => x.CONFIG_CODE);
            Map(x => x.CONFIG_DESC);
            Map(x => x.CGROUP);
            Map(x => x.TEMPLATE);
            Map(x => x.PAGE_X);
            Map(x => x.PAGE_Y);
            Map(x => x.MIN_VALUE);
            Map(x => x.MAX_VALUE);
            Map(x => x.MIN_MIN_VALUE);
            Map(x => x.MAX_MAX_VALUE);
            Map(x => x.ORDER_NUM);
            Map(x => x.ALERTRATE);
            Map(x => x.NBSS);
            Map(x => x.COLOR_VALUE);
            Map(x => x.PRECISION);
            Map(x => x.L1_START);
            Map(x => x.L1_END);
            Map(x => x.L1_COLOR_VALUE);
            Map(x => x.L1_RETURNVALUE);
            Map(x => x.L2_START);
            Map(x => x.L2_END);
            Map(x => x.L2_COLOR_VALUE);
            Map(x => x.L2_RETURNVALUE);
            Map(x => x.L3_START);
            Map(x => x.L3_END);
            Map(x => x.L3_COLOR_VALUE);
            Map(x => x.L3_RETURNVALUE);
            Map(x => x.REMARK);
            Map(x => x.ENABLE);
            
            Map(x => x.ISPUSH);
            Map(x => x.ENABLEDOWN);
            Map(x => x.L1_STARTDOWN);
            Map(x => x.L1_ENDDOWN);
            Map(x => x.L1_COLOR_VALUEDOWN);
            Map(x => x.L1_DOWNRETURNVALUE);
            Map(x => x.L2_STARTDOWN);
            Map(x => x.L2_ENDDOWN);
            Map(x => x.L2_COLOR_VALUEDOWN);
            Map(x => x.L2_DOWNRETURNVALUE);
            Map(x => x.L3_STARTDOWN);
            Map(x => x.L3_ENDDOWN);
            Map(x => x.L3_COLOR_VALUEDOWN);
            Map(x => x.L3_DOWNRETURNVALUE);
            Map(x => x.VARIABLE_NAME);
            Map(x => x.UNITS);
            Map(x => x.TAG_VALUE);
            Map(x => x.SAVE_DATE);
            Map(x => x.PermissionType);
            Map(x => x.ReportType);
            Map(x => x.ADDTIME);
            Map(x => x.EXTENDCODE);
            Map(x => x.EXTENDCODE2);
            Map(x => x.EXTENDCODE3);
            Map(x => x.EXTENDCODE4);
            Map(x => x.EXTENDCODE5);
            Map(x => x.ALERTLEVEL);
            
            References(o => o.STATIONID).Not.LazyLoad().Column("stationid_id");
            References(o => o.TAGID).Not.LazyLoad().Column("tagid_id");
            References(o => o.SENSORID).Not.LazyLoad().Column("sensorid_id");
        }
    }


    /// <summary>
    ///     监测项配置表
    /// </summary>
    public class ConfigRecord : Entity<int>
    {
        /// <summary>
        ///     主键Id
        /// </summary>
        /// <summary>
        ///     监测项编码
        /// </summary>
        public virtual string CONFIG_CODE { get; set; }

        /// <summary>
        ///     监测项描述
        /// </summary>
        public virtual string CONFIG_DESC { get; set; }

        /// <summary>
        ///     是否推送
        /// </summary>
        public virtual string ISPUSH { get; set; }

        /// <summary>
        ///     关联监测点id
        /// </summary>
        public virtual BasicMonitorRecord STATIONID { get; set; }

        /// <summary>
        ///     关联设备编号
        /// </summary>
        public virtual SensorRecord SENSORID { get; set; }

        /// <summary>
        ///     关联检测项编号
        /// </summary>
        public virtual TagInfoRecord TAGID { get; set; }

        ///// <summary>
        ///// 监测项类型编号
        ///// </summary>
        //public virtual string TAG_KEY { get; set; }
        ///// <summary>
        ///// 监测项类型名称
        ///// </summary>
        //public virtual string TAG_NAME { get; set; }
        /// <summary>
        ///     分组
        /// </summary>
        public virtual string CGROUP { get; set; }

        /// <summary>
        ///     消息模板
        /// </summary>
        public virtual string TEMPLATE { get; set; }

        /// <summary>
        ///     变量名称
        /// </summary>
        public virtual string VARIABLE_NAME { get; set; }

        /// <summary>
        ///     页面相对位置X
        /// </summary>
        public virtual decimal PAGE_X { get; set; }

        /// <summary>
        ///     页面相对位置Y
        /// </summary>
        public virtual double PAGE_Y { get; set; }

        /// <summary>
        ///     最小正常值
        /// </summary>
        public virtual double MIN_VALUE { get; set; }

        /// <summary>
        ///     最大正常值
        /// </summary>
        public virtual double MAX_VALUE { get; set; }

        /// <summary>
        ///     量程最小值
        /// </summary>
        public virtual double MIN_MIN_VALUE { get; set; }

        /// <summary>
        ///     量程最大值
        /// </summary>
        public virtual double MAX_MAX_VALUE { get; set; }

        /// <summary>
        ///     单位
        /// </summary>

        public virtual string UNITS { get; set; }

        /// <summary>
        ///     排序号
        /// </summary>
        public virtual int? ORDER_NUM { get; set; }

        /// <summary>
        ///     监测值是否是正常值
        /// </summary>
        public virtual string ISNORMAL { get; set; }

        /// <summary>
        ///     报警级别
        /// </summary>
        public virtual string ALERTLEVEL { get; set; }

        /// <summary>
        ///     上报频率
        /// </summary>
        public virtual double? ALERTRATE { get; set; }

        /// <summary>
        ///     最后更新时间
        /// </summary>
        public virtual DateTime? SAVE_DATE { get; set; }

        /// <summary>
        ///     最后更新值
        /// </summary>
        public virtual double? TAG_VALUE { get; set; }

        /// <summary>
        ///     内部设施如泵站的水泵
        /// </summary>
        public virtual string NBSS { get; set; }

        /// <summary>
        ///     彩色值
        /// </summary>
        public virtual string COLOR_VALUE { get; set; }

        /// <summary>
        ///     显示精度
        /// </summary>
        public virtual double? PRECISION { get; set; }

        /// <summary>
        ///     一级预警范围起始 最高
        /// </summary>
        public virtual double L1_START { get; set; }

        /// <summary>
        ///     一级预警范围终止
        /// </summary>
        public virtual double L1_END { get; set; }

        /// <summary>
        ///     一级预警彩色值
        /// </summary>
        public virtual string L1_COLOR_VALUE { get; set; }

        /// <summary>
        ///     一级预警彩色值
        /// </summary>
        public virtual double L1_RETURNVALUE { get; set; }

        /// <summary>
        ///     二级预警范围起始
        /// </summary>
        public virtual double L2_START { get; set; }

        /// <summary>
        ///     二级预警范围终止
        /// </summary>
        public virtual double L2_END { get; set; }

        /// <summary>
        ///     二级预警彩色值
        /// </summary>
        public virtual string L2_COLOR_VALUE { get; set; }

        /// <summary>
        ///     二级预警返回值
        /// </summary>
        public virtual double L2_RETURNVALUE { get; set; }

        /// <summary>
        ///     三级预警范围起始
        /// </summary>
        public virtual double L3_START { get; set; }

        /// <summary>
        ///     三级预警范围终止
        /// </summary>
        public virtual double L3_END { get; set; }

        /// <summary>
        ///     三级预警彩色值
        /// </summary>
        public virtual string L3_COLOR_VALUE { get; set; }

        /// <summary>
        ///     三级预警返回值
        /// </summary>
        public virtual double L3_RETURNVALUE { get; set; }

        /// <summary>
        ///     REMARK
        /// </summary>
        public virtual string REMARK { get; set; }

        /// <summary>
        ///     添加时间
        /// </summary>
        public virtual DateTime ADDTIME { get; set; }

        /// <summary>
        ///     EXTENDCODE
        /// </summary>
        public virtual string EXTENDCODE { get; set; }

        /// <summary>
        ///     EXTENDCODE2
        /// </summary>
        public virtual string EXTENDCODE2 { get; set; }

        /// <summary>
        ///     EXTENDCODE3
        /// </summary>
        public virtual string EXTENDCODE3 { get; set; }

        /// <summary>
        ///     EXTENDCODE4
        /// </summary>
        public virtual string EXTENDCODE4 { get; set; }

        /// <summary>
        ///     EXTENDCODE5
        /// </summary>
        public virtual string EXTENDCODE5 { get; set; }

        /// <summary>
        ///     是否启用报警
        /// </summary>
        public virtual string ENABLE { get; set; }
        
        /// <summary>
        ///     是否启用下行预警范围 1启用 0停用
        /// </summary>
        public virtual string ENABLEDOWN { get; set; }

        /// <summary>
        ///     下行一级预警范围起始 最高
        /// </summary>
        public virtual double L1_STARTDOWN { get; set; }

        /// <summary>
        ///     下行一级预警范围终止
        /// </summary>
        public virtual double L1_ENDDOWN { get; set; }

        /// <summary>
        ///     下行一级预警彩色值
        /// </summary>
        public virtual string L1_COLOR_VALUEDOWN { get; set; }

        /// <summary>
        ///     下行一级预警返回值
        /// </summary>
        public virtual double L1_DOWNRETURNVALUE { get; set; }

        /// <summary>
        ///     下行二级预警范围起始
        /// </summary>
        public virtual double L2_STARTDOWN { get; set; }

        /// <summary>
        ///     下行二级预警范围终止
        /// </summary>
        public virtual double L2_ENDDOWN { get; set; }

        /// <summary>
        ///     下行二级预警彩色值
        /// </summary>
        public virtual string L2_COLOR_VALUEDOWN { get; set; }

        /// <summary>
        ///     下行二级预警返回值
        /// </summary>
        public virtual double L2_DOWNRETURNVALUE { get; set; }

        /// <summary>
        ///     下行三级预警范围起始
        /// </summary>
        public virtual double L3_STARTDOWN { get; set; }

        /// <summary>
        ///     下行三级预警范围终止
        /// </summary>
        public virtual double L3_ENDDOWN { get; set; }

        /// <summary>
        ///     下行三级预警彩色值
        /// </summary>
        public virtual string L3_COLOR_VALUEDOWN { get; set; }

        /// <summary>
        ///     下行三级预警返回值
        /// </summary>
        public virtual double L3_DOWNRETURNVALUE { get; set; }

        //新增了两个字段
        /// <summary>
        /// 权限类别   1：公有权限，2：私有权限
        /// </summary>
        public virtual string PermissionType { get; set; }

        /// <summary>
        /// 报表类型  1：通用报表，2：定制报表,3:其他，默认1
        /// </summary>
        public virtual string ReportType { get; set; }
    }
}