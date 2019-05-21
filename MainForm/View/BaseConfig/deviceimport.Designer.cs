namespace SmartKylinApp.View.BaseConfig
{
    partial class deviceimport
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.rich_result = new System.Windows.Forms.RichTextBox();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.btn_mb = new DevExpress.XtraEditors.SimpleButton();
            this.btn_ck = new DevExpress.XtraEditors.SimpleButton();
            this.txt_filepath = new DevExpress.XtraEditors.TextEdit();
            this.btn_open = new DevExpress.XtraEditors.SimpleButton();
            this.btn_check = new DevExpress.XtraEditors.SimpleButton();
            this.btn_close = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem10 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_filepath.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.groupControl2);
            this.layoutControl1.Controls.Add(this.groupControl1);
            this.layoutControl1.Controls.Add(this.labelControl2);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(354, 380);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.rich_result);
            this.groupControl2.Location = new System.Drawing.Point(16, 140);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(322, 164);
            this.groupControl2.TabIndex = 14;
            this.groupControl2.Text = "数据校验结果";
            // 
            // rich_result
            // 
            this.rich_result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rich_result.Location = new System.Drawing.Point(2, 27);
            this.rich_result.Name = "rich_result";
            this.rich_result.Size = new System.Drawing.Size(318, 135);
            this.rich_result.TabIndex = 11;
            this.rich_result.Text = "";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.btn_mb);
            this.groupControl1.Controls.Add(this.btn_ck);
            this.groupControl1.Controls.Add(this.txt_filepath);
            this.groupControl1.Controls.Add(this.btn_open);
            this.groupControl1.Controls.Add(this.btn_check);
            this.groupControl1.Controls.Add(this.btn_close);
            this.groupControl1.Location = new System.Drawing.Point(16, 16);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(322, 118);
            this.groupControl1.TabIndex = 13;
            this.groupControl1.Text = "设备信息导入";
            // 
            // btn_mb
            // 
            this.btn_mb.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btn_mb.ImageUri.Uri = "Pie;Size16x16;Colored";
            this.btn_mb.Location = new System.Drawing.Point(27, 35);
            this.btn_mb.Name = "btn_mb";
            this.btn_mb.Size = new System.Drawing.Size(75, 22);
            this.btn_mb.TabIndex = 5;
            this.btn_mb.Text = "模板下载";
            this.btn_mb.ToolTip = "模板下载";
            this.btn_mb.Click += new System.EventHandler(this.btn_mb_Click);
            // 
            // btn_ck
            // 
            this.btn_ck.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btn_ck.ImageUri.Uri = "Preview;Size16x16";
            this.btn_ck.Location = new System.Drawing.Point(122, 35);
            this.btn_ck.Name = "btn_ck";
            this.btn_ck.Size = new System.Drawing.Size(75, 22);
            this.btn_ck.TabIndex = 12;
            this.btn_ck.Text = "参考文档";
            this.btn_ck.ToolTip = "参考文档";
            this.btn_ck.Click += new System.EventHandler(this.btn_ck_Click);
            // 
            // txt_filepath
            // 
            this.txt_filepath.Location = new System.Drawing.Point(7, 64);
            this.txt_filepath.Name = "txt_filepath";
            this.txt_filepath.Size = new System.Drawing.Size(305, 24);
            this.txt_filepath.TabIndex = 6;
            // 
            // btn_open
            // 
            this.btn_open.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btn_open.ImageUri.Uri = "Up;Size16x16";
            this.btn_open.Location = new System.Drawing.Point(216, 36);
            this.btn_open.Name = "btn_open";
            this.btn_open.Size = new System.Drawing.Size(75, 22);
            this.btn_open.TabIndex = 7;
            this.btn_open.Text = "打开文件";
            this.btn_open.ToolTip = "打开文件";
            this.btn_open.Click += new System.EventHandler(this.btn_open_Click);
            // 
            // btn_check
            // 
            this.btn_check.Location = new System.Drawing.Point(120, 90);
            this.btn_check.Name = "btn_check";
            this.btn_check.Size = new System.Drawing.Size(75, 22);
            this.btn_check.TabIndex = 8;
            this.btn_check.Text = "校验";
            this.btn_check.Click += new System.EventHandler(this.btn_check_Click);
            // 
            // btn_close
            // 
            this.btn_close.Location = new System.Drawing.Point(215, 90);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(75, 22);
            this.btn_close.TabIndex = 9;
            this.btn_close.Text = "导入";
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl2.Appearance.Options.UseForeColor = true;
            this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            this.labelControl2.Location = new System.Drawing.Point(16, 310);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(322, 54);
            this.labelControl2.StyleController = this.layoutControl1;
            this.labelControl2.TabIndex = 11;
            this.labelControl2.Text = "注意事项：批量导入时必须严格按照模板中的字段值进行填写，不得有重复数据，可下载参考文件。";
            this.labelControl2.UseMnemonic = false;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem10,
            this.layoutControlItem1,
            this.layoutControlItem2});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Size = new System.Drawing.Size(354, 380);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.Control = this.groupControl1;
            this.layoutControlItem10.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Size = new System.Drawing.Size(328, 124);
            this.layoutControlItem10.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem10.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.labelControl2;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 294);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(328, 60);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.groupControl2;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 124);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(328, 170);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // deviceimport
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.layoutControl1);
            this.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.Name = "deviceimport";
            this.Size = new System.Drawing.Size(354, 380);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txt_filepath.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.SimpleButton btn_ck;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton btn_close;
        private DevExpress.XtraEditors.SimpleButton btn_check;
        private DevExpress.XtraEditors.SimpleButton btn_open;
        private DevExpress.XtraEditors.TextEdit txt_filepath;
        private DevExpress.XtraEditors.SimpleButton btn_mb;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem10;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private System.Windows.Forms.RichTextBox rich_result;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
    }
}
