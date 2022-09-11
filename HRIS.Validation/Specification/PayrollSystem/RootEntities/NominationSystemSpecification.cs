using HRIS.Domain.PayrollSystem.RootEntities;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.RootEntities
{
    public class NominationSystemSpecification : Validates<NominationSystem>
    {
        public NominationSystemSpecification()
        {
            IsDefaultForType();

            Check(x => x.EmploymentStatus, y => typeof(NominationSystem).GetProperty("EmploymentStatus").GetTitle()).Required();
            Check(x => x.PaymentType, y => typeof(NominationSystem).GetProperty("PaymentType").GetTitle()).Required();
        }
    }
}
