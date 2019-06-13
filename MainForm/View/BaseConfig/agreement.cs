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
using SmartKylinApp.Common;
using SmartKylinData.IOTModel;
using ServiceStack;
using log4net;

namespace SmartKylinApp.View.BaseConfig
{
    public partial class agreement : DevExpress.XtraEditors.XtraUserControl
    {
        private ILog _log = LogManager.GetLogger("agreement");
        public agreement()
        {
            InitializeComponent();
            ContextMenu emptyMenu = new ContextMenu();
            
            layoutControl1.Visible = false;

            //隐藏默认右键菜单
            layoutControl1.AllowCustomization = false;
            layoutControl2.AllowCustomization = false;
            bar2.Manager.AllowShowToolbarsPopup = false;
            bar2.OptionsBar.AllowQuickCustomization = false;
            GetData();
        }

        private void Btn_close_Click(object sender, EventArgs e)
        {
            layoutControl1.Visible = false;
        }

        private void Btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                if (isedit)
                {
                    var model = GlobalHandler.agreeresp.FirstOrDefault(a => a.Id == currentId);
                    model.Command = txt_command.Text;
                    model.Devicecj = txt_sbcj.Text;
                    model.Frequency = txt_sbpl.Text;
                    model.Acode = txt_xybh.Text;
                    model.Enable = int.Parse(rdo_yj.EditValue.ToString() == null ? "0" : rdo_yj.EditValue.ToString());
                    GlobalHandler.agreeresp.Update(model);
                }
                else
                {
                    var model = new AgreementRecord();
                    model.Command = txt_command.Text;
                    model.Devicecj = txt_sbcj.Text;
                    model.Frequency = txt_sbpl.Text;
                    model.Acode = txt_xybh.Text;
                    model.Enable = int.Parse(rdo_yj.EditValue.ToString() == null ? "0" : rdo_yj.EditValue.ToString());
                    GlobalHandler.agreeresp.Insert(model);
                }

                GetData();
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show("数据保存出错");
                _log.Error("数据保存出错，出错提示："+ exception.ToString());
            }
        }

        private bool isedit = false;
        private int currentId = 0;

        private void GridView1_Click(object sender, EventArgs e)
        {
            //编辑数据
            editData();
           
        }

        private void editData()
        {
            try{ 
            if (layoutControl1.Visible == false|| !isedit)
            {

                return;
            }
            var data = gridView1.GetRowCellValue(gridView1.GetSelectedRows()[0], "Id");

            var model = GlobalHandler.agreeresp.FirstOrDefault(a => a.Id == (int)data);

            if (model == null) return;
            txt_command.Text = model.Command;
            txt_sbcj.Text = model.Devicecj;
            txt_sbpl.Text = model.Frequency;
            txt_xybh.Text = model.Acode;
            rdo_yj.EditValue = model.Enable.ToString();
            currentId = model.Id;
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取行数据出错");
                _log.Error("获取行数据出错，出错提示：" + e.ToString());
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            layoutControl1.Visible = !layoutControl1.Visible;
            isedit = false;
            txt_command.Text = "";
            txt_sbcj.Text = "";
            txt_sbpl.Text = "";
            txt_xybh.Text = "";
            rdo_yj.EditValue = "1";
        }

        private void GetData()
        {
            try
            {
                var list = GlobalHandler.agreeresp.GetAllList();
                grid_agreement.DataSource = list;
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取设备协议数据出错");
                _log.Error("获取设备协议数据出错，出错提示：" + e.ToString());
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                //删除数据
                var count = gridView1.GetSelectedRows();
                if (count.Length <= 0)
                {
                    XtraMessageBox.Show("请选择一条记录");
                    return;
                }
                else
                {
                    var id = (int)gridView1.GetRowCellValue(gridView1.GetSelectedRows()[0], "Id");
                    //删除数据
                    DelectBox dbox = new DelectBox();
                    dbox.StartPosition = FormStartPosition.CenterScreen;
                    dbox.ShowDialog();
                    bool IfDelect = dbox.IfDelect;
                    if (!IfDelect)
                    {
                        return;
                    }
                    GlobalHandler.agreeresp.Delete(id);
                    var index = gridView1.GetSelectedRows();
                    index.Each(a => GlobalHandler.agreeresp.Delete((int)(gridView1.GetRowCellValue(a, "Id"))));
                    GetData();
                }
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show("删除数据失败");
                _log.Error("删除数据失败，出错提示：" + exception.ToString());
            }
           
        }

        private void Box_Showing(object sender, XtraMessageShowingArgs e)
        {
            throw new NotImplementedException();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
           
            layoutControl1.Visible = !layoutControl1.Visible;
            isedit = true;
            editData();
        }
    }
}
