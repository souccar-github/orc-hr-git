<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="Resources.Shared.Buttons" %>
<%@ Import Namespace="UI.Areas.PMSComprehensive.DTO.ViewModels" %>

<script type="text/javascript">
    function AddSectionItem() {
        $.ajax({
            url: '<%:Url.Action("Create", "AppraisalSectionItem")%>/',
            type: "POST",
            success: function (result) {
                $("#dialog-form").html(result.PartialViewHtml);
                open($("#dialog-form"), "Add Section Item");
            }
        });
    }

    function JsonSaveItem_OnComplete(context) {
        var jsonEdit = context.get_response().get_object();
        if (jsonEdit.Success) {
            $("#AppraisalSectionItemsGrid").data("tGrid").rebind();
            CloseDialog($('#dialog-form'));
        } else {
            $("#dialog-form").html(jsonEdit.PartialViewHtml);
        }
    }


    function loadAppraisalSectionItemKpis(e) {
        var id = e.row.cells[0].innerHTML;
        var url = '<%: Url.Action("Index", "AppraisalSectionItemKpi", new {sectionItemId = "Value1"}) %>';
        url = url.replace("Value1", id);
        $.ajax({
            url: url,
            type: "POST",
            success: function (result) {
                $("#AppraisalSectionItemKpiGridDiv").html(result.PartialViewHtml);
                $('#AppraisalSectionItemKpiGridDiv').fadeIn('fast');
            }
        });
    }

    function onAppraisalSectionItemComplete(e) {
        if (e.name == "editSectionItem") {
            $("#dialog-form").html(e.response.PartialViewHtml);
            open($("#dialog-form"), "Edit Section Item");
        }
    }

</script>

<% Html.Telerik().Grid<AppraisalSectionItemViewModel>("AppraisalSectionItems")
       .Name("AppraisalSectionItemsGrid")
       .DataKeys(k => k.Add(o => o.Id))
       .DataBinding(dataBinding => dataBinding.Ajax().Delete("Delete", "AppraisalSectionItem")
                                                     .Select("AjaxIndex", "AppraisalSectionItem"))
       .ToolBar(toolBar => toolBar.Template(() =>
                                                {%>
<table class="GridToolBar">
    <tr>
        <td>
            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
               {%>
            <input type="button" id="AddSectionButton" value="<%: Function.Add %>" onclick=" AddSectionItem() " />
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
                        columns.Bound(o => o.Name);
                        columns.Bound(o => o.Weight);
                        columns.Bound(o => o.Description);
                        columns.Command(s =>
                                            {
                                                s.Custom("editSectionItem")
                                                    .Text("Edit")
                                                    .Ajax(true)
                                                    .Action("Edit", "AppraisalSectionItem");
                                                if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                {
                                                    s.Delete().ButtonType(GridButtonType.Image);
                                                }
                                            });
                    }
       )
       .ClientEvents(builder =>
                         {
                             builder.OnRowSelect("loadAppraisalSectionItemKpis");
                             builder.OnComplete("onAppraisalSectionItemComplete");
                         })
       .Pageable()
       .Selectable()
       .Render();

%>
