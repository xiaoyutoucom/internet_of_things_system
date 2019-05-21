/*********************************************
* 命名空间:SmartKylinData.BaseModel
* 功 能： 综合展示列表中的实体
* 类 名： tableModel
* 作 者:  东腾
* 时 间： 2018-09-02 09:34:33 
**********************************************
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartKylinData.BaseModel
{
    public class TableModel
    {
        public string ConfigCode { get; set; }
        /// <summary>
        /// 行业名称
        /// </summary>
        public string IndustryName { get; set; }

        /// <summary>
        /// 监测点名称
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        /// 监测项名称
        /// </summary>
        public string ConfigName { get; set; }

        /// <summary>
        /// 设备位置描述
        /// </summary>
        public string ConfigDesc { get; set; }
        /// <summary>
        /// 在线离线状态
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 监测值
        /// </summary>
        public string ConfigValue { get; set; }
        /// <summary>
        /// 监测时间
        /// </summary>
        public DateTime?SaveDate { get; set; }
    }
}
