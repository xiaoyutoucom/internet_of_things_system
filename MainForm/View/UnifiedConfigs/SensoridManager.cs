using DevExpress.XtraEditors;
using DevExpress.XtraTreeList.Nodes;
using FluentNHibernate.Conventions;
using log4net;
using ServiceStack;
using SmartKylinApp.Common;
using SmartKylinData.IOTModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartKylinApp.View.UnifiedConfigs
{
    public partial class SensoridManager : DevExpress.XtraEditors.XtraForm
    {
        private ILog _log = LogManager.GetLogger("SensoridManager");
        private List<SensorRecord> DeviceList;
        public SensorRecord model;
        public string hytype;
        private IEnumerable<DeviceRecord> dt1;

        public SensoridManager(SensorRecord srmodel)
        {
            //DeviceList = GlobalHandler.sensorresp.GetAllList().ToList();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            InitializeComponent();
            //禁止右键菜单
            layoutControl1.AllowCustomization = false;
            layoutControl2.AllowCustomization = false;
            layoutControl3.AllowCustomization = false;
            gridView1.OptionsMenu.EnableColumnMenu = false;
            gridView2.OptionsMenu.EnableColumnMenu = false;

            gridView1.CustomDrawCell += GridView1_CustomDrawCell;
            gridView2.CustomDrawCell += GridView2_CustomDrawCell;
            model = srmodel;
            
        }
        private void CheckSelect()
        {
            if (model != null)
            {                
                //传感器
                var list2 = gridControl2.DataSource as IEnumerable<DeviceRecord>;
                gridView2.FocusedRowHandle = list2.ToList().IndexOf(model.Device);

                //设备
                var list1 = gridControl1.DataSource as List<SensorRecord>;
                gridView1.FocusedRowHandle = list1.IndexOf(model);
            }
        }
        private void GridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.RowHandle == gridView1.FocusedRowHandle)
            {
                e.Appearance.BackColor = Color.LightCyan;
            }
        }
        private void GridView2_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.RowHandle == gridView2.FocusedRowHandle)
            {
                e.Appearance.BackColor = Color.LightCyan;
            }
        }

        private void SensoridManager_Load(object sender, EventArgs e)
        {
            BindTree();

            //GetData();

            CheckSelect();
        }
        private void GetData(string mstype)
        {
            try { 
            if (mstype == null) return;
            var List = GlobalHandler.sensorresp.GetAllList(a => a.Device.Id.ToString() == mstype).ToList();
            gridControl1.DataSource = List;
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取数据出错");
                _log.Error("获取数据出错，出错提示：" + e.ToString());
            }
        }
        private void GetData()
        {
            try { 
            var aaList = GlobalHandler.sensorresp.GetAllList().ToList();
            DeviceList = aaList;
            gridControl1.DataSource = aaList;
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取数据出错");
                _log.Error("获取数据出错，出错提示：" + e.ToString());
            }
        }
        //绑定设备类型
        private void BindTree()
        {
            try { 
            var datas = GlobalHandler.deviceresp.GetAllList();
            if (datas == null) return;
            dt1 = datas.Where(a => a.SBTYPE.StartsWith(hytype));
            gridControl2.DataSource = dt1;
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取设备类型数据出错");
                _log.Error("获取设备类型数据出错，出错提示：" + e.ToString());
            }
        }
   


        private void gridView2_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            //获取选中状态
            if (gridView2.GetSelectedRows().IsNotEmpty())
            {
                var type = gridView2.GetRowCellValue(gridView2.GetSelectedRows()[0], "Id").ToString();
                GetData(type);
            }
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            //双击选择
            if (gridView1.GetSelectedRows().Length > 0)
            {
                var Id = int.Parse(gridView1.GetRowCellValue(gridView1.GetSelectedRows()[0], "Id").ToString());
                model = GlobalHandler.sensorresp.Get(Id);
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //查询
            if (txt_where.Text == "")
            {
                gridControl2.DataSource = dt1;
                return;
            }
            //var datas = GlobalHandler.deviceresp.GetAllList();
            if (dt1 == null) return;
            gridControl2.DataSource = dt1.Where(a => a.SBMC.Contains(txt_where.Text));
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (gridView1.GetSelectedRows().Length > 0)
            {
                var Id = int.Parse(gridView1.GetRowCellValue(gridView1.GetSelectedRows()[0], "Id").ToString());
                model = GlobalHandler.sensorresp.Get(Id);
                this.Close();
            }
            else
            {
                XtraMessageBox.Show("请先选择数据！");
            }
        }
    }
}
