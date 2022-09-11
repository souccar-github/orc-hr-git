<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/JobDesc/Views/Shared/JobDesc.master"
    Inherits="System.Web.Mvc.ViewPage<HRIS.Domain.JobDesc.Indexes.LanguageName>" %>

<%@ Import Namespace="HRIS.Domain.JobDesc.Indexes" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("IndexesNavigator"); %>
    <fieldset class="ParentFieldset">
        <legend class="ParentLegend"><%:Resources.Areas.JobDesc.Indexes.LanguageName.LanguageNameModel.Title %></legend>
        <br />
        <%:
            Html.Telerik().Grid<LanguageName>()
                .Name("LanguageNameGrid")
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
                                     d.Ajax().Select("AjaxGridSelect", "LanguageName");
                                     d.Ajax().Update("AjaxGridUpdate", "LanguageName");
                                     d.Ajax().Delete("AjaxGridDelete", "LanguageName");
                                     d.Ajax().Insert("AjaxGridInsert", "LanguageName");
                                 })
                .Pageable(p=>p.PageSize(10))
                .Sortable().Filterable().ClientEvents(events => events.OnError("Grid_onError"))
        %>
    </fieldset>
</asp:Content>
