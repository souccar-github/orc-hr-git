using System;
using System.Collections.Generic;
using HRIS.Domain.OrgChart.Entities;
using HRIS.Domain.PMS.RootEntities;
using Infrastructure.Repositories;
using Repository.UnitOfWork;
using Souccar.Domain.Entities;
using UI.Helpers.Controllers;
using Infrastructure.Entities;
using HRIS.Domain.PMS.Entities;
using StructureMap;
using UI.Helpers.Security;
using Infrastructure.Validation;
using Repository.NHibernate;

namespace UI.Areas.PMSComprehensive.Controllers.EntitiesRoots
{
    public class AppraisalTemplateAggregateController : RootEntityController, IAggregateController<AppraisalTemplate>
    {

        #region Implementation of IAggregateController<AppraisalTemplate>

        private EntityServiceBase<AppraisalTemplate> _service;

        private INhibernateUnitOfWork _unitOfWork;

        public INhibernateUnitOfWork UnitOfWork
        {
            get { return _unitOfWork ?? (_unitOfWork = new UnitOfWork()); }
        }


        public EntityServiceBase<AppraisalTemplate> Service
        {
            get { throw new NotImplementedException("The service is not implemented use the repository object instead."); }
        }

        private IRepository<AppraisalSection> _appraisalSectionRepository;

        public IRepository<AppraisalSection> AppraisalSectionRepository
        {
            get { return _appraisalSectionRepository ?? (_appraisalSectionRepository = new Repository<AppraisalSection>(UnitOfWork)); }
        }

        private IRepository<AppraisalTemplate> _appraisalTemplateRepository;

        public IRepository<AppraisalTemplate> AppraisalTemplatesRepository
        {
            get { return _appraisalTemplateRepository ?? (_appraisalTemplateRepository = new Repository<AppraisalTemplate>(UnitOfWork)); }
        }

        private IRepository<Grade> _gradesRepository;

        public IRepository<Grade> GradesRepository
        {
            get { return _gradesRepository ?? (_gradesRepository = new Repository<Grade>(UnitOfWork)); }
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

        #region Virtuals

        public virtual void PrePublish()
        {
            LoadStepsList();
            CleanUpModelState();
            Permissions();
            ViewData["ExpiredRules"] = GetExpiredRules();
            FillList();
        }

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
