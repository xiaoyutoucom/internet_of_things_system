/*********************************************
* 命名空间: Smart.TaskCenter.MongoJob.Model
* 功 能： 历史数据表结构实体模型
* 类 名： HistoryModel
* 作 者:  东腾
* 时 间： 2018/4/2 10:03:22 
**********************************************
*/

using Mongodb.Data;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SmartKylinData.BaseModel
{
    public class Smart_Kylin_Runtime : BaseEntity
    {

        /// <summary>
        /// 监测项编号
        /// </summary>
        public virtual string CONFIG_CODE { get; set; }
        /// <summary>
        /// 设备状态，1在线，0不在线
        /// </summary>
        public virtual string STATUS { get; set; }
        /// <summary>
        /// 监测项值
        /// </summary>
        public virtual double CONFIG_VALUE { get; set; }
        /// <summary>
        /// 是否报警。0未报警，1报警
        /// 新增字段20181213
        /// </summary>
        public virtual string LEVEL { get; set; }

        /// <summary>
        /// 数据保存时间
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public virtual DateTime? SAVE_DATE { get; set; }

        /// <summary>
        /// 数据上传的时间间隔
        /// </summary>
        public virtual double TIME_SPAN { get; set; }

        public virtual string MONITORTYPE { get; set; }
    }

}