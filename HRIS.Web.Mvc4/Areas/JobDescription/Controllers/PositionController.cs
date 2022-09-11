using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using  Project.Web.Mvc4.Models.Controls;
using  Project.Web.Mvc4.Extensions;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.JobDescription.Indexes;
using Souccar.Infrastructure.Extenstions;
using System.Collections;


namespace Project.Web.Mvc4.Areas.JobDescription.Controllers
{
    public class PositionController : Controller
    {
        #region Report Target
        public ActionResult GetReportingTarget(int positionId)
        {
            var positionType = typeof(Position);
            var position = (Position)positionType.GetById(positionId);
            var jdIds = position.JobDescription.Reportings.Select(x => x.ManagerJobDescription.Id).ToList();
            var positions = positionType.GetAll<Position>().Where(x=>jdIds.Contains(x.JobDescription.Id)).ToList();

            var result = positions.Select(
                x => new DualSelectListModel()
                {
                    Value = x.Id.ToString(),
                    Title = x.NameForDropdown,
                    Group = x.JobDescription.Name,
                    Description = "",
                    Dir = DualSelectListDirection.Left.ToString()
                }).ToList();
            foreach (var item in result.Where(x => position.ReportingsTo.Select(y => y.ManagerPosition.Id.ToString()).Contains(x.Value)))
            {
                item.Dir = DualSelectListDirection.Right.ToString();
            }
            return Json(result.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveReportingTarget(int positionId, IList<DualSelectListModel> managers)
        {
            var position = (Position)typeof(Position).GetById(positionId);
            position.ReportingsTo.Clear();
            foreach (var item in managers.Where(x => x.Dir == DualSelectListDirection.Right.ToString()))
            {
                var magagerPosition = (Position)typeof(Position).GetById(int.Parse(item.Value));
                position.AddReportingTo(magagerPosition);
            }
            position.Save();
            return Json(true, JsonRequestBehavior.AllowGet);

        }
        #endregion 

        #region Assign manager
        public ActionResult GetAssignManager(int positionId)
        {
            var position = (Position)typeof(Position).GetById(positionId);
            var managerId = 0;
            var manager = position.ReportingsTo.FirstOrDefault();
            if (manager != null)
            {
                managerId = manager.ManagerPosition.Id;
            }
            var jobDescriptionReporting = position.JobDescription.Reportings.FirstOrDefault();
            if (jobDescriptionReporting != null)
            {
                var result =
                    jobDescriptionReporting.ManagerJobDescription.Positions
                        .Select(x => new { Id = x.Id, Name = x.NameForDropdown }).ToList();
                return Json(new { Data = result, ManagerId = managerId }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Data = new object[]{}, ManagerId = managerId }, JsonRequestBehavior.AllowGet);
            
        }

        
        public ActionResult SaveAssignManager(int positionId, int managerId)
        {
            var position = (Position)typeof(Position).GetById(positionId);
            position.ReportingsTo.Clear();

            var jdManager = (Position)typeof(Position).GetById(managerId);
            position.AddReportingTo(jdManager,true);
            position.Save();
            return Json(true, JsonRequestBehavior.AllowGet);

        }

        #endregion Assign Manager

        #region Delegate
        public ActionResult GetDelegate(int positionId,int delegateType)
        {
            var position= (Position)typeof(Position).GetById(positionId);
            var positions = position.JobDescription.Delegates
                .Where(x => x.AuthorityType.Id == delegateType)
                .SelectMany(x=>x.SecondaryJobDescription.Positions).ToList();
            ;
            var result = positions.Select(
                x => new DualSelectListModel()
                {
                    Value = x.Id.ToString(),
                    Title = x.NameForDropdown,
                    Group = x.JobDescription.Name,
                    Description = "",
                    Dir = DualSelectListDirection.Left.ToString()
                }).ToList();
            foreach (var item in position.Delegates.Where(x=>x.AuthorityType.Id==delegateType))
            {
                var temp = getItemByValue(result, item.SecondaryPosition.Id.ToString());
                if (temp != null)
                {
                    temp.Dir = DualSelectListDirection.Right.ToString();
                }
            }
            return Json(result.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveDelegate(int positionId,int delegateType, IList<DualSelectListModel> delegates)
        {
            if(delegates==null)
                return Json(true, JsonRequestBehavior.AllowGet);
            var position = (Position)typeof(Position).GetById(positionId);
            foreach (var item in position.Delegates.Where(x => x.AuthorityType.Id == delegateType).ToList())
            {
                position.Delegates.Remove(item);
            }
            var authorityType=(AuthorityType)typeof(AuthorityType).GetById(delegateType);
            foreach (var item in delegates.Where(x=>x.Dir==DualSelectListDirection.Right.ToString()))
            {
                var secondaryPosition = (Position)typeof(Position).GetById(int.Parse(item.Value));
                position.AddDelegate(secondaryPosition, authorityType);
            }
            position.Save();
            return Json(true, JsonRequestBehavior.AllowGet);

        }
      
        private DualSelectListModel getItemByValue(IList<DualSelectListModel> items, string value)
        {
            return items.SingleOrDefault(x => x.Value == value);
        }

        #endregion

        [HttpPost]
        public ActionResult GetGradeStep(int jobDesId)
        {
            var jobdes = (HRIS.Domain.JobDescription.RootEntities.JobDescription)typeof(HRIS.Domain.JobDescription.RootEntities.JobDescription).GetById(jobDesId);
            var steps = jobdes.JobTitle.Grade.Steps;
            var result = new ArrayList();

            if (steps.Count == 0)
                return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
           
            foreach (var item in steps)
            {
                var temp = new Dictionary<string, object>();
                temp["Id"] = item.Id;
                temp["Name"] = item.Name;
                result.Add(temp);
            }
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }
    }
}
