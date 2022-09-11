using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Souccar.Domain.Security;

namespace Souccar.Core.Services
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public interface ISecurityService : IService
    {
        IList<Role> Roles { get; }
        IList<User> Users { get; }

        IList<Role> RolesForUser(string userName);
        IList<Role> RolesForUser(int userId);

        User GetUserByUsername(string username);

        void AddRoleToUser(string userName, string roleName);
        void AddRoleToUser(int userId, int roleId);
        
        void RemoveRoleFromUser(string userName, string roleName);
        void RemoveRoleFromUser(int userId, int roleId);

        void SetRolesToUser(string userName, IList<Role> roles);
        void SetRolesToUser(int userId, IList<Role> roles);

        IList<AuthorizableElementRole> GetAuthorizeTypeRolesForElement(string elementName);
        IList<AuthorizableElementRole> GetAuthorizeTypeRolesForRole(string roleName);
        IList<AuthorizableFieldRole> GetAuthorizeTypeRolesForField(string elementName, string aggregatName, string modelName);
        IList<AuthorizableFieldRole> GetAuthorizeFieldTypeRolesForRole(string roleName);
        IList<AuthorizableDetailsFieldRole> GetAuthorizeTypeRolesForDetailsField(string elementName, string aggregatName, string modelName);
        IList<AuthorizableDetailsFieldRole> GetAuthorizeFieldDetailsTypeRolesForRole(string roleName);

    }
}
