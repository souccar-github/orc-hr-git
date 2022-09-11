<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Souccar.ReportGenerator.Domain.Classification.ReportTemplate>" %>
<%
    using (Html.BeginForm("Save", "ReportTemplate", FormMethod.Post, new { enctype = "multipart/form-data" }))
    {%>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend">
        <%: Resources.Areas.ReportGenerator.Domain.Entities.ReportTemplate.ReportTemplateModel.CreatePageTitle %></legend>
    <table style="margin-left: 1px">
    
    <tr>
            <td>
                <%:Html.ValidationMessage("InternalError")%>
            </td>
        </tr>
        <tr>
            <td>
                <%:Html.ValidationMessageFor(model => model.Name)%>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td colspan="2">
                <input type="submit" value="<%: Resources.Shared.Buttons.Function.Save %>" onclick=" DisableSaveButton(); " />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <%:Html.HiddenFor(model => model.Id)%>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table width="100%">
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
                            <br/>
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
                <a href="<%:Url.Action("Index", "ReportTemplate")%>">
                    <input type="button" value="<%: Resources.Shared.Buttons.Function.Cancel %>" />
                </a>
            </td>
            <td style="width: 10%; vertical-align: top">
                <input type="submit" value="<%: Resources.Shared.Buttons.Function.Save %>" onclick=" DisableSaveButton(); " />
            </td>
        </tr>
    </table>
</fieldset>
<%
    }%>

