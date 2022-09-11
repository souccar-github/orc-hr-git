
namespace HRIS.Domain.EmployeeRelationServices.Enums
{
    /// <summary>
    /// Author: Ammar Alziebak
    /// </summary>
    //نوع نهاية الخدمة
    public enum ServiceEndType
    {
        /// <summary>
        /// استقالة
        /// </summary>
        Resignation,
        /// <summary>
        /// الوفاة
        /// </summary>
        Death,
        /// <summary>
        /// طرد
        /// </summary>
        dismissal,
        /// <summary>
        /// بلوغ الستين
        /// </summary>
        SixtyYearsAge,
        /// <summary>
        /// ضعف أداء العامل
        /// </summary>
        PoorPerformance,
        /// <summary>
        /// صرف من الخدمة
        /// </summary>
        DismissalOfWork,
        /// <summary>
        /// تسريح صحي
        /// </summary>
        LayOffHealthy,
        /// <summary>
        /// تسريح تأديبي
        /// </summary>
        LayOffPunitive,
        /// <summary>
        /// عدم صلاحية العامل المتمرن
        /// </summary>
        NotEfficientFactor
    }
}