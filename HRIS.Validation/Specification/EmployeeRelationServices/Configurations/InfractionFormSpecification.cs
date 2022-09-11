using HRIS.Domain.EmployeeRelationServices.Configurations;
using SpecExpress;

namespace HRIS.Validation.Specification.EmployeeRelationServices.Configurations
{
    public class InfractionFormSpecification : Validates<InfractionForm>
    {
        public InfractionFormSpecification()
        {
            IsDefaultForType();
            Check(x => x.Number).Required().GreaterThan(0);
            Check(x => x.Infraction).Required();
            Check(x => x.Description).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
        }
    }
}
