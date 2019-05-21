/*********************************************
* 命名空间: Smart.Core.Model
* 功 能： 地图坐标系配置信息类
* 类 名： GeoInfoModel
* 作 者:  东腾
* 时 间： 2018/3/5 14:33:51 
**********************************************
*/
namespace SmartKylinData.IOTModel
{
    /// <summary>
    /// 地图坐标系配置信息类
    /// </summary>
    public class GeoInfoModel
    {
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
