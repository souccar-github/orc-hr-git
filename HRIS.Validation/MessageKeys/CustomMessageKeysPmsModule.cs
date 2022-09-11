//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//project manager:
//supervisor:
//author: Ammar Alziebak
//description:
//start date:
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Validation.MessageKeys
{
    public static class CustomMessageKeysPmsModule
    {
        public const string TotalSumWeight = "TotalSumWeightLessthen100";
        public const string ResourceGroupName = "CustomMessageKeysPmsModule";
        public const string totalWeightRange = "TotalWeightRangeBetween0-100";
        public const string ToMarkMustBeGreaterThanFromMark = "ToMarkMustBeGreaterThanFromMark";
        public const string SkillThresholdMustBeGreaterThanGapThreshold = "SkillThresholdMustBeGreaterThanGapThreshold";
        public const string ToMarkBelowExpectedMustBeGreaterThanFromMarkBelowExpected = "ToMarkBelowExpectedMustBeGreaterThanFromMarkBelowExpected";
        public const string FromMarkNeedTrainingMustBeGreaterThanToMarkBelowExpected = "FromMarkNeedTrainingMustBeGreaterThanToMarkBelowExpected";
        public const string ToMarkNeedTrainingMustBeGreaterThanFromMarkNeedTraining = "ToMarkNeedTrainingMustBeGreaterThanFromMarkNeedTraining";
        public const string FromMarkExpectedMustBeGreaterThanToMarkNeedTraining = "FromMarkExpectedMustBeGreaterThanToMarkNeedTraining";
        public const string ToMarkExpectedMustBeGreaterThanFromMarkExpected = "ToMarkExpectedMustBeGreaterThanFromMarkExpected";
        public const string FromMarkUpExpectedMustBeGreaterThanToMarkExpected = "FromMarkUpExpectedMustBeGreaterThanToMarkExpected";
        public const string ToMarkUpExpectedMustBeGreaterThanFromMarkUpExpected = "ToMarkUpExpectedMustBeGreaterThanFromMarkUpExpected";
        public const string FromMarkDistinctMustBeGreaterThanToMarkUpExpected = "FromMarkDistinctMustBeGreaterThanToMarkUpExpected";
        public const string ToMarkDistinctMustBeGreaterThanFromMarkDistinct = "ToMarkDistinctMustBeGreaterThanFromMarkDistinct";

        public const string MarkStepMustBeLessThanToMark = "MarkStepMustBeLessThanToMark";

        public const string FullMarkMustBeBetweenFromMarkAndToMark = "FullMarkMustBeBetweenFromMarkAndToMark";
        public const string GapThresholdMustBeBetweenFromMarkAndToMark = "GapThresholdMustBeBetweenFromMarkAndToMark";
        public const string SkillThresholdMustBeBetweenFromMarkAndToMark = "SkillThresholdMustBeBetweenFromMarkAndToMark";
        public const string FromMarkBelowExpectedMustBeBetweenFromMarkAndToMark = "FromMarkBelowExpectedMustBeBetweenFromMarkAndToMark";
        public const string ToMarkBelowExpectedMustBeBetweenFromMarkAndToMark = "ToMarkBelowExpectedMustBeBetweenFromMarkAndToMark";
        public const string FromMarkNeedTrainingMustBeBetweenFromMarkAndToMark = "FromMarkNeedTrainingMustBeBetweenFromMarkAndToMark";
        public const string ToMarkNeedTrainingMustBeBetweenFromMarkAndToMark = "ToMarkNeedTrainingMustBeBetweenFromMarkAndToMark";
        public const string FromMarkExpectedMustBeBetweenFromMarkAndToMark = "FromMarkExpectedMustBeBetweenFromMarkAndToMark";
        public const string ToMarkExpectedMustBeBetweenFromMarkAndToMark = "ToMarkExpectedMustBeBetweenFromMarkAndToMark";
        public const string FromMarkUpExpectedMustBeBetweenFromMarkAndToMark = "FromMarkUpExpectedMustBeBetweenFromMarkAndToMark";
        public const string ToMarkUpExpectedMustBeBetweenFromMarkAndToMark = "ToMarkUpExpectedMustBeBetweenFromMarkAndToMark";
        public const string FromMarkDistinctMustBeBetweenFromMarkAndToMark = "FromMarkDistinctMustBeBetweenFromMarkAndToMark";
        public const string ToMarkDistinctMustBeBetweenFromMarkAndToMark = "ToMarkDistinctMustBeBetweenFromMarkAndToMark";


        public static string GetFullKey(string key)
        {
            return ResourceGroupName + "_" + key;
        }
    }
}
