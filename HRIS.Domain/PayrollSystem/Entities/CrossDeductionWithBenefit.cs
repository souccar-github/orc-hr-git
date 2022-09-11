using System.Collections.Generic;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.Configurations;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.PayrollSystem.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.PayrollSystem.Entities
{//todo : Mhd Update changeset no.2
    
    public class CrossDeductionWithBenefit :  Entity, IAggregateRoot
    {

        public CrossDeductionWithBenefit()
        {
            CrossDependencys = new List<CrossDependency>();
        }

        [UserInterfaceParameter(Order = 5)]
        public virtual BenefitCard BenefitCard { get; set; } // التعويض الذي يتبع له هذا التقاطع اي الماستر

        [UserInterfaceParameter(Order = 10)]
        public virtual DeductionCard DeductionCard { get; set; } // الحسمية التي سنقاطع التعويض معها

        [UserInterfaceParameter(Order = 15)]
        public virtual CrossType CrossType { get; set; } // ألية التقاطع حسب المحدد بالبطاقة الشهرية او قيمة مخصصة

        [UserInterfaceParameter(Order = 20)]
        public virtual double Value { get; set; } // قيمة التقاطع في حال كانت الالية مخصصة

        [UserInterfaceParameter(Order = 25)]
        public virtual CrossFormula CrossFormula { get; set; }  // صيغة التقاطع في حال كانت الالية مخصصة


        //[UserInterfaceParameter(Order = 15)]
        //public virtual bool IncludedInCrossedInitialValue { get; set; } // 
        //[UserInterfaceParameter(Order = 15)]
        //public virtual bool DependsOnCrossedInitialValue { get; set; } // 

        public virtual IList<CrossDependency> CrossDependencys { get; set; }
        public virtual void AddCrossDependency(CrossDependency crossDependency)
        {
            CrossDependencys.Add(crossDependency);
            crossDependency.CrossDeductionWithBenefit = this;
        }
    }
}
