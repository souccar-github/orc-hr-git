using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.EmployeeRelationServices.Entities
{
    public class InfractionSliceSpecification : Validates<InfractionSlice>
    {
        public InfractionSliceSpecification()
        {
            IsDefaultForType();
            Check(x => x.MinimumRecurrence).Required().GreaterThan(0).And.LessThanEqualTo(x => x.MaximumRecurrence);
            Check(x => x.MaximumRecurrence).Required().GreaterThan(0);
            Check(x => x.Penalty).Required();

            //Check(x => x.Penalty)
            //        .Required()
            //        .Expect((infractionSlice, penalty) => penalty.IsTransient() == false, "")
            //        .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
        }
    }
}
