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
    public class SubCompanySpecification : Validates<SubCompany>
    {
        public SubCompanySpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            var pattern = @"^(http|https|ftp|)\://|[a-zA-Z0-9\-\.]+\.[a-zA-Z](:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$";
            
            Check(x => x.Name).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.NumberOfEmployees).Required().GreaterThan(GlobalConstant.MinimumValue);           

            //Check(x => x.Organization).Required();


            Check(x => x.Phone).Required().MaxLength(GlobalConstant.SimpleStringMaxLength).And.IsNumeric();
            Check(x => x.Fax).Required().MaxLength(GlobalConstant.SimpleStringMaxLength).And.IsNumeric();
            Check(x => x.Mobile).Required().MaxLength(GlobalConstant.SimpleStringMaxLength).And.IsNumeric();

            Check(x => x.Address).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            Check(x => x.POBox).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength);

            Check(x => x.WebSite).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength).And.Matches(pattern)
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.NotMatches));
            Check(x => x.Facebook).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength).And.Matches(pattern)
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.NotMatches));

            #endregion

            #region Indexes

            Check(x => x.Location)
                .Required()
                .Expect((node, location) => location.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Size)
                .Required()
                .Expect((node, size) => size.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

           

            #endregion
        }
    }
}
