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
    public abstract class NodeAggregateController : RootEntityController, IAggregateController<Node>
    {
        #region Implementation of IAggregateController<Node>

        #region Services

        #region Node Service

        private EntityServiceBase<Node> _service;

        public EntityServiceBase<Node> Service
        {
            get { return _service ?? (_service = ObjectFactory.GetInstance<EntityService<Node>>()); }
        }

        #endregion


        #region Organization Service

        private EntityServiceBase<Organization> _organizationService;

        public EntityServiceBase<Organization> OrganizationService
        {
            get
            {
                return _organizationService ??
                       (_organizationService = ObjectFactory.GetInstance<EntityService<Organization>>());
            }
        }

        #endregion


        #region Position Service

        private EntityServiceBase<Position> _positionService;

        public EntityServiceBase<Position> PositionService
        {
            get
            {
                return _positionService ??
                       (_positionService = ObjectFactory.GetInstance<EntityService<Position>>());
            }
        }

        #endregion
        #endregion

        public void Permissions()
        {
            if (Key.Status())
            {
                ViewData["CanCreate"] = User.IsInRole("Per_Create");
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