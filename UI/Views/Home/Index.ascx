<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<html>

    <head>
        <title>MAESTRO | Systems </title>
        <link rel="Stylesheet" type="text/css" href='<%=Url.Content("~/Content/default/general.css")%>' />
        <link rel="Stylesheet" type="text/css" href='<%=Url.Content("~/Content/default/home.css")%>' />
        <script type="text/javascript" src='<%=Url.Content("~/Scripts/jquery.min.js")%>'> </script>
        <script type="text/javascript" src='<%=Url.Content("~/Scripts/tooltip.js")%>'> </script>



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
           case 'jobDescription':
               url = '<%:Url.Action("Index", "JobDesc", new {area = "OrganizationChart"}, null)%>';
               break;
           case 'objective':
               url = '<%:Url.Action("Index", "Objective", new {area = "Objective"}, null)%>';
               break;
           case 'projectManagement':
               url = '<%:Url.Action("Index", "ProjectManagement", new {area = "ProjectManagement"}, null)%>';
               break;
           case 'pmsComprehensive':
               url = '<%:Url.Action("Index", "PMSComprehensive", new {area = "PMSComprehensive"}, null)%>';
               break;
           case 'ReportGenerator':
               url = '<%:Url.Action("Index", "QueryBuilder", new {area = "Reporting"}, null)%>';
               break;
           case 'oldHome':
               url = '<%:Url.Action("OldHome", "Home")%>';
               break;
           default:
               url = '<%:Url.Action("Index", "Home")%>';
           }
           ;
           window.location.replace(url);
       }
    </script>

  

    </head>


    <body>



        <div class="container">
            <div class="header">
    
            </div>
            <div class="body">
                <div class="left">
                    <span class="logo"></span>
                </div>
                <div class="right">
                    <div class="hris"></div>
                    <hr />
            
                    <div id="personnel" class="toolTip" onclick=" ChangeModule('personnel') " title="<div class='titleImg'><img src='<%=Url.Content("~/Content/default/imgs/personnel_img.jpg")%>'></div>i have followed up the integration problem with (ChenLei.Ice) via Skype,until we found the problem and solve it, but i still  have some ithe game.">
                        <span class="icon"></span>
                        <span class="title"> Personnel</span>
                    </div>
            
                    <div id="chart" class="toolTip" onclick=" ChangeModule('orgChart') " title="<div class='titleImg'><img src= '<%=Url.Content("~/Content/default/imgs/chart_img.jpg")%>'></div>i have followed up the integration problem with (ChenLei.Ice) via Skype,until we found the problem and solve it, but i still  have some ithe game.">
                        <span class="icon"></span>
                        <span class="title"> Organization Chart</span>
                    </div>
            
                    <div id="description" class="toolTip" onclick=" ChangeModule('jobDescription') " title="<div class='titleImg'><img src='<%=Url.Content("~/Content/default/imgs/job_description.jpg")%>'></div>i have followed up the integration problem with (ChenLei.Ice) via Skype,until we found the problem and solve it, but i still  have some ithe game.">
                        <span class="icon"></span>
                        <span class="title"> Job Description</span>
                    </div>
            
                    <div id="objective" class="toolTip"  onclick=" ChangeModule('objective') " title="<div class='titleImg'><img src= '<%=Url.Content("~/Content/default/imgs/objective.jpg")%>'></div>i have followed up the integration problem with (ChenLei.Ice) via Skype,until we found the problem and solve it, but i still  have some ithe game.">
                        <span class="icon"></span>
                        <span class="title"> Objective </span>
                    </div>
            
                    <div id="pm" class="toolTip"  onclick=" ChangeModule('projectManagement') " title="<div class='titleImg'><img src='<%=Url.Content("~/Content/default/imgs/pm.jpg")%>'></div>i have followed up the integration problem with (ChenLei.Ice) via Skype,until we found the problem and solve it, but i still  have some ithe game.">
                        <span class="icon"></span>
                        <span class="title"> Project Management</span>
                    </div>
                    <div id="pa" class="toolTip" onclick=" ChangeModule('pmsComprehensive') " title="<div class='titleImg'><img src='<%=Url.Content("~/Content/default/imgs/pa.jpg")%>'></div>i have followed up the integration problem with (ChenLei.Ice) via Skype,until we found the problem and solve it, but i still  have some ithe game.">
                        <span class="icon"></span>
                        <span class="title"> Performance Appraisal</span>
                    </div>
                    <div id="td" class="toolTip" title="<div class='titleImg'><img src='<%=Url.Content("~/Content/default/imgs/td.jpg")%>'></div>i have followed up the integration problem with (ChenLei.Ice) via Skype,until we found the problem and solve it, but i still  have some ithe game.">
                        <span class="icon"></span>
                        <span class="title"> Training & Development</span>
                    </div>
                    <div id="rs" class="toolTip" title="<div class='titleImg'><img src='<%=Url.Content("~/Content/default/imgs/rs.jpg")%>'></div>i have followed up the integration problem with (ChenLei.Ice) via Skype,until we found the problem and solve it, but i still  have some ithe game.">
                        <span class="icon"></span>
                        <span class="title"> Recruitment & Staffing</span>
                    </div>
                    <div id="pat" class="toolTip" title="<div class='titleImg'><img src='<%=Url.Content("~/Content/default/imgs/pat.jpg")%>'></div>i have followed up the integration problem with (ChenLei.Ice) via Skype,until we found the problem and solve it, but i still  have some ithe game.">
                        <span class="icon"></span>
                        <span class="title"> Payroll & Attendance</span>
                    </div>
                    <div id="ss" class="toolTip" title="<div class='titleImg'><img src='<%=Url.Content("~/Content/default/imgs/ss.jpg")%>'></div>i have followed up the integration problem with (ChenLei.Ice) via Skype,until we found the problem and solve it, but i still  have some ithe game.">
                        <span class="icon"></span>
                        <span class="title"> Self Services</span>
                    </div>
                    <div id="Div1" class="toolTip" onclick=" ChangeModule('oldHome') " title="<div class='titleImg'><img src='<%=Url.Content("~/Content/default/imgs/ss.jpg")%>'></div>i have followed up the integration problem with (ChenLei.Ice) via Skype,until we found the problem and solve it, but i still  have some ithe game.">
                        <span class="icon"></span>
                        <span class="title"> Old HRIS Home</span>
                    </div>
        
                </div>
                <div class="clear"></div>
            </div>
    

        </div>

        <div class="footer">
            ©2012 MAESTRO
        </div>


    </body>
</html>