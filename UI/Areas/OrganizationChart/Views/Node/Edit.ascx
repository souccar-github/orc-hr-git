<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.OrgChart.ValueObjects.Node>" %>
<%@ Import Namespace="UI.Areas.OrganizationChart.Helpers" %>
<% using (Ajax.BeginForm("Save", "Node", new AjaxOptions { OnComplete = "JsonEdit_OnComplete" }))
   {%>
<script type="text/javascript">
    function JsonEdit_OnComplete(context) {

        var JsonEdit = context.get_response().get_object();

        if (JsonEdit.Success) {
            $("#Nodes").attr('style', 'Display:none');
            $("#center-container").html(JsonEdit.PartialViewHtml);
        }
        else {
            $("#Nodes").html(JsonEdit.PartialViewHtml);
        }
    }
    function Hide() {
        $("#Nodes").attr('style', 'Display:none');
    }
</script>
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
                    <% if (ViewData["CanEditType"] != null && ViewData["CanEditType"].ToString().ToLower() == "false")
                       { %>
                    <%: Html.Label(Resources.Areas.OrgChart.ValueObjects.Node.NodeRules.CanEditTypeMessage) %>
                    <%  } %>
                    <% else
                       { %>
                    <% if (Model.Type != null)
                       { %>
                    <%: Html.Telerik().DropDownListFor(model => model.Type.Id)
                                      .BindTo(DropDownListHelpers.ListOfNodeTypesOrders(Model.Type.Id - 1))
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                    %>
                    <% } %>
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