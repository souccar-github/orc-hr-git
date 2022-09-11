using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Global.Enums;
using HRIS.Domain.PMS.RootEntities;
using HRIS.Domain.Workflow;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using SpecExpress;
using HRIS.Domain.PMS.Configurations;
using HRIS.Validation.MessageKeys;

namespace HRIS.Validation.Specification.PMS.Configurations
{
    public class AppraisalPhaseSettingSpecification : Validates<AppraisalPhaseSetting>
    {
        public AppraisalPhaseSettingSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.WorkflowSetting).Required();
            Check(x => x.FromMark).Optional().GreaterThanEqualTo(0);

            Check(x => x.ToMark).Required().GreaterThan(x => x.FromMark).With(x => x.MessageKey =
                    CustomMessageKeysPmsModule.GetFullKey(CustomMessageKeysPmsModule.ToMarkMustBeGreaterThanFromMark));

            Check(x => x.FullMark).Required().Between(x => x.FromMark, x => x.ToMark).With(x => x.MessageKey =
            CustomMessageKeysPmsModule.GetFullKey(CustomMessageKeysPmsModule.FullMarkMustBeBetweenFromMarkAndToMark));

            // Check(x => x.MarkStep).Required().Between(x => x.FromMark, x => x.ToMark);

            Check(x => x.MarkStep).Required().LessThan(x => x.ToMark).With(x => x.MessageKey =
            CustomMessageKeysPmsModule.GetFullKey(CustomMessageKeysPmsModule.MarkStepMustBeLessThanToMark));

            Check(x => x.Title).Required();

            Check(x => x.GapThreshold).Required().Between(x => x.FromMark, x => x.ToMark).With(x => x.MessageKey =
            CustomMessageKeysPmsModule.GetFullKey(CustomMessageKeysPmsModule.GapThresholdMustBeBetweenFromMarkAndToMark));

            Check(x => x.SkillThreshold).Required().GreaterThan(x => x.GapThreshold).With(x => x.MessageKey =
            CustomMessageKeysPmsModule.GetFullKey(CustomMessageKeysPmsModule.SkillThresholdMustBeGreaterThanGapThreshold))
            .And.Between(x => x.FromMark, x => x.ToMark).With(x => x.MessageKey =
            CustomMessageKeysPmsModule.GetFullKey(CustomMessageKeysPmsModule.SkillThresholdMustBeBetweenFromMarkAndToMark));

            Check(x => x.FromMarkBelowExpected).Optional().Between(x => x.FromMark, x => x.ToMark).With(x => x.MessageKey =
            CustomMessageKeysPmsModule.GetFullKey(CustomMessageKeysPmsModule.FromMarkBelowExpectedMustBeBetweenFromMarkAndToMark));

            Check(x => x.ToMarkBelowExpected).Required().Between(x => x.FromMark, x => x.ToMark).With(x => x.MessageKey =
            CustomMessageKeysPmsModule.GetFullKey(CustomMessageKeysPmsModule.ToMarkBelowExpectedMustBeBetweenFromMarkAndToMark))
                .And.GreaterThan(x => x.FromMarkBelowExpected).With(x => x.MessageKey =
            CustomMessageKeysPmsModule.GetFullKey(CustomMessageKeysPmsModule.ToMarkBelowExpectedMustBeGreaterThanFromMarkBelowExpected));
            
            Check(x => x.FromMarkNeedTraining).Required()
                .Between(x => x.FromMark, x => x.ToMark).With(x => x.MessageKey =
                CustomMessageKeysPmsModule.GetFullKey(CustomMessageKeysPmsModule.FromMarkNeedTrainingMustBeBetweenFromMarkAndToMark))
                .And.GreaterThan(x => x.ToMarkBelowExpected).With(x => x.MessageKey =
                CustomMessageKeysPmsModule.GetFullKey(CustomMessageKeysPmsModule.FromMarkNeedTrainingMustBeGreaterThanToMarkBelowExpected));

