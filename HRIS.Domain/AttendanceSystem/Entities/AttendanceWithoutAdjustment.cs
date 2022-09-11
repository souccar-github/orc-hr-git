using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.AttendanceSystem.Indexes;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Global.Constant;
using Souccar.Core.Utilities;
using HRIS.Domain.EmployeeRelationServices.Entities;

namespace HRIS.Domain.AttendanceSystem.Entities
{
    public class AttendanceWithoutAdjustment : Entity, IAggregateRoot
    {
        public AttendanceWithoutAdjustment()
        {
            AttendanceWithoutAdjustmentDetails = new List<AttendanceWithoutAdjustmentDetail>();
            EmployeeDisciplinarys = new List<EmployeeDisciplinary>();
        }

        [UserInterfaceParameter(Order = 1)]
        public virtual AttendanceRecord AttendanceRecord { get; set; }

        //[UserInterfaceParameter(Order = 1, IsReference = true)]
        //public virtual EmployeeAttendanceCard EmployeeAttendanceCard { get; set; }

        [UserInterfaceParameter(Order = 2, IsReference = true, ReferenceReadUrl = "AttendanceSystem/Home/FilterEmployeeCard")]
        public virtual EmployeeCard EmployeeAttendanceCard { get; set; }
        public virtual string FatherName
        {
            get
            {
                return EmployeeAttendanceCard.Employee.FatherName;
            }

        }
        [UserInterfaceParameter(Order = 3)]
        public virtual double TotalAbsenceDaysValue
        {
            get
            {
                return AttendanceWithoutAdjustmentDetails.Count(x => x.ActualWorkValue <= 0 && !x.HasMission && !x.HasVacation && !x.IsOffDay && !x.IsHoliday);
            }
        } // عدد أيام الغياب ليوم كامل

        [UserInterfaceParameter(Order = 4, IsHidden = true)]
        public virtual double InitialLatenessTotalValue
        {
            get
            {
                return AttendanceWithoutAdjustmentDetails.Sum(x => x.LatenessHoursValue);
            }
        } // التأخر الصباحي قبل الضرب مع معامل  

        [UserInterfaceParameter(Order = 4)]
        public virtual string InitialLatenessTotalValueFormatedValue
        {
            get
            {
                var result = AttendanceWithoutAdjustmentDetails.Sum(x => x.LatenessHoursValue);
                return DateTimeFormatter.ConvertDoubleToTimeFormat(result);
            }
        } // التأخر الصباحي قبل الضرب مع معامل  

        [UserInterfaceParameter(Order = 5, IsHidden = true)]
        public virtual double FinalLatenessTotalValue { get; set; } // التأخر الصباحي بعد الضرب مع معامل  

        [UserInterfaceParameter(Order = 5)]
        public virtual string FinalLatenessTotalValueFormatedValue { get; set; } // التأخر الصباحي بعد الضرب مع معامل  

        [UserInterfaceParameter(Order = 6, IsHidden = true)]
        public virtual double InitialNonAttendanceTotalValue
        {
            get
            {
                return AttendanceWithoutAdjustmentDetails.Sum(x => x.NonAttendanceHoursValue);
            }
        } // عدم التواجد الساعي قبل الضرب مع معامل  

        [UserInterfaceParameter(Order = 6)]
        public virtual string InitialNonAttendanceTotalValueFormatedValue
        {
            get
            {
                var result = AttendanceWithoutAdjustmentDetails.Sum(x => x.NonAttendanceHoursValue);
                return DateTimeFormatter.ConvertDoubleToTimeFormat(result);
            }
        } // عدم التواجد الساعي قبل الضرب مع معامل  

        [UserInterfaceParameter(Order = 7, IsHidden = true)]
        public virtual double FinalNonAttendanceTotalValue { get; set; } // عدم التواجد الساعي بعد الضرب مع معامل  

        [UserInterfaceParameter(Order = 7)]
        public virtual string FinalNonAttendanceTotalValueFormatedValue { get; set; } // عدم التواجد الساعي بعد الضرب مع معامل  
        
