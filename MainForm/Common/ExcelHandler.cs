/******************************************************
*******命名空间： SmartKylinApp.Common
*******类名称  ： ExcelHandler
*******类说明  ： 
*******创建人  ： 东腾
*******创建时间： 2018-11-27 9:22:37
*******************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using SmartKylinData.BaseModel;
using SmartKylinData.IOTModel;

namespace SmartKylinApp.Common
{
    class ExcelHandler
    {
       
    }

    public class OutputFile
    {
        //写文件
        private static bool WriteFile(string filename, string strInfo)
        {
            try
            {
                //创建文件
                if (!File.Exists(filename))
                {
                    using (var sw = File.CreateText(filename))
                    { }
                }
                //写文件
                using (var sw = File.AppendText(filename))
                {
                    sw.WriteLine(strInfo);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool OutputHistory_CSV(string filename, List<Smart_Kylin_History> dataList)
        {
            try
            {
                var strCol = "";
                //for (int i = 0; i < UPPER; i++)
                //{

                //}
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        //导入数据
        public static DataSet LoadDataFromExcel(string filePath, ref string error)
        {
            try
            {
                var conStr = "";

                conStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1';";

                var conn = new OleDbConnection(conStr);

                conn.Open();

                var sql = @"SELECT * FROM [Sheet0$]";

                var excel = new OleDbDataAdapter(sql, conn);

                var dt = new DataSet();

                excel.Fill(dt, "数据源");

                conn.Close();

                return dt;
            }
            catch (Exception e)
            {
                error = e.Message;
                return null;
            }
        }
    }
}
