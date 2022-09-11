using HRIS.Domain.EmployeeRelationServices.Entities;
using SpecExpress;

namespace HRIS.Validation.Specification.EmployeeRelationServices.Entities
{
    public class LeaveAttachmentSpecification : Validates<LeaveAttachment>
    {
        public LeaveAttachmentSpecification()
        {
            IsDefaultForType();

            Check(x => x.FilePath).Required();
        }
    }
}
