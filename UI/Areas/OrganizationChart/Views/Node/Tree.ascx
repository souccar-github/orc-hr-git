<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div id="infovis">
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $.ajax({
            url: '<%= Url.Action("NodeToJson", "Node", new {area = "OrganizationChart"}) %>/', type: "POST",
            data: {},
            dataType: "json",
            success: function (result) {
                if (result.Success) {
                    $("#SelectedNodeID").attr("value", result.NodeId);
                    $("#SelectedNodeCode").attr("value", result.NodeCode);
                    init(result.Message);
                }
            }
        });
    });  
</script>
