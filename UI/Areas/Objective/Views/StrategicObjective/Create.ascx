<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Objectives.RootEntities.StrategicObjective>" %>
<%@ Import Namespace="UI.Areas.Objective.Helpers" %>

<% using (Html.BeginForm("JsonInsert", "StrategicObjective"))
   {%>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend">Strategic Objective</legend>
    <table>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Name) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Description) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Period) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.FromYear) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.ToYear) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Dimension) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.DoesNotMeet) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Meet) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Above) %>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td colspan="2">
                <input type="submit" value="Save" onclick="DisableSaveButton()" />
            </td>
            <%: Html.HiddenFor(model => model.Id) %>
        </tr>
        <tr>
            <td colspan="2">
                <fieldset style="height: auto">
                    <legend>Defenition</legend>
                    <table>
                        <tr>
                            <td style="vertical-align: top;">
                                <div class="editor-label-required">
                                    <%: Html.LabelFor(model => model.Name) %>
                                </div>
                                <div class="editor-field">
                                    <%: Html.TextBoxFor(model => model.Name)%>
                                </div>
                            </td>
                            <td style="vertical-align: top;">
                                <div class="editor-label-required">
                                    <%: Html.LabelFor(model => model.Dimension) %>
                                </div>
                                <div class="editor-field">
                                    <%:Html.Telerik().DropDownListFor(model => model.Dimension.Id)
                                .BindTo(DropDownListHelpers.ListOfDimensions)
                              .HtmlAttributes(new {style = string.Format("width:{0}px", 200)})%>
                                </div>
                            </td>
                            <td style="vertical-align: top;">
                                <div class="editor-label">
                                    <%: Html.LabelFor(model => model.Description) %>
                                </div>
                                <div class="editor-field">
                                    <%:Html.TextAreaFor(model => model.Description)%>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
        </tr>
        <tr>
            <td colspan="2">
                <fieldset style="height: auto">
                    <legend>Planning</legend>
                    <table>
                        <tr>
                            <td style="vertical-align: top;">
                                <div class="editor-label-required">
                                    <%: Html.LabelFor(model=>model.Period) %>
                                </div>
                                <div class="editor-field">
                                    <%:Html.Telerik().DropDownListFor(model => model.Period)
                                .BindTo(DropDownListHelpers.ListOfObjectivePeriods)
                              .HtmlAttributes(new {style = string.Format("width:{0}px", 200)})%>
                                </div>
                            </td>
                            <td style="vertical-align: top;">
                                <div class="editor-label-required">
                                    <%: Html.LabelFor(model=>model.FromYear) %>
                                </div>
                                <div class="editor-field">
                                    <%: Html.Telerik().IntegerTextBoxFor(model => model.FromYear).NumberGroupSize(0)%>
                                </div>
                            </td>
                            <td style="vertical-align: top;">
                                <div class="editor-label-required">
                                    <%: Html.LabelFor(model=>model.ToYear) %>
                                </div>
                                <div class="editor-field">
                                    <%: Html.Telerik().IntegerTextBoxFor(model => model.ToYear).NumberGroupSize(0)%>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <fieldset style="height: auto">
                    <legend>Evaluation Criteria</legend>
                    <table>
                        <tr>
                            <td style="width: 30%; vertical-align: top;">
                                <div class="editor-label-required">
                                    <%: Html.LabelFor(model=>model.DoesNotMeet) %>
                                </div>
                                <div class="editor-field">
                                    <%:Html.TextAreaFor(model => model.DoesNotMeet)%>
                                </div>
                            </td>
                            <td style="width: 30%; vertical-align: top;">
                                <div class="editor-label-required">
                                    <%: Html.LabelFor(model=>model.Meet) %>
                                </div>
                                <div class="editor-field">
                                    <%:Html.TextAreaFor(model => model.Meet)%>
                                </div>
                            </td>
                            <td style="vertical-align: top;">
                                <div class="editor-label-required">
                                    <%: Html.LabelFor(model=>model.Above) %>
                                </div>
                                <div class="editor-field">
                                    <%:Html.TextAreaFor(model => model.Above)%>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ActionLink("Back", "Index", "StrategicObjective")%>
            </td>
            <td style="width: 10%; vertical-align: top">
                <input type="submit" value="Save" onclick="DisableSaveButton()" />
            </td>
        </tr>
    </table>
</fieldset>
<% } %>
