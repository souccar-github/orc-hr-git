using HRIS.Domain.AttendanceSystem.Enums;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.AttendanceSystem.RootEntities
{
    [Order(6)]
    [Module(ModulesNames.AttendanceSystem)]
    public class DailyEnternaceExitRecord : Entity, IAggregateRoot
    {
        [UserInterfaceParameter(Order = 1, IsReference = true, ReferenceReadUrl = "AttendanceSystem/Home/FilterEmployee")]
        public virtual Employee Employee { get; set; }
        [UserInterfaceParameter(Order = 2)]
        public virtual string FatherName
        {
            get
            {
                return Employee.FatherName;
            }
        }

        [UserInterfaceParameter(Order = 3, IsNonEditable = true, IsReference = true)]
        public virtual Node Node { get; set; }


        [UserInterfaceParameter(Order = 40)]
        public virtual DateTime Date { get; set; }

        [UserInterfaceParameter(Order = 50)]
        public virtual DayOfWeek Day { get; set; }


        [UserInterfaceParameter(Order = 55, IsNonEditable = true)]
        public virtual DayStatus Status { get; set; }

        [UserInterfaceParameter(Order = 60, IsDateTime = true, IsHidden = true)]
        public virtual DateTime? LoginDateTime { get; set; }

        [UserInterfaceParameter(Order = 65, IsTime = true)]
        public virtual DateTime? LoginTime { get; set; }

        [UserInterfaceParameter(Order = 66)]
        public virtual DateTime? LoginDate { get; set; }

        [UserInterfaceParameter(Order = 70, IsDateTime = true, IsHidden = true)]
        public virtual DateTime? LogoutDateTime { get; set; }

        [UserInterfaceParameter(Order = 75, IsTime = true)]
        public virtual DateTime? LogoutTime { get; set; }
        [UserInterfaceParameter(Order = 77)]
        public virtual DateTime? LogoutDate { get; set; }

        [UserInterfaceParameter(Order = 80, IsDateTime = true, IsHidden = true)]
        public virtual DateTime? SecondLoginDateTime { get; set; }

        [UserInterfaceParameter(Order = 90, IsTime = true)]
        public virtual DateTime? SecondLoginTime { get; set; }
        [UserInterfaceParameter(Order = 94)]
        public virtual DateTime? SecondLoginDate { get; set; }

        [UserInterfaceParameter(Order = 95, IsDateTime = true, IsHidden = true)]
        public virtual DateTime? SecondLogoutDateTime { get; set; }

        [UserInterfaceParameter(Order = 100, IsTime = true)]
        public virtual DateTime? SecondLogoutTime { get; set; }
        [UserInterfaceParameter(Order = 101)]
        public virtual DateTime? SecondLogoutDate { get; set; }

        [UserInterfaceParameter(Order = 105, IsDateTime = true, IsHidden = true)]
        public virtual DateTime? ThirdLoginDateTime { get; set; }

        [UserInterfaceParameter(Order = 110, IsTime = true)]
        public virtual DateTime? ThirdLoginTime { get; set; }
        [UserInterfaceParameter(Order = 112)]
        public virtual DateTime? ThirdLoginDate { get; set; }
        [UserInterfaceParameter(Order = 115, IsDateTime = true, IsHidden = true)]
        public virtual DateTime? ThirdLogoutDateTime { get; set; }

        [UserInterfaceParameter(Order = 120, IsTime = true)]
        public virtual DateTime? ThirdLogoutTime { get; set; }
        [UserInterfaceParameter(Order = 122)]
        public virtual DateTime? ThirdLogoutDate { get; set; }


        [UserInterfaceParameter(Order = 130, IsHidden = true)]
        public virtual double RequiredWorkHours { get; set; }
        [UserInterfaceParameter(Order = 130, IsNonEditable = true)]
        public virtual string RequiredWorkHoursFormatedValue { get; set; }
        [UserInterfaceParameter(Order = 130, IsHidden = true)]
        public virtual double WorkHoursValue { get; set; }
        [UserInterfaceParameter(Order = 133, IsNonEditable = true)]
        public virtual string WorkHoursFormatedValue { get; set; }
        [UserInterfaceParameter(Order = 140, IsHidden = true)]
        public virtual double AbsentHoursValue { get; set; }
        [UserInterfaceParameter(Order = 141, IsNonEditable = true)]
        public virtual string AbsentHoursFormatedValue { get; set; }
        [UserInterfaceParameter(Order = 150, IsHidden = true)]
        public virtual double LateHoursValue { get; set; }
        [UserInterfaceParameter(Order = 151, IsNonEditable = true)]
        public virtual string LatenessHoursFormatedValue { get; set; }
        [UserInterfaceParameter(Order = 160, IsHidden = true)]
        public virtual double OvertimeHoursValue { get; set; }

        [UserInterfaceParameter(Order = 161, IsNonEditable = true)]
        public virtual string OvertimeHoursFormatedValue { get; set; }
        [UserInterfaceParameter(Order = 170, IsHidden = true)]
        public virtual double HolidayOvertimeHoursValue { get; set; }
        [UserInterfaceParameter(Order = 171, IsNonEditable = true)]
        public virtual string HolidayOvertimeFormatedValue { get; set; }
        [UserInterfaceParameter(Order = 172, IsNonEditable = true)]
        public virtual bool HasMission { get; set; }
        [UserInterfaceParameter(Order = 173, IsHidden = true)]
        public virtual double MissionValue { get; set; }
        [UserInterfaceParameter(Order = 174, IsNonEditable = true)]
        public virtual string MissionFormatedValue { get; set; }
        [UserInterfaceParameter(Order = 175, IsNonEditable = true)]
        public virtual bool HasVacation { get; set; }
        [UserInterfaceParameter(Order = 176, IsHidden = true)]
        public virtual double VacationValue { get; set; }
        [UserInterfaceParameter(Order = 177, IsNonEditable = true)]
        public virtual string VacationFormatedValue { get; set; }
        [UserInterfaceParameter(Order = 180, IsNonEditable = true)]
        public virtual LateType LateType { get; set; }

        [UserInterfaceParameter(Order = 190, IsNonEditable = true)]
        public virtual AbsenseType AbsenseType { get; set; }

        [UserInterfaceParameter(Order = 200, IsHidden = true)]
        public virtual bool IsClosed { get; set; }

        [UserInterfaceParameter(Order = 210, IsHidden = true)]
        public virtual bool IsCalculated { get; set; }

        [UserInterfaceParameter(Order = 220)]
        public virtual string UpdateReason { get; set; } // الرسالة التي سيتم ادخالها في حال تم تعديل الريكورد من قبل المستخدم

        [UserInterfaceParameter(Order = 230)]
        public virtual string Note { get; set; } // الرسالة التي سيتم ادخالها في حال تم تعديل الريكورد من قبل المستخدم

        [UserInterfaceParameter(Order = 240)]
        public virtual InsertSource InsertSource { get; set; } // مصدر السجل هل هو ادخال يدوي أو توليد تلقائي أو من الجهاز

    }
}
