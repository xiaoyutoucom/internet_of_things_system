using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SmartKylinApp.Common;
using SmartKylinData.IOTModel;
using Robin.Domain.Entities;
using DevExpress.XtraEditors;
using Newtonsoft.Json;
using RestSharp;

namespace SmartKylinApp.View.SMSConfigs
{
    public partial class SmsSendCtr : UserControl
    {
        public SmsSendCtr()
        {
            InitializeComponent();
        }

        private void SmsSendCtr_Load(object sender, EventArgs e)
        {
            InitialParamName();
            InitialTemplate();            
        }

        private Dictionary<string, string> dicItemMap = new Dictionary<string, string>();
        void InitialParamName()
        {
            dicItemMap.Clear();
            dicItemMap.Add("monitorid", "监测点编号");
            dicItemMap.Add("monitorname", "监测点名称");
            dicItemMap.Add("tagvalue", "监测值");
            dicItemMap.Add("tagdesc", "监测项");
            dicItemMap.Add("unit", "单位");
            dicItemMap.Add("time", "时间");
            dicItemMap.Add("phones", "发送手机号群");
            dicItemMap.Add("templatecode", "模板编号");
            dicItemMap.Add("params", "参数信息");
        }

        List<SmsConfigt> listSmsConfigt;
        private void InitialTemplate()
        {
            listSmsConfigt = GlobalHandler.msgSmsConfigtresp.GetAllList();
            List<SmsConfigt> lstConfig = new List<SmsConfigt>();
            listSmsConfigt.ForEach(a => { if (lstConfig.Where(p => p.NAME == a.NAME).Count() == 0) { lstConfig.Add(a); } });
            cmbTemplate.DisplayMember = "NAME";
            cmbTemplate.ValueMember = "CODE";
            cmbTemplate.DataSource = lstConfig;
            cmbTemplate.SelectedIndexChanged += CmbTemplate_SelectedIndexChanged;
            if (lstConfig.Count > 0)
            {
                cmbTemplate.SelectedIndex = 0;
                InitialForm(lstConfig.FirstOrDefault().CODE);
            }
        }
        List<Control> lstCtr = new List<Control>();
        private void CmbTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            string code = cmbTemplate.SelectedValue.ToString();
            if (string.IsNullOrEmpty(code))
            {
                return;
            }
            lstCtr.Clear();
            InitialForm(code);

        }

        private void InitialForm(string code)
        {
            IEnumerable<SmsConfigt> select = listSmsConfigt.Where(p => p.CODE.Trim() == code.Trim());

            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.RowCount = select.Count();
            tableLayoutPanel1.ColumnCount = 2;
            int i = 0;
            foreach (var v in select)
            {
                SmsConfigt s = v as SmsConfigt;
                LabelControl lc1 = new LabelControl();
                //lc1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
                //lc1.Location = new System.Drawing.Point(69, 16);
                //lc1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
                lc1.Dock = DockStyle.Left;
                lc1.Name = "lbl" + v.PARAMNAME.Trim();
                lc1.Size = new System.Drawing.Size(75, 20);
                lc1.TabIndex = 1;
                lc1.Text = dicItemMap[v.PARAMNAME.Trim().ToLower()];
                tableLayoutPanel1.Controls.Add(lc1, 0, i);

                if (v.PARAMNAME.Trim() != "Time")
                {
                    TextEdit te = new TextEdit();
                    te.Size = new Size(200, 20);
                    te.Dock = DockStyle.Left;
                    te.Name = "txt" + v.PARAMNAME.Trim();
                    tableLayoutPanel1.Controls.Add(te, 1, i);
                    lstCtr.Add(te);
                }
                else
                {
                    DateTimePicker dtp = new DateTimePicker();
                    dtp.CustomFormat = "yyyy-MM-dd HH:mm:ss";
                    dtp.Format = DateTimePickerFormat.Custom;
                    dtp.Size = new Size(200, 20);
                    dtp.Dock = DockStyle.Left;
                    dtp.Name = "txt" + v.PARAMNAME.Trim();
                    tableLayoutPanel1.Controls.Add(dtp, 1, i);
                    lstCtr.Add(dtp);
                }

                i++;
            }
        }

        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (CheckParam() == false)
            {
                return;
            }
            try
            {
                //设置模板编号
                paramModel.TemplateCode = cmbTemplate.SelectedValue.ToString().Trim();

                Send();
            }
            catch (Exception ex)
            {
                MessageBox.Show("发送异常："+ex.InnerException);
                return;
            }
        }
        private void Send()
        {
            string MessageHostUrl = ConfigHelp.Config["Application:Config:MessageHostUrl"]; //"http://59.203.106.22:9001";
            var client = new RestClient(MessageHostUrl);
            var request = new RestRequest("MessageCenter/IOTSms", Method.POST);
            request.AddParameter("data", JsonConvert.SerializeObject(paramModel)); 
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            if (content.Contains("true"))
            {
                MessageBox.Show("发送完成！");
            }
            else
            {
                MessageBox.Show("发送异常！");
            }
        }
        SMSModel paramModel = new SMSModel();
        private bool CheckParam()
        {
            //检测参数
            foreach (Control ctr in lstCtr)
            {
                if (string.IsNullOrEmpty(ctr.Text.Trim()))
                {                    
                    MessageBox.Show(dicItemMap[ctr.Name.Substring(3).ToLower()] + "不允许为空！");
                    return false;
                }
                paramModel.GetType().GetProperty(ctr.Name.Substring(3)).SetValue(paramModel, ctr.Text.Trim());
            }
            //检测联系人
            if (btnPersion.Text.Trim() == "" || paramModel.Phones.Length == 0)
            {
                MessageBox.Show("请选择联系人！");
                return false;
            }

            return true;
        }

        private void btnPersion_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            btnPersion.Text = "";
            SelectPerson sp = new SelectPerson();
            sp.StartPosition = FormStartPosition.CenterScreen;
            sp.ShowDialog();
            List<Contact> lstSelect = sp.lstSelect;
            List<string> lstPhone = new List<string>();
            lstSelect.ForEach(a => { lstPhone.Add(a.PHONE); btnPersion.Text += a.NAME + ","; });
            paramModel.Phones = lstPhone.ToArray();
        }
    }
    public class TemplateParams : Entity<int>
    {
        /// <summary>
        /// 模板编号
        /// </summary>
        public virtual string CODE { get; set; }
        /// <summary>
        /// 模板名称
        /// </summary>
        public virtual string NAME { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public virtual string PARAMNAME { get; set; }
        /// <summary>
        /// 参数顺序
        /// </summary>
        public virtual int DISORDER { get; set; }
    }
    //
    // 摘要:
    //     短信标准传输信息实体
    public class SMSModel
    {
        //
        // 摘要:
        //     监测点编号
        public string MonitorId { get; set; }
        //
        // 摘要:
        //     监测点名称
        public string MonitorName { get; set; }
        //
        // 摘要:
        //     监测值
        public string TagValue { get; set; }
        //
        // 摘要:
        //     监测项
        public string TagDesc { get; set; }
        //
        // 摘要:
        //     单位
        public string Unit { get; set; }
        //
        // 摘要:
        //     时间
        public string Time { get; set; }
        //
        // 摘要:
        //     发送手机号群
        public string[] Phones { get; set; }
        //
        // 摘要:
        //     模板编号
        public string TemplateCode { get; set; }
        //
        // 摘要:
        //     参数信息
        public string[] Params { get; set; }
    }
}
