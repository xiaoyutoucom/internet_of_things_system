using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using log4net;
using SmartKylinApp.Common;
using SmartKylinData.IOTModel;

namespace SmartKylinApp.View.UnifiedConfigs
{
    public partial class RainLinkWaterHelp : DevExpress.XtraEditors.XtraForm
    {

        //泵站类型编号
        private static string PumpCode = "010301";
        //积水点类型编号
        private static string WaterCode = "011196";
        private ILog _log = LogManager.GetLogger("RainWaterRecord");
        private List<RainWaterRecord> lstBMR = new List<RainWaterRecord>();
        BasicMonitorRecord rainpoint = null;
        public RainLinkWaterHelp()
        {
            InitializeComponent();
        }
        public RainLinkWaterHelp(BasicMonitorRecord _rainpoint, List<RainWaterRecord> _lstBMR) :this()
        {
            lstBMR = _lstBMR;
            rainpoint = _rainpoint;
        }

        private void RainLinkWaterHelp_Load(object sender, EventArgs e)
        {
            GetPumpPoint();

            CheckSelectedItem();
        }

        private void CheckSelectedItem()
        {
            if (lstBMR == null)
            {
                return;
            }

            ArrayList _arr = new ArrayList();
            for (int i = 0; i < lstBMR?.Count; i++)
            {
                _arr.Add(lstBMR[i].water_id.Id.ToString());
            }
            for (int j = 0; j < gridView1.RowCount; j++)
            {
                var Id = gridView1.GetRowCellValue(j, "Id").ToString();
                if (_arr.Contains(Id))
                {
                    gridView1.SelectRow(j);
                }
            }
        }

        /// <summary>
        /// 获取所有积水点
        /// </summary>
        private void GetPumpPoint()
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
                    gridControl1.DataSource = aList;
                }
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取数据出错");
                _log.Error("获取数据出错，出错提示：" + e.ToString());
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                //获取选中行号
                int[] rownumber = this.gridView1.GetSelectedRows();
                if (rownumber.Length < 1 && gridView1.RowCount == 0)
                {
                    XtraMessageBox.Show("请勾选积水点！");
                    return;
                }
                foreach (var k in lstBMR)
                {
                    GlobalHandler.rainLinkWaterresp.Delete(k);
                }
                List<int> arr = gridView1.GetSelectedRows().ToList();
                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    if (arr.Contains(i) == false)
                    {
                        continue;
                    }
                    RainWaterRecord wlmodel = new RainWaterRecord();
                    int Id = int.Parse(gridView1.GetRowCellValue(i, "Id").ToString());
                    BasicMonitorRecord bmmodel = GlobalHandler.monitorresp.Get(Id);
                    wlmodel.water_id = bmmodel;
                    wlmodel.rain_id = rainpoint;                    
                    GlobalHandler.rainLinkWaterresp.Insert(wlmodel);
                }
                XtraMessageBox.Show("保存成功");
                this.Close();
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show("保存失败");
                _log.Error("保存失败，出错提示：" + exception.ToString());
            }
        }
    }
}
