<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.ProjectManagment.ValueObjects.PhaseTask>" %>
<fieldset style="background-color: Yellow; height: auto; width: 300px">
    <%:Html.Telerik().ComboBoxFor(model => model.TeamRole.Id)          
                                  .AutoFill(true)
                                  .BindTo(UI.Areas.ProjectManagement.Helpers.DropDownListHelpers.ListOfProjectTeamRoles(0))
                                  .HtmlAttributes(new {style = string.Format("width:{0}px", 300)})
                                  .Filterable(filtering => filtering.FilterMode(AutoCompleteFilterMode.Contains))
                                  .HighlightFirstMatch(true)
                                  .ClientEvents(e => e.OnClose("GetRelevantMembers")) 
                                 
    %>
</fieldset>
