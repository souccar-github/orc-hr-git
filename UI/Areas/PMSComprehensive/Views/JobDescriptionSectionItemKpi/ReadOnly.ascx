<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.PMS.ValueObjects.Implementation.JobDescription.JobDescriptionSectionItemKpi>" %>
<%@ Import Namespace="HRIS.Domain.PMS.ValueObjects.Implementation.JobDescription" %>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td>
            <%
                Html.Telerik().Grid<JobDescriptionSectionItemKpi>("jobDescriptionSectionItemKpis")
                    .Name("JobDescriptionSectionItemKpisGrid")
                    .DataKeys(k => k.Add(o => o.Id))
                    .Columns(c =>
                    {
                        c.Bound(o => o.Id).Title("No");
                        c.Bound(o => o.Value);
                        c.Bound(o => o.Description);
                    }).Footer(false)
                     .Render();
            %>
        </td>
    </tr>
</table>
