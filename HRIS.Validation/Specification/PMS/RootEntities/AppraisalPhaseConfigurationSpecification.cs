////-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
////*******company name: souccar for electronic industries*******//
////project manager:
////supervisor:
////author: Ammar Alziebak
////description:
////start date:
////end date:
////last update:
////update by:
////-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//

//using HRIS.Domain.PMS.Entities;
//using HRIS.Domain.PMS.RootEntities;
//using HRIS.Validation.MessageKeys;
//using SpecExpress;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace HRIS.Validation.Specification.PMS.RootEntities
//{
//    public class AppraisalPhaseConfigurationSpecification : Validates<AppraisalPhaseConfiguration>
//    {
//        public AppraisalPhaseConfigurationSpecification()
//        {
//            IsDefaultForType();
            
//            #region Primitive Types

//            Check(x => x.Name).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
//            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
//            Check(x => x.InitStepCount).Required().GreaterThan(0);
//            Check(x => x.CreatedDate).Required();

//            #endregion

//            #region Indexes

//            //Check(x => x.AppraisalPhaseConfigurationWorkflows)
//            //       .Required()
//            //       .Expect((appraisalPhaseConfiguration, appraisalPhaseConfigurationWorkflows) => appraisalPhaseConfigurationWorkflows.IsTransient() == false, "")
//            //       .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

//            #endregion

//        }
//    }
//}