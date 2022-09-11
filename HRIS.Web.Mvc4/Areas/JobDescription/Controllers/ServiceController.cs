#region using
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.JobDescription.Enum;
using  Project.Web.Mvc4.Areas.JobDescription.Helpers;
using  Project.Web.Mvc4.Areas.JobDescription.Models;
using  Project.Web.Mvc4.Helpers.DomainExtensions;
using  Project.Web.Mvc4.Helpers;
using  Project.Web.Mvc4.Helpers.Resource;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Notification;
using Souccar.Domain.Workflow.Entities;
using Souccar.Domain.Workflow.Enums;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Souccar.Domain.Extensions;
using  Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;
using  Project.Web.Mvc4.Factories;
#endregion 

namespace Project.Web.Mvc4.Areas.JobDescription.Controllers
{
    public class ServiceController : Controller
    {
        //
        // GET: /JobDescription/Service/DelegateAuthoritiesToPositionService
        #region Services Action
        public ActionResult DelegateRolesToPositionService()
        {
            return PartialView();
        }
        public ActionResult DelegateAuthoritiesToPositionService()
        {
            return PartialView();
        }
        #endregion

        // Get vacant positions
        public ActionResult GetVacantPositions()
        {
            var positions = ServiceFactory.ORMService.All<Position>().Where(x => x.AssigningEmployeeToPosition == null)
                .Select(x => EmployeeActionViewModelFactory.Create(x,GlobalResource.Delegate));
            return Json(positions.ToList(), JsonRequestBehavior.AllowGet);
        }

        // Get Authorities for position Id
        public ActionResult GetPositionAuthorities(int positionId)
            {
            var posAuth = ServiceFactory.ORMService.GetById<Position>(positionId).JobDescription.Authorities
                .Select(x => new AuthorityView()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description ?? string.Empty,
                    Type=x.Type.Name
                });

            var allAuthDel = ServiceFactory.ORMService.GetById<Position>(positionId).DelegateAuthoritiesFromPositions.ToList()
                .Select(x => new DelegateAuthResultViewModel()
                {
                    FromPositionId = x.SourcePosition.Id,
                    ToPositionId = x.DestinationPosition.Id,
                    ToPosition = x.DestinationPosition.NameForDropdown,
                    Comment = x.DelegationComment ?? string.Empty,
                    Reason = x.DelegationReason ?? string.Empty,
                    FromDate = x.FromDate,
                    ToDate = x.ToDate
                });

            return Json(new { Authorities = posAuth.ToList(), AllAuthDel =allAuthDel}, JsonRequestBehavior.AllowGet);
        }

        // Save delegation Authorities 
        public ActionResult SaveAuthDel(DelegateAuthResultViewModel model)
        {
            var fromPosition = ServiceFactory.ORMService.GetById<Position>(model.FromPositionId);
            var toPosition = ServiceFactory.ORMService.GetById<Position>(model.ToPositionId);
            var del=new DelegateAuthoritiesToPosition()
            {
                FromDate = model.FromDate,
                ToDate = model.ToDate,
                DelegationReason = model.Reason ?? string.Empty,
                DelegationComment = model.Comment ?? string.Empty,
                SourcePosition=fromPosition,
                DestinationPosition=toPosition
            };
            foreach (var item in model.AuthChecked)
            {
                var authority = ServiceFactory.ORMService.GetById<Authority>(item);
                del.AddDelegateAuthorities(new DelegateAuthoritiesToPositionAuthority()
                {
                    Authorities=authority 
                });
            }
            fromPosition.AddPositionDelegateAuthority(del);
            fromPosition.Save();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        // Save delegation Roles 
        public ActionResult SaveRoleDel(DelegateRolResultViewModel model)
        {
            var fromPosition = ServiceFactory.ORMService.GetById<Position>(model.FromPositionId);
            var toPosition = ServiceFactory.ORMService.GetById<Position>(model.ToPositionId);
            var superiorName = ServiceFactory.ORMService.GetById<Position>(model.SuperiorName);
            var del = new DelegateRolesToPosition()
            {
                FromDate = model.FromDate,
                ToDate = model.ToDate,
                DelegationReason = model.Reason ?? string.Empty,
                DelegationComment = model.Comment ?? string.Empty,
                SourcePosition = fromPosition,
                DestinationPosition = toPosition,
                PerformanceAppraisal = model.PerformanceAppraisal,
                Superior = superiorName
            };
            foreach (var item in model.RoleChecked)
            {
                var roles = ServiceFactory.ORMService.GetById<Role>(item);
                del.AddDelegateRoles(new DelegateRolesToPositionRole()
                {
                    Roles = roles
                });
            }
            fromPosition.AddPositionDelegateRole(del);
            fromPosition.Save();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        // Get roles for position Id 
        public ActionResult GetPositionRoles(int positionId)
        {
            var posrole = ServiceFactory.ORMService.GetById<Position>(positionId).JobDescription.Roles
                .Select(x => new RoleView()
                {
                    Id = x.Id,
                    RoleName = x.Name ?? string.Empty,
                    RoleDescreption = x.Summary ?? string.Empty
                });

            var allRolDel = ServiceFactory.ORMService.GetById<Position>(positionId).DelegateRolesFromPositions.ToList()
                .Select(x => new DelegateRolResultViewModel()
                {
                    FromPositionId = x.SourcePosition.Id,
                    ToPositionId = x.DestinationPosition.Id,
                    ToPosition=x.DestinationPosition.NameForDropdown,
                    PerformanceAppraisal = x.PerformanceAppraisal,
                    SuperiorName = x.Superior.Id,
                    Comment = x.DelegationComment ?? string.Empty,
                    Reason = x.DelegationReason ?? string.Empty,
                    FromDate = x.FromDate,
                    ToDate = x.ToDate
                });

            return Json(new { Roles = posrole.ToList(), AllroleDel = allRolDel }, JsonRequestBehavior.AllowGet);
        }

        // Get Not vacant positions
        public ActionResult GetNOtVacantPositions()
        {
            var positions = ServiceFactory.ORMService.All<Position>().Where(x => x.AssigningEmployeeToPosition != null)
                .Select(x => EmployeeActionViewModelFactory.Create(x, GlobalResource.Delegate));
            return Json(positions.Select(x => new { Id=x.PositionId,Name=x.PositionName}).ToList(), JsonRequestBehavior.AllowGet);
        }
        
    }
}
