<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Objective/Views/Shared/Objective.master" Inherits="System.Web.Mvc.ViewPage<HRIS.Domain.Objectives.Indexes.StepStatus>" %>


<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("IndexesNavigator"); %>
    <fieldset class="ParentFieldset">
        <legend class="ParentLegend"><%:Resources.Areas.Objective.Indexes.StepStatus.StepStatusModel.ObjectiveStepStatus %></legend>
        <br />
        <%:
            Html.Telerik().Grid<Model.Objectives.Indexes.StepStatus>()
                .Name("StepStatusGrid")
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
                                     d.Ajax().Select("AjaxGridSelect", "StepStatus");
                                     d.Ajax().Update("AjaxGridUpdate", "StepStatus");
                                     d.Ajax().Delete("AjaxGridDelete", "StepStatus");
                                     d.Ajax().Insert("AjaxGridInsert", "StepStatus");
                                 })
                .Pageable(p=>p.PageSize(10))
                .Sortable()
                .Filterable()
                .ClientEvents(events => events.OnError("Grid_onError"))
        %>
    </fieldset>
</asp:Content>
