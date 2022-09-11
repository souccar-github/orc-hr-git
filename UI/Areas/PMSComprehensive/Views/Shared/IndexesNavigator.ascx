<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div id="IndexNav">
    <%: Html.ActionLink("Back To Previous Page", "GoToTheLatestPage", "PMSComprehensive")%>
</div>
<script type="text/javascript">

    $(document).ready(function () {
        $('#FA').fadeOut('fast');
        $('#IN').fadeOut('fast');
        $('#PMSComprehensiveFunctionsArea').fadeOut('fast');
        $('#Navigator').fadeOut('fast');
    });
</script>
