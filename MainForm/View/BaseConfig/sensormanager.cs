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
using FluentNHibernate.Conventions;
using ServiceStack;
using SmartKylinApp.Common;
using SmartKylinData.IOTModel;
using DevExpress.XtraLayout.Utils;
using log4net;
using System.Web.UI.WebControls;

namespace SmartKylinApp.View.BaseConfig
{
    public partial class sensormanager : DevExpress.XtraEditors.XtraUserControl
    {
        private ILog _log = LogManager.GetLogger("sensormanager");
        private bool isedit = false;
        public sensormanager()
        {
            InitializeComponent();
            //禁止右键菜单
            layoutControl3.AllowCustomization = false;
            layoutControl2.AllowCustomization = false;
            layoutControl1.AllowCustomization = false;
            bar2.Manager.AllowShowToolbarsPopup = false;
            bar2.OptionsBar.AllowQuickCustomization = false;
            gridView1.OptionsMenu.EnableColumnMenu = false;
            gridView2.OptionsMenu.EnableColumnMenu = false;
            tree_mstype.Properties.TreeList.OptionsMenu.EnableColumnMenu = false;
        }
        private void sensormanager_Load(object sender, EventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            splashScreenManager1.SetWaitFormCaption("请稍后,数据加载中....");     // 标题
            //splashScreenManager1.SetWaitFormDescription("正在更新.....");　　　　　// 信息
            BindTree();
            BindSPJXLX();
            //GetDeviceInfo(null);
            layoutControl1.Visible = false;
            splashScreenManager1.CloseWaitForm();
        }

        private void BindSPJXLX()
        {
            //数据集绑定
            listS = new List<TreeListModel>();
            listS.Add(new TreeListModel() { ID = "超声波式", Name = "超声波式" });
            listS.Add(new TreeListModel() { ID = "压力式", Name = "压力式" });
            listS.Add(new TreeListModel() { ID = "投放式", Name = "投放式" });
            listS.Add(new TreeListModel() { ID = "光敏式", Name = "光敏式" });
            listS.Add(new TreeListModel() { ID = "电磁式", Name = "电磁式" });
            listS.Add(new TreeListModel() { ID = "电流式", Name = "电流式" });
            listS.Add(new TreeListModel() { ID = "激光式", Name = "激光式" });
            listS.Add(new TreeListModel() { ID = "力矩式", Name = "力矩式" });
            listS.Add(new TreeListModel() { ID = "测距式", Name = "测距式" });
            listS.Add(new TreeListModel() { ID = "图像式", Name = "图像式" });
            ListItem it = null;
            foreach (TreeListModel item in listS)
            {
                it = new ListItem(item.Name, item.ID);
                this.com_sensortype.Properties.Items.Add(it);
            }
            com_sensortype.SelectedIndex = 0;
        }
        private void btn_add_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //添加
            layoutControlGroup1.Visibility = LayoutVisibility.Always;
            layoutControl1.Controls.Remove(layoutControl1.Controls["import"]);
            layoutControl1.Visible = !layoutControl1.Visible;
            isedit = false;
            ClearForms();
        }

        private void ClearForms()
        {
            txt_name.Text = "";
            txt_sersorcode.Text = "";
            txt_sensorbz.Text = "";
            com_sensortype.Text = "";
            date_ccrq.DateTime = DateTime.Now;
            mbmodel = null;
            bet_link.Text = "";
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //编辑
            layoutControlGroup1.Visibility = LayoutVisibility.Always;
            layoutControl1.Controls.Remove(layoutControl1.Controls["import"]);
            layoutControl1.Visible = !layoutControl1.Visible;
            isedit = true;
            EditData();
        }

