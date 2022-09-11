<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="UI.Areas.Services.DTO.ViewModels" %>

<legend class="ParentLegend">
        <%: Resources.Areas.Services.ResourceEditor.ResourceEditorModel.ResourceGridTitle %></legend>

    <%= Html.Telerik().Grid<ResourceEditorViewModel>().Name("Resouces")
        .DataKeys(k => k.Add(e => e.Key))
        .Columns(columns =>
        {
            columns.Bound(e => e.Key).Width(140).ReadOnly();
            columns.Bound(e => e.Value).Width(140);
            columns.Command(s => s.Edit().ButtonType(GridButtonType.Image)).Width("15%").HtmlAttributes(new { @class = "t-edit" });
        })
        .ClientEvents(events => events
                .OnRowDataBound("resouces_onRowDataBound"))
                .DataBinding(d =>
                    {
                        d.Ajax().Select("ReadResource", "ResourceEditor");
                        d.Ajax().Update("UpdateResource", "ResourceEditor");
                            })
        
        .Scrollable(scrolling => scrolling.Height(580))
        .Sortable()
        .Filterable()
        .KeyboardNavigation()
    %>
    <script type="text/javascript">

        function resouces_onRowDataBound(e) {
            $(this).data('tGrid');
        }

    </script>
