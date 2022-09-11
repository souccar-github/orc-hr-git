<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<UI.Models.LogOnModel>" %>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>MAESRO | Systems </title>

    <link rel="Stylesheet" type="text/css" href='<%=Url.Content("~/Content/default/general.css")%>' />
    <link rel="Stylesheet" type="text/css" href='<%=Url.Content("~/Content/default/login.css")%>' />

</head>

<body>
    <div class="container">
        <div class="header">
    
        </div>
        <div class="body">
            <div class="left">
                <span class="logo"></span>
            </div>

            <div class="right"> 
                <div class="login">
                    <% using (Html.BeginForm())
                       {%>
                    <p>
                        <label> User Name </label>  <%:Html.TextBoxFor(m => m.UserName)%>   <%: Html.ValidationMessageFor(m => m.UserName) %> </p>
  
                    <p>
                        <label> Password </label> <%:Html.PasswordFor(m => m.Password)%>  <%: Html.ValidationMessageFor(m => m.Password) %>
                    </p>
                    <p>
                        <input type="submit" value="Login" class="submit"/>
                    </p>
                    <% } %>
                </div>
        
            </div>
         
            <div class="clear"></div>
        </div>
    
        <div class="footer">
            ©2012 MAESTRO
        </div>
    </div>
</body>

