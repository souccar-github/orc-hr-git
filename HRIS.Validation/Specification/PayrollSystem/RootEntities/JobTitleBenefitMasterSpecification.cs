//using HRIS.Domain.PayrollSystem.RootEntities;
//using HRIS.Validation.MessageKeys;
//using Souccar.Infrastructure.Extenstions;
//using SpecExpress;

//namespace HRIS.Validation.Specification.PayrollSystem.RootEntities
//{
//    public class JobTitleBenefitMasterSpecification : Validates<JobTitleBenefitMaster>
//    {
//        public JobTitleBenefitMasterSpecification()
//        {
//            IsDefaultForType();

//            Check(x => x.JobTitle, y => typeof(JobTitleBenefitMaster).GetProperty("JobTitle").GetTitle())
//                .Required()
//                .Expect((jobTitleBenefitMaster, jobTitle) => jobTitle.IsTransient() == false, "")
//                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
//        }
//    }

//}
