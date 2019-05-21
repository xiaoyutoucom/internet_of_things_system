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
    public class Smart_Kylin_Alert : BaseEntity
    {
        /// <summary>
        /// 日志文本
        /// </summary>
        public string LOG_TEXT { get; set; }
        /// <summary>
        /// 监测值
        /// </summary>
        public double CONFIG_VALUE { get; set; }
        /// <summary>
        /// 监测项编号
        /// </summary>
        public string CONFIG_CODE { get; set; }
        /// <summary>
        /// 保存日期
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime SAVE_DATE { get; set; }
        /// <summary>
        /// 是否有效 0 未确认 1确认
        /// </summary>
        public string ISCONFIRM { get; set; }
        /// <summary>
        /// 是否已处理，0未处理，1处理，-1表示中间状态
        /// </summary>
        public string ISOPER { get; set; }
        /// <summary>
        /// 报警级别
        /// </summary>
        public string LEVEL { get; set; }
        /// <summary>
        /// 处理人
        /// </summary>
        public string OPERNAME { get; set; }
        /// <summary>
        /// 处理方式，自动处置、撤销、上报城管
        /// </summary>
        public string OPERSTYLE { get; set; }
        /// <summary>
        /// 处理时间
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? OPERTIME { get; set; }
        /// <summary>
        /// 撤销执行人
        /// </summary>
        public string REVNAME { get; set; }
        /// <summary>
        /// 撤销时间
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? REVDATE { get; set; }
        /// <summary>
        /// 撤销说明
        /// </summary>
        public string REVDES { get; set; }

        public string EXTENDCODE { get; set; }

        public string EXTENDCODE2 { get; set; }
    }
}