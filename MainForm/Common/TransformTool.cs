/*********************************************
* 命名空间:SmartKylinApp.Common
* 功 能： 数据转换工具
* 类 名： TransformTool
* 作 者:  东腾
* 时 间： 2018-08-17 13:03:42 
**********************************************
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace SmartKylinApp.Common
{
    /// <summary>
    ///     数据转化类
    /// </summary>
    public class TransformTool
    {
        public static DataConvert Convert=>new DataConvert();

    }

    public class DataConvert
    {
        /// <summary>
        ///     List转DataSet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public DataSet ListToDataSet<T>(List<T> list)
        {
            if (list == null || list.Count <= 0) return null;

            var ds = new DataSet();
            var dt = new DataTable(typeof(T).Name);
            DataColumn column;
            DataRow row;

            var myPropertyInfo = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var t in list)
            {
                if (t == null) continue;

                row = dt.NewRow();

                for (int i = 0, j = myPropertyInfo.Length; i < j; i++)
                {
                    var pi = myPropertyInfo[i];

                    var name = pi.Name;

                    if (dt.Columns[name] == null)
                    {
                        column = new DataColumn(name, pi.PropertyType);
                        dt.Columns.Add(column);
                    }

                    row[name] = pi.GetValue(t, null);
                }

                dt.Rows.Add(row);
            }

            ds.Tables.Add(dt);
            return ds;
        }

        /// <summary>
        ///     List对象转Datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static DataTable ListToTable<T>(List<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                var t = GetCoreType(prop.PropertyType);
                tb.Columns.Add(prop.Name, t);
            }

            foreach (var item in items)
            {
                var values = new object[props.Length];

                for (var i = 0; i < props.Length; i++) values[i] = props[i].GetValue(item, null);

                tb.Rows.Add(values);
            }

            return tb;
        }

        private static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                    return t;
                return Nullable.GetUnderlyingType(t);
            }

            return t;
        }

        private static bool IsNullable(Type t)
        {
            return !t.IsValueType || t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}