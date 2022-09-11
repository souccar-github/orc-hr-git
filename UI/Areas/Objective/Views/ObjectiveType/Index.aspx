<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Objective/Views/Shared/Objective.master" Inherits="System.Web.Mvc.ViewPage<HRIS.Domain.Objectives.Indexes.ObjectiveType>" %>


<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("IndexesNavigator"); %>
    <fieldset class="ParentFieldset">
        <legend class="ParentLegend"><%:Resources.Areas.Objective.Indexes.ObjectiveType.ObjectiveTypeModel.ObjectiveType %></legend>
        <br />
        <%:
            Html.Telerik().Grid<Model.Objectives.Indexes.ObjectiveType>()
                .Name("ObjectiveTypeGrid")
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
                                     d.Ajax().Select("AjaxGridSelect", "ObjectiveType");
                                     d.Ajax().Update("AjaxGridUpdate", "ObjectiveType");
                                     d.Ajax().Delete("AjaxGridDelete", "ObjectiveType");
                                     d.Ajax().Insert("AjaxGridInsert", "ObjectiveType");
                                 })
                .Pageable(p=>p.PageSize(10))
                .Sortable()
                .Filterable()
                .ClientEvents(events => events.OnError("Grid_onError"))
        %>
    </fieldset>
</asp:Content>
