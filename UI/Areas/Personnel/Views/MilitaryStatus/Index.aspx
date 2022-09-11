<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Personnel/Views/Shared/Personnel.master"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<HRIS.Domain.Personnel.Indexes.MilitaryStatus>>" %>

<%@ Import Namespace="HRIS.Domain.Personnel.Indexes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("IndexesNavigator"); %>
   <fieldset class="ParentFieldset">
        <legend class=ParentLegend><%: Resources.Areas.Personnel.Indexes.MilitaryStatus.MilitaryStatusModel.MilitaryStatus %></legend>
        <br />
        <%:
            Html.Telerik().Grid<MilitaryStatus>()
                .Name("MilitaryStatusesGrid")
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
                                     d.Ajax().Select("AjaxGridSelect", "MilitaryStatus");
                                     d.Ajax().Update("AjaxGridUpdate", "MilitaryStatus");
                                     d.Ajax().Delete("AjaxGridDelete", "MilitaryStatus");
                                     d.Ajax().Insert("AjaxGridInsert", "MilitaryStatus");
                                 })
                .Pageable(p => p.PageSize(10))
                .Sortable().Filterable().ClientEvents(events => events.OnError("Grid_onError"))
        %>
    </fieldset>
</asp:Content>
