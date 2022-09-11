#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 04/03/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference
using FluentNHibernate.Mapping;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
namespace HRIS.Mapping.EmployeeRelationServices.Configurations
{
    public sealed class GeneralEmployeeRelationSettingMap : ClassMap<GeneralEmployeeRelationSetting>
    {
        public GeneralEmployeeRelationSettingMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.TerminationWorkflowName);
            References(x => x.ResignationWorkflowName);
            References(x => x.PromotionWorkflowName);
            References(x => x.FinancialPromotionWorkflowName);
            References(x => x.EntranceExitRequestWorkflowName);
            References(x => x.MissionRequestWorkflowName);
            References(x => x.AdvanceWorkflowName);
            References(x => x.CustodiesWorkflowName);
            References(x => x.EmployeeSourceTransferRequest);
            References(x => x.EmployeeDistenationTransferRequest);
            References(x => x.EmployeeLoanRequest);

        }
    }
}
