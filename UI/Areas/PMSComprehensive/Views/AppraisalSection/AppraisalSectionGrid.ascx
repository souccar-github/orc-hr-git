<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="UI.Areas.PMSComprehensive.DTO.ViewModels" %>
<% Html.Telerik().Grid<AppraisalSectionViewModel>("appraisalSections")
                   .Name("Grid")
                  .DataBinding(dataBinding =>
                                   {
                                       dataBinding.Ajax().Select("AjaxIndex", "AppraisalSection");
                                       dataBinding.Ajax().Delete("Delete", "AppraisalSection");
                                   })
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
                                    c.Bound(o => o.Id).Title("No").Width(1).Groupable(false).Filterable(false).
                                        Sortable(false);
                                    c.Bound(o => o.Name);
                                    c.Command(s =>
                                                  {
                                                      s.Custom("editSection")
                                                          .Text("Edit")
                                                          .Ajax(true)
                                                          .Action("Edit", "AppraisalSection");
                                                      if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                      {
                                                          s.Delete().ButtonType(GridButtonType.Image);
                                                      }
                                                  });
                                })
                   .ClientEvents(builder =>
                                     {
                                         builder.OnComplete("onAppraisalSectionComplete");
                                         builder.OnRowSelect("loadAppraisalSectionItems");
                                         builder.OnError("Grid_onError");
                                     })

                   .Pageable()
                   .Selectable()
                   .Render();
%>

<script type="text/javascript">
    function showCreateDialog() {
        $.ajax({
            url: '<%:Url.Action("Save", "AppraisalSection")%>/',
            type: "POST",
            success: function (result) {
                $("#dialog-form").html(result.PartialViewHtml);
                open($("#dialog-form"), "Add Section");
            }
        });
    }

    function loadAppraisalSectionItems(e) {
        var id = e.row.cells[0].innerHTML;
        var url = '<%: Url.Action("Index", "AppraisalSectionItem", new {sectionId = "Value1"}) %>';
        url = url.replace("Value1", id);
        $.ajax({
            url: url,
            type: "POST",
            success: function (result) {
                $("#AppraisalSectionItemGridDiv").html(result.PartialViewHtml);
                $('#AppraisalSectionItemGridDiv').fadeIn('fast');
            }
        });
        $('#AppraisalSectionItemKpiGridDiv').html('');
    }

    function loadRibbon() {
        $('#PMSComprehensiveFunctionsArea').load('<%: Url.Action("GetFunctionsPartial", "PMSComprehensive") %>');
    }
</script>
