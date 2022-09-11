#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 03/03/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Validation.MessageKeys;
using SpecExpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
namespace HRIS.Validation.Specification.EmployeeRelationServices.RootEntities
{
    public class EmployeeCardSpecification : Validates<EmployeeCard>
    {
        public EmployeeCardSpecification()
        {
            IsDefaultForType();

            #region Primitive Types
            Check(x => x.StartWorkingDate).Required();
            Check(x => x.ProbationPeriodEndDate).Required();
            Check(x => x.CardStatus).Required();
            Check(x => x.SocialSecurityNo).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength);
            //Check(x => x.SocialSecurityStartingDate).Required();
            
            //Check(x => x.Threshold).Required();
            //Check(x => x.TempSalary1).Required();
            //Check(x => x.TempSalary2).Required();
            Check(x => x.EmployeeMachineCode).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength);

            #endregion
            #region Indexes
            Check(x => x.ContractType)
                   .Required()
                   .Expect((employeeCard, contractType) => contractType.IsTransient() == false, "")
                   .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            
            //Check(x => x.CurrencyType)
            //       .Required()
            //       .Expect((employeeCard, currencyType) => currencyType.IsTransient() == false, "")
            //       .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            Check(x => x.Employee)
                   .Required()
                   .Expect((employeeCard, employee) => employee.IsTransient() == false, "")
                   .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            Check(x => x.EmployeeType)
                   .Required()
                   .Expect((employeeCard, employeeType) => employeeType.IsTransient() == false, "")
                   .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.AttendanceForm).Optional();
                    //.Required()
                    //.Expect((employeeCard, attendanceForm) => attendanceForm.IsTransient() == false, "")
                    //.With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.LatenessForm).Optional();
                    //.Required()
                    //.Expect((employeeCard, latenessForm) => latenessForm.IsTransient() == false, "")
                    //.With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.AbsenceForm).Optional();
                    //.Required()
                    //.Expect((employeeCard, absenceForm) => absenceForm.IsTransient() == false, "")
                    //.With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.OvertimeForm).Optional();
                    //.Required()
                    //.Expect((employeeCard, overtimeForm) => overtimeForm.IsTransient() == false, "")
                    //.With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            #endregion

        }
    }
}
