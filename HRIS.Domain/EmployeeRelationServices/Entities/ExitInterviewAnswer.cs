#region Namespace Reference

using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace HRIS.Domain.EmployeeRelationServices.Entities
{
    [Module(ModulesNames.EmployeeRelationServices)]
    public class ExitInterviewAnswer : Entity
    {
        [UserInterfaceParameter(Order = 10)]
        public virtual ExitInterview SurveyExitInterview { get; set; }

        [UserInterfaceParameter(Order = 20)]
        public virtual ExitSurveyItem ExitSurveyItem { get; set; }

        [UserInterfaceParameter(Order = 25)]
        public virtual String ExitSurveyItemName {
            get
            {
                return ExitSurveyItem.Name;
            }        
        }

        [UserInterfaceParameter(Order = 26)]
        public virtual String ExitSurveyItemDescription
        {
            get
            {
                return ExitSurveyItem.Description;
            }
        } 

        [UserInterfaceParameter(Order = 30)]
        public virtual string EmployeeAnswer { get; set; }

        [UserInterfaceParameter(Order = 40)]
        public virtual string InterviewerComment { get; set; }
    }
}
