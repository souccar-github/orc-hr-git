<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.PMS.Entities.Appraisal>" %>
<%@ Import Namespace="UI.Helpers.Views" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend">Appraisal No. (<%:Html.DisplayFor(model => model.Id)%>) Details</legend>
    <table width="100%" style="vertical-align: middle">
        <tr>
            <td>
                <input type="button" value="Cancel" onclick="CancelButton()" class="CancelButton" />
            </td>
            <td style="width: 50%; vertical-align: top">
                <%
                    if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
                    {%>
                <input type="button" value="Edit Info" onclick="ShowEditUserControl()" class="EditButton" />
                <%
                    }%>
                <script type="text/javascript">
                    function ShowEditUserControl() {
                        $('#result').load('<%:Url.Action("Edit", "Appraisal", new {id = Model.Id})%>');
                    }
                </script>
            </td>
        </tr>
        <tr>
            <td style="width: 100%; vertical-align: top">
                <table width="100%">
                    <tr>
                        <td style="width: 50%; vertical-align: top;">

                        //basic Information
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</fieldset>
