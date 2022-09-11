using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Souccar.Core.Fasterflect;
using Souccar.Core.Services;
using Souccar.Domain.Localization;
using Souccar.Domain.PersistenceSupport;
using Souccar.Domain.Security;
using Souccar.NHibernate;



namespace Souccar.Infrastructure.Services.Sys
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class SecurityService : ISecurityService
    {
        public IList<Role> Roles
        {
            get
            {
                var repository = new NHibernateRepository<Role>();
                return repository.GetAll().Where(x => !x.IsVertualDeleted).ToList();
            }
        }

        public IList<User> Users
        {
            get
            {
                var repository = new NHibernateRepository<User>();
                return repository.GetAll().ToList();
            }
        }

        public IList<Role> RolesForUser(string userName)
        {
            var repository = new NHibernateRepository<User>();
            var user = repository.GetAll().SingleOrDefault(x=>x.Username.Equals(userName));
            return user == null ? null : user.Roles.Select(x=>x.Role).ToList();
        }
       
        public IList<Role> RolesForUser(int userId)
        {
            var repository = new NHibernateRepository<User>();
            var user = repository.GetById(userId);
            return user == null ? null : user.Roles.Select(x=>x.Role).ToList();
        }

        public IList<AuthorizableElementRole> GetAuthorizeTypeRolesForElement(string elementName)
        {
            var repository = new NHibernateRepository<AuthorizableElementRole>();

            return repository.GetAll().Any()
                ? repository.GetAll().Where(x => x.AuthorizableElementId.Equals(elementName)).ToList()
                : new List<AuthorizableElementRole>();
        }

        public IList<AuthorizableElementRole> GetAuthorizeTypeRolesForRole(string roleName)
        {
            var repository = new NHibernateRepository<AuthorizableElementRole>();
            return repository.GetAll().Where(x => x.Role.Name.Equals(roleName)).ToList();
        }
        public IList<AuthorizableFieldRole> GetAuthorizeTypeRolesForField(string elementName, string aggregatName, string modelName)
        {
            var repository = new NHibernateRepository<AuthorizableFieldRole>();

            return repository.GetAll().Any()
                ? repository.GetAll().Where(x => x.AuthorizableElementId.Equals(aggregatName) && x.AuthorizableFieldId.Equals(elementName) && x.ModuleName == modelName).ToList()
                : new List<AuthorizableFieldRole>();
        }
        public IList<AuthorizableFieldRole> GetAuthorizeFieldTypeRolesForRole(string roleName)
        {
            var repository = new NHibernateRepository<AuthorizableFieldRole>();
            return repository.GetAll().Where(x => x.Role.Name.Equals(roleName)).ToList();
        }
        public IList<AuthorizableDetailsFieldRole> GetAuthorizeTypeRolesForDetailsField(string elementName, string aggregatName, string modelName)
        {
            var repository = new NHibernateRepository<AuthorizableDetailsFieldRole>();
            return repository.GetAll().Any()
                ? repository.GetAll().Where(x => x.AuthorizableElementId.Equals(aggregatName) && x.AuthorizableFieldId.Equals(elementName) && x.ModuleName == modelName).ToList()
                : new List<AuthorizableDetailsFieldRole>();
        }

        public IList<AuthorizableDetailsFieldRole> GetAuthorizeFieldDetailsTypeRolesForRole(string roleName)
        {
            var repository = new NHibernateRepository<AuthorizableDetailsFieldRole>();
            return repository.GetAll().Where(x => x.Role.Name.Equals(roleName)).ToList();
        }

        public User GetUserByUsername(string username)
        {
            var repository = new NHibernateRepository<User>();
            return repository.GetAll().SingleOrDefault(x => x.Username.Equals(username));
        }

        public void AddRoleToUser(string userName, string roleName)
        {
            var repository = new NHibernateRepository<User>();
            var user = repository.GetAll().SingleOrDefault(x => x.Username.Equals(userName));
            var role = Roles.SingleOrDefault(x => x.Name.Equals(roleName));
            user.AddRole(role);
            commitTransaction(repository.DbContext);
        }

        public void AddRoleToUser(int userId, int roleId)
        {
            var repository = new NHibernateRepository<User>();
            var user = repository.GetById(userId);
            var role = Roles.SingleOrDefault(x => x.Id.Equals(roleId));
            user.AddRole(role);
            commitTransaction(repository.DbContext);
        }

        public void RemoveRoleFromUser(string userName, string roleName)
        {
            var repository = new NHibernateRepository<User>();
            var user = repository.GetAll().SingleOrDefault(x => x.Username.Equals(userName));
            var role = user.Roles.SingleOrDefault(x => x.Role.Name.Equals(roleName)).Role;
            user.RemoveRole(role);
            commitTransaction(repository.DbContext);
        }

        public void RemoveRoleFromUser(int userId, int roleId)
        {
            var repository = new NHibernateRepository<User>();
            var user = repository.GetById(userId);
            var role = user.Roles.SingleOrDefault(x => x.Role.Id.Equals(roleId)).Role;
            user.RemoveRole(role);
            commitTransaction(repository.DbContext);
        }


        public void SetRolesToUser(string userName, IList<Role> roles)
        {
            var repository = new NHibernateRepository<User>();
            var user = repository.GetAll().SingleOrDefault(x => x.Username.Equals(userName));
            while (user.Roles.Any())
            {
                user.RemoveRole(user.Roles.First().Role);
            }
            foreach (var role in roles)
            {
                user.Roles.Add(new UserRole() { User = user, Role = role });
            }
            commitTransaction(repository.DbContext);
        }

        public void SetRolesToUser(int userId, IList<Role> roles)
        {
            var repository = new NHibernateRepository<User>();
            var user = repository.GetById(userId);
            while (user.Roles.Any())
            {
                user.RemoveRole(user.Roles.First().Role);
            }

            foreach (var role in roles)
            {
                user.Roles.Add(new UserRole() { User = user, Role = role });
            }
            commitTransaction(repository.DbContext);
        }

        private void commitTransaction(IDbContext dbContext)
        {
            using (dbContext.BeginTransaction())
            {
                dbContext.CommitTransaction();
            }
        }
    }
}
