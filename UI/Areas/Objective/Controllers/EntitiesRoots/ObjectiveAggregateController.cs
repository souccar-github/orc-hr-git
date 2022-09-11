#region

using System.Collections.Generic;
using Infrastructure.Entities;
using Infrastructure.Validation;
using HRIS.Domain.Objectives.Entities;
using HRIS.Domain.OrgChart.ValueObjects;
using Service;
using Souccar.Domain.Entities;
using StructureMap;
using UI.Helpers.Controllers;
using UI.Helpers.Security;

#endregion

namespace UI.Areas.Objective.Controllers.EntitiesRoots
{
    public class ObjectiveAggregateController : RootEntityController,
                                                IAggregateController<HRIS.Domain.Objectives.RootEntities.Objective>
    {
        #region Implementation of IAggregateController<Objective>

        #region Services

        #region Entity Service

        private EntityServiceBase<Position> _positionService;
        private EntityServiceBase<HRIS.Domain.Objectives.RootEntities.Objective> _service;

        public EntityServiceBase<Position> PositionService
        {
            get
            {
                return _positionService ??
                       (_positionService = ObjectFactory.GetInstance<EntityService<Position>>());
            }
        }

        public EntityServiceBase<HRIS.Domain.Objectives.RootEntities.Objective> Service
        {
            get
            {
                return _service ??
                       (_service = ObjectFactory.GetInstance<EntityService<HRIS.Domain.Objectives.RootEntities.Objective>>());
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

        #region Shared With Value Object Relations

        #region Objective Related Node

        public int SharedWithRelatedNode
        {
            get
            {
                int id = 0;
                if (Session["SharedWithRelatedNode"] != null)
                {
                    int.TryParse(Session["SharedWithRelatedNode"].ToString(), out id);
                }

                return id;
            }

            set { Session["SharedWithRelatedNode"] = value != 0 ? value : 0; }
        }

        public void SaveSharedWithNodeToSession(int nodeId)
        {
            SharedWithRelatedNode = nodeId;
            SharedWithRelatedPosition = 0;
        }

        #endregion

        #region SharedWith Related Position

        public int SharedWithRelatedPosition
        {
            get
            {
                int id = 0;
                if (Session["SharedWithRelatedPosition"] != null)
                {
                    int.TryParse(Session["SharedWithRelatedPosition"].ToString(), out id);
                }

                return id;
            }

            set { Session["SharedWithRelatedPosition"] = value != 0 ? value : 0; }
        }

        public void SaveSharedWithPositionToSession(int positionId)
        {
            SharedWithRelatedPosition = positionId;
        }

        #endregion

        #endregion
    }
}