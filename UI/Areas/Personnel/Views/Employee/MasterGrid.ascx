<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend">
        <%: Resources.Areas.Personnel.Entities.Employee.EmployeeModel.SelectEmployeeMessage %></legend>
    <table width="100%">
        <tr>
            <td style="width: 90%">
                <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                   {%>
                <a href="<%: Url.Action("Insert", "Employee") %>">
                    <img src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>" title="<%: Resources.Shared.Buttons.Function.Add %>"
                        alt="<%: Resources.Shared.Buttons.Function.Add %>" height="36" width="36" align="middle" />
                </a>
                <% } %>
                <% Html.Telerik().Grid<HRIS.Domain.Personnel.Entities.Employee>("employees")
                                       .Name("Grid")
                                       .DataKeys(k => k.Add(o => o.Id))
                                       .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "Employee"))
                                       .Filterable()
                                       .Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                                       .Scrollable(builder => builder.Height(350))
                                       .Columns(c =>
                                       {
                                           c.Bound(b => b.Id).Width(35).Title(Resources.Shared.Model.Fields.Id).Groupable(false).Filterable(false).Sortable(false);
                                           c.Bound(b => b.FirstName);
                                           c.Bound(b => b.LastName);
                                           c.Bound(b => b.LoginName);
                                           c.Bound(b => b.DateOfBirth).Format("{0:MM/dd/yyyy}");
                                           c.Bound(o => o.Id).Title("")
                                    .Format(Html.ActionLink(Resources.Shared.Buttons.Function.ViewReport, "Index", "EmployeeReport", new { Area = "Reporting" }, new { target = "_blank" }).ToHtmlString())
                                    .Encoded(false).Width("5%").Sortable(false).Filterable(false);
                                           c.Command(s =>
                                           {
                                               //s.Select().ButtonType(GridButtonType.Image).HtmlAttributes(new { @class = "MasterGridSelect" });
                                               if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                               {
                                                   s.Delete().ButtonType(GridButtonType.Image);
                                               }
                                           }).Width(1);
                                       })
                            .DetailView(detailView => detailView.Template(e => // Set the server template
                            {
                                // Define a tabstrip 
                %>
                <% Html.Telerik().TabStrip()
                       .Name("TabStrip_" + e.Id)
                       .SelectedIndex(Session["SelectedTabIndex"] != null ? int.Parse(Session["SelectedTabIndex"].ToString()) : 0)
                       .Items(items =>
                        {

                            //Details
                            items.Add().Text(Resources.Areas.Personnel.Entities.Employee.EmployeeModel.PersonalPhoto).Content(() =>
                           {
                               
                %>
                <div id="dialog-form<%: e.Id %>" title="<%: Resources.Shared.Messages.General.UploadPhotoDialogTitle %>">
                    <% using (Html.BeginForm("Upload", "Employee", new { empID = e.Id }, FormMethod.Post, new { enctype = "multipart/form-data" }))
                       {%>
                    <p>
                        <input type="file" id="uploadImage" name="fileUpload" size="23" />
                    </p>
                    <p>
                        <input id="upload" type="submit" value="<%: Resources.Shared.Buttons.Function.UploadFile %>"
                            onclick="CloseDialog()" style="width: 100px" /></p>
                    <%
                       } %>
                </div>
                <table>
                    <tr>
                        <td style="width: 25%; vertical-align: middle">
                            <%if (System.IO.File.Exists(ViewData["Path"] + "//" + e.Id + ".jpg"))
                              { %>
                            <img src="<%= Url.Content("~/Areas/Personnel/Uploads/" + e.Id + ".jpg") %>" alt="<%= e.FirstName + " " + e.LastName
                                            %>" />
                            <% } %>
                            <%else
                              {%>
                            <img src="<%= Url.Content("~/Areas/Personnel/Uploads/no.jpg") %>" alt="No Image" />
                            <% } %>
                            <br />
                            <a href="#" onclick="ShowDialog(<%: e.Id %>)">
                                <%: Resources.Shared.Buttons.Function.ChangePhoto %></a> <a href="#" onclick="Delete(<%: e.Id %>)">
                                    <%: Resources.Shared.Buttons.Function.DeletePhoto %></a>
                        </td>
                    </tr>
                </table>
                <%
                           });
                            //Contacts
                            items.Add().Text(Resources.Areas.Personnel.Entities.Employee.EmployeeModel.Contacts).Content(() =>
                            {
                                
                %>
                <table>
                    <tr>
                        <td>

                            <% if (e.Contact.Count == 0)
                               { %>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="<%: Resources.Shared.Buttons.Function.Add %>" onclick="GridAddContact()"
                                src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>" title='<%: Resources.Shared.Buttons.Function.Add %>'
                                alt="<%: Resources.Shared.Buttons.Function.Add %>" />
                            <script type="text/javascript">
                                function GridAddContact() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "Contact") %>', function () {
                                        $('#result').fadeIn('fast');
                                    });
                                }
                            </script>
                            <% } %>
                            <% } %>                      
                        </td>
                    </tr>
                </table>
                <%:Html.Telerik().Grid(e.Contact)
                                         .Name("ContactGrid" + e.Id)
                                        .DataKeys(s => s.Add(x => x.Id))
                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "Contact")) 
                                        .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Title(Resources.Shared.Model.Fields.Id).Groupable(false).Filterable(false).Sortable(false);
                                            columns.Bound(o => o.FirstContact);
                                            columns.Bound(o => o.SecondContact);
                                            columns.Bound(o => o.Address);
                                            columns.Bound(o => o.Fax);
                                            columns.Bound(o => o.PrimaryEMail);
                                            
                                            columns.Command(s =>
                                            {
                                                if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                {
                                                    s.Delete().ButtonType(GridButtonType.Image);
                                                }
                                            }).Width(1);
                                        })
                                        .Selectable()
                                        .ClientEvents(events => events.OnRowSelect("contactServiceRowSelected"))                                                                          
                %>
                <%
                            });

                            //Passport  
                            items.Add().Text(Resources.Areas.Personnel.Entities.Employee.EmployeeModel.Passports).Content(() =>
                           {
                                
                %>
                <table>
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="GridAddPassport()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>"
                                title='Add' alt="Add" height="24" width="24" />
                            <script type="text/javascript">
                                function GridAddPassport() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "Passport") %>', function () {
                                        $('#result').fadeIn('fast');
                                    });
                                }
                            </script>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <%:Html.Telerik().Grid(e.Passports)
                                        .Name("Passports" + e.Id)
                                        .DataKeys(s => s.Add(x => x.Id))
                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "Passport")) 
                                        .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Title(Resources.Shared.Model.Fields.Id).Groupable(false).Filterable(false).Sortable(false);
                                            columns.Bound(o => o.Number);
                                            columns.Bound(o => o.FirstName);
                                            columns.Bound(o => o.LastName);
                                            columns.Bound(o => o.IssuanceDate);
                                            columns.Bound(o => o.ExpiryDate);
                                            columns.Command(s =>
                                            {
                                                if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                {
                                                    s.Delete().ButtonType(GridButtonType.Image);
                                                }
                                            }).Width(1);
                                        })
                                        .Selectable()
                                        .ClientEvents(events => events.OnRowSelect("passportRowSelected"))                                
                                        .Pageable(pager => pager.PageSize(3))
                                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                %>
                <%
                           });

                            //Residencies
                            items.Add().Text(Resources.Areas.Personnel.Entities.Employee.EmployeeModel.Residencies).Content(() =>
                            {
                                
                %>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="left">
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="GridAddResidency()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>"
                                title='Add' alt="Add" height="24" width="24" align="middle" />
                            <script type="text/javascript">
                                function GridAddResidency() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "Residency") %>', function () {
                                        $('#result').fadeIn('fast');
                                    });
                                }
                            </script>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <%:Html.Telerik().Grid(e.Residencies)
                                        .Name("Residency" + e.Id)
                                        .DataKeys(s => s.Add(x => x.Id))
                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "Residency")) 
                                        .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Title(Resources.Shared.Model.Fields.Id).Groupable(false).Filterable(false).Sortable(false);
                                            columns.Bound(o => o.No);
                                            columns.Bound(o => o.FirstName);
                                            columns.Bound(o => o.LastName);
                                            columns.Bound(o => o.IssuanceDate);
                                            columns.Bound(o => o.ExpiryDate);
                                            columns.Command(s =>
                                            {
                                                if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                {
                                                    s.Delete().ButtonType(GridButtonType.Image);
                                                }
                                            }).Width(1);
                                        })
                                        .Selectable()
                                        .ClientEvents(events => events.OnRowSelect("residencyRowSelected"))                                
                                        .Pageable(pager => pager.PageSize(3))
                                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                %>
                <%
                            });
                            //Driving License  

                            items.Add().Text(Resources.Areas.Personnel.Entities.Employee.EmployeeModel.DrivingLicense).Content(() =>
                            {
                                
                %>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="left">
                            <% if (e.DrivingLicense.Count == 0)
                               { %>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="GridAddDriving()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>"
                                title='Add' alt="Add" height="24" width="24" align="middle" />
                            <script type="text/javascript">
                                function GridAddDriving() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "DrivingLicense") %>', function () {
                                        $('#result').fadeIn('fast');
                                    });
                                }
                            </script>
                            <% } %>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <%:Html.Telerik().Grid(e.DrivingLicense)
                                        .Name("DrivingGrid" + e.Id)
                                        .DataKeys(s => s.Add(x => x.Id))
                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "DrivingLicense")) 
                                        .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Title(Resources.Shared.Model.Fields.Id).Groupable(false).Filterable(false).Sortable(false);
                                            columns.Bound(o => o.Number);
                                            columns.Bound(o => o.Type.Name).Title(Resources.Areas.Personnel.ValueObjects.DrivingLicense.DrivingLicenseModel.Type) ;
                                            columns.Bound(o => o.PlaceOfIssuance.Name).Title(Resources.Areas.Personnel.ValueObjects.DrivingLicense.DrivingLicenseModel.PlaceOfIssuance);
                                            columns.Bound(o => o.IssuanceDate);
                                            columns.Bound(o => o.ExpiryDate);
                                            
                                            columns.Command(s =>
                                            {
                                                if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                {
                                                    s.Delete().ButtonType(GridButtonType.Image);
                                                }
                                            }).Width(1);
                                        })
                                        .Selectable()
                                        .ClientEvents(events => events.OnRowSelect("drivingRowSelected"))                                                                          
                %>
                <%
                            });
                            // Military Service
                            items.Add().Text(Resources.Areas.Personnel.Entities.Employee.EmployeeModel.MilitaryService).Content(() =>
                            {
                                
                %>
                <table>
                    <tr>
                        <td align="left">
                            <% if (e.MilitaryService.Count == 0)
                               { %>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="GridAddMilitary()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>"
                                title='Add' alt="Add" height="24" width="24" align="middle" />
                            <script type="text/javascript">
                                function GridAddMilitary() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "MilitaryService") %>', function () {
                                        $('#result').fadeIn('fast');
                                    });
                                }
                            </script>
                            <% } %>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <%:Html.Telerik().Grid(e.MilitaryService)
                                        .Name("MilitaryGrid" + e.Id)
                                        .DataKeys(s => s.Add(x => x.Id))
                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "MilitaryService")) 
                                        .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Title(Resources.Shared.Model.Fields.Id).Groupable(false).Filterable(false).Sortable(false);
                                            columns.Bound(o => o.MilitiryServiceNo);
                                            columns.Bound(o => o.ServiceStartDate);
                                            columns.Bound(o => o.Months);
                                            columns.Bound(o => o.Days);
                                            columns.Command(s =>
                                            {
                                                if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                {
                                                    s.Delete().ButtonType(GridButtonType.Image);
                                                }
                                            }).Width(1);
                                        })
                                        .Selectable()
                                        .ClientEvents(events => events.OnRowSelect("militaryServiceRowSelected"))                                                                          
                %>
                <%
                            });



                            //Spouse    
                            items.Add().Text(Resources.Areas.Personnel.Entities.Employee.EmployeeModel.Spouse).Content(() =>
                            {
                                
                %>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="left">
                            <% if (e.Spouse.Count == 0)
                               { %>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="GridAddSpouse()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>"
                                title='Add' alt="Add" height="24" width="24" align="middle" />
                            <script type="text/javascript">
                                function GridAddSpouse() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "Spouse") %>', function () {
                                        $('#result').fadeIn('fast');
                                    });
                                }
                            </script>
                            <% } %>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <%:Html.Telerik().Grid(e.Spouse)
                                        .Name("SpouseGrid" + e.Id)
                                        .DataKeys(s => s.Add(x => x.Id))
                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "Spouse")) 
                                        .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Title(Resources.Shared.Model.Fields.Id).Groupable(false).Filterable(false).Sortable(false);
                                            columns.Bound(o => o.FirstName);
                                            columns.Bound(o => o.LastName);
                                            columns.Bound(o => o.PlaceOfBirth.Name).Title(Resources.Areas.Personnel.ValueObjects.Spouse.SpouseModel.PlaceOfBirth);
                                            columns.Bound(o => o.DateOfBirth);
                                            columns.Bound(o => o.Nationality.Name).Title(Resources.Areas.Personnel.ValueObjects.Spouse.SpouseModel.Nationality);
                                            columns.Command(s =>
                                            {
                                                if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                {
                                                    s.Delete().ButtonType(GridButtonType.Image);
                                                }
                                            }).Width(1);
                                        })
                                        .Selectable()
                                        .ClientEvents(events => events.OnRowSelect("spouseRowSelected"))
                %>
                <%
                            });
                            //End Spouse 

                            //Children    
                            items.Add().Text(Resources.Areas.Personnel.Entities.Employee.EmployeeModel.Children).Content(() =>
                                        {%>
                <table>
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="GridAddChild()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>"
                                title='Add' alt="Add" height="24" width="24" />
                            <script type="text/javascript">
                                function GridAddChild() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "Children") %>', function () {
                                        $('#result').fadeIn('fast');
                                    });
                                }
                            </script>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <%:Html.Telerik().Grid(e.Children)
                                        .Name("Children" + e.Id)
                                        .DataKeys(s => s.Add(x => x.Id))
                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "Children"))    
                                        .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Title(Resources.Shared.Model.Fields.Id).Groupable(false).Filterable(false).Sortable(false);
                                            columns.Bound(o => o.FirstName);
                                            columns.Bound(o => o.LastName);
                                            columns.Bound(o => o.Nationality.Name).Title(Resources.Areas.Personnel.ValueObjects.Child.ChildModel.Nationality);
                                            columns.Bound(o => o.PlaceOfBirth.Name).Title(Resources.Areas.Personnel.ValueObjects.Child.ChildModel.PlaceOfBirth);
                                            columns.Bound(o => o.DateOfBirth);
                                            columns.Bound(o => o.ResidencyNo);
                                            columns.Bound(o => o.ResidencyExpiryDate);
                                            columns.Command(s =>
                                                                {
                                                                    if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                                    {
                                                                        s.Delete().ButtonType(GridButtonType.Image);
                                                                    }
                                                                }).Width(1);
                                        })
                                        .ClientEvents(events => events.OnRowSelect("childrenRowSelected"))  
                                        .Selectable()                                  
                                        .Pageable(pager => pager.PageSize(3))
                                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                %>
                <%});
                            //End Children 


                            //Dependent    
                            items.Add().Text(Resources.Areas.Personnel.Entities.Employee.EmployeeModel.Dependents).Content(() =>
                                        {%>
                <table>
                    <tr>
                        <td align="left">
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="GridAddDependent()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>"
                                title='Add' alt="Add" height="24" width="24" align="middle" />
                            <script type="text/javascript">
                                function GridAddDependent() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "Dependent") %>', function () {
                                        $('#result').fadeIn('fast');
                                    });
                                }
                            </script>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <%:Html.Telerik().Grid(e.Dependents)
                                        .Name("Dependents" + e.Id)
                                        .DataKeys(s => s.Add(x => x.Id))
                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "Dependent"))    
                                        .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Title(Resources.Shared.Model.Fields.Id).Groupable(false).Filterable(false).Sortable(false);
                                            columns.Bound(o => o.FirstName);
                                            columns.Bound(o => o.LastName);
                                            columns.Bound(o => o.PlaceOfBirth.Name).Title(Resources.Areas.Personnel.ValueObjects.Dependent.DependentModel.PlaceOfBirth);
                                            columns.Bound(o => o.DateOfBirth);
                                            columns.Bound(o => o.Nationality.Name).Title(Resources.Areas.Personnel.ValueObjects.Dependent.DependentModel.Nationality);
                                            columns.Bound(o => o.ContactNumber);
                                            columns.Command(s =>
                                                                {
                                                                    if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                                    {
                                                                        s.Delete().ButtonType(GridButtonType.Image);
                                                                    }
                                                                }).Width(1);
                                        })
                                        .ClientEvents(events => events.OnRowSelect("dependentRowSelected"))  
                                        .Selectable()                                  
                                        .Pageable(pager => pager.PageSize(3))
                                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                                                                             
                %>
                <%});

                            //Education    
                            items.Add().Text(Resources.Areas.Personnel.Entities.Employee.EmployeeModel.Education).Content(() =>
                                        {%>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="left">
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="GridAddEducation()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>"
                                title='Add' alt="Add" height="24" width="24" align="middle" />
                            <script type="text/javascript">
                                function GridAddEducation() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "Education") %>', function () {
                                        $('#result').fadeIn('fast');
                                    });
                                }
                            </script>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <%:Html.Telerik().Grid(e.Educations)
                                        .Name("Educations" + e.Id)
                                        .DataKeys(s => s.Add(x => x.Id))
                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "Education"))    
                                        .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Title(Resources.Shared.Model.Fields.Id).Groupable(false).Filterable(false).Sortable(false);
                                            columns.Bound(o => o.Type.Name).Title(Resources.Areas.Personnel.ValueObjects.Education.EducationModel.Type);
                                            columns.Bound(o => o.Major.Name).Title(Resources.Areas.Personnel.ValueObjects.Education.EducationModel.Major);
                                            columns.Bound(o => o.ScoreType.Name).Title(Resources.Areas.Personnel.ValueObjects.Education.EducationModel.ScoreType);
                                            columns.Bound(o => o.Score);
                                            columns.Bound(o => o.Rank.Name).Title(Resources.Areas.Personnel.ValueObjects.Education.EducationModel.Rank);
                                            columns.Bound(o => o.University);
                                            columns.Command(s =>
                                                                {
                                                                    if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                                    {
                                                                        s.Delete().ButtonType(GridButtonType.Image);
                                                                    }
                                                                }).Width(1);
                                        })
                                        .ClientEvents(events => events.OnRowSelect("educationRowSelected"))  
                                        .Selectable()                                  
                                        .Pageable(pager => pager.PageSize(3))
                                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                                                                             
                %>
                <%});

                            //Certification   
                            items.Add().Text(Resources.Areas.Personnel.Entities.Employee.EmployeeModel.Certifications).Content(() =>
                                        {%>
                <table>
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="GridAddCertification()" src="<%: Url.Content("~/Content/Ribbon/Icons/48/112.png") %>"
                                title='Add' alt="Add" height="24" width="24" />
                            <script type="text/javascript">
                                function GridAddCertification() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "Certification") %>', function () {
                                        $('#result').fadeIn('fast');
                                    });
                                }
                            </script>
                            <% } %>
                            <%--<% if (TempData["ExpiredRules"] != null && ((IList<Infrastructure.Validation.BrokenBusinessRule>)TempData["ExpiredRules"]).Count > 0)
                                               {%>
                                            <input type="image" value="Add" onclick="CertificationExpiredRules()" src="<%: Url.Content("~/Content/Ribbon/Icons/48/25.png") %>"
                                                title='Add' alt="Add" height="24" width="24" />
                                            <script type="text/javascript">
                                                function CertificationExpiredRules() {
                                                    $('#result').fadeOut('fast');

                                                    $('#result').load('<%: Url.Action("CertificationExpiredRules", "Certification") %>', function () {
                                                        $('#result').fadeIn('fast');
                                                    });
                                                }
                                            </script>
                                            <% } %>--%>
                        </td>
                    </tr>
                </table>
                <%:Html.Telerik().Grid(e.Certifications)
                                        .Name("Certifications" + e.Id)
                                        .DataKeys(s => s.Add(x => x.Id))
                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "Certification"))    
                                        .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Title(Resources.Shared.Model.Fields.Id).Groupable(false).Filterable(false).Sortable(false);
                                            columns.Bound(o => o.Type.Name).Width(1);
                                            columns.Bound(o => o.Status.Name).Title(Resources.Areas.Personnel.ValueObjects.Certification.CertificationModel.Status);
                                            columns.Bound(o => o.DateOfIssuance);
                                            columns.Bound(o => o.ExpirationDate);
                                            columns.Bound(o => o.PlaceOfIssuance.Name).Title(Resources.Areas.Personnel.ValueObjects.Certification.CertificationModel.PlaceOfIssuance).Width(1);
                                            columns.Command(s =>
                                                                {
                                                                    if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                                    {
                                                                        s.Delete().ButtonType(GridButtonType.Image);
                                                                    }
                                                                }).Width(1);
                                        })
                                        .ClientEvents(events => events.OnRowSelect("certificationRowSelected"))  
                                        .Selectable()                                  
                                        .Pageable(pager => pager.PageSize(3))
                                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                                                                             
                %>
                <%});

                            //Experiences    
                            items.Add().Text(Resources.Areas.Personnel.Entities.Employee.EmployeeModel.Experiences).Content(() =>
                                        {
                %>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="left">
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="GridAddExperience()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>"
                                title='Add' alt="Add" height="24" width="24" align="middle" />
                            <script type="text/javascript">
                                function GridAddExperience() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "Experience") %>', function () {
                                        $('#result').fadeIn('fast');
                                    });
                                }
                            </script>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <%:Html.Telerik().Grid(e.Experiences)
                                         .Name("Experience" + e.Id)
                                        .DataKeys(s => s.Add(x => x.Id))
                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "Experience"))    
                                        .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Title(Resources.Shared.Model.Fields.Id).Groupable(false).Filterable(false).Sortable(false);
                                            columns.Bound(o => o.JobTitle);
                                            columns.Bound(o => o.CompanyName);
                                            columns.Bound(o => o.CompanyLocation);
                                            columns.Bound(o => o.StartDate);
                                            columns.Bound(o => o.EndDate);
                                            columns.Bound(o => o.LeaveReason);
                                            columns.Command(s =>
                                                                {
                                                                    if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                                    {
                                                                        s.Delete().ButtonType(GridButtonType.Image);
                                                                    }
                                                                }).Width(1);
                                        })
                                        .ClientEvents(events => events.OnRowSelect("experienceRowSelected"))  
                                        .Selectable()                                  
                                        .Pageable(pager => pager.PageSize(3))
                                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                                                                             
                %>
                <%                              
                                        });
                            //End Experiences 



                            //Training    
                            items.Add().Text(Resources.Areas.Personnel.Entities.Employee.EmployeeModel.TrainingSkills).Content(() =>
                                        {%>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="left">
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="GridAddTraining()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>"
                                title='Add' alt="Add" height="24" width="24" align="middle" />
                            <script type="text/javascript">
                                function GridAddTraining() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "Training") %>', function () {
                                        $('#result').fadeIn('fast');
                                    });
                                }
                            </script>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <%:Html.Telerik().Grid(e.Trainings)
                                        .Name("Trainings" + e.Id)
                                        .DataKeys(s => s.Add(x => x.Id))
                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "Training"))    
                                        .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Title(Resources.Shared.Model.Fields.Id).Groupable(false).Filterable(false).Sortable(false);
                                            columns.Bound(o => o.CourseName);
                                            columns.Bound(o => o.CourseDuration);
                                            columns.Bound(o => o.CertificateIssuanceDate);
                                            columns.Bound(o => o.Status.Name).Title(Resources.Areas.Personnel.ValueObjects.Training.TrainingModel.Status); ;
                                            columns.Bound(o => o.TrainingCenter);
                                            columns.Bound(o => o.TrainingCenterLocation);
                                            columns.Command(s =>
                                                                {
                                                                    if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                                    {
                                                                        s.Delete().ButtonType(GridButtonType.Image);
                                                                    }
                                                                }).Width(1);
                                        })
                                        .ClientEvents(events => events.OnRowSelect("trainingRowSelected"))  
                                        .Selectable()                                  
                                        .Pageable(pager => pager.PageSize(3))
                                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                                                                             
                %>
                <%});
                            //End Training 

                            //Language    
                            items.Add().Text(Resources.Areas.Personnel.Entities.Employee.EmployeeModel.Languages).Content(() =>
                                        {%>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="left">
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="GridAddLanguage()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>"
                                title='Add' alt="Add" height="24" width="24" align="middle" />
                            <script type="text/javascript">
                                function GridAddLanguage() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "Language") %>', function () {
                                        $('#result').fadeIn('fast');
                                    });
                                }
                            </script>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <%:Html.Telerik().Grid(e.Languages)
                                        .Name("Languages" + e.Id)
                                        .DataKeys(s => s.Add(x => x.Id))
                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "Language"))    
                                        .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Title(Resources.Shared.Model.Fields.Id).Groupable(false).Filterable(false).Sortable(false);
                                            columns.Bound(o => o.Name).Title(
                                                Resources.Areas.Personnel.ValueObjects.Language.LanguageModel.Name);
                                            columns.Bound(o => o.Reading.Name).Title(Resources.Areas.Personnel.ValueObjects.Language.LanguageModel.Reading);
                                            columns.Bound(o => o.Writing.Name).Title(
                                                Resources.Areas.Personnel.ValueObjects.Language.LanguageModel.Writing);
                                            columns.Bound(o => o.Listening.Name).Title(
                                                Resources.Areas.Personnel.ValueObjects.Language.LanguageModel.Listening);
                                            columns.Bound(o => o.Speaking.Name).Title(
                                                Resources.Areas.Personnel.ValueObjects.Language.LanguageModel.Speaking);
                                            columns.Command(s =>
                                                                {
                                                                    if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                                    {
                                                                        s.Delete().ButtonType(GridButtonType.Image);
                                                                    }
                                                                }).Width(1);
                                        })
                                        .ClientEvents(events => events.OnRowSelect("languageRowSelected"))  
                                        .Selectable()                                  
                                        .Pageable(pager => pager.PageSize(3))
                                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                                                                             
                %>
                <%});
                            //End Language 

                            //Skill    
                            items.Add().Text(Resources.Areas.Personnel.Entities.Employee.EmployeeModel.Skills).Content(() =>
                                        {%>
                <table>
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="GridAddSkill()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>"
                                title='Add' alt="Add" height="24" width="24" align="middle" />
                            <script type="text/javascript">
                                function GridAddSkill() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "Skill") %>', function () {
                                        $('#result').fadeIn('fast');
                                    });
                                }
                            </script>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <%:Html.Telerik().Grid(e.Skills)
                                        .Name("Skills" + e.Id)
                                        .DataKeys(s => s.Add(x => x.Id))
                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "Skill"))    
                                        .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Title(Resources.Shared.Model.Fields.Id).Groupable(false).Filterable(false).Sortable(false);
                                            columns.Bound(o => o.Name.Name).Title(
                                                Resources.Areas.Personnel.ValueObjects.Skill.SkillModel.Name);
                                            columns.Bound(o => o.Level.Name).Title(
                                                Resources.Areas.Personnel.ValueObjects.Skill.SkillModel.Level); ;
                                            columns.Command(s =>
                                                                {
                                                                    if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                                    {
                                                                        s.Delete().ButtonType(GridButtonType.Image);
                                                                    }
                                                                }).Width(1);
                                        })
                                        .ClientEvents(events => events.OnRowSelect("skillRowSelected"))  
                                        .Selectable()                                  
                                        .Pageable(pager => pager.PageSize(3))
                                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                                                                             
                %>
                <%});
                            //End Skill 

                            //Conviction    
                            items.Add().Text(Resources.Areas.Personnel.Entities.Employee.EmployeeModel.Convictions).Content(() =>
                                        {%>
                <table>
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="GridAddConviction()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>"
                                title='Add' alt="Add" height="24" width="24" />
                            <script type="text/javascript">
                                function GridAddConviction() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "Conviction") %>', function () {
                                        $('#result').fadeIn('fast');
                                    });
                                }
                            </script>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <%:Html.Telerik().Grid(e.Convictions)
                                        .Name("Convictions" + e.Id)
                                        .DataKeys(s => s.Add(x => x.Id))
                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "Conviction"))    
                                        .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Title(Resources.Shared.Model.Fields.Id).Groupable(false).Filterable(false).Sortable(false);
                                            columns.Bound(o => o.Number);
                                            columns.Bound(o => o.Rule.Name).Title(
                                                Resources.Areas.Personnel.ValueObjects.Conviction.ConvictionModel.Rule); ;
                                            columns.Bound(o => o.ConvictionDate);
                                            columns.Bound(o => o.ReleaseDate);
                                            columns.Bound(o => o.Reason);
                                            columns.Command(s =>
                                                                {
                                                                    if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                                    {
                                                                        s.Delete().ButtonType(GridButtonType.Image);
                                                                    }
                                                                }).Width(1);
                                        })
                                        .ClientEvents(events => events.OnRowSelect("convictionRowSelected"))  
                                        .Selectable()                                  
                                        .Pageable(pager => pager.PageSize(3))
                                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                                                                             
                %>
                <%});
                            //End Conviction


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

        $('#result').fadeOut('fast');

        $.ajax({
            url: '<%: Url.Action("SaveTabIndex", "Employee")%>/', type: "POST",
            data: { selectedIndex: item.index() },
            success: function () {
            }
        });
    }

    function SetMasterRecordValue(e) {

        var x = e.masterRow.cells[1].innerHTML;

        $('#result').load('<%: Url.Action("PartialMasterInfo", "Employee") %>', { selectedRowId: x }, function () {
            $('#result').fadeIn('fast');

            loadRibbon();
        });
    }

    function loadPartialView(e) {

        $('> .t-hierarchy-cell > .t-icon', e.row).click();

        $('#result').fadeOut('fast');
    }

    function loadRibbon() {
        $('#PersonnelFunctionsArea').load('<%: Url.Action("GetFunctionsPartial", "Personnel") %>');
    }


    function childrenRowSelected(e) {

        $('#result').fadeOut('fast');

        var x = e.row.cells[0].innerHTML;

        var url = '<%: Url.Action("Index", "Children", new { selectedSubRowId = "Value1"}) %>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('fast');
        });
    }

    function dependentRowSelected(e) {

        $('#result').fadeOut('fast');

        var x = e.row.cells[0].innerHTML;

        var url = '<%: Url.Action("Index", "Dependent", new { selectedSubRowId = "Value1"}) %>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('fast');
        });
    }

    function educationRowSelected(e) {

        $('#result').fadeOut('fast');

        var x = e.row.cells[0].innerHTML;

        var url = '<%: Url.Action("Index", "Education", new { selectedSubRowId = "Value1"}) %>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('fast');
        });
    }


    function experienceRowSelected(e) {

        $('#result').fadeOut('fast');

        var x = e.row.cells[0].innerHTML;

        var url = '<%: Url.Action("Index", "Experience", new { selectedSubRowId = "Value1"}) %>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('fast');
        });
    }

    function certificationRowSelected(e) {

        $('#result').fadeOut('fast');

        var x = e.row.cells[0].innerHTML;

        var url = '<%: Url.Action("Index", "Certification", new { selectedSubRowId = "Value1"}) %>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('fast');
        });
    }


    function convictionRowSelected(e) {

        $('#result').fadeOut('fast');

        var x = e.row.cells[0].innerHTML;

        var url = '<%: Url.Action("Index", "Conviction", new { selectedSubRowId = "Value1"}) %>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('fast');
        });
    }

    function languageRowSelected(e) {

        $('#result').fadeOut('fast');

        var x = e.row.cells[0].innerHTML;

        var url = '<%: Url.Action("Index", "Language", new { selectedSubRowId = "Value1"}) %>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('fast');
        });
    }

    function skillRowSelected(e) {

        $('#result').fadeOut('fast');

        var x = e.row.cells[0].innerHTML;

        var url = '<%: Url.Action("Index", "Skill", new { selectedSubRowId = "Value1"}) %>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('fast');
        });
    }

    function trainingRowSelected(e) {

        $('#result').fadeOut('fast');

        var x = e.row.cells[0].innerHTML;

        var url = '<%: Url.Action("Index", "Training", new { selectedSubRowId = "Value1"}) %>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('fast');
        });
    }

    function passportRowSelected(e) {

        $('#result').fadeOut('fast');

        var x = e.row.cells[0].innerHTML;

        var url = '<%: Url.Action("Index", "Passport", new { selectedSubRowId = "Value1"}) %>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('fast');
        });
    }

    function residencyRowSelected(e) {

        $('#result').fadeOut('fast');

        var x = e.row.cells[0].innerHTML;

        var url = '<%: Url.Action("Index", "Residency", new { selectedSubRowId = "Value1"}) %>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('fast');
        });
    }

    function militaryServiceRowSelected(e) {

        $('#result').fadeOut('fast');

        var x = e.row.cells[0].innerHTML;

        var url = '<%: Url.Action("Index", "MilitaryService", new { selectedSubRowId = "Value1"}) %>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('fast');
        });
    }

    function contactServiceRowSelected(e) {

        $('#result').fadeOut('fast');

        var x = e.row.cells[0].innerHTML;

        var url = '<%: Url.Action("Index", "Contact", new { selectedSubRowId = "Value1"}) %>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('fast');
        });
    }

    function spouseRowSelected(e) {

        $('#result').fadeOut('fast');

        var x = e.row.cells[0].innerHTML;

        var url = '<%: Url.Action("Index", "Spouse", new { selectedSubRowId = "Value1"}) %>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('fast');
        });
    }

    function drivingRowSelected(e) {

        $('#result').fadeOut('fast');

        var x = e.row.cells[0].innerHTML;

        var url = '<%: Url.Action("Index", "DrivingLicense", new { selectedSubRowId = "Value1"}) %>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('fast');
        });
    }


    $(document).ready(function () {
        var translations = {};
        translations["cancel"] = "<%: Resources.Shared.Buttons.Function.Cancel %>";
        var buttonsOpts = {};
        buttonsOpts[translations["cancel"]] = function () {
            $(this).dialog('close');
        };


        $("div").each(function (i) {
            var subSection = $(this).attr('id').substring(6, 0);
            if (subSection == 'dialog') {
                $(this).dialog({
                    bgiframe: true,
                    height: 200,
                    modal: true,
                    autoOpen: false,
                    resizable: false,
                    buttons: buttonsOpts
                });
            }
        });

        $('#upload').bind("click", function (e) {
            var imgVal = $('#uploadImage').val();
            if (imgVal == '') {
                alert("<%: Resources.Shared.Messages.General.SelectImageMessage %>");
                e.preventDefault();
            }
            else {
                var extension = imgVal.substr(imgVal.lastIndexOf('.') + 1, 3);
                if ((extension != "jpg") && (extension != "png")) {
                    alert("<%: Resources.Shared.Messages.General.ImageTypeMessage %>");
                    e.preventDefault();
                }
            }
        });
    });

    function ShowDialog(empID) {
        var id = empID;
        $("subBtn").click(
                $("#dialog-form" + id).dialog('open'))

    };
    function CloseDialog() {

        $('#dialog-form').dialog('close');
    }

    function Delete(empID) {
        if (confirm("<%: Resources.Shared.Messages.General.DeleteConfirm %>")) {
            var Id = empID;

            $.ajax({
                url: '<%= Url.Action("DeletePhoto","Employee") %>/', type: "POST",
                data: { empId: Id },
                success: function (result) {
                    if (result.Success != null) {
                        if (result.Success == false)
                            alert(result.Message);
                        else {
                            location.reload();
                        }
                    }
                }
            });
        }
    }
</script>
