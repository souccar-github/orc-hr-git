#region
using System;
using System.Collections.Generic;
using HRIS.Domain.PMS.RootEntities;
using Infrastructure.Entities;
using Infrastructure.Validation;
using Repository.NHibernate;
using Infrastructure.Repositories;
using Souccar.Domain.Entities;
using UI.Helpers.Controllers;
using UI.Helpers.Security;

#endregion

namespace UI.Areas.PMSComprehensive.Controllers.EntitiesRoots
{
    public class AppraisalSectionAggregateController : RootEntityController, IAggregateController<AppraisalSection>
    {
        #region Implementation of IAggregateController<Appraisal>

        #region Services

        #region Entity Service

        public EntityServiceBase<AppraisalSection> Service
        {
            get { throw new NotImplementedException("This service is not implemented. Use Repository instead."); }
        }

        private IRepository<AppraisalSection> _repository;

        public IRepository<AppraisalSection> Repository
        {
            get
            {
                return _repository ??
       (_repository = new Repository<AppraisalSection>());

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