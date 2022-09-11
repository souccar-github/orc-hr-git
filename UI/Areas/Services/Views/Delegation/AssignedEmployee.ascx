<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<UI.Areas.Services.DTO.ViewModels.AssignedEmployeeViewModel>>" %>
<div style="height: 200px; overflow: auto">
    <% for (int i = 0; i < Model.Count; i++)
       {%>
    <div id="divEmployeeInfo_<%:Model[i].Employee.Id %>">
        <table style="border: 1px solid black;">
            <tr style="border: 1px solid black;">
                <td colspan="2" style="border: 1px solid black; padding: 3px;">
                    <%:Html.DisplayFor(x=>Model[i].Employee.FullName) %>
                </td>
            </tr>
            <tr style="border: 1px solid black;">
                <td style="border: 1px solid black; padding: 3px;">
                    <%: Resources.Areas.Services.Delegation.Messages.RolesColumnTitle%>
                </td>
                <td style="border: 1px solid black; padding: 3px;">
                    <%: Resources.Areas.Services.Delegation.Messages.AuthoritiesColumnTitle%>
                </td>
            </tr>
            <tr style="border: 1px solid black; padding: 3px;">
                <td style="border: 1px solid black; padding: 3px;">
                    <div id="AssignedRoles_<%:Model[i].Employee.Id%>">
                        <% for (int z = 0; z < Model[i].Roles.Count; z++)
                           {%>
                        <div id="AssignedRole_<%:Model[i].Employee.Id%>_<%:Model[i].Roles[z].Id %>">
                            <%:Html.DisplayFor(x=>Model[i].Roles[z].Name) %>
                            <input type="button" value="X" onclick="DeleteAssignedRole(<%:Model[i].Employee.Id%>,<%:Model[i].Roles[z].Id %>, 
                                                                        'AssignedRole_<%:Model[i].Employee.Id%>_<%:Model[i].Roles[z].Id %>')" />
                        </div>
                        <%}%>
                    </div>
                </td>
                <td style="border: 1px solid black; padding: 3px;">
                    <div id="AssignedAuthorities_<%:Model[i].Employee.Id%>">
                        <% for (int z = 0; z < Model[i].Authorities.Count; z++)
                           {%>
                        <div id="AssignedAuthority_<%:Model[i].Employee.Id%>_<%:Model[i].Authorities[z].Id %>">
                            <%:Html.DisplayFor(x=>Model[i].Authorities[z].Name) %>
                            <input type="button" value="X" onclick="DeleteAssignedAuthority(<%:Model[i].Employee.Id%>,<%:Model[i].Authorities[z].Id %>, 
                                                                        'AssignedAuthority_<%:Model[i].Employee.Id%>_<%:Model[i].Authorities[z].Id %>')" />
                        </div>
                        <%}%>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <%}%>
</div>
