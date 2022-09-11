<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="HRIS.Domain.PMS.RootEntities" %>
<% Html.Telerik().Grid<AppraisalTemplate>("appraisalTemplates")
                   .Name("AppraisalTemplatesGrid")
                  .DataBinding(dataBinding => dataBinding.Ajax().Delete("Delete", "AppraisalTemplate").Select("AjaxIndex", "AppraisalTemplate"))
                   .DataKeys(k => k.Add(o => o.Id))
                   .ToolBar(toolBar => toolBar.Template(() =>
                                                            {%>
<table class="GridToolBar">
    <tr>
        <td>
            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
               {%>
            <input type="button" value="<%: Resources.Shared.Buttons.Function.Add %>" onclick=" showCreateDialog() " />
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
                                    c.Bound(o => o.Id).Title(Resources.Shared.Model.Fields.Id).Width(1).Groupable(false).Filterable(false).
                                        Sortable(false);
                                    c.Bound(o => o.Name);
                                    c.Bound(o => o.Type.Name).Title(
                                        Resources.Areas.PMS.Entities.AppraisalTemplate.AppraisalTemplateModel.Type);
                                    c.Command(s =>
                                                  {
                                                      s.Custom("editTemplate")
                                                          .Text("Edit")
                                                          .Ajax(true)
                                                          .Action("Edit", "AppraisalTemplate");
                                                      if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                      {
                                                          s.Delete().ButtonType(GridButtonType.Image);
                                                      }
                                                  });
                                })
                   .ClientEvents(builder => builder.OnComplete("onAppraisalTemplateComplete"))

                   .Pageable()
                   .Selectable()
                   .Render();
%>

<script type="text/javascript">
    function onAppraisalTemplateComplete(e) {
        if (e.name == "editTemplate") {
            $("#dialog-form").html(e.response.PartialViewHtml);
            open($("#dialog-form"), "Edit Template");
        }
    }

    function showCreateDialog() {
        $.ajax({
            url: '<%:Url.Action("Insert", "AppraisalTemplate")%>/',
            type: "POST",
            success: function (result) {
                $("#dialog-form").html(result.PartialViewHtml);
                open($("#dialog-form"), "Add Template");
            }
        });
    }

    function loadRibbon() {
        $('#PMSComprehensiveFunctionsArea').load('<%: Url.Action("GetFunctionsPartial", "PMSComprehensive") %>');
    }
</script>
