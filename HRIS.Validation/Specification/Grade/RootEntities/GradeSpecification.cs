using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Grade.RootEntities
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class GradeSpecification : Validates<Domain.Grades.RootEntities.Grade>
    {
        public GradeSpecification()
        {
            IsDefaultForType();

            #region Primitive types

            Check(x => x.Name).Required();
            Check(x => x.MinSalary).Required().GreaterThanEqualTo(0);
            Check(x => x.MaxSalary).Required().GreaterThanEqualTo(x => x.MinSalary);
            //Check(x => x.PayGroup).Required();

            #endregion

            #region Indexs

            Check(x => x.OrganizationalLevel)
                  .Required()
                  .Expect((grade, organizationalLevel) => organizationalLevel.IsTransient() == false, "")
                  .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.CurrencyType)
                   .Required()
                   .Expect((grade, currencyType) => currencyType.IsTransient() == false, "")
                   .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            
                 

            Check(x => x.AttendanceForm).Optional();
                  //.Required()
                  //.Expect((grade, attendanceForm) => attendanceForm.IsTransient() == false, "")
                  //.With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.LatenessForm).Optional();
                  //.Required()
                  //.Expect((grade, latenessForm) => latenessForm.IsTransient() == false, "")
                  //.With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.OvertimeForm).Optional();
                  //.Required()
                  //.Expect((grade, overtimeForm) => overtimeForm.IsTransient() == false, "")
                  //.With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.AbsenceForm).Optional();
                  //.Required()
                  //.Expect((grade, absenceForm) => absenceForm.IsTransient() == false, "")
                  //.With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion
        }
    }
}
