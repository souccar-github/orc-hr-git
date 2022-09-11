<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="UI.Helpers.Route" %>

<% 
    var propertyName = ViewData.ModelMetadata.PropertyName;
    var propertyValue = ViewData.ModelMetadata.Model;
    var id = Guid.NewGuid().ToString();

    var urlData = (RouteValueDictionary)ViewData.ModelMetadata.AdditionalValues.Where(x => x.Key == "AutoCompleteUrlData").Single().Value;

    var url = RouteHelper.GetUrl(this.ViewContext.RequestContext, urlData);
%>

<input type="text" name="<%: propertyName %>" value="<%: propertyValue %>" id="autoList"
    class="autoComplete" />

<script type="text/javascript">
    $(function () {
        $("#autoList").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/Home/AutoCompleteResult/" + request.term,
                    dataType: "json",
                    success: function (data) {
                        response(data);
                    }
                });
            },
            minLength: 2
        });
    });
</script>
