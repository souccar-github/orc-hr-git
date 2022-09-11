using System;
using HRIS.Domain.AttendanceSystem.Enums;
using Souccar.Core.CustomAttribute;
using Souccar.Core.Utilities;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.AttendanceSystem.Entities
{

    public class AttendanceDailyAdjustmentDetail : Entity, IAggregateRoot
    {
        [UserInterfaceParameter(Order = 1)]
        public virtual AttendanceDailyAdjustment AttendanceDailyAdjustment { get; set; }

        [UserInterfaceParameter(Order = 2)]
        public virtual DayOfWeek DayOfWeek { get; set; }

        [UserInterfaceParameter(Order = 3)]
        public virtual DateTime Date { get; set; }

        [UserInterfaceParameter(Order = 4)]
        public virtual bool HasVacation { get; set; } // هل اليوم فيه إجازة أو لا سواء كانت يومية او ساعية

        [UserInterfaceParameter(Order = 5)]
        public virtual bool HasMission { get; set; } // هل اليوم فيه مهمة سواء كانت يومية او ساعية أو لا

        [UserInterfaceParameter(Order = 6)]
        public virtual bool IsOffDay { get; set; } // هل اليوم هو يوم عطلة دوارة أو لا

        [UserInterfaceParameter(Order = 7)]
        public virtual bool IsHoliday { get; set; } // هل اليوم هو يوم عطلة اسبوعية أو لا

        [UserInterfaceParameter(Order = 8)]
        public virtual bool IsWorkDay { get; set; } // هل اليوم هو يوم عمل أو لا

        [UserInterfaceParameter(Order = 9, IsHidden = true)]
        public virtual double VacationValue { get; set; } //  عدد ساعات الاجازة في هذا اليوم ان وجدت الساعية منها او اليومية بحيث نحول اليوم الى ساعات

        [UserInterfaceParameter(Order = 9)]
        public virtual string VacationValueFormatedValue { get; set; } //  عدد ساعات الاجازة في هذا اليوم ان وجدت الساعية منها او اليومية بحيث نحول اليوم الى ساعات

        [UserInterfaceParameter(Order = 10, IsHidden = true)]
        public virtual double WorkHoursValue { get; set; } // عدد ساعات العمل حسب الوردية بهذا اليوم لكامل الفترات

        [UserInterfaceParameter(Order = 10)]
        public virtual string WorkHoursValueFormatedValue { get; set; } // عدد ساعات العمل حسب الوردية بهذا اليوم لكامل الفترات

        [UserInterfaceParameter(Order = 11, IsHidden = true)]
        public virtual double MissionValue { get; set; }  // عدد ساعات المهمة لليوم سواء كانت يومية او ساعية

        [UserInterfaceParameter(Order = 11)]
        public virtual string MissionValueFormatedValue { get; set; }  // عدد ساعات المهمة لليوم سواء كانت يومية او ساعية

        [UserInterfaceParameter(Order = 12, IsHidden = true)]
        public virtual double RequiredWorkHoursValue
        {
            get
            {
                return WorkHoursValue - VacationValue - MissionValue;
            }
        } // عدد ساعات العمل المطلوبة لهذا اليوم حسب الوردية مع طرح قيمة الاجازات اليومية او الساعية  وكذلك المهام اليومية والساعية

        [UserInterfaceParameter(Order = 12)]
        public virtual string RequiredWorkHoursValueFormatedValue
        {
            get
            {
                var result = WorkHoursValue - VacationValue - MissionValue;
                return DateTimeFormatter.ConvertDoubleToTimeFormat(result);
            }
        } // عدد ساعات العمل المطلوبة لهذا اليوم حسب الوردية مع طرح قيمة الاجازات اليومية او الساعية  وكذلك المهام اليومية والساعية

        [UserInterfaceParameter(Order = 13, IsHidden = true)]
        public virtual double ActualWorkHoursValue { get; set; } // عدد ساعات العمل التي حققها الموظف في هذا اليوم لكامل الفترات

        [UserInterfaceParameter(Order = 13)]
        public virtual string ActualWorkHoursValueFormatedValue { get; set; } // عدد ساعات العمل التي حققها الموظف في هذا اليوم لكامل الفترات

        [UserInterfaceParameter(Order = 14)]
        public virtual DailyAdjustmentAttendanceStatus DailyAdjustmentAttendanceStatus
        {
            get
            {
                if (ActualWorkHoursValue == 0 && IsWorkDay && !HasMission && !HasVacation)
                {
                    return DailyAdjustmentAttendanceStatus.Absence;
                }
                if (ActualWorkHoursValue == RequiredWorkHoursValue)
                {
                    return DailyAdjustmentAttendanceStatus.Ok;
                }
                if (ActualWorkHoursValue < RequiredWorkHoursValue && ActualWorkHoursValue > 0)
                {
                    return DailyAdjustmentAttendanceStatus.NonAttendance;
                }
                if (ActualWorkHoursValue > RequiredWorkHoursValue)
                {
                    return DailyAdjustmentAttendanceStatus.Overtime;
                }
                return DailyAdjustmentAttendanceStatus.Ok;
            }
        }

        [UserInterfaceParameter(Order = 15, IsHidden = true)]
        public virtual double NonAttendanceHoursValue
        {
            get
            {
                return (RequiredWorkHoursValue - ActualWorkHoursValue) > 0 ? RequiredWorkHoursValue - ActualWorkHoursValue : 0;
            }
        } // نقص الدوام اليومي سواء بسبب عدم التواجد ضمن اليوم او بسبب التأخر الصباحي

        [UserInterfaceParameter(Order = 15)]
        public virtual string NonAttendanceHoursValueFormatedValue
        {
            get
            {
                var result = (RequiredWorkHoursValue - ActualWorkHoursValue) > 0 ? RequiredWorkHoursValue - ActualWorkHoursValue : 0;
                return DateTimeFormatter.ConvertDoubleToTimeFormat(result);
            }
        } // نقص الدوام اليومي سواء بسبب عدم التواجد ضمن اليوم او بسبب التأخر الصباحي

        [UserInterfaceParameter(Order = 16, IsHidden = true)]
        public virtual double OvertimeOrderValue { get; set; } // عدد ساعات التكليف بالاضافي لهذا اليوم ان وجد

        [UserInterfaceParameter(Order = 16)]
        public virtual string OvertimeOrderValueFormatedValue { get; set; } // عدد ساعات التكليف بالاضافي لهذا اليوم ان وجد

        [UserInterfaceParameter(Order = 17, IsHidden = true)]
        public virtual double ExpectedOvertimeValue
        {
            get
            {
                return ActualWorkHoursValue > RequiredWorkHoursValue
                    ? ActualWorkHoursValue - RequiredWorkHoursValue
                    : 0;
            }
        } // الاضافي المحتمل لهذا اليوم

        [UserInterfaceParameter(Order = 17)]
        public virtual string ExpectedOvertimeValueFormatedValue
        {
            get
            {
                var result = ActualWorkHoursValue > RequiredWorkHoursValue
                    ? ActualWorkHoursValue - RequiredWorkHoursValue
                    : 0;
                return DateTimeFormatter.ConvertDoubleToTimeFormat(result);
            }
        } // الاضافي المحتمل لهذا اليوم

        [UserInterfaceParameter(Order = 18, IsHidden = true)]
        public virtual double OrderedOvertimeValue
        {
            get 
            {
                return ExpectedOvertimeValue > OvertimeOrderValue
                    ? OvertimeOrderValue
                    : ExpectedOvertimeValue;
            }
        } // الاضافي المكلف القيمة الصغرى بين المحتمل وعدد ساعات التكليف

        [UserInterfaceParameter(Order = 18)]
        public virtual string OrderedOvertimeValueFormatedValue
        {
            get
            {
                var result = ExpectedOvertimeValue > OvertimeOrderValue
                    ? OvertimeOrderValue
                    : ExpectedOvertimeValue;
                return DateTimeFormatter.ConvertDoubleToTimeFormat(result);
            }
        } // الاضافي المكلف القيمة الصغرى بين المحتمل وعدد ساعات التكليف

        [UserInterfaceParameter(Order = 19, IsHidden = true)]
        public virtual double NormalOvertimeValue { get; set; } // الاضافي المحتسب -عطلة- ليوم عادي

        [UserInterfaceParameter(Order = 19)]
        public virtual string NormalOvertimeValueFormatedValue { get; set; } // الاضافي المحتسب -عطلة- ليوم عادي

        [UserInterfaceParameter(Order = 20, IsHidden = true)]
        public virtual double HolidayOvertimeValue { get; set; } // الاضافي المحتسب -عادي- ليوم عمل عطلة 

        [UserInterfaceParameter(Order = 20)]
        public virtual string HolidayOvertimeValueFormatedValue { get; set; } // الاضافي المحتسب -عادي- ليوم عمل عطلة 

        [UserInterfaceParameter(Order = 21)]
        public virtual int RecurrenceIndex { get; set; } // ترتيب التواتر بحسب التواتر المعرف بالوردية 

        [UserInterfaceParameter(Order = 22)]
        public virtual bool IsCalculated { get; set; } // هل تم حسابه

    }

}
