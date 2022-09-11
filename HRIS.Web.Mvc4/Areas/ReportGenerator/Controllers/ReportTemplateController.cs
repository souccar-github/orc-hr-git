using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Souccar.Domain.PersistenceSupport;
using Souccar.NHibernate;
using Souccar.NHibernate.Web.Mvc;
using Souccar.Reflector;
using Souccar.ReportGenerator.Domain.Classification;
using Souccar.Core.Extensions;
using Project.Web.Mvc4.Models;

namespace HRIS.Web.Mvc4.Areas.ReportGenerator.Controllers
{
    public class ReportTemplateController : Controller
    {

        private IRepository<ReportTemplate> _repository;

        public ReportTemplateController(IRepository<ReportTemplate> repository)
        {
            _repository = repository;
        }

        //
        // GET: /ReportGenerator/ReportTemplate/

        [Transaction]
        public void Create(IDictionary<string, object> data = null, RequestInformation requestInformation = null)
        {

            var reportTemplate = new ReportTemplate {
                Name = data["Name"].ToString(),
                ShowDateTime = bool.Parse(data["ShowDateTime"].ToString()),
                ShowUserName = bool.Parse(data["ShowUserName"].ToString()),
                ShowPageNumber = bool.Parse(data["ShowPageNumber"].ToString()),
                ShowHeader = bool.Parse(data["ShowHeader"].ToString()),
                ShowFooter = bool.Parse(data["ShowFooter"].ToString()),
            };

            //var reportTemplateContent = new ReportTemplateContent
            //                                {
            //                                    ShowDateTime = bool.Parse(data["ShowDateTime"].ToString()),
            //                                    ShowUserName = bool.Parse(data["ShowUserName"].ToString()),
            //                                    ShowPageNumber = bool.Parse(data["ShowPageNumber"].ToString()),
            //                                    ShowHeader = bool.Parse(data["ShowHeader"].ToString()),
            //                                    ShowFooter = bool.Parse(data["ShowFooter"].ToString()),
            //                                    RtfReportHeader = readPostedFileContent((HttpPostedFileBase)TempData["RtfReportHeader"]),
            //                                    RtfReportFooter = readPostedFileContent((HttpPostedFileBase)TempData["RtfReportFooter"])
            //                                };


            //reportTemplate.Content = reportTemplateContent;

            _repository.Add(reportTemplate);
            

        }

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

        [Transaction]
        public void Update(IDictionary<string, object> data = null, RequestInformation requestInformation = null)
        {
            var reportTemplate = _repository.GetById(int.Parse(data["Id"].ToString()));
            var classTree = ClassTreeFactory.Create(typeof(ReportTemplateContent));
            //var reportTemplateContent = reportTemplate.Content;

            foreach (var item in data)
            {
                if (classTree.SimpleProperties.SingleOrDefault(x => x.Name == item.Key) == null)
                    continue;

                //reportTemplateContent.SetPropertyValue(item.Key, item.Value);
            }

            if (data.ContainsKey("Name"))
                reportTemplate.Name = data["Name"].ToString();

            //if (TempData["RtfReportHeader"] != null)
            //    reportTemplateContent.RtfReportHeader = readPostedFileContent((HttpPostedFileBase)TempData["RtfReportHeader"]);

            //if (TempData["RtfReportFooter"] != null)
            //    reportTemplateContent.RtfReportFooter = readPostedFileContent((HttpPostedFileBase)TempData["RtfReportFooter"]);

            //reportTemplate.Content = reportTemplateContent;

            _repository.Update(reportTemplate);

            //var dbContext = (IDbContext)repository.GetPropertyValue("DbContext");
            //using (dbContext.BeginTransaction())
            //{w
            //    dbContext.CommitTransaction();
            //}

        }

        [Transaction]
        public void Delete(IDictionary<string, object> data = null, RequestInformation requestInformation = null)
        {
            var reportTemplate = _repository.GetById(int.Parse(data["Id"].ToString()));

            _repository.Delete(reportTemplate);

            //var previous = requestInformation.NavigationInfo.Previous;
            //var entity = new ReportTemplate();

            //var classTree = ClassTreeFactory.Create(previous[0].TypeName.ToType());
            //foreach (var property in classTree.SimpleProperties.Where(x => !x.IsPrimaryKey))
            //    entity.SetPropertyValue(property.Name, data[property.Name].To(property.PropertyType));

            
            //var repository = new NHibernateRepository<ReportTemplate>();
            //var currentReportTemplate = repository.GetById(entity.Id);
            //repository.Delete(currentReportTemplate);

            //var dbContext = (IDbContext)repository.GetPropertyValue("DbContext");
            //using (dbContext.BeginTransaction())
            //{
            //    dbContext.CommitTransaction();
            //}

        }

    }
}
