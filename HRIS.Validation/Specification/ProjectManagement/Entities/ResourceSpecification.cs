using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.ProjectManagement.Entities;
using HRIS.Domain.ProjectManagement.RootEntities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.ProjectManagement.Entities
{
    public class ResourceSpecification : Validates<Resource>
    {
        public ResourceSpecification()
        {
            IsDefaultForType();

            Check(x => x.ItemName).Required();
            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.Comment).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            Check(x => x.Type)
               .Required()
               .Expect((defineProject, baseDefineProject) => baseDefineProject.IsTransient() == false, "")
               .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            Check(x => x.Status)
               .Required()
               .Expect((defineProject, baseDefineProject) => baseDefineProject.IsTransient() == false, "")
               .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
        }
    }
}
