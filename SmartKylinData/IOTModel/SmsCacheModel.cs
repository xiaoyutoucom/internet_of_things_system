/*********************************************
* 命名空间:Smart.Core.Model
* 功 能： 短信发送信息缓存实体
* 类 名： SmsCacheModel
* 作 者:  东腾
* 时 间： 2018/6/14 16:21:27 
**********************************************
*/
using System;
using System.Linq;

namespace SmartKylinData.IOTModel{
    public class SmsCacheModel
    {
        public string StationName { get; set; }
        /// <summary>
        /// 发送频率
        /// </summary>
        public decimal Interval { get; set; }
        /// <summary>
        /// 变化差值 </summary>
        public decimal ChangeDiff { get; set; }
        /// <summary>
        /// 模板Id
        /// </summary>
        public string TemplateId { get; set; }
        /// <summary>
        /// 是否发送
        /// </summary>
        public int IsEnabled { get; set; }
        /// <summary>
        /// 用户电话
        /// </summary>
        public string PhoneString { get; set; }
        /// <summary>
        /// 最后一次保存值
        /// </summary>
        public decimal LastValue { get; set; }
        /// <summary>
        /// 最后一次保存时间
        /// </summary>
        public DateTime LastTime { get; set; }
    }
}
