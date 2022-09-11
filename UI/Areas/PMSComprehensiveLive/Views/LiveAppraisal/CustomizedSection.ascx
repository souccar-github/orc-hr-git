<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.PMS.Entities.Organizational.OrganizationalSection>" %>
<% using (Ajax.BeginForm("SaveCutomizedSection", "LiveAppraisal", new AjaxOptions { OnComplete = "JsonAdd_OnComplete" }))
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
            $("#CustomSectionDiv").html(jsonAdd.PartialViewHtml);
            //  var url = '<%: Url.Action("Index", "LiveAppraisal") %>';
            // window.location.replace(url);
            //  window.locaiton.reaload();
            alert("Submit Success");
        }
    }

    function ShowKPIList(sectionId) {
        var d = $('#SectionItems' + sectionId);
        $('#dialog-form').html(d.html());
        $('#dialog-form').dialog('open');
//        $.ajax({
//            type: "POST",
//            traditional: true,
//            url: '<%: Url.Action("GetCustomizedKPIList", "LiveAppraisal") %>',
//            data: { sectionID: sectionId, sectionItemId: sectionItemId },
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
<legend><b>Custom Section </b></legend>
<fieldset class="ParentFieldset">
    <div id="dialog-form" title="KPI List">
    </div>
    <div class="layer1" id="CustomSectionDiv">
        <table style="width: 100%;">
            <%: Html.HiddenFor(model => Model.Id)%>
            <%: Html.HiddenFor(model => Model.Weight)%>
            <%: Html.HiddenFor(model => Model.TotalRate)%>
            <%: Html.HiddenFor(model => Model.Name)%>
            <%: Html.HiddenFor(model => Model.Appraisal.Id)%>
            <tr>
                <% for (var i = 0; i < Model.Items.Count; i++)
                   { %>
                <table>
                    <tr>
                        <td colspan="5">
                            custom section
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 40%">
                            Name
                        </td>
                        <td style="width: 20%">
                            Weight
                        </td>
                        <td style="width: 10%">
                            Rate
                        </td>
                        <td style="width: 10%">
                            Description
                        </td>
                        <td style="width: 20%">
                            Comment
                        </td>
                        <td>
                            KPI List
                        </td>
                    </tr>
                    <tr>
                           <%: Html.HiddenFor(model => Model.Items[i].Id)%>
                        <td style="width: 20%; vertical-align: central; border-style: groove">
                            <div class="editor-field">
                                <%: Html.DisplayFor(model => Model.Items[i].Name)%>
                                <%: Html.HiddenFor(model => Model.Items[i].Name)%>
                            </div>
                        </td>
                        <td style="width: 20%; border-style: groove">
                            <%: Html.Telerik().PercentTextBox().Name("SectionItems[" + i + "].Weight").Value(Model.Items[i].Weight)%>
                        </td>
                        <td style="width: 20%; border-style: groove">
                            <div class="editor-field">
                                <%: Html.Telerik().NumericTextBoxFor(model => Model.Items[i].Rate).MinValue(0).MaxValue(5).IncrementStep(0.5f)%>
                            </div>
                        </td>
                        <td style="width: 20%; vertical-align: central; border-style: groove">
                            <div class="editor-field">
                                <%: Html.TextAreaFor(model => Model.Items[i].Description)%>
                            </div>
                        </td>
                        <td style="width: 20%; vertical-align: central; border-style: groove">
                            <div class="editor-field">
                                <%: Html.TextAreaFor(model => Model.Items[i].Comment)%>
                            </div>
                        </td>
                        <td>
                        <% if (Model.Items[i].Kpis.Count > 0)
                                {%>
                        <div id='<%= "SectionItems"+Model.Items[i].Id%>' style="display: none;" >
                            <% Html.RenderPartial("CustomizedSectionItemKPI", Model.Items[i].Kpis);%>
                            </div>
                            
                       <input type="button" value="Show KPI" onclick="ShowKPIList(<%= Model.Items[i].Id%>)"  />
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