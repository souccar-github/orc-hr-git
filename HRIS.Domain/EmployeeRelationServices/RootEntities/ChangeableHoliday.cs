using System;
using HRIS.Domain.EmployeeRelationServices.Indexes;
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.EmployeeRelationServices.RootEntities
{
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>
    [Module(ModulesNames.EmployeeRelationServices)]
    [Order(12)]
    public class ChangeableHoliday : Entity, IAggregateRoot
    {

        #region Basic Info

        [UserInterfaceParameter(Order = 1)]
        public virtual ChangeableHolidayName HolidayName { get; set; }

        [UserInterfaceParameter(Order = 2)]
        public virtual DateTime StartDate { get; set; }

        [UserInterfaceParameter(Order = 3)]
        public virtual DateTime EndDate { get; set; }

        [UserInterfaceParameter(Order = 4)]
        public virtual int NumberOfHolidayDays
        {
            get
            {
                return (int)(DateTime.Parse(EndDate.ToShortDateString()) - DateTime.Parse(StartDate.ToShortDateString())).TotalDays + 1;
            }
        }

        #endregion



    }
}
