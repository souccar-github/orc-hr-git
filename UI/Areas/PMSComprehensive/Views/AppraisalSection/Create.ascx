<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.PMS.RootEntities.AppraisalSection>" %>
<%
    using (
        Ajax.BeginForm(Model != null && !Model.IsTransient() ? "JsonEdit" : "JsonSave", "AppraisalSection",
                       new AjaxOptions
                           {
                               OnComplete = "JsonSave_OnComplete",
                               HttpMethod = "POST",
                           }))
    {%>

<%: Html.ValidationSummary(true) %>
<%: Html.ValidationMessageFor(model => model.Name) %>
<%:Html.HiddenFor(model=>model.Id) %>
<div class="editor-label">
    <%: Html.LabelFor(model => model.Name) %>
</div>
<div class="editor-field">
    <%: Html.TextBoxFor(model=>model.Name) %>
</div>

<p>
    <input type="submit" value="<%:Resources.Shared.Buttons.Function.Save %>" />
</p>
<% } %>
