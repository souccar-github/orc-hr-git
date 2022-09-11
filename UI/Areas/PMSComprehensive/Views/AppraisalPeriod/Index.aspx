<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/PMSComprehensive/Views/Shared/PMSComprehensive.master" Inherits="System.Web.Mvc.ViewPage<HRIS.Domain.PMS.Indexes.AppraisalPeriod>" %>


<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("IndexesNavigator"); %>
    <fieldset class="ParentFieldset">
        <legend class="ParentLegend">Appraisal Period</legend>
        <br />
        <%:
            Html.Telerik().Grid<Model.PMS.Indexes.AppraisalPeriod>()
                            .Name("AppraisalPeriodGrid")
                .DataKeys(k => k.Add(o => o.Id))
                .ToolBar(t => t.Insert())
                .Columns(c =>
                {
                    c.Bound(o => o.Period).Width(300);
                    c.Command(s =>
                                  {
                                      s.Edit().ButtonType(GridButtonType.Image);
                                      s.Delete().ButtonType(GridButtonType.Image);
                                  }).Width(1);
                })
                .DataBinding(d =>
                                 {
                                     d.Ajax().Select("AjaxGridSelect", "AppraisalPeriod");
                                     d.Ajax().Update("AjaxGridUpdate", "AppraisalPeriod");
                                     d.Ajax().Delete("AjaxGridDelete", "AppraisalPeriod");
                                     d.Ajax().Insert("AjaxGridInsert", "AppraisalPeriod");
                                 })
                .Pageable(p=>p.PageSize(10))
                .Sortable()
                .Filterable()
                .ClientEvents(events => events.OnError("Grid_onError"))
        %>
    </fieldset>
</asp:Content>

