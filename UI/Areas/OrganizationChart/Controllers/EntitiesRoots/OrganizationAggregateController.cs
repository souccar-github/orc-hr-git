#region

using System.Collections.Generic;
using Infrastructure.Entities;
using Infrastructure.Validation;
using HRIS.Domain.OrgChart.Entities;
using HRIS.Domain.OrgChart.ValueObjects;
using Service;
using Souccar.Domain.Entities;
using StructureMap;
using UI.Helpers.Controllers;
using UI.Helpers.Security;

#endregion

namespace UI.Areas.OrganizationChart.Controllers.EntitiesRoots
{
    public abstract class OrganizationAggregateController : RootEntityController, IAggregateController<Organization>
    {
        #region Implementation of IAggregateController<Organization>

        #region Services

        #region Organization

        private EntityService<Organization> _service;

        public EntityServiceBase<Organization> Service
        {
            get { return _service ?? (_service = ObjectFactory.GetInstance<EntityService<Organization>>()); }
        }

        #endregion

        #region Node Service

        private EntityServiceBase<Node> _nodeService;

        public EntityServiceBase<Node> NodeService
        {
            get { return _nodeService ?? (_nodeService = ObjectFactory.GetInstance<EntityService<Node>>()); }
        }

        #endregion

        #endregion

        public void Permissions()
        {
            if (Key.Status())
            {
                ViewData["CanCreate"] = User.IsInRole("Org_Create");
                ViewData["CanRead"] = User.IsInRole("Org_Read");
                ViewData["CanUpdate"] = User.IsInRole("Org_Update");
                ViewData["CanDelete"] = User.IsInRole("Org_Delete");
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

        public virtual List<BrokenBusinessRule> GetExpiredRules()
        {
            return new List<BrokenBusinessRule>();
        }

        public virtual void FillList()
        {
        }

        public virtual void CleanUpModelState()
        {
        }

        #endregion

        #endregion
    }
}