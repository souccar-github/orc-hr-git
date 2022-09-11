using HRIS.Domain.JobDescription.Entities;
using SpecExpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Validation.Specification.JobDescription.Entities
{
    public class WorkingRestrictionSpecification : Validates<WorkingRestriction>
    {
        public WorkingRestrictionSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.InternalRelationships).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.ExternalRelationships).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            #endregion Primitive Types

            #region Indexes

            #endregion Indexes
        }
    }
}
