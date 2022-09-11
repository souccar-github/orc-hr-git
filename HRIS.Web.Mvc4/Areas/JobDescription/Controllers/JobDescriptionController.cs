using Project.Web.Mvc4.Models.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project.Web.Mvc4.Extensions;
using HRIS.Domain.JobDescription.Indexes;
using System.Collections;
using Souccar.Infrastructure.Extenstions;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Helpers.Resource;

namespace Project.Web.Mvc4.Areas.JobDescription.Controllers
{
    public class JobDescriptionController : Controller
    {
        #region Reporting Target
        public ActionResult GetReportingTarget(int jobDescriptionId)
        {
            var jdType = typeof(HRIS.Domain.JobDescription.RootEntities.JobDescription);
            var jobDescriptions = jdType.GetAll<HRIS.Domain.JobDescription.RootEntities.JobDescription>();
            var jd = (HRIS.Domain.JobDescription.RootEntities.JobDescription)jdType.GetById(jobDescriptionId);
            var result = jobDescriptions.Select(
                x => new DualSelectListModel()
                {
                    Value = x.Id.ToString(),
                    Title = x.Name,
                    Group = x.JobTitle.Name,
                    Description = "",
                    Dir = DualSelectListDirection.Left.ToString()
                }).ToList();

            if (jd.Reportings.Count != 0)
            {
                var managers = result.Where(x => 
                    jd.Reportings.Select(y => y.ManagerJobDescription.Id.ToString())
                    .Contains(x.Value)).ToList();
                foreach (var item in managers)
                {
                    item.Dir = DualSelectListDirection.Right.ToString();
                }
            }
            return Json(result.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveReportingTarget(int jobDescriptionId, IList<DualSelectListModel> managers)
        {
            var jdType = typeof(HRIS.Domain.JobDescription.RootEntities.JobDescription);
            var jd = (HRIS.Domain.JobDescription.RootEntities.JobDescription)jdType.GetById(jobDescriptionId);
            jd.Reportings.Clear();
            foreach (var item in managers.Where(x => x.Dir == DualSelectListDirection.Right.ToString()))
            {
                var mJD = (HRIS.Domain.JobDescription.RootEntities.JobDescription)jdType.GetById(int.Parse(item.Value));
                jd.AddManager(mJD,true);
            }
            jd.Save();
            return Json(true, JsonRequestBehavior.AllowGet);
        
        }
        #endregion 
       
        #region Delegate
        public ActionResult GetDelegate(int jobDescriptionId)
        {
            var jdType = typeof(HRIS.Domain.JobDescription.RootEntities.JobDescription);
            var jobDescriptions = jdType.GetAll<HRIS.Domain.JobDescription.RootEntities.JobDescription>();
            var jd = (HRIS.Domain.JobDescription.RootEntities.JobDescription)jdType.GetById(jobDescriptionId);
            var result = jobDescriptions.Select(
                x => new DualSelectListModel()
                {
                    Value = x.Id.ToString(),
                    Title = x.Name,
                    Group=x.JobTitle.Name,
                    Description = "",
                    Dir = DualSelectListDirection.Left.ToString()
                }).ToList();
            foreach (var item in jd.Delegates)
	        {
		        var temp=getItemByValue(result,item.SecondaryJobDescription.Id.ToString());
                temp.Dir = DualSelectListDirection.Right.ToString();
                if(item.AuthorityType!=null)
                    temp.Metadata.Add(new MetadataItem() 
                    {
                        Id=item.AuthorityType.Id.ToString(),
                        Name=item.AuthorityType.Name
                    });
	        }
            return Json(result.ToList(), JsonRequestBehavior.AllowGet);
        }
       
        [HttpPost]
        public ActionResult SaveDelegate(int jobDescriptionId, IList<DualSelectListModel> delegates)
        {
            var info = JobDescriptionLocalizationHelper.GetResource(JobDescriptionLocalizationHelper.ManageDelegate);
            var start = DateTime.Now;
            var jdType = typeof(HRIS.Domain.JobDescription.RootEntities.JobDescription);
            var jd = (HRIS.Domain.JobDescription.RootEntities.JobDescription)jdType.GetById(jobDescriptionId);
            jd.Delegates.Clear();
            foreach (var item in delegates.Where(x => x.Dir == DualSelectListDirection.Right.ToString()))
            {
                var mJD = (HRIS.Domain.JobDescription.RootEntities.JobDescription)jdType.GetById(int.Parse(item.Value));
                var authorityType = (AuthorityType)typeof(AuthorityType).GetById(int.Parse(item.Metadata[0].Id));
                jd.AddDelegate(mJD, authorityType);
            }
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { jd }, UserExtensions.CurrentUser, jd, Souccar.Domain.Audit.OperationType.Update, info, start);
            return Json(true, JsonRequestBehavior.AllowGet);

        }

        private DualSelectListModel getItemByValue(IList<DualSelectListModel> items, string value)
        {
            return items.SingleOrDefault(x => x.Value == value);
        }
        #endregion 

        #region Assign Manager
        public ActionResult GetAssignManager(int jobDescrptionId)
        {
            var jd = (HRIS.Domain.JobDescription.RootEntities.JobDescription)
                typeof (HRIS.Domain.JobDescription.RootEntities.JobDescription).GetById(jobDescrptionId);
            var jobDescriptionReporting = jd.Reportings.FirstOrDefault();
            var managerId = 0;
            if (jobDescriptionReporting != null)
            {
                 managerId = jobDescriptionReporting.ManagerJobDescription.Id;
            }
            var result =
                typeof (HRIS.Domain.JobDescription.RootEntities.JobDescription).GetAll<HRIS.Domain.JobDescription.RootEntities.JobDescription>()
                    .Select(x => new {Id = x.Id, Name = x.Name}).ToList();
            return Json(new { Data = result ,ManagerId=managerId}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveAssignManager(int jobDescriptionId,int managerId)
        {
            var jd = (HRIS.Domain.JobDescription.RootEntities.JobDescription)
               typeof(HRIS.Domain.JobDescription.RootEntities.JobDescription).GetById(jobDescriptionId);
            jd.Reportings.Clear();

            var jdManager = (HRIS.Domain.JobDescription.RootEntities.JobDescription)
               typeof(HRIS.Domain.JobDescription.RootEntities.JobDescription).GetById(managerId);
            jd.AddManager(jdManager);
            jd.Save();
            return Json(true, JsonRequestBehavior.AllowGet);

        }
        #endregion
    }
}
