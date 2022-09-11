    <%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div id="IndexNav">
    <%: Html.ActionLink(Resources.Shared.Buttons.Function.BackFromIndex, "GoToTheLatestPage", "Project")%>
</div>
<script type="text/javascript">

    $(document).ready(function () {
        $('#FA').fadeOut('fast');
        $('#IN').fadeOut('fast');
        $('#ProjectFunctionsArea').fadeOut('fast');
        $('#Navigator').fadeOut('fast');
    });
</script>
