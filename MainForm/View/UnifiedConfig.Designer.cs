namespace SmartKylinApp.View
{
    partial class UnifiedConfig
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UnifiedConfig));
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipItem toolTipItem2 = new DevExpress.Utils.ToolTipItem();
            this.tabbedView = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(this.components);
            this.navBarControl = new DevExpress.XtraNavBar.NavBarControl();
            this.employeesNavBarGroup = new DevExpress.XtraNavBar.NavBarGroup();
            this.nav_agreement = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItem2 = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItem3 = new DevExpress.XtraNavBar.NavBarItem();
            this.customersNavBarGroup = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarItem4 = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItem5 = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItem6 = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItem7 = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarGroup1 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarItem8 = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItem9 = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItem1 = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarGroup2 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarItem10 = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItem11 = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItem13 = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarGroup3 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarItem12 = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItem14 = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarGroup4 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarItem15 = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItem16 = new DevExpress.XtraNavBar.NavBarItem();
            this.navigationFrame = new DevExpress.XtraBars.Navigation.NavigationFrame();
            this.navigationPage1 = new DevExpress.XtraBars.Navigation.NavigationPage();
            this.configPanel = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.navigationFrame)).BeginInit();
            this.navigationFrame.SuspendLayout();
            this.navigationPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.configPanel)).BeginInit();
            this.SuspendLayout();
            // 
            // tabbedView
            // 
            this.tabbedView.RootContainer.Element = null;
            // 
            // navBarControl
            // 
            this.navBarControl.ActiveGroup = this.employeesNavBarGroup;
            this.navBarControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.navBarControl.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] {
            this.employeesNavBarGroup,
            this.customersNavBarGroup,
            this.navBarGroup1,
            this.navBarGroup2,
            this.navBarGroup3,
            this.navBarGroup4});
            this.navBarControl.Items.AddRange(new DevExpress.XtraNavBar.NavBarItem[] {
            this.nav_agreement,
            this.navBarItem2,
            this.navBarItem3,
            this.navBarItem4,
            this.navBarItem5,
            this.navBarItem6,
            this.navBarItem7,
            this.navBarItem8,
            this.navBarItem9,
            this.navBarItem10,
            this.navBarItem11,
            this.navBarItem12,
            this.navBarItem1,
            this.navBarItem13,
            this.navBarItem14,
            this.navBarItem15,
            this.navBarItem16});
            this.navBarControl.Location = new System.Drawing.Point(0, 0);
            this.navBarControl.Name = "navBarControl";
            this.navBarControl.OptionsNavPane.ExpandedWidth = 192;
            this.navBarControl.Size = new System.Drawing.Size(192, 646);
            this.navBarControl.TabIndex = 0;
            this.navBarControl.Text = "navBarControl";
            this.navBarControl.View = new DevExpress.XtraNavBar.ViewInfo.StandardSkinExplorerBarViewInfoRegistrator("Office 2016 Dark");
            this.navBarControl.ActiveGroupChanged += new DevExpress.XtraNavBar.NavBarGroupEventHandler(this.navBarControl_ActiveGroupChanged);
            // 
            // employeesNavBarGroup
            // 
            this.employeesNavBarGroup.Caption = "基础配置";
            this.employeesNavBarGroup.Expanded = true;
            this.employeesNavBarGroup.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.SmallIconsText;
            this.employeesNavBarGroup.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.nav_agreement),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItem2),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItem3)});
            this.employeesNavBarGroup.Name = "employeesNavBarGroup";
            // 
            // nav_agreement
            // 
            this.nav_agreement.Caption = "设备协议";
            this.nav_agreement.Name = "nav_agreement";
            this.nav_agreement.SmallImage = ((System.Drawing.Image)(resources.GetObject("nav_agreement.SmallImage")));
            this.nav_agreement.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.nav_agreement_LinkClicked);
            // 
            // navBarItem2
            // 
            this.navBarItem2.Caption = "设备信息";
            this.navBarItem2.Name = "navBarItem2";
            this.navBarItem2.SmallImage = ((System.Drawing.Image)(resources.GetObject("navBarItem2.SmallImage")));
            this.navBarItem2.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.navBarItem2_LinkClicked);
            this.navBarItem2.ItemChanged += new System.EventHandler(this.navBarItem2_ItemChanged);
            // 
            // navBarItem3
            // 
            this.navBarItem3.Caption = "传感器信息";
            this.navBarItem3.Name = "navBarItem3";
            this.navBarItem3.SmallImage = ((System.Drawing.Image)(resources.GetObject("navBarItem3.SmallImage")));
            this.navBarItem3.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.navBarItem3_LinkClicked);
            // 
            // customersNavBarGroup
            // 
            this.customersNavBarGroup.Caption = "统一配置";
            this.customersNavBarGroup.Expanded = true;
            this.customersNavBarGroup.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.SmallIconsText;
            this.customersNavBarGroup.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItem4),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItem5),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItem6),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItem7)});
            this.customersNavBarGroup.Name = "customersNavBarGroup";
            // 
            // navBarItem4
            // 
            this.navBarItem4.Caption = "监测点类型";
            this.navBarItem4.Name = "navBarItem4";
            this.navBarItem4.SmallImage = ((System.Drawing.Image)(resources.GetObject("navBarItem4.SmallImage")));
            this.navBarItem4.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.navBarItem4_LinkClicked);
            // 
            // navBarItem5
            // 
            this.navBarItem5.Caption = "监测项类型";
            this.navBarItem5.Name = "navBarItem5";
            this.navBarItem5.SmallImage = ((System.Drawing.Image)(resources.GetObject("navBarItem5.SmallImage")));
            this.navBarItem5.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.navBarItem5_LinkClicked);
            // 
            // navBarItem6
            // 
            this.navBarItem6.Caption = "监测点管理";
            this.navBarItem6.Name = "navBarItem6";
            this.navBarItem6.SmallImage = ((System.Drawing.Image)(resources.GetObject("navBarItem6.SmallImage")));
            this.navBarItem6.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.navBarItem6_LinkClicked);
            // 
            // navBarItem7
            // 
            this.navBarItem7.Caption = "监测项管理";
            this.navBarItem7.Name = "navBarItem7";
            this.navBarItem7.SmallImage = ((System.Drawing.Image)(resources.GetObject("navBarItem7.SmallImage")));
            this.navBarItem7.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.navBarItem7_LinkClicked);
            // 
            // navBarGroup1
            // 
            this.navBarGroup1.Caption = "短信配置";
            this.navBarGroup1.Expanded = true;
            this.navBarGroup1.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.SmallIconsText;
            this.navBarGroup1.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItem8),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItem9),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItem1)});
            this.navBarGroup1.Name = "navBarGroup1";
            this.navBarGroup1.Visible = false;
            // 
            // navBarItem8
            // 
            this.navBarItem8.Caption = "联系人管理";
            this.navBarItem8.Name = "navBarItem8";
            this.navBarItem8.SmallImage = ((System.Drawing.Image)(resources.GetObject("navBarItem8.SmallImage")));
            this.navBarItem8.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.navBarItem8_LinkClicked);
            // 
            // navBarItem9
            // 
            this.navBarItem9.Caption = "短信模板";
            this.navBarItem9.Name = "navBarItem9";
            this.navBarItem9.SmallImage = ((System.Drawing.Image)(resources.GetObject("navBarItem9.SmallImage")));
            this.navBarItem9.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.navBarItem9_LinkClicked);
            // 
            // navBarItem1
            // 
            this.navBarItem1.Caption = "关联监测点";
            this.navBarItem1.Name = "navBarItem1";
            this.navBarItem1.SmallImage = ((System.Drawing.Image)(resources.GetObject("navBarItem1.SmallImage")));
            toolTipItem1.Text = "关联需要发送短信的监测点信息";
            superToolTip1.Items.Add(toolTipItem1);
            this.navBarItem1.SuperTip = superToolTip1;
            this.navBarItem1.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.navBarItem1_LinkClicked);
            // 
            // navBarGroup2
            // 
            this.navBarGroup2.Caption = "LED配置";
            this.navBarGroup2.Expanded = true;
            this.navBarGroup2.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.SmallIconsText;
            this.navBarGroup2.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItem10),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItem11),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItem13)});
            this.navBarGroup2.Name = "navBarGroup2";
            // 
            // navBarItem10
            // 
            this.navBarItem10.Caption = "字库管理";
            this.navBarItem10.Name = "navBarItem10";
            this.navBarItem10.SmallImage = ((System.Drawing.Image)(resources.GetObject("navBarItem10.SmallImage")));
            this.navBarItem10.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.navBarItem10_LinkClicked);
            // 
            // navBarItem11
            // 
            this.navBarItem11.Caption = "LED信息";
            this.navBarItem11.Name = "navBarItem11";
            this.navBarItem11.SmallImage = ((System.Drawing.Image)(resources.GetObject("navBarItem11.SmallImage")));
            this.navBarItem11.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.navBarItem11_LinkClicked);
            // 
            // navBarItem13
            // 
            this.navBarItem13.Caption = "关联监测点";
            this.navBarItem13.LargeImage = ((System.Drawing.Image)(resources.GetObject("navBarItem13.LargeImage")));
            this.navBarItem13.Name = "navBarItem13";
            this.navBarItem13.SmallImage = ((System.Drawing.Image)(resources.GetObject("navBarItem13.SmallImage")));
            toolTipItem2.Text = "LED关联监测点";
            superToolTip2.Items.Add(toolTipItem2);
            this.navBarItem13.SuperTip = superToolTip2;
            this.navBarItem13.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.navBarItem13_LinkClicked);
            // 
            // navBarGroup3
            // 
            this.navBarGroup3.Caption = "视频配置";
            this.navBarGroup3.Expanded = true;
            this.navBarGroup3.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.SmallIconsText;
            this.navBarGroup3.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItem12),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItem14)});
            this.navBarGroup3.Name = "navBarGroup3";
            // 
            // navBarItem12
            // 
            this.navBarItem12.Caption = "视频信息";
            this.navBarItem12.Name = "navBarItem12";
            this.navBarItem12.SmallImage = ((System.Drawing.Image)(resources.GetObject("navBarItem12.SmallImage")));
            this.navBarItem12.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.navBarItem12_LinkClicked);
            // 
            // navBarItem14
            // 
            this.navBarItem14.Caption = "关联监测点";
            this.navBarItem14.LargeImage = ((System.Drawing.Image)(resources.GetObject("navBarItem14.LargeImage")));
            this.navBarItem14.Name = "navBarItem14";
            this.navBarItem14.SmallImage = ((System.Drawing.Image)(resources.GetObject("navBarItem14.SmallImage")));
            this.navBarItem14.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.navBarItem14_LinkClicked);
            // 
            // navBarGroup4
            // 
            this.navBarGroup4.Caption = "关联配置";
            this.navBarGroup4.Expanded = true;
            this.navBarGroup4.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItem15),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItem16)});
            this.navBarGroup4.Name = "navBarGroup4";
            // 
            // navBarItem15
            // 
            this.navBarItem15.Caption = "积水点关联";
            this.navBarItem15.Name = "navBarItem15";
            this.navBarItem15.SmallImage = ((System.Drawing.Image)(resources.GetObject("navBarItem15.SmallImage")));
            this.navBarItem15.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.navBarItem15_LinkClicked);
            // 
            // navBarItem16
            // 
            this.navBarItem16.Caption = "雨量站关联";
            this.navBarItem16.Name = "navBarItem16";
            this.navBarItem16.SmallImage = ((System.Drawing.Image)(resources.GetObject("navBarItem16.SmallImage")));
            this.navBarItem16.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.navBarItem16_LinkClicked);
            // 
            // navigationFrame
            // 
            this.navigationFrame.Appearance.BackColor = System.Drawing.Color.White;
            this.navigationFrame.Appearance.Options.UseBackColor = true;
            this.navigationFrame.Controls.Add(this.navigationPage1);
            this.navigationFrame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navigationFrame.Location = new System.Drawing.Point(192, 0);
            this.navigationFrame.Name = "navigationFrame";
            this.navigationFrame.Pages.AddRange(new DevExpress.XtraBars.Navigation.NavigationPageBase[] {
            this.navigationPage1});
            this.navigationFrame.RibbonAndBarsMergeStyle = DevExpress.XtraBars.Docking2010.Views.RibbonAndBarsMergeStyle.Always;
            this.navigationFrame.SelectedPage = this.navigationPage1;
            this.navigationFrame.Size = new System.Drawing.Size(741, 646);
            this.navigationFrame.TabIndex = 0;
            this.navigationFrame.Text = "navigationFrame";
            // 
            // navigationPage1
            // 
            this.navigationPage1.Caption = "navigationPage1";
            this.navigationPage1.Controls.Add(this.configPanel);
            this.navigationPage1.Name = "navigationPage1";
            this.navigationPage1.Size = new System.Drawing.Size(741, 646);
            // 
            // configPanel
            // 
            this.configPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.configPanel.Location = new System.Drawing.Point(0, 0);
            this.configPanel.Name = "configPanel";
            this.configPanel.Size = new System.Drawing.Size(741, 646);
            this.configPanel.TabIndex = 0;
            // 
            // UnifiedConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.navigationFrame);
            this.Controls.Add(this.navBarControl);
            this.Name = "UnifiedConfig";
            this.Size = new System.Drawing.Size(933, 646);
            this.Load += new System.EventHandler(this.UnifiedConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.navigationFrame)).EndInit();
            this.navigationFrame.ResumeLayout(false);
            this.navigationPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.configPanel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView tabbedView;
        private DevExpress.XtraNavBar.NavBarControl navBarControl;
        private DevExpress.XtraNavBar.NavBarGroup employeesNavBarGroup;
        private DevExpress.XtraNavBar.NavBarGroup customersNavBarGroup;
        private DevExpress.XtraNavBar.NavBarItem nav_agreement;
        private DevExpress.XtraNavBar.NavBarItem navBarItem2;
        private DevExpress.XtraNavBar.NavBarItem navBarItem3;
        private DevExpress.XtraNavBar.NavBarItem navBarItem4;
        private DevExpress.XtraNavBar.NavBarItem navBarItem5;
        private DevExpress.XtraNavBar.NavBarItem navBarItem6;
        private DevExpress.XtraNavBar.NavBarItem navBarItem7;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroup1;
        private DevExpress.XtraNavBar.NavBarItem navBarItem8;
        private DevExpress.XtraNavBar.NavBarItem navBarItem9;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroup2;
        private DevExpress.XtraNavBar.NavBarItem navBarItem10;
        private DevExpress.XtraNavBar.NavBarItem navBarItem11;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroup3;
        private DevExpress.XtraNavBar.NavBarItem navBarItem12;
        private DevExpress.XtraBars.Navigation.NavigationFrame navigationFrame;
        private DevExpress.XtraBars.Navigation.NavigationPage navigationPage1;
        private DevExpress.XtraEditors.PanelControl configPanel;
        private DevExpress.XtraNavBar.NavBarItem navBarItem1;
        private DevExpress.XtraNavBar.NavBarItem navBarItem13;
        private DevExpress.XtraNavBar.NavBarItem navBarItem14;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroup4;
        private DevExpress.XtraNavBar.NavBarItem navBarItem15;
        private DevExpress.XtraNavBar.NavBarItem navBarItem16;
    }
}
