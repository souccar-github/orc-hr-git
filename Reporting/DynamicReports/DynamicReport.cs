using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using DevExpress.Utils;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using Reporting.Infrastructure;
using Souccar.ReportGenerator.Domain.Classification;
using Souccar.ReportGenerator.Domain.QueryBuilder;
using StructureMap;
using Souccar.ReportGenerator.Services;
using System.Collections;
using Souccar.Domain.Security;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Personnel.RootEntities;
using System.Reflection;
using System.Dynamic;
using System.Data;
using Souccar.Infrastructure.Core;
using Souccar.Core.Extensions;

namespace Reporting.DynamicReports
{
    public partial class DynamicReport : XtraReport
    {
        private const int FieldWidth = 200;
        private const int ReportHeight = 1169;
        private const int SectionWhiteSpacePreceding = 50;
        private const int MinReportWidth = 827;
        private const int MaxFieldsCount = 5;

        public DynamicReport(CultureInfo cultureInfo, Report report, User user, NHibernate.ISession session)
        {
            if (report != null)
            {
                InitializeComponent();
                QueryTree = report.QueryTreesList.FirstOrDefault();
                ReportTitle = GetResource(report.ReportResourceName);
                SetReportSettings();
                LoadDataSource(session);
                GenerateMasterDetailsReport(QueryTree);
                ApplyReportTemplateSettings(report.Template, user);

            }
        }

        private QueryTree QueryTree { get; set; }
        private string ReportTitle { get; set; }
        private object ReportDataSource { get; set; }

