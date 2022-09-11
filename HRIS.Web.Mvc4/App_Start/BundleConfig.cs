using System.Web.Optimization;

namespace Project.Web.Mvc4.App_Start
{
    public class BundleConfig
    {
        //After Apply Master Detail Feature
        // public const string KendoVersion = "2013.2.716";
        public const string KendoVersion = "2014.1.528";//
        public const string highChartsVersion = "7.1.1";//
        // public const string KendoVersion = "2014.2.716";



        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/require").Include(
                "~/Scripts/require.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/underscore").Include(
                "~/Scripts/underscore.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/mainTheme").Include(
                //"~/Scripts/mainTheme/fg.menu.js",
                //"~/Scripts/mainTheme/chosen.jquery.js",
                //"~/Scripts/mainTheme/uniform.jquery.js",
                "~/Scripts/mainTheme/bootstrap-dropdown.js",
                //"~/Scripts/mainTheme/bootstrap-colorpicker.js",
                //"~/Scripts/mainTheme/sticky.full.js",
                //"~/Scripts/mainTheme/fg.menu.js",
                //"~/Scripts/mainTheme/jquery.tagsinput.js",
                //"~/Scripts/mainTheme/jquery.peity.js",
                //"~/Scripts/mainTheme/jquery.simplemodal.js",
                //"~/Scripts/mainTheme/jquery.jBreadCrumb.1.1.js",
                //"~/Scripts/mainTheme/jquery.colorbox-min.js",
                //"~/Scripts/mainTheme/jquery.idTabs.min.j",
                //"~/Scripts/mainTheme/jquery.multiFieldExtender.min.js",
                //"~/Scripts/mainTheme/jquery.confirm.js",
                //"~/Scripts/mainTheme/elfinder.min.js",
                "~/Scripts/mainTheme/accordion.jquery.js",
                //"~/Scripts/mainTheme/autogrow.jquery.js",
                //"~/Scripts/mainTheme/check-all.jquery.js",
                //"~/Scripts/mainTheme/data-table.jquery.js",
                //"~/Scripts/mainTheme/ZeroClipboard.js",
                //"~/Scripts/mainTheme/TableTools.min.js",
                //"~/Scripts/mainTheme/jeditable.jquery.js",
                //"~/Scripts/mainTheme/duallist.jquery.js",
                //"~/Scripts/mainTheme/easing.jquery.js",
                //"~/Scripts/mainTheme/full-calendar.jquery.js",
                //"~/Scripts/mainTheme/input-limiter.jquery.js",
                //"~/Scripts/mainTheme/inputmask.jquery.js",
                //"~/Scripts/mainTheme/meta-data.jquery.js",
                //"~/Scripts/mainTheme/quicksand.jquery.js",
                //"~/Scripts/mainTheme/raty.jquery.js",
                //"~/Scripts/mainTheme/smart-wizard.jquery.js",
                //"~/Scripts/mainTheme/stepy.jquery.js",
                //"~/Scripts/mainTheme/treeview.jquery.js",
                //"~/Scripts/mainTheme/ui-accordion.jquery.js",
                //"~/Scripts/mainTheme/vaidation.jquery.js",
                //"~/Scripts/mainTheme/mosaic.1.0.1.min.js",
                //"~/Scripts/mainTheme/jquery.collapse.js",
                "~/Scripts/mainTheme/jquery.cookie.js",
                //"~/Scripts/mainTheme/jquery.autocomplete.min.js",
                //"~/Scripts/mainTheme/localdata.js",
                //"~/Scripts/mainTheme/excanvas.min.js",
                "~/Scripts/Grid.js",
                "~/Scripts/Souccar.js",
                "~/Scripts/ColumnEditors.js",
                "~/Scripts/GridModel.js",
                "~/Scripts/DetailGrid.js",
                "~/Scripts/MainGrid.js",
                "~/Scripts/IndexGrid.js",
                "~/Scripts/Breadcrumb.js",
                "~/Scripts/RibbonModel.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
               "~/Scripts/kendo-" + KendoVersion + "/jquery.min.js",
               "~/Scripts/kendo-" + KendoVersion + "/kendo.web.min.js",
               "~/Scripts/kendo-" + KendoVersion + "/kendo.grid.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                //"~/Scripts/bootstrap/js/jquery.min.js" ,
                "~/Scripts/bootstrap/js/bootstrap.min.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/clockPicker").Include(
                "~/Scripts/clock-time-picker/bootstrap-clockpicker.min.js"));
           
            bundles.Add(new ScriptBundle("~/bundles/Highcharts").Include(
                "~/Scripts/Highcharts-7.1.1/highcharts.js",
                "~/Scripts/Highcharts-7.1.1/highcharts-3d.js",
                "~/Scripts/Highcharts-7.1.1/highcharts-more.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/highChartsModules").Include(
                "~/Scripts/Highcharts-" + highChartsVersion + "/modules/accessibility.js",
                "~/Scripts/Highcharts-" + highChartsVersion + "/modules/annotations.js",
                "~/Scripts/Highcharts-" + highChartsVersion + "/modules/data.js",
                "~/Scripts/Highcharts-" + highChartsVersion + "/modules/exporting.js",
                "~/Scripts/Highcharts-" + highChartsVersion + "/modules/offline-exporting.js",

                "~/Scripts/Highcharts-" + highChartsVersion + "/modules/funnel.js",
                "~/Scripts/Highcharts-" + highChartsVersion + "/modules/bullet.js",
                "~/Scripts/Highcharts-" + highChartsVersion + "/modules/series-label.js",
                "~/Scripts/Highcharts-" + highChartsVersion + "/modules/solid-gauge.js",
                "~/Scripts/Highcharts-" + highChartsVersion + "/modules/cylinder.js",
                "~/Scripts/Highcharts-" + highChartsVersion + "/modules/vector.js",
                "~/Scripts/Highcharts-" + highChartsVersion + "/modules/venn.js",
                "~/Scripts/Highcharts-" + highChartsVersion + "/modules/heatmap.js",
                "~/Scripts/Highcharts-" + highChartsVersion + "/modules/networkgraph.js",
                "~/Scripts/Highcharts-" + highChartsVersion + "/modules/sankey.js",
                "~/Scripts/Highcharts-" + highChartsVersion + "/modules/organization.js",
                "~/Scripts/Highcharts-" + highChartsVersion + "/modules/sunburst.js",
                "~/Scripts/Highcharts-" + highChartsVersion + "/modules/timeline.js",
                "~/Scripts/Highcharts-" + highChartsVersion + "/modules/variable-pie.js",
                "~/Scripts/Highcharts-" + highChartsVersion + "/modules/variwide.js",
                "~/Scripts/Highcharts-" + highChartsVersion + "/modules/windbarb.js",
                "~/Scripts/Highcharts-" + highChartsVersion + "/modules/xrange.js",
                "~/Scripts/Highcharts-" + highChartsVersion + "/modules/histogram-bellcurve.js",

                "~/Scripts/Highcharts-" + highChartsVersion + "/lib/rgbcolor.js",
                "~/Scripts/Highcharts-" + highChartsVersion + "/lib/canvg.js",
                "~/Scripts/Highcharts-" + highChartsVersion + "/lib/jspdf.js",
                "~/Scripts/Highcharts-" + highChartsVersion + "/lib/svg2pdf.js"



                ));

            bundles.Add(new ScriptBundle("~/bundles/json").Include(
                "~/Scripts/json2.js"));

            bundles.Add(new StyleBundle("~/Content/mainTheme").Include(
                "~/Content/template/reset.css",
                "~/Content/template/themes.css",
                "~/Content/template/typography.css",
                //"~/Content/template/shCore.css",
                //"~/Content/template/jquery.jqplot.css", 
                //"~/Content/template/jquery-ui-1.8.18.custom.css", 
                //"~/Content/template/form.css",
                //"~/Content/template/ui-elements.css", 
                //"~/Content/template/wizard.css", 
                //"~/Content/template/sprite.css"
                "~/Content/template/gradient.css"
            //"~/Content/template/maestro-style/main.css"
            ));

            //bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
            //    "~/Content/themes/base/jquery.ui.core.css",
            //    "~/Content/themes/base/jquery.ui.resizable.css",
            //    "~/Content/themes/base/jquery.ui.selectable.css",
            //    "~/Content/themes/base/jquery.ui.accordion.css",
            //    "~/Content/themes/base/jquery.ui.autocomplete.css",
            //    "~/Content/themes/base/jquery.ui.button.css",
            //    "~/Content/themes/base/jquery.ui.dialog.css",
            //    "~/Content/themes/base/jquery.ui.slider.css",
            //    "~/Content/themes/base/jquery.ui.tabs.css",
            //    "~/Content/themes/base/jquery.ui.datepicker.css",
            //    "~/Content/themes/base/jquery.ui.progressbar.css",
            //    "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new StyleBundle("~/Content/kendo").Include(
                "~/Content/kendo-" + KendoVersion + "/kendo.common.min.css",
                "~/Content/kendo-" + KendoVersion + "/kendo.flat.min.css"));
            
            bundles.Add(new StyleBundle("~/Content/kendo-rtl").Include(
                "~/Content/kendo-" + KendoVersion + "/kendo.rtl.min.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                "~/Scripts/bootstrap/css/bootstrap.min.css"));
            bundles.Add(new StyleBundle("~/Content/clockPicker").Include(
                "~/Scripts/clock-time-picker/bootstrap-clockpicker.min.css"));
            // Clear all items from the default ignore list to allow minified CSS and JavaScript files to be included in debug mode
            bundles.IgnoreList.Clear();

            // Add back the default ignore list rules sans the ones which affect minified files and debug mode
            bundles.IgnoreList.Ignore("*.intellisense.js");
            bundles.IgnoreList.Ignore("*-vsdoc.js");
            bundles.IgnoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);

            ////orgChart tree scripts
            //bundles.Add(new ScriptBundle("~/bundles/orgChart").Include(
            //       "~/Scripts/orgChart/js/jquery.orgchart.min.js",
            //       "~/Scripts/orgChart/js/html2canvas.min.js",
            //       "~/Scripts/orgChart/js/jspdf.umd.min.js"
            //));

            //orgChart tree css styles
            bundles.Add(new StyleBundle("~/Content/orgChart").Include(
                "~/Scripts/orgChart/css/jquery.orgchart.min.css"));
            bundles.Add(new ScriptBundle("~/bundles/orgChart").Include(
                   "~/Scripts/orgChart/js/jquery.orgchart.min.js",
                   "~/Scripts/orgChart/js/html2canvas.min.js",
                   "~/Scripts/orgChart/js/jspdf.umd.min.js"
            ));
        }
    }
}