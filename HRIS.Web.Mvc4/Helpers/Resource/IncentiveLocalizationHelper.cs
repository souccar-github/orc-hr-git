using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Core.Extensions;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Helpers.Resource
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class IncentiveLocalizationHelper
    {
        public const string ResourceGroupName = "IncentiveModule";
        public const string InvalidItemsName = "InvalidItemsName";
        public const string RefinementIncentive = "RefinementIncentive";
        public const string ManageEmployeeInIncentive = "ManageEmployeeInIncentive";
        public const string OverwriteTemplateSetting = "OverwriteTemplateSetting";
        public const string LimitMark = "LimitMark";
        public const string BodyIncentiveAppraisalNotify = "YouHaveIncentiveAppraisalNotifyFor";
        public const string SubjectIncentiveAppraisalNotify = "YouHaveIncentiveAppraisalNotifyFor";
        public const string PhaseDescription = "PhaseDescription";
        public const string UpdateExcepted = "UpdateExcepted";
        public const string CalculateIncentive = "CalculateIncentive";
        public const string IncentiveAppraisal = "IncentiveAppraisal";
        public const string AreYouSureUpdatePhase = "AreYouSureUpdatePhase";
        /// <summary>
        /// الحافز النظري
        /// </summary>
        public const string AbstractIncentiveAmount = "AbstractIncentiveAmount";
        /// <summary>
        /// الدوام الفعلي
        /// </summary>
        public const string ActualAttendancePeriod = "ActualAttendancePeriod";
        /// <summary>
        /// الحافز المستحق
        /// </summary>
        public const string ActualIncentiveAmount = "ActualIncentiveAmount";

        /// <summary>
        /// نسبةالاسترداد
        /// </summary>
        public const string PercentageOfRetrieval = "PercentageOfRetrieval";
        /// <summary>
        /// قيمة الاسترداد
        /// </summary>
        public const string RetrievalAmount = "RetrievalAmount";
        /// <summary>
        /// قيمة النهائية بعد الحسم
        /// </summary>
        public const string FinalAmountBeforeExtraDeduction = "FinalAmountBeforeExtraDeduction";
        /// <summary>
        /// مجموع الحافز النهائي حسب علامة التصفية
        /// </summary>
        public const string SumFinalIncentive = "SumFinalIncentive";
        /// <summary>
        /// مجموع ما تم قبضه
        /// </summary>
        public const string SumPayedIncentive = "SumPayedIncentive";
        /// <summary>
        /// ناتج طرح مجموع الحوافز المقبوضة من النهائية
        /// </summary>
        public const string ResultOfMinus = "ResultOfMinus";
        /// <summary>
        /// نسبة الحسم الاضافي
        /// </summary>
        public const string PercentageOfExtraDeduction = "PercentageOfExtraDeduction";
        /// <summary>
        /// الحسم الاضافي
        /// </summary>
        public const string ExtraDeductionAmount = "ExtraDeductionAmount";
        /// <summary>
        /// القيمة النهائلة للحافز
        /// </summary>
        public const string FinalIncentiveAmount = "FinalIncentiveAmount";


        public static string GetResource(string key)
        {
            var result = ServiceFactory.LocalizationService.GetResource(ResourceGroupName + "_" + key);
            return string.IsNullOrEmpty(result) ? key.ToCapitalLetters() : result;
        }
    }
}