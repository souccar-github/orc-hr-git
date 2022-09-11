using System;
using Souccar.Core.CustomAttribute;
using Souccar.Core.Utilities;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.AttendanceSystem.Entities
{

    public class AttendanceMonthlyAdjustmentDetail : Entity, IAggregateRoot
    {
        [UserInterfaceParameter(Order = 1)]
        public virtual AttendanceMonthlyAdjustment AttendanceMonthlyAdjustment { get; set; }

        [UserInterfaceParameter(Order = 2)]
        public virtual DayOfWeek DayOfWeek { get; set; }

        [UserInterfaceParameter(Order = 3)]
        public virtual DateTime Date { get; set; }

        //[UserInterfaceParameter(Order = 1)]
        //public virtual AttendanceType AttendanceType { get; set; } // هل هو يوم عمل للموظف او كان في اجازة او عطلة او مهمة او عطلة دوارة

        [UserInterfaceParameter(Order = 4)]
        public virtual bool IsWorkDay { get; set; } // هل اليوم هو يوم عمل أو لا

        [UserInterfaceParameter(Order = 5)]
        public virtual bool HasVacation { get; set; } // هل اليوم فيه إجازة يومية وساعية أو لا   

        [UserInterfaceParameter(Order = 6)]
        public virtual bool IsOffDay { get; set; } // هل اليوم هو يوم عطلة دوارة أو لا

        [UserInterfaceParameter(Order = 7)]
        public virtual bool IsHoliday { get; set; } // هل اليوم هو يوم عطلة اسبوعية أو لا

        [UserInterfaceParameter(Order = 8)]
        public virtual bool HasMission { get; set; } // هل اليوم فيه مهمة سواء كانت يومية او ساعية أو لا

        [UserInterfaceParameter(Order = 9, IsHidden = true)]
        public virtual double VacationValue { get; set; } //  عدد ساعات الاجازة في هذا اليوم ان وجدت الساعية منها او اليومية بحيث نحول اليوم الى ساعات

        [UserInterfaceParameter(Order = 9)]
        public virtual string VacationValueFormatedValue { get; set; } //  عدد ساعات الاجازة في هذا اليوم ان وجدت الساعية منها او اليومية بحيث نحول اليوم الى ساعات

        [UserInterfaceParameter(Order = 10, IsHidden = true)]
        public virtual double OvertimeOrderValue { get; set; } // عدد ساعات التكليف بالاضافي لهذا اليوم ان وجد

        [UserInterfaceParameter(Order = 10)]
        public virtual string OvertimeOrderValueFormatedValue { get; set; } // عدد ساعات التكليف بالاضافي لهذا اليوم ان وجد

        [UserInterfaceParameter(Order = 11, IsHidden = true)]
        public virtual double WorkHoursValue { get; set; } // عدد ساعات العمل حسب الوردية بهذا اليوم لكامل الفترات

        [UserInterfaceParameter(Order = 11)]
        public virtual string WorkHoursValueFormatedValue { get; set; } // عدد ساعات العمل حسب الوردية بهذا اليوم لكامل الفترات

        [UserInterfaceParameter(Order = 12, IsHidden = true)]
        public virtual double ActualWorkHoursValue { get; set; } // عدد ساعات العمل التي حققها الموظف في هذا اليوم لكامل الفترات

        [UserInterfaceParameter(Order = 12)]
        public virtual string ActualWorkHoursValueFormatedValue { get; set; } // عدد ساعات العمل التي حققها الموظف في هذا اليوم لكامل الفترات

        [UserInterfaceParameter(Order = 13, IsHidden = true)]
        public virtual double MissionValue { get; set; } // عدد ساعات المهمة لليوم سواء كانت يومية او ساعية

        [UserInterfaceParameter(Order = 13)]
        public virtual string MissionValueFormatedValue { get; set; } // عدد ساعات المهمة لليوم سواء كانت يومية او ساعية

        [UserInterfaceParameter(Order = 14, IsHidden = true)]
        public virtual double RequiredWorkHoursValue
        {
            get
            {
                if (IsOffDay == true || IsHoliday == true)
                    return 0;
                return WorkHoursValue - VacationValue - MissionValue;
            }
        } // عدد ساعات العمل المطلوبة لهذا اليوم حسب الوردية مع طرح قيمة الاجازات اليومية او الساعية  وكذلك المهام اليومية والساعية

        [UserInterfaceParameter(Order = 14)]
        public virtual string RequiredWorkHoursValueFormatedValue
        {
            get
            {

                if (IsOffDay || IsHoliday)
                    return DateTimeFormatter.ConvertDoubleToTimeFormat(0);
                var result = WorkHoursValue - VacationValue - MissionValue;
                return DateTimeFormatter.ConvertDoubleToTimeFormat(result);
            }
        } // عدد ساعات العمل المطلوبة لهذا اليوم حسب الوردية مع طرح قيمة الاجازات اليومية او الساعية  وكذلك المهام اليومية والساعية

        [UserInterfaceParameter(Order = 15)]
        public virtual int RecurrenceIndex { get; set; } // ترتيب التواتر بحسب التواتر المعرف بالوردية 


        [UserInterfaceParameter(Order = 22)]
        public virtual bool IsCalculated { get; set; } // هل تم حسابه
    }

}
