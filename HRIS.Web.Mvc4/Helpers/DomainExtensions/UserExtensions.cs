using HRIS.Domain.Personnel.RootEntities;
using Souccar.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Infrastructure.Core;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.Security.Configuration;
using Souccar.Infrastructure.Extenstions;

namespace Project.Web.Mvc4.Helpers.DomainExtensions { 

    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
public static class UserExtensions
    {
        public static Employee Employee(this User user)
        {
            return EmployeeExtensions.GetEmployeeByUsername(user.Username);
        }

        public static Position GetPrimaryPosition(this User user)
        {
            if (user.Employee() == null)
                return null;
            var assigningEmployeeToPosition = user.Employee().Positions.FirstOrDefault(x => x.IsPrimary);
            if (assigningEmployeeToPosition != null)
                return assigningEmployeeToPosition.Position;
            return null;
        }
        public static Position GetSecondaryPositionElsePrimary(this User user)
        {
            if (user.Employee().Positions.Any(x => x.IsPrimary == false))
                return user.Employee().Positions.LastOrDefault(x => x.IsPrimary == false).Position;
            return user.Employee().PrimaryPosition();
        }
        public static string FullName()
        {
            return EmployeeExtensions.CurrentEmployee == null ? null : EmployeeExtensions.CurrentEmployee.FullName;

        }

        public static string PhotoId()
        {
            return EmployeeExtensions.CurrentEmployee == null ? null : EmployeeExtensions.CurrentEmployee.PhotoId;

        }

        public static Position Position(this User user)
        {
            return user.Employee().PrimaryPosition();
        }
        public static User GetUserByUsername(string username)
        {
            return ServiceFactory.ORMService.All<User>().SingleOrDefault(x => x.Username.Equals(username));
        }

        public static User GetManager(this User user)
        {
            var emp = user.Employee();
            if (emp == null)
                return null;
            var manager= emp.Manager();
            if (manager == null)
                return null;
            return manager.User();
        }

        public static Employee GetManagerAsEmployee(this User user)
        {
            var emp = user.Employee();
            if (emp == null)
                return null;
            return emp.Manager();
        }

        public static Position GetManagerAsPosition(this User user)
        {
            var emp = user.Employee();
            if (emp == null)
                return null;
            return emp.ManagerAsPosition();
        }

        /// <summary>
        /// Return  The used in this session
        /// </summary>
        public static User CurrentUser
        {
            get
            {
                if (HttpContext.Current==null || !HttpContext.Current.User.Identity.IsAuthenticated)
                    return null;
                return ServiceFactory.ORMService.All<User>().SingleOrDefault(x=>x.Username== HttpContext.Current.User.Identity.Name);
            }

        }

        public static bool IsCurrentUserFullAuthorized(string elementId = "")
        {
            try
            {
                var userRoles = CurrentUser.Roles.Select(x => x.Role).ToList();
                var allFullAuthorityRoles = ServiceFactory.ORMService.All<FullAuthorityRole>().ToList();
                if (allFullAuthorityRoles.Count == 0 || allFullAuthorityRoles.Any(x => userRoles.Any(y => y.Id == x.Role.Id)))
                {
                    if(string.IsNullOrEmpty(elementId))
                        return true;
                    var matchedRoles = allFullAuthorityRoles.Where(x => userRoles.Any(y => y.Id == x.Role.Id)).ToList();
                    foreach (var role in matchedRoles)
                    {
                        List<AuthorizableElementRole> authorizableElements = typeof(AuthorizableElementRole).GetAll<AuthorizableElementRole>().Where(x => x.Role.Id == role.Role.Id).ToList();
                        if(authorizableElements.Any(x => x.AuthorizableElementId == elementId))
                            return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static bool CheckIfCurrentUserIsAuthorized(string elementId , AuthorizeType authorizeType)
        {
            try
            {
                var roles = CurrentUser.Roles.Select(x => x.Role).ToList();

                foreach (var role in roles)
                {
                    List<AuthorizableElementRole> authorizableElements = typeof(AuthorizableElementRole).GetAll<AuthorizableElementRole>().Where(x => x.Role.Id == role.Id).ToList();
                    if (authorizableElements.Any(x => x.AuthorizableElementId == elementId && x.AuthorizeType == authorizeType))
                        return true;
                }
                return false;
            }
            catch (Exception ex)
            {

                return false;
            }
            
        }

        public static List<Employee> GetChildrenAsEmployess(User user)
        {
            var employees = new List<Employee>();
            try
            {
                var empOfUser = user.Employee();
                if (empOfUser == null)
                    return new List<Employee>();
                employees.Add(empOfUser);
                var positions = empOfUser.Positions;
                if (positions.Any())
                    foreach (var pos in positions)
                    {
                        var childPositions = ServiceFactory.ORMService.All<Position>().ToList()
                               .Where(x => x.Manager == pos.Position && x.AssigningEmployeeToPosition != null);
                        var emps = childPositions.Select(x => x.AssigningEmployeeToPosition.Employee).ToList();
                        employees.AddRange(emps);
                        foreach (var childPosition in childPositions)
                        {
                            var childEmps =  GetChildrenAsEmployess(childPosition.User());
                            employees.AddRange(childEmps);
                        }

                    }

                return employees;
            }
            catch(Exception ex)
            {
                return new List<Employee>();
            }
        }
        public static string CurrentUserTheming
        {
            get
            {
                if (CurrentUser!=null)
                    return CurrentUser.ThemingType.ToString().ToLower();
                var httpContext = new HttpContextWrapper(HttpContext.Current);

                var myCookie = httpContext.Request.Cookies["Theming"];

                // Read the cookie information and display it.
                if (myCookie != null)
                {
                    return myCookie.Value;
                }
                return ThemingHelper.DefaultTheme.ToString().ToLower();
            }

        }
        public static ThemingType CurrentUserThemingType
        {
            get
            {
                if (CurrentUser != null)
                    return CurrentUser.ThemingType;
               
                return ThemingHelper.DefaultTheme;
            }

        }

        public static object CurrentUserSouccar { get; internal set; }
    }
}