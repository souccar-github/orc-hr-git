#region

using System;
using System.Collections.Generic;
using HRIS.Domain.OrgChart.Entities;
using HRIS.Domain.PMS.RootEntities;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Infrastructure.Validation;
using HRIS.Domain.PMS.Entities;
using Repository.UnitOfWork;
using Service;
using Souccar.Domain.Entities;
using StructureMap;
using UI.Helpers.Controllers;
using UI.Helpers.Security;
using Repository.NHibernate;

#endregion

namespace UI.Areas.PMSComprehensive.Controllers.EntitiesRoots
{
    public class AppraisalPhaseAggregateController : RootEntityController, IAggregateController<AppraisalPhase>
    {
        #region Implementation of IAggregateController<AppraisalPhase>

        #region Services

        #region Entity Service

        private EntityServiceBase<AppraisalPhase> _service;

        public EntityServiceBase<AppraisalPhase> Service
        {
            get
            {
                throw new NotImplementedException("The service is not implemented use the repository object instead.");
            }
        }

        private INhibernateUnitOfWork _unitOfWork;

        public INhibernateUnitOfWork UnitOfWork
        {
            get { return _unitOfWork ?? (_unitOfWork = new UnitOfWork()); }
        }
        private IRepository<AppraisalPhase> _appraisalPhaseRepository;

        public IRepository<AppraisalPhase> AppraisalPhaseRepository
        {
            get { return _appraisalPhaseRepository ?? (_appraisalPhaseRepository = new Repository<AppraisalPhase>(UnitOfWork)); }
        }

        private IRepository<Grade> _gradeRepository;

        public IRepository<Grade> GradeRepository
        {
            get { return _gradeRepository ?? (_gradeRepository = new Repository<Grade>(UnitOfWork)); }
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