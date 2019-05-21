using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;
using Robin;
using SmartKylinApp.Common;
using SmartKylinApp.Module;
using SmartKylinApp.View.ComprehensiveDocument;
using SmartKylinData.BaseModel;
using SmartKylinData.Interface;
using SmartKylinData.IOTModel;
using Timer = System.Windows.Forms.Timer;

namespace SmartKylinApp
{
    public partial class Progressing : SplashScreen
    {
        public Progressing()
        {
            InitializeComponent();
            //var start = new Thread((aa) =>
            //{

            //});
            //start.Start();        
        }

        private void Time_Tick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            ((Timer)sender).Stop();
        }

        #region Overrides

        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion

        public enum SplashScreenCommand
        {
        }

        private void Progressing_Load(object sender, EventArgs e)
        {
            var boot = RobinBootstrapper.Create<BootstrapMoudle>();
            boot.Initialize();
            GlobalHandler.Bootstrapper = boot;
            var monitor = GlobalHandler.Bootstrapper.IocManager.Resolve<IBaseMonitorRepository>();
            var config = GlobalHandler.Bootstrapper.IocManager.Resolve<IConfig>();
            var msType = GlobalHandler.Bootstrapper.IocManager.Resolve<IMsType>();
            // GlobalHandler.monitors = monitor.GetAllList();
            // GlobalHandler.config = config.GetAllList();
            var type = msType.GetAllList();
            var bindlist = new System.ComponentModel.BindingList<TableModel>();
            //foreach (var item in GlobalHandler.monitors)
            //{
            //    foreach (var ite in GlobalHandler.config)
            //    {
            //        if (ite.STATIONID.BMID == item.BMID)
            //        {
            //            var model = new TableModel();
            //            model.ConfigCode = ite.CONFIG_CODE;
            //            model.ConfigName = ite.CONFIG_DESC;
            //            model.StationName = item.BMMC;
            //            model.ConfigDesc = item.BMMS;
            //            model.IndustryName = type.FirstOrDefault(a => a.TYPE_KEY == item.BMID.Substring(6, 6))
            //                ?.TYPE_NAME;
            //            model.SaveDate = ite.SAVE_DATE;
            //            model.State = "离线";

            //            bindlist.Add(model);

            //        }
            //    }
            //}

             GlobalHandler.resourList = bindlist;
            // this.Close();
            //this.DialogResult = DialogResult.OK;

            //初始化数据
            var time = new Timer();
            time.Interval = 1000;
            time.Tick += Time_Tick;
            time.Start();
        }
    }
}