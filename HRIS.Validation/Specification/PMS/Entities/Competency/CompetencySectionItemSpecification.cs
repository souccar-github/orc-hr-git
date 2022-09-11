//using HRIS.Domain.Incentive.Entities;
//using HRIS.Domain.JobDescription.Entities;
//using HRIS.Domain.PMS.Entities.Competency;
//using HRIS.Validation.MessageKeys;
//using HRIS.Validation.Specification.Index;
//using Souccar.Domain.DomainModel;
//using SpecExpress;

//namespace HRIS.Validation.Specification.PMS.Entities.Competency
//{
//    public class CompetencySectionItemSpecification : Validates<CompetencySectionItem>
//    {
//        public CompetencySectionItemSpecification()
//        {
//            IsDefaultForType();

//            #region Primitive Types
//            Check(x => x.Name).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
//            Check(x => x.Weight).Required().Between(GlobalConstant.MinimumPercentageValue, GlobalConstant.MaximumPercentageValue);
//            Check(x => x.Rate).Required().Between(GlobalConstant.MinimumValue, GlobalConstant.MaximumValue);
//            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
//            Check(x => x.Comment).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
//            #endregion Primitive Types

//            #region Indexes
//            Check(x => x.Section)
//                .Optional()
//                .Expect((obj, prop) => prop.IsTransient() == false, "")
//                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
//            #endregion Indexes

//        }
//    }
//}
