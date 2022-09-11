<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Model.PMS.ValueObjects.Implementation.JobDescription.JobDescriptionSection>" %>
<%@ Import Namespace="Model.PMS.ValueObjects.Implementation.JobDescription" %>
<legend>Job Description Section</legend>
<fieldset class="ParentFieldset">
    <div class="layer1">
        <table width="100%">
            <% foreach (var item in (IEnumerable<JobDescriptionSection>)ViewData["JobDescriptionSections"])
               { %>
            <tr>
                <td>
                    <p class="heading">
                        <%: Html.DisplayFor(model => item.Name) %>
                    </p>
                    <div class="content">
                        <table style="border: 3">
                            <tr>
                                <%--                                <td style="width: 25%; vertical-align: central">
                                    <div class="display-label">
                                        <center>
                                            <b>
                                                <%: Html.LabelFor(model => model.Name) %>
                                            </b>
                                        </center>
                                    </div>
                                    <div class="display-field">
                                        <%: Html.DisplayFor(model => item.Name) %>
                                    </div>
                                </td>--%>
                                <td style="width: 25%; vertical-align: central">
                                    <div class="editor-label">
                                        <center>
                                            <b>
                                                <%: Html.LabelFor(model => model.Weight) %>
                                            </b>
                                        </center>
                                    </div>
                                    <div class="editor-field">
                                        <%: Html.EditorFor(model => item.Weight)%>
                                    </div>
                                </td>
                                <td style="width: 25%; vertical-align: central">
                                    <div class="editor-label">
                                        <center>
                                            <b>
                                                <%: Html.LabelFor(model => model.TotalRate) %></b>
                                        </center>
                                    </div>
                                    <div class="editor-field">
                                        <%: Html.EditorFor(model => item.TotalRate)%>
                                    </div>
                                </td>
                                <td style="width: 25%; vertical-align: central">
                                    <div class="editor-label">
                                        <center>
                                            <b>
                                                <%: Html.LabelFor(model => model.FinalSubmit) %></b>
                                        </center>
                                    </div>
                                    <div class="editor-field">
                                        <%: Html.EditorFor(model => item.FinalSubmit)%>
                                    </div>
                                </td>
                                <td style="width: 25%; vertical-align: central">
                                    <input type="button" value="save" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <% } %>
        </table>
    </div>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            jQuery(".content").hide();
            //toggle the componenet with class msg_body
            jQuery(".heading").click(function () {
                jQuery(this).next(".content").slideToggle(200);
            });
        });
    </script>
</fieldset>
