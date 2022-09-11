using HRIS.Domain.JobDescription.Entities;
using HRIS.Validation.MessageKeys;
using HRIS.Validation.Specification.Index;
using SpecExpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Validation.Specification.JobDescription.Entities
{
    public class DelegateRolesToPositionSpecification : Validates<DelegateRolesToPosition>
    {
        public DelegateRolesToPositionSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.SourcePosition).Required();
            Check(x => x.DestinationPosition).Required();
            Check(x => x.ToDate).Required();
            Check(x => x.FromDate).Required();
            Check(x => x.Superior).Required();
            Check(x => x.DelegationComment).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.DelegationReason).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            #endregion Primitive Types
        }
    }
}
