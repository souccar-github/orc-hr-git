<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/JobDesc/Views/Shared/JobDesc.master"
    Inherits="System.Web.Mvc.ViewPage<HRIS.Domain.JobDesc.Indexes.AuthorityType>>" %>

<%@ Import Namespace="HRIS.Domain.JobDesc.Indexes" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("IndexesNavigator"); %>
    <fieldset class="ParentFieldset">
        <legend class="ParentLegend"><%:Resources.Areas.JobDesc.Indexes.AuthorityType.AuthorityTypeModel.Title %></legend>
        <br />
        <%:
            Html.Telerik().Grid<AuthorityType>()
                .Name("AuthorityTypeGrid")
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
                                     d.Ajax().Select("AjaxGridSelect", "AuthorityType");
                                     d.Ajax().Update("AjaxGridUpdate", "AuthorityType");
                                     d.Ajax().Delete("AjaxGridDelete", "AuthorityType");
                                     d.Ajax().Insert("AjaxGridInsert", "AuthorityType");
                                 })
                .Pageable(p=>p.PageSize(10))
                .Sortable().Filterable().ClientEvents(events => events.OnError("Grid_onError"))
                
        %>
    </fieldset>
    
</asp:Content>
