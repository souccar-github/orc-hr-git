#region

using System.Collections.Generic;
using System.Web.Mvc;
using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.Personnel.Indexes;
using Service;
using UI.Extensions;
using UI.Helpers.Cache;

#endregion

namespace UI.Areas.Personnel.Helpers
{
    public class PersonnelDropDownListHelpers
    {
        #region Entities

        public static SelectList ListOfEmployees
        {
            get
            {
                List<Employee> employees = CacheProvider.Get(PersonnelCacheKeys.Employee.ToString(),
                                                             () => new EntityService<Employee>().GetList());

                return employees.SelectFromList(x => x.Id.ToString(), y => y.FirstName + " " + y.LastName);
            }
        }

        #endregion

        #region Indexes

        public static SelectList ListOfCertificationTypes
        {
            get
            {
                List<CertificationType> certificationTypes =
                    CacheProvider.Get(PersonnelCacheKeys.CertificationType.ToString(),
                                      () => new EntityService<CertificationType>().GetList());

                return certificationTypes.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfReligions
        {
            get
            {
                List<Religion> religions = CacheProvider.Get(PersonnelCacheKeys.Religion.ToString(),
                                                             () => new EntityService<Religion>().GetList());

                return religions.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfMajorTypes
        {
            get
            {
                List<MajorType> majorTypes = CacheProvider.Get(PersonnelCacheKeys.MajorType.ToString(),
                                                               () => new EntityService<MajorType>().GetList());

                return majorTypes.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfGenders
        {
            get
            {
                List<Gender> genders = CacheProvider.Get(PersonnelCacheKeys.Gender.ToString(),
                                                         () => new EntityService<Gender>().GetList());

                return genders.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfBloodTypes
        {
            get
            {
                List<BloodType> bloodTypes = CacheProvider.Get(PersonnelCacheKeys.BloodType.ToString(),
                                                               () => new EntityService<BloodType>().GetList());

                return bloodTypes.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfMilitaryStatuses
        {
            get
            {
                List<MilitaryStatus> militaryStatuses = CacheProvider.Get(PersonnelCacheKeys.MilitaryStatus.ToString(),
                                                                          () =>
                                                                          new EntityService<MilitaryStatus>().GetList());

                return militaryStatuses.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfMaritalStatuses
        {
            get
            {
                List<MaritalStatus> maritalStatuses = CacheProvider.Get(PersonnelCacheKeys.MaritalStatus.ToString(),
                                                                        () =>
                                                                        new EntityService<MaritalStatus>().GetList());

                return maritalStatuses.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfStatuses
        {
            get
            {
                List<Status> statuses = CacheProvider.Get(PersonnelCacheKeys.Status.ToString(),
                                                          () => new EntityService<Status>().GetList());

                return statuses.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfRaces
        {
            get
            {
                List<Race> races = CacheProvider.Get(PersonnelCacheKeys.Race.ToString(),
                                                     () => new EntityService<Race>().GetList());

                return races.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfCountries
        {
            get
            {
                List<Country> countries = CacheProvider.Get(PersonnelCacheKeys.Country.ToString(),
                                                            () => new EntityService<Country>().GetList());

                return countries.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfNationalities
        {
            get
            {
                List<Nationality> nationalities = CacheProvider.Get(PersonnelCacheKeys.Nationality.ToString(),
                                                                    () => new EntityService<Nationality>().GetList());

                return nationalities.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfResidencyTypes
        {
            get
            {
                List<ResidencyType> residencyTypes = CacheProvider.Get(PersonnelCacheKeys.ResidencyType.ToString(),
                                                                       () =>
                                                                       new EntityService<ResidencyType>().GetList());

                return residencyTypes.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfConvictionRules
        {
            get
            {
                List<ConvictionRule> convictionRule = CacheProvider.Get(PersonnelCacheKeys.ConvictionRule.ToString(),
                                                                        () =>
                                                                        new EntityService<ConvictionRule>().GetList());

                return convictionRule.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfDrivingLicenseType
        {
            get
            {
                List<DrivingLicenseType> drivingLicenseType =
                    CacheProvider.Get(PersonnelCacheKeys.DriveingLicenseType.ToString(),
                                      () =>
                                      new EntityService<DrivingLicenseType>().GetList());

                return drivingLicenseType.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfLevels
        {
            get
            {
                List<Level> level =
                    CacheProvider.Get(PersonnelCacheKeys.Level.ToString(),
                                      () =>
                                      new EntityService<Level>().GetList());

                return level.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfSkillTypes
        {
            get
            {
                List<SkillType> skillType =
                    CacheProvider.Get(PersonnelCacheKeys.SkillType.ToString(),
                                      () =>
                                      new EntityService<SkillType>().GetList());

                return skillType.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfMajors
        {
            get
            {
                List<Major> major =
                    CacheProvider.Get(PersonnelCacheKeys.Major.ToString(),
                                      () =>
                                      new EntityService<Major>().GetList());

                return major.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfBoolCondition
        {
            get
            {
                List<BoolCondition> markScale =
                    CacheProvider.Get(PersonnelCacheKeys.BoolCondition.ToString(),
                                      () =>
                                      new EntityService<BoolCondition>().GetList());

                return markScale.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfScoreType
        {
            get
            {
                List<ScoreType> markScale =
                    CacheProvider.Get(PersonnelCacheKeys.ScoreType.ToString(),
                                      () =>
                                      new EntityService<ScoreType>().GetList());

                return markScale.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        #endregion
    }
}