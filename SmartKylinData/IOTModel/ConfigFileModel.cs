/*********************************************
* 命名空间: Smart.Core.Model
* 功 能： 用户格式化和实例化数据库及服务配置
* 类 名： ConfigFileModel
* 作 者:  东腾
* 时 间： 2018/2/6 14:15:26 
**********************************************
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartKylinData.IOTModel
{
    public class ConfigFileModel
    {
        public string SigServer { get; set; }
        public string SigPort { get; set; }
        public string SigSName { get; set; }

        public string PgServer { get; set; }
        public string PgPort { get; set; }
        public string PgDatabase { get; set; }
        public string PgUserId { get; set; }
        public string PgPassword { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 区/县
        /// </summary>
        public string District { get; set; }
        /// <summary>
        /// 坐标系统，一般包括Xian80,国家2000,北京54
        /// </summary>
        public string CoordSystem { get; set; }
        /// <summary>
        /// 分度带
        /// </summary>
        public int DivBelt { get; set; }
        /// <summary>
        /// 中央经线值
        /// </summary>
        public int CentialMeridian { get; set; }
        /// <summary>
        /// x方向坐标偏移
        /// </summary>
        public double DeviationX { get; set; }
        /// <summary>
        /// y方向坐标偏移
        /// </summary>
        public double DeviationY { get; set; }

        public bool IsBigNum { get; set; }
    }
}
