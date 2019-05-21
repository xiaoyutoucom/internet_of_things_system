/******************************************************
*******命名空间： SmartKylinApp.Common
*******类名称  ： TreeListModel
*******类说明  ： 树控件专用类
*******创建人  ： 东腾
*******创建时间： 2018-11-20 11:55:51
*******************************************************/

namespace SmartKylinApp.Common
{
    public class TreeListModel
    {
        public string ID { get; set; } = "";

        public string ParentID { get; set; } = "";

        public string Name { get; set; } = string.Empty;
    }
}