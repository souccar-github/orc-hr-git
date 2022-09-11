<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Personnel.ValueObjects.Contact>" %>
<%@ Import Namespace="UI.Areas.Personnel.Helpers" %>
<table>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.FirstContact)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.SecondContact)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Fax)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.POBox)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.PrimaryEMail)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.SecondaryEMail)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Address)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.WebSite)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Twitter)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Facebook)%>
        </td>
    </tr>
</table>
<fieldset class="ParentFieldset">
    <table width="100%">
        <tr>
            <td style="width: 33%; vertical-align: top">
                <%: Html.HiddenFor(model => model.Id) %>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.FirstContact) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.FirstContact)%>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.SecondContact) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.SecondContact)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Fax) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.Fax)%>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.POBox) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.POBox)%>
                </div>
            </td>
            <td style="width: 33%; vertical-align: top">
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.PrimaryEMail) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.PrimaryEMail)%>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.SecondaryEMail) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.SecondaryEMail)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Address) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextAreaFor(model => model.Address)%>
                </div>
            </td>
            <td style="width: 33%; vertical-align: top">
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.WebSite) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.WebSite)%>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.Twitter) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.Twitter)%>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.Facebook) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.Facebook)%>
                </div>
            </td>
        </tr>
    </table>
</fieldset>
<table>
    <tr>
        <td>
            <input type="submit" value="<%:Resources.Shared.Buttons.Function.Save %>" onclick=" DisableSaveButton(); " />
        </td>
    </tr>
</table>
