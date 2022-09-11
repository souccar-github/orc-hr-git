using System;
using System.Drawing.Printing;
using System.Globalization;
using System.Reflection;
using System.Resources;
using DevExpress.Utils;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using Resources.Areas.JobDesc.Reports;

namespace Reporting.Infrastructure
{
    public partial class BaseReport : XtraReport
    {
        private ResourceManager resourceMan;


        public BaseReport()
        {
            InitializeComponent();
            BeforePrint += XtraReport1_BeforePrint;
        }

        public BaseReport(CultureInfo lang, string resourceName = "")
        {
            Culture = lang;
            resourceFileName = resourceName;
            Assembly assembly = typeof (JobDescriptionTemplate).Assembly;
            Type type = assembly.GetType(resourceFileName);
            resourceMan = new ResourceManager(type.FullName, assembly);

            InitializeComponent();
            BeforePrint += XtraReport1_BeforePrint;
        }

        protected CultureInfo Culture { get; set; }
        protected string resourceFileName { get; set; }

        protected void ReflectToCulture()
        {
            //if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name.StartsWith("ar"))
            //{
            //    ReflectLayoutToReport();
            //}
            if (Culture.ToString().StartsWith("ar"))
            {
                ReflectLayoutToReport();
            }
        }

        private void ReflectLayoutToReport()
        {
            for (int i = 0; i < Bands.Count; i++)
            {
                ReflectLayoutToControls(Bands[i].Controls);
            }
        }

        private void ReflectLayoutToControls(XRControlCollection controls)
        {
            for (int i = 0; i < controls.Count; i++)
            {
                float xPosition = (controls[i].Parent.Size.Width == 0
                                       ? controls[i].Parent.BoundsF.Size.Width
                                       : controls[i].Parent.Size.Width)
                                  - controls[i].LocationFloat.X - controls[i].Size.Width;

                controls[i].LocationFloat = new PointFloat(xPosition, controls[i].LocationFloat.Y);

                #region Text Alignment

                if (controls[i].Styles.Style == null)
                {
                    switch (controls[i].TextAlignment)
                    {
                        case TextAlignment.BottomLeft:
                            controls[i].TextAlignment = TextAlignment.BottomRight;
                            break;
                        case TextAlignment.BottomRight:
                            controls[i].TextAlignment = TextAlignment.BottomLeft;
                            break;
                        case TextAlignment.MiddleLeft:
                            controls[i].TextAlignment = TextAlignment.MiddleRight;
                            break;
                        case TextAlignment.TopLeft:
                            controls[i].TextAlignment = TextAlignment.TopRight;
                            break;
                    }
                }
                else
                {
                    switch (controls[i].Styles.Style.TextAlignment)
                    {
                        case TextAlignment.BottomLeft:
                            controls[i].Styles.Style.TextAlignment = TextAlignment.BottomRight;
                            break;
                        case TextAlignment.BottomRight:
                            controls[i].Styles.Style.TextAlignment = TextAlignment.BottomLeft;
                            break;
                        case TextAlignment.MiddleLeft:
                            controls[i].Styles.Style.TextAlignment = TextAlignment.MiddleRight;
                            break;
                        case TextAlignment.TopLeft:
                            controls[i].Styles.Style.TextAlignment = TextAlignment.TopRight;
                            break;
                    }
                }

                #endregion

                //var type = controls[i].ControlType;

                /*
                                if (controls[i] is XRLabel && controls[i].DataBindings.Count == 0)
                                {
                                    if (resourceMan.GetString(controls[i].Name.Substring(3), this.Culture) != null)
                                    {
                                        controls[i].Text = resourceMan.GetString(controls[i].Name.Substring(3), this.Culture);
                                    }

                   
                                }
                */

                if (controls[i].HasChildren)
                {
                    ReflectLayoutToControls(controls[i].Controls);
                }
            }
        }


        private void XtraReport1_BeforePrint(object sender, PrintEventArgs e)
        {
            ReflectToCulture();
        }
    }
}