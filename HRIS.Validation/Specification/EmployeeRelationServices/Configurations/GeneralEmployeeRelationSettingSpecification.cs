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
using System;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Validation.MessageKeys;
using SpecExpress;
using HRIS.Domain.EmployeeRelationServices.Configurations;
#endregion

namespace HRIS.Validation.Specification.EmployeeRelationServices.Configurations
{
    public class GeneralEmployeeRelationSettingSpecification : Validates<GeneralEmployeeRelationSetting>
    {
        public GeneralEmployeeRelationSettingSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            #endregion

            #region Indexes
            Check(x => x.TerminationWorkflowName)
                .Required()
                .Expect((generalEmployeeRelationSetting, terminationWorkflowName) => terminationWorkflowName.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            Check(x => x.ResignationWorkflowName)
                .Required()
                .Expect((generalEmployeeRelationSetting, resignationWorkflowName) => resignationWorkflowName.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            Check(x => x.PromotionWorkflowName)
                .Required()
                .Expect((generalEmployeeRelationSetting, promotionWorkflowName) => promotionWorkflowName.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            Check(x => x.FinancialPromotionWorkflowName)
                .Required()
                .Expect((generalEmployeeRelationSetting, financialPromotion) => financialPromotion.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            Check(x => x.EntranceExitRequestWorkflowName)
                .Required()
                .Expect((generalEmployeeRelationSetting, entranceExitRequest) => entranceExitRequest.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            Check(x => x.MissionRequestWorkflowName)
                .Required()
                .Expect((generalEmployeeRelationSetting,missionRequest) => missionRequest.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            Check(x => x.AdvanceWorkflowName)
                 .Required()
                 .Expect((generalEmployeeRelationSetting, missionRequest) => missionRequest.IsTransient() == false, "")
                 .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            Check(x => x.CustodiesWorkflowName)
              .Required()
              .Expect((generalEmployeeRelationSetting, missionRequest) => missionRequest.IsTransient() == false, "")
              .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.EmployeeSourceTransferRequest)
                .Required()
                .Expect((generalEmployeeRelationSetting, missionRequest) => missionRequest.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            Check(x => x.EmployeeDistenationTransferRequest)
                .Required()
                .Expect((generalEmployeeRelationSetting, missionRequest) => missionRequest.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion
        }
    }
}
