#region

using System.Collections.Generic;
using HRIS.Domain.Objectives.RootEntities;
using Infrastructure.Entities;
using Infrastructure.Validation;
using HRIS.Domain.Objectives.Entities;
using Service;
using Souccar.Domain.Entities;
using StructureMap;
using UI.Helpers.Controllers;
using UI.Helpers.Security;

#endregion

namespace UI.Areas.Objective.Controllers.EntitiesRoots
{
    public class StrategicObjectiveAggregateController : RootEntityController,
                                                              IAggregateController<StrategicObjective>
    {
        #region Implementation of IAggregateController<OrganizationalObjective>

        #region Services

        #region Entity Service

        private EntityServiceBase<StrategicObjective> _service;

        public EntityServiceBase<StrategicObjective> Service
        {
            get
            {
                return _service ??
                       (_service = ObjectFactory.GetInstance<EntityService<StrategicObjective>>());
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