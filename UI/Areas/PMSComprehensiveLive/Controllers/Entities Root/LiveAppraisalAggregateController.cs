#region

using System.Collections.Generic;
using HRIS.Domain.PMS.RootEntities;
using Infrastructure.Entities;
using Infrastructure.Validation;
using HRIS.Domain.PMS.Entities;
using Service;
using Souccar.Domain.Entities;
using StructureMap;
using UI.Helpers.Controllers;
using UI.Helpers.Security;

#endregion

namespace UI.Areas.PMSComprehensiveLive.Controllers.EntitiesRoots
{
    public class LiveAppraisalAggregateController : RootEntityController, IAggregateController<Appraisal>
    {
        #region Implementation of IAggregateController<Appraisal>

        #region Services

        #region Entity Service

        private EntityServiceBase<Appraisal> _service;

        public EntityServiceBase<Appraisal> Service
        {
            get
            {
                return _service ??
                       (_service = ObjectFactory.GetInstance<EntityService<Appraisal>>());
            }
        }

        #endregion 

        #region Appraisal Phase Service


        private EntityServiceBase<AppraisalPhase> _appraisalPhaseService;

        public EntityServiceBase<AppraisalPhase> AppraisalPhaseService
        {
            get
            {
                return _appraisalPhaseService ??
                       (_appraisalPhaseService = ObjectFactory.GetInstance<EntityService<AppraisalPhase>>());
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
                ViewData["CanCreate"] = User.IsInRole("PMSComprehensive_Create");
                ViewData["CanRead"] = User.IsInRole("PMSComprehensive_Read");
                ViewData["CanUpdate"] = User.IsInRole("PMSComprehensive_Update");
                ViewData["CanDelete"] = User.IsInRole("PMSComprehensive_Delete");
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