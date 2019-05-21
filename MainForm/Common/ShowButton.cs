using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartKylinApp.Common
{
     public class ShowButton
    {
        public static void Box_Showing(object sender, XtraMessageShowingArgs e)
        {
            foreach (var control in e.Form.Controls)
            {
                var button = control as SimpleButton;
                if (button != null)
                {
                    switch (button.DialogResult.ToString())
                    {
                        case "OK":
                            button.Text = "确定";
                            break;
                        case "Cancel":
                            button.Text = "取消";
                            break;
                    }
                }
            }
        }

    }
}
