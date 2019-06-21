using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraWaitForm;
using log4net;
using SmartKylinApp.Common;
using SmartKylinData.IOTModel;

namespace SmartKylinApp.View.BaseConfig
{
    public partial class BaseConfigProgress : WaitForm
    {
        private ILog _log = LogManager.GetLogger("BaseConfigProgress");

        public DataTable datatable;

        public string Topic;

        public StringBuilder logbuild;

        public BaseConfigProgress()
        {
            InitializeComponent();
            this.progressPanel1.AutoHeight = true;
        }

        #region Overrides

        public override void SetCaption(string caption)
        {
            base.SetCaption(caption);
            this.progressPanel1.Caption = caption;
        }
        public override void SetDescription(string description)
        {
            base.SetDescription(description);
            this.progressPanel1.Description = description;
        }
        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion

        public enum WaitFormCommand
        {

        }

        private Action<string> aa;
        private void BaseConfigProgress_Shown(object sender, EventArgs e)
        {
            logbuild =new StringBuilder();
            var a = new Thread(aa =>
              {
              try
              {
                  logbuild.Clear();
                //导入数据
                if (datatable != null)
                  {
                      if (Topic == "device")
                      {
                          foreach (DataRow row in datatable.Rows)
                          {
                              var mstype = row[0].ToString();
                              var sbbm = "";
                              var count = GlobalHandler.deviceresp.Count(s => s.SBBM.StartsWith(mstype));
                              if (count > 0)
                              {
                                  var str = (int.Parse(GlobalHandler.deviceresp.GetAllList(s => s.SBBM.StartsWith(mstype))
                                                         .OrderByDescending(q => q.SBBM.Substring(6))
                                                         .FirstOrDefault()
                                                         ?.SBBM.Substring(6) ?? throw new InvalidOperationException()) + 1)
                                    .ToString();
                                  var hl = "";
                                  for (var i = 0; i < 5 - str.Length; i++)
                                      hl += "0";
                                  sbbm = mstype + hl + str;
                              }
                              else
                              {
                                  sbbm = mstype + @"00001";
                              }

                              var model = new DeviceRecord();

                              model.SBBM = sbbm;
                              model.SBTYPE = mstype;
                              model.SBMC = row[1].ToString();
                              model.CITYCODE = row[2].ToString();
                              model.CITYNAME = row[3].ToString();
                              model.CCBH = row[4].ToString();
                              model.SCCJ = row[5].ToString();
                              model.FREQUENCY = double.Parse(row[6].ToString() ?? "0");
                                //操作人
                                model.EXTENDCODE2 = row[7].ToString();
                                  var str1 = row[8];
                                  var dad = str1 ?? DateTime.Now.ToString("YY-MM-DD");
                                model.AZRQ = row[8].ToString()==""? DateTime.Now: DateTime.Parse(row[8].ToString());
                                model.GLDW = row[9].ToString();
                                model.BZ = row[10].ToString();
                                model.ADDTIME = DateTime.Now;

                                //判断设备编号是否已经添加
                                var isadd = GlobalHandler.deviceresp.FirstOrDefault(s => s.CCBH == model.CCBH);
                                if (isadd != null)
                                {
                                    logbuild.AppendLine($"出厂编号：{model.CCBH}已存在数据库");
                                    continue;
                                }
                                GlobalHandler.deviceresp.Insert(model);

                            }
                        }

                        if (Topic == "sensor")
                        {
                            foreach (DataRow row in datatable.Rows)
                            {
                                var sbbm = row[1].ToString();

                                var sm = GlobalHandler.deviceresp.FirstOrDefault(b => b.CCBH == sbbm);
                                if (sm == null) continue;

                                var model = new SensorRecord();
                                model.BZ = row[7].ToString();
                                model.CGQBM = sbbm + "_" + row[2].ToString();
                                model.CGQMC = row[3].ToString();
                                model.CGQLXMC = row[6].ToString();
                                model.CGQLXBM = row[5].ToString();
                                model.CGQXH = row[4].ToString();
                                model.Device = sm;
                                model.ADDTIME = DateTime.Now;
                                var isH = GlobalHandler.sensorresp.FirstOrDefault(b =>
                                    b.CGQBM == sbbm + "_" + row[2].ToString());
                                if (isH != null)
                                {
                                    logbuild.AppendLine($"传感器编号：{model.CGQBM}已存在数据库");
                                    continue;
                                }

                                GlobalHandler.sensorresp.Insert(model);
                            }
                        }

                        if (Topic == "monitor")
                        {
                            foreach (DataRow row in datatable.Rows)
                            {
                                if (string.IsNullOrEmpty(row[5].ToString()))
                                {
                                    XtraMessageBox.Show("监测点名称不能为空");
                                    this.DialogResult = DialogResult.OK;
                                    return;
                                }

                                var model = new BasicMonitorRecord();
                                string bh = string.Empty;
                                int num = 1;
                                if (row[4].ToString() == "")
                                {
                                    string nub = num.ToString().PadLeft(7, '0');
                                    bh = row[3].ToString() + row[1].ToString();
                                    var count = GlobalHandler.monitorresp.Count(s => s.BMID.StartsWith(bh));
                                    //var count = GlobalHandler.monitorresp.GetAllList().Where(s => s.BMMC == bh).ToList().Count;
                                    if (count > 0)
                                    {

                                        var str = (long.Parse(GlobalHandler.monitorresp.GetAllList(s => s.BMID.StartsWith(bh))
                                                                 .OrderByDescending(q => q.Id)
                                                                 .FirstOrDefault().BMID.ToString()
                                                                 ) + 1).ToString();
                                        bh = str;
                                    }
                                    else
                                    {
                                        bh = bh + nub;
                                    }
                                    model.BMID = bh;
                                }
                                else
                                {
                                    model.BMID = row[4].ToString();
                                }

                                model.BMMC = row[5].ToString();
                                model.BJBM = row[6].ToString();
                                model.Template = row[7].ToString();
                                model.TXFS = row[8].ToString();
                                model.BMMS = row[9].ToString();
                                model.BMX = decimal.Parse(row[10].ToString() == "" ? "0" : row[10].ToString());
                                model.BMY = decimal.Parse(row[11].ToString() == "" ? "0" : row[11].ToString());
                                model.IMGURL = row[12].ToString();

                                model.STATIONTYPE = row[1].ToString();

                                model.WebUrl = row[13].ToString();
                                model.MLEVEL = row[14].ToString();
                                model.ADDTIME = DateTime.Now;
                                var isH = GlobalHandler.monitorresp.FirstOrDefault(b =>
                                    b.BMID == model.BMID);
                                if (isH != null)
                                {
                                    logbuild.AppendLine($"监测点编号：{model.BMID}已存在数据库");
                                    continue;
                                }
                                GlobalHandler.monitorresp.Insert(model);

                            }
                        }
                        if (Topic == "config")
                        {

                            foreach (DataRow row in datatable.Rows)
                            {
                                // 生成编号
                                string bh = string.Empty;
                                if (row[2].ToString() == "")
                                {
                                    logbuild.AppendLine($"监测项编码不能为空");
                                    continue;
                                }
                                else
                                {
                                    decimal num = 1;
                                    bh = row[1].ToString()+ row[2].ToString().Substring(row[2].ToString().Length-4,4);
                                    var count = GlobalHandler.configresp.Count(s => s.CONFIG_CODE.StartsWith(bh));
                                    //var count = GlobalHandler.monitorresp.GetAllList().Where(s => s.BMMC == bh).ToList().Count;
                                    if (count > 0)
                                    {
                                        var str = (long.Parse(GlobalHandler.configresp.GetAllList(s => s.CONFIG_CODE.StartsWith(bh))
                                                                 .OrderByDescending(q => q.Id)
                                                                 .FirstOrDefault().CONFIG_CODE.Split('_')[2]
                                                                 ) + 1).ToString();
                                        bh = bh + "_" + str;
                                    }
                                    else
                                    {
                                        bh = bh + "_" + num.ToString();
                                    }
                                }
                                var model = new ConfigRecord();
                                //监测点
                                var bmrmodel = GlobalHandler.monitorresp.FirstOrDefault(b => b.BMID == row[1].ToString());
                                model.STATIONID = bmrmodel;
                                //检测项
                                var cgqbh = row[2].ToString();
                                var tnrmodel = GlobalHandler.tagresp.FirstOrDefault(b => b.TAG_KEY == cgqbh);
                                model.TAGID = tnrmodel;
                                //传感器
                                //if (row[9].ToString()=="")
                                //{
                                //    logbuild.AppendLine($"传感器编码不能为空");
                                //    continue;
                                //}
                                //else { 

                                var srda = GlobalHandler.sensorresp.FirstOrDefault(b => b.CGQBM == row[9].ToString());
                                if (srda == null)
                                {
                                    //如无匹配数据默认一条
                                    SensorRecord sr = new SensorRecord();
                                    DeviceRecord dr = new DeviceRecord();
                                    sr.Id = 1;
                                    dr.Id = 1;
                                    sr.Device = dr;
                                    model.SENSORID = sr;
                                }
                                else
                                {
                                    model.SENSORID = srda;
                                }

                                //}
                                model.CONFIG_CODE = bh;
                                model.CONFIG_DESC = row[3].ToString();
                                model.VARIABLE_NAME = row[4].ToString();
                                model.PAGE_X = decimal.Parse(row[5].ToString() == "" ? "0" : row[5].ToString());
                                model.PAGE_Y = double.Parse(row[6].ToString() == "" ? "0" : row[6].ToString());


                                model.UNITS = row[7].ToString();

                                model.PRECISION = double.Parse(row[8].ToString() == "" ? "0" : row[8].ToString());

                                model.NBSS = row[10].ToString();


                                model.ALERTRATE = double.Parse(row[11].ToString() == "" ? "0" : row[11].ToString());
                                model.ISPUSH = row[12].ToString() == "是" || row[12].ToString() == "1" ? "1" : "0";
                                model.ENABLE = row[13].ToString() == "是" || row[13].ToString() == "1" ? "1" : "0";

                                model.L1_START = double.Parse(row[14].ToString() == "" ? "0" : row[14].ToString());
                                model.L1_END = double.Parse(row[15].ToString() == "" ? "0" : row[15].ToString());
                                //model.L1_RETURNVALUE = double.Parse(row[15].ToString() == "" ? "0" : row[15].ToString());
                                model.L1_COLOR_VALUE = row[16].ToString();
                                model.L2_START = double.Parse(row[17].ToString() == "" ? "0" : row[17].ToString());
                                model.L2_END = double.Parse(row[18].ToString() == "" ? "0" : row[18].ToString());
                                //model.L2_RETURNVALUE = double.Parse(row[19].ToString() == "" ? "0" : row[19].ToString());
                                model.L2_COLOR_VALUE = row[19].ToString();
                                model.L3_START = double.Parse(row[20].ToString() == "" ? "0" : row[20].ToString());
                                model.L3_END = double.Parse(row[21].ToString() == "" ? "0" : row[21].ToString());
                                //model.L3_RETURNVALUE = double.Parse(row[23].ToString() == "" ? "0" : row[23].ToString());
                                model.L3_COLOR_VALUE = row[22].ToString();
                                //model.MIN_IDEALVALUE = double.Parse(row[25].ToString() == "" ? "0" : row[25].ToString());
                                //model.MAX_IDEALVALUE = double.Parse(row[26].ToString() == "" ? "0" : row[26].ToString());
                                model.ENABLEDOWN = row[23].ToString() == "是" || row[23].ToString() == "1" ? "1" : "0";
                                model.L1_STARTDOWN = double.Parse(row[24].ToString() == "" ? "0" : row[24].ToString());
                                model.L1_ENDDOWN = double.Parse(row[25].ToString() == "" ? "0" : row[25].ToString());
                                //model.L1_DOWNRETURNVALUE = double.Parse(row[28].ToString() == "" ? "0" : row[28].ToString());
                                model.L1_COLOR_VALUEDOWN = row[26].ToString();
                                model.L2_STARTDOWN = double.Parse(row[27].ToString() == "" ? "0" : row[27].ToString());
                                model.L2_ENDDOWN = double.Parse(row[28].ToString() == "" ? "0" : row[28].ToString());
                                //model.L2_DOWNRETURNVALUE = double.Parse(row[32].ToString() == "" ? "0" : row[32].ToString());
                                model.L2_COLOR_VALUEDOWN = row[29].ToString();
                                model.L3_STARTDOWN = double.Parse(row[30].ToString() == "" ? "0" : row[30].ToString());
                                model.L3_ENDDOWN = double.Parse(row[31].ToString() == "" ? "0" : row[31].ToString());
                                //model.L3_DOWNRETURNVALUE = double.Parse(row[36].ToString() == "" ? "0" : row[36].ToString());
                                model.L3_COLOR_VALUEDOWN = row[32].ToString();


                                model.MAX_VALUE = double.Parse(row[33].ToString() == "" ? "0" : row[33].ToString());
                                model.MIN_VALUE = double.Parse(row[34].ToString() == "" ? "0" : row[34].ToString());
                                model.MAX_MAX_VALUE = double.Parse(row[35].ToString() == "" ? "0" : row[35].ToString());
                                model.MIN_MIN_VALUE = double.Parse(row[36].ToString() == "" ? "0" : row[36].ToString());
                                model.ORDER_NUM = int.Parse(row[37].ToString() == "" ? "0" : row[37].ToString());
                                model.CGROUP = row[38].ToString();

                                model.COLOR_VALUE = row[39].ToString();
                                model.TEMPLATE = row[40].ToString();
                                model.REMARK = row[41].ToString();
                                model.ADDTIME = DateTime.Now;
                                if (row[42].ToString() == "私有权利" || row[42].ToString() == "0")
                                {
                                    model.PermissionType = "0";
                                }
                                else
                                {
                                    model.PermissionType = "1";
                                }

                                if (row[43].ToString() == "定制报表" || row[43].ToString() == "2")
                                {
                                    model.ReportType = "2";
                                }
                                else if (row[43].ToString() == "其他" || row[43].ToString() == "3")
                                {
                                    model.ReportType = "3";
                                }
                                else
                                {
                                    model.ReportType = "1";
                                }
                                //model.PermissionType = row[42].ToString();
                                //model.ReportType = row[43].ToString();
                                //判断设备编号是否已经添加

                                var isadd = GlobalHandler.configresp.FirstOrDefault(s => s.CONFIG_CODE == bh);
                                if (isadd != null)
                                {
                                    logbuild.AppendLine($"监测项编号：{bh}已存在数据库");
                                    continue;
                                }
                                GlobalHandler.configresp.Insert(model);

                            }
                        }

                    }
                    if (logbuild.Length < 1)
                    { 
                    XtraMessageBox.Show($"数据导入成功");
  
                    }
                    this.DialogResult = DialogResult.OK;
                }
                catch (Exception exception)
                {
                    XtraMessageBox.Show($""+Topic + "数据导入失败{exception}");
                    _log.Error(Topic + "数据导入失败，出错提示：" + exception.ToString());
                    this.DialogResult = DialogResult.OK;
                }
            });
            a.Start();
            
        }
    }
}