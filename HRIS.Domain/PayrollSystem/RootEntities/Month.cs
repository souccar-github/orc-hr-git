using System;
using System.Collections.Generic;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Domain.PayrollSystem.Enums;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.PayrollSystem.RootEntities
{
    [Command(CommandsNames.GenerateMonth, Order = 1)]
    [Command(CommandsNames.CalculateMonth, Order = 2)]
    [Command(CommandsNames.ApproveMonth, Order = 3)]
    [Command(CommandsNames.RejectMonth, Order = 4)]
    [Command(CommandsNames.LockMonth, Order = 5)]


    [Module(ModulesNames.PayrollSystem)]
    [Order(10)]
    public class Month : Entity, IAggregateRoot
    {
        public Month()
        {
            MonthlyCards = new List<MonthlyCard>();
        }
        [UserInterfaceParameter(Order = 1)]
        public virtual int Year { get; set; } // 

        [UserInterfaceParameter(Order = 2)]
        public virtual HRIS.Domain.Global.Enums.Month MonthName { get; set; } // تاريخ الشهر

        [UserInterfaceParameter(Order = 3)]
        public virtual string Name { get; set; } // اسم الشهر

        [UserInterfaceParameter(Order = 4, IsNonEditable = true)]
        public virtual DateTime FromDate { get; set; }
        [UserInterfaceParameter(Order = 4, IsNonEditable = true)]
        public virtual DateTime ToDate { get; set; }

        [UserInterfaceParameter(Order = 20)]
        public virtual MonthStatus MonthStatus { get; set; } // حالة الشهر 

        [UserInterfaceParameter(Order = 25)]
        public virtual MonthType MonthType { get; set; } // استحقاق الراتب ومنه نعرف هل الشهر بهدف تسليم الرواتب أو فقط لصرف تعويضات

        [UserInterfaceParameter(Order = 30)]
        public virtual bool ImportPrimaryBenefits { get; set; } // تحديد اذا كان سيتم استيراد تعويضات البطاقة الاساسية ام لا

        [UserInterfaceParameter(Order = 35)]
        public virtual bool ImportBenefitDistribution { get; set; } // تحديد اذا كان سيتم استيراد التعويضات المحددة في توزيع التعويضات

        [UserInterfaceParameter(Order = 37)]
        public virtual bool ImportDeductionDistribution { get; set; } // تحديد اذا كان سيتم استيراد حسميات المحددة في توزيع التعويضات

        [UserInterfaceParameter(Order = 40)]
        public virtual bool ImportFromEmployeeRelation { get; set; } // استيراد الصحية وبلا اجر وغيرها من علاقات العمل

        [UserInterfaceParameter(Order = 45)]
        public virtual bool ImportFromAttendance { get; set; } // استيراد الغياب والتأخير ونقص الدوام والإضافي من الدوام

        public virtual IList<MonthlyCard> MonthlyCards { get; set; }// بطاقات الموظفين الشهرية المولدة لهذا الشهر
        public virtual void AddMonthlyCard(MonthlyCard monthlyCard)
        {
            MonthlyCards.Add(monthlyCard);
            monthlyCard.Month = this;
        }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return Name + " " + Year; } }
    }
}
