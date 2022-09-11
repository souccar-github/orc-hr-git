#region

using System.Collections.Generic;
using Infrastructure.Entities;
using Infrastructure.Validation;
using HRIS.Domain.Personnel.Entities;
using Service;
using Souccar.Domain.Entities;
using StructureMap;
using UI.Helpers.Controllers;
using UI.Helpers.Security;

#endregion

namespace UI.Areas.Personnel.Controllers.EntitiesRoots
{
    public abstract class EmployeeAggregateController : RootEntityController, IAggregateController<Employee>
    {
        #region Implementation of IAggregateController<Employee>

        private EntityServiceBase<Employee> _service;

        public EntityServiceBase<Employee> Service
        {
            get { return _service ?? (_service = ObjectFactory.GetInstance<EntityService<Employee>>()); }
        }

        public virtual void PrePublish()
        {
            LoadStepsList();
            CleanUpModelState();
            Permissions();
            ViewData["ExpiredRules"] = GetExpiredRules();
            FillList();
        }

        public void Permissions()
        {
            if (Key.Status())
            {
                ViewData["CanCreate"] = false; // User.IsInRole("Per_Create");
                ViewData["CanRead"] = User.IsInRole("Per_Read");
                ViewData["CanUpdate"] = User.IsInRole("Per_Update");
                ViewData["CanDelete"] = User.IsInRole("Per_Delete");
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