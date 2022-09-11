<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.PMS.Entities.Competency.CompetencySection>" %>
<% using (Ajax.BeginForm("SaveCompetencySection", "LiveAppraisal", new AjaxOptions { OnComplete = "JsonAdd_OnComplete" }))
   {%>
<script type="text/javascript">
    function JsonAdd_OnComplete(context) {
        var jsonAdd = context.get_response().get_object();
        if (jsonAdd.Success) {
            $("#CompetencySectionDiv").html(jsonAdd.PartialViewHtml);
            //  var url = '<%: Url.Action("Index", "LiveAppraisal") %>';
            // window.location.replace(url);
            //  window.locaiton.reaload();
            alert("Submit Success");
        }
    }
</script>
<legend><b>CompetencySection put link to competencies page</b></legend>
<fieldset class="ParentFieldset">
    <div class="layer1" id="CompetencySectionDiv">
        <table style="width: 100%;">
            <b>Job Description</b>
            <%: Html.HiddenFor(model => Model.Id)%>
            <%: Html.HiddenFor(model => Model.Weight)%>
            <%: Html.HiddenFor(model => Model.TotalRate)%>
            <%: Html.HiddenFor(model => Model.Name)%>
            <%: Html.HiddenFor(model => Model.Appraisal.Id)%>
            <tr>
                <% foreach (var type in ((IList<string>)ViewData["CompetencyTypes"]))
                   { %>
                <table>
                    <i>
                        <%:Html.Encode(string.Format("Competencies : {0}",type)) %>
                    </i>
                    <tr>
                        <td style="width: 16.6%; vertical-align: central; border-style: groove">
                            <div class="display-field">
                                Name
                            </div>
                        </td>
                        <td style="width: 16.6%; vertical-align: central; border-style: groove">
                            <div class="display-field">
                                Description
                            </div>
                        </td>
                        <td style="width: 16.6%; vertical-align: central; border-style: groove">
                            <div class="display-field">
                                Type
                            </div>
                        </td>
                        <td style="width: 16.6%; vertical-align: central; border-style: groove">
                            <div class="display-field">
                                Weight
                            </div>
                        </td>
                        <td style="width: 16.6%; vertical-align: central; border-style: groove">
                            <div class="display-field">
                                Rate
                            </div>
                        </td>
                        <td style="width: 16.6%; vertical-align: central; border-style: groove">
                            <div class="display-field">
                                Comments
                            </div>
                        </td>
                    </tr>
                    <% for (var i = 0; i < Model.Items.Count; i++)
                       { %>
                    <% if (Model.Items[i].Type == type)
                       { %>
                    <tr>
                        <%: Html.HiddenFor(model => Model.Items[i].Id)%>
                        <%: Html.HiddenFor(model => Model.Items[i].Level)%>
                        <td style="width: 16.6%; vertical-align: central; border-style: groove">
                            <div class="editor-field">
                                <%: Html.DisplayFor(model => Model.Items[i].Name)%>
                                <%: Html.HiddenFor(model => Model.Items[i].Name)%>
                            </div>
                        </td>
                        <td style="width: 16.6%; vertical-align: central; border-style: groove">
                            <div class="editor-field">
                                <%: Html.DisplayFor(model => Model.Items[i].Description)%>
                                <%: Html.HiddenFor(model => Model.Items[i].Description)%>
                            </div>
                        </td>
                        <td style="width: 16.6%; vertical-align: central; border-style: groove">
                            <div class="editor-field">
                                <%: Html.DisplayFor(model => Model.Items[i].Type)%>
                                <%: Html.HiddenFor(model => Model.Items[i].Type)%>
                            </div>
                        </td>
                        <td style="width: 16.6%; border-style: groove">
                            <%: Html.Telerik().PercentTextBox().Name("Items[" + i + "].Weight").Value(Model.Items[i].Weight)%>
                        </td>
                        <td style="width: 16.6%; border-style: groove">
                            <div class="editor-field">
                                <%: Html.Telerik().NumericTextBoxFor(model => Model.Items[i].Rate).MinValue(0).MaxValue(5).IncrementStep(0.5f)%>
                            </div>
                        </td>
                        <td style="width: 16.6%; vertical-align: central; border-style: groove">
                            <div class="editor-field">
                                <%: Html.TextAreaFor(model => Model.Items[i].Comment)%>
                            </div>
                        </td>
                    </tr>
                    <% } %>
                    <% } %>
                </table>
                <% } %>
            </tr>
            <tr>
                <td>
                    <input type="submit" value="Save" />
                </td>
                <td>
                    Put Final Submit here & Validation Rules
                </td>
            </tr>
        </table>
    </div>
</fieldset>
<% } %>
