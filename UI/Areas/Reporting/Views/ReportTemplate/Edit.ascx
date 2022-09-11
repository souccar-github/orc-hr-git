<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ReportGenerator.Domain.Classification.ReportTemplate>" %>
<%@ Import Namespace="UI.Areas.PMSComprehensive.Helpers" %>

<% using (Ajax.BeginForm("JsonEdit", "ReportTemplate", new AjaxOptions { OnComplete = "JsonEdit_OnComplete" }))
   {%>
<fieldset class="ParentFieldset">
    <%--<legend class="ParentLegend"><%: string.Format(Resources.Areas.PMS.Entities.AppraisalPhase.AppraisalPhaseModel.BasicInfoTitle.ToLower(), Model.Id)%></legend>--%>
    <script type="text/javascript">

        function JsonEdit_OnComplete(context) {
            var jsonEdit = context.get_response().get_object();
            if (jsonEdit.Success) {
                $.ajax({
                    url: '<%: Url.Action("SaveTabIndex", "ReportTemplate")%>/', type: "POST",
                    data: { selectedIndex: 100 },
                    success: function () {
                        location.reload();
                    }
                });
            } else {
                $("#result").html(jsonEdit.PartialViewHtml);
            }
        }
    </script>
    <table style="margin-left: 1px">
        <tr>
            <td>
                <%:Html.ValidationMessageFor(model => model.Name)%>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td style="width: 10%; vertical-align: top" colspan="2">
                <input type="submit" value="<%: Resources.Shared.Buttons.Function.Save %>" onclick="DisableSaveButton()" />
            </td>
             <td>
                <%:Html.HiddenFor(model => model.Id)%>         
            </td>
        </tr>
        <tr>
            <td style="width: 100%; vertical-align: top" colspan="2">
                <table width="80%">
                    <tr>
                        <td style="width: 25%; vertical-align: top;">
                            <div class="editor-label">
                                <%: Html.LabelFor(model => model.Name) %>
                            </div>
                            <div class="editor-field">
                                <%:Html.EditorFor(model => model.Name)%>
                            </div>
                        </td>
                        <td style="width: 25%; vertical-align: top;">
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.Content.ShowDateTime)%>
                            </div>
                            <div class="editor-field">
                                <%: Html.CheckBoxFor(model => model.Content.ShowDateTime)%>
                            </div>
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.Content.ShowUserName)%>
                            </div>
                            <div class="editor-field">
                                <%: Html.CheckBoxFor(model => model.Content.ShowUserName)%>
                            </div>
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.Content.ShowPageNumber)%>
                            </div>
                            <div class="editor-field">
                                <%: Html.CheckBoxFor(model => model.Content.ShowPageNumber)%>
                            </div>
                        </td>
                        <td style="width: 25%; vertical-align: top;">
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.Content.ShowHeader)%>
                            </div>
                            <div class="editor-field">
                                <%: Html.CheckBoxFor(model => model.Content.ShowHeader)%>
                            </div>
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.Content.ShowFooter)%>
                            </div>
                            <div class="editor-field">
                                <%: Html.CheckBoxFor(model => model.Content.ShowFooter)%>
                            </div>
                        </td>
                        <td style="width: 25%; vertical-align: top;">
                            <p>
                                <input type="file" id="RtfReportHeader" name="RtfReportHeader" size="23" />
                            </p>
                            <p>
                                <input type="file" id="RtfReportFooter" name="RtfReportFooter" size="23" />
                            </p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <input type="button" value="<%: Resources.Shared.Buttons.Function.Cancel %>" onclick="cancel()" class="CancelButton" />
                <script type="text/javascript">
                    function cancel() {
                        $('#result').load('<%: Url.Action("PartialMasterInfo", "ReportTemplate") %>');
                    }
                </script>
            </td>
            <td style="width: 10%; vertical-align: top" align="right">
                <input type="submit" value="<%: Resources.Shared.Buttons.Function.Save %>" onclick="DisableSaveButton()" />
            </td>
        </tr>
    </table>
</fieldset>
<% } %>

<script type="text/javascript">

    $(document).ready(function () {

        $('#upload').bind("click", function (e) {
            var imgVal = $('#uploadImage').val();
            if (imgVal == '') {
                alert("<%: Resources.Shared.Messages.General.SelectRtfFileMessage %>");
                e.preventDefault();
            }
            else {
                var extension = imgVal.substr(imgVal.lastIndexOf('.') + 1, 3);
                if (extension != "rtf") {
                    alert("<%: Resources.Shared.Messages.General.RTFTypeMessage %>");
                    e.preventDefault();
                }
            }
        });
    });
    
    
</script>