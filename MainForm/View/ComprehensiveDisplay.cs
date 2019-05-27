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
using ServiceStack;
using SmartKylinData.IOTModel;
using System.Web.UI.WebControls;
using SmartKylinData.BaseModel;
using System.IO;
using NPOI.HSSF.UserModel;
using log4net;
using MongoDB.Bson;
using MongoDB.Driver;
using DevExpress.XtraCharts;
using DevExpress.XtraLayout.Utils;
using Label = System.Windows.Forms.Label;
using System.Reflection.Emit;
using DevExpress.XtraGauges.Core.Model;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Alerter;

namespace SmartKylinApp.View.Query
{
    public partial class ComprehensiveDisplay : DevExpress.XtraEditors.XtraUserControl
    {
        private ILog _log = LogManager.GetLogger("ComprehensiveDisplay");
        private List<Smart_Kylin_Runtime> rlist;
        private Timer timea;
        private bool frist = true;
        private Label lab3;
        private Label lab4;
        private Label lab22;
        private Label lab2;
        private Label lab1;
        private List<ConfigRecord> cfglist;
        private List<DataListModel> dllist;
        private List<BasicMonitorRecord> bmrlist;
        private object sd;
        private bool fristload = true;
        private string[] tp;

        //private string mc;
        //private string key;

        public ComprehensiveDisplay()
        {
            InitializeComponent();
            //禁止右键菜单
            layoutControl2.AllowCustomization = false;
            layoutControl1.AllowCustomization = false;
            bar2.Manager.AllowShowToolbarsPopup = false;
            bar2.OptionsBar.AllowQuickCustomization = false;
            gridView2.OptionsMenu.EnableColumnMenu = false;
        }

        private List<MsTypeRecord> lstMsType;
        string typeCountInfo = "";
        private void ComprehensiveDisplay_Load(object sender, EventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            splashScreenManager1.SetWaitFormCaption("请稍后,数据加载中....");     // 标题
            splashScreenManager1.SetWaitFormDescription("正在更新数据.....");

            lstMsType = GlobalHandler.mstyperesp.GetAllList();
            //getLoad();
            getData();//先启动刷新数据一次
            //定时器，根据配置时间刷新
            timea = new Timer();
            int t = int.Parse(ConfigHelp.Config["Application:Config:Time"]);
            barTime.Caption = "刷新频率：" + t.ToString();
            timea.Interval = 60000*t;
            timea.Enabled = true;
            timea.Tick += Time_Tick;

        }

        //private void getLoad()
        //{
        //    if(cfglist==null)
        //    { 
        //    cfglist = GlobalHandler.configresp.GetAllList();
        //    }

