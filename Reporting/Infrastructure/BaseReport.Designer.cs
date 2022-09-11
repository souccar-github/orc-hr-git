namespace Reporting.Infrastructure
{
    partial class BaseReport
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

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.pageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.pageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.Data = new DevExpress.XtraReports.UI.XRControlStyle();
            this.SectionTitle = new DevExpress.XtraReports.UI.XRControlStyle();
            this.SectionHeaders = new DevExpress.XtraReports.UI.XRControlStyle();
            this.PageInfoUserName = new DevExpress.XtraReports.UI.XRControlStyle();
            this.PageInfoDate = new DevExpress.XtraReports.UI.XRControlStyle();
            this.PageInfoNumberOfTotal = new DevExpress.XtraReports.UI.XRControlStyle();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.HeightF = 25F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 10F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // TopMargin
            // 
            this.TopMargin.Name = "TopMargin";
            // 
            // BottomMargin
            // 
            this.BottomMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.pageInfo1,
            this.pageInfo2});
            this.BottomMargin.Name = "BottomMargin";

            // Data
            // 
            this.Data.BorderColor = System.Drawing.Color.Gainsboro;
            this.Data.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.Data.BorderWidth = 1;
            this.Data.Name = "Data";
            this.Data.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // SectionTitle
            // 
            this.SectionTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            this.SectionHeaders.BorderColor = System.Drawing.Color.Transparent;
            this.SectionTitle.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.SectionTitle.BorderWidth = 1F;
            this.SectionTitle.Font = new System.Drawing.Font("Arial", 11.25F);
            this.SectionTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.SectionTitle.Name = "SectionTitle";
            this.SectionTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 10F);
            this.SectionHeaders.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // SectionHeaders
            // 
            //this.SectionHeaders.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
            this.SectionHeaders.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.SectionHeaders.BorderColor = System.Drawing.Color.White;
            this.SectionHeaders.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.SectionHeaders.BorderWidth = 2F;
            this.SectionHeaders.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.SectionHeaders.ForeColor = System.Drawing.Color.White;
            this.SectionHeaders.Name = "SectionHeaders";
            this.SectionHeaders.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 10F);
            this.SectionHeaders.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            /// 
            // PageInfoUserName
            // 
            this.PageInfoUserName.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PageInfoUserName.Name = "PageInfoUserName";
            this.PageInfoUserName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // PageInfoDate
            // 
            this.PageInfoDate.Font = new System.Drawing.Font("Arial", 8.25F);
            this.PageInfoDate.Name = "PageInfoDate";
            this.PageInfoDate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
            // 
            // PageInfoNumberOfTotal
            // 
            this.PageInfoNumberOfTotal.Font = new System.Drawing.Font("Arial", 8.25F);
            this.PageInfoNumberOfTotal.Name = "PageInfoNumberOfTotal";
            this.PageInfoNumberOfTotal.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
            // pageInfo1
            // 
            this.pageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.pageInfo1.Name = "pageInfo1";
            this.pageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.pageInfo1.SizeF = new System.Drawing.SizeF(325F, 23F);
            this.pageInfo1.StyleName = "PageInfo";
            // 
            // pageInfo2
            // 
            this.pageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(325F, 0F);
            this.pageInfo2.Name = "pageInfo2";
            this.pageInfo2.SizeF = new System.Drawing.SizeF(325F, 23F);
            this.pageInfo2.StyleName = "PageInfo";
            this.pageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // BaseReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.Margins = new System.Drawing.Printing.Margins(100, 100, 50, 50);//(int left, int right, int top, int bottom)
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.Data,
            this.SectionTitle,
            this.SectionHeaders,
            this.PageInfoUserName,
            this.PageInfoDate,
            this.PageInfoNumberOfTotal});
            this.Version = "11.2";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.HeightF = 0F;
            this.Detail.Name = "Detail";
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 1;
            this.TopMargin.Name = "TopMargin";
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 23.79163F;
            this.BottomMargin.Name = "BottomMargin";
            // 
            // DynamicReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.Margins = new System.Drawing.Printing.Margins(100, 100, 50, 50);
            this.PageHeight = 1000;
            this.PageWidth = 827;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.Version = "11.2";
            this.Controls.SetChildIndex(this.BottomMargin, 0);
            this.Controls.SetChildIndex(this.TopMargin, 0);
            this.Controls.SetChildIndex(this.Detail, 0);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRPageInfo pageInfo1;
        private DevExpress.XtraReports.UI.XRPageInfo pageInfo2;
        private DevExpress.XtraReports.UI.XRControlStyle Data;
        private DevExpress.XtraReports.UI.XRControlStyle SectionTitle;
        private DevExpress.XtraReports.UI.XRControlStyle SectionHeaders;
        private DevExpress.XtraReports.UI.XRControlStyle PageInfoUserName;
        private DevExpress.XtraReports.UI.XRControlStyle PageInfoDate;
        private DevExpress.XtraReports.UI.XRControlStyle PageInfoNumberOfTotal;
    }
}
