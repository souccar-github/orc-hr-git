<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.OrgChart.Indexes.NodeType>" %>
<% using (Ajax.BeginForm("Save", "NodeType", new AjaxOptions { OnComplete = "JsonAdd_OnComplete" }))
   {%>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%:Resources.Areas.OrgChart.Indexes.NodeType.NodeTypeModel.NodeTypeTitle %></legend>
    <script type="text/javascript">
        function JsonAdd_OnComplete(context) {

            var JsonAdd = context.get_response().get_object();
            if (JsonAdd.Success) {
                location.reload();
            } else {
                $("#result").html(JsonAdd.PartialViewHtml);

            }
        }
    </script>
    <fieldset class="ParentFieldset">
        <table border="0" cellpadding="0" cellspacing="0" style="margin-left: 1px">
            <tr>
                <td>
                    <%: Html.ValidationMessageFor(model => model.NodeOrder) %>
                </td>
            </tr>
            <tr>
                <td>
                    <%: Html.ValidationMessageFor(model => model.Name) %>
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="80%">
            <tr>
                <td style="width: 50%; vertical-align: top">

                                        <div class="editor-label-required">
                        <%: Html.LabelFor(model => model.NodeOrder) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.Telerik().NumericTextBox().Name("NodeOrder").MinValue(0).MaxValue(int.MaxValue).DecimalDigits(0)%>
                    </div>
                </td>
                <td style="width: 50%; vertical-align: top">
                    <div class="editor-label-required">
                        <%: Html.LabelFor(model => model.Name) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.TextAreaFor(model => model.Name)%>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <a href="<%: Url.Action("Index", "NodeType") %>">
                        <img src="<%: Url.Content("~/Content/Ribbon/Icons/48/90.png") %>" title=<%:Resources.Shared.Buttons.Function.BackToMainPage %>
                            alt=<%:Resources.Shared.Buttons.Function.BackToMainPage %> height="24" width="24" align="middle" />
                    </a>
                </td>
                <td style="width: 10%; vertical-align: top" align="right">
                    <input type="image" value=<%:Resources.Shared.Buttons.Function.Save %> src="<%= Url.Content("~/Content/Ribbon/Icons/48/37.png") %>"
                        title=<%:Resources.Shared.Buttons.Function.Save %> alt=<%:Resources.Shared.Buttons.Function.Save %> height="24" width="24" align="middle" />
                </td>
            </tr>
        </table>
    </fieldset>
    <% } %>
