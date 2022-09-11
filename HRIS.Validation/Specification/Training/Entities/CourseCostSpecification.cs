using HRIS.Domain.Training.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Training.Entities
{
    public class CourseCostSpecification: Validates<CourseCost>
    {
        public CourseCostSpecification()
        {
            IsDefaultForType();

            Check(x => x.Cost).Required().GreaterThan(0);
            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            Check(x => x.Name)
                  .Required()
                  .Expect((courseCost, name) => name.IsTransient() == false, "")
                  .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));


            Check(x => x.CostCenter)
                 .Required()
                 .Expect((courseCost, costCenter) => costCenter.IsTransient() == false, "")
                 .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
        }
    }
}
      