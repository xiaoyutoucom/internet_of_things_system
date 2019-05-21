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

namespace SmartKylinApp.View.BaseConfig
{
    public partial class sensorilinkmodbus : DevExpress.XtraEditors.XtraForm
    {
        private ILog _log = LogManager.GetLogger("sensorilinkmodbus");
        public sensorilinkmodbus()
        {
            InitializeComponent();
            //禁止右键菜单
            layoutControl1.AllowCustomization = false;
        }

        internal int Id;
        internal bool isEdit = false;
        public ModbusRecord mbmodel = null;

        private void sensorilinkmodbus_Load(object sender, EventArgs e)
        {
            rdo_ISFOMULA.EditValue = "0";
            if (mbmodel!=null)
            {
                getData();
               
            }
        }

        private void getData()
        {
            //获取数据
             txt_DZW.Text = mbmodel.DZW;
             txt_JCNAME.Text = mbmodel.JCNAME;
             txt_UNIT.Text = mbmodel.UNIT;
             txt_STARTBYTE.Text = mbmodel.STARTBYTE.ToString();
             txt_DTYPE.Text = mbmodel.DTYPE;
             txt_bz.Text = mbmodel.BZ;
             rdo_ISFOMULA.EditValue = mbmodel.ISFOMULA;
             txt_SFOMULA.Text = mbmodel.SFOMULA;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //保存
            try
            {//验证必填项
                bool validate = false;
                StringBuilder st = new StringBuilder();
                if (string.IsNullOrEmpty(txt_DZW.Text))
                {
                    txt_DZW.Properties.Appearance.BorderColor = Color.Red;
                    validate = true;
                    st.Append("地址位不能为空！\n\r");
                }
                else
                {
                    txt_DZW.Properties.Appearance.BorderColor = Color.White;
                }
                if (string.IsNullOrEmpty(txt_JCNAME.Text))
                {
                    txt_JCNAME.Properties.Appearance.BorderColor = Color.Red;
                    validate = true;
                    st.Append("监测项名称不能为空！\n\r");
                }
                else
                {
                    txt_JCNAME.Properties.Appearance.BorderColor = Color.White;
                }
                if (validate)
                {
                    XtraMessageBox.Show(st.ToString());
                    return;
                }
                if (mbmodel==null)
                { 
                mbmodel = new ModbusRecord();
                }
                //新增
                mbmodel.DZW = txt_DZW.Text;
                    mbmodel.JCNAME = txt_JCNAME.Text;
                    mbmodel.UNIT = txt_UNIT.Text;
                    mbmodel.STARTBYTE = double.Parse(txt_STARTBYTE.Text == "" ? "0" : txt_STARTBYTE.Text);
                    mbmodel.DTYPE = txt_DTYPE.Text;
                    mbmodel.BZ = txt_bz.Text;

                    mbmodel.ISFOMULA = rdo_ISFOMULA.EditValue.ToString();
                    mbmodel.SFOMULA = txt_SFOMULA.Text;
                //XtraMessageBox.Show("保存成功");
                this.Close();
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show("数据保存失败");
                _log.Error("数据保存失败，出错提示：" + exception.ToString());
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}