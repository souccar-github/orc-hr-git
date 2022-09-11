#region

using System.Collections.Generic;
using Infrastructure.Entities;
using Infrastructure.Validation;
using HRIS.Domain.OrgChart.Entities;
using Service;
using Souccar.Domain.Entities;
using StructureMap;
using UI.Helpers.Controllers;
using UI.Helpers.Security;

#endregion

namespace UI.Areas.OrganizationChart.Controllers.EntitiesRoots
{
    public abstract class GradeAggregateController : RootEntityController, IAggregateController<Grade>
    {
        #region Implementation of IAggregateController<Grade>

        private EntityServiceBase<Grade> _service;

        public EntityServiceBase<Grade> Service
        {
            get { return _service ?? (_service = ObjectFactory.GetInstance<EntityService<Grade>>()); }
        }

        public void Permissions()
        {
            if (Key.Status())
            {
                ViewData["CanCreate"] = true; //User.IsInRole("Per_Create");
                ViewData["CanRead"] = true; //User.IsInRole("Per_Read");
                ViewData["CanUpdate"] = true; //User.IsInRole("Per_Update");
                ViewData["CanDelete"] = true; //User.IsInRole("Per_Delete");
            }
            else
            {
                ViewData["CanCreate"] = true;
                ViewData["CanRead"] = true;
                ViewData["CanUpdate"] = true;
                ViewData["CanDelete"] = true;
            }
        }

        public void PrePublish()
        {
            LoadStepsList();

            CleanUpModelState();
            Permissions();
            ViewData["ExpiredRules"] = GetExpiredRules();
            FillList();
        }

        #region Virtuals

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