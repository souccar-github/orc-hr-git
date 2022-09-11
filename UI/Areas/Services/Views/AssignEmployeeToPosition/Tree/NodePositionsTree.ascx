<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="HRIS.Domain.OrgChart.ValueObjects" %>

<fieldset class="ParentFieldset">
        <legend class="ParentLegend">
        <%:Resources.Areas.Services.AssignEmployeeToPosition.AssignEmployeeToPositionModel.NodePositionsTreeFieldsetTitle %></legend>
    <%= Html.Telerik().TreeView()
        .Name("NodePositionsTreeView")
        .BindTo((IEnumerable) Model, mappings =>
        {
            mappings.For<Node>(binding => binding
                    .ItemDataBound((item, node) =>
                                       {
                                           item.Text = node.Name;
                                           item.Value = "Node";
                                           item.ImageUrl = ("~/Content/images/NodePositionsTree/generic.png");
                                           item.Expanded = (int)ViewData["PositionID"] != 0;
                                       })
                    .Children(node => node.Positions)
                    );

            mappings.For<Position>(binding => binding
                    .ItemDataBound((item, position) =>
                    {
                        if (position != null && position.Code != null)
                        {
                            item.Text = position.Code;
                            item.Value = position.Id.ToString();
                            item.ImageUrl = ("~/Content/images/NodePositionsTree/male_ueser.png");
                            item.Expanded = true;
                            item.Selected= (int)ViewData["PositionID"] == position.Id;
                        }
                    }));
        })
        .ClientEvents(events => events.OnSelect("onNodeClicked"))

    %>
</fieldset>

