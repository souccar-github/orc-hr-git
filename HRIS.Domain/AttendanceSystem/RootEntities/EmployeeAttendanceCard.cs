//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using HRIS.Domain.AttendanceSystem.Entities;
//using HRIS.Domain.Global.Constant;
//using HRIS.Domain.Personnel.RootEntities;
//using Souccar.Core.CustomAttribute;
//using Souccar.Domain.DomainModel;

//namespace HRIS.Domain.AttendanceSystem.RootEntities
//{
//    [Order(1)]
//    [Module(ModulesNames.AttendanceSystem)]
//    public class EmployeeAttendanceCard : Entity, IAggregateRoot //  بطاقة دوام لموظف
//    {
//        [UserInterfaceParameter(Order = 1, IsReference = true)]
//        public virtual  Employee Employee { get; set; } // الموظف الذي سيتم ربط بطاقة الدوام به

//        [UserInterfaceParameter(Order = 1)]
//        public virtual  string EmployeeMachineCode { get; set; } // 

//        [UserInterfaceParameter(Order = 1)]
//        public virtual  bool AttendanceDemand { get; set; } // مطالبة الدوام

//        [UserInterfaceParameter(Order = 1, IsReference = true)]
//        public virtual  AttendanceForm AttendanceForm { get; set; } // 

//        [UserInterfaceParameter(Order = 1, IsReference = true)]
//        public virtual  NonAttendanceForm LatenessForm { get; set; } // 

//        [UserInterfaceParameter(Order = 1, IsReference = true)]
//        public virtual  NonAttendanceForm AbsenceForm { get; set; } // 

//        [UserInterfaceParameter(Order = 1, IsReference = true)]
//        public virtual  OvertimeForm OvertimeForm { get; set; } // 

//        [UserInterfaceParameter(Order = 1)]
//        public virtual int AbsenceCounterRecurrence { get; set; } // عداد عدم التواجد على مستوى الشهر اي كل شهر يمثل واحد

//        [UserInterfaceParameter(Order = 1)]
//        public virtual int LatenessCounterRecurrence { get; set; } // عداد التأخر الصباحي على مستوى الشهر اي كل شهر يمثل واحد


//        [UserInterfaceParameter(Order = 200, IsHidden = true)]
//        public virtual string NameForDropdown
//        {
//            get
//            {
//                return Employee.FullName;
//            }
//        }


//        [UserInterfaceParameter(Order = 1)]
//        public virtual  IList<TemporaryWorkshop> TemporaryWorkshops { get; set; }
//        public virtual void AddTemporaryWorkshop(TemporaryWorkshop temporaryWorkshop)
//        {
//            TemporaryWorkshops.Add(temporaryWorkshop);
//            temporaryWorkshop.EmployeeAttendanceCard = this;
//        }
//    }
//}
