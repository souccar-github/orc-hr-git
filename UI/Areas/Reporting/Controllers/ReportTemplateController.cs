#region

using System;
using System.Web;
using System.Web.Mvc;
using Infrastructure.Validation;
using Souccar.Core;
using Souccar.ReportGenerator.Domain.Classification;
using Telerik.Web.Mvc;
using UI.Areas.Reporting.Controllers.EntitiesRoots;
using UI.Helpers.Model;
using Validation.Reporting;

#endregion

namespace UI.Areas.Reporting.Controllers
{
    public class ReportTemplateController : ReportAggregateController, IRule<ReportTemplate>
    {

        #region IRule<ReportTemplate> Members

        public ObjectRules<ReportTemplate> Rules
        {
            get { return new ReportTemplateRules(); }
        }

        #endregion

        #region Overrides of ReportAggregateController

        public override void CleanUpModelState()
        {
            
        }

        #endregion

        #region CRUD

        #region Read

        [GridAction]
        public ActionResult Index()
        {
            PrePublish();

            #region Get Data

            var reportTemplates = ReportTemplateService.GetAll();
            ViewData["reportTemplates"] = reportTemplates;
            
            #endregion

            return View();
        }

        #endregion

        #region Create & Update

        public ActionResult Insert(int id =0)
        {
            LoadStepsList();

            if(id != 0)
            {
                ReportTemplate reportTemplate = ReportTemplateService.GetById(id);

                return View("Insert", reportTemplate);
            }

            return View("Insert", new ReportTemplate());
        }

        [HttpPost]
        public ActionResult Save(ReportTemplate reportTemplate)
        {
            PrePublish();
           
            if (reportTemplate.IsTransient())
            {
                reportTemplate.Content.RtfReportHeader = readPostedFileContent(Request.Files["RtfReportHeader"]);
                reportTemplate.Content.RtfReportFooter = readPostedFileContent(Request.Files["RtfReportFooter"]);
            }
            else
            {
                var orginalReportTemplate = ReportTemplateService.GetById(reportTemplate.Id);
                var headerArray = readPostedFileContent(Request.Files["RtfReportHeader"]);
                var footerArray = readPostedFileContent(Request.Files["RtfReportFooter"]);

                if ((headerArray != null) && headerArray.Length > 0)
                    orginalReportTemplate.Content.RtfReportHeader = headerArray;
                if ((footerArray != null) && footerArray.Length > 0)
                    orginalReportTemplate.Content.RtfReportFooter = footerArray;

                orginalReportTemplate.Name = reportTemplate.Name;
                orginalReportTemplate.Content.ShowDateTime = reportTemplate.Content.ShowDateTime;
                orginalReportTemplate.Content.ShowFooter = reportTemplate.Content.ShowFooter;
                orginalReportTemplate.Content.ShowHeader = reportTemplate.Content.ShowHeader;
                orginalReportTemplate.Content.ShowPageNumber = reportTemplate.Content.ShowPageNumber;
                orginalReportTemplate.Content.ShowUserName = reportTemplate.Content.ShowUserName;
                reportTemplate = orginalReportTemplate;

            }
            if ((Rules.GetBrokenRules(reportTemplate).Count == 0))
            {
                try
                {
                    ReportTemplateService.Update(reportTemplate);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(DomainErrors.InternalError.ToString(),ex.Message);
                    return View("Insert", reportTemplate);
                }
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(reportTemplate));

                return View("Insert", reportTemplate);
            }

            PrePublish();

            return RedirectToAction("Index", new { id = reportTemplate.Id });

        }

        #endregion

        #region Delete

        [HttpPost]
        public ActionResult Delete(int id)
        {
            PrePublish();

            try
            {
                ReportTemplate reportTemplate = ReportTemplateService.LoadById(id);
                ReportTemplateService.Delete(reportTemplate);
                
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(DomainErrors.InternalError.ToString(), ex.Message);
            }
           
            return RedirectToAction("Index");
        }

        #endregion

        private byte[] readPostedFileContent(HttpPostedFileBase postedFile)
        {
            byte[] fileArray = null;

            if ((postedFile != null) && (postedFile.InputStream != null))
            {
                var streamRtfReportHeader = postedFile.InputStream;
                var fileRtfReportHeaderLength = (int)streamRtfReportHeader.Length;

                using (streamRtfReportHeader)
                {
                    fileArray = new byte[fileRtfReportHeaderLength];
                    streamRtfReportHeader.Read(fileArray, 0, fileRtfReportHeaderLength);
                    streamRtfReportHeader.Close();

                }
            }
            return fileArray;
        }

        private ActionResult ReadRtf(byte[] source)
        {
            if ((source == null) || (source.Length == 0))
            {
                return new EmptyResult();
            }

            var memoryStream = new System.IO.MemoryStream(source);

            return new FileStreamResult(memoryStream, "application/rtf");
        }

        public ActionResult OpenRTFFile(int id,string header = "Header")
        {
            ReportTemplate reportTemplate = ReportTemplateService.GetById(id);
            if (reportTemplate!=null)
            {
                if (header == "Header")
                    return ReadRtf(reportTemplate.Content.RtfReportHeader);
                else
                    return ReadRtf(reportTemplate.Content.RtfReportFooter);

            }
            return RedirectToAction("Index");
        }

        #endregion
    }
}
