using System;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using HRIS.Domain.EmployeeRelationServices.Indexes;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.EmployeeRelationServices.Entities
{
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>

    public class AdministrativeProfile : Entity
    {

        #region Basic Info
        /// <summary>
        /// الجهة القادم منها
        /// </summary>
        [UserInterfaceParameter(Order = 1, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual WorkSide WorkSide { get; set; }
        /// <summary>
        /// تاريخ المباشرة  في الدولة 
        /// </summary>
        [UserInterfaceParameter(Order = 2, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual DateTime StartDateInCountry { get; set; }
        /// <summary>
        /// تاريخ الانفكاك عن الجهة
        /// </summary>
        [UserInterfaceParameter(Order = 3, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual DateTime AbruptionDate { get; set; }
        /// <summary>
        /// سبب الانفكاك 
        /// </summary>
        [UserInterfaceParameter(Order = 4, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string AbruptionCause { get; set; }
        /// <summary>
        /// العمر الوظيفي السابق بالايام
        /// </summary>
        [UserInterfaceParameter(Order = 5, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.ServicePeriod)]
        public virtual int DaysCount { get; set; }
        /// <summary>
        /// العمر الوظيفي السابق بالاشهر
        /// </summary>
        [UserInterfaceParameter(Order = 6, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.ServicePeriod)]
        public virtual int MonthsCount { get; set; }
        /// <summary>
        /// العمر الوظيفي السابق بالسنوات
        /// </summary>
        [UserInterfaceParameter(Order = 7, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.ServicePeriod)]
        public virtual int YearsCount { get; set; }
        /// <summary>
        /// المستهلك من الاجازات الادارية للعام الحالي 
        /// </summary>
        [UserInterfaceParameter(Order = 8, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.LeavesBalance)]
        public virtual decimal AdministrativeLeaveDaysForCurrentYear { get; set; }
        /// <summary>
        /// المستهلك من الاجازات الادارية الاضطرارية للعام الحالي  
        /// </summary>
        [UserInterfaceParameter(Order = 9, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.LeavesBalance)]
        public virtual decimal ForcedAdministrativeLeaveDaysForCurrentYear { get; set; }
        /// <summary>
        /// المستهلك من الاجازات بلا اجر التي تحسب من الخدمة لتاريخ اخر ترفيع 
        /// </summary>
        [UserInterfaceParameter(Order = 10, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.LeavesBalance)]
        public virtual decimal UnpaidLeaveDaysCalculatedOfServiceForLastPromotionDate { get; set; }
        /// <summary>
        /// المستهلك من الاجازات بلا اجر التي لا تحسب من الخدمة لتاريخ اخر ترفيع 
        /// </summary>
        [UserInterfaceParameter(Order = 11, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.LeavesBalance)]
        public virtual decimal UnpaidLeaveDaysNotCalculatedOfServiceForLastPromotionDate { get; set; }
        /// <summary>
        /// المستهلك من الاجازات بلا اجر التي تحسب من الخدمة بين تاريخ اخر ترفيع وتاريخ المباشرة 
        /// </summary>
        [UserInterfaceParameter(Order = 12, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.LeavesBalance)]
        public virtual decimal UnpaidLeaveDaysCalculatedOfServiceBetweenLastPromotionAndStartDate { get; set; }
        /// <summary>
        /// المستهلك من الاجازات بلا اجر التي لا تحسب من الخدمة بين تاريخ اخر ترفيع وتاريخ المباشرة 
        /// </summary>
        [UserInterfaceParameter(Order = 13, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.LeavesBalance)]
        public virtual decimal UnpaidLeaveDaysNotCalculatedOfServiceBetweenLastPromotionAndStartDate { get; set; }
        /// <summary>
        /// المستهلك السنوي للعام الحالي من الاجازات الصحية كعدد أيام منفصلة 
        /// </summary>
        [UserInterfaceParameter(Order = 14, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.LeavesBalance)]
        public virtual decimal HealthyLeaveSeparateDaysForCurrentYear { get; set; }
        /// <summary>
        /// المستهلك السنوي للعام الحالي من الاجازات الصحية كعدد أيام متصلة 
        /// </summary>
        [UserInterfaceParameter(Order = 15, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.LeavesBalance)]
        public virtual decimal HealthyLeaveConsecutiveDaysForCurrentYear { get; set; }
        /// <summary>
        /// الأيام المستهلكة خلال الخمس أعوام السابقة من الاجازات الصحية متصلة أو منفصلة
        /// </summary>
        [UserInterfaceParameter(Order = 16, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.LeavesBalance)]
        public virtual decimal HealthyLeaveDaysForPreviousYears { get; set; }
        /// <summary>
        /// عدد أيام الغياب الغير أصولي من تاريخ اخر ترفيع 
        /// </summary>
        [UserInterfaceParameter(Order = 17, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.LeavesBalance)]
        public virtual decimal AbsenceDaysUnjustifiedFromLastPromotionDate { get; set; }
        /// <summary>
        /// رصيد اجازات الحج كعدد مرات
        /// </summary>
        [UserInterfaceParameter(Order = 18, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.LeavesBalance)]
        public virtual int PilgrimageLeavesCount { get; set; }
        /// <summary>
        /// رصيد اجازات الامومة كعدد مرات 
        /// </summary>
        [UserInterfaceParameter(Order = 19, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.LeavesBalance)]
        public virtual int MaternityLeavesCount { get; set; }
        /// <summary>
        /// الموظف
        /// </summary>
        [UserInterfaceParameter(Order = 20)]
        public virtual Employee Employee { get; set; }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return WorkSide != null ? WorkSide.Name : ""; } }
        //[UserInterfaceParameter(Order = 10, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.LeavesBalance)]
        //public virtual int UnpaidLeaveDaysForCurrentYear { get; set; }

        //[UserInterfaceParameter(Order = 11, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.LeavesBalance)]
        //public virtual int UnpaidLeaveDaysForPreviousYears { get; set; }

        #endregion

    }
}
