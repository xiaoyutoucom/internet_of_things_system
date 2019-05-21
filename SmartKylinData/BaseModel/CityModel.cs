/*********************************************
* 命名空间:SmartKylinData.BaseModel
* 功 能： 城市区划实体
* 类 名： CityModel
* 作 者:  东腾
* 时 间： 2018-08-17 10:53:32 
**********************************************
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartKylinData.BaseModel
{
    public class CityModel
    {
        public virtual string CITYCODE { get; set; }

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
