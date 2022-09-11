<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<script type="text/javascript">

    function onSelect(e) {
        var item = $(e.item);

        if (item.find('> .t-link').text() == "Details") {
            $('#result').load('<%: Url.Action("PartialInfo", "Position") %>');
        }
        if (item.find('> .t-link').text() == "Basic Details") {
            $('#result').load('<%: Url.Action("Index", "PositionGrade") %>');
        }
        if (item.find('> .t-link').text() == "Insurances") {
            $('#result').load('<%: Url.Action("Index", "AssignedInsurance") %>');
        }

        if (item.find('> .t-link').text() == "Cash Deductions") {
            $('#result').load('<%: Url.Action("Index", "AssignedCashDeduction") %>');
        }

        if (item.find('> .t-link').text() == "Cash Benefits") {
            $('#result').load('<%: Url.Action("Index", "AssignedCashBenefit") %>');
        }
        if (item.find('> .t-link').text() == "Assets") {
            $('#result').load('<%: Url.Action("Index", "AssignedAsset") %>');
        }
        if (item.find('> .t-link').text() == "None-Cash Benefits") {
            $('#result').load('<%: Url.Action("Index", "AssignedNonCashBenefit") %>');
        }
    }    

</script>

<%: Html.Telerik().PanelBar()
            .Name("PanelBar")
            .HtmlAttributes(new { style = "width: 125px;" })
                                    .ClientEvents(events => events
                                            .OnSelect("onSelect")
                                    )
                                   .Items(panelbar =>
                                              {
                                                  panelbar.Add().Text("Position Info.")
                                                      .Items(item =>
                                                                 {
                                                                     item.Add().Text("Details");
                                                                 })
                                                      .Expanded(true);

                                                  panelbar.Add().Text("Grade Info.")
                                                      .Items(item =>
                                                                 {
                                                                     item.Add().Text("Basic Details");//.Enabled((bool)ViewData["PositionExist"]);
                                                                     item.Add().Text("Assets");//.Enabled((bool)ViewData["PositionExist"]);
                                                                     item.Add().Text("Cash Benefits");//.Enabled((bool)ViewData["PositionExist"]);
                                                                     item.Add().Text("Cash Deductions");//.Enabled((bool)ViewData["PositionExist"]);
                                                                     item.Add().Text("Insurances");//.Enabled((bool)ViewData["PositionExist"]);
                                                                     item.Add().Text("None-Cash Benefits");//.Enabled((bool)ViewData["PositionExist"]);
                                                                 })
                                                      .Expanded(true);
                                              })%>