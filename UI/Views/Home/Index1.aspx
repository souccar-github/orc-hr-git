<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="Resources.Shared.Messages" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%:General.Welcome%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="MainData" style="vertical-align: middle; height: 700px;">
        <% Html.RenderPartial("GlobalError"); %>
        <div id="ContentPlaceHolder">
            <table style="width: 100%; height: 100%; margin-top: 150px">
                <%--                <tr>
                    <td style="width: 33%; vertical-align: middle; text-align: center">
                        <img src="<%= Url.Content("~/Content/Ribbon/home-page-icon.png") %>" title="<%:Resources.Views.Home.Buttons.AddJobDescription %>"
                            alt="<%:Resources.Views.Home.Buttons.AddJobDescription %>" />
                        <%: Html.ActionLink(Resources.Views.Home.Buttons.Administration, "Index", "Portal", new { area = "Portal" }, null)%>
                    </td>
                    <td style="width: 33%; vertical-align: middle; text-align: center">
                        <img src="<%= Url.Content("~/Content/Ribbon/application-icon.png") %>" title="<%:Resources.Views.Home.Buttons.AddJobDescription %>"
                            alt="<%:Resources.Views.Home.Buttons.AddJobDescription %>" />
                        <%: Html.ActionLink(Resources.Views.Home.Buttons.Objectives, "Index", "Portal", new { area = "Portal" }, null) %>
                    </td>
                    <td style="width: 33%; vertical-align: middle; text-align: center">
                        <% if ((bool)ViewData["StartAppraisal"])
                           { %>
                        <img src="<%= Url.Content("~/Content/Ribbon/application-icon.png") %>" title="<%:Resources.Views.Home.Buttons.AddJobDescription %>"
                            alt="<%:Resources.Views.Home.Buttons.AddJobDescription %>" />
                        <%: Html.ActionLink(Resources.Views.Home.Buttons.PMS, "Index", "PMSComprehensiveLive", new { area = "PMSComprehensiveLive" }, null)%>
                        <% } %>
                    </td>
                </tr>--%>
                <div class="tRTF">
                    <div class="section position3">
                        <h3>
                            Our Modules</h3>
                        <div class="tTwoColumnsLeft">
                            <div class="color-box dev-tools">
                                <div>
                                    <h4>
                                        Personnel</h4>
                                    <strong>Manage Employee Personal Information
                                        <br>
                                        and all related things.</strong> <a href="#" class="box-link" onclick=" ChangeModule('personnel'); ">
                                                                             Organization chart module</a>
                                </div>
                            </div>
                        </div>
                        <div class="tTwoColumnsRight">
                            <div class="color-box kendo">
                                <div>
                                    <h4>
                                        Organization chart</h4>
                                    <strong>Everything you need to build Company hierarchy
                                        <br>
                                        and Company positions. </strong><a href="#" class="box-link" onclick=" ChangeModule('orgChart'); ">
                                                                            Organization chart module</a>
                                </div>
                            </div>
                        </div>
                        <div class="tThreeColumnsLeftCenter  tRemoveBottomMargin">
                            <div class="color-box test-studio">
                                <div>
                                    <h4>
                                        Objective</h4>
                                    <strong>Make your business go by objective
                                        <br>
                                        your company and staff objectives can be defined here. </strong><a href="#" class="box-link"
                                                                                                           onclick=" ChangeModule('objective'); ">Objective module</a>
                                </div>
                            </div>
                        </div>
                        <div class="tThreeColumnsLeftCenter tRemoveBottomMargin">
                            <div class="color-box sitefinity">
                                <div>
                                    <h4>
                                        Project Management</h4>
                                    <strong>Project management, Tasks, Resources.<br>
                                                                                 All you need to track you projects in the company. </strong><a href="#" class="box-link"
                                                                                                                                                onclick=" ChangeModule('projectManagement'); ">ProjectManagement module</a>
                                </div>
                            </div>
                        </div>
                        <div class="tThreeColumnsRight tRemoveBottomMargin">
                            <div class="color-box team-pulse">
                                <div>
                                    <h4>
                                        Performance Appraisal</h4>
                                    <strong>Simple All types of Appraisal (Comprehensive,360)
                                        <br>
                                        Evaluate your Employees </strong><a href="#" class="box-link" onclick=" ChangeModule('pmsComprehensive'); ">
                                                                             ProjectManagement module</a>
                                </div>
                            </div>
                        </div>
                        <div class="tThreeColumnsRight tRemoveBottomMargin">
                            <div class="color-box team-pulse">
                                <div>
                                    <h4>
                                        Assign Employee To Position</h4>
                                    <strong>Assign Employee To Position
                                        <br>
                                        </strong><a href="#" class="box-link" onclick=" ChangeModule('AssignEmployeeToPosition'); ">
                                                                             Assign Employee To Position</a>
                                </div>
                            </div>
                        </div>
                        <div class="tThreeColumnsRight tRemoveBottomMargin">
                            <div class="color-box team-pulse">
                                <div>
                                    <h4>
                                        Delegation</h4>
                                    <strong>Delegation
                                        <br>
                                        </strong><a href="#" class="box-link" onclick=" ChangeModule('Delegation'); ">
                                                                             Delegation</a>
                                </div>
                            </div>
                        </div>
                        <div class="tThreeColumnsLeftCenter tRemoveBottomMargin">
                            <div class="color-box sitefinity">
                                <div>
                                    <h4>
                                        Online Editible Grid</h4>
                                    <strong>You can edit a record online
                                    <br>
                                    Good bye to partial edit and welcome to online edit. 
                                    </strong><a href="#" class="box-link" onclick=" ChangeModule('OnlineEdit'); ">ProjectManagement module</a>
                                </div>
                            </div>
                        </div>
                        <div class="tThreeColumnsLeftCenter tRemoveBottomMargin">
                            <div class="color-box sitefinity">
                                <div>
                                    <h4>
                                        Report</h4>
                                    <strong> 
                                    </strong><a href="#" class="box-link" onclick=" ChangeModule('Report'); ">Report</a>
                                </div>
                            </div>
                        </div>
                        <div class="tThreeColumnsLeftCenter tRemoveBottomMargin">
                            <div class="color-box sitefinity">
                                <div>
                                    <h4>
                                        ReportGenerator</h4>
                                    <strong> 
                                    </strong><a href="#" class="box-link" onclick=" ChangeModule('ReportGenerator'); ">ReportGenerator</a>
                                </div>
                            </div>
                        </div>
                        <div class="tThreeColumnsLeftCenter tRemoveBottomMargin">
                            <div class="color-box dev-tools">
                                <div>
                                    <h4>
                                        Resource Editor</h4>
                                    <strong></strong> <a href="#" class="box-link" onclick=" ChangeModule('ResourceEditor'); ">
                                                                             ResourceEditor</a>
                                </div>
                                </div>
                            </div>
                            <div class="tThreeColumnsLeftCenter tRemoveBottomMargin">
                            <div class="color-box test-studio">
                                <div>
                                    <h4>
                                        Resource File Translate</h4>
                                    <strong></strong><a href="#" class="box-link" onclick=" ChangeModule('ResourceFileTranslate'); ">ResourceFileTranslate</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </table>
        </div>
    </div>
    <script type="text/javascript">
        function ChangeModule(module) {
            var url;
            switch (module) {
                case 'ResourceEditor':
                    url = '<%:Url.Action("Index", "ResourceEditor", new {area = "Services"}, null)%>';
                    break;
                case 'ResourceFileTranslate':
                    url = '<%:Url.Action("Index", "ResourceFileTranslate", new {area = "Services"}, null)%>';
                    break;
                case 'OnlineEdit':
                    url = '<%:Url.Action("Index", "EmployeeOnline", new {area = "Services"}, null)%>';
                    break;
                case 'Report':
                    url = '<%:Url.Action("Index", "ReportTemplate", new {area = "Reporting"}, null)%>';
                    break;
                case 'Delegation':
                    url = '<%:Url.Action("Index", "Delegation", new {area = "Services"}, null)%>';
                    break;
                case 'AssignEmployeeToPosition':
                    url = '<%:Url.Action("GetTreeNodes", "AssignEmployeeToPosition", new {area = "Services"}, null)%>';
                break;
            case 'personnel':
                url = '<%:Url.Action("Index", "Personnel", new {area = "Personnel"}, null)%>';
                break;
            case 'orgChart':
                url = '<%:Url.Action("Index", "OrganizationChart", new {area = "OrganizationChart"}, null)%>';
                break;
            case 'objective':
                url = '<%:Url.Action("Index", "Objective", new {area = "Objective"}, null)%>';
                break;
            case 'projectManagement':
                url = '<%:Url.Action("Index", "ProjectManagement", new {area = "ProjectManagement"}, null)%>';
                break;
            case 'pmsComprehensive':
                url = '<%:Url.Action("Index", "AppraisalSection", new {area = "PMSComprehensive"}, null)%>';
                break;
            case 'ReportGenerator':
                url = '<%:Url.Action("Index", "QueryBuilder", new {area = "Reporting"}, null)%>';
                break;
            default:
                url = '<%:Url.Action("Index", "Home")%>';
            }
            ;
            window.location.replace(url);
        }
    </script>
</asp:Content>