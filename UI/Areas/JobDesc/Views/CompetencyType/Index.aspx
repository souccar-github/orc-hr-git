<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/JobDesc/Views/Shared/JobDesc.master"
    Inherits="System.Web.Mvc.ViewPage<HRIS.Domain.JobDesc.Indexes.CompetencyType>" %>

<%@ Import Namespace="HRIS.Domain.JobDesc.Indexes" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("IndexesNavigator"); %>
    <fieldset class="ParentFieldset">
        <legend class="ParentLegend"><%:Resources.Areas.JobDesc.Indexes.CompetencyType.CompetencyTypeModel.Title %></legend>
        <br />
        <%:
            Html.Telerik().Grid<CompetencyType>()
                .Name("competencyTypeGrid")
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
                                     d.Ajax().Select("AjaxGridSelect", "CompetencyType");
                                     d.Ajax().Update("AjaxGridUpdate", "CompetencyType");
                                     d.Ajax().Delete("AjaxGridDelete", "CompetencyType");
                                     d.Ajax().Insert("AjaxGridInsert", "CompetencyType");
                                 })
                .Pageable(p => p.PageSize(10))
                .Sortable().Filterable().ClientEvents(events => events.OnError("Grid_onError"))
        %>
    </fieldset>
</asp:Content>
