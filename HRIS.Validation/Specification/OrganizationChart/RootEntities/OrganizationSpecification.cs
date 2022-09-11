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
    public class OrganizationSpecification : Validates<Organization>
    {
        public OrganizationSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.Name).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
           

            #endregion

            #region Indexes

            var pattern = @"^(http|https|ftp|)\://|[a-zA-Z0-9\-\.]+\.[a-zA-Z](:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$";
            
            Check(x => x.Location)
                .Required()
                .Expect((node, location) => location.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Phone).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength).And.IsNumeric();
            Check(x => x.Fax).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength).And.IsNumeric();
            Check(x => x.Mobile).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength).And.IsNumeric();

            Check(x => x.Address).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            Check(x => x.POBox).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength);

            Check(x => x.WebSite).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength).And.Matches(pattern)
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.NotMatches));
            Check(x => x.Facebook).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength).And.Matches(pattern)
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.NotMatches));

            #endregion
        }
    }
}
