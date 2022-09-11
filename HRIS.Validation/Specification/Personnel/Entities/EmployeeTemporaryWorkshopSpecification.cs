
using HRIS.Domain.Personnel.Entities;
using SpecExpress;

namespace HRIS.Validation.Specification.Personnel.Entities
{
    public class EmployeeTemporaryWorkshopSpecification : Validates<EmployeeTemporaryWorkshop>
    {
        public EmployeeTemporaryWorkshopSpecification()
        {
            IsDefaultForType();

            Check(x => x.FromDate).Required().LessThanEqualTo(x => x.ToDate);
            Check(x => x.ToDate).Required();
            
        }
    }
}
