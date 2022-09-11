<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.Collections.Generic.IEnumerable<HRIS.Domain.PMS.Entities.JobDescription.JobDescriptionSectionItemKpi>>" %>

<fieldset class="ParentFieldset">
    <table style="width: 100%;">
        <tr>
            <td class="defaultTD">
                <div class="display-field">
                    Value
                </div>
            </td>
            <td class="defaultTD">
                <div class="display-field">
                    Description
                </div>
            </td>
        </tr>
        <% foreach (var item in Model)
           { %>
        <tr>
  
            <td class="defaultTD">
                <div class="editor-field">
                    <%: Html.DisplayFor(model => item.Value)%>
                </div>
            </td>
            <td class="defaultTD">
                <div class="editor-field">
                    <%: Html.DisplayFor(model => item.Description)%>
                </div>
            </td>            
        </tr>
        <% } %>
    </table>
</fieldset>
<style type="text/css">
    .defaultTD
    {
        border-style: groove;
        border-width: 1px;
        width: 50%;
    }
</style>
