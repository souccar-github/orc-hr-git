using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.ProjectManagement.RootEntities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.ProjectManagement.RootEntities
{
    public class ProjectSpecification : Validates<Project>
    {
        public ProjectSpecification()
        {
            IsDefaultForType();

            Check(x => x.Name).Required();
            Check(x => x.Code).Required();
            Check(x => x.Status).Required();
            Check(x => x.Node).Required();
            Check(x => x.Position).Required();
            Check(x => x.PlannedEndingDate).Required().GreaterThan(x=>x.PlannedStartingDate);
            Check(x => x.PlannedStartingDate).Required();
            Check(x => x.ActualEndingDate).Optional().GreaterThan(x=>x.ActualStartingDate);
            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            Check(x => x.Type)
               .Required()
               .Expect((defineProject, baseDefineProject) => baseDefineProject.IsTransient() == false, "")
               .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
        }
    }
}
