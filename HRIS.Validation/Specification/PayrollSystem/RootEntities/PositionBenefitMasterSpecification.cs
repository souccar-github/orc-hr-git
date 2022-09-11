//using HRIS.Domain.PayrollSystem.RootEntities;
//using HRIS.Validation.MessageKeys;
//using Souccar.Infrastructure.Extenstions;
//using SpecExpress;

//namespace HRIS.Validation.Specification.PayrollSystem.RootEntities
//{
//    public class PositionBenefitMasterSpecification : Validates<PositionBenefitMaster>
//    {
//        public PositionBenefitMasterSpecification()
//        {
//            IsDefaultForType();

//            Check(x => x.Position, y => typeof(PositionBenefitMaster).GetProperty("Position").GetTitle())
//                .Required()
//                .Expect((positionBenefitMaster, position) => position.IsTransient() == false, "")
//                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
//        }
//    }

//}
