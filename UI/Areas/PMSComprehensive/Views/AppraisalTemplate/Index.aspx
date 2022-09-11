<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/PMSComprehensive/Views/Shared/PMSComprehensive.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#dialog-form").dialog({
                autoOpen: false,
                height: 'auto',
                width: 'auto',
                modal: true,
                resizable: false
            });
        });

        function JsonSave_OnComplete(context) {
            var jsonEdit = context.get_response().get_object();
            if (jsonEdit.Success) {
                $("#AppraisalTemplatesGrid").data("tGrid").rebind();
                CloseDialog($('#dialog-form'));
            } else {
                $("#dialog-form").html(jsonEdit.PartialViewHtml);
            }
        }
    </script>
    <script type="text/javascript">
        function open(dialogDiv, title) {
            dialogDiv.dialog('option', 'title', title);
            dialogDiv.dialog('open');
        }

        function CloseDialog(dialogDiv) {
            dialogDiv.dialog('close');
        }

    </script>

    <div id="dialog-form">
    </div>
    <div id="MasterGridDiv">
        <%Html.RenderPartial("MasterGrid"); %>
    </div>

    <br />
    <div id="result" style="display: none">
    </div>
</asp:Content>