        private void btn_delete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //删除
            try
            {
                if (gridView1.GetSelectedRows().IsNotAny())
                {
                    var id = gridView1.GetRowCellValue(gridView1.GetSelectedRows()[0], "Id");
                    //删除数据
                    DelectBox dbox = new DelectBox();
                    dbox.StartPosition = FormStartPosition.CenterScreen;
                    dbox.ShowDialog();
                    bool IfDelect = dbox.IfDelect;
                    if (!IfDelect)
                    {
                        return;
                    }
                    // GlobalHandler.sensorresp.Delete((int)id);
                    var index = gridView1.GetSelectedRows();
                    index.Each(a => GlobalHandler.sensorresp.Delete((int)(gridView1.GetRowCellValue(a, "Id"))));
                    if (gridView2.GetSelectedRows().IsNotEmpty())
                    {
                        GetSensorInfo(gridView2.GetRowCellValue(gridView2.GetSelectedRows()[0], "Id")?.ToString());
                    }
                }
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show("删除失败");
                _log.Error("删除失败，出错提示：" + e.ToString());
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //导入
            if (!layoutControl1.Visible)
            {
                layoutControlGroup1.Visibility = LayoutVisibility.Never;
                foreach (Control item in layoutControl1.Controls)
                {
                    item.Visible = false;
                }
                var c = new sensorimport();
                c.Name = "import";
                c.Dock = DockStyle.Fill;
                layoutControl1.Controls.Add(c);
                layoutControl1.Padding = new System.Windows.Forms.Padding(0, 0, 30, 0);
                layoutControl1.Visible = true;
            }
            else
            {
                foreach (Control item in layoutControl1.Controls)
                {
                    item.Visible = true;
                }
                layoutControl1.Controls.Remove(layoutControl1.Controls["import"]);
                layoutControl1.Visible = false;
                layoutControlGroup1.Visibility = LayoutVisibility.Always;

            }
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //刷新
            if (gridView2.GetSelectedRows().IsNotAny())
            {
                GetSensorInfo(null);
            }
        }

        private void btn_query_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //查询
            try
            {
                if (txt_code.EditValue == null)
                {
                    XtraMessageBox.Show("请输入查询内容");
                    return;
                }

                gridControl1.DataSource = list.Where(a => a.SBMC.Contains(txt_code.EditValue.ToString()));
            }
            catch (Exception exception)
            {
                _log.Error("查询失败，出错提示：" + e.ToString());
                XtraMessageBox.Show("查询失败");
                //Console.WriteLine(exception);
                throw;

            }
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {
            //设备信息点击
            if (gridView2.GetSelectedRows().IsNotEmpty())
            {
                GetSensorInfo(gridView2.GetRowCellValue(gridView2.GetSelectedRows()[0], "Id")?.ToString());
                currentCCBH = ((gridView2.GetRow(gridView2.GetSelectedRows()[0])) as DeviceRecord)?.CCBH;
                EditData();
            }
        }

        private string currentCCBH = "";
        private List<DeviceRecord> list;
        private string oldsersorcode;
        private List<TreeListModel> listS;
        private ModbusRecord mbmodel = null;

        private void grid_agreement_Click(object sender, EventArgs e)
        {
            ClearForms();
            EditData();
            
        }

