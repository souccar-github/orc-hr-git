using System.Collections.Generic;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Domain.PayrollSystem.Enums;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.PayrollSystem.RootEntities
{
    //[Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    //[Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    [Order(105)]
    //[Module(ModulesNames.PayrollSystem)]
    public class TravelCategory : Entity, IAggregateRoot
    {
        public TravelCategory()
        {
            TravelCategoryCountries = new List<TravelCategoryCountry>();
        }
        
        [UserInterfaceParameter(Order = 5)]
        public virtual int Number { get; set; } // رمز الفئة تعداد تلقائي
        
        [UserInterfaceParameter(Order = 10)]
        public virtual string Name { get; set; } // اسم الفئة
        
        [UserInterfaceParameter(Order = 15)]
        public virtual double ValueRate { get; set; } //  الامثال
        
        public virtual IList<TravelCategoryCountry> TravelCategoryCountries { get; set; }
        public virtual void AddTravelCategoryCountry(TravelCategoryCountry travelCategoryCountry)
        {
            TravelCategoryCountries.Add(travelCategoryCountry);
            travelCategoryCountry.TravelCategory = this;
        }
    }
}
