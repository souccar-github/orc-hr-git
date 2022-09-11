using System;
using HRIS.Domain.Personnel.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;
using HRIS.Domain.Personnel.Enums;

namespace HRIS.Validation.Specification.Personnel.Entities
{
    public class MilitaryServiceSpecification : Validates<MilitaryService>
    {
        public MilitaryServiceSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.Status).Required();
            //Check(x => x.ExemptionReason).If(x => x.Status == MilitaryStatus.Exempt).Required();
            Check(x => x.ExemptionReason).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            //Check(x => x.DateOfExemption).If(x => x.Status == MilitaryStatus.Exempt).Required();

            //Check(x => x.DelayReason).If(x => x.Status == MilitaryStatus.Delayed).Required();
            Check(x => x.DelayReason).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            //Check(x => x.DateOfDelay).If(x => x.Status == MilitaryStatus.Delayed).Required();

            //Check(x => x.MilitiryServiceNo).If(x => 
            //    x.Status == MilitaryStatus.Served || 
            //    x.Status == MilitaryStatus.Hold||
            //    x.Status == MilitaryStatus.Reserve).Required();

            //Check(x => x.MilitiryServiceDocIssuance).If(x =>
            //   x.Status == MilitaryStatus.Served ||
            //   x.Status == MilitaryStatus.Hold ||
            //   x.Status == MilitaryStatus.Reserve).Required();

            //Check(x => x.Granter).If(x =>
            //   x.Status == MilitaryStatus.Served ||
            //   x.Status == MilitaryStatus.Hold ||
            //   x.Status == MilitaryStatus.Reserve).Required();

            //Check(x => x.ServiceStartDate).If(x =>
            //   x.Status == MilitaryStatus.Served ||
            //   x.Status == MilitaryStatus.Hold ||
            //   x.Status == MilitaryStatus.Reserve).Required();

            //Check(x => x.ServiceEndDate).If(x =>
            //   x.Status == MilitaryStatus.Served ||
            //   x.Status == MilitaryStatus.Hold ||
            //   x.Status == MilitaryStatus.Reserve).Required();

            //Check(x => x.Years).If(x =>
            //   x.Status == MilitaryStatus.Served ||
            //   x.Status == MilitaryStatus.Hold ||
            //   x.Status == MilitaryStatus.Reserve).Required();

            //Check(x => x.Months).If(x =>
            //   x.Status == MilitaryStatus.Served ||
            //   x.Status == MilitaryStatus.Hold ||
            //   x.Status == MilitaryStatus.Reserve).Required().Between(1, 12);

            //Check(x => x.Days).If(x =>
            //   x.Status == MilitaryStatus.Served ||
            //   x.Status == MilitaryStatus.Hold ||
            //   x.Status == MilitaryStatus.Reserve).Required().Between(1, 31);

            Check(x => x.HoldDate).If(x => x.Status == MilitaryStatus.Hold).Required();
            Check(x => x.ReserveStartDate).If(x => x.Status == MilitaryStatus.Reserve).Required();


            Check(x => x.Notes).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);


            #endregion

            #region Indexes

            //Check(x => x.Employee.Gender)
            //    .Optional()
            //    .Expect((militaryService, gender) => gender== Domain.Personnel.Enums.Gender.Male, "")
            //    .With(x => x.MessageKey = CustomMessageKeysPersonnelModule.GetFullKey(CustomMessageKeysPersonnelModule.AddFemaleMilitaryService));

            #endregion
        }
    }
}
