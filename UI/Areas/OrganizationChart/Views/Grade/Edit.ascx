<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.OrgChart.Entities.Grade>" %>

<%@ Import Namespace="UI.Areas.OrganizationChart.Helpers" %>

<% using (Ajax.BeginForm("JsonEdit", "Grade", new AjaxOptions { OnComplete = "JsonEdit_OnComplete" }))
   {%>

   <script type="text/javascript">
       function JsonEdit_OnComplete(context) {
           var jsonEdit = context.get_response().get_object();
           if (jsonEdit.Success) {

               $.ajax({
                   url: '<%=Url.Action("SaveTabIndex", "Grade")%>/', type: "POST",
                   data: { selectedIndex: 100 },
                   success: function () {
                       location.reload();
                   }
               });
           }
           else {
               $("#result").html(jsonEdit.PartialViewHtml);
           }
       }
    </script>

<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%: string.Format(Resources.Areas.OrgChart.Entities.Grade.GradeModel.BasicDetailsTitle.ToLower(), Model.Name)%></legend>
    <table>
    <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Level) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Name) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Step) %>
            </td>
        </tr>
        
       
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.MinSalary) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.MidPointSalary) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.MaxSalary) %>
            </td>
        </tr>
    </table>
    <table width="100%" style="vertical-align: top">
        <tr>
            <td style="width: 50%; vertical-align: top">
                <div class="editor-label">
                    <%: Html.HiddenFor(model => model.Id) %>
                </div>
            </td>
            <td style="width: 50%; vertical-align: top" align="right">
                <input type="image" value="Save" src="<%= Url.Content("~/Content/Ribbon/Icons/48/37.png") %>"
                    title=<%: Resources.Shared.Buttons.Function.Save %> alt=<%: Resources.Shared.Buttons.Function.Save %> height="24" width="24" align="middle" />
            </td>
        </tr>
        <tr>
            <td style="width: 100%; vertical-align: top" colspan="2">
                <table width="80%">
                    <tr>
                          <td style="width: 33%; vertical-align: top">
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.Level) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().DropDownListFor(model => model.Level.Id)
                           .BindTo(DropDownListHelpers.ListOfOrganizationalLevel)
                           .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                                %>
                            </div>
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.Name) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().DropDownListFor(model => model.Name.Id)
                                      .BindTo(DropDownListHelpers.ListOfGradeName)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                                %>
                            </div>
                             <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.Step) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().DropDownListFor(model => model.Step.Id)
                                      .BindTo(DropDownListHelpers.ListOfGradeStepName)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                                %>
                            </div>
                           
                        </td>
                        <td style="width: 33%; vertical-align: top;">
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.MinSalary) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().NumericTextBox().Name("MinSalary").MinValue(0).MaxValue(float.MaxValue)%>
                            </div>
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.MidPointSalary) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().NumericTextBox().Name("MidPointSalary").MinValue(0).MaxValue(float.MaxValue)%>
                            </div>
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.MaxSalary) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().NumericTextBox().Name("MaxSalary").MinValue(0).MaxValue(float.MaxValue)%>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <br />
                <img onclick="cancel()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/90.png") %>"
                    title=<%: Resources.Shared.Buttons.Function.Cancel %> alt=<%: Resources.Shared.Buttons.Function.Cancel %> height="24" width="24" align="middle" />
                <script type="text/javascript">
                    function cancel() {
                        $('#result').fadeOut();
                    }
                </script>
            </td>
            <td style="width: 50%; vertical-align: top" >
                <input type="image" value="Save" src="<%= Url.Content("~/Content/Ribbon/Icons/48/37.png") %>"
                    title=<%: Resources.Shared.Buttons.Function.Save %> alt=<%: Resources.Shared.Buttons.Function.Save %> height="24" width="24" />
            </td>
        </tr>
    </table>
</fieldset>

<% } %>