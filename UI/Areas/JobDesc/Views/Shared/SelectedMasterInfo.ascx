<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<br />

<% if (Session["SelectedEntityRootId"] != null)
   {	%>
<%: Resources.Areas.JobDesc.Views.Shared.SelectedMasterInfo.SelectedMasterInfo.CurrentlySelectedMasterNo %>
<%: Session["SelectedEntityRootId"]!= null ? Session["SelectedEntityRootId"].ToString() : string.Empty %>
<%} %>
