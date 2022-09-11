#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 11/03/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference
using FluentNHibernate.Mapping;
using HRIS.Domain.EmployeeRelationServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
namespace HRIS.Mapping.EmployeeRelationServices.Entities
{
    public sealed class FinancialPromotionMap : ClassMap<FinancialPromotion>
    {
        public FinancialPromotionMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.IsPercentage);
            Map(x => x.FixedValue);
            Map(x => x.Percentage);
           
            Map(x => x.Comment);
            Map(x => x.NewSalary);
            Map(x => x.CreationDate);
            Map(x => x.ApprovedDate).Nullable();
            Map(x => x.FinancialPromotionStatus);
            Map(x => x.Salary);

            References(x => x.FinancialPromotionReason);
            References(x => x.WorkflowItem);
            References(x => x.EmployeeCard).Column("EmployeeCard_Id");
            References(x => x.Creator);
        }
    }
}