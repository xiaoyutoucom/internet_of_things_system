/******************************************************
*******命名空间： SmartKylinApp.Common
*******类名称  ： TreeListModel
*******类说明  ： 树控件专用类
*******创建人  ： 东腾
*******创建时间： 2018-11-20 11:55:51
*******************************************************/

using System;

namespace SmartKylinApp.Common
{
    public class DataListModel
    {
        /// <summary>
        /// 监测项编号
        /// </summary>
        public virtual string CONFIG_CODE { get; set; }
        /// <summary>
        /// 监测项值
        /// </summary>
        public virtual double CONFIG_VALUE { get; set; }
        /// <summary>
        /// 监测项名称
        /// </summary>
        public virtual string CONFIGMC { get; set; }
        /// <summary>
        /// 监测点名称
        /// </summary>
        public virtual string MONITORMC { get; set; }

        /// <summary>
        /// 数据保存时间
        /// </summary>
        public virtual string SAVE_DATE { get; set; }
        /// <summary>
        /// 数据保存时间
        /// </summary>
        public virtual string LEVEL { get; set; }
        /// <summary>
        /// 检测项类型
        /// </summary>
        public virtual string MONITORTYPE{ get; set; }
        public string CCBH { get; internal set; }
    }
}