        [UserInterfaceParameter(Order = 10, IsHidden = true)]
        public virtual double TotalNormalOvertimeValue
        {
            get
            {
                return AttendanceWithoutAdjustmentDetails.Sum(x => x.NormalOvertimeValue);
            }
        } // الاضافي الاجمالي المحتسب العادي

        [UserInterfaceParameter(Order = 10)]
        public virtual string TotalNormalOvertimeValueFormatedValue
        {
            get
            {
                var result = AttendanceWithoutAdjustmentDetails.Sum(x => x.NormalOvertimeValue);
                return DateTimeFormatter.ConvertDoubleToTimeFormat(result);
            }
        } // الاضافي الاجمالي المحتسب العادي

        [UserInterfaceParameter(Order = 11, IsHidden = true)]
        public virtual double TotalHolidayOvertimeValue
        {
            get
            {
                return AttendanceWithoutAdjustmentDetails.Sum(x => x.HolidayOvertimeValue);
            }
        } // الاضافي الاجمالي المحتسب العطل

        [UserInterfaceParameter(Order = 11)]
        public virtual string TotalHolidayOvertimeValueFormatedValue
        {
            get
            {
                var result = AttendanceWithoutAdjustmentDetails.Sum(x => x.HolidayOvertimeValue);
                return DateTimeFormatter.ConvertDoubleToTimeFormat(result);
            }
        } // الاضافي الاجمالي المحتسب العطل

        [UserInterfaceParameter(Order = 12, IsHidden = true)]
        public virtual double TotalParticularOvertimeValue
        {
            get
            {
                return AttendanceWithoutAdjustmentDetails.Sum(x => x.ParticularOvertimeValue);
            }
        } // الاضافي الاجمالي المحتسب خاصة

        [UserInterfaceParameter(Order = 12)]
        public virtual string TotalParticularOvertimeValueFormatedValue
        {
            get
            {
                var result = AttendanceWithoutAdjustmentDetails.Sum(x => x.ParticularOvertimeValue);
                return DateTimeFormatter.ConvertDoubleToTimeFormat(result);
            }
        } // الاضافي الاجمالي المحتسب خاصة

        [UserInterfaceParameter(Order = 13, IsHidden = true)]
        public virtual double FinalTotalOvertimeValue { get; set; } // الاضافي الاجمالي المحتسب بعد الضرب بالمعامل

        [UserInterfaceParameter(Order = 13)]
        public virtual string FinalTotalOvertimeValueFormatedValue { get; set; } // الاضافي الاجمالي المحتسب بعد الضرب بالمعامل

        [UserInterfaceParameter(IsHidden = true)]
        public virtual bool IsOvertimeTransferToPayroll { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual bool IsAbsenceTransferToPayroll { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual bool IsNonAttendanceTransferToPayroll { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual bool IsLatenessTransferToPayroll { get; set; }

        [UserInterfaceParameter(Order = 14)]
        public virtual IList<AttendanceWithoutAdjustmentDetail> AttendanceWithoutAdjustmentDetails { get; set; }
        public virtual void AddAttendanceMonthlyAdjustmentDetail(AttendanceWithoutAdjustmentDetail attendanceWithoutAdjustmentDetail)
        {
            AttendanceWithoutAdjustmentDetails.Add(attendanceWithoutAdjustmentDetail);
            attendanceWithoutAdjustmentDetail.AttendanceWithoutAdjustment = this;
        }
        public virtual IList<EmployeeDisciplinary> EmployeeDisciplinarys { get; set; }

        public virtual void AddEmployeeDisciplinary(EmployeeDisciplinary employeeDisciplinary)
        {
            employeeDisciplinary.EmployeeCard = this.EmployeeAttendanceCard;
            employeeDisciplinary.AttendanceWithoutAdjustment = this;
            EmployeeDisciplinarys.Add(employeeDisciplinary);
        }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return EmployeeAttendanceCard.Employee.NameForDropdown; } }

        [UserInterfaceParameter(Order = 22)]
        public virtual bool IsCalculated { get {
                return !AttendanceWithoutAdjustmentDetails.Any(x => !x.IsCalculated);
            }
        } // هل تم حسابه
    }

}
