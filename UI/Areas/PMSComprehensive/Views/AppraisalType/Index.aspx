<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/PMSComprehensive/Views/Shared/PMSComprehensive.master" Inherits="System.Web.Mvc.ViewPage<HRIS.Domain.PMS.Indexes.AppraisalType>" %>


<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("IndexesNavigator"); %>
    <fieldset class="ParentFieldset">
        <legend class="ParentLegend">Appraisal Type</legend>
        <br />
        <%:
            Html.Telerik().Grid<Model.PMS.Indexes.AppraisalType>()
                            .Name("AppraisalTypeGrid")
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
                                     d.Ajax().Select("AjaxGridSelect", "AppraisalType");
                                     d.Ajax().Update("AjaxGridUpdate", "AppraisalType");
                                     d.Ajax().Delete("AjaxGridDelete", "AppraisalType");
                                     d.Ajax().Insert("AjaxGridInsert", "AppraisalType");
                                 })
                .Pageable(p=>p.PageSize(10))
                .Sortable()
                .Filterable()
                .ClientEvents(events => events.OnError("Grid_onError"))
        %>
    </fieldset>
</asp:Content>

