using System;
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.EmployeeRelationServices.RootEntities
{
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>
    [Module(ModulesNames.EmployeeRelationServices)]
    [Order(13)]
    public class PublicHoliday : Entity, IAggregateRoot
    {

        #region Basic Info

        [UserInterfaceParameter(Order = 1)]
        public virtual DayOfWeek DayOfWeek { get; set; }

        

        #endregion



    }
}
