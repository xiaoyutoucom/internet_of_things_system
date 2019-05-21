using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SmartKylinApp.View.Cache
{
    public partial class QueryForm : DevExpress.XtraEditors.XtraForm
    {
        /**
         *主要功能说明：
         * 1、更具缓存数据库中的键查询出元数据对应的信息
         * 2、更具传递
         *
         *
         */
        public QueryForm()
        {
            InitializeComponent();
        }

        private void QueryForm_Load(object sender, EventArgs e)
        {

        }
    }
}