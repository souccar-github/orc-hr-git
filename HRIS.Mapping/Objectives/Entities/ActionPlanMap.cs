#region

using System;
using FluentNHibernate.Mapping;
using HRIS.Domain.Objectives.Entities;
using HRIS.Domain.Objectives.Enums;

#endregion

namespace HRIS.Mapping.Objectives.Entities
{
    public sealed class ActionPlanMap : ClassMap<ActionPlan>
    {
        public ActionPlanMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            #region Basic Info

            

            #endregion

            #region Action Plan

            Map(x => x.Description);
            Map(x => x.ExpectedResult);

            References(x => x.Owner);

            Map(x => x.Status);


            Map(x => x.PlannedStartDate);
            Map(x => x.PlannedEndDate);

            Map(x => x.ActualStartDate);
            Map(x => x.ActualEndDate);
            Map(x => x.PercentageOfCompletion);
            Map(x => x.Mark);

            References(x => x.Objective);

            #endregion

        }
    }
}