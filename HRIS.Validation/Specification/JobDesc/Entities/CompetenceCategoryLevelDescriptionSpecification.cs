using HRIS.Domain.JobDescription.Configurations;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Validation.MessageKeys;
using HRIS.Validation.Specification.Index;
using SpecExpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Souccar.Domain.Extensions;

namespace HRIS.Validation.Specification.JobDescription.Entities
{
    public class CompetenceCategoryLevelDescriptionSpecification : Validates<CompetenceCategoryLevelDescription>
    {
        public CompetenceCategoryLevelDescriptionSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.Description).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
          
            #endregion Primitive Types
         
       
            #region Indexes
           
            Check(x => x.Level)
                .Required()
                .Expect(IndexSpecification.IsTransient, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
           
            #endregion Indexes

        }
    }
}
