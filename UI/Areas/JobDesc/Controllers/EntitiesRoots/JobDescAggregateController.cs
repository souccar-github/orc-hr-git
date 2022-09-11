#region

using System.Collections.Generic;
using Infrastructure.Entities;
using Infrastructure.Validation;
using HRIS.Domain.JobDesc.Entities;
using Service;
using Souccar.Domain.Entities;
using StructureMap;
using UI.Helpers.Controllers;
using UI.Helpers.Security;

#endregion

namespace UI.Areas.JobDesc.Controllers.EntitiesRoots
{
    public class JobDescAggregateController : RootEntityController, IAggregateController<JobDescription>
    {
        #region Implementation of IAggregateController<JobDescription>

        private EntityServiceBase<JobDescription> _service;

        public EntityServiceBase<JobDescription> Service
        {
            get { return _service ?? (_service = ObjectFactory.GetInstance<EntityService<JobDescription>>()); }
        }

        public void Permissions()
        {
            if (Key.Status())
            {
                ViewData["CanCreate"] = User.IsInRole("JobDesc_Create");
                ViewData["CanRead"] = User.IsInRole("JobDesc_Read");
                ViewData["CanUpdate"] = User.IsInRole("JobDesc_Update");
                ViewData["CanDelete"] = User.IsInRole("JobDesc_Delete");
            }
            else
            {
                ViewData["CanCreate"] = true;
                ViewData["CanRead"] = true;
                ViewData["CanUpdate"] = true;
                ViewData["CanDelete"] = true;
            }
        }

        #region Virtuals

        public virtual void PrePublish()
        {
            LoadStepsList();
            CleanUpModelState();
            Permissions();
            ViewData["ExpiredRules"] = GetExpiredRules();
            FillList();
        }

        public virtual void CleanUpModelState()
        {
        }

        public virtual List<BrokenBusinessRule> GetExpiredRules()
        {
            return new List<BrokenBusinessRule>();
        }

        public virtual void FillList()
        {
        }

        #endregion

        #endregion
    }
}