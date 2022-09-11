<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="HRIS.Domain.PMS.ValueObjects.Implementation.JobDescription" %>
<fieldset class="ParentFieldset">
    <div id="dialog-form" title="Job Description Section Item Kpis">
    </div>
    <legend class="ParentLegend">Job Description Section Item Details</legend>
    <table width="100%">
        <tr>
            <td>
                <%
                    Html.Telerik().Grid<JobDescriptionSectionItem>("jobDescriptionSectionItems")
                        .Name("Grid")
                        .DataKeys(k => k.Add(o => o.Id))
                        .Filterable()
                        .Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                        .Columns(c =>
                                     {
                                         c.Bound(o => o.Id).Title("No").Width(1).Groupable(false).Filterable(false).Sortable(false);
                                         c.Bound(o => o.JobTitle);
                                         c.Bound(o => o.Weigth);
                                         c.Bound(o => o.Rate);
                                     }).DetailView(detailView => detailView.Template(e =>
                                                                                         {
                %>
                <% Html.Telerik().TabStrip()
                    .Name("TabStrip_" + e.Id)
                    .Effects(fx => fx.Opacity())
                    .SelectedIndex(Session["SelectedTabIndexSecondLevel"] != null ? int.Parse(Session["SelectedTabIndexSecondLevel"].ToString()) : 111)
                    .Items(items =>
                               {

                                   //Job Description Section Task
                                   items.Add().Text("Job Description Section Task").Content(() =>
                                                                                {
                %>
                <% Html.Telerik().Grid(e.JobDescriptionSectionTasks)
                           .Name("JobDescriptionSectionTasks" + e.Id)
                           .DataKeys(s => s.Add(x => x.Id))
                           .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Title("No").Groupable(false).Filterable(
                                                false).Sortable(false);
                                            columns.Bound(o => o.RoleName);
                                            columns.Bound(o => o.JobTask);
                                            columns.Bound(o => o.Rate);
                                            columns.Bound(o => o.Weight);
                                        })
                            .ClientEvents(events => events.OnRowSelect("JobDescriptionSectionTaskRowSelected"))
                            //.OnDetailViewExpand("OnDetailViewExpandJobDescriptionSectionTask"))
                           .Selectable()
                           .Pageable(pager => pager.PageSize(5))
                           .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending())).Render();
            %>
                <%
                                                                 });

                                                      })
                                        .ClientEvents(events => events.OnSelect("tabStripSelect"))
                                        .Render();
                %>
                <%
                            }))

                                            .RowAction(row =>
                                            {
                                                if (ViewData["SelectedRow"] != null)
                                                    if (row.DataItem.Id == (int)ViewData["SelectedRow"])
                                                    {
                                                        {
                                                            row.DetailRow.Expanded = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        row.DetailRow.Expanded = false;
                                                    }
                                            })
                                            .ClientEvents(builder =>
                                            {
                                                builder.OnRowSelect("loadPartialView");
                                                builder.OnDetailViewExpand("SetMasterRecordValue");
                                                builder.OnDetailViewCollapse("SetMasterRecordValue");
                                            })
                                            .Pageable(p => p.PageSize(5).PageTo((int)ViewData["PageTo"]))
                                            .Sortable()
                                            .Filterable()
                                            .Selectable()
                                            .Render();
                %>
            </td>
        </tr>
    </table>
</fieldset>
<script type="text/javascript">

    function tabStripSelect(e) {

        var item = $(e.item);

        $('#result').fadeOut();

        $.ajax({
            url: '<%=Url.Action("SaveTabIndexSecondLevel", "Appraisal")%>/', type: "POST",
            data: { selectedIndex: item.index() }
        });
    }

    function SetMasterRecordValue(e) {

        var x = e.masterRow.cells[1].innerHTML;

        $('#result').fadeOut('fast');

        $('#result').load('<%: Url.Action("Index", "JobDescriptionSectionItem") %>', { selectedSubRowId: x.toString() }, function () {
            $('#result').fadeIn('slow');
        });
    }

    function loadPartialView(e) {

        $('> .t-hierarchy-cell > .t-icon', e.row).click();

        $('#result').fadeOut('fast');
    }

    function loadRibbon() {
        $('#ProjectFunctionsArea').load('<%:Url.Action("GetFunctionsPartial", "PMSComprehensive")%>');
    }

   

    function JobDescriptionSectionTaskRowSelected(e) {
        $('> .t-hierarchy-cell > .t-icon', e.row).click();
        $('#result').fadeOut('fast');

        var x = e.row.cells[0].innerHTML;

        var url = '<%:Url.Action("Index", "JobDescriptionSectionTask", new {selectedSubRowId = "Value1"})%>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('fast');
            loadRibbon();
            ShowDialog();
        });
    }

    function OnDetailViewExpandJobDescriptionSectionTask(e) {

        var x = e.masterRow.cells[1].innerHTML;

        var url = '<%: Url.Action("Index", "JobDescriptionSectionTask", new { selectedSubRowId = "Value1"}) %>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('slow');
            loadRibbon();
        });
    }

    function JobDescriptionSectionItemKpiRowSelected(e) {
        $('#result').fadeOut('fast');

        var x = e.row.cells[0].innerHTML;

        var url = '<%: Url.Action("Index", "JobDescriptionSectionItemKpi", new { selectedSubRowId = "Value1" }) %>';
        url = url.replace("Value1", x);
        $('#result').load(url, function () {
            $('#result').fadeIn('slow');
        });
    }

    function ShowDialog() {
        $.ajax({
            type: "POST",
            traditional: true,
            url: '<%: Url.Action("ReadOnly", "JobDescriptionSectionItemKpi") %>',
            success: function (result) {
                if (result.Success == false) {
                    alert(result.Message);
                } else {
                    $('#dialog-form').html(result.PartialViewHtml);
                    $('#dialog-form').dialog('open');
                }
            }
        });
    }

    $(document).ready(function () {

        $("#dialog").dialog("destroy");
        $("#dialog-form").dialog({
            autoOpen: false,
            height: 'auto',
            width: 'auto',
            modal: true,
            resizable: false,
            buttons: {
                Cancel: function () {
                    $(this).dialog('close');
                }
            }
        });
    });


</script>
