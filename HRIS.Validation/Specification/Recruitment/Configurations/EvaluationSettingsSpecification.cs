using System;
using HRIS.Domain.Recruitment.Configurations;
using HRIS.Domain.Recruitment.RootEntities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Recruitment.Configurations
{
    public class EvaluationSettingsSpecification : Validates<EvaluationSettings>
    {
        public EvaluationSettingsSpecification()
        {

            IsDefaultForType();

            #region Primitive Types

            Check(x => x.RecruitmentType).Required();
            Check(x => x.FullSuccessMark).Required().LessThanEqualTo(GlobalConstant.MaximumValue).And.GreaterThan(GlobalConstant.MinimumValue);
            Check(x => x.MinSuccessMark).Required().LessThan(x => x.FullSuccessMark).And.GreaterThan(GlobalConstant.MinimumValue);
            Check(x => x.WrittenWeightFactor).Required().LessThan(x => x.FullSuccessMark).And.GreaterThan(GlobalConstant.MinimumValue);
            Check(x => x.MinWrittenMark).Required().LessThan(x => x.WrittenWeightFactor).And.GreaterThan(GlobalConstant.MinimumValue);
            Check(x => x.OralWeightFactor).Required().LessThan(x => x.FullSuccessMark).And.GreaterThan(GlobalConstant.MinimumValue);
            Check(x => x.MinOralMark).Required().LessThan(x => x.OralWeightFactor).And.GreaterThan(GlobalConstant.MinimumValue);
            Check(x => x.OldnessWeightFactor).Required().LessThan(x => x.FullSuccessMark).And.GreaterThan(GlobalConstant.MinimumValue);
            Check(x => x.LaborOfficeStartingDate).Required().LessThan(DateTime.Now.AddYears(-1));
            Check(x => x.RoundType).Required();
            Check(x => x.MartyrSonFactor).Required().LessThan(x => x.FullSuccessMark).And.GreaterThan(GlobalConstant.MinimumValue);

            #endregion

            #region Indexes

            Check(x => x.Grade)
                .Required()
                .Expect((evaluationSettings, grade) => grade.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion
        }
    }
}
