#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 26/02/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Global.Enums;
using HRIS.Domain.OrganizationChart.Indexes;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Personnel.Helpers;
using HRIS.Domain.Personnel.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
namespace HRIS.Domain.Personnel.Configurations
{
    [Module(ModulesNames.Personnel)]
    [Order(4)]
    public class CustodieDetails : Entity, IConfigurationRoot
    {

        
        [UserInterfaceParameter(Order = 2)]
        public virtual CustodiesType CustodiesType { get; set; }
        [UserInterfaceParameter(Order = 3)]
        public virtual string Name { get; set; }
        [UserInterfaceParameter(Order = 4)]
        public virtual string Description { get; set; }
        [UserInterfaceParameter(Order = 5)]
        public virtual double Cost { get; set; }
        [UserInterfaceParameter(Order = 6)]
        public virtual CurrencyType Currency { get; set; }
        [UserInterfaceParameter(Order = 7)]
        public virtual DateTime PurchaseDate { get; set; }
        [UserInterfaceParameter(Order = 8)]
        public virtual Supplier Supplier { get; set; }
        [UserInterfaceParameter(Order = 9)]
        public virtual int DepreciationPeriod { get; set; }
        [UserInterfaceParameter(Order = 10, ReferenceReadUrl = "Personnel/Reference/ReadPeriodWithoutCustom")]
        public virtual Period Period { get; set; }
        [UserInterfaceParameter(Order = 11)]
        public virtual double MonthlyDepreciationAmount
        {
            get
            {
                double temp = 0;
                if (DepreciationPeriod == 0)
                    return temp;
                switch ((int)Period)
                {
                    case 0:
                        temp = Cost / (DepreciationPeriod*12);
                        break;
                    case 1:
                        temp = Cost / (DepreciationPeriod * 6);
                        break;
                    case 2:
                        temp = Cost / (DepreciationPeriod * 3);
                        break;
                    case 3:
                        temp = Cost / (DepreciationPeriod * 1);
                        break;
                }
                return Math.Round(temp,2);
            }
        }
    }
}

