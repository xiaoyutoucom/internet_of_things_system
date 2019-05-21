using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CefSharp;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;
using log4net;
using SmartKylinApp;
using SmartKylinApp.Common;

namespace MainForm
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            GlobalHandler.ApplicationStart();
            BonusSkins.Register();
            SkinManager.EnableFormSkins();
            log4net.Config.XmlConfigurator.Configure();
            Application.Run(new MainForm());
           
        }
    }
}
