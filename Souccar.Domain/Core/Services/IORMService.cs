using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Security;
using Souccar.Domain.Audit;

namespace Souccar.Core.Services
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public interface IORMService : IService
    {
        IQueryable<T> All<T>() where T : Entity, IAggregateRoot;
        IQueryable<T> AllWithVertualDeleted<T>() where T : Entity, IAggregateRoot;
        T GetById<T>(int id) where T : Entity, IAggregateRoot;
        void Save<T>(T entity, User user) where T : Entity, IAggregateRoot;
        void SaveTransaction<T>(IList<T> entities, User user, Entity AffectedEntity = null, OperationType operationType = OperationType.Insert, String Information = null, DateTime? StartTime = null, List<Entity> AffectedEntities = null) where T : class, IAggregateRoot;
        void Delete<T>(T entity, User user) where T : Entity, IAggregateRoot;
        void DeleteTransaction<T>(IList<T> entities, User user) where T : Entity, IAggregateRoot;
    }
}
