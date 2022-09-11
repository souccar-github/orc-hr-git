using HRIS.Domain.JobDescription.Configurations;
using HRIS.Domain.JobDescription.RootEntities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.JobDescription.Configurations
{
    public class PositionCodeSpecification : Validates<PositionCode>
    {
        public PositionCodeSpecification()
        {
            IsDefaultForType();

            #region Primitive Types
            Check(x => x.FixedPrefix).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.FixedSuffix).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.CustomPrefix).Required();
            Check(x => x.CustomPrefixLength).Required().Between(GlobalConstant.MinimumCustomPrefixLength, GlobalConstant.MaximumCustomPrefixLength);
            Check(x => x.CustomPrefixStartingPosition).Optional().Between(GlobalConstant.MinimumCustomPrefixStartingPosition, GlobalConstant.MaximumCustomPrefixStartingPosition);
            Check(x => x.CustomSuffix).Required();
            Check(x => x.CustomSuffixLength).Required().Between(GlobalConstant.MinimumCustomSuffixLengthn, GlobalConstant.MaximumCustomSuffixLength);
            Check(x => x.CustomSuffixStartingPosition).Optional().Between(GlobalConstant.MinimumCustomSuffixStartingPosition, GlobalConstant.MaximumCustomSuffixStartingPosition);
            Check(x => x.SeparatorSymbol).Required();
            #endregion

        }
    }


}
