<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div id="IndexNav">
    <%: Html.ActionLink(Resources.Shared.Buttons.Function.BackFromIndex, "GoToTheLatestPage", "Employee")%>
</div>
<script type="text/javascript">

    $(document).ready(function () {
        $('#FA').fadeOut('fast');
        $('#IN').fadeOut('fast');
        $('#PersonnelFunctionsArea').fadeOut('fast');
        $('#Navigator').fadeOut('fast');
    });
</script>