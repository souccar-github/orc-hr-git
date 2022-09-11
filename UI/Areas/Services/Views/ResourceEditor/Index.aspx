<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Services/Views/Shared/Services.master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%  Html.Telerik().Splitter().Name("SplitterTree")
            .Orientation(SplitterOrientation.Vertical)
            .HtmlAttributes(new { style = "height: 500px;" })
            .Panes(hPanes =>
            {
                hPanes.Add()
                    .Size("490px")
                    .Resizable(false)
                    .Collapsible(false)
                    .Scrollable(false)
                    .Collapsed(true)
                    .Content(() =>
                    {
                    });

                hPanes.Add()
                    .Resizable(true)
                    .Scrollable(true)
                    .Content(() => Html.RenderPartial("Container"));
            })
            .Render();
    %>
    <br />
    <div id="result" style="display: none">
    </div>
</asp:Content>
