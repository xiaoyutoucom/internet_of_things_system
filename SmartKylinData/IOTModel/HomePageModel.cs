/*********************************************
* 命名空间: Smart.Core.Model
* 功 能： 用于平台首页显示设备指标的实体变量
* 类 名： HomePageModel
* 作 者:  东腾
* 时 间： 2018/2/26 15:01:51 
**********************************************
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartKylinData.IOTModel{
    /// <summary>
    /// 用于平台首页显示设备指标的实体变量
    /// </summary>
public class HomePageModel
    {
        /// <summary>
        /// 行业类型
        /// </summary>
        public string IndustryType { get; set; }
        /// <summary>
        /// 设备总数
        /// </summary>
        public int DeviceTotal { get; set; }
        /// <summary>
        /// 设备在线数量
        /// </summary>
        public int DeviceOnline { get; set; }
        /// <summary>
        /// 设备离线数量
        /// </summary>
        public int DeviceOffLine { get; set; }
        /// <summary>
        /// 设备报警数量
        /// </summary>
        public int DeviceAlert { get; set; }

    }
}
