using Robin.Domain.Entities;
using Robin.NHibernate.EntityMappings;

/**
* 命名空间: Smart.Core.Model
* 功 能： N/A
* 类 名： CityRecord
* 作 者:  张保东
* 时 间： 2018/1/22 12:35:43 
*/
namespace SmartKylinData.IOTModel
{
    public class CityRecordMap : EntityMap<CityRecord>
    {
        public CityRecordMap()
            : base("sys_code_city")
        {
            //Id(x => x.CITYCODE).GeneratedBy.Increment();
            Id(x => x.CITYCODE);
            Map(x => x.CITYNAME);
            Map(x => x.CITYID);
            Map(x => x.PID);
        }
    }

    public class CityRecord : Entity<int>
    {

        //public virtual int Id { get; set; }
        /// <summary>
        /// 区号编码 主键
        /// </summary>

        public virtual decimal CITYCODE { get; set; }
        /// <summary>
        /// 行业分类
        /// </summary>
        public virtual string CITYNAME { get; set; }

        /// <summary>
        /// 区划编号
        /// </summary>
        public virtual decimal CITYID { get; set; }

        /// <summary>
        /// 行业分类
        /// </summary>
        public virtual string PID { get; set; }
    }
}
