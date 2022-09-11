<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="HRIS.Domain.OrgChart.ValueObjects" %>
<%@ Import Namespace="UI.Areas.OrganizationChart.DTO.ViewModels" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend">
        <%: Resources.Areas.Services.AssignEmployeeToPosition.AssignEmployeeToPositionModel.PositionFulfillmentTitle %></legend>
    <br />
    <div id="errorMessage" class="field-validation-error">
    </div>
    <br />
    <%= Html.Telerik().Grid<PositionFulfillmentViewModel>()
                           .Name("PositionFulfillments")
                           .DataKeys(k => k.Add(e => e.Id))
                           .Columns(columns =>
                                        {

                                            columns.Bound(e => e.EmployeeFirstName).Width(100).ReadOnly().Title(Resources.Areas.OrgChart.ValueObjects.PositionFulfillment.PositionFulfillmentModel.FirstName);
                                            columns.Bound(e => e.EmployeeLastName).Width(100).ReadOnly().Title(Resources.Areas.OrgChart.ValueObjects.PositionFulfillment.PositionFulfillmentModel.LastName);
                                            columns.Bound(e => e.FromDate).Format("{0:dd/MM/yyyy}").Width(100).ReadOnly().Title(Resources.Areas.OrgChart.ValueObjects.PositionFulfillment.PositionFulfillmentModel.FromDate);
                                            columns.Bound(e => e.ExpireDate).Format("{0:dd/MM/yyyy}").Width(100).ReadOnly().Title(Resources.Areas.OrgChart.ValueObjects.PositionFulfillment.PositionFulfillmentModel.ExpireDate);
                                            columns.Bound(e => e.Weight).Width(75).Title(Resources.Areas.OrgChart.ValueObjects.PositionFulfillment.PositionFulfillmentModel.Weight);
                                            columns.Bound(e => e.TypeText).Width(75).ReadOnly().Title(Resources.Areas.OrgChart.ValueObjects.PositionFulfillment.PositionFulfillmentModel.Type);
                                            columns.Command(s =>
                                                                {
                                                                    s.Edit().ButtonType(GridButtonType.Image);
                                                                    s.Delete().ButtonType(GridButtonType.Image);
                                                                    s.Custom("ExpireCommand").ButtonType(GridButtonType.Image).Ajax(true)
                                                                        .Action("Expire", "PositionFulfillment")
                                                                        .HtmlAttributes(new { @class = "t-expired" });
                                                                }).Width("15%");
            
                                        })
                           .DataBinding(d =>
                                            {
                                                d.Ajax().Select("Read", "PositionFulfillment");
                                                d.Ajax().Update("Update", "PositionFulfillment");
                                                d.Ajax().Delete("Delete", "PositionFulfillment");
                                               
                                                
                                            })
                           .ClientEvents(e =>
                                             {
                                                 e.OnError("showError").OnComplete("onComplete");
                                                 e.OnRowDataBound("onRowDataBound");
                                             })
                           //.Pageable(paging => paging.PageSize(5))
                           .Scrollable(scrolling => scrolling.Height(150))
                           .Sortable()
    %>
</fieldset>
<script type="text/javascript">
    function showError(e) {
        var divErrorMessage = document.getElementById("errorMessage");
        if (e.textStatus == 'error') {
            if (e.XMLHttpRequest.status == "500") {
                divErrorMessage.innerHTML = e.XMLHttpRequest.responseText;
               
            }

        }
        else {
            divErrorMessage.innerHTML = "";
        }
        e.preventDefault();
    }

   

    function onComplete(e) {
        if (e.name == "ExpireCommand") {
            var data = e.response.Data;
            var grid = $("#PositionFulfillments").data("tGrid");
            grid.dataBind(data);
            $console.log(JSON.stringify(e));

        }
        showError(e);
    }
    function onRowDataBound(e) {
        if (e.dataItem.ExpireDate != null) {
            e.row.style["color"] = "red";
        }
    }

</script>
