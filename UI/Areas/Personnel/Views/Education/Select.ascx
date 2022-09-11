<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Personnel.ValueObjects.Education>" %>
<%@ Import Namespace="HRIS.Domain.Personnel.ValueObjects" %>
<table width="100%">
    <%
        foreach (var item in (IEnumerable<Education>)ViewData["ValueObjectsList"])
        {%>
    <tr style="border-width: 11; border-color: Black; border-spacing: 11">
        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%:Html.LabelFor(model => model.Type)%>
            </div>
            <div class="display-field">
                <%:Html.DisplayFor(model => item.Type.Name)%>
            </div>
            <div class="display-label">
                <%:Html.LabelFor(model => model.Major)%>
            </div>
            <div class="display-field">
                <%:Html.DisplayFor(model => item.Major.Name)%>
            </div>
        </td>
        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%:Html.LabelFor(model => model.ScoreType)%>
            </div>
            <div class="display-field">
                <%:Html.DisplayFor(model => item.ScoreType.Name)%>
            </div>
            <div class="display-label">
                <%:Html.LabelFor(model => model.Score)%>
            </div>
            <div class="display-field">
                <%:Html.DisplayFor(model => item.Score)%>
            </div>
            <div class="display-label">
                <%:Html.LabelFor(model => model.Rank)%>
            </div>
            <div class="display-field">
                <%:Html.DisplayFor(model => item.Rank.Name)%>
            </div>
        </td>
        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%:Html.LabelFor(model => model.University)%>
            </div>
            <div class="display-field">
                <%:Html.TextBoxFor(model => item.University,
                                              new {@readonly = true, @class = "SingleLine"})%>
            </div>
            <div class="display-label">
                <%:Html.LabelFor(model => model.DateOfIssuance)%>
            </div>
            <div class="display-field">
                <%:Html.Encode(String.Format("{0:MM/dd/yyyy}", item.DateOfIssuance))%>
            </div>
            <div class="display-label">
                <%:Html.LabelFor(model => model.Country)%>
            </div>
            <div class="display-field">
                <%:Html.DisplayFor(model => item.Country.Name)%>
            </div>
        </td>
        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%:Html.LabelFor(model => model.Comments)%>
            </div>
            <div class="display-field">
                <%:Html.TextAreaFor(model => item.Comments,
                                               new {@readonly = true, @class = "MultiLine"})%>
            </div>
        </td>
        <td>
            <%
            if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
            {%>
            <%:Ajax.ActionLink(Resources.Shared.Buttons.Function.Edit, "JsonEdit", "Education", new {item.Id},
                                                  new AjaxOptions {OnComplete = "JsonEdit_OnComplete"})%>
            <%
            }%>
        </td>
    </tr>
    <%
        }%>
</table>
<script type="text/javascript">

    function JsonEdit_OnComplete(context) {

        var jsonEdit = context.get_response().get_object();
        if (jsonEdit.Success) {
            $("#addValueObjectArea").html(jsonEdit.PartialViewHtml);
            Toggle("edit");
        }
    }


</script>
