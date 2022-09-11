<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<table style="margin-left:0; margin-top:0; height:auto">
    <tr style="height: 100%; width:100%" >
            <fieldset class="ParentFieldset">
                    <legend class="ParentLegend"><%: Resources.Areas.Services.ResourceEditor.ResourceEditorModel.SelectResourceFileLegend %></legend>
                    <%Html.RenderPartial("DropDownLists/SoftwareLanguagesList");%>
                    <br/>
                    <%Html.RenderPartial("DropDownLists/ResourceFilesList");%>
            </fieldset>
            <br/>
            <div id="ResourceGrid" style="overflow: auto">
            </div>
    </tr>
    
</table>

<script type="text/javascript">

    var rsourceFileNamesComboBox = null;
    var softwareLanguagesComboBox = null;
    var fullPath = "";
    var language = "";

    function pageLoaded() {
        rsourceFileNamesComboBox = $("#AutoCompleteRsourceFileNamesComboBox").data("tComboBox");
        softwareLanguagesComboBox = $("#AutoCompleteSoftwareLanguagesComboBox").data("tComboBox");
    }

    function softwareLanguagesValueChanged() {
        fullPath = "";
        
        if (softwareLanguagesComboBox.value() != null && softwareLanguagesComboBox.value() != "") {
            language = softwareLanguagesComboBox.value();

            $.ajax({
                url: '<%:Url.Action("AssignLanguage", "ResourceEditor", new { area = "Services"})%>/',
                type: "POST",
                data: { language: language },
                success: function (result) {
                    rsourceFileNamesComboBox.dataBind(result);
                }
            });

            if (rsourceFileNamesComboBox.value() != null && rsourceFileNamesComboBox.value() != "") {
                FillDataGrid();
            }
        }
    }
    
    function rsourceFileNamesValueChanged() {   
        if (rsourceFileNamesComboBox.value() != null && rsourceFileNamesComboBox.value() != "") {
            fullPath = rsourceFileNamesComboBox.value();
        }

        FillDataGrid();
    }

    function FillDataGrid() {

        $.ajax({
            url: '<%:Url.Action("ResourceGrid", "ResourceEditor", new { area = "Services"})%>/',
            type: "POST",
            data: { fullPath: fullPath },
            success: function (result) {
                if (result.Success) {
                    $('#ResourceGrid').html(result.PartialViewHtml);
                    $('#ResourceGrid').fadeIn('fast');
                }
            }
        });
    }

    </script>
    <body onload="pageLoaded() ;">