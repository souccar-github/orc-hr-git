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
    public sealed class ExitInterviewAnswerMap : ClassMap<ExitInterviewAnswer>
    {
        public ExitInterviewAnswerMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.SurveyExitInterview).Column("SurveyExitInterview_Id");
            References(x => x.ExitSurveyItem).Column("ExitSurveyItem_Id");
            Map(x => x.EmployeeAnswer);
            Map(x => x.InterviewerComment);

        }
    }
}
