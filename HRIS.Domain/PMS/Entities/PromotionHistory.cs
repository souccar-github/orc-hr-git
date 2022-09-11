using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.PMS.Configurations;
using HRIS.Domain.PMS.RootEntities;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.PMS.Entities
{

    //Ammar Alziebak
    public class PromotionHistory : Entity, IAggregateRoot
    {
        /// <summary>
        /// الموظف
        /// </summary>
        public virtual Employee Employee { get; set; }
        /// <summary>
        /// إعدادات الترفيعات
        /// </summary>
        public virtual PromotionsSettings PromotionsSettings { get; set; }

        /// <summary>
        /// الراتب الاساسي
        /// </summary>
        public virtual double OldSalary { get; set; }
        /// <summary>
        /// نسبة الترفيع
        /// </summary>
        public virtual double Rate { get; set; }
        /// <summary>
        /// مقدار العلاوة
        /// </summary>
        public virtual double Benefit { get; set; }
        /// <summary>
        /// الراتب بعد الترفيع
        /// </summary>
        public virtual double NewSalary { get; set; }
        /// <summary>
        /// ملاحظات
        /// </summary>
        public virtual string Note { get; set; }
    }
}
