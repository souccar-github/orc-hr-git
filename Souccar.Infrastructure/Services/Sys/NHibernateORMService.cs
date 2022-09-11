using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Souccar.Core.Services;
using Souccar.Domain.DomainModel;
using Souccar.Domain.PersistenceSupport;
using Souccar.Infrastructure.Exceptions;
using Souccar.NHibernate;
using Souccar.Domain.Audit;
using Souccar.Domain.Security;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.Localization;
using Souccar.Infrastructure.Core;

namespace Souccar.Infrastructure.Services.Sys
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class NHibernateORMService : IORMService
    {
        public IQueryable<T> All<T>() where T : Entity, IAggregateRoot
        {
            var repository = new NHibernateRepository<T>();
            var data = repository.GetAll();
            return data.Any() ? data.Where(x => x.IsVertualDeleted == false) : new List<T>().AsQueryable();
        }
        public IQueryable<T> AllWithVertualDeleted<T>() where T : Entity, IAggregateRoot
        {
            var repository = new NHibernateRepository<T>();
            var data = repository.GetAll();
            return data.Any() ? data : new List<T>().AsQueryable();
        }

        public T GetById<T>(int id) where T : Entity, IAggregateRoot
        {
            var repository = new NHibernateRepository<T>();
            return repository.GetById(id);
        }

        public void Delete<T>(T entity, User user) where T : Entity, IAggregateRoot
        {
            var repository = new NHibernateRepository<T>();
            if (entity == null)
                throw new ORMException("Null pointer exception.");

            var dbContext = repository.DbContext;
            using (dbContext.BeginTransaction())
            {
                if ((entity as Entity).Id == 0)
                {
                    throw new ORMException("Entity not save.");
                }
                else
                {
                    repository.Delete(entity);
                }
                try
                {
                    dbContext.CommitTransaction();
                }
                catch (Exception e)
                {
                    dbContext.RollbackTransaction();
                    throw new ORMException(e.Message);
                }

            }

        }
        public void DeleteTransaction<T>(IList<T> entities, User user) where T : Entity, IAggregateRoot
        {
            var session = NHibernateSession.Current;
            using (session.BeginTransaction())
            {
                foreach (var entity in entities)
                {
                    if (entity != null)
                        session.Delete(entity);
                }
                try
                {
                    session.Transaction.Commit();
                }
                catch (Exception e)
                {
                    session.Transaction.Rollback();
                    throw new ORMException(e.Message);
                }
            }
        }
        public void Save<T>(T entity, User user) where T : Entity, IAggregateRoot
        {
            var repository = new NHibernateRepository<T>();
            if (entity == null)
                throw new ORMException("Null pointer exception.");
            var session = NHibernateSession.Current;
            using (session.BeginTransaction())
            {
                session.SaveOrUpdate(entity);
                try
                {
                    session.Transaction.Commit();
                }
                catch (Exception e)
                {
                    session.Transaction.Rollback();
                    throw new ORMException(e.Message);
                }
            }
        }

        public void SaveTransaction<T>(IList<T> entities, User user, Entity AffectedEntity, OperationType operationType, String Information, DateTime? StartTime, List<Entity> AffectedEntities) where T : class, Souccar.Domain.DomainModel.IAggregateRoot
        {
            var session = NHibernateSession.Current;

            using (session.BeginTransaction())
            {
                foreach (var entity in entities)
                {
                    if (entity != null)
                    {
                        session.SaveOrUpdate(entity);
                    }
                }
                if (AffectedEntity != null || Information != null)
                    session.SaveOrUpdate(getLogForOperation(user, operationType, AffectedEntity, Information, StartTime, AffectedEntities));

                try
                {
                    session.Transaction.Commit();
                }
                catch (Exception e)
                {
                    session.Transaction.Rollback();
                    if (e.InnerException != null)
                        throw new ORMException(e.InnerException.Message);
                    else
                        throw new ORMException(e.Message);
                }
            }
        }



        #region Helper
        private Log getLogForOperation(User user, OperationType operationType, Entity entity, String Information, DateTime? StartTime, List<Entity> AffectedEntities)
        {
            var sperationStr = "";
            var IsWithWorkFlow = false;
            var entityDescription = "";
            TimeSpan Difference = new TimeSpan();
            DateTime EndTimeOfProcess = DateTime.Now;
            if (StartTime != null)
                Difference = (EndTimeOfProcess - StartTime) ?? TimeSpan.Zero;
            else
                Difference = EndTimeOfProcess - EndTimeOfProcess;
            //if Entity is null And you can use information variable to include the namespace of the Service
            if (AffectedEntities != null)
                for (var i = 0; i < AffectedEntities.Count; i++)
                {
                    sperationStr = i == 0 ? "" : "-";
                    var NameForDropdownAffectedProperty = AffectedEntities[i].GetType().GetProperty("NameForDropdown");
                    var NameAffectedProperty = entity.GetType().GetProperty("Name");
                    if (NameForDropdownAffectedProperty != null || NameAffectedProperty != null)
                        entityDescription += NameForDropdownAffectedProperty != null ? sperationStr + (string)NameForDropdownAffectedProperty.GetValue(AffectedEntities[i], null) : sperationStr + (string)NameAffectedProperty.GetValue(entity, null);
                }
            if (entity != null)
            {
                var IsWithworkflowProperty = entity.GetType().GetProperty("IsWithWorkFlow");

                sperationStr = AffectedEntities == null ||
                    (AffectedEntities != null && AffectedEntities.Count == 0) ? "" : "-";
                var NameForDropdownProperty = entity.GetType().GetProperty("NameForDropdown");
                var NameProperty = entity.GetType().GetProperty("Name");
                if (IsWithworkflowProperty != null)
                    IsWithWorkFlow = (bool)IsWithworkflowProperty.GetValue(entity, null);
                if (NameForDropdownProperty != null || NameProperty !=null)
                    entityDescription += NameForDropdownProperty != null ? sperationStr + (string)NameForDropdownProperty.GetValue(entity, null) : sperationStr + (string)NameProperty.GetValue(entity, null);
            }
            if (Information != null)
            {
                return new Log()
                {
                    ClassName = Information,
                    AffecetedRow = entityDescription,
                    OperationType = operationType,
                    Date = DateTime.Today,
                    Time = new DateTime(1900, 1, 1, DateTime.Now.Hour, DateTime.Now.Minute, 0),
                    User = user,
                    IsWithWorkFlow = IsWithWorkFlow,
                    ProcessingPeriod = Math.Round(Difference.TotalMilliseconds, 2)

                };
            }
            return new Log()
            {
                ClassName = getLocalizationNameOfClass(entity),
                AffecetedRow = entityDescription,
                OperationType = operationType,
                Date = DateTime.Today,
                Time = new DateTime(1900, 1, 1, DateTime.Now.Hour, DateTime.Now.Minute, 0),
                User = user,
                IsWithWorkFlow = IsWithWorkFlow,
                ProcessingPeriod = Math.Round(Difference.TotalMilliseconds, 2)
            };
        }
        private string getLocalizationNameOfClass(Entity entity)
        {
            try
            {
                var ResourceName_ = entity.GetType().FullName;
                return ResourceName_;
                //getLocalizationName(ResourceName_);

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }


        #endregion
    }
}
