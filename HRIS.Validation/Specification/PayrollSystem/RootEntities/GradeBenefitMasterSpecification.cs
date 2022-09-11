//using HRIS.Domain.PayrollSystem.RootEntities;
//using HRIS.Validation.MessageKeys;
//using Souccar.Infrastructure.Extenstions;
//using SpecExpress;

//namespace HRIS.Validation.Specification.PayrollSystem.RootEntities
//{
//    public class GradeBenefitMasterSpecification : Validates<GradeBenefitMaster>
//    {
//        public GradeBenefitMasterSpecification()
//        {
//            IsDefaultForType();

//            Check(x => x.Grade, y => typeof(GradeBenefitMaster).GetProperty("Grade").GetTitle())
//                .Required()
//                .Expect((gradeBenefitMaster, grade) => grade.IsTransient() == false, "")
//                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
//        }
//    }

//}
