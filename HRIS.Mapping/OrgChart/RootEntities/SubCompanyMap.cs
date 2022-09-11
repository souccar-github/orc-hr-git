using FluentNHibernate.Mapping;
using HRIS.Domain.OrganizationChart.RootEntities;
using Souccar.Core;

namespace HRIS.Mapping.OrganizationChart.RootEntities
{

    public sealed class SubCompanyMap : ClassMap<SubCompany>
    {
        public SubCompanyMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Name).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.NumberOfEmployees);
            Map(x => x.Phone).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Fax).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Mobile).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Address).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.POBox).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.WebSite).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Facebook).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.LogoPath);

            References(x => x.Location);
            References(x => x.Size);
            References(x => x.Organization);

        }
    }
}