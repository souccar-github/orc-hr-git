<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div id="IndexNav">
    <%: Html.ActionLink(Resources.Shared.Buttons.Function.BackFromIndex, "GoToTheLatestPage", "JobDescEntity")%>
</div>
<script type="text/javascript">

    $(document).ready(function () {
        $('#FA').fadeOut('fast');
        $('#IN').fadeOut('fast');
        $('#JobFunctionsArea').fadeOut('fast');
        $('#Navigator').fadeOut('fast');
    });
</script>
