<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<UI.Areas.Services.DTO.ViewModels.DelegationViewModel>" %>
<% using (Ajax.BeginForm("JsonEdit", "Delegation", new AjaxOptions { OnComplete = "JsonEdit_OnComplete" }))
   {%>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend">
        <%:String.Format(Resources.Areas.Services.Delegation.Messages.DelegationTitleWithNO, Html.DisplayFor(model => model.Delegation.Id))%>
    </legend>
    <script type="text/javascript">
        function JsonEdit_OnComplete(context) {
            var jsonEdit = context.get_response().get_object();
            if (jsonEdit.Success) {
                var url = '<%:Url.Action("Index", "Delegation")%>';
                window.location.replace(url);

            } else {
                $("#result").html(jsonEdit.PartialViewHtml);
            }
        }
    </script>
    <table style="margin-left: 1px">
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Delegation.Reason) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Delegation.From)%>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Delegation.To)%>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td style="width: 10%; vertical-align: top">
                <input type="submit" value="<%:Resources.Shared.Buttons.Function.Save %>" onclick="DisableSaveButton()" />
                <%: Html.HiddenFor(model => model.Delegation.Id)%>
                <%: Html.HiddenFor(model => model.Delegation.Position.Id)%>
            </td>
        </tr>
        <tr>
            <td style="width: 50%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Delegation.Reason)%>
                </div>
                <div class="editor-field">
                    <%:Html.TextAreaFor(model => model.Delegation.Reason)%>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.Delegation.Comment)%>
                </div>
                <div class="editor-field">
                    <%:Html.TextAreaFor(model => model.Delegation.Comment)%>
                </div>
            </td>
            <td style="width: 50%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Delegation.From)%>
                </div>
                <div class="editor-field">
                    <%:Html.Telerik().DatePickerFor(model => model.Delegation.From).Value(Model.Delegation.From).Min(DateTime.MinValue)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Delegation.To)%>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DatePickerFor(model => model.Delegation.To).Value(Model.Delegation.To).Min(DateTime.MinValue)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.CheckBoxFor(model => model.Delegation.Appraisable)%>
                    <%: Html.LabelFor(model => model.Delegation.Appraisable)%>
                </div>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td>
                <input type="button" value="<%:Resources.Shared.Buttons.Function.Cancel %>" onclick="cancel()"
                    class="CancelButton" />
                <script type="text/javascript">
                    function cancel() {
                        $('#result').fadeOut('slow', function () {
                            $('#result').empty();
                        });
                    }
                </script>
            </td>
            <td style="width: 10%; vertical-align: top">
                <input type="submit" value="<%:Resources.Shared.Buttons.Function.Save %>" onclick="DisableSaveButton()" />
            </td>
        </tr>
    </table>
</fieldset>
<% } %>