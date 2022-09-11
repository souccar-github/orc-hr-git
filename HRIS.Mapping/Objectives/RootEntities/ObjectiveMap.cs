#region

using FluentNHibernate.Mapping;
using HRIS.Domain.Objectives.Entities;
using HRIS.Domain.Objectives.RootEntities;

#endregion

namespace HRIS.Mapping.Objectives.Entities
{
    public sealed class ObjectiveMap : ClassMap<Objective>
    {
        public ObjectiveMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            #region Basic Info

          
            #endregion

            #region Abstract Basic Info

            Map(x => x.Name);
            Map(x => x.Description);
            Map(x => x.Code);
            Map(x => x.Status);


            Map(x => x.DoesNotMeetExpectation);
            Map(x => x.MeetExpectation);
            Map(x => x.AboveExpectation);

            #endregion


            #region Objective Info
            References(x => x.Creator);
            Map(x => x.CreationDate);

            References(x => x.Department);

            Map(x => x.Type);

            References(x => x.Owner);
            References(x => x.JopDescription);

            Map(x => x.Priority);

            Map(x => x.Weight);

            References(x => x.ParentObjective).Column("ParentObjective_id");


            Map(x => x.PlannedStartingDate);
            Map(x => x.PlannedClosingDate);

            References(x => x.StrategicObjective);

            #region list

            HasMany(x => x.SharedWiths).Inverse().LazyLoad().Cascade.AllDeleteOrphan().KeyColumn("Objective_Id");
           
            HasMany(x => x.Kpis).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            HasMany(x => x.Constraints).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            HasMany(x => x.ActionPlans).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            
            HasMany(x => x.Objectives).Inverse().LazyLoad().Cascade.AllDeleteOrphan().KeyColumns.Add("ParentObjective_id");

            #endregion
            
           #region creation
            References(x => x.CreationPhase);
            References(x => x.CreationWorkflow);
       
            #endregion
            References(x => x.EvaluationWorkflow);

            //Objective phase workflow.
            //HasMany(x => x.ObjectivePhaseWorkflows).Inverse().LazyLoad().Cascade.AllDeleteOrphan();Cause an exception at edit.
            //HasMany(x => x.ObjectivePhaseWorkflows).Cascade.Delete().Inverse().AsBag();Cause an exception at delete.

            //Objective evaluation workflow.
            //HasMany(x => x.ObjectiveEvaluationWorkflows).Inverse().LazyLoad().Cascade.AllDeleteOrphan();Cause an exception at edit.
            //HasMany(x => x.ObjectiveEvaluationWorkflows).Cascade.Delete().Inverse().AsBag();Cause an exception at delete.

            #endregion
            Map(x => x.IsFinishedAndEvaluated);



        }
    }
}