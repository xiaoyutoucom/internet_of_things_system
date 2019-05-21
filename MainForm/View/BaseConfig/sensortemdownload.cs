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
using log4net;

namespace SmartKylinApp.View.BaseConfig
{
    public partial class sensortemdownload : DevExpress.XtraEditors.XtraForm
    {
        private ILog _log = LogManager.GetLogger("sensortemdownload");
        public sensortemdownload()
        {
            InitializeComponent();
        }

        private List<DeviceRecord> DeviceList;
        private void treeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {

        }

        private void sensortemdownload_Load(object sender, EventArgs e)
        {
            DeviceList = GlobalHandler.deviceresp.GetAllList();
            treeList1.BeforeCheckNode += TreeList1_BeforeCheckNode;
            treeList1.AfterCheckNode += TreeList1_AfterCheckNode;
            treeList1.NodeChanged += TreeList1_NodeChanged;
            BindTree();

            gridView1.SelectionChanged += GridView1_SelectionChanged;
        }

        private void GridView1_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            lab_selectnum.Text = gridView1.GetSelectedRows().Length.ToString();}

        private void TreeList1_NodeChanged(object sender, DevExpress.XtraTreeList.NodeChangedEventArgs e)
        {
            var node = e.Node.Nodes;
            if (node.Count <= 0) return;
            var a = node[0].CheckState;
        }

        private void TreeList1_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            lab_selectnum.Text = "";
            //设置子节点状态
            SetCheckedChildNodes(e.Node, e.Node.CheckState);
            SetCheckedParentNodes(e.Node, e.Node.CheckState);
            //获取选中状态
            var list = treeList1.GetAllCheckedNodes();
            var types = new List<string>();
            foreach (var item in list)
            {
                if (item.GetValue("ID").ToString().Length == 6)
                {
                    types.Add(item.GetValue("ID").ToString());
                }
            }

            GetData(types);
        }
        private void GetData(IReadOnlyCollection<string> mstype)
        {
            if (mstype == null) return;
            var list = new List<DeviceRecord>();
            foreach (var item in mstype)
            {
                if (item.Length < 6) continue;
                var aaList = DeviceList.Where(a => a.SBTYPE == item).ToList();
                list.AddRange(aaList);
            }
            BindView(list);
        }
        private void BindView(object list)
        {
            gridControl1.DataSource = list;
        }

        private void SetCheckedChildNodes(DevExpress.XtraTreeList.Nodes.TreeListNode node, CheckState check)
        {
            for (var i = 0; i < node.Nodes.Count; i++)
            {
                node.Nodes[i].CheckState = check;
                SetCheckedChildNodes(node.Nodes[i], check);
            }
        }

        private void SetCheckedParentNodes(DevExpress.XtraTreeList.Nodes.TreeListNode node, CheckState check)
        {
            while (true)
            {
                if (node.ParentNode == null) return;
                var b = false;
                for (var i = 0; i < node.ParentNode.Nodes.Count; i++)
                {
                    var state = (CheckState)node.ParentNode.Nodes[i].CheckState;
                    if (check.Equals(state)) continue;
                    b = true;
                    break;
                }
                node.ParentNode.CheckState = b ? CheckState.Indeterminate : check;
                node = node.ParentNode;
            }
        }

        private void TreeList1_BeforeCheckNode(object sender, DevExpress.XtraTreeList.CheckNodeEventArgs e)
        {
            //节点选中前
            e.State = e.PrevState == CheckState.Checked ? CheckState.Unchecked : CheckState.Checked;
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
            treeList1.DataSource = list;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            FileStream fs = null;
            try
            {
                var saveFileName = "传感器信息导入模板.xls";
                fs = new FileStream(saveFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                var workbook = new HSSFWorkbook();
                var sheet = workbook.CreateSheet();
                //(Optional) set the width of the columns
                sheet.SetColumnWidth(0, 20 * 256);
                sheet.SetColumnWidth(1, 40 * 256);
                sheet.SetColumnWidth(2, 30 * 256);
                sheet.SetColumnWidth(3, 30 * 256);
                sheet.SetColumnWidth(4, 30 * 256);
                sheet.SetColumnWidth(5, 30 * 256);
                sheet.SetColumnWidth(6, 30 * 256);
                sheet.SetColumnWidth(7, 30 * 256);


                var headerRow = sheet.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue("设备名称");
                headerRow.CreateCell(1).SetCellValue("设备编码（必填）");
                headerRow.CreateCell(2).SetCellValue("传感器编码（必填）");
                headerRow.CreateCell(3).SetCellValue("传感器名称（必填）");
                headerRow.CreateCell(4).SetCellValue("传感器型号");
                headerRow.CreateCell(5).SetCellValue("传感器类型编码");
                headerRow.CreateCell(6).SetCellValue("传感器类型名称");
                headerRow.CreateCell(7).SetCellValue("备注信息");

                sheet.CreateFreezePane(0, 1, 0, 1);
                var headerRow2 = sheet.CreateRow(1);
                //Set the column names in the header row
                headerRow2.CreateCell(0).SetCellValue("（示例数据请手动删除）燃气调压站_御河观塘");
                headerRow2.CreateCell(1).SetCellValue("1808132134");
                headerRow2.CreateCell(2).SetCellValue("SD");
                headerRow2.CreateCell(3).SetCellValue("湿度");
                headerRow2.CreateCell(4).SetCellValue("传感器型号");
                headerRow2.CreateCell(5).SetCellValue("JGCFQ");
                headerRow2.CreateCell(6).SetCellValue("传感器类型名称");
                headerRow2.CreateCell(6).SetCellValue("燃气调压站");
                //获取选中的监测点信息
                var index = gridView1.GetSelectedRows();

                for (var i = 0; i < index.Length; i++)
                {
                    var row = sheet.CreateRow(i + 2);
                    row.CreateCell(0).SetCellValue(gridView1.GetRowCellValue(index[i],"SBMC").ToString());
                    row.CreateCell(1).SetCellValue(gridView1.GetRowCellValue(index[i],"CCBH").ToString());
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
                    fs = File.OpenWrite(saveDialog.FileName);
                    workbook.Write(fs);
                    XtraMessageBox.Show(@"下载成功（第一条数据为示例数据，请手动删除）！");
                }
                catch (Exception ex)
                {
                   XtraMessageBox.Show(@"导出文件时出错,文件可能正被打开！\n" + ex.Message);
                    _log.Error("导出文件时出错，文件可能正被打开，出错提示：" + ex.ToString());
                }

               
            }
            catch (Exception exception)
            {
                fs?.Close();
                GC.SuppressFinalize(this);
               XtraMessageBox.Show(@"下载失败！");
                _log.Error("下载失败，出错提示：" + exception.ToString());
            }
            finally
            {
                fs?.Close();

            }
            this.DialogResult = DialogResult.OK;
        }
    }
}