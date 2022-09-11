<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<script type="text/javascript">

    function onSelect(e) {
        var item = $(e.item);

        if (item.find('> .t-link').text() == "Details") {
            $('#result').load('<%: Url.Action("PartialInfo", "Position") %>');
        }
        if (item.find('> .t-link').text() == "Add Role") {
            $('#result').load('<%: Url.Action("Index", "Role") %>');
        }
        if (item.find('> .t-link').text() == "Add Authority") {
            $('#result').load('<%: Url.Action("Index", "Authority") %>');
        }
    }    

</script>
<%: Html.Telerik().PanelBar()
            .Name("PanelBar")
            .HtmlAttributes(new { style = "width: 125px;" })
                                    .ClientEvents(events => events
                                            .OnSelect("onSelect")
                                    )
                                   .Items(panelbar => panelbar.Add().Text("Details Info.")
                                                          .Items(item =>
                                                                     {
                                                                         item.Add().Text("Add Role");
                                                                         item.Add().Text("Add Authority");
                                                                         item.Add().Text("Add Specification");
                                                                     })
                                                          .Expanded(true))%>
<%--<% Html.Telerik().Window()
           .Name("Window")
           .Title("Menu")
           .Draggable(true)
           .Resizable(resizing => resizing
               .Enabled(false)
               .MinHeight(250)
               .MinWidth(100)
               .MaxHeight(500)
               .MaxWidth(100)
            )
           .Modal(false)
           .Buttons(b => b.Refresh())
           .Content(() =>
           {%>

<%: Html.Telerik().PanelBar()
            .Name("PanelBar")
            .HtmlAttributes(new { style = "width: 125px;" })
                                    .ClientEvents(events => events
                                            .OnSelect("onSelect")
                                    )
                                   .Items(panelbar => panelbar.Add().Text("Details Info.")
                                                          .Items(item =>
                                                                     {
                                                                         item.Add().Text("Add Role");
                                                                         item.Add().Text("Add Authority");
                                                                         item.Add().Text("Add Specification");
                                                                     })
                                                          .Expanded(true))%>


<%})
           .Width(130)
           .Height(300)
           .Render();
%>--%>