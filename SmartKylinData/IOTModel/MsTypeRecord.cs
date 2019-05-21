

/**
* 命名空间: Smart.Core.Model
* 功 能： N/A
* 类 名： MsTypeRecord
* 作 者:  张保东
* 时 间： 2018/1/24 16:39:43 
*/

using Robin.Domain.Entities;
using Robin.NHibernate.EntityMappings;

namespace SmartKylinData.IOTModel
{
    public class MsTypeRecordMap : EntityMap<MsTypeRecord>
    {
        public MsTypeRecordMap()
            : base("smart_kylin_mstype")
        {
            Id(x => x.Id).GeneratedBy.Increment();
            Map(x => x.TYPE_KEY);
            Map(x =>x.TYPE_NAME);
            Map(x => x.LXBS);
            Map(x => x.TEMPLATE);
            Map(x => x.SPACESERVICEURL);
            Map(x => x.PANELCONFIG);
            Map(x => x.TEMPLATEPATH);
            Map(x => x.SCRIPTPATH);
            Map(x => x.CSSPATH);
            Map(x => x.SPATIALQUERYID);
            Map(x => x.SPATIALQUERYNAME);
            Map(x => x.SPATIALQUERYBY);
            Map(x => x.ICON);
            Map(x =>x.EXPLAIN);
            Map(x => x.EXTENDCODE);
            Map(x => x.EXTENDCODE2);
            Map(x => x.EXTENDCODE3);
            Map(x => x.EXTENDCODE4);
            Map(x=> x.EXTENDCODE5);
        }
    }
    public class MsTypeRecord:Entity<int>
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        //public virtual int Id { get; set; }
        /// <summary>
        /// 监测点类型编号
        /// </summary>
        public virtual string TYPE_KEY { get; set; }
        /// <summary>
        ///监测点类型名称 
        /// </summary>
        public virtual string TYPE_NAME { get; set; }
        /// <summary>
        ///类型标示
        /// </summary>
        public virtual string LXBS { get; set; }

        /// <summary>
        ///消息模板 用户短信发送时的模板
        /// </summary>
        public virtual string TEMPLATE { get; set; }

        /// <summary>
        ///空间服务地址
        /// </summary>
        public virtual string SPACESERVICEURL { get; set; }


        /// <summary>
        ///自定义显示配置
        /// </summary>
        public virtual string PANELCONFIG { get; set; }



        /// <summary>
        ///模板HTM路径
        /// </summary>
        public virtual string TEMPLATEPATH { get; set; }

        /// <summary>
        ///模板脚本路径
        /// </summary>
        public virtual string SCRIPTPATH { get; set; }

        /// <summary>
        ///模板CSS路径
        /// </summary>
        public virtual string CSSPATH { get; set; }


        /// <summary>
        ///空间查询ID字段
        /// </summary>
        public virtual string SPATIALQUERYID { get; set; }

        /// <summary>
        ///空间查询NAME字段
        /// </summary>
        public virtual string SPATIALQUERYNAME { get; set; }

        /// <summary>
        ///空间查询备用字段
        /// </summary>
        public virtual string SPATIALQUERYBY { get; set; }

        /// <summary>
        ///图片URL
        /// </summary>
        public virtual string ICON { get; set; }

        /// <summary>
        ///描述
        /// </summary>
        public virtual string EXPLAIN { get; set; }
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
