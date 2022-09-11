<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Objective/Views/Shared/Objective.master"
    Inherits="System.Web.Mvc.ViewPage<IQueryable<HRIS.Domain.Objectives.RootEntities.Objective>>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">  
    
   <% Html.RenderPartial("MasterGrid"); %>
    <br />
    <div id="result" style="display: none">
    </div>


</asp:Content>
