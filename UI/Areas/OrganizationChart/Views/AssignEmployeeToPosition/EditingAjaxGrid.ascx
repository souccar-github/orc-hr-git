<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="Model.OrgChart.ValueObjects" %>


<%= Html.Telerik().Grid<PositionFulfillment>()
        .Name("Grid")
        .Columns(columns =>
        {
            columns.Bound(o => o.Id).Width(100);
            columns.Bound(o => o.Weight);
        })
            .DataBinding(dataBinding => dataBinding.Ajax().Select("_AjaxBinding", "AssignEmployeeToPosition"))
        .Pageable()
        .Sortable()
        .Scrollable()
        .Groupable()
        .Filterable()
%>


