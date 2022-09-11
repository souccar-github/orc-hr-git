using System;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.Enums;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.PayrollSystem.Configurations;

namespace HRIS.Domain.PayrollSystem.RootEntities
{
    [Command(CommandsNames.InternalTravelLicencePaymentOrder, Order = 3)]
    [Order(15)]
    [Module(ModulesNames.PayrollSystem)]
    [Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    [Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    public class InternalTravelLicence : AuditableEntity
    {   // نحتاج الى معرفة المسافة المقطوعة ويتم الحصول عليها من البروبرتي الخاصة بالمدن والمسافات
        // سعر الكيلو متر موجود في اعدادات أذونات السفر

        [UserInterfaceParameter(Order = 5, IsReference = true)]
        public virtual PrimaryCard PrimaryCard { get; set; }

        [UserInterfaceParameter(Order = 10)]
        public virtual DateTime Date { get; set; } // تاريخ   لا اعلم الفائدة منه تم اضافته بناء على التعديلات الجديدة

        [UserInterfaceParameter(Order = 15)]
        public virtual int Number { get; set; }// رقم يتولد تلقائيا

        [UserInterfaceParameter(Order = 20, IsReference = true)]
        public virtual CitiesDistance CitiesDistance { get; set; }//  المدن والمسافات بينها

        [UserInterfaceParameter(Order = 25)]
        public virtual float Distance
        {
            get
            {
                if (CitiesDistance == null)
                {
                    return 0;
                }
                return CitiesDistance.Distance;
            }
        } // المسافة

        [UserInterfaceParameter(Order = 30)]
        public virtual float ActualTransferenceDays { get; set; } // عدد أيام الانتقال ويكون من تاريخ مغادرة الى العودة

        [UserInterfaceParameter(Order = 35)]
        public virtual float TransferenceBenefitValue { get; set; } // تعويض الانتقال

        [UserInterfaceParameter(Order = 40)]
        public virtual bool WithSpecificTransportationBenefitValue { get; set; } // هل اجور محددة القيمة من القيمة المدخلة بالحقل او هي حساب الي

        [UserInterfaceParameter(Order = 45)]
        public virtual float TransportationBenefitValue { get; set; } // تعويض أجور النقل

        [UserInterfaceParameter(Order = 50)]
        public virtual float OtherExpenseBenefitValue { get; set; } // تعويض  نفقات نثرية


        [UserInterfaceParameter(Order = 53)] 
        public virtual float AdvanceValue { get; set; } // السلفة التي سيتم طرحها بعد الحساب


        //ويتم تخزينه وليس حسابه لانه يعتمد على الخيارات فيما يخص الساعات
        [UserInterfaceParameter(Order = 55)]
        public virtual float FinalBenefitValues { get; set; } //  القيمة النهائية لمجموع التعويضات  ونخزن فيها القيمة النهائية بعد التقريب 

        [UserInterfaceParameter(Order = 60)]
        public virtual TravelLicenceStatus Status { get; set; } //

        [UserInterfaceParameter(Order = 65)]
        public virtual float Salary { get; set; }// راتب الموظف

        [UserInterfaceParameter(Order = 70)]
        public virtual float BenefitSalary { get; set; }// أجر التعويضات الموظف

        [UserInterfaceParameter(Order = 75)]
        public virtual string DocumentNumber { get; set; }// رقم الكتاب المرفق

        [UserInterfaceParameter(Order = 80)]
        public virtual DateTime LeavingDate { get; set; } // تاريخ المغادرة

        [UserInterfaceParameter(Order = 83, IsTime = true)]
        public virtual DateTime LeavingTime { get; set; } //  ساعة المغادرة

        [UserInterfaceParameter(Order = 85)]
        public virtual DateTime ArrivalDate { get; set; } // تاريخ الوصول 

        [UserInterfaceParameter(Order = 88, IsTime = true)]
        public virtual DateTime ArrivalTime { get; set; } // ساعة الوصول 

        [UserInterfaceParameter(Order = 90)]
        public virtual bool WithFood { get; set; } // هل سيتم تقديم الطعام او لا

        [UserInterfaceParameter(Order = 95)]
        public virtual float TotalFoodDays { get; set; } // عدد الايام التي سيتم تقديم الطعام فيها  

        [UserInterfaceParameter(Order = 100)]
        public virtual bool WithRest { get; set; }  // هل سيتم تقديم السكن والمبيت او لا

        [UserInterfaceParameter(Order = 105)]
        public virtual float TotalRestDays { get; set; } // عدد الايام التي سيتم تقديم المبيت فيها  

        [UserInterfaceParameter(Order = 110)]
        public virtual float OrdinanceTotalDays { get; set; } // عدد الايام الاقصى حسب القرار

        [UserInterfaceParameter(Order = 115)]
        public virtual TransportationType GoingTransportationType { get; set; }  // وسيلة الانتقال بالذهاب

        [UserInterfaceParameter(Order = 120)]
        public virtual TransportationType ReturnTransportationType { get; set; }  //  وسيلة الانتقال بالاياب

        [UserInterfaceParameter(Order = 125, IsNonEditable = true)]
        public virtual string PaymentOrderNumber { get; set; }// رقم صرف اذن السفر

        [UserInterfaceParameter(Order = 130, IsNonEditable = true)]
        public virtual DateTime? PaymentOrderDate { get; set; }// تاريخ صرف اذن السفر

        [UserInterfaceParameter(Order = 135, IsNonEditable = true)]
        public virtual DateTime? PaymentOrderAvailabilityDate { get; set; }// تاريخ استحقاق اذن السفر

        [UserInterfaceParameter(Order = 150)]
        public virtual string Note { get; set; }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return PrimaryCard?.Employee?.NameForDropdown + "-" + Number; } }
    }
}
