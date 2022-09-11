using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.AttendanceSystem.Enums;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.AttendanceSystem.RootEntities
{
    [Order(5)]
    [Module(ModulesNames.AttendanceSystem)]
    public class EntranceExitRecord : Entity, IAggregateRoot
    {
        //[UserInterfaceParameter(Order = 1)]
        //public virtual  string EmployeeCode { get; set; } //باركود الموظف للقراءة فقط 

        [UserInterfaceParameter(Order = 3, IsReference = true, ReferenceReadUrl = "AttendanceSystem/Home/FilterEmployee")]
        public virtual Employee Employee { get; set; } // الموظف المرتبط بالتسجيل وهو للقراءة فقط
        [UserInterfaceParameter(Order = 4)]
        public virtual string FatherName
        {
            get
            {
                return Employee.FatherName;
            }
        }
        [UserInterfaceParameter(Order = 20, IsDateTime = true, IsHidden = true)]
        public virtual DateTime LogDateTime { get; set; } // تاريخ ووقت تسجيل العملية سواء كانت دخول او خروج

        [UserInterfaceParameter(Order = 5, IsTime = true)]
        public virtual DateTime LogTime { get; set; }

        [UserInterfaceParameter(Order = 1)]
        public virtual DateTime LogDate { get; set; }

        [UserInterfaceParameter(Order = 6)]
        public virtual LogType LogType { get; set; } // نوع التسجيل للريكورد هل هو دخول او خروج


        [UserInterfaceParameter(Order = 2)]
        public virtual DayOfWeek Day { get { return LogDate.DayOfWeek; } }  // تاريخ ووقت تسجيل الخروج

        //[UserInterfaceParameter(Order = 4)]
        //public virtual string ErrorMessage { get; set; } // رسالة الخطأ المولدة تلقائيا في حال وجود اي مشكلة بالريكورد

        [UserInterfaceParameter(Order = 7)]
        public virtual ErrorType ErrorType { get; set; } // رسالة الخطأ المولدة تلقائيا في حال وجود اي مشكلة بالريكورد

        [UserInterfaceParameter(Order = 8)]
        public virtual string UpdateReason { get; set; } // الرسالة التي سيتم ادخالها في حال تم تعديل الريكورد من قبل المستخدم

        [UserInterfaceParameter(Order = 9)]
        public virtual string Note { get; set; } // الرسالة التي سيتم ادخالها في حال تم تعديل الريكورد من قبل المستخدم

        [UserInterfaceParameter(Order = 10)]
        public virtual InsertSource InsertSource { get; set; } // مصدر السجل هل هو ادخال يدوي أو توليد تلقائي أو من الجهاز

        [UserInterfaceParameter(Order = 9, IsNonEditable = true)]
        public virtual bool IsChecked { get; set; } // تم فحص السجل ام لا
    }


}
