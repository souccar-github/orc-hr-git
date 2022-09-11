#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.Objectives.Enums;
using HRIS.Domain.Objectives.Indexes;
using HRIS.Domain.Personnel.Entities;
using Repository.NHibernate;
using Service;
using UI.Extensions;
using UI.Helpers.Cache;

#endregion

namespace UI.Areas.Objective.Helpers
{
    public class DropDownListHelpers
    {
        #region Entities

        public static SelectList ListOfObjectives
        {
            get
            {
                List<HRIS.Domain.Objectives.RootEntities.Objective> objectives =
                    CacheProvider.Get(ObjectiveCacheKeys.Objective.ToString(),
                                      () =>
                                      new EntityService<HRIS.Domain.Objectives.RootEntities.Objective>().GetList());

                return objectives.SelectFromList(x => x.Id.ToString(), y => y.Description);
            }
        }

        public static SelectList ListOfPossibleStepOwners(int objectiveId)
        {
            var owners = new List<Employee>();

            #region Fill List

            var objective = new EntityService<HRIS.Domain.Objectives.RootEntities.Objective>().LoadById(objectiveId);

            //if (objective.Owner != null);

            //if (objective.SharedWiths.Count != 0)
            //    owners.AddRange(objective.SharedWiths.Select(sharedWith => sharedWith.Employee));

            #endregion

            return owners.SelectFromList(x => x.Id.ToString(), y => y.FirstName + " " + y.LastName);
        }

        #endregion

        #region Indexes

        public static SelectList ListOfDimensions
        {
            get
            {
                List<Dimension> dimensions = CacheProvider.Get(ObjectiveCacheKeys.ObjectiveDimension.ToString(),
                                                               () =>
                                                               new Repository<Dimension>().GetAll().ToList());
                return dimensions.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfObjectivePeriods
        {
            get
            {
                var periods = Enum.GetNames(typeof(Period)).ToList();
                return periods.SelectFromList(x => x.ToString(), y => y.ToString());
            }
        }


        public static SelectList ListOfObjectiveTypes()
        {



            var names = Enum.GetNames(typeof(ObjectiveType)).ToList();

            return names.SelectFromList(x => x.ToString(), y => y.ToString());


        }

        public static SelectList ListOfObjectiveKpiType
        {
            get
            {   
                List<ObjectiveKpiType> types = CacheProvider.Get(ObjectiveCacheKeys.ObjectiveKpiType.ToString(),
                                                                 () =>
                                                                 new EntityService<ObjectiveKpiType>().GetList());

                return types.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfStepStatus
        {
            get
            {
                //List<StepStatus> types = CacheProvider.Get(ObjectiveCacheKeys.StepStatus.ToString(),
                //                                           () =>
                //                                           new EntityService<StepStatus>().GetList());

                //return types.SelectFromList(x => x.Id.ToString(), y => y.Name);
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}