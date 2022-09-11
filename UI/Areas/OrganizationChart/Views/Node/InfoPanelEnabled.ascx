<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% Html.Telerik().Window()
.Name("Window")
.Title("Node Operations")
.Draggable(true)
.Content(() =>
{%>
    <%: Html.Telerik().PanelBar()
                    .Name("PanelBar")
                    .HtmlAttributes(new { style = "width: 150px;" })
                    .ClientEvents(events => events.OnSelect("PanelBarOnSelecta"))                                 
                    .Items(panelbar => panelbar.Add().Text("Selected Node")
                    .Items(item =>
                            {
                                item.Add().Text("Add SubNode");
                                item.Add().Text("Edit Node");
                                item.Add().Text("Delete Node");
                                item.Add().Text("Go To Positions");

                            })
                    .Expanded(true))%>
    <%})
.Width(165)
.Height(120)
.Render();
%>