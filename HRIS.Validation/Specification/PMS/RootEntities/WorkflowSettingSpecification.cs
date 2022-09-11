//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//project manager:
//supervisor:
//author: Ammar Alziebak
//description:
//start date:
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//

using HRIS.Domain.PMS.RootEntities;
using HRIS.Domain.Workflow;
using HRIS.Validation.MessageKeys;
using SpecExpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Validation.Specification.PMS.RootEntities
{
    public class WorkflowSettingSpecification : Validates<WorkflowSetting>
    {
        public WorkflowSettingSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.InitStepCount).Optional().GreaterThanEqualTo(0);
            Check(x => x.Title).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.CreationDate).Required();

            #endregion

            #region Indexes

            #endregion

        }
    }
}