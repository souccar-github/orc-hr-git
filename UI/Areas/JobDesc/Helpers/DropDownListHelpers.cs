#region

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.JobDesc.Entities;
using HRIS.Domain.JobDesc.Indexes;
using HRIS.Domain.OrgChart.ValueObjects;
using Service;
using UI.Extensions;
using UI.Helpers.Cache;

#endregion

namespace UI.Areas.JobDesc.Helpers
{
    public class DropDownListHelpers
    {
        #region Entities

        public static SelectList ListOfJobDescriptions
        {
            get
            {
                List<JobDescription> jobDescriptions = new EntityService<JobDescription>().GetList();

                return jobDescriptions.SelectFromList(x => x.Id.ToString(), y => y.JobTitle.Name);
            }
        }

        public static SelectList ListOfRoles(int jobDescriptionId)
        {
            JobDescription jobDescription = new EntityService<JobDescription>().LoadById(jobDescriptionId);

            return jobDescription.Roles.ToList().SelectFromList(x => x.Id.ToString(), y => y.Name);
        }

        public static SelectList ListOfPositionRoles(int positionId)
        {

            Position position = new EntityService<Position>().GetById(positionId);

            if (position == null)
            {
                return new SelectList(new List<SelectListItem>());
            }

            JobDescription jobDescription = new EntityService<JobDescription>().GetAll().SingleOrDefault(x => x.JobTitle.Id == position.JobTitle.Id);

            if (jobDescription == null)
            {
                return new SelectList(new List<SelectListItem>());
            }

            return jobDescription.Roles.ToList().SelectFromList(x => x.Id.ToString(), y => y.Name);
        }

        public static SelectList ListOfPositionAuthorities(int positionId)
        {
            Position position = new EntityService<Position>().GetById(positionId);

            if (position == null)
            {
                return new SelectList(new List<SelectListItem>());
            }

            JobDescription jobDescription = new EntityService<JobDescription>().GetAll().SingleOrDefault(x => x.JobTitle.Id == position.JobTitle.Id);

            if (jobDescription == null)
            {
                return new SelectList(new List<SelectListItem>());
            }

            return jobDescription.Authorities.ToList().SelectFromList(x => x.Id.ToString(), y => y.Title);
        }

        public static SelectList ListOfAuthorities(int jobDescriptionId)
        {
            JobDescription jobDescription = new EntityService<JobDescription>().LoadById(jobDescriptionId);

            return jobDescription.Authorities.ToList().SelectFromList(x => x.Id.ToString(), y => y.Title);
        }

        #endregion

        #region Indexes

        public static SelectList ListOfCompetencyType
        {
            get
            {
                List<CompetencyType> competencies = CacheProvider.Get(JobDescCacheKeys.CompetencyType.ToString(),
                                                                      () =>
                                                                      new EntityService<CompetencyType>().GetList());

                return competencies.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfAuthorityType
        {
            get
            {
                List<AuthorityType> authorityType = CacheProvider.Get(JobDescCacheKeys.AuthorityType.ToString(),
                                                                      () => new EntityService<AuthorityType>().GetList());

                return authorityType.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfCareerLevel
        {
            get
            {
                List<CareerLevel> careerLevel = CacheProvider.Get(JobDescCacheKeys.CareerLevel.ToString(),
                                                                  () => new EntityService<CareerLevel>().GetList());

                return careerLevel.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfComputerSkillType
        {
            get
            {
                List<ComputerSkillType> computerSkillType =
                    CacheProvider.Get(JobDescCacheKeys.ComputerSkillType.ToString(),
                                      () => new EntityService<ComputerSkillType>().GetList());

                return computerSkillType.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfConditionType
        {
            get
            {
                List<ConditionType> conditionType = CacheProvider.Get(JobDescCacheKeys.ConditionType.ToString(),
                                                                      () => new EntityService<ConditionType>().GetList());

                return conditionType.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfPriorities
        {
            get
            {
                List<Priority> priority = CacheProvider.Get(JobDescCacheKeys.Priority.ToString(),
                                                            () => new EntityService<Priority>().GetList());

                return priority.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfLanguageName
        {
            get
            {
                List<LanguageName> languageName = CacheProvider.Get(JobDescCacheKeys.LanguageName.ToString(),
                                                                    () => new EntityService<LanguageName>().GetList());

                return languageName.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        #endregion
    }
}