<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.PMS.RootEntities.Appraisal>" %>
<fieldset>
   <style type="text/css">
       .sectionTable {
           border: 1px;
           font-family: sans-serif;
           border-bottom: 1px;
           border-style: solid;          
       }

       .tdDefault {
           width: auto;
           vertical-align: top;
           border-style: solid;
           horiz-align: center;
       }
       
    .headerSectionTable {
           width: auto;
           vertical-align: top;
           border-style: solid;  
           font-size: large
       }
       
   </style> 
    <legend>Sections </legend>
    <% Html.HiddenFor(model => model.Id);%>
    <table style="width: 100%" class="sectionTable">
        <tr>
            <td style="width: 30%; vertical-align: top;font-size: large; border-style: solid; ">
                Description
            </td>
            <td class="headerSectionTable">
                Weight
            </td>
            <td class="headerSectionTable">
                Rate
            </td>
            <td class="headerSectionTable" rowspan="7">
                Total
            </td>
        </tr>
        <% if (Model != null && (Model.CompetencySections.SingleOrDefault() != null))
           { %>
        <tr>
            <td class="tdDefault">
                <a href="#" onclick="goToSection('com')" style="font-size: small">Competency Section
                </a>
            </td>
            <td class="tdDefault">
                <%: Html.Encode(Model.CompetencySections.SingleOrDefault().Weight) %>
            </td>
            <td class="tdDefault">
                <%: Html.Encode(Model.CompetencySections.SingleOrDefault().TotalRate) %>
            </td>
      
        </tr>
        <% } %>
        <% if (Model != null && (Model.JobDescriptionSections.SingleOrDefault() != null))
           { %>
        <tr>
            <td class="tdDefault">
                <a href="#" onclick="goToSection('job')">Job Description Section </a>
            </td>
            <td class="tdDefault">
                <%: Html.Encode(Model.JobDescriptionSections.SingleOrDefault().Weight) %>
            </td>
            <td class="tdDefault">
                <%: Html.Encode(Model.JobDescriptionSections.SingleOrDefault().TotalRate)%>
            </td>
        </tr>
        <% } %>
        <% if (Model != null && (Model.ObjectiveSections.SingleOrDefault() != null))
           { %>
        <tr>
            <td class="tdDefault">
                <a href="#" onclick="goToSection('obj')">Objective Section </a>
            </td>
            <td class="tdDefault">
                <%: Html.Encode(Model.ObjectiveSections.SingleOrDefault().Weight) %>
            </td>
            <td class="tdDefault">
                <%: Html.Encode(Model.ObjectiveSections.SingleOrDefault().TotalRate) %>
            </td>
        </tr>
        <% } %>
        <% if (Model != null && (Model.OrganizationalSections.Count != 0))
           { %>
        <% foreach (var section in Model.OrganizationalSections)
           { %>
        <tr>
            <td class="tdDefault">
                <a href="#" onclick="goToGenericSection('<%=section.Id %>')">  <%: Html.Encode(section.Name) %> </a>
            </td>
            <td class="tdDefault">
                <%: Html.Encode(section.Weight) %>
            </td>
            <td class="tdDefault">
                <%: Html.Encode(section.TotalRate)%>
            </td>
        </tr>
        <tr>
         <%--   <%= Html.Telerik().Chart<Model.PMS.Entities.Appraisal>()
            .Name("pieChart")       
            .Title("Break-up of Spain Electricity Production for 2008")
            .Legend(legend => legend
                .Position(ChartLegendPosition.Bottom)
            )
            .Series(series => {
                series.Pie("Percentage", "Source")
                      .Labels(labels => labels.Visible((bool)ViewBag.showLabels).Template("<#= value
                          #>%")
                          .Align((ChartPieLabelsAlign)ViewBag.align)
                          .Position((ChartPieLabelsPosition)ViewBag.position))
                      .StartAngle((int)ViewBag.startAngle).Padding((int)ViewBag.padding);
            })
            .DataBinding(dataBinding => dataBinding
                .Ajax().Select("_SpainElectricity", "Chart")
            )
            .Tooltip(tooltip => tooltip.Visible(true).Template("<#= value #>%"))
            .HtmlAttributes(new { style = "width: 500px; height: 400px;" })
    %>--%>
        </tr>
        <% } %>
        <% } %>

    </table>
</fieldset>
<script type="text/javascript">

    function goToGenericSection(sectionId) {

        var url = '<%:Url.Action("CustomizedSection", "LiveAppraisal", new {sectionID = "Value"})%>';
        url = url.replace("Value", sectionId);
        $.ajax({
            data: { appraisalId: "<%: Model.Id %>" },
            url: url, type: "POST",
            success: function (result) {
                $('#sectionsDiv').html(result.PartialViewHtml);
                $('#sectionsDiv').show();

            }

        });
    }

    function goToSection(section) {
        var url;
        switch (section) {

            case 'job':
                url = '<%= Url.Action("JobDescriptionSection","LiveAppraisal") %>/';
                break;
            case 'com':
                url = '<%= Url.Action("CompetencySection","LiveAppraisal") %>/';
                break;
            case 'obj':
                url = '<%= Url.Action("ObjectiveSection","LiveAppraisal") %>/';
                break;
            case 'pro':
                url = '<%= Url.Action("ProjectSection","LiveAppraisal") %>/';
                break;
            default:
                url = '<%= Url.Action("index","LiveAppraisal") %>/';

        }

   
        $.ajax({
            data: { appraisalId: "<%: Model.Id %>" },
            url: url, type: "POST",
            success: function (result) {
                $('#sectionsDiv').html(result.PartialViewHtml);
                $('#sectionsDiv').show();

            }

        });

    }

</script>
