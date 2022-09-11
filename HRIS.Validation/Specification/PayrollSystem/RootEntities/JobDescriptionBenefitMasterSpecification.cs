//using HRIS.Domain.PayrollSystem.RootEntities;
//using HRIS.Validation.MessageKeys;
//using Souccar.Infrastructure.Extenstions;
//using SpecExpress;

//namespace HRIS.Validation.Specification.PayrollSystem.RootEntities
//{
//    public class JobDescriptionBenefitMasterSpecification : Validates<JobDescriptionBenefitMaster>
//    {
//        public JobDescriptionBenefitMasterSpecification()
//        {
//            IsDefaultForType();

//            Check(x => x.JobDescription, y => typeof(JobDescriptionBenefitMaster).GetProperty("JobDescription").GetTitle())
//                .Required()
//                .Expect((jobDescriptionBenefitMaster, jobDescription) => jobDescription.IsTransient() == false, "")
//                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
//        }
//    }

//}
