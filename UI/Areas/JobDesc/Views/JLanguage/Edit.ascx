<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.JobDesc.ValueObjects.JLanguage>" %>
<%@ Import Namespace="UI.Areas.Personnel.Helpers" %>
<fieldset class="ParentFieldset">
    <div>
        <div>
            <ul>
                <li>
                    <%: Html.ValidationMessageFor(model => model.Name) %>
                </li>
                <li>
                    <%: Html.ValidationMessageFor(model => model.Reading) %>
                </li>
                <li>
                    <%: Html.ValidationMessageFor(model => model.Writing) %>
                </li>
                <li>
                    <%: Html.ValidationMessageFor(model => model.Speaking) %>
                </li>
                <li>
                    <%: Html.ValidationMessageFor(model => model.Listening) %>
                </li>
                <li>
                    <%: Html.ValidationMessageFor(model => model.Weight) %>
                </li>
            </ul>
        </div>
        <div>
            <%: Html.HiddenFor(model => model.Id) %>
            <div>
                <%: Html.LabelFor(model => model.Name) %>
                <br />
                <%: Html.Telerik().DropDownListFor(model => model.Name.Id)
                                      .BindTo(UI.Areas.JobDesc.Helpers.DropDownListHelpers.ListOfLanguageName)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })%>
            </div>
            <div>
                <%: Html.LabelFor(model => model.Reading) %>
                <br />
                <%: Html.Telerik().DropDownListFor(model => model.Reading.Id)
                                      .BindTo(PersonnelDropDownListHelpers.ListOfLevels)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                %>
            </div>
            <div>
                <%: Html.LabelFor(model => model.Writing) %>
                <br />
                <%: Html.Telerik().DropDownListFor(model => model.Writing.Id)
                                      .BindTo(PersonnelDropDownListHelpers.ListOfLevels)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                %>
            </div>
            <div>
                <%: Html.LabelFor(model => model.Speaking) %>
                <br />
                <%: Html.Telerik().DropDownListFor(model => model.Speaking.Id)
                                      .BindTo(PersonnelDropDownListHelpers.ListOfLevels)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                %>
            </div>
            <div>
                <%: Html.LabelFor(model => model.Listening) %>
                <br />
                <%: Html.Telerik().DropDownListFor(model => model.Listening.Id)
                                      .BindTo(PersonnelDropDownListHelpers.ListOfLevels)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                %>
            </div>
            <div>
                <%: Html.LabelFor(model => model.Required) %>
                <br />
                <%: Html.CheckBoxFor(model => model.Required) %>
            </div>
            <div>
                <%: Html.LabelFor(model => model.Weight) %>
                <br />
                <%: Html.Telerik().NumericTextBox().Name("Weight").MinValue(0).MaxValue(100)%>
            </div>
        </div>
        <div id="CommandsDiv">
            <input type="image" value="Save" src="<%= Url.Content("~/Content/Ribbon/Icons/48/37.png") %>"
                title="<%: Resources.Shared.Buttons.Function.Save %>" alt="<%: Resources.Shared.Buttons.Function.Save %>" height="24" width="24" align="middle" />
        </div>
    </div>
</fieldset>
