<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/ProjectManagement/Views/Shared/ProjectManagement.master" Inherits="System.Web.Mvc.ViewPage<HRIS.Domain.ProjectManagment.Indexes.ResourceStatus>" %>


<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("IndexesNavigator"); %>
    <fieldset class="ParentFieldset">
        <legend class="ParentLegend"><%:Resources.Areas.ProjectManagment.Indexes.ResourceStatus.ResourceStatusModel.Title %></legend>
        <br />
        <%:
            Html.Telerik().Grid<Model.ProjectManagment.Indexes.ResourceStatus>()
                .Name("ResourceStatusGrid")
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
                                     d.Ajax().Select("AjaxGridSelect", "ResourceStatus");
                                     d.Ajax().Update("AjaxGridUpdate", "ResourceStatus");
                                     d.Ajax().Delete("AjaxGridDelete", "ResourceStatus");
                                     d.Ajax().Insert("AjaxGridInsert", "ResourceStatus");
                                 })
                .Pageable(p=>p.PageSize(10))
                .Sortable()
                .Filterable()
                .ClientEvents(events => events.OnError("Grid_onError"))
        %>
    </fieldset>
</asp:Content>
