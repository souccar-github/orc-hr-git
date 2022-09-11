using HRIS.Domain.Personnel.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Personnel.Entities
{
    public class SkillSpecification : Validates<Skill>
    {
        public SkillSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.Comments).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            #endregion

            #region Indexes
            Check(x => x.SkillType)
                .Required()
                .Expect((skill, name) => name.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Level)
                .Required()
                .Expect((skill, level) => level.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            #endregion
        }
    }
}
