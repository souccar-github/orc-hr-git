<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.OrgChart.ValueObjects.Position>" %>
<%@ Import Namespace="HRIS.Domain.OrgChart.ValueObjects" %>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td>
            <%
                Html.Telerik().Grid<Position>("Positions")
                    .Name("PositionsGrid")
                    .DataKeys(k => k.Add(o => o.Id))
                    .Columns(c =>
                    {
                        c.Bound(o => o.Id).Width(100);
                        c.Bound(o => o.Code).Width(100);
                        c.Bound(o => o.JobTitle.Name).Width(100).Title(Resources.Areas.OrgChart.ValueObjects.Position.PositionModel.JobTitle);
                        
                    }).Footer(false)
                     .Render();
            %>
        </td>
    </tr>
</table>