        private ReportHeaderBand GenerateReportHeaderBand(ReportTemplate reportTemplate, User user)
        {
            if (reportTemplate.ShowHeader)
            {
                var reportHeaderBand = new ReportHeaderBand { HeightF = 0F };
                var fullName = typeof(ReportTemplate).FullName;
                var path = System.Web.HttpContext.Current.Server.MapPath("~/Content/UploadedFiles/" + fullName + "/RtfReportHeader/" + reportTemplate.RtfReportHeader);
                //var fullPath = $"{ path }\\{ fullName }\\RtfReportHeader\\{ reportTemplate.RtfReportHeader }";
                if (!string.IsNullOrEmpty(reportTemplate.RtfReportHeader) && File.Exists(path))
                {
                    var richText = new XRRichText { WidthF = BoundsF.Size.Width };

                    richText.LoadFile(path, XRRichTextStreamType.RtfText);
                    reportHeaderBand.Controls.Add(richText);
                }
                else
                {
                    var titleXRLabel = new XRLabel
                    {
                        LocationFloat = new PointFloat(0F, 0F),
                        SizeF = new SizeF(BoundsF.Size.Width, 40F),
                        //StyleName = "PageInfoUserName",
                        TextAlignment = TextAlignment.MiddleCenter,
                        Font = new System.Drawing.Font("Arial", 16F),
                        ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75))))),
                        Text = ReportTitle,
                    };

                    reportHeaderBand.Controls.Add(titleXRLabel);
                }


                return reportHeaderBand;
            }
            return null;
        }

        private PageFooterBand GenerateReportFooterBand(ReportTemplate reportTemplate, User user)
        {
            if (reportTemplate.ShowFooter)
            {
                var reportFooterBand = new PageFooterBand
                {
                    HeightF = 0F,
                    //PrintAtBottom = true
                };
                var fullName = typeof(ReportTemplate).FullName;
                var path = System.Web.HttpContext.Current.Server.MapPath("~/Content/UploadedFiles/" + fullName + "/RtfReportFooter/" + reportTemplate.RtfReportFooter);
                //var fullPath = $"{ path }\\{ fullName }\\RtfReportHeader\\{ reportTemplate.RtfReportFooter }";
                if (!string.IsNullOrEmpty(reportTemplate.RtfReportFooter) && File.Exists(path))
                {
                    var richText = new XRRichText { WidthF = BoundsF.Size.Width };
                    richText.LoadFile(path, XRRichTextStreamType.RtfText);
                    reportFooterBand.Controls.Add(richText);
                }

                const int numberOfControls = 3;

                float controlWidth = BoundsF.Size.Width / numberOfControls;
                if (reportTemplate.ShowUserName)
                {
                    var usernameXRLabel = new XRLabel
                    {
                        LocationFloat = new PointFloat(0F, 0F),
                        SizeF = new SizeF(controlWidth, 25F),
                        //StyleName = "PageInfoUserName",
                        TextAlignment = TextAlignment.BottomLeft,
                        Text = user.FullName,
                    };

                    reportFooterBand.Controls.Add(usernameXRLabel);
                }

                if (reportTemplate.ShowPageNumber)
                {
                    var pageInfo = new XRPageInfo
                    {
                        LocationFloat = new PointFloat(controlWidth, 0F),
                        PageInfo = PageInfo.NumberOfTotal,
                        SizeF = new SizeF(controlWidth, 25F),
                        Name = "PageInfoPageNumber",
                        StyleName = "PageInfoNumberOfTotal"
                    };
                    reportFooterBand.Controls.Add(pageInfo);
                }
                if (reportTemplate.ShowDateTime)
                {
                    var pageInfo = new XRPageInfo
                    {
                        LocationFloat = new PointFloat(controlWidth + controlWidth, 0F),
                        PageInfo = PageInfo.DateTime,
                        SizeF = new SizeF(controlWidth, 25F),
                        Name = "PageInfoDateTime",
                        StyleName = "PageInfoDate"
                    };
                    reportFooterBand.Controls.Add(pageInfo);
                }
                return reportFooterBand;
            }
            return null;
        }

        private void ApplyReportTemplateSettings(ReportTemplate reportTemplate, User user)
        {
            ReportHeaderBand reportHeaderBand = GenerateReportHeaderBand(reportTemplate, user);
            if (reportHeaderBand != null)
            {
                Bands.Add(reportHeaderBand);
            }

            PageFooterBand reportFooterBand = GenerateReportFooterBand(reportTemplate, user);
            if (reportFooterBand != null)
            {
                Bands.Add(reportFooterBand);
            }

            //BottomMargin.Controls.AddRange(GenerateReportMarginInfo(reportTemplate));
        }

        private XRControl[] GenerateReportMarginInfo(ReportTemplate reportTemplate)
        {
            var controls = new List<XRControl>();
            const int numberOfControls = 3;

            float controlWidth = BoundsF.Size.Width / numberOfControls;
            if (reportTemplate.ShowDateTime)
            {
                var pageInfo = new XRPageInfo
                {
                    LocationFloat = new PointFloat(0F, 0F),
                    PageInfo = PageInfo.DateTime,
                    SizeF = new SizeF(controlWidth, 25F),
                    Name = "PageInfoDateTime",
                    StyleName = "PageInfoDate"
                };
                controls.Add(pageInfo);
            }
            if (reportTemplate.ShowPageNumber)
            {
                var pageInfo = new XRPageInfo
                {
                    LocationFloat = new PointFloat(controlWidth, 0F),
                    PageInfo = PageInfo.NumberOfTotal,
                    SizeF = new SizeF(controlWidth, 25F),
                    Name = "PageInfoPageNumber",
                    StyleName = "PageInfoNumberOfTotal"
                };
                controls.Add(pageInfo);
            }
            if (reportTemplate.ShowUserName)
            {
                var pageInfo = new XRPageInfo
                {
                    LocationFloat = new PointFloat(2 * controlWidth, 0F),
                    PageInfo = PageInfo.UserName,
                    SizeF = new SizeF(controlWidth, 25F),
                    Name = "PageInfoUserName",
                    StyleName = "PageInfoUserName"
                };
                controls.Add(pageInfo);
            }
            return controls.ToArray();
        }

        private void LoadDataSource(NHibernate.ISession session)
        {
            var parser = new NHibernateQueryTreeParser(session);
            var service = new NHibernateQueryTreeService(parser);
            object dataSource = service.ExecuteQuery(QueryTree);
            //DataSource = dataSource;

            var listDic = new List<Dictionary<string, object>>();

            //DataTable table = new DataTable(QueryTree.PropertyName);
            CreateDataSource2(listDic, dataSource, QueryTree);
            var list = new List<dynamic>();
            foreach (var dic in listDic)
            {
                var dynamicObject = GetDynamicObject(dic);
                list.Add(dynamicObject);
            }

            ReportDataSource = dataSource;

        }

        private void CreateDataSource(DataTable table, object data, QueryTree queryTree)
        {
            table.Columns.AddRange(GetDataTableColumns(queryTree));
            var list = data as IList;
            foreach (var item in list)
            {
                var array = new List<dynamic>();
                foreach (var queryLeaf in queryTree.Leaves.Where(x => x.IsSelected))
                {
                    var propName = queryLeaf.PropertyFullPath.Substring(queryLeaf.PropertyFullPath.IndexOf('.') + 1);
                    if (queryLeaf.IsReference)
                    {
                        array.Add(GetValueFromReference(item, queryLeaf));
                    }
                    else
                    {
                        array.Add(GetPropValue(item, queryLeaf.PropertyShortName));
                    }
                }

                //References
                foreach (QueryTree supQueryTree in queryTree.Nodes.Where(x => x.HasSelectedFields).OrderBy(x => x.SelectOrder))
                {
                    if (queryTree.Type.GetProperty(supQueryTree.DisplayName)
                       .PropertyType
                       .GetInterface("IEnumerable") == null)
                    {
                        foreach (var queryLeaf in supQueryTree.Leaves.Where(x => x.IsSelected))
                        {

                            var propName = queryLeaf.PropertyFullPath.Substring(queryLeaf.PropertyFullPath.IndexOf('.') + 1);
                            if (propName.Split('.').Length == 2)
                            {
                                array.Add(GetValueFromReference(item, queryLeaf));
                            }
                            else
                            {
                                array.Add(GetPropValue(supQueryTree, propName));
                            }
                        }
                    }
                }

                //List
                foreach (QueryTree supQueryTree in queryTree.Nodes.Where(x => x.HasSelectedFields).OrderBy(x => x.SelectOrder))
                {
                    if (queryTree.Type.GetProperty(supQueryTree.DisplayName)
                       .PropertyType
                       .GetInterface("IEnumerable") != null)
                    {
                        var supDataSource = GetPropValue(item, supQueryTree.DisplayName);
                        var dataTable = new DataTable(supQueryTree.DisplayName);
                        CreateDataSource(dataTable, supDataSource, supQueryTree);
                        array.Add(dataTable);
                    }
                }

                table.Rows.Add(array.ToArray());
            }
        }

        private DataColumn[] GetDataTableColumns(QueryTree queryTree)
        {
            var columns = new List<DataColumn>();
            columns.AddRange(queryTree.Leaves.Where(leaf => leaf.IsSelected).OrderBy(x => x.Selected)
                .Select(x => new DataColumn(x.PropertyName)).ToList());

            //Reference
            foreach (QueryTree item in queryTree.Nodes.Where(x => x.HasSelectedFields).OrderBy(x => x.SelectOrder))
            {
                if (queryTree.Type.GetProperty(item.DisplayName)
                       .PropertyType
                       .GetInterface("IEnumerable") == null)
                {
                    columns.AddRange(item.Leaves.Where(leaf => leaf.IsSelected).OrderBy(x => x.Selected)
                    .Select(x => new DataColumn(item.DisplayName + "." + x.PropertyName)).ToList());

                }
            }

            //List
            foreach (QueryTree supQueryTree in queryTree.Nodes.Where(x => x.HasSelectedFields).OrderBy(x => x.SelectOrder))
            {
                if (queryTree.Type.GetProperty(supQueryTree.DisplayName)
                   .PropertyType
                   .GetInterface("IEnumerable") != null)
                {
                    columns.Add(new DataColumn(supQueryTree.DisplayName));
                }
            }

            return columns.ToArray();
        }

        private object GetPropValue(object data, string propName)
        {
            var propInfo = data.GetType().GetProperty(propName);
            return propInfo.GetValue(data, null);
        }
        private object GetValueFromReference(object data, QueryLeaf queryLeaf)
        {
            var refName = queryLeaf.PropertyFullPath.Split('.')[queryLeaf.PropertyFullPath.Split('.').Length - 2];

            var propRefInfo = data.GetType().GetProperty(refName);
            var refValue = propRefInfo.GetValue(data, null);

            return GetPropValue(refValue, queryLeaf.PropertyShortName);
        }

        private void CreateDataSource2(IList<Dictionary<string, object>> listDic, object data, QueryTree queryTree)
        {
            var list = data as IList;
            foreach (var item in list)
            {
                var dic = new Dictionary<string, object>();
                foreach (var queryLeaf in queryTree.Leaves.Where(x => x.IsSelected))
                {
                    var propName = queryLeaf.PropertyFullPath.Substring(queryLeaf.PropertyFullPath.IndexOf('.') + 1);
                    if (propName.Split('.').Length == 2)
                    {
                        dic.Add(propName, GetValueFromReference(item, queryLeaf));
                    }
                    else
                    {
                        dic.Add(propName, GetPropValue(item, propName));
                    }
                }

                listDic.Add(dic);
            }
        }

        public dynamic GetDynamicObject(Dictionary<string, object> properties)
        {
            return new ReportDynamicObject(properties);
        }

        private void SetReportSettings()
        {
            PaperKind = PaperKind.Custom;
            PageHeight = ReportHeight;
            PageWidth = GetReportWidth();
            Landscape = CheckSelectedFiledsCount();
        }

        private int GetReportWidth()
        {
            return Math.Max(GetMaxSelectedFields(QueryTree) * FieldWidth, MinReportWidth);
        }
        private bool CheckSelectedFiledsCount()
        {
            return GetMaxSelectedFields(QueryTree) > MaxFieldsCount;
        }
        /// <summary>
        /// zero Based Level
        /// </summary>
        private int GetSectionLevel(QueryTree queryTree)
        {
            return queryTree.FullClassPath.Split('.').Count() - 1;
        }

        private int GetMaxSelectedFields(QueryTree queryTree)
        {
            if (queryTree.Nodes.Count == 0)
                return queryTree.Leaves.Count(leaf => leaf.IsSelected);
            return Math.Max(queryTree.Leaves.Count(leaf => leaf.IsSelected),
                            queryTree.Nodes.Max(node => GetMaxSelectedFields(node)));
        }

        private void GenerateMasterDetailsReport(QueryTree queryTree)
        {
            DetailReportBand detailReportBand = GenerateDetailReportBand(queryTree);
            Bands.Add(detailReportBand);
            GenerateDetailBand(queryTree, detailReportBand.Bands);
        }

        private void GenerateDetailBand(QueryTree queryTree, BandCollection bandCollection)
        {
            foreach (QueryTree item in queryTree.Nodes.Where(x => x.HasSelectedFields).OrderBy(x => x.SelectOrder))
            {

                if (queryTree.Type.GetProperty(item.DisplayName)
                   .PropertyType
                   .GetInterface("IEnumerable") != null)
                {
                    DetailReportBand detailReportBand = GenerateDetailReportBand(item);
                    bandCollection.Add(detailReportBand);
                    GenerateDetailBand(item, detailReportBand.Bands);
                }
            }
        }

        private DetailReportBand GenerateDetailReportBand(QueryTree queryTree)
        {
            int level = GetSectionLevel(queryTree);
            int counter = 0;
            bool hasDetails = queryTree.Nodes.Any(x => x.HasSelectedFields && queryTree.Type.GetProperty(x.DisplayName).PropertyType.GetInterface("IEnumerable") != null);
            int whiteSpacePreceding = level * SectionWhiteSpacePreceding;
            float filedWidth = (BoundsF.Size.Width - whiteSpacePreceding) /
                               (queryTree.Leaves.Count(leaf => leaf.IsSelected) +
                               queryTree.Nodes.Where(x => x.HasSelectedFields && queryTree.Type.GetProperty(x.DisplayName).PropertyType.GetInterface("IEnumerable") == null).Sum(y => y.Leaves.Count(z => z.IsSelected)) +
                               queryTree.AggregateOperations.Count);
            float sectionWidth = BoundsF.Size.Width - whiteSpacePreceding;
            float controlsStartPoint = BoundsF.Size.Width - sectionWidth;
            var detailBand = new DetailBand
            {
                HeightF = 0,
                KeepTogether = true,
            };

            var groupHeaderBand = new GroupHeaderBand
            {
                HeightF = 0,
                KeepTogether = true,
            };
            var detailReportBand = new DetailReportBand
            {
                KeepTogether = true,
                DataMember =
                    level == 0
                        ? ""
                        : queryTree.FullClassPath.Substring(
                            queryTree.FullClassPath.IndexOf('.') + 1),
                DataSource = ReportDataSource

            };

            //detailReportBand.ReportPrintOptions.PrintOnEmptyDataSource = false;
            var lblsectionLabel = new XRLabel
            {
                LocationFloat = new PointFloat(controlsStartPoint, 0F),
                SizeF = level == 0 ? new SizeF(0F, 0F) : new SizeF(sectionWidth, 25F),
                Text = level == 0 ? string.Empty : queryTree.DisplayName,
                StyleName = "SectionTitle",
                //TextAlignment= TextAlignment.MiddleCenter
            };
            //var lblsectionLabel = new XRLabel
            //{
            //    LocationFloat = new PointFloat(controlsStartPoint, 0F),
            //    SizeF = new SizeF(sectionWidth, 0F),
            //    //Text = queryTree.DisplayName,
            //    StyleName = "SectionTitle"
            //};


            foreach (QueryLeaf item in queryTree.Leaves.Where(leaf => leaf.IsSelected).OrderBy(x => x.Selected))
            {
                var lblHeaderLabel = new XRLabel
                {
                    LocationFloat =
                        new PointFloat((counter * filedWidth) + whiteSpacePreceding,
                                       lblsectionLabel.HeightF),
                    SizeF = new SizeF(filedWidth, 25F),
                    Text = item.DisplayName,
                    StyleName = "SectionHeaders"
                };

                var lblDetailLabel = new XRLabel
                {
                    LocationFloat = new PointFloat((counter * filedWidth) + whiteSpacePreceding,
                                                   hasDetails
                                                       ? (lblHeaderLabel.HeightF +
                                                          lblsectionLabel.HeightF)
                                                       : 0),
                    SizeF = new SizeF(filedWidth, 25F),
                    StyleName = "Data"
                };

                //if(item.PropertyType == typeof(bool))
                //{
                //    lblDetailLabel.Font = new System.Drawing.Font("Wingdings", 12F);
                //}

                lblDetailLabel.DataBindings.Add(new XRBinding("Text", null, 
                                                              item.PropertyFullPath.Substring(
                                                                  item.PropertyFullPath.IndexOf('.') + 1)));

                if (!hasDetails)
                {
                    groupHeaderBand.Controls.Add(lblsectionLabel);
                    groupHeaderBand.Controls.Add(lblHeaderLabel);
                }
                else
                {
                    detailBand.Controls.Add(lblsectionLabel);
                    detailBand.Controls.Add(lblHeaderLabel);
                }
                detailBand.Controls.Add(lblDetailLabel);

                counter++;
            }
            foreach (QueryTree item in queryTree.Nodes.Where(x => x.HasSelectedFields).OrderBy(x => x.SelectOrder))
            {
                if (queryTree.Type.GetProperty(item.DisplayName)
                       .PropertyType
                       .GetInterface("IEnumerable") == null)
                {


                    foreach (QueryLeaf queryleafItem in item.Leaves.Where(x => x.IsSelected).OrderBy(x => x.Selected))
                    {
                        var lblHeaderLabel = new XRLabel
                        {
                            LocationFloat =
                                new PointFloat((counter * filedWidth) + whiteSpacePreceding,
                                               lblsectionLabel.HeightF),
                            SizeF = new SizeF(filedWidth, 25F),
                            Text = $"{queryTree.DisplayName} -> {queryleafItem.DisplayName}",
                            StyleName = "SectionHeaders"
                        };
                        //var lblHeaderLabel = new XRLabel
                        //{
                        //    LocationFloat =
                        //        new PointFloat((counter * filedWidth) + whiteSpacePreceding,
                        //                       lblsectionLabel.HeightF),
                        //    SizeF = new SizeF(filedWidth, 0F),
                        //    //Text = queryleafItem.DisplayName,
                        //    StyleName = "SectionHeaders"
                        //};

                        var lblDetailLabel = new XRLabel
                        {
                            LocationFloat = new PointFloat((counter * filedWidth) + whiteSpacePreceding,
                                                           hasDetails
                                                               ? (lblHeaderLabel.HeightF +
                                                                  lblsectionLabel.HeightF)
                                                               : 0),
                            SizeF = new SizeF(filedWidth, 25F),
                            StyleName = "Data"
                        };
                        lblDetailLabel.DataBindings.Add(new XRBinding("Text", null,
                                                                      item.DisplayName + "." + queryleafItem.PropertyName));
                        if (!hasDetails)
                        {
                            groupHeaderBand.Controls.Add(lblsectionLabel);
                            groupHeaderBand.Controls.Add(lblHeaderLabel);
                        }
                        else
                        {
                            detailBand.Controls.Add(lblsectionLabel);
                            detailBand.Controls.Add(lblHeaderLabel);
                        }
                        detailBand.Controls.Add(lblDetailLabel);
                        counter++;
                    }
                }
            }
            if (queryTree.HasAggregateOperations)
            {
                foreach (var operation in queryTree.AggregateOperations)
                {
                    var lblHeaderLabel = new XRLabel
                    {
                        LocationFloat =
                                new PointFloat((counter * filedWidth) + whiteSpacePreceding,
                                               lblsectionLabel.HeightF),
                        SizeF = new SizeF(filedWidth, 25F),
                        Text = operation.DisplayName,
                        StyleName = "SectionHeaders"
                    };

                    var lblDetailLabel = new XRLabel
                    {
                        LocationFloat = new PointFloat((counter * filedWidth) + whiteSpacePreceding,
                                                       hasDetails
                                                           ? (lblHeaderLabel.HeightF +
                                                              lblsectionLabel.HeightF)
                                                           : 0),
                        SizeF = new SizeF(filedWidth, 25F),
                        StyleName = "Data"
                    };
                    var expretion = operation.PropertyName + "." + operation.AggregateFunction.ToString() + "(x=>x." + operation.SubPropertyName + ")";
                    lblDetailLabel.DataBindings.Add(new XRBinding("Text", null, expretion));
                    if (!hasDetails)
                    {
                        groupHeaderBand.Controls.Add(lblsectionLabel);
                        groupHeaderBand.Controls.Add(lblHeaderLabel);
                    }
                    else
                    {
                        detailBand.Controls.Add(lblsectionLabel);
                        detailBand.Controls.Add(lblHeaderLabel);
                    }
                    detailBand.Controls.Add(lblDetailLabel);
                    counter++;
                }
            }
            detailReportBand.Bands.Add(detailBand);
            if (!hasDetails)
            {
                detailReportBand.Bands.Add(groupHeaderBand);
            }

            return detailReportBand;
        }

        private void DynamicReport_DataSourceDemanded(object sender, EventArgs e)
        {
            //DataSource = ReportDataSource;
        }

        public string GetResource(string key)
        {
            if (string.IsNullOrEmpty(key))
                return string.Empty;

            var result = ServiceFactory.LocalizationService.GetResource(key);
            return string.IsNullOrEmpty(result) ? key.ToCapitalLetters() : result;
        }
    }
}