//using HRIS.Domain.Incentive.Entities;
//using HRIS.Domain.JobDescription.Entities;
//using HRIS.Domain.PMS.Entities.Competency;
//using HRIS.Validation.MessageKeys;
//using HRIS.Validation.Specification.Index;
//using Souccar.Domain.DomainModel;
//using SpecExpress;

//namespace HRIS.Validation.Specification.PMS.Entities.Competency
//{
//    public class CompetencySectionSpecification : Validates<AppraisalCompetenceSpecification>
//    {
//        public CompetencySectionSpecification()
//        {
//            IsDefaultForType();

//            #region Primitive Types
//            Check(x => x.Weight).Required().Between(GlobalConstant.MinimumPercentageValue, GlobalConstant.MaximumPercentageValue);
//            Check(x => x.Rate).Required().Between(GlobalConstant.MinimumPercentageValue, GlobalConstant.MaximumPercentageValue);
//            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
//            #endregion Primitive Types

//            #region Indexes

//            #endregion Indexes

//        }
//    }
//}
