<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Personnel/Views/Shared/Personnel.master"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<HRIS.Domain.Personnel.Indexes.Race>>" %>

<%@ Import Namespace="HRIS.Domain.Personnel.Indexes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("IndexesNavigator"); %>
    <fieldset class="ParentFieldset">
        <legend class=ParentLegend><%: Resources.Areas.Personnel.Indexes.Race.RaceModel.Race%></legend>
        <br />
        <%:
            Html.Telerik().Grid<Race>()
                .Name("RacesGrid")
                .DataKeys(k => k.Add(o => o.Id))
                .ToolBar(t => t.Insert())
                .Columns(c =>
                {
                    c.Bound(o => o.Name).Width(300);
                    c.Command(s =>
                                  {
                                      s.Edit().ButtonType(GridButtonType.Image);
                                      s.Delete().ButtonType(GridButtonType.Image);
                                  }).Width(1) ;
                })
                .DataBinding(d =>
                                 {
                                     d.Ajax().Select("AjaxGridSelect", "Race");
                                     d.Ajax().Update("AjaxGridUpdate", "Race");
                                     d.Ajax().Delete("AjaxGridDelete", "Race");
                                     d.Ajax().Insert("AjaxGridInsert", "Race");
                                 })
                .Pageable(p => p.PageSize(10))
                .Sortable().Filterable().ClientEvents(events => events.OnError("Grid_onError"))
        %>
    </fieldset>
</asp:Content>
