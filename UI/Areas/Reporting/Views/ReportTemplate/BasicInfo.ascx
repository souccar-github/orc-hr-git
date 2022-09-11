<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ReportGenerator.Domain.Classification.ReportTemplate>" %>
<%@ Import Namespace="UI.Helpers.Views" %>

<fieldset class="ParentFieldset">
    <%--<legend class="ParentLegend"><%: string.Format(Resources.Areas.PMS.Entities.AppraisalPhase.AppraisalPhaseModel.BasicInfoTitle.ToLower(), Model.Id)%></legend>--%>
    <table width="100%" style="vertical-align: middle">
        <tr>
            <td>
                <input type="button" value="<%: Resources.Shared.Buttons.Function.Cancel %>" onclick="CancelButton()" class="CancelButton" />
            </td>
            <td style="width: 50%; vertical-align: top">
                <%
                    if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
                    {%>
                <input type="button" value="<%: Resources.Shared.Buttons.Function.Edit %>" onclick="ShowEditUserControl()" class="EditButton" />
                <%
                    }%>
                <script type="text/javascript">
                    function ShowEditUserControl() {
                        $('#result').load('<%:Url.Action("Edit", "ReportTemplate", new {id = Model.Id})%>');
                    }
                </script>
            </td>
        </tr>
        <tr>
            <td style="width: 100%; vertical-align: top">
                <table width="100%">
                    <tr>
                        <td style="width: 25%; vertical-align: top;">
                            <div class="display-label">
                                <%:Html.LabelFor(model => model.Name)%>
                            </div>
                            <div class="display-field">
                                <%: Html.DisplayTextFor(model => model.Name)%>
                            </div>
                        </td>
                        <td style="width: 25%; vertical-align: top;">
                            <div class="display-label">
                                <%: Html.LabelFor(model => model.Content.ShowDateTime)%>
                            </div>
                            <div class="display-field">
                                <%: Html.DisplayFor(model => model.Content.ShowDateTime)%>
                            </div>
                            <div class="display-label">
                                <%: Html.LabelFor(model => model.Content.ShowUserName)%>
                            </div>
                            <div class="display-field">
                                <%: Html.DisplayFor(model => model.Content.ShowUserName)%>
                            </div>
                            <div class="display-label">
                                <%: Html.LabelFor(model => model.Content.ShowPageNumber)%>
                            </div>
                            <div class="display-field">
                                <%: Html.DisplayFor(model => model.Content.ShowPageNumber)%>
                            </div>
                        </td>
                        <td style="width: 25%; vertical-align: top;">
                            <div class="display-label">
                                <%: Html.LabelFor(model => model.Content.ShowHeader)%>
                            </div>
                            <div class="display-field">
                                <%: Html.DisplayFor(model => model.Content.ShowHeader)%>
                            </div>
                            <div class="display-label">
                                <%: Html.LabelFor(model => model.Content.ShowFooter)%>
                            </div>
                            <div class="display-field">
                                <%: Html.DisplayFor(model => model.Content.ShowFooter)%>
                            </div>
                        </td>
                        <td style="width: 25%; vertical-align: top;">
                            
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</fieldset>

