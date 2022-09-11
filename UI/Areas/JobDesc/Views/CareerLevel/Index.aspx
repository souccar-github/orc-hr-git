<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/JobDesc/Views/Shared/JobDesc.master"
    Inherits="System.Web.Mvc.ViewPage<HRIS.Domain.JobDesc.Indexes.CareerLevel>>" %>

<%@ Import Namespace="HRIS.Domain.JobDesc.Indexes" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("IndexesNavigator"); %>
    <fieldset class="ParentFieldset">
        <legend class="ParentLegend"><%:Resources.Areas.JobDesc.Indexes.CareerLevel.CareerLevelModel.Title %></legend>
        <br />
        <%:
            Html.Telerik().Grid<CareerLevel>()
                            .Name("CareerLevelGrid")
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
                                     d.Ajax().Select("AjaxGridSelect", "CareerLevel");
                                     d.Ajax().Update("AjaxGridUpdate", "CareerLevel");
                                     d.Ajax().Delete("AjaxGridDelete", "CareerLevel");
                                     d.Ajax().Insert("AjaxGridInsert", "CareerLevel");
                                 })
                .Pageable(p=>p.PageSize(10))
                .Sortable().Filterable().ClientEvents(events => events.OnError("Grid_onError"))
        %>
    </fieldset>
</asp:Content>
