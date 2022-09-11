#region

using System.Collections.Generic;
using Infrastructure.Entities;
using Infrastructure.Validation;
using Service;
using Souccar.Domain.Entities;
using Souccar.ReportGenerator.Domain.Classification;
using Souccar.ReportGenerator.Domain.QueryBuilder;
using StructureMap;
using UI.Helpers.Controllers;
using UI.Helpers.Security;

#endregion

namespace UI.Areas.Reporting.Controllers.EntitiesRoots
{
    public class ReportAggregateController : RootEntityController, IAggregateController<Report>
    {
        #region Implementation of IAggregateController<ReportClassification>

        #region Services

        #region Entity Service

        private EntityServiceBase<ReportTemplate> _reportTemplateService;

        public EntityServiceBase<ReportTemplate> ReportTemplateService
        {
            get
            {
                return _reportTemplateService ??
                       (_reportTemplateService = ObjectFactory.GetInstance<EntityService<ReportTemplate>>());
            }
        }

        private EntityServiceBase<Report> _service;

        public EntityServiceBase<Report> Service
        {
            get
            {
                return _service ??
                       (_service = ObjectFactory.GetInstance<EntityService<Report>>());
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