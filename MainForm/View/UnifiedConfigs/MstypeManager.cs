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

//监测点类型

namespace SmartKylinApp.View
{
    public partial class MstypeManager : DevExpress.XtraEditors.XtraUserControl
    {
        private ILog _log = LogManager.GetLogger("MstypeManager");
        public MstypeManager()
        {
            InitializeComponent();
            //禁止右键菜单
            layoutControl1.AllowCustomization = false;
            layoutControl2.AllowCustomization = false;
            bar2.Manager.AllowShowToolbarsPopup = false;
            bar2.OptionsBar.AllowQuickCustomization = false;
            gridView1.OptionsMenu.EnableColumnMenu = false;
        }

        private bool isedit = false;
        private string oldcode;

        private void MstypeManager_Load(object sender, EventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            splashScreenManager1.SetWaitFormCaption("请稍后,数据加载中....");     // 标题
            //splashScreenManager1.SetWaitFormDescription("正在更新.....");　　　　　// 信息
            //初始化数据
            layoutControl1.Visible = false;
            GetData();
            repositoryItemComboBox1.Items.Add(new ComModel()
            {
                Text = "全部",
                Value = "00"
            });
            var list = GlobalHandler.mstyperesp.GetAllList(a=>a.TYPE_KEY.Length==2).GroupBy(a => a.TYPE_KEY).Select(a =>
                new ComModel()
                {
                    Text = a.FirstOrDefault()?.TYPE_NAME,
                    Value = a.Key

                }).ToList();
            list.ForEach(a=>repositoryItemComboBox1.Items.Add(a));
            
            repositoryItemComboBox1.SelectedValueChanged += RepositoryItemComboBox1_SelectedValueChanged;

            com_mstype.EditValue = "全部";
            splashScreenManager1.CloseWaitForm();
        }

        private void RepositoryItemComboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if ((com_mstype.EditValue as ComModel)?.Value.ToString()== "00")
            {
                GetData();
            }
            else
            {
                var list = GlobalHandler.mstyperesp.GetAllList(a =>
                    a.TYPE_KEY.StartsWith((com_mstype.EditValue as ComModel).Value.ToString()));
                gridControl1.DataSource = list;
            }
        }

        private void GetData()
        {
            try
            {
                var list = GlobalHandler.mstyperesp.GetAllList();
                gridControl1.DataSource = list;
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("数据获取失败");
                _log.Error("数据获取失败，出错提示：" + e.ToString());
            }
        }

