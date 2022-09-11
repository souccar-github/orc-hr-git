#region

using System.Collections.Generic;
using Infrastructure.Entities;
using Infrastructure.Validation;
using HRIS.Domain.OrgChart.ValueObjects;
using HRIS.Domain.ProjectManagment.Entities;
using Service;
using Souccar.Domain.Entities;
using StructureMap;
using UI.Helpers.Controllers;
using UI.Helpers.Security;

#endregion

namespace UI.Areas.ProjectManagement.Controllers.EntitiesRoots
{
    public class ProjectAggregateController : RootEntityController, IAggregateController<Project>
    {
        #region Implementation of IAggregateController<Project>

        #region Services

        #region Entity Service

        private EntityServiceBase<Position> _positionService;
        private EntityServiceBase<Project> _service;

        public EntityServiceBase<Position> PositionService
        {
            get
            {
                return _positionService ??
                       (_positionService = ObjectFactory.GetInstance<EntityService<Position>>());
            }
        }

        public EntityServiceBase<Project> Service
        {
            get
            {
                return _service ??
                       (_service = ObjectFactory.GetInstance<EntityService<Project>>());
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
                ViewData["CanCreate"] = User.IsInRole("Objective_Create");
                ViewData["CanRead"] = User.IsInRole("Objective_Read");
                ViewData["CanUpdate"] = User.IsInRole("Objective_Update");
                ViewData["CanDelete"] = User.IsInRole("Objective_Delete");
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

        public virtual List<BrokenBusinessRule> GetExpiredRules()
        {
            return new List<BrokenBusinessRule>();
        }

        public virtual void FillList()
        {
        }

        #endregion
    }
}