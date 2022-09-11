<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Model.OrgChart.Indexes.NodeType>" %>
<%: Html.DropDownListFor(model => model.Name, UI.Areas.OrganizationChart.Helpers.DropDownListHelpers.listofn, "Please Select ..")%>
