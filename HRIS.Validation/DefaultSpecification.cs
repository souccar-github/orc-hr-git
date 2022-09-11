using Souccar.Domain.DomainModel;
using SpecExpress;

namespace HRIS.Validation
{
    public class DefaultSpecification : Validates<IEntityWithTypedId<int>>
    {
        public DefaultSpecification()
        {
           
        }
    }
}
