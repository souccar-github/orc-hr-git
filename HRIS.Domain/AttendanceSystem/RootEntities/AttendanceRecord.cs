using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.AttendanceSystem.Entities;
using HRIS.Domain.AttendanceSystem.Enums;
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Global.Enums;
using Souccar.Infrastructure.Core;
using HRIS.Domain.AttendanceSystem.Configurations;

namespace HRIS.Domain.AttendanceSystem.RootEntities
{
    [Command(CommandsNames.ChooseEmployees, Order = 1)]
    //[Command(CommandsNames.GenerateAttendanceRecord, Order = 2)]
    [Command(CommandsNames.CalculateAttendanceRecord, Order = 3)]
    [Command(CommandsNames.LockAttendanceRecord, Order = 4)]


    //  يشبه الشهر في الرواتب والاجور ويعبر عن المعلومات الاساسية للشهر الذس سنقوم بحساب الدوام للموظفين فيه
    [Order(1)]
    [Module(ModulesNames.AttendanceSystem)]
    public class AttendanceRecord : Entity, IAggregateRoot // سجل دوام الموظفين مع تقاص شهري
    {
        public AttendanceRecord()
        {
            AttendanceDailyAdjustments = new List<AttendanceDailyAdjustment>();
            AttendanceMonthlyAdjustments = new List<AttendanceMonthlyAdjustment>();
            AttendanceWithoutAdjustments = new List<AttendanceWithoutAdjustment>();
        }

        public virtual DateTime getToDateDependOnLastMonth(IQueryable<AttendanceRecord> months, int attendanceCycleStartDay)
        {
            if (months.Any())
            {
                var previousMonth = months.OrderByDescending(x => x.Id).FirstOrDefault();
                if (previousMonth != null)
                {
                    var year = previousMonth.Month == Month.December ? previousMonth.Year + 1 : previousMonth.Year;
                    var month = previousMonth.Month == Month.December ? (int)Month.January : (int)previousMonth.Month + 1;
                    return attendanceCycleStartDay == 1 ?
                    new DateTime(year , month, DateTime.DaysInMonth(year, month)):
                    new DateTime(month == 12 ? year + 1 : year,
                        month == 12 ? 1 : month + 1,
                        DateTime.DaysInMonth(month == 12 ? year + 1 : year, month == 12 ? 1 : month + 1) < (attendanceCycleStartDay - 1) ?
                        DateTime.DaysInMonth(month == 12 ? year + 1 : year, month == 12 ? 1 : month + 1) : attendanceCycleStartDay - 1);
                }
            }
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
        }

        public virtual DateTime getFromDateDependOnLastMonth(IQueryable<AttendanceRecord> months, int attendanceCycleStartDay)
        {
            if (months.Any())
            {
                var previousMonth = months.OrderByDescending(x => x.Id).FirstOrDefault();
                if (previousMonth != null)
                {
                    return new DateTime(previousMonth.Month == Month.December ? previousMonth.Year + 1 : previousMonth.Year,
                        previousMonth.Month == Month.December ? (int)Month.January : (int)previousMonth.Month + 1, attendanceCycleStartDay);
                } 
            }
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }
        

        [UserInterfaceParameter(Order = 1)]
        public virtual int Year { get; set; } // 

        [UserInterfaceParameter(Order = 2)]
        public virtual Month Month { get; set; }

        [UserInterfaceParameter(Order = 3)]
        public virtual string Name { get; set; } // اسم الشهر
        
        [UserInterfaceParameter(Order = 4)]
        public virtual DateTime FromDate { get; set; }
        [UserInterfaceParameter(Order = 4)]
        public virtual DateTime ToDate { get; set; }

        [UserInterfaceParameter(Order = 4)]
        public virtual string Note { get; set; }

        [UserInterfaceParameter(Order = 2)]
        public virtual AttendanceMonthStatus AttendanceMonthStatus { get; set; } // حالة الشهر 

        [UserInterfaceParameter(Order = 5)]
        public virtual IList<AttendanceWithoutAdjustment> AttendanceWithoutAdjustments { get; set; }
        public virtual void AddAttendanceWithoutAdjustment(AttendanceWithoutAdjustment attendanceWithoutAdjustment)
        {
            AttendanceWithoutAdjustments.Add(attendanceWithoutAdjustment);
            attendanceWithoutAdjustment.AttendanceRecord = this;
        }

        [UserInterfaceParameter(Order = 6)]
        public virtual IList<AttendanceDailyAdjustment> AttendanceDailyAdjustments { get; set; }
        public virtual void AddAttendanceDailyAdjustment(AttendanceDailyAdjustment attendanceDailyAdjustment)
        {
            AttendanceDailyAdjustments.Add(attendanceDailyAdjustment);
            attendanceDailyAdjustment.AttendanceRecord = this;
        }

        [UserInterfaceParameter(Order = 7)]
        public virtual IList<AttendanceMonthlyAdjustment> AttendanceMonthlyAdjustments { get; set; }
        public virtual void AddAttendanceMonthlyAdjustment(AttendanceMonthlyAdjustment attendanceMonthlyAdjustment)
        {
            AttendanceMonthlyAdjustments.Add(attendanceMonthlyAdjustment);
            attendanceMonthlyAdjustment.AttendanceRecord = this;
        }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return Name; } }
    }

}
