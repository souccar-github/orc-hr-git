<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/PMSComprehensiveLive/Views/Shared/PMSComprehensiveLive.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width: 100%; border: 3">
        <tr style="height: 2%">
            <td colspan="2">
                <h1>
                    Appraisal</h1>
                <div id="appraisalInfo">
                    <% Html.RenderPartial("AppraisalInfo"); %>
                </div>
            </td>
        </tr>

        <tr>
            <td style="width: 35%">
                <% Html.RenderPartial("SectionsMenu"); %>
            </td>
            <td style="width: 65%">
                <div id="sectionsDiv">
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
