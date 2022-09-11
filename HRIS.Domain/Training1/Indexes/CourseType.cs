#region

using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.Training1.Indexes
{
    [Module("Training1")]
    public class CourseType : IndexEntity, IAggregateRoot
    {
    }
}