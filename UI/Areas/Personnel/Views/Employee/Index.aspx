<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Personnel/Views/Shared/Personnel.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("MasterGrid"); %>
    <br />
    <div id="result" style="display: none">
    </div>
</asp:Content>
