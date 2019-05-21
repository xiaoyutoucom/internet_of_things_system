using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using NPOI.HSSF.UserModel;
using ServiceStack;
using SmartKylinApp.Common;
using SmartKylinData.IOTModel;
using NPOI.SS.UserModel;
using CefSharp.Structs;
using NPOI.SS.Util;
using SmartKylinData.BaseModel;
using log4net;
using NPOI.HSSF.Util;

namespace SmartKylinApp.View.BaseConfig
{
    public partial class Monitordownload : DevExpress.XtraEditors.XtraForm
    {
        private ILog _log = LogManager.GetLogger("Monitordownload");
        public Monitordownload()
        {
            InitializeComponent();
            //禁止右键菜单
            layoutControl1.AllowCustomization = false;
            tree_mstype.Properties.TreeList.OptionsMenu.EnableColumnMenu = false;
        }

        private List<DeviceRecord> DeviceList;
        private string mstype;
        private void Monitordownload_Load(object sender, EventArgs e)
        {
            
            BindTree();

         
        }
        //绑定行业信息
        private void BindTree()
        {
            var datas = GlobalHandler.mstyperesp.GetAllList();
            if (datas == null) return;
            var list = new List<TreeListModel>();
            var dt1 = datas.Where(a => a.TYPE_KEY.ToString().Length == 2);
            dt1.Each(a => list.Add(new TreeListModel() { ID = a.TYPE_KEY, ParentID = "1", Name = a.TYPE_NAME }));
            var dt2 = datas.Where(a => a.TYPE_KEY.ToString().Length == 4);
            dt2.Each(a => list.Add(new TreeListModel() { ID = a.TYPE_KEY, ParentID = a.TYPE_KEY.Substring(0, 2), Name = a.TYPE_NAME }));
            var dt3 = datas.Where(a => a.TYPE_KEY.ToString().Length == 6);
            dt3.Each(a => list.Add(new TreeListModel() { ID = a.TYPE_KEY, ParentID = a.TYPE_KEY.Substring(0, 4), Name = a.TYPE_NAME }));

            tree_mstype.Properties.DataSource = list;
            tree_mstype.Properties.DisplayMember = "Name";
            tree_mstype.Properties.ValueMember = "ID";
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //点击下载
            FileStream fs = null;
            try
            {
                var code = (tree_mstype.GetSelectedDataRow() as TreeListModel)?.ID;
                var name = (tree_mstype.GetSelectedDataRow() as TreeListModel)?.Name;
                if (code == ""|| code==null)
                {
                   XtraMessageBox.Show(@"请选择行业类型");
                    return;
                }
                var saveFileName = "监测点信息导入模板.xls";
                fs = new FileStream(saveFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                var workbook = new HSSFWorkbook();
                var sheet = workbook.CreateSheet();
                //(Optional) set the width of the columns
                //"监测项类型名称", "监测项类型编码",
                string[] names = new string[] { "监测点类型名称", "监测点类型编码（必填）", "区划信息名称:", "区划信息编码（必填）", "监测点编号", "监测点名称", "部件编码", "消息模板", "通信方式", "监测点描述", "X", "Y", "OPC的WebUrl", "上图片URL", "级别" };
                var headerRow = sheet.CreateRow(0);
                for (int i = 0; i < names.Length; i++)
                {
                    sheet.SetColumnWidth(i, 30 * 256);
                    headerRow.CreateCell(i).SetCellValue(names[i]);
                }

                sheet.CreateFreezePane(0, 1, 0, 1);
                string[] names2 = new string[] { "（示例数据请手动删除）采集点", "030301",   "埇桥区",  "341302",   "", "二水厂",  "空",    "", "", "二水厂",  "495864.46",    "721852.26"};
                var headerRow2 = sheet.CreateRow(1);
                for (int i = 0; i < names2.Length; i++)
                {
                    sheet.SetColumnWidth(i, 30 * 256);
                    headerRow2.CreateCell(i).SetCellValue(names2[i]);
                }
                //获取选中的监测点信息
                if (code.Length==6)
                {
                    var row = sheet.CreateRow(2);
                    row.CreateCell(0).SetCellValue(name);
                    row.CreateCell(1).SetCellValue(code);
                }
                else
                {
                    var datas = GlobalHandler.mstyperesp.GetAllList(a => a.TYPE_KEY.StartsWith(code) ).Where(a => a.TYPE_KEY.Length==6).ToList();
                    for (var i = 0; i < datas.Count; i++)
                    {
                        var row = sheet.CreateRow(i + 2);
                        row.CreateCell(0).SetCellValue(datas[i].TYPE_NAME);
                        row.CreateCell(1).SetCellValue(datas[i].TYPE_KEY);
                    }

                }

                
                var saveDialog = new SaveFileDialog
                {
                    DefaultExt = "xls",
                    Filter = @"Excel文件|*.xls;*.xlsx",
                    FileName = saveFileName
                };
                saveDialog.ShowDialog(); saveFileName = saveDialog.FileName;
                if (saveFileName.IndexOf(":", StringComparison.Ordinal) < 0) return; //被点了取消

                if (saveFileName == "") return;

                try
                {
                    #region 生成参考文档
                    fs = File.OpenWrite(saveDialog.FileName);
                    workbook.Write(fs);
                    fs?.Close();
                    try
                    {
                        var saveFileName1 = "监测点信息填写说明.xls";
                        var fs1 = new FileStream(saveFileName1, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        var workbook1 = new HSSFWorkbook();
                        var sheet1 = workbook1.CreateSheet();

                        sheet1.SetColumnWidth(0, 20 * 256);
                        var headerRowTop = sheet1.CreateRow(0);
                        headerRowTop.CreateCell(0).SetCellValue($"监测点编码: 可为空,为空时系统自动按规则生成,格式： 区划号码+监测点类型编号+七位数字 示例：3705020111960000001");
                        sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, 6));
                        IFont font = workbook1.CreateFont();
                        font.FontHeightInPoints = 14;//设置字体大小
                        font.Color = HSSFColor.Red.Index;
                        ICellStyle style = workbook1.CreateCellStyle();
                        style.SetFont(font);
                        headerRowTop.GetCell(0).CellStyle = style;
                        var headerRow1 = sheet1.CreateRow(1);
                        headerRow1.Height = 300;
                        string[] names1 = new string[] { "监测点类型名称", "监测点类型编码", "区划信息名称:", "区划信息编码" };
                        for (int i = 0; i < names1.Length; i++)
                        {
                            sheet1.SetColumnWidth(i, 30 * 256);
                            headerRow1.CreateCell(i).SetCellValue(names1[i]);
                        }

                        sheet1.CreateFreezePane(0, 2, 0, 2);

                        //获取选中的监测点信息
                        //var index1 = gridView1.GetSelectedRows();

                        var datas = GlobalHandler.mstyperesp.GetAllList(a => a.TYPE_KEY.ToString().StartsWith(code)).Where(a => a.TYPE_KEY.Length == 6).ToList();
                        for (var i = 1; i < datas.Count + 1; i++)
                        {
                            var row = sheet1.CreateRow(i + 1);
                            row.CreateCell(0).SetCellValue(datas[i - 1].TYPE_NAME);
                            row.CreateCell(1).SetCellValue(datas[i - 1].TYPE_KEY);
                        }
                        int cot = 0;
                        int num = 0;
                        var CityCode = GlobalHandler.CityCode;
                        if (CityCode != null)
                        {
                            var list = new List<CityModel>();
                            if (CityCode.Contains("0000")) CityCode = CityCode.Substring(0, 2);
                            if (CityCode.Contains("00")) CityCode = CityCode.Substring(0, 4);
                            list = GlobalHandler.CityInfo.Where(a => a.CITYCODE.StartsWith(CityCode)).ToList();
                            for (var i = 0; i < list.Count; i++)
                            {
                                IRow row;
                                if (sheet1.GetRow(i+2) != null)
                                {
                                    row = sheet1.GetRow(i+2);
                                }
                                else
                                {
                                    row = sheet1.CreateRow(i+2);
                                }
                                if(list[i].CITYCODE!=null)
                                { 
                                    row.CreateCell(3).SetCellValue(list[i].CITYCODE);
                                }
                                if (list[i].CITYNAME != null)
                                {
                                    row.CreateCell(2).SetCellValue(list[i].CITYNAME);
                                }
                            }
                        }

                        var saveDialog1 = new SaveFileDialog
                        {
                            DefaultExt = "xls",
                            Filter = @"Excel文件|*.xls;*.xlsx",
                            FileName = saveFileName1
                        };
                        saveDialog1.ShowDialog(); saveFileName1 = saveDialog1.FileName;

                        if (saveFileName1.IndexOf(":", StringComparison.Ordinal) < 0) return; //被点了取消

                        if (saveFileName1 == "") return;

                        try
                        {
                            fs1 = File.OpenWrite(saveDialog1.FileName);
                            workbook1.Write(fs1);
                            fs1?.Close();
                            XtraMessageBox.Show(@"下载成功！");
                        }
                        catch (Exception ex)
                        {
                           XtraMessageBox.Show(@"导出文件时出错,文件可能正被打开！\n" + ex.Message);
                            _log.Error("导出文件时出错,文件可能正被打开，出错提示：" + e.ToString());
                            fs1?.Close();
                        }
                    }
                    catch (Exception exception)
                    {
                        XtraMessageBox.Show(@"导出失败！\n" + exception.Message);
                        _log.Error("导出失败，出错提示：" + exception.ToString());
                        fs?.Close();
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                   XtraMessageBox.Show(@"导出文件时出错,文件可能正被打开！\n" + ex.Message);
                    _log.Error("导出文件时出错,文件可能正被打开，出错提示：" + e.ToString());
                    fs?.Close();
                }


            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(@"导出失败！\n" + exception.Message);
                _log.Error("导出失败，出错提示：" + exception.ToString());
                fs?.Close();
            GC.SuppressFinalize(this);
            }
            finally
            {
                fs?.Close();

            }
            this.DialogResult = DialogResult.OK;
        }

        private void tree_mstype_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}