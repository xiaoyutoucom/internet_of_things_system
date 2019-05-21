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
    public partial class ContactAddManager : DevExpress.XtraEditors.XtraForm
    {
        private ILog _log = LogManager.GetLogger("ContactAddManager");
        public ContactAddManager()
        {
            InitializeComponent();
            //禁止右键菜单
            layoutControl1.AllowCustomization = false;
        }

        internal int Id;
        internal bool isEdit = false;
        private string oldcode;

        private void ContactAddManager_Load(object sender, EventArgs e)
        {
            if (!isEdit)
            {
                getData();
                oldcode = txt_GROUPNUM.Text;
            }
        }

        private void getData()
        {
            //获取数据
            try { 
            ContactsGroup model = GlobalHandler.contactgroupresp.Get(Id);
            txt_GROUPNAME.Text = model.GROUPNAME;
            txt_GROUPNUM.Text = model.GROUPNUM;
            txt_REMARK.Text = model.REMARK;
            txt_EXTENDCODE.Text = model.EXTENDCODE;
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取数据出错");
                _log.Error("获取数据出错，出错提示：" + e.ToString());
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //保存
            try
            {//验证必填项
                bool validate = false;
                StringBuilder st = new StringBuilder();
                if (string.IsNullOrEmpty(txt_GROUPNAME.Text))
                {
                    txt_GROUPNAME.Properties.Appearance.BorderColor = Color.Red;
                    validate = true;
                    st.Append("分组名称不能为空！\n\r");
                }
                else
                {
                    txt_GROUPNAME.Properties.Appearance.BorderColor = Color.White;
                }
                if (string.IsNullOrEmpty(txt_GROUPNUM.Text))
                {
                    txt_GROUPNUM.Properties.Appearance.BorderColor = Color.Red;
                    validate = true;
                    st.Append("分组编号不能为空！\n\r");
                }
                else
                {
                    txt_GROUPNUM.Properties.Appearance.BorderColor = Color.White;
                }
                if (validate)
                {
                    XtraMessageBox.Show(st.ToString());
                    return;
                }
                ContactsGroup model = new ContactsGroup();
                
                if (isEdit)
                {
                    //新增

                        int count = GlobalHandler.contactgroupresp.Count(a => a.GROUPNUM == txt_GROUPNUM.Text);
                        if (count > 0)
                        {
                            XtraMessageBox.Show("分组编号已存在！");
                            return;
                        }
                    model.GROUPNAME=txt_GROUPNAME.Text;
                    model.GROUPNUM=txt_GROUPNUM.Text;
                    model.REMARK=txt_REMARK.Text;
                    model.EXTENDCODE=txt_EXTENDCODE.Text;
                    model.PARENTID=Id;
                    GlobalHandler.contactgroupresp.Insert(model);
                }
                else
                {
                    //修改
                    if (oldcode != txt_GROUPNUM.Text)
                    {
                        int count = GlobalHandler.contactgroupresp.Count(a => a.GROUPNUM == txt_GROUPNUM.Text);
                        if (count > 0)
                        {
                            XtraMessageBox.Show("分组编号已存在！");
                            return;
                        }
                    }
                    model = GlobalHandler.contactgroupresp.Get(Id);
                    model.GROUPNAME = txt_GROUPNAME.Text;
                    model.GROUPNUM = txt_GROUPNUM.Text;
                    model.REMARK = txt_REMARK.Text;
                    model.EXTENDCODE = txt_EXTENDCODE.Text;
                    GlobalHandler.contactgroupresp.Update(model);
                }
                XtraMessageBox.Show("保存成功");
                this.Close();
            }
            catch (Exception exception)
            {
                _log.Error("数据保存失败，出错提示：" + e.ToString());
                XtraMessageBox.Show("数据保存失败");
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}