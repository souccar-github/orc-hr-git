<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.PMS.ValueObjects.Implementation.Competency.CompetencySectionItem>" %>
<%@ Import Namespace="HRIS.Domain.PMS.ValueObjects.Implementation.Competency" %>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td>
            <%
                Html.Telerik().Grid<CompetencySectionItem>("competencySectionItems")
                    .Name("CompetencySectionItemsGrid")
                    .DataKeys(k => k.Add(o => o.Id))
                    .Columns(c =>
                    {
                        c.Bound(o => o.Id).Title("No");
                        c.Bound(o => o.Name);
                        c.Bound(o => o.Type);
                        c.Bound(o => o.Level);
                        c.Bound(o => o.Weight);
                        c.Bound(o => o.Rate);
                    }).Footer(false)
                     .Render();
            %>
        </td>
    </tr>
</table>
