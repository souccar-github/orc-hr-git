<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.ProjectManagment.ValueObjects.ProjectEvaluation>" %>
<table width="100%">
    <% foreach (var item in (IEnumerable<HRIS.Domain.ProjectManagment.ValueObjects.ProjectEvaluation>)ViewData["ValueObjectsList"])
       { %>
    <tr>
        <td style="width: 33.3%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Evaluator)%>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Evaluator.FirstName)%>
            </div>            
            <div class="display-label">
                <%: Html.LabelFor(model => model.EvaluatorProjectRole)%>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.EvaluatorProjectRole)%>
            </div> 
        </td>
        <td style="width: 33.3%; vertical-align: top">
            <div class="display-label">
                <%:Html.LabelFor(model => model.Date)%>
            </div>
            <div class="display-field">
                <%:Html.Encode(String.Format("{0:MM/dd/yyyy}", item.Date))%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Quarter)%>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Quarter)%>
            </div>
        </td>
        <td style="width: 33.3%; vertical-align: top">
        <div class="display-label">
                <%:Html.LabelFor(model => model.CompletionPercentage)%>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.CompletionPercentage)%>
            </div>
            <div class="display-label">
                <%:Html.LabelFor(model => model.TotalProjectRate)%>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.TotalProjectRate)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Status) %>
            </div>
            <div class="display-field" id="original">
                <%: Html.DisplayFor(model => item.Status.Name)%>
            </div>
        </td>
        <td align="right">
            <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
               {%>
            <input type="button" value="<%: Resources.Shared.Buttons.Function.Edit %>" onclick="ShowEditUserControl()" class="EditButton" />
            <script type="text/javascript">
                function ShowEditUserControl() {
                    $('#addValueObjectArea').load('<%:Url.Action("JsonEdit", "ProjectEvaluation", new { })%>');
                }
            </script>
            <% } %>
        </td>
    </tr>
    <% } %>
</table>
