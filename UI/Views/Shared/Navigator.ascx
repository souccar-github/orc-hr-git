<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="UI.Utilities" %>
<%
    if (ViewData["PathStepsList"] != null)
    {
        var pathStepsList = ViewData["PathStepsList"] as IList<PathStep>;
        if (pathStepsList != null && pathStepsList.Count != 0)
        {
            if (pathStepsList.Count == 1)
            {
                var pathStep1 = pathStepsList[0];
%>
                
                <%:Html.ActionLink(pathStep1.StepName, "GoBackward", pathStep1.ContextName,
                                                  new
                                                      {
                                                          area = pathStep1.AreaName,
                                                          id = pathStep1.StepId,
                                                          stepOrder = pathStep1.StepOrder
                                                      }, new { })%>
              
           <%
            }
            else
            {
                for (var i = 0; i < pathStepsList.Count - 1; i++)
                {
                    var pathStep = pathStepsList[i];

%>

<%:Html.ActionLink(pathStep.StepName, "GoBackward", pathStep.ContextName,
                                                      new
                                                          {
                                                              area = pathStep.AreaName,
                                                              id = pathStep.StepId,
                                                              stepOrder = pathStep.StepOrder
                                                          }, new {})%>
-->
<%
                }
                var pathStep2 = pathStepsList[pathStepsList.Count-1];
              %>
              
              <%:Html.ActionLink(pathStep2.StepName, "GoBackward", pathStep2.ContextName,
                                                      new
                                                          {
                                                              area = pathStep2.AreaName,
                                                              id = pathStep2.StepId,
                                                              stepOrder = pathStep2.StepOrder
                                                          }, new {})%>
              <%  
                
            }
        }
    }
%>