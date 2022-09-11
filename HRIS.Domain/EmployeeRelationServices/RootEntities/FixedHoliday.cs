
using System;
using System.Globalization;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Global.Enums;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.EmployeeRelationServices.Indexes;

namespace HRIS.Domain.EmployeeRelationServices.RootEntities
{
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>
    [Module(ModulesNames.EmployeeRelationServices)]
    [Order(11)]
    public class FixedHoliday : Entity, IAggregateRoot
    {

        #region Basic Info

        [UserInterfaceParameter(Order = 1)]
        public virtual FixedHolidayName HolidayName { get; set; }

        [UserInterfaceParameter(Order = 2)]
        public virtual DayOfMonth Day { get; set; }

        [UserInterfaceParameter(Order = 3)]
        public virtual Month Month { get; set; }

        [UserInterfaceParameter(Order = 4, IsNonEditable = true)]
        public virtual int NumberOfHolidayDays { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual DateTime StartDate
        {
            get
            {
                if ((int)Month != 0)
                {
                    var daysCount = DateTime.DaysInMonth(DateTime.Today.Year, (int)Month);
                    return new DateTime(DateTime.Today.Year, (int)Month,
                        (daysCount >= (int)Day && (int)Day != 0) ? (int)Day : daysCount);
                }
                else
                {
                    var daysCount = DateTime.DaysInMonth(DateTime.Today.Year, 1);
                    return new DateTime(DateTime.Today.Year, 1,
                        (daysCount >= (int)Day && (int)Day != 0) ? (int)Day : daysCount);
                }
            }
        }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual DateTime EndDate
        {
            get
            {
                return StartDate.AddDays(NumberOfHolidayDays - 1);
            }
        }

        #endregion

    }
}
