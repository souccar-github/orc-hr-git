<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.OrgChart.Entities.Organization>" %>
<%@ Import Namespace="UI.Areas.Personnel.Helpers" %>
<script type="text/javascript">
    $(":text,.t-input").keypress(function (e) {
        if (e.which == 13) {
            e.preventDefault();
        }
    });     
</script>
<% using (Ajax.BeginForm("Save", "Organization", new AjaxOptions() { OnComplete = "JsonAdd_OnComplete" }))
   {%>
<script type="text/javascript">

    function JsonAdd_OnComplete(context) {
        var JsonAdd = context.get_response().get_object();
        if (JsonAdd.Success) {
            $("#result").html(JsonAdd.PartialViewHtml);
        }

    }   
</script>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%:Resources.Areas.OrgChart.Entities.Organization.OrganizationModel.OrganizationDetailsTitles %></legend>
    <table border="0" cellpadding="0" cellspacing="0" style="margin-left: 1px; vertical-align: top">
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Name) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.MotherCompanyName) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Location) %>
            </td>
        </tr>
    </table>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td colspan="2" align="right">
                <input id="AddBtn" type="submit" value=<%:Resources.Shared.Buttons.Function.Save %> />
                <br />
            </td>
            <%: Html.HiddenFor(model => model.Id) %>
        </tr>
        <tr>
            <td colspan="2" style="width: 100%; vertical-align: top">
                <fieldset class="ParentFieldset">
                    <div class="editor-label-required">
                        <%: Html.LabelFor(model => model.Name) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.TextBoxFor(model => model.Name) %>
                    </div>
                    <div class="editor-label-required">
                        <%: Html.LabelFor(model => model.MotherCompanyName) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.TextAreaFor(model => model.MotherCompanyName) %>
                    </div>
                    <div class="editor-label-required">
                        <%: Html.LabelFor(model => model.Location) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.Telerik().DropDownListFor(model => model.Location.Id)

                                      .BindTo(PersonnelDropDownListHelpers.ListOfCountries)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                        %>
                    </div>
                </fieldset>
            </td>
        </tr>
        <tr>
            <td align="left">
                <br />
                <input type="button" value=<%:Resources.Shared.Buttons.Function.Cancel %> onclick="ShowUserControl()" />
                <script type="text/javascript">
                    function ShowUserControl() {
                        $('#result').load('<%: Url.Action("Load", "Organization") %>');
                    }
                </script>
            </td>
            <td align="right">
                <br />
                <input id="AddBtn2" type="submit" value=<%:Resources.Shared.Buttons.Function.Save %> />
            </td>
        </tr>
    </table>
</fieldset>
<% } %>
