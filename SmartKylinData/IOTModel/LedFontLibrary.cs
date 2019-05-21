/*********************************************
* 命名空间:Smart.Core.Model
* 功 能： led信息字库
* 类 名： LedFontLibrary
* 作 者:  东腾
* 时 间： 2018/6/24 11:29:39 
**********************************************
*/using System;
using Robin.Domain.Entities;
using Robin.NHibernate.EntityMappings;

namespace SmartKylinData.IOTModel
{
    public class LedFontLibraryMap : EntityMap<LedFontLibrary>
    {
        public LedFontLibraryMap() : base("smart_kylin_fontlibrary")
        {
            Id(x => x.Id).GeneratedBy.Increment();
            Map(x => x.WEATHERTYPE);
            Map(x => x.WATERLEVEL);
            Map(x => x.WEATHERDESC);
            Map(x => x.CONTENT);
            Map(x => x.EXTEND);
            Map(x => x.EXTEND2);
            Map(x => x.EXTEND3);
        }
    }
    public class LedFontLibrary:Entity<int>
    {
        /// <summary>
        /// 天气类型，0,：正常，1：雨天，2：雪天
        /// </summary>
        public virtual int WEATHERTYPE { get; set; }

        public virtual string WEATHERDESC { get; set; }
        /// <summary>
        /// 水位级别，范围之间用小短线分割，如1-10,2-13
        /// </summary>
        public virtual string WATERLEVEL { get; set; }
        /// <summary>
        /// 信息内容
        /// </summary>
        public virtual string CONTENT { get; set; }
        /// <summary>
        /// 备用1
        /// </summary>
        public virtual string EXTEND { get; set; }
        public virtual string EXTEND2 { get; set; }
        public virtual string EXTEND3 { get; set; }
    }
}
