using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using SmartKylinApp.View.BaseConfig;
using SmartKylinData.IOTModel;
using SmartKylinApp.Common;
using ServiceStack;
using log4net;
using FluentNHibernate.Conventions;

namespace SmartKylinApp.View.UnifiedConfigs
{
    public partial class WaterLink : DevExpress.XtraEditors.XtraUserControl
    {
        //积水点类型编号
        private static string WaterCode = "011196";
        //泵站类型编号
        private static string PumpCode = "010301";
        private List<SmartKylinData.IOTModel.WaterLink> list;
        private ILog _log = LogManager.GetLogger("WaterLink");
        BasicMonitorRecord selectedWaterM = null;
        public WaterLink()
        {
            InitializeComponent();
            //禁止右键菜单
            layoutControl2.AllowCustomization = false;
            bar2.Manager.AllowShowToolbarsPopup = false;
            bar2.OptionsBar.AllowQuickCustomization = false;
            gridView1.OptionsMenu.EnableColumnMenu = false;

        }
        private void WaterLink_Load(object sender, EventArgs e)
        {
            GetWaterPoint();
            GetData();
            this.gridView2.FocusedRowChanged += GridView2_FocusedRowChanged;
        }

        private void GridView2_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            GetData();
        }

        /// <summary>
        /// 获取所有积水点
        /// </summary>
        private void GetWaterPoint()
        {
            try
            {
                var aList = GlobalHandler.monitorresp.GetAllList(a => a.BMID.Substring(6, 6).StartsWith(WaterCode)).ToList();
                if (aList.Count < 1)
                {
                    return;
                }
                else
                {
                    gridControl2.DataSource = aList;
                }
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取数据出错");
                _log.Error("获取数据出错，出错提示：" + e.ToString());
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string id = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "Id")?.ToString();
            selectedWaterM = GlobalHandler.monitorresp.Get(int.Parse(id));
            var list = gridControl1.DataSource as List<SmartKylinData.IOTModel.WaterLink>;
            if (list == null)
            {
                list = new List<SmartKylinData.IOTModel.WaterLink>();
            }
            WaterLinkHelp from = new WaterLinkHelp(selectedWaterM, list);
            from.StartPosition = FormStartPosition.CenterScreen;
            from.ShowDialog();
            GetData();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        private void GetData()
        {            
            try
            {
                string id = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "Id")?.ToString();
                list = GlobalHandler.waterLinkresp.GetAllList(p => p.water_id.Id == int.Parse(id)).ToList();
                gridControl1.DataSource = list;
                barStaticItem3.Caption = list.Count.ToString();
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取数据出错");
                _log.Error("获取数据出错，出错提示：" + e.ToString());
            }
        }
         
        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GetData();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (gridView1.GetSelectedRows().Length == 0) return;
                if (XtraMessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.OKCancel) != DialogResult.OK)
                {
                    return;
                }
                var index = gridView1.GetSelectedRows();
                index.Each(a => GlobalHandler.waterLinkresp.Delete((int)(gridView1.GetRowCellValue(a, "Id"))));
                GetData();
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show("删除失败");
                _log.Error("删除失败，出错提示：" + exception.ToString());
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barEdit_mc.EditValue == null || barEdit_mc.EditValue.ToString() == "")
            {
                XtraMessageBox.Show("请输入查询内容");
                return;
            }
            //查找
            var sbbm = barEdit_mc.EditValue.ToString();

            //if (string.IsNullOrEmpty(sbbm)) return;
            //var listwhere = list.Where(a => a.MonitorRecord.BMMC.Contains(sbbm)).ToList();
            //gridControl1.DataSource = listwhere;
            //barStaticItem3.Caption = listwhere.Count.ToString();
        }

        private void gridView2_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            //获取选中状态
            if (gridView2.GetSelectedRows().IsNotEmpty())
            {
                string waterId = gridView2.GetRowCellValue(gridView2.GetSelectedRows()[0], "Id").ToString();
                GetAllCheck(waterId);
            }
        }

        /// <summary>
        /// 积水点关联的泵站
        /// </summary>
        /// <param name="waterId"></param>
        private void GetAllCheck(string waterId)
        {
            var list = GlobalHandler.waterLinkresp.GetAllList(a => a.water_id.BMID.ToString() == waterId).ToList();
            List<BasicMonitorRecord> bmlist = new List<BasicMonitorRecord>();
            for (int i = 0; i < list.Count; i++)
            {
                bmlist.Add(list[i].pmonitorrecord_id);
            }
            gridControl1.DataSource = bmlist;
            gridView1.SelectAll();
        }
    }
}
