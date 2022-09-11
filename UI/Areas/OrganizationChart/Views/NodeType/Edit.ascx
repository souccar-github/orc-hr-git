<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.OrgChart.Indexes.NodeType>" %>
<% using (Ajax.BeginForm("JsonEdit", "NodeType", new AjaxOptions { OnComplete = "JsonEdit_OnComplete" }))
   {%>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%:Resources.Areas.OrgChart.Indexes.NodeType.NodeTypeModel.NodeTypeTitle %></legend>
    <script type="text/javascript">
        function JsonEdit_OnComplete(context) {

            var JsonEdit = context.get_response().get_object();
            if (JsonEdit.Success) {
                location.reload();
            } else {
                $("#result").html(JsonEdit.PartialViewHtml);
                $("#result").show();
            }
        }
    </script>
    <table border="0" cellpadding="0" cellspacing="0" style="margin-left: 1px">
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.NodeOrder) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Name) %>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td style="width: 10%; vertical-align: top" align="right" colspan="2">
                <input type="image" value=<%:Resources.Shared.Buttons.Function.Save %> src="<%= Url.Content("~/Content/Ribbon/Icons/48/37.png") %>"
                    title=<%:Resources.Shared.Buttons.Function.Save %> alt=<%:Resources.Shared.Buttons.Function.Save %> height="24" width="24" align="middle" />
                <%: Html.HiddenFor(model => model.Id) %>
            </td>
        </tr>
        <td style="width: 100%; vertical-align: top">
            <table border="0" cellpadding="0" cellspacing="0" width="80%">
                <tr>
                    <td style="width: 50%; vertical-align: top">
                        <div class="editor-label-required">
                            <%: Html.LabelFor(model => model.NodeOrder) %>
                        </div>
                        <div class="editor-field">
                            <%: Html.Telerik().NumericTextBox().Name("NodeOrder").MinValue(0).MaxValue(int.MaxValue).DecimalDigits(0)%>
                        </div>
                    </td>
                    <td style="width: 50%; vertical-align: top">
                        <div class="editor-label-required">
                            <%: Html.LabelFor(model => model.Name) %>
                        </div>
                        <div class="editor-field">
                            <%: Html.TextAreaFor(model => model.Name)%>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
        <tr>
            <td align="left">
                <img onclick="cancel()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/90.png") %>"
                    title=<%:Resources.Shared.Buttons.Function.Cancel %> alt=<%:Resources.Shared.Buttons.Function.Cancel %> height="24" width="24" align="middle" />
                <script type="text/javascript">
                    function cancel() {
                        $('#result').fadeIn('slow');
                        $('#result').hide();
                    }
                </script>
            </td>
            <td style="width: 10%; vertical-align: top" align="right">
                <input type="image" value=<%:Resources.Shared.Buttons.Function.Save %> src="<%= Url.Content("~/Content/Ribbon/Icons/48/37.png") %>"
                    title=<%:Resources.Shared.Buttons.Function.Save %> alt=<%:Resources.Shared.Buttons.Function.Save %> height="24" width="24" align="middle" />
            </td>
        </tr>
    </table>
</fieldset>
<% } %>