        //}
        //计时器
        private void Time_Tick(object sender, EventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            splashScreenManager1.SetWaitFormCaption("请稍后,数据加载中....");     // 标题
            splashScreenManager1.SetWaitFormDescription("正在更新数据.....");
            upData("1");
            splashScreenManager1.CloseWaitForm();
            ReClick();
            ShowAlertControl(typeCountInfo);
        }
        //重新刷新详细数据
        private void ReClick()
        {
            if (sd == null) return;
            Label l = (Label)sd;
            var labname=l.Name.Substring(0, 6);
            if (labname == "lab_bj")
            {
                Lab_bj_Click(sd, null);
            }
            if (labname == "lab_zc")
            {
                Lab_zc_Click(sd, null);
            }
            if (labname == "lab_zs")
            {
                Lab_zs_Click(sd, null);
            }
            if (labname == "lab_lc")
            {
                Lab_lx_Click(sd, null);
            }

        }
        //代码生成界面及点击事件
        private void getData()
        {
            try {
                typeCountInfo = "";

                tableLayoutPanel1.Controls.Clear();
                //供水03 热力04 排水01 燃气02 井盖990498
                rlist = GlobalHandler.runtimeResp.Table.ToList();
                cfglist = GlobalHandler.configresp.GetAllList();
                bmrlist= GlobalHandler.monitorresp.GetAllList();
                barDate.Caption = "最后更新时间：" + DateTime.Now.ToLongTimeString().ToString();
                var Type = ConfigHelp.Config["Application:Config:Type"];
                 tp = Type.Split(',');
                this.tableLayoutPanel1.RowCount = tp.Length / 2;
                string mc, key;
                for (int i = 0; i < tp.Length; i++)
                {
                    mc = tp[i].Split(':')[0];
                    key = tp[i].Split(':')[1];
                    GroupControl ct1 = new GroupControl();
                    TableLayoutPanel typ1 = new TableLayoutPanel();
                    LabelControl lc1 = new LabelControl();
                    lc1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
                    lc1.Location = new System.Drawing.Point(69, 16);
                    lc1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
                    lc1.Name = "lczs" + key;
                    lc1.Size = new System.Drawing.Size(75, 18);
                    lc1.TabIndex = 16;
                    lc1.Text = "监测点总数：";                    
                    LabelControl lc2 = new LabelControl();
                    lc2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
                    lc2.Location = new System.Drawing.Point(69, 16);
                    lc2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
                    lc2.Name = "lczc" + key;
                    lc2.Size = new System.Drawing.Size(75, 18);
                    lc2.TabIndex = 16;
                    lc2.Text = "在线个数：";
                    LabelControl lc3 = new LabelControl();
                    lc3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
                    lc3.Location = new System.Drawing.Point(69, 16);
                    lc3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
                    lc3.Name = "lcbj" + key;
                    lc3.Size = new System.Drawing.Size(75, 18);
                    lc3.TabIndex = 16;
                    lc3.Text = "报警个数：";
                    LabelControl lc4 = new LabelControl();
                    lc4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
                    lc4.Location = new System.Drawing.Point(69, 16);
                    lc4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
                    lc4.Name = "lclx" + key;
                    lc4.Size = new System.Drawing.Size(75, 18);
                    lc4.TabIndex = 16;
                    lc4.Text = "离线个数：";
                    lab1 = new Label();
                    lab1.AutoSize = true;
                    lab1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Underline);
                    lab1.ForeColor = System.Drawing.Color.Blue;
                    lab1.Cursor = Cursors.Hand;
                    lab1.Location = new System.Drawing.Point(147, 56);
                    lab1.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
                    lab1.Name = "lab_zs" + key;
                    lab1.Size = new System.Drawing.Size(16, 18);
                    lab1.TabIndex = 18;
                    lab1.Tag = key;
                    lab1.Text = "0";
                    lab1.Click += new EventHandler(Lab_zs_Click);                    
                    lab2 = new Label();
                    lab2.AutoSize = true;
                    lab2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Underline);
                    lab2.ForeColor = System.Drawing.Color.Green;
                    lab2.Cursor = Cursors.Hand;
                    lab2.Location = new System.Drawing.Point(147, 56);
                    lab2.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
                    lab2.Name = "lab_zc" + key;
                    lab2.Tag = key;
                    lab2.Size = new System.Drawing.Size(16, 18);
                    lab2.TabIndex = 18;
                    lab2.Text = "0";
                    lab2.Click += new EventHandler(Lab_zc_Click);
                    lab3 = new Label();
                    lab3.AutoSize = true;
                    lab3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Underline);
                    lab3.ForeColor = System.Drawing.Color.Red;
                    lab3.Cursor = Cursors.Hand;
                    lab3.Location = new System.Drawing.Point(147, 56);
                    lab3.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
                    lab3.Name = "lab_bj" + key;
                    lab3.Tag = key;
                    lab3.Size = new System.Drawing.Size(16, 18);
                    lab3.TabIndex = 18;
                    lab3.Text = "0";
                    lab3.Click += new EventHandler(Lab_bj_Click);
                    lab4 = new Label();
                    lab4.AutoSize = true;
                   
