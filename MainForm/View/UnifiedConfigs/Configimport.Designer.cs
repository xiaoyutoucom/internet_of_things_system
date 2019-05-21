namespace SmartKylinApp.View.BaseConfig
{
    partial class Configimport
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.rich_result = new System.Windows.Forms.RichTextBox();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.btn_mb = new DevExpress.XtraEditors.SimpleButton();
            this.btn_ck = new DevExpress.XtraEditors.SimpleButton();
            this.txt_filepath = new DevExpress.XtraEditors.TextEdit();
            this.btn_open = new DevExpress.XtraEditors.SimpleButton();
            this.btn_check = new DevExpress.XtraEditors.SimpleButton();
            this.btn_close = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_filepath.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.labelControl2);
            this.layoutControl1.Controls.Add(this.groupControl2);
            this.layoutControl1.Controls.Add(this.groupControl1);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(355, 513);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl2.Appearance.Options.UseForeColor = true;
            this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            this.labelControl2.Location = new System.Drawing.Point(16, 443);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(323, 54);
            this.labelControl2.StyleController = this.layoutControl1;
            this.labelControl2.TabIndex = 12;
            this.labelControl2.Text = "注意事项：批量导入时必须严格按照模板中的字段值进行填写，不得有重复数据，可下载参考文件。";
            this.labelControl2.UseMnemonic = false;
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.rich_result);
            this.groupControl2.Location = new System.Drawing.Point(16, 180);
            this.groupControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(323, 257);
            this.groupControl2.TabIndex = 15;
            this.groupControl2.Text = "数据校验结果";
            // 
            // rich_result
            // 
            this.rich_result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rich_result.Location = new System.Drawing.Point(2, 27);
            this.rich_result.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rich_result.Name = "rich_result";
            this.rich_result.Size = new System.Drawing.Size(319, 228);
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
            this.groupControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(323, 158);
            this.groupControl1.TabIndex = 14;
            this.groupControl1.Text = "监测项信息导入";
            // 
            // btn_mb
            // 
            this.btn_mb.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btn_mb.ImageUri.Uri = "Pie;Size16x16";
            this.btn_mb.Location = new System.Drawing.Point(53, 45);
            this.btn_mb.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_mb.Name = "btn_mb";
            this.btn_mb.Size = new System.Drawing.Size(89, 28);
            this.btn_mb.TabIndex = 5;
            this.btn_mb.Text = "模板下载";
            this.btn_mb.ToolTip = "模板下载";
            this.btn_mb.Click += new System.EventHandler(this.btn_mb_Click);
            // 
            // btn_ck
            // 
            this.btn_ck.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btn_ck.ImageUri.Uri = "Show;Size16x16";
            this.btn_ck.Location = new System.Drawing.Point(-13, 45);
            this.btn_ck.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_ck.Name = "btn_ck";
            this.btn_ck.Size = new System.Drawing.Size(55, 28);
            this.btn_ck.TabIndex = 12;
            this.btn_ck.Text = "simpleButton5";
            this.btn_ck.ToolTip = "参考文档";
            this.btn_ck.Visible = false;
            this.btn_ck.Click += new System.EventHandler(this.btn_ck_Click);
            // 
            // txt_filepath
            // 
            this.txt_filepath.Location = new System.Drawing.Point(10, 82);
            this.txt_filepath.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_filepath.Name = "txt_filepath";
            this.txt_filepath.Size = new System.Drawing.Size(308, 24);
            this.txt_filepath.TabIndex = 6;
            // 
            // btn_open
            // 
            this.btn_open.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btn_open.ImageUri.Uri = "Up;Size16x16";
            this.btn_open.Location = new System.Drawing.Point(186, 45);
            this.btn_open.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_open.Name = "btn_open";
            this.btn_open.Size = new System.Drawing.Size(87, 28);
            this.btn_open.TabIndex = 7;
            this.btn_open.Text = "打开文件";
            this.btn_open.ToolTip = "打开文件";
            this.btn_open.Click += new System.EventHandler(this.btn_open_Click);
            // 
            // btn_check
            // 
            this.btn_check.Enabled = false;
            this.btn_check.Location = new System.Drawing.Point(53, 114);
            this.btn_check.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_check.Name = "btn_check";
            this.btn_check.Size = new System.Drawing.Size(87, 28);
            this.btn_check.TabIndex = 8;
            this.btn_check.Text = "校验";
            this.btn_check.Click += new System.EventHandler(this.btn_check_Click);
            // 
            // btn_close
            // 
            this.btn_close.Enabled = false;
            this.btn_close.Location = new System.Drawing.Point(186, 114);
            this.btn_close.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(87, 28);
            this.btn_close.TabIndex = 9;
            this.btn_close.Text = "导入";
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(355, 513);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.groupControl1;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(329, 164);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.groupControl2;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 164);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(329, 263);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.labelControl2;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 427);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(329, 60);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // Configimport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Configimport";
            this.Size = new System.Drawing.Size(355, 513);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txt_filepath.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton btn_mb;
        private DevExpress.XtraEditors.SimpleButton btn_ck;
        private DevExpress.XtraEditors.TextEdit txt_filepath;
        private DevExpress.XtraEditors.SimpleButton btn_open;
        private DevExpress.XtraEditors.SimpleButton btn_check;
        private DevExpress.XtraEditors.SimpleButton btn_close;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private System.Windows.Forms.RichTextBox rich_result;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
    }
}