        private void EditData()
        {
            try { 
            //传感器信息点击
            if (layoutControl1.Visible)
            {
                if (!isedit)
                {
                    //ClearForms();
                    return;
                }
                    //Modbus信息
                    var id = gridView1.GetRowCellValue(gridView1.GetSelectedRows()[0], "Id");
                    mbmodel = GlobalHandler.modbusresp.FirstOrDefault(b => b.SENSOR.Id == (int)id);
                    if(mbmodel!=null)
                    { 
                    bet_link.Text = mbmodel.DZW;
                    }
                    //绑定传感器信息
                    if (gridView1.GetSelectedRows().Length != 0)
                {

                    var model = GlobalHandler.sensorresp.Get((int)id);
                    //currentCCBH =
                    txt_name.Text = model.CGQMC;
                    txt_sersorcode.Text = model.CGQBM.Split('_')[1];
                    oldsersorcode = model.CGQBM.Split('_')[1];
                    txt_sensorbz.Text = model.BZ;
                    com_sensortype.Text = model.CGQLXBM;
                    date_ccrq.DateTime = model.CCRQ;
                }
                else
                {
                        //ClearForms();
                        return;
                }
            }
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取行数据出错");
                _log.Error("获取行数据出错，出错提示：" + e.ToString());
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //保存
            try
            {
                if (!gridView2.GetSelectedRows().IsNotEmpty())
                {
                    XtraMessageBox.Show("请选择监测点!");
                    return;
                }
                bool validate = false;
                StringBuilder st = new StringBuilder();
                if (string.IsNullOrEmpty(txt_name.Text))
                {
                    txt_name.Properties.Appearance.BorderColor = Color.Red;
                    validate = true;
                    st.Append("传感器名称不能为空！\n\r");
                }
                else
                {
                    txt_name.Properties.Appearance.BorderColor = Color.White;
                }
                if (string.IsNullOrEmpty(txt_sersorcode.Text))
                {
                    txt_sersorcode.Properties.Appearance.BorderColor = Color.Red;
                    validate = true;
                    st.Append("传感器编码不能为空！\n\r");
                }
                else
                {
                    txt_sersorcode.Properties.Appearance.BorderColor = Color.White;
                }
                //if (string.IsNullOrEmpty(bet_link.Text))
                //{
                //    bet_link.Properties.Appearance.BorderColor = Color.Red;
                //    validate = true;
                //    st.Append("modbus配置不能为空！\n\r");
                //}
                //else
                //{
                //    bet_link.Properties.Appearance.BorderColor = Color.White;
                //}
                if (validate)
                {
                    XtraMessageBox.Show(st.ToString());
                    return;
                }
                currentCCBH = ((gridView2.GetRow(gridView2.GetSelectedRows()[0])) as DeviceRecord)?.CCBH;
                if (isedit)
                {
                    var model = GlobalHandler.sensorresp.Get((int) gridView1.GetRowCellValue(gridView1.GetSelectedRows()[0], "Id"));
                    if (model != null)
                    {
                        int count = GlobalHandler.sensorresp.GetAllList(b=>b.Device.Id == model.Device.Id).Count(a => a.CGQBM == currentCCBH + "_" + txt_sersorcode.Text);
                        if (count > 0 && oldsersorcode != txt_sersorcode.Text && txt_sersorcode.Text != "")
                        {
                            XtraMessageBox.Show("传感器编码已存在！");
                            return;
                        }
                        model.CGQMC = txt_name.Text;
                        model.CGQBM = currentCCBH + "_" + txt_sersorcode.Text;
                        model.BZ = txt_sensorbz.Text;
                        model.CCRQ = date_ccrq.DateTime;
                        //model.ADDTIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        model.CGQLXBM = com_sensortype.Text;
                        model.CGQLXMC = com_sensortype.Text;
                        GlobalHandler.sensorresp.Update(model);
                        //modbus保存
                        if (bet_link.Text != "")
                        {
                            mbmodel.SENSOR = model;
                            if (mbmodel.Id == 0)
                            {
                                GlobalHandler.modbusresp.Insert(mbmodel);
                            }
                            else
                            {
                                GlobalHandler.modbusresp.Update(mbmodel);
                            }
                        }
                        EditData();
                    }
                }
                else
                {
                    if(string.IsNullOrEmpty(txt_name.Text)||string.IsNullOrEmpty(txt_sersorcode.Text))
                        return;
                    var model = new SensorRecord();
                    int count = GlobalHandler.sensorresp.GetAllList(b => b.Device.CCBH == currentCCBH).Count(a => a.CGQBM == currentCCBH + "_" + txt_sersorcode.Text);
                    if (count > 0 && txt_sersorcode.Text != "")
                    {
                        XtraMessageBox.Show("传感器编码已存在！");
                        return;
                    }
                    DeviceRecord drmdel= new DeviceRecord();
                    model.CGQMC = txt_name.Text;
                    model.CGQBM = currentCCBH+"_"+txt_sersorcode.Text;
                    model.BZ = txt_sensorbz.Text;
                    model.CCRQ = date_ccrq.DateTime;
                    drmdel = GlobalHandler.deviceresp.Get(int.Parse(gridView2.GetRowCellValue(gridView2.GetSelectedRows()[0], "Id").ToString()));
                    model.Device = drmdel;
                    //model.ADDTIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    model.CGQLXBM = com_sensortype.Text;
                    model.CGQLXMC = com_sensortype.Text;
                    GlobalHandler.sensorresp.Insert(model);

                    //modbus保存
                    //ModbusRecord mdbmoel = mbmodel;
                    if (bet_link.Text!="")
                    {
                        mbmodel.SENSOR = model;
                        GlobalHandler.modbusresp.Insert(mbmodel);
                    }
                    ClearForms();
                }

                XtraMessageBox.Show("保存成功");
                if (gridView2.GetSelectedRows().IsNotEmpty())
                {
                    GetSensorInfo(gridView2.GetRowCellValue(gridView2.GetSelectedRows()[0], "Id")?.ToString());
                }
                layoutControl1.Visible = !layoutControl1.Visible;
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show("保存失败");
                _log.Error("保存失败，出错提示：" + exception.ToString());
            }
            
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //取消
            layoutControl1.Visible = false;
        }

        private void tree_mstype_EditValueChanged(object sender, EventArgs e)
        {
            //行业类型选择
            var code = (tree_mstype.GetSelectedDataRow() as TreeListModel)?.ID;
            GetDeviceInfo(code);
            EditData();
        }
        //绑定行业信息
        private void BindTree()
        {
            try { 
            var datas = GlobalHandler.mstyperesp.GetAllList();
            if (datas == null) return;
            var list1 = new List<TreeListModel>();
            var dt1 = datas.Where(a => a.TYPE_KEY.ToString().Length == 2);
            dt1.Each(a => list1.Add(new TreeListModel() { ID = a.TYPE_KEY, ParentID = "1", Name = a.TYPE_NAME }));
            var dt2 = datas.Where(a => a.TYPE_KEY.ToString().Length == 4);
            dt2.Each(a => list1.Add(new TreeListModel() { ID = a.TYPE_KEY, ParentID = a.TYPE_KEY.Substring(0, 2), Name = a.TYPE_NAME }));
            var dt3 = datas.Where(a => a.TYPE_KEY.ToString().Length == 6);
            dt3.Each(a => list1.Add(new TreeListModel() { ID = a.TYPE_KEY, ParentID = a.TYPE_KEY.Substring(0, 4), Name = a.TYPE_NAME }));
            
            tree_mstype.Properties.DataSource = list1;
            tree_mstype.Properties.DisplayMember = "Name";
            tree_mstype.Properties.ValueMember = "ID";
            tree_mstype.EditValue = "03";
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取行业信息数据出错");
                _log.Error("保存失败，出错提示：" + e.ToString());
            }

        }
        //获取设备信息
        private void GetDeviceInfo(string type)
        {
            try
            {
                if (string.IsNullOrEmpty(type))
                {
                    list = GlobalHandler.deviceresp.GetAllList();
                    gridControl1.DataSource = list;
                }
                else
                {
                    list = GlobalHandler.deviceresp.GetAllList(a => a.SBTYPE.StartsWith(type));
                    if (list.Count < 1)
                    {
                        DeviceRecord model = new DeviceRecord();
                        model.Id = -1;
                        model.SBMC = "没有匹配监测点！";
                        list.Add(model);
                        gridControl1.DataSource = list;
                    }
                    else
                    {
                        gridControl1.DataSource = list;

                    }
                }
                GetSensorInfo(gridView2.GetRowCellValue(0, "Id")?.ToString());
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取设备信息数据出错");
                _log.Error("获取设备信息数据出错，出错提示：" + e.ToString());
            }

        }

        

        //获取传感器信息
        private void GetSensorInfo(string deviceid)
        {
            try { 
            if (string.IsNullOrEmpty(deviceid)|| deviceid=="-1")
            {
                grid_agreement.DataSource = null;
                return;
            }
            else
            {
                var sensorlist=GlobalHandler.sensorresp.GetAllList(a=>a.Device.Id.ToString()==deviceid);
                grid_agreement.DataSource = sensorlist;
            }
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取传感器数据出错");
                _log.Error("获取传感器数据出错，出错提示：" + e.ToString());
            }
        }

        private void searchControl1_Click(object sender, EventArgs e)
        {

        }

        private void searchControl1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //菜单查询
            var listwhere = list.Where(a => a.SBMC.Contains(searchControl1.Text)).ToList();
            if (listwhere.Count < 1)
            {
                DeviceRecord model = new DeviceRecord();
                model.Id = -1;
                model.SBMC = "没有匹配监测点！";
                listwhere.Add(model);
            }
            gridControl1.DataSource = listwhere;
            GetSensorInfo(gridView2.GetRowCellValue(0, "Id")?.ToString());
            EditData();
        }

        private void bet_cgqbm_Click(object sender, EventArgs e)
        {
            sensorilinkmodbus from = new sensorilinkmodbus();
            from.StartPosition = FormStartPosition.CenterScreen;
            from.mbmodel = mbmodel;
            from.ShowDialog();
            mbmodel = from.mbmodel;
            if (mbmodel == null) return;
            bet_link.Text = mbmodel.DZW;
        }
    }
}
