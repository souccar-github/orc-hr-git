<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="HRIS.Domain.PMS.Entities" %>
<%@ Import Namespace="HRIS.Domain.PMS.Entities.Template" %>
<%@ Import Namespace="Resources.Shared.Buttons" %>
<%@ Import Namespace="UI.Areas.PMSComprehensive.DTO.ViewModels" %>

<script type="text/javascript">
    function AddSectionItemKpi() {
        $.ajax({
            url: '<%:Url.Action("Create", "AppraisalSectionItemKpi")%>/',
            type: "POST",
            success: function (result) {
                $("#dialog-form").html(result.PartialViewHtml);
                open($("#dialog-form"), "Add Section Item Kpi");
            }
        });
    }

    function onAppraisalSectionItemKpiComplete(e) {
        if (e.name == "editSectionItemKpi") {
            $("#dialog-form").html(e.response.PartialViewHtml);
            open($("#dialog-form"), "Edit Section Item Kpi");
        }
    }

    function JsonSaveItemKpi_OnComplete(context) {
        var jsonEdit = context.get_response().get_object();
        if (jsonEdit.Success) {
            $("#AppraisalSectionItemkpisGrid").data("tGrid").rebind();
            CloseDialog($('#dialog-form'));
        } else {
            $("#dialog-form").html(jsonEdit.PartialViewHtml);
        }
    }

</script>

<% Html.Telerik().Grid<AppraisalSectionItemKpiViewModel>("appraisalSectionItemKpis")
       .Name("AppraisalSectionItemkpisGrid")
       .DataKeys(k => k.Add(o => o.Id))
       .DataBinding(dataBinding => dataBinding.Ajax().Delete("Delete", "AppraisalSectionItemKpi").Select("AjaxIndex", "AppraisalSectionItemKpi"))
       .ToolBar(toolBar => toolBar.Template(() =>
                                                {%>
<table class="GridToolBar">
    <tr>
        <td>
            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
               {%>
            <input type="button" id="AddSectionItemKpiButton" value="<%: Function.Add %>" onclick=" AddSectionItemKpi() " />
            <% } %>
        </td>
    </tr>
</table>
<%
                                                }))
       .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
       .Columns(columns =>
                    {
                        columns.Bound(o => o.Id);
                        columns.Bound(o => o.Value);
                        columns.Bound(o => o.Description);
                        columns.Command(s =>
                                            {
                                                s.Custom("editSectionItemKpi")
                                                    .Text("Edit")
                                                    .Ajax(true)
                                                    .Action("Edit", "AppraisalSectionItemKpi");
                                                if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                {
                                                    s.Delete().ButtonType(GridButtonType.Image);
                                                }
                                            });
                        columns.Command(s =>
                                            {

                                            }).Width(1);
                    }
       )
       .ClientEvents(builder => builder.OnComplete("onAppraisalSectionItemKpiComplete"))
       .Pageable()
       .Selectable()
       .Render();
%>
