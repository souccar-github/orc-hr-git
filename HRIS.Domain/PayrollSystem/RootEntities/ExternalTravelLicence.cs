using System;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.Personnel.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.PayrollSystem.Enums;

namespace HRIS.Domain.PayrollSystem.RootEntities
{
    [Command(CommandsNames.ExternalTravelLicencePaymentOrder, Order = 3)]
    [Order(20)]
    [Module(ModulesNames.PayrollSystem)]
    [Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    [Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    public class ExternalTravelLicence : AuditableEntity
    { // يجب معرفة التعويضات الناتجة عن العملية هل هي تعويض وحيد او عدة تعويضات 
        //[UserInterfaceParameter(Order = 1)]
        //public virtual int PersonalNumber { get; set; } 

        [UserInterfaceParameter(Order = 5, IsReference = true)]
        public virtual PrimaryCard PrimaryCard { get; set; }

        [UserInterfaceParameter(Order = 10)]
        public virtual DateTime Date { get; set; } // تاريخ   لا اعلم الفائدة منه تم اضافته بناء على التعديلات الجديدة

        [UserInterfaceParameter(Order = 15)]
        public virtual int Number { get; set; }// رقم يتولد تلقائيا

        [UserInterfaceParameter(Order = 20, IsReference = true)]
        public virtual Country Country { get; set; } // بلد الاغتراب ومنه نصل الى فئة الاغراب

        [UserInterfaceParameter(Order = 25)]
        public virtual double ActualTransferenceDays { get; set; } // عدد أيام الانتقال ويكون من تاريخ مغادرة سوريا الى العودة

        //ويتم تخزينه وليس حسابه لانه يعتمد على الخيارات فيما يخص الساعات
        [UserInterfaceParameter(Order = 30)]
        public virtual double ActualTravelDays { get; set; } // عدد أيام الاغتراب ويكون من تاريخ مغادرة بلد الاغتراب الى العودة
        
        [UserInterfaceParameter(Order = 33)]
        public virtual bool WithBenefit { get; set; } // هل سيتم اعتماد التعويضات او تصفيرها
         
        [UserInterfaceParameter(Order = 35)]
        public virtual double TransferenceBenefitValue { get; set; } //  القيمة النهائية لتعويض الانتقال

        [UserInterfaceParameter(Order = 40)]
        public virtual double TravelBenefitValue { get; set; } //  القيمة النهائية لتعويض الاغتراب

        [UserInterfaceParameter(Order = 45)]
        public virtual double PreparationBenefitValue { get; set; }//  تعويض التهيوء

        [UserInterfaceParameter(Order = 50)]
        public virtual double OtherExpenseBenefitValue { get; set; } //  تعويض نفقات نثرية

        [UserInterfaceParameter(Order = 55)]
        public virtual double AddedValueBenefitValue { get; set; } //  تعويض النسبة المضافة

        [UserInterfaceParameter(Order = 57)]
        public virtual double LeavingFee { get; set; }// أجور النقل ورسم المغدرة

        [UserInterfaceParameter(Order = 58)]
        public virtual float AdvanceValue { get; set; } // السلفة التي سيتم طرحها بعد الحساب


        [UserInterfaceParameter(Order = 60)]
        public virtual double FinalBenefitValues { get; set; } //  القيمة النهائية لاجمالي التعويضات ونخزن فيها القيمة النهائية بعد التقريب

        [UserInterfaceParameter(Order = 65)]
        public virtual TravelLicenceStatus Status { get; set; } //

        [UserInterfaceParameter(Order = 70)]
        public virtual double Salary { get; set; }// راتب الموظف

        [UserInterfaceParameter(Order = 75)]
        public virtual double BenefitSalary { get; set; }// أجر التعويضات الموظف

        [UserInterfaceParameter(Order = 80)]
        public virtual string DocumentNumber { get; set; }// رقم الكتاب المرفق

        [UserInterfaceParameter(Order = 85)]
        public virtual DateTime InternalLeavingDate { get; set; } // تاريخ المغادرة لسوريا 

        [UserInterfaceParameter(Order = 88, IsTime = true)]
        public virtual DateTime InternalLeavingTime { get; set; } // ساعة المغادرة لسوريا

        [UserInterfaceParameter(Order = 90)]
        public virtual DateTime DestinationArrivalDate { get; set; } // تاريخ الوصول لبلد الاغتراب 

        [UserInterfaceParameter(Order = 93, IsTime = true)]
        public virtual DateTime DestinationArrivalTime { get; set; } // ساعة الوصول لبلد الاغتراب

        [UserInterfaceParameter(Order = 95)]
        public virtual DateTime DestinationLeavingDate { get; set; } // تاريخ المغادرة لبلد الاغتراب

        [UserInterfaceParameter(Order = 98, IsTime = true)]
        public virtual DateTime DestinationLeavingTime { get; set; } // ساعة المغادرة لبلد الاغتراب

        [UserInterfaceParameter(Order = 100)]
        public virtual DateTime InternalArrivalDate { get; set; } // تاريخ الوصول لسوريا 

        [UserInterfaceParameter(Order = 103, IsTime = true)]
        public virtual DateTime InternalArrivalTime { get; set; } // ساعة الوصول لسوريا 

        [UserInterfaceParameter(Order = 105)]
        public virtual bool WithFood { get; set; } // هل سيتم تقديم الطعام او لا

        [UserInterfaceParameter(Order = 110)]
        public virtual double TotalFoodDays { get; set; } // عدد الايام التي سيتم تقديم الطعام فيها

        [UserInterfaceParameter(Order = 115)]
        public virtual bool WithRest { get; set; }  // هل سيتم تقديم السكن والمبيت او لا

        [UserInterfaceParameter(Order = 120)]
        public virtual double TotalRestDays { get; set; } // عدد الايام التي سيتم تقديم المبيت فيها 

        //ويتم تخزينه وليس حسابه لانه يعتمد على الخيارات فيما يخص الساعات
        [UserInterfaceParameter(Order = 125)]
        public virtual int OrdinanceTotalDays { get; set; } // عدد الايام الاقصى حسب القرار  

        [UserInterfaceParameter(Order = 130)]
        public virtual bool IsMinister { get; set; } // هل اذن السفر لوزير ام لا مفيد لمعرفة السقف في تعويض الاغتراب

        [UserInterfaceParameter(Order = 135, IsNonEditable = true)]
        public virtual string PaymentOrderNumber { get; set; }// رقم صرف اذن السفر

        [UserInterfaceParameter(Order = 140, IsNonEditable = true)]
        public virtual DateTime? PaymentOrderDate { get; set; }// تاريخ صرف اذن السفر

        [UserInterfaceParameter(Order = 145, IsNonEditable = true)]
        public virtual DateTime? PaymentOrderAvailabilityDate { get; set; }// تاريخ استحقاق اذن السفر
        [UserInterfaceParameter(Order = 150)]
        public virtual string Note { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return PrimaryCard?.Employee?.NameForDropdown + "-" + Number; } }

    }
}
