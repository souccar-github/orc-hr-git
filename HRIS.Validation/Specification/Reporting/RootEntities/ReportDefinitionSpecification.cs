
using Souccar.Domain.Reporting;
using SpecExpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Validation.Specification.Reporting.RootEntities
{
    public class ReportDefinitionSpecification: Validates<ReportDefinition>
    {
        public ReportDefinitionSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.Title).Required();
            Check(x => x.FileName).Required();
            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
           
            #endregion Primitive Types
            
            #region Indexes

            #endregion Indexes

        }
    }
}
