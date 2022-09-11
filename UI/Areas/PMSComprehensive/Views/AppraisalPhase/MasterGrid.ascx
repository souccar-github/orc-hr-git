<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="HRIS.Domain.PMS.RootEntities" %>
<%@ Import Namespace="UI.Areas.PMSComprehensive.DTO.ViewModels" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%: Resources.Areas.PMS.Entities.AppraisalPhase.AppraisalPhaseModel.AppraisalPhaseMasterGridTitle %></legend>
    <table width="100%">
        <tr>
            <td>
                <%
                    Html.Telerik().Grid<AppraisalPhaseGridViewModel>("appraisalPhases")
                           .Name("Grid")
                           .DataKeys(k => k.Add(o => o.Id))
                           .DataBinding(dataBinding => dataBinding.Ajax().Delete("Delete", "AppraisalPhase").Select("AjaxIndex", "AppraisalPhase"))
                           .Scrollable(builder => builder.Height(350))
                           .ToolBar(toolBar => toolBar.Template(() =>
                           { 
                %>
                <table class="GridToolBar">
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="button" value="<%: Resources.Shared.Buttons.Function.Add%>" onclick=" showCreateDialog() " />
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
                                         c.Bound(o => o.Id).Width(35).Title(Resources.Shared.Model.Fields.Id).Groupable(false).Filterable(false).Sortable(false);
                                         c.Bound(o => o.OpenDate).Title(Resources.Areas.PMS.Entities.AppraisalPhase.AppraisalPhaseModel.OpenDate).Format("{0:dd/MM/yyyy}");
                                         c.Bound(o => o.CloseDate).Title(Resources.Areas.PMS.Entities.AppraisalPhase.AppraisalPhaseModel.CloseDate).Format("{0:dd/MM/yyyy}");
                                         c.Bound(o => o.Year).Title(Resources.Areas.PMS.Entities.AppraisalPhase.AppraisalPhaseModel.Year);
                                         c.Bound(o => o.Period).Title(Resources.Areas.PMS.Entities.AppraisalPhase.AppraisalPhaseModel.Period);
                                         c.Bound(o => o.StartQuarter).Title(Resources.Areas.PMS.Entities.AppraisalPhase.AppraisalPhaseModel.StartQuarter);
                                         c.Command(s =>
                                                       {
                                                           s.Custom("editPhase")
                                                               .Text("Edit")
                                                               .Ajax(true)
                                                               .Action("Edit", "AppraisalPhase");

                                                           if (ViewData["CanDelete"] != null &&
                                                               (bool)ViewData["CanDelete"])
                                                           {
                                                               s.Delete().ButtonType(GridButtonType.Image);
                                                           }
                                                       }).Width(175);
                                     })
                   .ClientEvents(builder => builder.OnComplete("onAppraisalPhaseComplete"))
                       .Pageable(p => p.PageSize(5))
                       .Selectable()
                       .Render();

                %>
            </td>
        </tr>
    </table>
</fieldset>
<script type="text/javascript">
    function onAppraisalPhaseComplete(e) {
        if (e.name == "editPhase") {
            $("#dialog-form").html(e.response.PartialViewHtml);
            open($("#dialog-form"), "Edit Phase");
        }
    }

    function showCreateDialog() {
        $.ajax({
            url: '<%:Url.Action("Insert", "AppraisalPhase")%>/',
            type: "POST",
            success: function (result) {
                $("#dialog-form").html(result.PartialViewHtml);
                open($("#dialog-form"), "Add Appraisal Phase");
            }
        });
    }

    function loadRibbon() {

        $('#PMSComprehensiveFunctionsArea').load('<%:Url.Action("GetFunctionsPartial", "PMSComprehensive")%>');
    }
</script>
