<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
    <table width="100%">
        <tr>
            <td>
                <input type="button" onclick=" openWindow(); " id="restoreButton" value="Appraisal Sections Window"
                    style="width: auto" />
            </td>
        </tr>
    </table>
<%
    Html.Telerik().Window()
        .Name("Window")
        .Title("Appraisal Sections")
        .Draggable(true)
        .Content(() =>
                     {%>
<div id="operations">
    <a href="#" onclick=" ShowAppraisalGrid(); ">Appraisal General Info</a>


    <br />
    <a href="#" onclick=" PanelBarOnSelect('edit'); ">Competency Section</a>
    <br />
    <a href="#" onclick=" PanelBarOnSelect('delete'); ">Job Description Section</a>
    <br />
    <a href="#" onclick=" PanelBarOnSelect('addPos'); ">Objective Section</a>
    <br />
    <a href="#" onclick=" PanelBarOnSelect('showPos'); ">Project Section</a>
    <br />
</div>
<%
                     })
        .Width(170)
        .Height(100)
        .Render();
%>


<script type="text/javascript">


    function ShowAppraisalGrid() {
        var id = $("#appID").attr("value");
        $('#ValueObjectsList').load('<%: Url.Action("MasterGrid", "LiveAppraisal") %>');

//        var id = $("#appID").attr("value");
//         alert(id);
//        $.ajax({
//            url: '<%: Url.Action("MasterGrid", "LiveAppraisal")%>/', type: "POST",
//            dataType: "json",
//            data: { appraisalId: id },          
//            success: function (data) {
//                alert("Success back from Master gird controller");
//                $("#ValueObjectsList").html(data.PartialViewHtml);
//            }
//                });
}

    function openWindow() {
        var window = $("#Window").data("tWindow");
        window.open();

    }

</script>