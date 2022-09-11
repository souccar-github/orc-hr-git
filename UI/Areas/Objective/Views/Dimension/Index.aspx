<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Objective/Views/Shared/Objective.master" Inherits="System.Web.Mvc.ViewPage<HRIS.Domain.Objectives.Indexes.Dimension>" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("IndexesNavigator"); %>
    <fieldset class="ParentFieldset">
        <legend class="ParentLegend">Dimension</legend>
        <br />
        <%:
            Html.Telerik().Grid<HRIS.Domain.Objectives.Indexes.Dimension>()
                .Name("DimensionGrid")
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
                                     d.Ajax().Select("AjaxGridSelect", "Dimension");
                                     d.Ajax().Update("AjaxGridUpdate", "Dimension");
                                     d.Ajax().Delete("AjaxGridDelete", "Dimension");
                                     d.Ajax().Insert("AjaxGridInsert", "Dimension");
                                 })
                .Pageable(p=>p.PageSize(10))
                .Sortable()
                .Filterable()
                .ClientEvents(events => events.OnError("Grid_onError"))
        %>
    </fieldset>
</asp:Content>
