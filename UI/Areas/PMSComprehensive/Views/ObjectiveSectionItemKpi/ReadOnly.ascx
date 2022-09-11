<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.PMS.ValueObjects.Implementation.Objective.ObjectiveSectionItemKpi>" %>
<%@ Import Namespace="HRIS.Domain.PMS.ValueObjects.Implementation.Objective" %>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td>
            <%
                Html.Telerik().Grid<ObjectiveSectionItemKpi>("ObjectiveSectionItemKpis")
                    .Name("ObjectiveSectionItemKpisGrid")
                    .DataKeys(k => k.Add(o => o.Id))
                    .Columns(c =>
                    {
                        c.Bound(o => o.Id).Title("No");
                        c.Bound(o => o.Value);
                        c.Bound(o => o.Type);
                        c.Bound(o => o.Weight);
                    }).Footer(false)
                     .Render();
            %>
        </td>
    </tr>
</table>
