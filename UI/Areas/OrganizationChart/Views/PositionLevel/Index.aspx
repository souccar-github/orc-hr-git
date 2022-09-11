<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/OrganizationChart/Views/Shared/OrganizationChart.master" 
Inherits="System.Web.Mvc.ViewPage<HRIS.Domain.OrgChart.Indexes.PositionLevel>" %>
<%@ Import Namespace="HRIS.Domain.OrgChart.Indexes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <% Html.RenderPartial("IndexesNavigator"); %>
    <fieldset class="ParentFieldset">
        <legend class=ParentLegend><%:Resources.Areas.OrgChart.Indexes.PositionLevel.PositionLevelModel.Title %></legend>
        <br />
        <%:
            Html.Telerik().Grid<PositionLevel>()
                .Name("PositionLevelGrid")
                .DataKeys(k => k.Add(o => o.Id))
                .ToolBar(t => t.Insert())
                .Columns(c =>
                {
                    c.Bound(o => o.Name).Width(300);
                    c.Command(s =>
                                  {
                                      s.Edit();
                                      s.Delete();
                                  }).Width(100).Title("Commands"); ;
                })
                .DataBinding(d =>
                                 {
                                     d.Ajax().Select("AjaxGridSelect", "PositionLevel");
                                     d.Ajax().Update("AjaxGridUpdate", "PositionLevel");
                                     d.Ajax().Delete("AjaxGridDelete", "PositionLevel");
                                     d.Ajax().Insert("AjaxGridInsert", "PositionLevel");
                                 })
                .Pageable(p=>p.PageSize(10))
                .Sortable().Filterable().ClientEvents(events => events.OnError("Grid_onError"))
        %>
    </fieldset>
</asp:Content>

