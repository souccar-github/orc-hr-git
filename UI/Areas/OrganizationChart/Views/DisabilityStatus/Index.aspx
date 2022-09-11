<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/OrganizationChart/Views/Shared/OrganizationChart.master" 
Inherits="System.Web.Mvc.ViewPage<HRIS.Domain.OrgChart.Indexes.DisabilityStatus>" %>
<%@ Import Namespace="HRIS.Domain.OrgChart.Indexes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <% Html.RenderPartial("IndexesNavigator"); %>
    <fieldset class="ParentFieldset">
        <legend class=ParentLegend><%:Resources.Areas.OrgChart.Indexes.DisabilityStatus.DisabilityStatusModel.Title %></legend>
        <br />
        <%:
            Html.Telerik().Grid<DisabilityStatus>()
                .Name("DisabilityStatusGrid")
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
                                     d.Ajax().Select("AjaxGridSelect", "DisabilityStatus");
                                     d.Ajax().Update("AjaxGridUpdate", "DisabilityStatus");
                                     d.Ajax().Delete("AjaxGridDelete", "DisabilityStatus");
                                     d.Ajax().Insert("AjaxGridInsert", "DisabilityStatus");
                                 })
                .Pageable(p=>p.PageSize(10))
                .Sortable().Filterable().ClientEvents(events => events.OnError("Grid_onError"))
        %>
    </fieldset>
</asp:Content>

