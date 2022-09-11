#region

using System;
using System.Collections.Generic;
using Infrastructure.Entities;
using Infrastructure.Validation;
using HRIS.Domain.OrgChart.ValueObjects;
using HRIS.Domain.Personnel.Entities;
using Service;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Entities;
using StructureMap;
using UI.Helpers.Controllers;
using UI.Helpers.Security;

#endregion

namespace UI.Areas.Services.Controllers.EntitiesRoots
{
    public class ServicesAggregateController : RootEntityController, IAggregateController<IAggregateRoot>
    {
        #region Implementation of IAggregateController<IEntity>

        #region Services

        #region IEntity Service

        private EntityServiceBase<IAggregateRoot> _service;

        public EntityServiceBase<IAggregateRoot> Service
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        

        #endregion

        public virtual void CleanUpModelState()
        {
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

        public virtual void PrePublish()
        {
            LoadStepsList();
            CleanUpModelState();
            Permissions();
            ViewData["ExpiredRules"] = GetExpiredRules();
            FillList();
        }

        public List<BrokenBusinessRule> GetExpiredRules()
        {
            return new List<BrokenBusinessRule>();
        }

        public virtual void FillList()
        {
        }

        #endregion
    }
}