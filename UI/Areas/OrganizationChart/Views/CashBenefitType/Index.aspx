<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/OrganizationChart/Views/Shared/OrganizationChart.master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="HRIS.Domain.OrgChart.Indexes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <% Html.RenderPartial("IndexesNavigator"); %>
    <fieldset class="ParentFieldset">
        <legend class=ParentLegend><%:Resources.Areas.OrgChart.Indexes.CashBenefitType.CashBenefitTypeModel.Title %></legend>
        <br />
        <%if (ViewData["Error"] != null)
          {%>
        <%:Html.TextBox(ViewData["Error"].ToString())%>
        <%
          }%>
        <%:
            Html.Telerik().Grid<CashBenefitType>()
                .Name("CashBenefitTypeGrid")
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
                                     d.Ajax().Select("AjaxGridSelect", "CashBenefitType");
                                     d.Ajax().Update("AjaxGridUpdate", "CashBenefitType");
                                     d.Ajax().Delete("AjaxGridDelete", "CashBenefitType");
                                     d.Ajax().Insert("AjaxGridInsert", "CashBenefitType");
                                 })
                .Pageable(p=>p.PageSize(10))
                .Sortable().Filterable().ClientEvents(events => events.OnError("Grid_onError"))
        %>
    </fieldset>
</asp:Content>