                    lab4.ForeColor = System.Drawing.Color.Gray;
                    lab4.Cursor = Cursors.Hand;
                    lab4.Location = new System.Drawing.Point(147, 56);
                    lab4.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
                    lab4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Underline);
                    lab4.Name = "lab_lx" + key;
                    lab4.Tag = key;
                    lab4.Size = new System.Drawing.Size(16, 18);
                    lab4.TabIndex = 18;
                    lab4.Text = "0";
                    lab4.Click += new EventHandler(Lab_lx_Click);
                    //typ1.Controls.Add(ct1, 0, 0);
                    typ1.ColumnCount = 2;
                    typ1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                    typ1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                    typ1.Controls.Add(lc1, 0, 1);
                    typ1.Controls.Add(lc2, 0, 2);
                    typ1.Controls.Add(lc3, 0, 3);
                    typ1.Controls.Add(lc4, 0, 4);
                    typ1.Controls.Add(lab1, 1, 1);
                    typ1.Controls.Add(lab2, 1, 2);
                    typ1.Controls.Add(lab3, 1, 3);
                    typ1.Controls.Add(lab4, 1, 4);
                    typ1.Dock = System.Windows.Forms.DockStyle.Fill;
                    typ1.Location = new System.Drawing.Point(2, 27);
                    typ1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
                    typ1.Name = "typ" + key;
                    typ1.RowCount = 6;
                    typ1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
                    typ1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22.22222F));
                    typ1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22.22222F));
                    typ1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22.22222F));
                    typ1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22.22222F));
                    typ1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.444445F));
                    typ1.Size = new System.Drawing.Size(294, 183);
                    typ1.TabIndex = i;

                    ct1.Controls.Add(typ1);
                    ct1.Dock = System.Windows.Forms.DockStyle.Fill;
                   
                    ct1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
                    ct1.Name = "ct" + key;
                    ct1.Size = new System.Drawing.Size(298, 212);
                    ct1.TabIndex = i;
                    ct1.Text = mc;
                    this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100/tp.Length / 2));
                    if (i == 0)
                    {
                        this.tableLayoutPanel1.Controls.Add(ct1, 0, 0);
                    }
                    else
                    {
                        int a, b;
                        if (i%2==0)
                        {
                             a = i / 2;
                             b = 0;
                        }
                        else
                        {
                            a = i / 2;
                            b = 1;
                        }
                        this.tableLayoutPanel1.Controls.Add(ct1, a, b);
                    }


                    //监测总数
                    //int zs = cfglist.Where(a => a.CONFIG_CODE.ToString().Substring(6, 2) == key).Count();
                    int zs = bmrlist.Count(a => a.BMID.Substring(6, 2) == key);
                    lab1.Text = zs.ToString();                    

                    //报警检测项个数
                    //int bj = rlist.Where(a => a.CONFIG_CODE.Substring(6, 2) == key).Where(a => a.LEVEL == "1").Where(a => a.STATUS == "1").Count();
                    //lab3.Text = bj.ToString();
                    //报警监测点个数
                    var alst = rlist.Where(a => a.CONFIG_CODE.Substring(6, 2) == key && a.LEVEL == "1" && a.STATUS == "1").ToList().GroupBy(p => p.CONFIG_CODE.Substring(0, 19)).Select(p => p.Key);
                     int alert=alst.Count();
                    lab3.Text = alert.ToString();

                    typeCountInfo += mc + ":" + alert.ToString() + "\r\n";

                    //离线检测点个数
                    //var llst = rlist.Where(a => a.CONFIG_CODE.Substring(6, 2) == key && (a.STATUS == "0" || a.STATUS == null)).ToList().GroupBy(p => p.CONFIG_CODE.Substring(0, 19)).Select(p => p.Key);
                    //int llx=llst.Count();
                    //lab4.Text = llx.ToString();
                    //离线检测项个数
                    //int lx = rlist.Where(a => a.CONFIG_CODE.Substring(6, 2) == key).Where(a => a.STATUS == "0"|| a.STATUS == null).Count();
                    //lab4.Text = lx.ToString();

                    //正常检测项个数
                    //int zc = rlist.Where(a => a.CONFIG_CODE.Substring(6, 2) == key).Where(a => a.LEVEL == "0").Where(a => a.STATUS == "1").Count();
                    //lab2.Text = zc.ToString();
                    //正常检测点个数
                    //var jcdzs = rlist.Where(a => a.CONFIG_CODE.Substring(6, 2) == key).GroupBy(p => p.CONFIG_CODE.Substring(0, 19)).Select(p => p.Key).ToList();
                    //List<string> zcjcd=new List<string>();
                    //foreach (var k in jcdzs)
                    //{
                    //    if (alst.Contains(k) == true)
                    //    {
                    //        continue;
                    //    }
                    //    if (llst.Contains(k) == true)
                    //    {
                    //        continue;
                    //    }
                    //    zcjcd.Add(k);
                    //}
                    //int zcjcds = zcjcd.Count();
                    //lab2.Text = zcjcds.ToString();
                    //在线监测点个数
                    int zxgs = rlist.Where(a => a.CONFIG_CODE.Substring(6, 2) == key && a.STATUS == "1").GroupBy(p => p.CONFIG_CODE.Substring(0, 19)).Select(p => p.Key).Count();
                    lab2.Text = zxgs.ToString();

                    //离线监测点个数
                    lab4.Text = (zs-zxgs).ToString();

                }
                splashScreenManager1.CloseWaitForm();
                return;
            }
            catch(Exception e)
            {
                splashScreenManager1.CloseWaitForm();
                XtraMessageBox.Show("获取总览数据失败");
                _log.Error("获取总览数据失败，出错提示：" + e.ToString());
            }
            finally
            {

            }
        }
        //更显数据
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">type=1更新除燃气井盖以外的数据；type=2更新燃气；type=3更新井盖</param>
        private void upData(string type)
        {
            try
            {
                typeCountInfo = "";
                barDate.Caption = "最后更新时间：" + DateTime.Now.ToLongTimeString().ToString();
                string mc, key;
                rlist = GlobalHandler.runtimeResp.Table.ToList();
                cfglist = GlobalHandler.configresp.GetAllList();
                bmrlist = GlobalHandler.monitorresp.GetAllList();
                for (int i = 0; i < tp.Length; i++)
                {
                    mc = tp[i].Split(':')[0];
                    key = tp[i].Split(':')[1];
                    if (type == "1")
                    {
                        if (key == "02" || key == "99")
                        {
                            continue;
                        }
                    }
                    else if (type == "2")
                    {
                        if (key != "02")
                        {
                            continue;
                        }
                    }
                    else if (type == "3")
                    {
                        if (key != "99")
                        {
                            continue;
                        }
                    }
                    Control ct1 = Controls.Find("lab_zs" + key, true)[0];
                    Control ct2 = Controls.Find("lab_zc" + key, true)[0];
                    Control ct3 = Controls.Find("lab_bj" + key, true)[0];
                    Control ct4 = Controls.Find("lab_lx" + key, true)[0];
                    //监测总数
                    //int zs = cfglist.Where(a => a.CONFIG_CODE.ToString().Substring(6, 2) == key).Count();
                    //int zs = rlist.Where(a => a.CONFIG_CODE.Substring(6, 2) == key).Count();
                    int zs = bmrlist.Count(a => a.BMID.Substring(6, 2) == key);
                    ct1.Text = zs.ToString();
                    //正常个数
                    //int zc = rlist.Where(a => a.CONFIG_CODE.Substring(6, 2) == key && (a.STATUS == "1") && (a.LEVEL == "0" || a.LEVEL == null)).Count();
                    //ct2.Text = zc.ToString();

                    //报警个数
                    //int bj = rlist.Where(a => a.CONFIG_CODE.Substring(6, 2) == key && a.LEVEL == "1" && a.STATUS == "1").Count();
                    //ct3.Text = bj.ToString();

                    //typeCountInfo += mc + ":" + bj.ToString() + "\r\n";

                    //离线个数
                    //int lx = rlist.Where(a => a.CONFIG_CODE.Substring(6, 2) == key && (a.STATUS == "0" || a.STATUS == null)).Count();
                    //ct4.Text = lx.ToString();

                    //报警监测点个数
                    var alst = rlist.Where(a => a.CONFIG_CODE.Substring(6, 2) == key && a.LEVEL == "1" && a.STATUS == "1").ToList().GroupBy(p => p.CONFIG_CODE.Substring(0, 19)).Select(p => p.Key);
                    int alert = alst.Count();
                    ct3.Text = alert.ToString();

                    typeCountInfo += mc + ":" + alert.ToString() + "\r\n";

                    //离线检测点个数
                    var llst = rlist.Where(a => a.CONFIG_CODE.Substring(6, 2) == key && (a.STATUS == "0" || a.STATUS == null)).ToList().GroupBy(p => p.CONFIG_CODE.Substring(0, 19)).Select(p => p.Key);
                    int llx = llst.Count();
                    ct4.Text = llx.ToString();

                    //正常检测点个数
                    var jcdzs = rlist.Where(a => a.CONFIG_CODE.Substring(6, 2) == key).GroupBy(p => p.CONFIG_CODE.Substring(0, 19)).Select(p => p.Key).ToList();
                    List<string> zcjcd = new List<string>();
                    foreach (var k in jcdzs)
                    {
                        if (alst.Contains(k) == true)
                        {
                            continue;
                        }
                        if (llst.Contains(k) == true)
                        {
                            continue;
                        }
                        zcjcd.Add(k);
                    }
                    int zcjcds = zcjcd.Count();
                    ct2.Text = zcjcds.ToString();
                }
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show("更新数据失败");
                _log.Error("更新数据失败，出错提示：" + exception.ToString());
            }

        }
        //总数点击事件显示数据
        private void Lab_zs_Click(object sender, EventArgs e)
        {
            try
            {

                splashScreenManager1.ShowWaitForm();
                splashScreenManager1.SetWaitFormCaption("请稍后,数据加载中....");     // 标题
                splashScreenManager1.SetWaitFormDescription("正在更新数据.....");
                //getLoad();
                sd = sender;
                Label la = (Label)sender;
                var key = la.Tag.ToString();
                List<ConfigRecord> listr = cfglist.Where(a => a.CONFIG_CODE.ToString().Substring(6, 2) == key).ToList();
                dllist = new List<DataListModel>();
                Smart_Kylin_Runtime cr = new Smart_Kylin_Runtime();
                string msType = "";
                foreach (ConfigRecord dtm in listr)
                {
                    cr = rlist.FirstOrDefault(a => a.CONFIG_CODE == dtm.CONFIG_CODE);
                    msType = lstMsType.Where(p => p.TYPE_KEY == dtm.STATIONID.STATIONTYPE).FirstNonDefault()?.TYPE_NAME;
                    if (cr == null)
                    {
                        dllist.Add(new DataListModel() { CONFIG_CODE = dtm.CONFIG_CODE, CONFIG_VALUE = 0, SAVE_DATE = "", CONFIGMC = dtm.CONFIG_DESC, MONITORMC = dtm.STATIONID.BMMC, MONITORTYPE=msType });
                    }
                    else
                    {
                        dllist.Add(new DataListModel() { CONFIG_CODE = dtm.CONFIG_CODE, CONFIG_VALUE = cr.CONFIG_VALUE, SAVE_DATE = cr.SAVE_DATE.ToString(), CONFIGMC = dtm.CONFIG_DESC, MONITORMC = dtm.STATIONID.BMMC, MONITORTYPE = msType });
                    }
                }
                gridView2.IndicatorWidth = 12 + 9 * la.Text.Length;//行号宽度       
                gridControl1.DataSource = dllist;
                splashScreenManager1.CloseWaitForm();
            }
            catch (Exception ex)
            {
                splashScreenManager1.CloseWaitForm();
                XtraMessageBox.Show("获取详细数据失败");
                _log.Error("获取详细数据失败，出错提示：" + ex.ToString());
            }
        }
        //正常点击事件显示数据
        private void Lab_zc_Click(object sender, EventArgs e)
        {
            try
            {

                splashScreenManager1.ShowWaitForm();
                splashScreenManager1.SetWaitFormCaption("请稍后,数据加载中....");     // 标题
                splashScreenManager1.SetWaitFormDescription("正在更新数据.....");
                //getLoad();
                sd = sender;
                Label la = (Label)sender;
                var key = la.Tag.ToString();
                //List < Smart_Kylin_Runtime > listr = rlist.Where(a => a.CONFIG_CODE.Substring(6, 2) == key && (a.STATUS == "1") && (a.LEVEL == "0"||a.LEVEL==null)).ToList();
                //在线监测点
                List<Smart_Kylin_Runtime> listr = rlist.Where(a => a.CONFIG_CODE.Substring(6, 2) == key && a.STATUS == "1").ToList();
                dllist = new List<DataListModel>();
                ConfigRecord cr = new ConfigRecord();
                string msType = "";
                foreach (Smart_Kylin_Runtime dtm in listr)
                {
                    cr = GlobalHandler.configresp.FirstOrDefault(a => a.CONFIG_CODE == dtm.CONFIG_CODE);                    
                    if (cr == null)
                    {
                        dllist.Add(new DataListModel() { CONFIG_CODE = dtm.CONFIG_CODE, CONFIG_VALUE = dtm.CONFIG_VALUE, SAVE_DATE = dtm.SAVE_DATE.ToString(), CONFIGMC = "", MONITORMC = "", MONITORTYPE = "" });
                    }
                    else
                    {
                        msType = lstMsType.Where(p => p.TYPE_KEY == cr.STATIONID.STATIONTYPE).FirstNonDefault()?.TYPE_NAME;
                        dllist.Add(new DataListModel() { CONFIG_CODE = dtm.CONFIG_CODE, CONFIG_VALUE = dtm.CONFIG_VALUE, SAVE_DATE = dtm.SAVE_DATE.ToString(), CONFIGMC = cr.CONFIG_DESC, MONITORMC = cr.STATIONID.BMMC, MONITORTYPE = msType });
                    }
                }
                gridView2.IndicatorWidth = 12 + 9 * la.Text.Length;//行号宽度       
                gridControl1.DataSource = dllist;
                splashScreenManager1.CloseWaitForm();
            }
            catch (Exception ex)
            {
                splashScreenManager1.CloseWaitForm();
                XtraMessageBox.Show("获取详细数据失败");
                _log.Error("获取详细数据失败，出错提示：" + ex.ToString());
            }
        }
        //离线点击事件显示数据
        private void Lab_lx_Click(object sender, EventArgs e)
        {
            try
            {

                splashScreenManager1.ShowWaitForm();
                splashScreenManager1.SetWaitFormCaption("请稍后,数据加载中....");     // 标题
                splashScreenManager1.SetWaitFormDescription("正在更新数据.....");
                //getLoad();
                sd = sender;
                Label la = (Label)sender;
                var key = la.Tag.ToString();
                List<Smart_Kylin_Runtime> listr = rlist.Where(a => a.CONFIG_CODE.Substring(6, 2) == key && (a.STATUS == "0"|| a.STATUS == null)).ToList();
                //dllist = new List<DataListModel>();
                //ConfigRecord cr = new ConfigRecord();
                //string msType = "";
                //foreach (Smart_Kylin_Runtime dtm in listr)
                //{
                //    cr = GlobalHandler.configresp.FirstOrDefault(a => a.CONFIG_CODE == dtm.CONFIG_CODE);
                //    if (cr == null)
                //    {
                //        dllist.Add(new DataListModel() { CONFIG_CODE = dtm.CONFIG_CODE, CONFIG_VALUE = dtm.CONFIG_VALUE, SAVE_DATE = dtm.SAVE_DATE.ToString(), CONFIGMC = "", MONITORMC = "", MONITORTYPE = "" });
                //    }
                //    else
                //    {
                //        msType = lstMsType.Where(p => p.TYPE_KEY == cr.STATIONID.STATIONTYPE).FirstNonDefault()?.TYPE_NAME;
                //        dllist.Add(new DataListModel() { CONFIG_CODE = dtm.CONFIG_CODE, CONFIG_VALUE = dtm.CONFIG_VALUE, SAVE_DATE = dtm.SAVE_DATE.ToString(), CONFIGMC = cr.CONFIG_DESC, MONITORMC = cr.STATIONID.BMMC, MONITORTYPE = msType });
                //    }
                //}

                List<ConfigRecord> listr1 = cfglist.Where(a => a.CONFIG_CODE.ToString().Substring(6, 2) == key).ToList();
                //在线监测点
                List<Smart_Kylin_Runtime> listzx = rlist.Where(a => a.CONFIG_CODE.Substring(6, 2) == key && a.STATUS == "1").ToList();
                dllist = new List<DataListModel>();
                Smart_Kylin_Runtime cr1 = new Smart_Kylin_Runtime();
                string msType1 = "";
                foreach (ConfigRecord dtm in listr1)
                {
                    cr1= listzx.FirstOrDefault(a => a.CONFIG_CODE == dtm.CONFIG_CODE);
                    //检测项所属监测点在线
                    if (cr1 != null)
                    {
                        continue;
                    }
                    cr1 = listr.FirstOrDefault(a => a.CONFIG_CODE == dtm.CONFIG_CODE);
                    msType1 = lstMsType.Where(p => p.TYPE_KEY == dtm.STATIONID.STATIONTYPE).FirstNonDefault()?.TYPE_NAME;
                    if (cr1 == null)
                    {
                        dllist.Add(new DataListModel() { CONFIG_CODE = dtm.CONFIG_CODE, CONFIG_VALUE = 0, SAVE_DATE = "", CONFIGMC = dtm.CONFIG_DESC, MONITORMC = dtm.STATIONID.BMMC, MONITORTYPE = msType1 });
                    }
                    else
                    {
                        dllist.Add(new DataListModel() { CONFIG_CODE = dtm.CONFIG_CODE, CONFIG_VALUE = cr1.CONFIG_VALUE, SAVE_DATE = cr1.SAVE_DATE.ToString(), CONFIGMC = dtm.CONFIG_DESC, MONITORMC = dtm.STATIONID.BMMC, MONITORTYPE = msType1 });
                    }
                }

                gridView2.IndicatorWidth = 12 + 9 * la.Text.Length;//行号宽度       
                gridControl1.DataSource = dllist;
                splashScreenManager1.CloseWaitForm();
            }
            catch (Exception ex)
            {
                splashScreenManager1.CloseWaitForm();
                XtraMessageBox.Show("获取详细数据失败");
                _log.Error("获取详细数据失败，出错提示：" + ex.ToString());
            }
        }
        //报警点击事件显示数据
        private void Lab_bj_Click(object sender, EventArgs e)
        {
            try
            {
                splashScreenManager1.ShowWaitForm();
                splashScreenManager1.SetWaitFormCaption("请稍后,数据加载中....");     // 标题
                splashScreenManager1.SetWaitFormDescription("正在更新数据.....");
                // getLoad();
                sd = sender;
                Label la = (Label)sender;
                gridView2.IndicatorWidth = 12 + 9 * la.Text.Length;//行号宽度       
                var key = la.Tag.ToString();
                List<Smart_Kylin_Runtime> listr = rlist.Where(a => a.CONFIG_CODE.Substring(6, 2) == key && (a.STATUS == "1")&& a.LEVEL=="1").ToList();
                dllist = new List<DataListModel>();
                ConfigRecord cr = new ConfigRecord();
                string msType = "";
                foreach (Smart_Kylin_Runtime dtm in listr)
                {
                    cr = GlobalHandler.configresp.FirstOrDefault(a => a.CONFIG_CODE == dtm.CONFIG_CODE);
                    if (cr == null)
                    {
                        dllist.Add(new DataListModel() { CONFIG_CODE = dtm.CONFIG_CODE, CONFIG_VALUE = dtm.CONFIG_VALUE, SAVE_DATE = dtm.SAVE_DATE.ToString(), CONFIGMC = "", MONITORMC = "", MONITORTYPE = msType });
                    }
                    else { 
                    msType = lstMsType.Where(p => p.TYPE_KEY == cr.STATIONID.STATIONTYPE).FirstNonDefault()?.TYPE_NAME;
                    dllist.Add(new DataListModel() { CONFIG_CODE = dtm.CONFIG_CODE, CONFIG_VALUE = dtm.CONFIG_VALUE, SAVE_DATE = dtm.SAVE_DATE.ToString(), CONFIGMC = cr.CONFIG_DESC, MONITORMC = cr.STATIONID.BMMC, MONITORTYPE = msType });
                    }
                }
                gridControl1.DataSource = dllist;
                splashScreenManager1.CloseWaitForm();
            }
            catch (Exception ex)
            {
                splashScreenManager1.CloseWaitForm();
                XtraMessageBox.Show("获取详细数据失败");
                _log.Error("获取详细数据失败，出错提示：" + ex.ToString());
            }
        }
        //行号显示
        private void gridView2_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle > -1)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
        //关闭或切换窗体计时器关闭
        private void ComprehensiveDisplay_VisibleChanged(object sender, EventArgs e)
        {
            if(!frist)
            { 
            timea.Stop();
            }
            else
            {
                frist = false;
            }
        }
        //刷新
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            splashScreenManager1.SetWaitFormCaption("请稍后,数据加载中....");     // 标题
            splashScreenManager1.SetWaitFormDescription("正在更新数据.....");
            upData("1");
            splashScreenManager1.CloseWaitForm();
            ReClick();
            ShowAlertControl(typeCountInfo);
        }
        //导出数据
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (list == null || list.Count < 1)
            //{
            //    XtraMessageBox.Show("导出数据为空！");
            //    return;
            //}
            //导出
            FileStream fs = null;
            try
            {
                var saveFileName = "数据总览.xls";
                fs = new FileStream(saveFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                var workbook = new HSSFWorkbook();
                var sheet = workbook.CreateSheet();
                string[] names = new string[] { "监测点名称", "监测项名称", "监测项编码", "监测值","保存时间" };
                var headerRow = sheet.CreateRow(0);
                for (int i = 0; i < names.Length; i++)
                {
                    sheet.SetColumnWidth(i, 30 * 256);
                    headerRow.CreateCell(i).SetCellValue(names[i]);
                }
                for (var i = 0; i < dllist.Count; i++)
                {
                    var row = sheet.CreateRow(i + 1);
                    row.CreateCell(0).SetCellValue(dllist[i].MONITORMC);
                    row.CreateCell(1).SetCellValue(dllist[i].CONFIGMC);
                    row.CreateCell(2).SetCellValue(dllist[i].CONFIG_CODE);
                    row.CreateCell(3).SetCellValue(dllist[i].CONFIG_VALUE);
                    row.CreateCell(4).SetCellValue(dllist[i].SAVE_DATE.ToString());
                }
                sheet.CreateFreezePane(0, 1, 0, 1);
                var saveDialog = new SaveFileDialog
                {
                    DefaultExt = "xls",
                    Filter = @"Excel文件|*.xls;*.xlsx",
                    FileName = saveFileName
                };
                saveDialog.ShowDialog();
                saveFileName = saveDialog.FileName;
                if (saveFileName.IndexOf(":", StringComparison.Ordinal) < 0) return; //被点了取消

                if (saveFileName == "") return;
                try
                {
                    fs = File.OpenWrite(saveDialog.FileName);
                    workbook.Write(fs);
                    XtraMessageBox.Show(@"导出成功！");
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(@"导出文件时出错,文件可能正被打开！\n" + ex.Message);
                }
            }
            catch (Exception exception)
            {
                //fs?.Close();
                _log.Error("导出数据出错，出错提示：" + exception.ToString());
            }
            finally
            {
                fs?.Close();
            }
        }

        /// <summary>
        /// true:每次刷新都弹出提示窗；false刷新不弹出报警提示窗。
        /// </summary>
        bool flag = true;
        private void barToggleSwitchItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //BarToggleSwitchItem ts = sender as BarToggleSwitchItem;
        }
        private int alertheight = 0;
        private void ShowAlertControl(string msg)
        {
            try
            {
                if (barToggleSwitchItem1.Checked == false)
                {
                    return;
                }
                AlertControl alertControl1 = new AlertControl();
                alertControl1.AutoFormDelay = 10000;//弹框显示10秒
                //alertControl1.LookAndFeel.SkinName = "Office 2007 Blue";
                alertControl1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
                alertControl1.BeforeFormShow += new AlertFormEventHandler(alertControl1_BeforeFormShow);
                alertControl1.Show(null, "预警提示信息", msg, File.Exists(Application.StartupPath + "\\logo.ico") ? System.Drawing.Image.FromFile(Application.StartupPath + "\\logo.ico") : null);
                alertControl1.BeforeFormShow -= new AlertFormEventHandler(alertControl1_BeforeFormShow);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "提示信息失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void alertControl1_BeforeFormShow(object sender, AlertFormEventArgs e)
        {
            e.AlertForm.Size = new Size(e.AlertForm.Size.Width + 50, 250 + alertheight);
        }

        private void barToggleSwitchItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            BarToggleSwitchItem ts = sender as BarToggleSwitchItem;
            timea.Enabled = !ts.Checked;
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            splashScreenManager1.SetWaitFormCaption("请稍后,数据加载中....");     // 标题
            splashScreenManager1.SetWaitFormDescription("正在更新数据.....");
            upData("2");
            splashScreenManager1.CloseWaitForm();
            ReClick();
            ShowAlertControl(typeCountInfo);
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            splashScreenManager1.SetWaitFormCaption("请稍后,数据加载中....");     // 标题
            splashScreenManager1.SetWaitFormDescription("正在更新数据.....");
            upData("3");
            splashScreenManager1.CloseWaitForm();
            ReClick();
            ShowAlertControl(typeCountInfo);
        }
    }
}
