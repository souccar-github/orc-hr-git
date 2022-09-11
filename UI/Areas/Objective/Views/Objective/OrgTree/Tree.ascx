<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div id="infovis">
</div>
<script type="text/javascript">
    $(document).ready(function() {
        $.ajax({
                url: '<%:Url.Action("NodeToJson", "BasicObjective")%>/',
                type: "POST",
                data: {  },
                success: function(result) {
                    if (result.Success) {
                        $("#SelectedNodeID").attr("value", result.NodeId);
                        init(result.Message);
                    }
                }
            });
    });  
</script>
