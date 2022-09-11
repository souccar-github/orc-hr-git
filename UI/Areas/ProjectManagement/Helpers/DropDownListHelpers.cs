#region

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.ProjectManagment.Entities;
using HRIS.Domain.ProjectManagment.Indexes;
using HRIS.Domain.ProjectManagment.ValueObjects;
using Service;
using UI.Extensions;
using UI.Helpers.Cache;

#endregion

namespace UI.Areas.ProjectManagement.Helpers
{
    public class DropDownListHelpers
    {
        #region Entities

        #endregion

        #region Value Objects

        public static SelectList ListOfProjectTeams(int projectId)
        {
            Project project = new EntityService<Project>().LoadById(projectId);

            var teams = new List<ProjectTeam>();

            teams.AddRange(project.ProjectTeams.ToList());

            return teams.SelectFromList(x => x.Id.ToString(), y => y.Name);
        }

        public static SelectList ListOfProjectTeamRoles(int projectTeamId)
        {
            List<ProjectTeamRole> roles;

            if (projectTeamId != 0)
            {
                //var team = new EntityService<ProjectTeam>().LoadById(projectTeamId);

                //roles = team.ProjectTeamRoles.ToList();

                //CacheProvider.GetFromDataSource(ProjectCacheKeys.ProjectTeamRole.ToString(),
                //                                () => roles);

                roles = (List<ProjectTeamRole>)Service.ProjectManagement.ProjectManagementHelper.GetProjectTeamRoles(projectTeamId);
                CacheProvider.GetFromDataSource(ProjectCacheKeys.ProjectTeamRole.ToString(),
                                                () => roles);

            }
            else
            {
                roles = CacheProvider.Get(ProjectCacheKeys.ProjectTeamRole.ToString(),
                                          () =>
                                          new List<ProjectTeamRole>());
            }

            return roles.SelectFromList(x => x.Id.ToString(), y => y.Role.Name);
        }

        public static SelectList ListOfProjectTeamRoles(int projectTeamId, int projectTeamRoleId)
        {
            List<ProjectTeamRole> roles;

            if (projectTeamId != 0)
            {
                //var team = new EntityService<ProjectTeam>().LoadById(projectTeamId);

                //roles = team.ProjectTeamRoles.ToList();

                //roles.RemoveAll(x=> x.Id == projectTeamRoleId);

                //CacheProvider.GetFromDataSource(ProjectCacheKeys.ProjectTeamRole.ToString(),
                //                                () => roles);

                roles = (List<ProjectTeamRole>)Service.ProjectManagement.ProjectManagementHelper.GetProjectTeamRoles(projectTeamId);
                roles.RemoveAll(x => x.Id == projectTeamRoleId);
                CacheProvider.GetFromDataSource(ProjectCacheKeys.ProjectTeamRole.ToString(),
                                                () => roles);

            
            }
            else
            {
                roles = CacheProvider.Get(ProjectCacheKeys.ProjectTeamRole.ToString(),
                                          () =>
                                          new List<ProjectTeamRole>());
            }

            return roles.SelectFromList(x => x.Id.ToString(), y => y.Role.Name);
        }

        public static SelectList ListOfTeamMembers(int projectTeamRoleId)
        {
            List<TeamMember> teams;

            if (projectTeamRoleId != 0)
            {
                //var role = new EntityService<ProjectTeamRole>().LoadById(projectTeamRoleId);

                //teams = role.TeamMembers.ToList();

                //CacheProvider.GetFromDataSource(ProjectCacheKeys.TeamMembers.ToString(),
                //                                () => teams);

                //var role = new EntityService<ProjectTeamRole>().LoadById(projectTeamRoleId);

                teams = (List<TeamMember>)Service.ProjectManagement.ProjectManagementHelper.GetProjectRoleTeamMember(projectTeamRoleId);

                CacheProvider.GetFromDataSource(ProjectCacheKeys.TeamMembers.ToString(),
                                                () => teams);
            }
            else
            {
                teams = CacheProvider.Get(ProjectCacheKeys.TeamMembers.ToString(),
                                          () =>
                                          new List<TeamMember>());
            }
            return teams.SelectFromList(x => x.Id.ToString(), y => y.Employee.FirstName);
        }

        #endregion

        #region Indexes

        public static SelectList ListOfProjectType
        {
            get
            {
                List<ProjectType> types = CacheProvider.Get(ProjectCacheKeys.ProjectType.ToString(),
                                                            () =>
                                                            new EntityService<ProjectType>().GetList());

                return types.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfProjectResourceType
        {
            get
            {
                List<ProjectResourceType> types = CacheProvider.Get(ProjectCacheKeys.ProjectResourceType.ToString(),
                                                                    () =>
                                                                    new EntityService<ProjectResourceType>().GetList());

                return types.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfResourceStatus
        {
            get
            {
                List<ResourceStatus> status = CacheProvider.Get(ProjectCacheKeys.ResourceStatus.ToString(),
                                                                () =>
                                                                new EntityService<ResourceStatus>().GetList());

                return status.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfProjectKpiType
        {
            get
            {
                List<ProjectKpiType> projectKpiType = CacheProvider.Get(ProjectCacheKeys.ProjectKpiType.ToString(),
                                                                        () =>
                                                                        new EntityService<ProjectKpiType>().GetList());

                return projectKpiType.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfProjectRoleName
        {
            get
            {
                List<ProjectRole> projectRoles = CacheProvider.Get(ProjectCacheKeys.ProjectRole.ToString(),
                                                                   () =>
                                                                   new EntityService<ProjectRole>().GetList());

                return projectRoles.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfProjectRoleNameWithEmptyValue
        {
            get
            {
                List<ProjectRole> projectRoles = CacheProvider.Get(ProjectCacheKeys.ProjectRole.ToString(),
                                                                   () =>
                                                                   new EntityService<ProjectRole>().GetList());

                var listWithEmptyItem = new List<ProjectRole> {new ProjectRole()};

                listWithEmptyItem.AddRange(projectRoles);

                return listWithEmptyItem.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfPhaseStatus
        {
            get
            {
                List<PhaseStatus> phaseStatus = CacheProvider.Get(ProjectCacheKeys.PhaseStatus.ToString(),
                                                                  () =>
                                                                  new EntityService<PhaseStatus>().GetList());

                return phaseStatus.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        #endregion
    }
}