#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 05/03/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference
using System;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;
#endregion
namespace HRIS.Validation.Specification.EmployeeRelationServices.Entities
{
    public class ExitInterviewSpecification : Validates<ExitInterview>
    {
        public ExitInterviewSpecification()
        {

            //#region Primitive Types
            //Check(x => x.InterviewDate).Required();
            //Check(x => x.Description).Required();
            //Check(x => x.ReasonLeaving).Required();
            //Check(x => x.EmployeeComment).Required();
            //Check(x => x.InterviewerComment).Required();
            //#endregion

            //#region Indexes
            //Check(x => x.ExitSurvey)
            //    .Required()
            //    .Expect((exitInterview, exitSurvey) => exitSurvey.IsTransient() == false, "")
            //    .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            //#endregion

        }
    }
}
