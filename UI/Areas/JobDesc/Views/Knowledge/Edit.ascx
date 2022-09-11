<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.JobDesc.ValueObjects.Knowledge>" %>
<%@ Import Namespace="UI.Areas.Personnel.Helpers" %>
<fieldset class="ParentFieldset">
    <div>
        <div>
            <%: Html.ValidationMessageFor(model => model.Field) %>
            <%: Html.ValidationMessageFor(model => model.Level) %>
            <%: Html.ValidationMessageFor(model => model.Weight) %>
            <%: Html.ValidationMessageFor(model => model.Required) %>
        </div>
        <div>
            <%: Html.HiddenFor(model => model.Id) %>
            <div>
            </div>
            <div>
                <%: Html.LabelFor(model => model.Field) %><br />
                <%: Html.TextBoxFor(model => model.Field)%>
            </div>
            <div>
                <%: Html.LabelFor(model => model.Level) %><br />
                <%: Html.Telerik().DropDownListFor(model => model.Level.Id)
                                      .BindTo(PersonnelDropDownListHelpers.ListOfLevels)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                %>
            </div>
            <div>
                <%: Html.LabelFor(model => model.Weight) %><br />
                <%: Html.Telerik().NumericTextBox().Name("Weight").MinValue(0).MaxValue(100)%>
            </div>
            <div>
                <%: Html.LabelFor(model => model.Required) %><br />
                <%: Html.CheckBoxFor(model => model.Required) %>
            </div>
        </div>
        <div id="CommandsDiv">
            <input type="image" value="Save" src="<%= Url.Content("~/Content/Ribbon/Icons/48/37.png") %>"
                title="<%: Resources.Shared.Buttons.Function.Save %>" alt="<%: Resources.Shared.Buttons.Function.Save %>" height="24" width="24" align="middle" />
        </div>
    </div>
</fieldset>
