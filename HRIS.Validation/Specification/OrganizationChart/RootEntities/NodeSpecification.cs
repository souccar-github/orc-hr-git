using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.OrganizationChart.RootEntities
{
    public class NodeSpecification : Validates<Node>
    {
        public NodeSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.Name).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.Code).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
           

            #endregion

            #region Indexes

            Check(x => x.Type)
                .Required()
                .Expect((node, type) => type.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

           

            #endregion
        }
    }
}
