using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Personnel.Enums
{
    public enum EmploymentStatus
    {
        /// <summary>
        /// بحكم المستقيل
        /// </summary>
        ByVirtueOfTheOutgoing, 
        /// <summary>
        /// مندب إلى
        /// </summary>
        ApuloticTo, 
        /// <summary>
        /// مندب من
        /// </summary>
        ApuloticOf, 
        /// <summary>
        /// أصيل
        /// </summary>
        Thoroughbred, 
        /// <summary>
        /// عقد فرق تقاعد
        /// </summary>
        HoldingTeamsRetirement ,
        /// <summary>
        /// عقد خبرة
        /// </summary>
        ContractExperience ,
        /// <summary>
        /// منتهي من الخدمة تسريح صحي
        /// </summary>
        RetiredFromTheServiceDemobilizationHealthy ,
        /// <summary>
        /// متمرن
        /// </summary>
        Probationer ,
        /// <summary>
        /// منتهي الندب
        /// </summary>
        RetiredScars, 
        /// <summary>
        /// منتهي من الخدمة
        /// </summary>
        RetiredFromService, 
        /// <summary>
        /// منتهي من الاستعارة
        /// </summary>
        RetiredFromTheMetaphor, 
        /// <summary>
        /// مستعار من
        /// </summary>
        PseudonymOf, 
        /// <summary>
        /// معار الى
        /// </summary>
        CheckedTo, 
        /// <summary>
        /// مكفوف اليد
        /// </summary>
        HemmedHand, 
        /// <summary>
        /// منقول
        /// </summary>
        Transported, 
        /// <summary>
        /// منهي من الخدمة \استقالة
        /// </summary>
        TerminatorOfService_Resignation ,
        /// <summary>
        /// منتهي من الخدمة- تسريح بسبب ضعف اداء العامل
        /// </summary>
        RetiredFromTheService_LayOffDueToPoorPerformanceFactor ,
        /// <summary>
        /// منهي من الخدمة - التسريح التأديبي
        /// </summary>
        TerminatorOfService_DemobilizationDisciplinary ,
        /// <summary>
        /// منهي من الخدمة - الطرد
        /// </summary>
        TerminatorOfService_expulsion ,
        /// <summary>
        /// منتهي من الخدمة - الوفاة
        /// </summary>
        RetiredFromTheService_death ,
        /// <summary>
        /// منتهي من الخدمة-لثبوت عدم صلاحية العامل المتمرن
        /// </summary>
        RetiredFromTheService_ActorProvenToBeUnfitExerciser ,
        /// <summary>
        /// منتهي العقد
        /// </summary>
        ExpiredContract, 
        /// <summary>
        /// متعاقد
        /// </summary>
        Contractor,
        /// <summary>
        /// معار من
        /// </summary>
        CheckedFrom, 
    }
}
