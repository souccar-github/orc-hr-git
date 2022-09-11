<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/JobDesc/Views/Shared/JobDesc.master"
    Inherits="System.Web.Mvc.ViewPage<HRIS.Domain.JobDesc.Indexes.Priority>>" %>

<%@ Import Namespace="HRIS.Domain.JobDesc.Indexes" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("IndexesNavigator"); %>
    <fieldset class="ParentFieldset">
        <legend class="ParentLegend"><%:Resources.Areas.JobDesc.Indexes.Priority.PriorityModel.Title %></legend>
        <br />
        <%:
            Html.Telerik().Grid<Priority>()
                .Name("PriorityGrid")
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
                                     d.Ajax().Select("AjaxGridSelect", "Priority");
                                     d.Ajax().Update("AjaxGridUpdate", "Priority");
                                     d.Ajax().Delete("AjaxGridDelete", "Priority");
                                     d.Ajax().Insert("AjaxGridInsert", "Priority");
                                 })
                .Pageable(p => p.PageSize(10))
                .Sortable().Filterable().ClientEvents(events => events.OnError("Grid_onError"))
        %>
    </fieldset>
</asp:Content>
