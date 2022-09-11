using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Domain.Seedwork;

namespace UI.Filters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class SecurityActionAttribute : ActionFilterAttribute
    {
        private string permssionSet = "Default";
        private string title = "";
        private string description = "";
        //private string redirectToController = "Security";
        private string redirectToAction = "";
        private bool throwOnDeny = false;
        private string resBaseName = "perms";
        private string titleResName = "";
        private string descResName = "";
        private string permssionSetResName = "";

        public string PermssionSetResName
        {
            get { return permssionSetResName; }
            set { permssionSetResName = value; }
        }

        public string DescResName
        {
            get { return descResName; }
            set { descResName = value; }
        }

        public string TitleResName
        {
            get { return titleResName; }
            set { titleResName = value; }
        }

        public string ResBaseName
        {
            get { return resBaseName; }
            set { resBaseName = value; }
        }

        public bool ThrowOnDeny
        {
            get { return throwOnDeny; }
            set { throwOnDeny = value; }
        }

        /// <summary>
        /// Gets/Sets the Action to redirect when authorize fail
        /// </summary>
        public string RedirectToAction
        {
            get
            {
                return redirectToAction;
            }
            set { redirectToAction = value; }
        }
        protected SecurityActionAttribute()
        {
            Context = new WebSiteContext(DependencyResolver.Current.GetService<IUnitOfWork>());
        }

        /// <summary>
        /// Init the SecurityActionAccribute class
        /// </summary>
        /// <param name="permissionSetName">Set the PermissionSetName which this Action belongs to.</param>
        /// <param name="permissionTitle">Set the PermissionTitle of this Action</param>
        public SecurityActionAttribute(string permissionSetName, string permissionTitle):this()
        {
            permssionSet = permissionSetName;
            title = permissionTitle;
        }

        /// <summary>
        /// Init the SecurityActionAccribute class
        /// </summary>
        /// <param name="permissionTitle">Set the PermissionTitle of this Action</param>
        public SecurityActionAttribute(string permissionTitle):this()
        {
            title = permissionTitle;
        }

        public SecurityActionAttribute(string permissionSetName, string permissionTitle, string permissionDescription):this()
        {
            permssionSet = permissionSetName;
            title = permissionTitle;
            description = permissionDescription;
        }

        /// <summary>
        /// Get/Sets the Description of the Permission for this Action
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// Gets/Sets the Title text of the Action
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// Gets/Sets the PermissionSet name which the security action belongs to
        /// </summary>
        ///<remark>
        ///  If the permission name is not exists Portal will add a new one
        ///</remark>
        public string PermssionSet
        {
            get { return permssionSet; }
            set { permssionSet = value; }
        }

        public virtual WebSiteContext Context { get; private set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Context.EnableSecurity)
            {
                if (
                    !(filterContext.HttpContext.User.Identity.IsAuthenticated &&
                      Context.IsAuthorized(filterContext.Controller.GetType(), filterContext.ActionDescriptor.ActionName)))
                {
                    filterContext.HttpContext.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                    string redirectTo = "~/Permission/Info";
                    if (!string.IsNullOrEmpty(Context.HttpContext.Request.RawUrl))
                    {
                        redirectTo = string.Format("~/Permission/Info?ReturnUrl={0}",
                                                   HttpUtility.UrlEncode(Context.HttpContext.Request.RawUrl));
                    }
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.Result = new JsonResult()
                                                   {
                                                       Data =
                                                           new
                                                               {
                                                                   Success = false,
                                                                   Status = filterContext.HttpContext.Response.StatusCode,
                                                                   ResponseText = "AUTHORIZATION_FAILED",
                                                                   Url = redirectTo
                                                               }
                                                   };
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult(redirectTo);

                    }

                }
            }
            base.OnActionExecuting(filterContext);
        }

    }
}