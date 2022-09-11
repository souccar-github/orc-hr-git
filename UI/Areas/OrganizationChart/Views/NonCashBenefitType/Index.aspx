<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/OrganizationChart/Views/Shared/OrganizationChart.master"
    Inherits="System.Web.Mvc.ViewPage<HRIS.Domain.OrgChart.Indexes.NoneCashBenefitType>" %>

<%@ Import Namespace="HRIS.Domain.OrgChart.Indexes" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("IndexesNavigator"); %>
    <fieldset class="ParentFieldset">
        <legend class="ParentLegend"><%:Resources.Areas.OrgChart.Indexes.NoneCashBenefitType.NoneCashBenefitTypeModel.Title %></legend>
        <br />
        <%:
            Html.Telerik().Grid<NoneCashBenefitType>()
                .Name("NoneCashBenefitTypeGrid")
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
                    d.Ajax().Select("AjaxGridSelect", "NonCashBenefitType");
                    d.Ajax().Update("AjaxGridUpdate", "NonCashBenefitType");
                    d.Ajax().Delete("AjaxGridDelete", "NonCashBenefitType");
                    d.Ajax().Insert("AjaxGridInsert", "NonCashBenefitType");
                })
                .Pageable(p => p.PageSize(10))
                .Sortable().Filterable().ClientEvents(events => events.OnError("Grid_onError"))
        %>
    </fieldset>
</asp:Content>
