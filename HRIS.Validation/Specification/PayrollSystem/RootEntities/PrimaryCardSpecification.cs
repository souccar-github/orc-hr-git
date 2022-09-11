//using HRIS.Domain.PayrollSystem.RootEntities;
//using HRIS.Validation.MessageKeys;
//using Souccar.Infrastructure.Extenstions;
//using SpecExpress;

//namespace HRIS.Validation.Specification.PayrollSystem.RootEntities
//{
//    public class PrimaryCardSpecification : Validates<PrimaryCard>
//    {
//        public PrimaryCardSpecification()
//        {
//            IsDefaultForType();

//            Check(x => x.Salary, y => typeof(PrimaryCard).GetProperty("Salary").GetTitle()).Optional().GreaterThanEqualTo(0);
//            Check(x => x.InsuranceSalary, y => typeof(PrimaryCard).GetProperty("InsuranceSalary").GetTitle()).Optional().GreaterThanEqualTo(0);
//            Check(x => x.TempSalary1, y => typeof(PrimaryCard).GetProperty("TempSalary1").GetTitle()).Optional().GreaterThanEqualTo(0);
//            Check(x => x.TempSalary2, y => typeof(PrimaryCard).GetProperty("TempSalary2").GetTitle()).Optional().GreaterThanEqualTo(0);
//            Check(x => x.BenefitSalary, y => typeof(PrimaryCard).GetProperty("BenefitSalary").GetTitle()).Required().GreaterThan(0);
//            Check(x => x.Threshold, y => typeof(PrimaryCard).GetProperty("Threshold").GetTitle()).Optional().GreaterThanEqualTo(0);


//            Check(x => x.Employee, y => typeof(PrimaryCard).GetProperty("Employee").GetTitle())
//               .Required()
//               .Expect((primaryCard, employee) => employee.IsTransient() == false, "")
//               .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
//            //Check(x => x.NominationSystem, y => typeof(PrimaryCard).GetProperty("NominationSystem").GetTitle())
//            //        .Required()
//            //        .Expect((primaryCard, nominationSystem) => nominationSystem.IsTransient() == false, "")
//            //        .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
       
//        }
//    }
//}
