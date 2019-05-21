/*********************************************
* 命名空间: Smart.TaskCenter.MongoJob.Model
* 功 能： 历史数据表结构实体模型
* 类 名： HistoryModel
* 作 者:  东腾
* 时 间： 2018/4/2 10:03:22 
**********************************************
*/

using System;
using Mongodb.Data;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartKylinData.BaseModel
{
    public class Smart_Kylin_History:BaseEntity
    {
        /// <summary>
        ///     监测项编号
        /// </summary>
        public virtual string CONFIG_CODE { get; set; }

        /// <summary>
        ///     监测值
        /// </summary>
        public virtual double CONFIG_VALUE { get; set; }

        /// <summary>
        ///     监测时间
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public virtual DateTime? SAVE_DATE { get; set; }
    }
}