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
                $("#Grid").data("tGrid").rebind();
                CloseDialog($('#dialog-form'));
            } else {
                $("#dialog-form").html(jsonEdit.PartialViewHtml);
            }
        }
    </script>
    <script type="text/javascript">

        function onAppraisalSectionComplete(e) {
            if (e.name == "editSection") {
                $("#dialog-form").html(e.response.PartialViewHtml);
                open($("#dialog-form"), "Edit Section");
            }
            if (e.name == "deleteSection") {
                $("#AppraisalSectionGridDiv").html(e.response.PartialViewHtml);
                if (e.response.Refresh) {
                    $("#AppraisalSectionItemGridDiv").html("");
                    $("#AppraisalSectionItemKpiGridDiv").html("");
                }
            }
        }

        function open(dialogDiv, title) {
            dialogDiv.dialog('option', 'title', title);
            dialogDiv.dialog('open');
        }

        function CloseDialog(dialogDiv) {
            dialogDiv.dialog('close');
        }

    </script>

    <fieldset class="ParentFieldset">
        <legend class="ParentLegend">Appraisal Sections List - Select Row For Details</legend>
        <div id="dialog-form">
        </div>
        <div>
            <div id="AppraisalSectionGridDiv" style="float: left">
                <% Html.RenderPartial("AppraisalSectionGrid"); %>
            </div>
            <div style="float: right;">
                <div id="AppraisalSectionItemGridDiv">
                </div>
                <div id="AppraisalSectionItemKpiGridDiv">
                </div>
            </div>
        </div>

    </fieldset>

</asp:Content>
