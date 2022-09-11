//namespace Reporting.JobDesc
//{
//    partial class TestReporting
//    {
//        /// <summary>
//        /// Required designer variable.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        /// <summary> 
//        /// Clean up any resources being used.
//        /// </summary>
//        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        #region Designer generated code

//        /// <summary>
//        /// Required method for Designer support - do not modify
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestReporting));
//            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
//            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
//            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
//            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
//            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
//            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
//            this.xrRichText1 = new DevExpress.XtraReports.UI.XRRichText();
//            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
//            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
//            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
//            this.xrPageInfo3 = new DevExpress.XtraReports.UI.XRPageInfo();
//            this.ReportHeader1 = new DevExpress.XtraReports.UI.ReportHeaderBand();
//            this.PageHeader1 = new DevExpress.XtraReports.UI.PageHeaderBand();
//            this.ReportFooter1 = new DevExpress.XtraReports.UI.ReportFooterBand();
//            this.PageFooter1 = new DevExpress.XtraReports.UI.PageFooterBand();
//            this.xrSubreport1 = new DevExpress.XtraReports.UI.XRSubreport();
//            this.subReport1 = new Reporting.JobDesc.SubReport();
//            this.xrRichText2 = new DevExpress.XtraReports.UI.XRRichText();
//            ((System.ComponentModel.ISupportInitialize)(this.xrRichText1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.subReport1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.xrRichText2)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
//            // 
//            // Detail
//            // 
//            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
//            this.xrSubreport1});
//            this.Detail.Name = "Detail";
//            // 
//            // TopMargin
//            // 
//            this.TopMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
//            this.xrPageInfo2,
//            this.xrPageInfo1});
//            this.TopMargin.HeightF = 37.58332F;
//            this.TopMargin.Name = "TopMargin";
//            // 
//            // xrPageInfo2
//            // 
//            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(494.7917F, 0F);
//            this.xrPageInfo2.Name = "xrPageInfo2";
//            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
//            this.xrPageInfo2.PageInfo = DevExpress.XtraPrinting.PageInfo.UserName;
//            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(100F, 23F);
//            this.xrPageInfo2.StylePriority.UseTextAlignment = false;
//            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
//            // 
//            // xrPageInfo1
//            // 
//            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(56.25F, 0F);
//            this.xrPageInfo1.Name = "xrPageInfo1";
//            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
//            this.xrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
//            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(100F, 23F);
//            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
//            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
//            // 
//            // BottomMargin
//            // 
//            this.BottomMargin.HeightF = 25.95838F;
//            this.BottomMargin.Name = "BottomMargin";
//            // 
//            // ReportHeader
//            // 
//            this.ReportHeader.Borders = DevExpress.XtraPrinting.BorderSide.None;
//            this.ReportHeader.BorderWidth = 1;
//            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
//            this.xrRichText1});
//            this.ReportHeader.HeightF = 35.50002F;
//            this.ReportHeader.Name = "ReportHeader";
//            this.ReportHeader.StylePriority.UseBorders = false;
//            this.ReportHeader.StylePriority.UseBorderWidth = false;
//            // 
//            // xrRichText1
//            // 
//            this.xrRichText1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
//            this.xrRichText1.Name = "xrRichText1";
//            this.xrRichText1.SerializableRtfString = resources.GetString("xrRichText1.SerializableRtfString");
//            this.xrRichText1.SizeF = new System.Drawing.SizeF(649.9999F, 35.50002F);
//            // 
//            // PageHeader
//            // 
//            this.PageHeader.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
//            | DevExpress.XtraPrinting.BorderSide.Right) 
//            | DevExpress.XtraPrinting.BorderSide.Bottom)));
//            this.PageHeader.HeightF = 33.00001F;
//            this.PageHeader.Name = "PageHeader";
//            this.PageHeader.StylePriority.UseBorders = false;
//            // 
//            // ReportFooter
//            // 
//            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
//            this.xrRichText2});
//            this.ReportFooter.HeightF = 35.50002F;
//            this.ReportFooter.Name = "ReportFooter";
//            // 
//            // PageFooter
//            // 
//            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
//            this.xrPageInfo3});
//            this.PageFooter.HeightF = 23F;
//            this.PageFooter.Name = "PageFooter";
//            // 
//            // xrPageInfo3
//            // 
//            this.xrPageInfo3.LocationFloat = new DevExpress.Utils.PointFloat(275F, 0F);
//            this.xrPageInfo3.Name = "xrPageInfo3";
//            this.xrPageInfo3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
//            this.xrPageInfo3.SizeF = new System.Drawing.SizeF(100F, 23F);
//            this.xrPageInfo3.StylePriority.UseTextAlignment = false;
//            this.xrPageInfo3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
//            // 
//            // ReportHeader1
//            // 
//            this.ReportHeader1.Name = "ReportHeader1";
//            // 
//            // PageHeader1
//            // 
//            this.PageHeader1.Name = "PageHeader1";
//            // 
//            // ReportFooter1
//            // 
//            this.ReportFooter1.Name = "ReportFooter1";
//            // 
//            // PageFooter1
//            // 
//            this.PageFooter1.Name = "PageFooter1";
//            // 
//            // xrSubreport1
//            // 
//            this.xrSubreport1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
//            this.xrSubreport1.Name = "xrSubreport1";
//            this.xrSubreport1.ReportSource = this.subReport1;
//            this.xrSubreport1.SizeF = new System.Drawing.SizeF(650F, 100F);
//            // 
//            // xrRichText2
//            // 
//            this.xrRichText2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
//            this.xrRichText2.Name = "xrRichText2";
//            this.xrRichText2.SerializableRtfString = resources.GetString("xrRichText2.SerializableRtfString");
//            this.xrRichText2.SizeF = new System.Drawing.SizeF(649.9999F, 35.50002F);
//            // 
//            // TestReporting
//            // 
//            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
//            this.Detail,
//            this.TopMargin,
//            this.BottomMargin,
//            this.ReportHeader,
//            this.PageHeader,
//            this.ReportFooter,
//            this.PageFooter});
//            this.Margins = new System.Drawing.Printing.Margins(100, 100, 38, 26);
//            this.PageColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
//            this.Version = "11.2";
//            ((System.ComponentModel.ISupportInitialize)(this.xrRichText1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.subReport1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.xrRichText2)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

//        }

//        #endregion

//        private DevExpress.XtraReports.UI.DetailBand Detail;
//        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
//        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
//        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
//        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
//        private DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
//        private DevExpress.XtraReports.UI.PageFooterBand PageFooter;
//        private DevExpress.XtraReports.UI.XRSubreport xrSubreport1;
//        private JobDesc.SubReport subReport1;
//        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader1;
//        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader1;
//        private DevExpress.XtraReports.UI.ReportFooterBand ReportFooter1;
//        private DevExpress.XtraReports.UI.PageFooterBand PageFooter1;
//        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo2;
//        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo1;
//        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo3;
//        private DevExpress.XtraReports.UI.XRRichText xrRichText1;
//        private DevExpress.XtraReports.UI.XRRichText xrRichText2;
//    }
//}
