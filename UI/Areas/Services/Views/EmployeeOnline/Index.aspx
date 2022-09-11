<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Services/Views/Shared/Services.master"
    Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="HRIS.Domain.Personnel.Entities" %>
<%@ Import Namespace="HRIS.Domain.Personnel.ValueObjects" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%--<%= Html.Telerik().Grid<Employee>()--%>
    <%= Html.Telerik().Grid<Employee>()
    
        .Name("Employees")
        .DataKeys(k => k.Add(e => e.Id))
       // .Editable(editing => editing.Mode(GridEditMode.InCell))

        .Columns(columns =>
        {
            columns.Bound(e => e.FirstName).Width(140);
            columns.Bound(e => e.LastName).Width(140);
            columns.Bound(e => e.MiddleName).Width(140);
            columns.Bound(e => e.FatherName).Width(140);
            columns.Bound(e => e.MotherName).Width(140);
            columns.Bound(e => e.DateOfBirth).Format("{0:dd/MM/yyyy}").Width(140);
            columns.Bound(e => e.Nationality.Name).Width(140).EditorTemplateName("Nationalties");

            //columns.ForeignKey(e => e.Nationality.Id, (IEnumerable)ViewData["nationalties"], "Id", "Name").Width(140).
            //    Title("Nationality").Sortable(false).Filterable(false);
            columns.Command(s =>
            {
                s.Edit().ButtonType(GridButtonType.Image);
                s.Delete().ButtonType(GridButtonType.Image);
            }).Width("15%");
            
        })
        .ClientEvents(events => events
            .OnRowDataBound("employees_onRowDataBound")
            .OnEdit("onEdit")
            .OnSave("onSave"))
        

        .DetailView(details => details.ClientTemplate(
                        Html.Telerik().Grid<Contact>()
                            .Name("Contacts_<#= Id #>")
                                    .DataKeys(k => k.Add(o => o.Id))
                                    .ToolBar(t => t.Insert())
                    .Columns(columns =>
                    {
                        columns.Bound(o => o.FirstContact).Width(160);
                        columns.Bound(o => o.SecondContact).Width(101);
                        columns.Bound(o => o.Fax).Width(101);
                        columns.Bound(o => o.PrimaryEMail).Width(101);
                        columns.Bound(o => o.SecondaryEMail).Width(140);
                        columns.Bound(o => o.Address).Width(101);
                        columns.Bound(o => o.WebSite).Width(140);
                        columns.Bound(o => o.POBox).Width(101);
                        columns.Bound(o => o.Twitter).Width(101);
                        columns.Bound(o => o.Facebook).Width(101);
                        columns.Command(s =>
                        {
                            s.Edit().ButtonType(GridButtonType.Image);
                            s.Delete().ButtonType(GridButtonType.Image);
                        }).Width("15%");
                    })
                    .ClientEvents(events => events.OnRowDataBound("contacts_onRowDataBound"))
                            .DataBinding(d =>
                            {
                                d.Ajax().Select("ReadContacts", "ContactOnline", new { employeeId = "<#= Id #>" });
                                d.Ajax().Update("UpdateContact", "ContactOnline");
                                d.Ajax().Delete("DeleteContact", "ContactOnline");
                                d.Ajax().Insert("InsertContact", "ContactOnline");
                            })
                    .Pageable()
                    .Sortable()
                    .Filterable()
                    .ToHtmlString()
        ))
                .DataBinding(d =>
                    {
                        d.Ajax().Select("ReadEmployees", "EmployeeOnline");
                        d.Ajax().Update("UpdateEmployee", "EmployeeOnline");
                        d.Ajax().Delete("DeleteEmployee", "EmployeeOnline");
                                //d.Ajax().Insert("InsertEmployee", "EmployeeContact");
                            })
        .Pageable(paging => paging.PageSize(15))
        .Scrollable(scrolling => scrolling.Height(580))
        .Sortable()
        .Filterable()
        .KeyboardNavigation()
    %>
    <script type="text/javascript">

        function expandFirstRow(grid, row) {
            if (grid.$rows().index(row) == 0) {
                //grid.expandRow(row);
                //grid.editRow(row);
            }
        }

        function employees_onRowDataBound(e) {
            var grid = $(this).data('tGrid');
           // grid.editRow(e.row);
            expandFirstRow(grid, e.row);
        }

        function contacts_onRowDataBound(e) {
            var grid = $(this).data('tGrid');
            expandFirstRow(grid, e.row);
        }
        function onEdit(e) {
           
            $(e.form).find('#Nationalties').data('tDropDownList').select(function (dataItem) {
               // alert(e.dataItem['Nationality']);
                return dataItem.Value == e.dataItem['Nationality'].Id;
            });
        }
        function onSave(e) {
//            var grid = $(e).data('tGrid');
//            var hasChanges = grid.hasChanges();
//            alert(hasChanges);
        }
//        function onSelect(e) {
//            var grid = $(this).data('tGrid');
//             grid.editRow(e.row);
//           
//        }
        function dump(obj) {
            obj = obj || {};
            var result = [];
            $.each(obj, function (key, value) {
                result.push('"' + key + '":"' + value +
           '"');
            });
            return '{' + result.join(',') + '}';
        }

        
    </script>
</asp:Content>
