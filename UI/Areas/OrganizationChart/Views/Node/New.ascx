<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.OrgChart.ValueObjects.Node>" %>
<%@ Import Namespace="UI.Areas.OrganizationChart.Helpers" %>
<script type="text/javascript">
    $(":text,.t-input").keypress(function (e) {
        if (e.which == 13) {
            e.preventDefault();
        }
    });     
</script>
<% using (Ajax.BeginForm("SaveNew", "Node", new AjaxOptions { OnComplete = "JsonAdd_OnComplete" }))
   {%>
<script type="text/javascript">

    $(document).ready(function () {
        var txt = $('#ParentName').attr("value");
        $('#infoTxt').html('<%:Resources.Areas.OrgChart.ValueObjects.Node.NodeModel.NewNodeTitle%>' + txt);

    });

    function JsonAdd_OnComplete(context) {

        var JsonAdd = context.get_response().get_object();

        if (JsonAdd.Success) {
            $("#Nodes").attr('style', 'Display:none');
            $("#center-container").html(JsonAdd.PartialViewHtml);
        }
        else {
            $("#Nodes").html(JsonAdd.PartialViewHtml);
        }
    }
    function Hide() {
        $("#Nodes").attr('style', 'Display:none');
    }
</script>
<div id="infoTxt" class="editor-label-required">
</div>
<fieldset class="ParentFieldset">
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <%: Html.ValidationMessageFor(mode => Model.Name) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(mode => Model.Code) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(mode => Model.Type) %>
            </td>
        </tr>
    </table>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="width: 33%; vertical-align: top">
                <%: Html.HiddenFor(model => model.Id) %>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Code) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.Code)%>
                </div>
            </td>
            <td style="width: 33%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Type) %>
                </div>
                <div class="editor-field">
                    <% if (Model.Type == null)
                       { %>
                    <%: Html.Telerik().DropDownListFor(model => model.Type.Id)
 
                                                  .BindTo(DropDownListHelpers.ListOfNodeTypesOrders(int.Parse(ViewData["SelectedTypeId"].ToString())))
                                                  .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
%>
                    <% } %>
                    <% else
                       { %>
                    <%: Html.Telerik().ComboBoxFor(model => model.Type.Id)
                                                .SelectedIndex(Model.Type.NodeOrder > 0 ? Model.Type.NodeOrder - 1 : Model.Type.NodeOrder)
                                               .AutoFill(true)
                                              .BindTo(DropDownListHelpers.ListOfNodeTypesOrders(int.Parse(ViewData["SelectedTypeId"].ToString())))
                                              .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                                              .Filterable(filtering => filtering.FilterMode(AutoCompleteFilterMode.StartsWith))
                                              .HighlightFirstMatch(true)%>
                    <% } %>
                </div>
            </td>
            <td style="width: 33%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Name) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.Name)%>
                </div>
            </td>
        </tr>
    </table>
</fieldset>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td align="left" style="width: 50%;">
            <input type="button" value=<%:Resources.Shared.Buttons.Function.Cancel %> style="width: 60px;" onclick="Hide()" />
        </td>
        <td align="right" style="width: 50%;">
            <input type="submit" value=<%:Resources.Shared.Buttons.Function.Save %> style="width: 60px;" />
        </td>
    </tr>
</table>
<% } %>