using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FluentNHibernate.Testing.Values;
using SmartKylinApp.Common;

namespace SmartKylinApp.View.ComprehensiveDocument
{
    public partial class configTable : UserControl
    {
        public configTable()
        {
            InitializeComponent();

            gridControl1.DataSource = GlobalHandler.resourList;
            if (!mvvmContext1.IsDesignMode)
                InitializeBindings();
        }

        void InitializeBindings()
        {
            var fluent = mvvmContext1.OfType<configTableViewModel>();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
           // mvvmContext1.
        }
    }

    class aaa
    {
        public string name { get; set; }
        public string old { get; set; }
        public string code { get; set; }
    }
}
