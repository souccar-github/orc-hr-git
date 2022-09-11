<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Personnel/Views/Shared/Personnel.master" Inherits="System.Web.Mvc.ViewPage<HRIS.Domain.Personnel.Indexes.ConvictionRule>" %>
<%@ Import Namespace="HRIS.Domain.Personnel.Indexes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("IndexesNavigator"); %>
	<fieldset class="ParentFieldset">
		<legend class=ParentLegend><%: Resources.Areas.Personnel.Indexes.ConvictionRule.ConvictionRuleModel.ConvictionRule %></legend>
		<br />
		<%:
			Html.Telerik().Grid<ConvictionRule>()
				.Name("ConvictionRulesGrid")
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
									 d.Ajax().Select("AjaxGridSelect", "ConvictionRule");
									 d.Ajax().Update("AjaxGridUpdate", "ConvictionRule");
									 d.Ajax().Delete("AjaxGridDelete", "ConvictionRule");
									 d.Ajax().Insert("AjaxGridInsert", "ConvictionRule");
								 })
				.Pageable(p => p.PageSize(10))
                .Sortable().Filterable().ClientEvents(events => events.OnError("Grid_onError"))
		%>
	</fieldset>
</asp:Content>
