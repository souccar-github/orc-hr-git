<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Personnel.Entities.Employee>" %>
<%@ Import Namespace="UI.Helpers.Views" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend"></legend>
    <table width="100%" style="vertical-align: middle">
        <tr>
            <td style="width: 50%; vertical-align: top" align="right" colspan="3">
                <%
                    if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
                    {%>
                <input type="button" value="<%: Resources.Shared.Buttons.Function.Edit %>" onclick="ShowEditUserControl();" />
                <%
                    }%>
                <script type="text/javascript">
                    function ShowEditUserControl() {
                        $('#result').load('<%:Url.Action("Edit", "Employee", new {id = Model.Id})%>');
                    }
                </script>
            </td>
        </tr>
        <tr>
            <td style="width: 33%; vertical-align: top">
                <fieldset class="EditFieldset">
                    <legend><%: Resources.Areas.Personnel.Entities.Employee.EmployeeModel.NameInFirstLanguage %></legend>
                    <div class="display-label">
                        <%:Html.LabelFor(model => model.FirstName)%>
                    </div>
                    <div class="display-field">
                        <%:Html.TextBoxFor(model => model.FirstName, new ReadOnlyTextBox(true, "SingleLine"))%>
                    </div>
                    <div class="display-label">
                        <%:Html.LabelFor(model => model.FatherName)%>
                    </div>
                    <div class="display-field">
                        <%:Html.TextBoxFor(model => model.FatherName, new ReadOnlyTextBox(true, "SingleLine"))%>
                    </div>
                    <div class="display-label">
                        <%:Html.LabelFor(model => model.MiddleName)%>
                    </div>
                    <div class="display-field">
                        <%:Html.TextBoxFor(model => model.MiddleName, new ReadOnlyTextBox(true, "SingleLine"))%>
                    </div>
                    <div class="display-label">
                        <%:Html.LabelFor(model => model.LastName)%>
                    </div>
                    <div class="display-field">
                        <%:Html.TextBoxFor(model => model.LastName, new ReadOnlyTextBox(true, "SingleLine"))%>
                    </div>
                    <div class="display-label">
                        <%:Html.LabelFor(model => model.MotherName)%>
                    </div>
                    <div class="display-field">
                        <%:Html.TextBoxFor(model => model.MotherName, new ReadOnlyTextBox(true, "SingleLine"))%>
                    </div>
                </fieldset>
            </td>
            <td style="width: 33%; vertical-align: top;">
                <fieldset class="EditFieldset">
                    <legend><%: Resources.Areas.Personnel.Entities.Employee.EmployeeModel.NameInSecondLanguage %></legend>
                    <div class="display-label">
                        <%:Html.LabelFor(model => model.FirstNameL2)%>
                    </div>
                    <div class="display-field">
                        <%:Html.TextBoxFor(model => model.FirstNameL2, new ReadOnlyTextBox(true, "SingleLine"))%>
                    </div>
                    <div class="display-label">
                        <%:Html.LabelFor(model => model.FatherNameL2)%>
                    </div>
                    <div class="display-field">
                        <%:Html.TextBoxFor(model => model.FatherNameL2, new ReadOnlyTextBox(true, "SingleLine"))%>
                    </div>
                    <div class="display-label">
                        <%:Html.LabelFor(model => model.MiddleNameL2)%>
                    </div>
                    <div class="display-field">
                        <%:Html.TextBoxFor(model => model.MiddleNameL2, new ReadOnlyTextBox(true, "SingleLine"))%>
                    </div>
                    <div class="display-label">
                        <%:Html.LabelFor(model => model.LastNameL2)%>
                    </div>
                    <div class="display-field">
                        <%:Html.TextBoxFor(model => model.LastNameL2, new ReadOnlyTextBox(true, "SingleLine"))%>
                    </div>
                    <div class="display-label">
                        <%:Html.LabelFor(model => model.MotherNameL2)%>
                    </div>
                    <div class="display-field">
                        <%:Html.TextBoxFor(model => model.MotherNameL2, new ReadOnlyTextBox(true, "SingleLine"))%>
                    </div>
                </fieldset>
            </td>
            <td style="width: 33%; vertical-align: top;">
                <fieldset class="EditFieldset">
                    <legend><%: Resources.Areas.Personnel.Entities.Employee.EmployeeModel.NationalitiesAndBirthInformation %></legend>
                    <div class="display-label">
                        <%:Html.LabelFor(model => model.DateOfBirth)%>
                    </div>
                    <div class="display-field">
                        <%:Html.Encode(String.Format("{0:MM/dd/yyyy}", Model.DateOfBirth))%>
                    </div>
                    <div class="display-label">
                        <%:Html.LabelFor(model => model.CountryOfBirth)%>
                    </div>

                    <div class="display-field">
                        <%:Html.DisplayTextFor(model => model.CountryOfBirth.Name)%>
                    </div>
                     <div class="display-label">
                        <%:Html.LabelFor(model => model.PlaceOfBirth)%>
                    </div>
                    <div class="display-field">
                        <%:Html.DisplayTextFor(model => model.PlaceOfBirth)%>
                    </div>
                    <div class="display-label">
                        <%:Html.LabelFor(model => model.Nationality)%>
                    </div>
                    <div class="display-field">
                        <%:Html.DisplayTextFor(model => model.Nationality.Name)%>
                    </div>
                    <div class="display-label">
                        <%:Html.LabelFor(model => model.OtherNationality)%>
                    </div>
                    <div class="display-field">
                        <%:Html.DisplayTextFor(model => model.OtherNationality.Name)%>
                    </div>
                </fieldset>
            </td>
        </tr>
        <tr>
            <td style="width: 33%; vertical-align: top">
                <fieldset class="EditFieldset">
                    <legend><%: Resources.Areas.Personnel.Entities.Employee.EmployeeModel.PersonalInformation %></legend>
                    <div class="display-label">
                        <%:Html.LabelFor(model => model.Gender)%>
                    </div>
                    <div class="display-field">
                        <%:Html.DisplayTextFor(model => model.Gender.Name)%>
                    </div>
                    <div class="display-label">
                        <%:Html.LabelFor(model => model.BloodType)%>
                    </div>
                    <div class="display-field">
                        <%:Html.DisplayTextFor(model => model.BloodType.Name)%>
                    </div>
                    <div class="display-label">
                        <%:Html.LabelFor(model => model.Religion)%>
                    </div>
                    <div class="display-field">
                        <%:Html.DisplayTextFor(model => model.Religion.Name)%>
                    </div>
                    <div class="display-label">
                        <%:Html.LabelFor(model => model.Race)%>
                    </div>
                    <div class="display-field">
                        <%:Html.DisplayTextFor(model => model.Race.Name)%>
                    </div>
                </fieldset>
            </td>
            <td style="width: 33%; vertical-align: top">
                <fieldset class="EditFieldset">
                    <legend><%: Resources.Areas.Personnel.Entities.Employee.EmployeeModel.FamilyDetails %></legend>
                    <div class="display-label">
                        <%:Html.LabelFor(model => model.MaritalStatus)%>
                    </div>
                    <div class="display-field">
                        <%:Html.DisplayTextFor(model => model.MaritalStatus.Name)%>
                    </div>
                    <div class="display-label">
                        <%:Html.LabelFor(model => model.LoginName)%>
                    </div>
                    <div class="display-field">
                        <%:Html.TextBoxFor(model => model.LoginName, new ReadOnlyTextBox(true, "SingleLine"))%>
                    </div>
                    <div class="display-label">
                        <%:Html.LabelFor(model => model.NoOfChildren)%>
                    </div>
                    <div class="display-field">
                        <%: ViewData["ChildrenCounts"] %>
                    </div>
                    <div class="display-label">
                        <%:Html.LabelFor(model => model.NoOfDependents)%>
                    </div>
                    <div class="display-field">
                        <%:ViewData["DependentsCounts"]%>
                    </div>
                </fieldset>
            </td>
            <td style="width: 33%; vertical-align: top">
                <fieldset class="EditFieldset">
                    <legend><%: Resources.Areas.Personnel.Entities.Employee.EmployeeModel.Miscellaneous %></legend>
                    <div class="display-label">
                        <%:Html.LabelFor(model => model.MilitaryStatus)%>
                    </div>
                    <div class="display-field">
                        <%:Html.DisplayTextFor(model => model.MilitaryStatus.Name)%>
                    </div>
                    <div class="display-label">
                        <%:Html.LabelFor(model => model.IdentificationNo)%>
                    </div>
                    <div class="display-field">
                        <%:Html.TextBoxFor(model => model.IdentificationNo,
                                              new ReadOnlyTextBox(true, "SingleLine"))%>
                    </div>
                    <div class="display-label">
                        <%:Html.LabelFor(model => model.SocialSecurityNo)%>
                    </div>
                    <div class="display-field">
                        <%:Html.TextBoxFor(model => model.SocialSecurityNo,
                                              new ReadOnlyTextBox(true, "SingleLine"))%>
                    </div>
                    <div class="display-label">
                        <%:Html.LabelFor(model => model.DisabilityExist)%>
                    </div>
                    <div class="display-field">
                        <%:Html.DisplayTextFor(model => model.DisabilityExist.Name)%>
                    </div>
                    <div class="display-label">
                        <%:Html.LabelFor(model => model.DisabilityDescription)%>
                    </div>
                    <div class="display-field">
                        <%:Html.TextAreaFor(model => model.DisabilityDescription,
                                               new ReadOnlyTextBox(true, "MultiLine"))%>
                    </div>
                </fieldset>
            </td>
        </tr>
    </table>
</fieldset>
