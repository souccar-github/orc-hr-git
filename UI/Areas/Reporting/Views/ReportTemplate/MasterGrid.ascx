<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="Souccar.ReportGenerator.Domain.Classification" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%: Resources.Areas.ReportGenerator.Domain.Entities.ReportTemplate.ReportTemplateModel.ReportTemplateGridTitle %></legend>
    <table width="100%">
        <tr>
            <td>
                <%
                    Html.Telerik().Grid<ReportTemplate>("reportTemplates")
                        .Name("Grid")
                        .DataKeys(k => k.Add(o => o.Id))
                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "ReportTemplate"))
                        .DataBinding(dataBinding => dataBinding.Server().Insert("Insert", "ReportTemplate"))
                        .Scrollable(builder => builder.Height(350))
                        .ToolBar(toolBar => toolBar.Template(() =>
                        { 
                %>
                <table class="GridToolBar">
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <a href="<%:Url.Action("Insert", "ReportTemplate")%>">
                                <img src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>" title="<%: Resources.Shared.Buttons.Function.Add %>"
                                    alt="<%: Resources.Shared.Buttons.Function.Add %>" height="36" width="36" align="middle" />
                            </a>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <% 
                        }))
                        .Filterable()
                        .Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                        .Columns(c =>
                                     {
                                         c.Bound(b => b.Id).Width(35).Title(Resources.Shared.Model.Fields.Id).Groupable(false).Filterable(false).Sortable(false);
                                         c.Bound(o => o.Name).Sortable(true).Title(Resources.Areas.ReportGenerator.Domain.Entities.ReportTemplate.ReportTemplateModel.Name);
                                         c.Bound(o => o.Content.ShowDateTime).Sortable(false).Title(Resources.Areas.ReportGenerator.Domain.Entities.ReportTemplate.ReportTemplateModel.ShowDateTime);
                                         c.Bound(o => o.Content.ShowUserName).Sortable(false).Title(Resources.Areas.ReportGenerator.Domain.Entities.ReportTemplate.ReportTemplateModel.ShowUserName);
                                         c.Bound(o => o.Content.ShowPageNumber).Sortable(false).Title(Resources.Areas.ReportGenerator.Domain.Entities.ReportTemplate.ReportTemplateModel.ShowPageNumber);
                                         c.Bound(o => o.Content.ShowHeader).Sortable(false).Title(Resources.Areas.ReportGenerator.Domain.Entities.ReportTemplate.ReportTemplateModel.ShowHeader);
                                         c.Bound(o => o.Content.ShowFooter).Sortable(false).Title(Resources.Areas.ReportGenerator.Domain.Entities.ReportTemplate.ReportTemplateModel.ShowFooter);
                                         c.Template(o => (o.Content.RtfReportHeader != null)&&(o.Content.RtfReportHeader.Length>0) ?
                                             @Html.ActionLink(Resources.Areas.ReportGenerator.Domain.Entities.ReportTemplate.ReportTemplateModel.OpenRtfHeader, 
                                             "OpenRTFFile", "ReportTemplate", new { id = o.Id, Header = "Header" }, null) : 
                                             new MvcHtmlString("")).Width(150);
                                         c.Template(o => (o.Content.RtfReportFooter != null)&&(o.Content.RtfReportFooter.Length>0) ?
                                             @Html.ActionLink(Resources.Areas.ReportGenerator.Domain.Entities.ReportTemplate.ReportTemplateModel.OpenRtfFooter, 
                                             "OpenRTFFile", "ReportTemplate", new { id = o.Id, Header = "Footer" }, null) : 
                                             new MvcHtmlString("")).Width(150);
                                         c.Command(s =>
                                                       {
                                                           s.Custom("EditCommand").ButtonType(GridButtonType.Image).Ajax(false)
                                                                        .Action("Insert", "ReportTemplate")
                                                                        .HtmlAttributes(new { @class = "t-edit" });
                                                           
                                                           if (ViewData["CanDelete"] != null &&
                                                               (bool)ViewData["CanDelete"])
                                                           {
                                                               s.Delete().ButtonType(GridButtonType.Image);
                                                           }
                                                           
                                                       }).Width("8%");
                                         
                                     })
                       .Pageable(p => p.PageSize(5))
                       .Render();

                %>
            </td>
        </tr>
    </table>
</fieldset>
