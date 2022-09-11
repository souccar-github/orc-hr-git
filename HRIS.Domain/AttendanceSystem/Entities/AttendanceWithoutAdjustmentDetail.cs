using System;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.AttendanceSystem.Entities
{//todo : Mhd Update changeset no.1

    public class AttendanceWithoutAdjustmentDetail : Entity, IAggregateRoot
    {
        [UserInterfaceParameter(Order = 1)]
        public virtual AttendanceWithoutAdjustment AttendanceWithoutAdjustment { get; set; }

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

        [UserInterfaceParameter(Order = 9)]
        public virtual string RequiredWorkHoursRanges { get; set; } // الوردية لليوم كمجالات لكامل الفترات
        [UserInterfaceParameter(Order = 10, IsHidden = true)]
        public virtual double RequiredWorkHoursValue { get; set; } // عدد ساعات العمل المطلوبة لهذا اليوم حسب الوردية مع طرح قيمة الاجازات اليومية او الساعية

        [UserInterfaceParameter(Order = 10)]
        public virtual string RequiredWorkHoursFormatedValue { get; set; } // عدد ساعات العمل المطلوبة لهذا اليوم حسب الوردية مع طرح قيمة الاجازات اليومية او الساعية

        [UserInterfaceParameter(Order = 11)]
        public virtual string VacationRanges { get; set; } // الاجازة لليوم كمجالات لكامل الفترات
        [UserInterfaceParameter(Order = 12, IsHidden = true)]
        public virtual double VacationValue { get; set; } //  عدد ساعات الاجازة في هذا اليوم ان وجدت الساعية منها او اليومية بحيث نحول اليوم الى ساعات

        [UserInterfaceParameter(Order = 12)]
        public virtual string VacationFormatedValue { get; set; } //  عدد ساعات الاجازة في هذا اليوم ان وجدت الساعية منها او اليومية بحيث نحول اليوم الى ساعات

        [UserInterfaceParameter(Order = 13)]
        public virtual string ActualWorkRanges { get; set; } // الدوام المحقق لليوم كمجالات لكامل الفترات
        [UserInterfaceParameter(Order = 14, IsHidden = true)]
        public virtual double ActualWorkValue { get; set; } // عدد ساعات العمل التي حققها الموظف في هذا اليوم لكامل الفترات

        [UserInterfaceParameter(Order = 14)]
        public virtual string ActualWorkFormatedValue { get; set; } // عدد ساعات العمل التي حققها الموظف في هذا اليوم لكامل الفترات

        [UserInterfaceParameter(Order = 15, IsHidden = true)]
        public virtual double NonAttendanceHoursValue { get; set; } // نقص دوام  لفترة ضمن الدوام

        [UserInterfaceParameter(Order = 15)]
        public virtual string NonAttendanceHoursFormatedValue { get; set; } // نقص دوام  لفترة ضمن الدوام

        [UserInterfaceParameter(Order = 16)]
        public virtual string NonAttendanceHoursRanges { get; set; } // نقص دوام  لفترة ضمن الدوام




        [UserInterfaceParameter(Order = 11)]
        public virtual string RestRanges { get; set; } // الاستراحة لليوم كمجالات لكامل الفترات
        [UserInterfaceParameter(Order = 12, IsHidden = true)]
        public virtual double RestValue { get; set; } //  عدد ساعات الاستراحة في هذا اليوم ان وجدت 

        [UserInterfaceParameter(Order = 12)]
        public virtual string RestFormatedValue { get; set; } //  عدد ساعات الاستراحة في هذا اليوم ان وجدت 


        [UserInterfaceParameter(Order = 17, IsHidden = true)]
        public virtual double LatenessHoursValue { get; set; } // التأخر الصباحي

        [UserInterfaceParameter(Order = 17)]
        public virtual string LatenessHoursFormatedValue { get; set; } // التأخر الصباحي

        [UserInterfaceParameter(Order = 18)]
        public virtual string LatenessHoursRanges { get; set; } // التأخر الصباحي
        [UserInterfaceParameter(Order = 19, IsHidden = true)]
        public virtual double MissionValue { get; set; } //  عدد ساعات المهمة في هذا اليوم ان وجدت الساعية منها او اليومية بحيث نحول اليوم الى ساعات

        [UserInterfaceParameter(Order = 19)]
        public virtual string MissionFormatedValue { get; set; } //  عدد ساعات المهمة في هذا اليوم ان وجدت الساعية منها او اليومية بحيث نحول اليوم الى ساعات

        [UserInterfaceParameter(Order = 20)]
        public virtual string MissionRanges { get; set; } //  عدد ساعات المهمة في هذا اليوم ان وجدت الساعية منها او اليومية بحيث نحول اليوم الى ساعات

        [UserInterfaceParameter(Order = 21)]
        public virtual string OriginalOvertimeOrderFormatedValue { get; set; }

        [UserInterfaceParameter(Order = 21, IsHidden = true)]
        public virtual double OvertimeOrderValue { get; set; } // عدد ساعات التكليف بالاضافي لهذا اليوم ان وجد

        [UserInterfaceParameter(Order = 21)]
        public virtual string OvertimeOrderFormatedValue { get; set; } // عدد ساعات التكليف بالاضافي لهذا اليوم ان وجد

        [UserInterfaceParameter(Order = 22)]
        public virtual string OvertimeOrderRanges { get; set; } // الاضافي المكلف كمجالات لليوم كمجالات لكامل الفترات
        [UserInterfaceParameter(Order = 23, IsHidden = true)]
        public virtual double ExpectedOvertimeValue { get; set; } // الاضافي المحتمل لهذا اليوم

        [UserInterfaceParameter(Order = 23)]
        public virtual string ExpectedOvertimeFormatedValue { get; set; } // الاضافي المحتمل لهذا اليوم

        [UserInterfaceParameter(Order = 24)]
        public virtual string ExpectedOvertimeRanges { get; set; } // الاضافي المحتمل لليوم كمجالات لكامل الفترات

        [UserInterfaceParameter(Order = 25, IsHidden = true)]
        public virtual double NormalOvertimeValue { get; set; } // الاضافي المحتسب -عادي- ليوم عمل عادي

        [UserInterfaceParameter(Order = 25)]
        public virtual string NormalOvertimeFormatedValue { get; set; } // الاضافي المحتسب -عادي- ليوم عمل عادي

        [UserInterfaceParameter(Order = 26, IsHidden = true)]
        public virtual double HolidayOvertimeValue { get; set; } // الاضافي المحتسب -عطلة- ليوم عطلة

        [UserInterfaceParameter(Order = 26)]
        public virtual string HolidayOvertimeFormatedValue { get; set; } // الاضافي المحتسب -عطلة- ليوم عطلة

        [UserInterfaceParameter(Order = 27, IsHidden = true)]
        public virtual double ParticularOvertimeValue { get; set; } // الاضافي المحتسب -خاصة- للفترات الخاصة

        [UserInterfaceParameter(Order = 27)]
        public virtual string ParticularOvertimeFormatedValue { get; set; } // الاضافي المحتسب -خاصة- للفترات الخاصة

        [UserInterfaceParameter(Order = 28)]
        public virtual int RecurrenceIndex { get; set; } // ترتيب التواتر بحسب التواتر المعرف بالوردية 

        [UserInterfaceParameter(Order = 29)]
        public virtual bool IsCalculated { get; set; } // هل تم حسابه
    }

}
