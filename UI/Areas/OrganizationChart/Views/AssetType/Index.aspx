<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/OrganizationChart/Views/Shared/OrganizationChart.master" Inherits="System.Web.Mvc.ViewPage<HRIS.Domain.OrgChart.Indexes.AssetType>" %>
<%@ Import Namespace="HRIS.Domain.OrgChart.Indexes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <% Html.RenderPartial("IndexesNavigator"); %>
    <fieldset class="ParentFieldset">
        <legend class=ParentLegend><%:Resources.Areas.OrgChart.Indexes.AssetType.AssetTypeModel.Title %></legend>
        <br />
        <%:
            Html.Telerik().Grid<AssetType>()
                .Name("AssetTypeGrid")
                .DataKeys(k => k.Add(o => o.Id))
                .ToolBar(t => t.Insert())
                .Columns(c =>
                {
                    c.Bound(o => o.Name).Width(300);
                    c.Command(s =>
                    {
                        s.Edit().ButtonType(GridButtonType.Image);
                        s.Delete().ButtonType(GridButtonType.Image);
                    }).Width(1);
                })
                .DataBinding(d =>
                                 {
                                     d.Ajax().Select("AjaxGridSelect", "AssetType");
                                     d.Ajax().Update("AjaxGridUpdate", "AssetType");
                                     d.Ajax().Delete("AjaxGridDelete", "AssetType");
                                     d.Ajax().Insert("AjaxGridInsert", "AssetType");
                                 })
                .Pageable(p=>p.PageSize(10))
                .Sortable().Filterable().ClientEvents(events => events.OnError("Grid_onError"))
        %>
    </fieldset>
</asp:Content>
