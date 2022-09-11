<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div class="editor-label-required">
    <%: Html.Label(Resources.Areas.Services.AssignEmployeeToPosition.AssignEmployeeToPositionModel.Weight )%>
</div>
<div class="editor-field">
    <%= Html.Telerik().PercentTextBox()
                     .Name("Weight")
                     .MinValue(0.25)
                     .MaxValue(100)
                     .IncrementStep(0.25)
                     .HtmlAttributes(new { style = string.Format("width:{0}px", 210) })
                     .ClientEvents(events => events.OnChange("weightValueChanged"))
    %>
</div>
