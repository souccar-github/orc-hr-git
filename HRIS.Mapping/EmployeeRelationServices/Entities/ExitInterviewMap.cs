#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 09/03/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference
using FluentNHibernate.Mapping;
using HRIS.Domain.EmployeeRelationServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
namespace HRIS.Mapping.EmployeeRelationServices.Entities
{
    public sealed class ExitInterviewMap : ClassMap<ExitInterview>
    {
        public ExitInterviewMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion


            References(x => x.EmployeeCard).Column("EmployeeCard_Id");
            References(x => x.Interviewer).Column("Interviewer_Id");
            References(x => x.EmployeeResignation).Column("EmployeeResignation_Id");
            References(x => x.EmployeeTermination).Column("EmployeeTermination_Id");
            References(x => x.Creator);
            Map(x => x.InterviewDate);
            HasMany(x => x.InterviewAnswers).LazyLoad().Cascade.AllDeleteOrphan().KeyColumn("SurveyExitInterview_Id");

        }
    }
}
