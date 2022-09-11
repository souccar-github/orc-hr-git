<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.PMS.Entities.JobDescription.JobDescriptionSection>" %>
<% using (Ajax.BeginForm("SaveJobDescriptionSection", "LiveAppraisal", new AjaxOptions { OnComplete = "JsonAdd_OnComplete" }))
   {%>
<script type="text/javascript">


    $(document).ready(function () {

        $("#dialog").dialog("destroy");
        $("#dialog-form").dialog({
            autoOpen: false,
            height: 'auto',
            width: 'auto',
            modal: true,
            resizable: false
        });
    });
    function JsonAdd_OnComplete(context) {
        var jsonAdd = context.get_response().get_object();
        if (jsonAdd.Success) {
            $("#JDSectionDiv").html(jsonAdd.PartialViewHtml);
            alert("Submit Success");
        }
    }

    function ShowKPIList(taskId) {
        alert(taskId);
        var d = $('#JobDescriptionSectionTasks' + taskId);
       
        $('#dialog-form').html(d.html());
        $('#dialog-form').dialog('open');
       // alert(d.text());
//        $('#dialog-form').html(result.PartialViewHtml);
//        $('#dialog-form').dialog('open');
//        $.ajax({
//            type: "POST",
//            traditional: true,
//            url: '<%: Url.Action("GetJobDescriptionKPIList", "LiveAppraisal") %>',
//            data: {sectionItemId: sectionItemId, taskId: taskId },
//            success: function (result) {
//                if (result.Success == false) {
//                    alert(result.Message);
//                } else {
//                    $('#dialog-form').html(result.PartialViewHtml);
//                    $('#dialog-form').dialog('open');
//                }
//            }
//        });
    }
</script>
<style type="text/css">
    .defaultTd
    {
        width: auto;
        vertical-align: central;
        border-style: inset;
    }
</style>
    <div id="dialog-form" title="KPI List">
    </div>
<legend><b>JD Section put link to competencies page</b></legend>
<fieldset class="ParentFieldset">
    <div class="layer1" id="JDSectionDiv">
        <table style="width: 100%;">
            <b>Job Description Section</b>
            <%: Html.HiddenFor(model => Model.Id)%>
            <%: Html.HiddenFor(model => Model.Weight)%>
            <%: Html.HiddenFor(model => Model.TotalRate)%>
            <%: Html.HiddenFor(model => Model.Name)%>
            <%: Html.HiddenFor(model => Model.Appraisal.Id)%>
            <tr>
                <% for (var i = 0; i < Model.Items.Count; i++)
                   { %>
                <table style="width: 100%;">
                    <tr>
                         <%: Html.HiddenFor(model => Model.Items[i].Id)%>
                        <td colspan="5">
                        <%: Html.Encode(Model.Name)%>
                        <%: Html.HiddenFor(model => Model.Name)%>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 40%">
                            Role Name
                        </td>
                        <td style="width: 20%">
                            Task
                        </td>
                        <td style="width: 10%">
                            Weight
                        </td>
                        <td style="width: 10%">
                            Rate
                        </td>
                        <td style="width: 20%">
                            Comment
                        </td>
                    </tr>

                    <tr>                       
                        <td class="tdDefault">
                            <div class="display-field">
                                <%: Html.DisplayFor(model => Model.Items[i].RoleName)%>
                                <%: Html.HiddenFor(model => Model.Items[i].RoleName)%>
                            </div>
                        </td>
                        <td class="tdDefault">
                            <div class="display-field">
                                <%: Html.DisplayFor(model => Model.Items[i].JobTask)%>
                                <%: Html.HiddenFor(model => Model.Items[i].JobTask)%>
                            </div>
                        </td>
                        <td class="tdDefault">
                            <%: Html.Telerik().PercentTextBox().Name("Items[" + i + "].Weight").Value(Model.Items[i].Weight)%>
                        </td>
                        <td class="tdDefault">
                            <div class="editor-field">
                                <%: Html.Telerik().NumericTextBoxFor(model => Model.Items[i].Rate).MinValue(0).MaxValue(5).IncrementStep(0.5f)%>
                            </div>
                        </td>
                        <td class="tdDefault">
                            <div class="editor-field">
                                <%: Html.TextAreaFor(model => Model.Items[i].Comment)%>
                            </div>
                        </td>
                        <td>
                        <% if (Model.Items[i].Kpis.Count > 0)
                                {%>
                            <div id='<%= "JobDescriptionSectionTasks"+Model.Items[i].Id%>' style="display: none;" >
                            <%
                           Html.RenderPartial("JobDescriptionSectionItemKPI",
                                              Model.Items[i].
                                                  Kpis);%>
                            </div>
                            <input type="button" value="Show KPI" onclick="ShowKPIList(<%= Model.Items[i].Id%>)" />
                            <%    }
                            %>
                        </td>
                    </tr>
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