            Check(x => x.ToMarkNeedTraining).Required()
                .Between(x => x.FromMark, x => x.ToMark).With(x => x.MessageKey =
                CustomMessageKeysPmsModule.GetFullKey(CustomMessageKeysPmsModule.ToMarkNeedTrainingMustBeBetweenFromMarkAndToMark))
                .And.GreaterThan(x => x.FromMarkNeedTraining).With(x => x.MessageKey =
                CustomMessageKeysPmsModule.GetFullKey(CustomMessageKeysPmsModule.ToMarkNeedTrainingMustBeGreaterThanFromMarkNeedTraining));

            Check(x => x.FromMarkExpected).Required()
                .Between(x => x.FromMark, x => x.ToMark).With(x => x.MessageKey =
                CustomMessageKeysPmsModule.GetFullKey(CustomMessageKeysPmsModule.FromMarkExpectedMustBeBetweenFromMarkAndToMark))
                .And.GreaterThan(x => x.ToMarkNeedTraining).With(x => x.MessageKey =
                CustomMessageKeysPmsModule.GetFullKey(CustomMessageKeysPmsModule.FromMarkExpectedMustBeGreaterThanToMarkNeedTraining));

            Check(x => x.ToMarkExpected).Required()
                .Between(x => x.FromMark, x => x.ToMark).With(x => x.MessageKey =
                CustomMessageKeysPmsModule.GetFullKey(CustomMessageKeysPmsModule.ToMarkExpectedMustBeBetweenFromMarkAndToMark))
                .And.GreaterThan(x => x.FromMarkExpected).With(x => x.MessageKey =
                CustomMessageKeysPmsModule.GetFullKey(CustomMessageKeysPmsModule.ToMarkExpectedMustBeGreaterThanFromMarkExpected));

            Check(x => x.FromMarkUpExpected).Required()
                .Between(x => x.FromMark, x => x.ToMark).With(x => x.MessageKey =
                CustomMessageKeysPmsModule.GetFullKey(CustomMessageKeysPmsModule.FromMarkUpExpectedMustBeBetweenFromMarkAndToMark))
                .And.GreaterThan(x => x.ToMarkExpected).With(x => x.MessageKey =
                CustomMessageKeysPmsModule.GetFullKey(CustomMessageKeysPmsModule.FromMarkUpExpectedMustBeGreaterThanToMarkExpected));

            Check(x => x.ToMarkUpExpected).Required()
                .Between(x => x.FromMark, x => x.ToMark).With(x => x.MessageKey =
                CustomMessageKeysPmsModule.GetFullKey(CustomMessageKeysPmsModule.ToMarkUpExpectedMustBeBetweenFromMarkAndToMark))
                .And.GreaterThan(x => x.FromMarkUpExpected).With(x => x.MessageKey =
                CustomMessageKeysPmsModule.GetFullKey(CustomMessageKeysPmsModule.ToMarkUpExpectedMustBeGreaterThanFromMarkUpExpected));

            Check(x => x.FromMarkDistinct).Required()
                .Between(x => x.FromMark, x => x.ToMark).With(x => x.MessageKey =
                CustomMessageKeysPmsModule.GetFullKey(CustomMessageKeysPmsModule.FromMarkDistinctMustBeBetweenFromMarkAndToMark))
                .And.GreaterThan(x => x.ToMarkUpExpected).With(x => x.MessageKey =
                CustomMessageKeysPmsModule.GetFullKey(CustomMessageKeysPmsModule.FromMarkDistinctMustBeGreaterThanToMarkUpExpected));

            Check(x => x.ToMarkDistinct).Required()
                .Between(x => x.FromMark, x => x.ToMark).With(x => x.MessageKey =
                CustomMessageKeysPmsModule.GetFullKey(CustomMessageKeysPmsModule.ToMarkDistinctMustBeBetweenFromMarkAndToMark))
                .And.GreaterThan(x => x.FromMarkDistinct).With(x => x.MessageKey =
                CustomMessageKeysPmsModule.GetFullKey(CustomMessageKeysPmsModule.ToMarkDistinctMustBeGreaterThanFromMarkDistinct)); ;

            #endregion Primitive Types
            
            #region Indexes

            #endregion Indexes

        }
    }
}
