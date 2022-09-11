using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.OrgChart.Entities;
using HRIS.Domain.OrgChart.RootEntities;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.PMS.Configurations;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.PMS.Entities
{
    public class EmployeePromotion:Entity,IAggregateRoot
    {
        public virtual Employee Employee { get; set; }
        public virtual Node Node { get; set; }
        public virtual Grade Grade { get; set; }
        public virtual JobTitle JobTitle { get; set; }
        public virtual IncidenceDefinition IncidenceDefinition { get; set; }
        public virtual PromotionsSettings PromotionsSettings { get; set; }
        public virtual Domain.JobDesc.RootEntities.JobDescription JobDesc { get; set; }
        public virtual double PayBeforePromotion { get; set; }
        public virtual double ResultPerformanceEvaluation { get; set; }
        public virtual double EfficiencyDegree { get; set; }
        public virtual int PromotionPhaseDays { get; set; }
        public virtual int AbsenceDaysEffectOnPromotion { get; set; }
        public virtual int DaysEffectOnPromotion { get; set; }
        public virtual double BenefitAmount { get; set; }
        public virtual double PayAfterPromotion { get; set; }
        public virtual double BenefitSalaryBeforePromotion { get; set; }
        public virtual double BenefitSalaryAfterPromotion { get; set; }
        public virtual string Note { get; set; }
    }
}
