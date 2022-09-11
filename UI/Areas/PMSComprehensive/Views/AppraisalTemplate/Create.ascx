<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<UI.Areas.PMSComprehensive.DTO.ViewModels.AppraisalTemplateViewModel>" %>
<%@ Import Namespace="UI.Areas.PMSComprehensive.Helpers" %>
<%using (
        Ajax.BeginForm(Model != null && !Model.AppraisalTemplate.IsTransient() ? "JsonEdit" : "JsonInsert", "AppraisalTemplate",
                       new AjaxOptions
                       {
                           OnComplete = "JsonSave_OnComplete",
                           HttpMethod = "POST",
                       }))
  {%>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend">
        <%:Resources.Areas.PMS.Entities.AppraisalTemplate.Messages.AppraisalTemplateBasicInformation %></legend>
    <table style="margin-left: 1px">
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.AppraisalTemplate.Name) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.AppraisalTemplate.Grades) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model=>model.AppraisalTemplate.SectionWeights) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.AppraisalTemplate.Type)%>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.AppraisalTemplate.Grades)%>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessage("TotalWeightRule")%>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessage("TotalAlternativeWeightRule")%>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessage(Souccar.Core.DomainErrors.InternalError.ToString())%>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessage(Souccar.Core.DomainErrors.SecurityError.ToString())%>
            </td>
        </tr>
    </table>
    <table width="800">
        <tr>
            <td colspan="2">
                <input type="submit" value="<%:Resources.Shared.Buttons.Function.Save %>" />
            </td>
            <%: Html.HiddenFor(model => model.AppraisalTemplate.Id)%>
        </tr>
        <tr>
            <td colspan="2">
                <table width="80%">
                    <tr>
                        <td style="width: 20%; vertical-align: top;">
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.AppraisalTemplate.Name)%>
                            </div>
                            <div class="display-field">
                                <%: Html.TextBoxFor(model => model.AppraisalTemplate.Name)%>
                            </div>
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.AppraisalTemplate.Type)%>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().DropDownListFor(model => model.AppraisalTemplate.Type.Id)
                                      .BindTo(DropDownListHelpers.ListOfTemplateType)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                                %>
                            </div>
                        </td>
                        <td style="width: 20%; vertical-align: top">
                            <div class="editor-label-required">
                                <%: Html.CheckBox("CheckAllGrades", new { onclick = "AllGradesChecked()" })%>
                                <%:Resources.Areas.PMS.Entities.AppraisalTemplate.AppraisalTemplateModel.GradesTitle %>
                            </div>
                            <div style="height: 200px; overflow: auto">
                                <table style="border: 0">
                                    <thead>
                                        <th></th>
                                        <th>Position Level 
                                        </th>
                                        <th>Grade Name
                                        </th>
                                    </thead>
                                    <% for (int i = 0; i < Model.Grades.Count; i++)
                                       {%>
                                    <tr>
                                        <td>
                                            <%: Html.CheckBoxFor(x => x.Grades[i].Checked)%>
                                        </td>
                                        <td>
                                            <%: Html.DisplayFor(x => x.Grades[i].PositionLevel)%>
                                        </td>
                                        <td>
                                            <%: Html.DisplayFor(x => x.Grades[i].Name)%>
                                        </td>
                                        <%: Html.HiddenFor(x => x.Grades[i].Id)%>
                                        <%: Html.HiddenFor(x => x.Grades[i].PositionLevel)%>
                                        <%: Html.HiddenFor(x => x.Grades[i].Name)%>
                                    </tr>
                                    <%}%>
                                </table>
                            </div>
                            <div style="height: 200px; overflow: auto;">
                                <table style="border: 0">
                                    <thead>
                                        <th>Section Name
                                        </th>
                                        <th>Weight
                                        </th>
                                    </thead>
                                    <% for (int i = 0; i < Model.SectionWeights.Count; i++)
                                       {%>
                                    <tr>
                                        <td>
                                            <%: Html.DisplayFor(x =>x.SectionWeights[i].Name)%>
                                        </td>
                                        <td>
                                            <%: Html.TextBoxFor(x => x.SectionWeights[i].Weight)%>
                                            <%: Html.HiddenFor(x => x.SectionWeights[i].Name)%>

                                        </td>
                                    </tr>
                                    <%}%>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <br />
            </td>
        </tr>
        <tr>
            <td>
                <input type="button" value="<%:Resources.Shared.Buttons.Function.Cancel %>" onclick="CloseDialog($('#dialog-form'))" />
            </td>
            <td style="width: 10%; vertical-align: top">
                <input type="submit" value="<%:Resources.Shared.Buttons.Function.Save %>" />
            </td>
        </tr>
    </table>
</fieldset>
<% } %>
<script type="text/javascript">

    function AllGradesChecked() {
        if ($('#CheckAllGrades').attr('checked') == true) {
            for (var i = 0; i < <%:Model.Grades.Count() %> ; i++) {
                $('#Grades_'+i+'__Checked').attr('checked', true);
            }
            
        }
        else {
            for (var z = 0; z < <%:Model.Grades.Count() %>; z++) {
                $('#Grades_'+z+'__Checked').attr('checked', false);
            }
        }
    }

    function DisableNumericTextBox(par) {
        var numberTextBox = $('#' + par).data("tTextBox");
        numberTextBox.disable();
        numberTextBox.value(0);
    }

    function EnableNumericTextBox(par) {
        var numberTextBox = $('#' + par).data("tTextBox");
        numberTextBox.enable();            
    } 
</script>