        private void btn_add_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //添加
            layoutControl1.Visible = !layoutControl1.Visible;
            isedit = false;
            CleanData();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //编辑
            layoutControl1.Visible = !layoutControl1.Visible;
            isedit = true;
            EditData();
        }

        private void btn_delete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //删除
            try
            {
                if (gridView1.GetSelectedRows().Length == 0) return;
                var box = new XtraMessageBoxArgs();
                box.Caption = "提示";
                box.Text = "确定要删除吗？";
                box.Buttons = new DialogResult[] { DialogResult.OK, DialogResult.Cancel };
                box.Showing += ShowButton.Box_Showing;
                if (XtraMessageBox.Show(box) != DialogResult.OK)
                {
                    return;
                }
                //var Idd = int.Parse(gridView1.GetRowCellValue(gridView1.GetSelectedRows()[0], "Id").ToString());
                //GlobalHandler.mstyperesp.Delete(Idd);
                var index = gridView1.GetSelectedRows();
                index.Each(a => GlobalHandler.mstyperesp.Delete((int)(gridView1.GetRowCellValue(a, "Id"))));
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show($"删除失败");
            }

            GetData();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //刷新
            GetData();
        }

        private void btn_query_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //查询
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //保存

            try
            {
                //验证必填项
                bool validate = false;
                StringBuilder st = new StringBuilder();
                if (string.IsNullOrEmpty(txt_code.Text))
                {
                    txt_code.Properties.Appearance.BorderColor = Color.Red;
                    validate = true;
                    st.Append("监测点类型编码不能为空！\n\r");
                }
                else
                {
                    txt_code.Properties.Appearance.BorderColor = Color.White;
                }
                if (string.IsNullOrEmpty(txt_name.Text))
                {
                    txt_name.Properties.Appearance.BorderColor = Color.Red;
                    validate = true;
                    st.Append("监测点类型名称不能为空！\n\r");
                }
                else
                {
                    txt_name.Properties.Appearance.BorderColor = Color.White;
                }
                if (validate)
                {
                    XtraMessageBox.Show(st.ToString());
                    return;
                }
                if (isedit)
                {
                    if (oldcode != txt_code.Text)
                    {
                        int count = GlobalHandler.mstyperesp.Count(a => a.TYPE_KEY == txt_code.Text);
                        if (count > 0)
                        {
                            XtraMessageBox.Show("编号已存在！");
                            return;
                        }
                    }
                    if (gridView1.GetSelectedRows().Length == 0) return;
                    var Id = int.Parse(gridView1.GetRowCellValue(gridView1.GetSelectedRows()[0], "Id").ToString());
                    var model = GlobalHandler.mstyperesp.Get(Id);
                    model.TYPE_KEY = txt_code.Text;
                    model.TYPE_NAME = txt_name.Text;
                    model.ICON = txt_pic.Text;
                    model.LXBS = txt_lxbs.Text;
                    model.TEMPLATE = txt_tem.Text;
                    model.EXPLAIN = txt_desc.Text;
                    model.SPATIALQUERYID = txt_spaceid.Text;
                    model.EXTENDCODE = txt_by.Text;
                    GlobalHandler.mstyperesp.Update(model);
                    EditData();
                }
                else
                {

                    int count = GlobalHandler.mstyperesp.Count(a => a.TYPE_KEY == txt_code.Text);
                    if (count > 0)
                    {
                        XtraMessageBox.Show("编号已存在！");
                        return;
                    }
                    var model = new MsTypeRecord();
                    model.TYPE_KEY = txt_code.Text;
                    model.TYPE_NAME = txt_name.Text;
                    model.ICON = txt_pic.Text;
                    model.LXBS = txt_lxbs.Text;
                    model.TEMPLATE = txt_tem.Text;
                    model.EXPLAIN = txt_desc.Text;
                    model.SPATIALQUERYID = txt_spaceid.Text;
                    model.EXTENDCODE = txt_by.Text;
                    GlobalHandler.mstyperesp.Insert(model);
                    CleanData();
                }
                XtraMessageBox.Show("数据保存成功");
                layoutControl1.Visible = !layoutControl1.Visible;
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show("数据保存失败");
                _log.Error("数据保存失败，出错提示：" + e.ToString());
            }

            GetData();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //取消
            layoutControl1.Visible = false;
            txt_code.Text ="";
            txt_name.Text = "";
            txt_by.Text = "";
            txt_desc.Text ="";
            txt_exp1.Text ="";
            txt_lxbs.Text = "";
            txt_spaceid.Text = "";
            txt_pic.Text ="";
            txt_url.Text ="";
            txt_tem.Text = "";
        }
        private void CleanData()
        {
            //编辑
            txt_code.Text = "";
            txt_name.Text = "";
            txt_by.Text = "";
            txt_desc.Text = "";
            txt_exp1.Text = "";
            txt_lxbs.Text = "";
            txt_spaceid.Text = "";
            txt_pic.Text = "";
            txt_url.Text = "";
            txt_tem.Text = "";
        }
        private void gridControl1_Click(object sender, EventArgs e)
        {
            EditData();
           
        }

        private void EditData()
        {
            try { 
            if (layoutControl1.Visible && isedit)
            {
                if (gridView1.GetSelectedRows().Length == 0) return;

                var Id = int.Parse(gridView1.GetRowCellValue(gridView1.GetSelectedRows()[0], "Id").ToString());
                var model = GlobalHandler.mstyperesp.Get(Id);
                oldcode = model?.TYPE_KEY;
                txt_code.Text = model?.TYPE_KEY;
                txt_name.Text = model?.TYPE_NAME;
                txt_by.Text = model?.EXTENDCODE;
                txt_desc.Text = model?.EXPLAIN;
                txt_exp1.Text = model?.SPATIALQUERYBY;
                txt_lxbs.Text = model?.LXBS;
                txt_spaceid.Text = model?.SPATIALQUERYID;
                txt_pic.Text = model?.ICON;
                txt_url.Text = model?.SPACESERVICEURL;
                txt_tem.Text = model?.TEMPLATE;
            }
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("获取数据出错");
                _log.Error("获取数据出错，出错提示：" + e.ToString());
            }
        }
    }

    class ComModel
    {
        private object _text = 0;
        private object _Value = "";
        /// <summary>
        /// 显示值
        /// </summary>
        public object Text
        {
            get { return this._text; }
            set { this._text = value; }
        }
        /// <summary>
        /// 对象值
        /// </summary>
        public object Value
        {
            get { return this._Value; }
            set { this._Value = value; }
        }

        public override string ToString()
        {
            return this.Text.ToString();
        }
    }
}
