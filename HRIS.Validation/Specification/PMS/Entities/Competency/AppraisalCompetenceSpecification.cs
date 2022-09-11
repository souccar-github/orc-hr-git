using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.PMS.Entities.Competency;
using HRIS.Validation.MessageKeys;
using HRIS.Validation.Specification.Index;
using Souccar.Domain.DomainModel;
using SpecExpress;

namespace HRIS.Validation.Specification.PMS.Entities.Competency
{
    public class AppraisalCompetenceSpecification : Validates<AppraisalCompetence>
    {
        public AppraisalCompetenceSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.Weight).Required().Between(GlobalConstant.MinimumPercentageValue, GlobalConstant.MaximumPercentageValue);

            #endregion Primitive Types
         
       
            #region Indexes

            Check(x => x.Competence)
               .Required()
               .Expect((obj, prop) => prop.IsTransient() == false, "")
               .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion Indexes

        }
    }
}
