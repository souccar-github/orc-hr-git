<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<table style="margin-left:0; margin-top:0; height:auto">
    <tr style="height: 100%; width:100%" >
            <fieldset class="ParentFieldset">
                    <legend class="ParentLegend"><%: Resources.Areas.Services.ResourceFileTranslate.ResourceFileTranslateModel.SelectResourceFileLegend %></legend>
                    <%Html.RenderPartial("DropDownLists/ResourceFilesList");%>
                    <br/>
                    <%Html.RenderPartial("DropDownLists/SoftwareLanguagesList");%>
                    <br/>
                    <input type="button" onclick=" TranslateResourceFile(); " value=<%: Resources.Shared.Buttons.Function.Translate %> ;style="width: auto" />
            </fieldset>
    </tr>
    
</table>

<script type="text/javascript">

    var filePath = "";
    var fileName = "";
    var language = "";

    function rsourceFileNamesValueChanged() {

        var comboBox = $("#AutoCompleteRsourceFileNamesComboBox").data("tComboBox");
        if (comboBox.value() != null && comboBox.value() != 0 && comboBox.text() != null) {
            filePath = comboBox.value();
            fileName = comboBox.text();
        }
    }

    function softwareLanguagesValueChanged() {

        var comboBox = $("#AutoCompleteSoftwareLanguagesComboBox").data("tComboBox");
        if (comboBox.value() != null && comboBox.value() != 0) {
            language = comboBox.value();
        }
    }

    function TranslateResourceFile() {

        $.ajax({
            url: '<%:Url.Action("TranslateResourceFile", "ResourceFileTranslate", new { area = "Services"})%>/',
            type: "POST",
            data: { sourceFilePath: filePath,
                    sourceFileName: fileName,
                    language: language
            },
            success: function (result) {
                    alert(result.Msg);
            }
        });
    }

    </script>
