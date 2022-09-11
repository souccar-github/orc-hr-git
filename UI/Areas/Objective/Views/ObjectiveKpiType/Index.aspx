<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Objective/Views/Shared/Objective.master" Inherits="System.Web.Mvc.ViewPage<HRIS.Domain.Objectives.Indexes.ObjectiveKpiType>" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("IndexesNavigator"); %>
    <fieldset class="ParentFieldset">
        <legend class="ParentLegend"><%:Resources.Areas.Objective.Indexes.ObjectiveKpiType.ObjectiveKpiTypeModel.ObjectiveKPITypes %></legend>
        <br />
        <%:
            Html.Telerik().Grid<HRIS.Domain.Objectives.Indexes.ObjectiveKpiType>()
                .Name("ObjectiveKpiTypeGrid")
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
                                     d.Ajax().Select("AjaxGridSelect", "ObjectiveKpiType");
                                     d.Ajax().Update("AjaxGridUpdate", "ObjectiveKpiType");
                                     d.Ajax().Delete("AjaxGridDelete", "ObjectiveKpiType");
                                     d.Ajax().Insert("AjaxGridInsert", "ObjectiveKpiType");
                                 })
                .Pageable(p=>p.PageSize(10))
                .Sortable()
                .Filterable()
                .ClientEvents(events => events.OnError("Grid_onError"))
        %>
    </fieldset>
</asp:Content>
