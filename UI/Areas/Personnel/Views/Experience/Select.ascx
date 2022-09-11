<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Personnel.ValueObjects.Experience>" %>
<%@ Import Namespace="HRIS.Domain.Personnel.ValueObjects" %>
<table width="100%">
    <%
        foreach (var item in (IEnumerable<Experience>)ViewData["ValueObjectsList"])
        {%>
    <tr>
        <td style="width: 25%; vertical-align: top">
            <%:Html.HiddenFor(model => model.Id)%>
            <div class="display-label">
                <%:Html.LabelFor(model => model.JobTitle)%>
            </div>
            <div class="display-field">
                <%:Html.TextBoxFor(model => item.JobTitle, new {@readonly = true, @class = "SingleLine"})%>
            </div>
            <div class="display-label">
                <%:Html.LabelFor(model => model.CompanyName)%>
            </div>
            <div class="display-field">
                <%:Html.TextBoxFor(model => item.CompanyName,
                                              new {@readonly = true, @class = "SingleLine"})%>
            </div>
            <div class="display-label">
                <%:Html.LabelFor(model => model.CompanyLocation)%>
            </div>
            <div class="display-field">
                <%:Html.TextBoxFor(model => item.CompanyLocation,
                                              new {@readonly = true, @class = "InputSelectMode"})%>
            </div>
            <div class="display-label">
                <%:Html.LabelFor(model => model.CompanyWebSite)%>
            </div>
            <div class="display-field">
                <%:Html.TextBoxFor(model => item.CompanyWebSite,
                                              new {@readonly = true, @class = "SingleLine"})%>
            </div>
        </td>
        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%:Html.LabelFor(model => model.Industry)%>
            </div>
            <div class="display-field">
                <%:Html.TextBoxFor(model => item.Industry, new {@readonly = true, @class = "SingleLine"})%>
            </div>
            <div class="display-label">
                <%:Html.LabelFor(model => model.StartDate)%>
            </div>
            <div class="display-field">
                <%:Html.Encode(String.Format("{0:MM/dd/yyyy}", item.StartDate))%>
            </div>
            <div class="display-label">
                <%:Html.LabelFor(model => model.EndDate)%>
            </div>
            <div class="display-field">
                <%:Html.Encode(String.Format("{0:MM/dd/yyyy}", item.EndDate))%>
            </div>
            <div class="display-label">
                <%:Html.LabelFor(model => model.LeaveReason)%>
            </div>
            <div class="display-field">
                <%:Html.TextBoxFor(model => item.LeaveReason,
                                              new {@readonly = true, @class = "SingleLine"})%>
            </div>
        </td>
        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%:Html.LabelFor(model => model.ReferenceFullName)%>
            </div>
            <div class="display-field">
                <%:Html.TextBoxFor(model => item.ReferenceFullName,
                                              new {@readonly = true, @class = "SingleLine"})%>
            </div>
            <div class="display-label">
                <%:Html.LabelFor(model => model.ReferenceJobTitle)%>
            </div>
            <div class="display-field">
                <%:Html.TextBoxFor(model => item.ReferenceJobTitle,
                                              new {@readonly = true, @class = "SingleLine"})%>
            </div>
            <div class="display-label">
                <%:Html.LabelFor(model => model.ReferenceContact)%>
            </div>
            <div class="display-field">
                <%:Html.TextBoxFor(model => item.ReferenceContact,
                                              new {@readonly = true, @class = "SingleLine"})%>
            </div>
            <div class="display-label">
                <%:Html.LabelFor(model => model.ReferenceEMail)%>
            </div>
            <div class="display-field">
                <%:Html.TextBoxFor(model => item.ReferenceEMail,
                                              new {@readonly = true, @class = "SingleLine"})%>
            </div>
        </td>
        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%:Html.LabelFor(model => model.Notes)%>
            </div>
            <div class="display-field">
                <%:Html.TextAreaFor(model => item.Notes,
                                               new {@readonly = true, @class = "MultiLine"})%>
            </div>
        </td>
        <td>
            <%
            if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
            {%>
            <%:Ajax.ActionLink(Resources.Shared.Buttons.Function.Edit, "JsonEdit", "Experience", new {item.Id},
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
