<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/ProjectManagement/Views/Shared/ProjectManagement.master" Inherits="System.Web.Mvc.ViewPage<HRIS.Domain.ProjectManagment.Indexes.ProjectKpiType>" %>


<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("IndexesNavigator"); %>
    <fieldset class="ParentFieldset">
        <legend class="ParentLegend"><%:Resources.Areas.ProjectManagment.Indexes.ProjectKpiType.ProjectKpiTypeModel.Title %></legend>
        <br />
        <%:
            Html.Telerik().Grid<Model.ProjectManagment.Indexes.ProjectKpiType>()
                .Name("ProjectKpiTypeGrid")
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
                                     d.Ajax().Select("AjaxGridSelect", "ProjectKpiType");
                                     d.Ajax().Update("AjaxGridUpdate", "ProjectKpiType");
                                     d.Ajax().Delete("AjaxGridDelete", "ProjectKpiType");
                                     d.Ajax().Insert("AjaxGridInsert", "ProjectKpiType");
                                 })
                .Pageable(p=>p.PageSize(10))
                .Sortable()
                .Filterable()
                .ClientEvents(events => events.OnError("Grid_onError"))
        %>
    </fieldset>
</asp:Content>
