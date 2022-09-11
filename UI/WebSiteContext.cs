using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.WebPages;
using Domain.Seedwork;
using Souccar.Security.Domain;

namespace UI
{
    public class WebSiteContext : IDisposable
    {
        private readonly HttpContextBase _httpContext;
        private RouteData _routeData = null;
        private readonly IUnitOfWork _unitOfWork;
        private WebPage _page;
        private IEnumerable<Permission> _rolesPermissions = null;
        private int[] _permHashValues = null;
        private readonly IPermissionRepository _permissionRepository;
        
        public WebSiteContext(IUnitOfWork unitOfWork) : this(System.Web.HttpContext.Current, unitOfWork) { }

        public WebSiteContext(HttpContext httpContext, IUnitOfWork unitOfWork) : this(new HttpContextWrapper(httpContext), unitOfWork) { }

        public WebSiteContext(HttpContextBase httpContext, IUnitOfWork unitOfWork)
        {
            _httpContext = httpContext;
            _unitOfWork = unitOfWork;
            _permissionRepository = GetService<IPermissionRepository>();
        }

        /// <summary>
        ///  Gets the current web context for DNA
        /// </summary>
        public static WebSiteContext Current
        {
            get
            {
                return GetService<WebSiteContext>();
            }
        }

        public static IEnumerable<T> GetServices<T>()
        {
            return DependencyResolver.Current.GetServices<T>();
        }

        public static T GetService<T>()
        {
            return DependencyResolver.Current.GetService<T>();
        }

        #region DAL context properties

        public virtual IUnitOfWork UnitOfWork
        {
            get
            {
                return _unitOfWork;
            }
        }

       

        

        public bool IsAuthorized(Type controllerType, string action)
        {
            if (!HttpContext.Request.IsAuthenticated) return false;

            if (controllerType == null) return false;

            if (string.IsNullOrEmpty(action)) return false;

            if (Roles.IsUserInRole("administrators")) return true;

            if (IsNotInstalled) return false;

            if ((Permissions != null) && (_permHashValues != null))
            {

                var code = (controllerType.FullName + "." + action).ToLower().GetHashCode();
                return _permHashValues.Contains(code);
            }
            return false;
        }

        public bool IsAuthorized<TController>(string action)
        {
            return IsAuthorized(typeof(TController), action);
        }

        public bool IsAuthorized<TController>(TController controller, string action)
        {
            return IsAuthorized<TController>(action);
        }

        

        /// <summary>
        /// Return permissions for current user's roles.
        /// </summary>
        public IEnumerable<Permission> Permissions
        {
            get
            {
                if (IsNotInstalled) return null;

                if ((_rolesPermissions == null) && (HttpContext.Request.IsAuthenticated))
                {
                   
                    _rolesPermissions = _permissionRepository.GetUserPermissions(User.Identity.Name);
                    if ((_rolesPermissions != null) && (_rolesPermissions.Count() > 0))
                        _permHashValues = _rolesPermissions.Select(p => (p.Controller + "." + p.Action).ToLower().GetHashCode()).ToArray();
                }
                return _rolesPermissions;
            }
        }

        #endregion

        #region Http context properties

        internal bool IsNotInstalled
        {
            get
            {
                return false;
                //string.IsNullOrEmpty(WebConfigurationManager.AppSettings["HRISHash"]);
            }
        }

        /// <summary>
        /// Gets the current httpcontext object
        /// </summary>
        public HttpContextBase HttpContext
        {
            get { return _httpContext; }
        }

        /// <summary>
        /// Gets the current user object.
        /// </summary>
        public System.Security.Principal.IPrincipal User
        {
            get
            {
                return this.HttpContext.User;
            }
        }

        public List<CultureInfo> Cultures
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        
        public string Language
        {
            get
            {
                throw new NotImplementedException();
            }
        }

       
        public RouteData RouteData
        {
            get
            {
                if (_routeData == null)
                    return this._httpContext.Request.RequestContext.RouteData;
                else
                    return _routeData;
            }
        }

        public RequestContext RequestContext
        {
            get
            {
                return this._httpContext.Request.RequestContext;
            }
        }

        public static string GetTemporaryDirectory(HttpContextBase httpContext)
        {
            return httpContext.Server.MapPath("~/Shared/temporary/") + Path.GetRandomFileName().Replace(".", "");
        }

        public string GetTemporaryDirectory()
        {
            return GetTemporaryDirectory(HttpContext);
        }

        /// <summary>
        /// Gets the default uri for current request by current route.
        /// </summary>
        public string GetRouteDefaultUrl(string[] ignoreRouteDataKeys, RouteData routeData)
        {
            string defaultUrl = "";
            var routeDataValues = new RouteValueDictionary(routeData.Values);
            var route = (Route)routeData.Route;

            if (route != null)
            {
                if (ignoreRouteDataKeys != null)
                {
                    //Using default value
                    foreach (string key in ignoreRouteDataKeys)
                    {
                        if (route.Defaults.ContainsKey(key) && (routeDataValues.ContainsKey(key)))
                            routeDataValues[key] = route.Defaults[key];
                    }
                }

                var virtualPathData = route.GetVirtualPath(this.HttpContext.Request.RequestContext, routeDataValues);
                if (virtualPathData != null)
                    defaultUrl = virtualPathData.VirtualPath;
            }

            if (!string.IsNullOrEmpty(defaultUrl))
                defaultUrl = defaultUrl.StartsWith("/") ? ("~" + defaultUrl) : ("~/" + defaultUrl);

            return defaultUrl;
        }

        

        public RouteData FindRoute(Uri uri)
        {
            var innerhttpContext = new HttpContext(new System.Web.Hosting.SimpleWorkerRequest(VirtualPathUtility.ToAppRelative(uri.LocalPath).Replace("~/", ""), "", null));
            var wrapper = new HttpContextWrapper(innerhttpContext);

            foreach (var route in RouteTable.Routes)
            {
                _routeData = route.GetRouteData(wrapper);
                if (_routeData != null)
                {
                    var vpath = route.GetVirtualPath(wrapper.Request.RequestContext, _routeData.Values);
                    if (vpath != null)
                    {
                        return _routeData;
                    }
                }
            }
            return null;
        }

       

       
        #endregion

        #region Global settings

        public string ApplicationPath
        {
            get
            {
                var request = _httpContext.Request;
                string url = request.Url.Scheme + "://" + request.Url.Authority;
                if (!request.ApplicationPath.Equals("/"))
                    url += request.ApplicationPath;
                
                return url;
            }
        }

        public bool EnablePersonalWeb
        {
            get
            {
                var epw = System.Web.Configuration.WebConfigurationManager.AppSettings["enablePersonalWeb"];
                if (string.IsNullOrEmpty(epw)) return false;
                var result = false;
                bool.TryParse(epw, out result);
                return result;
            }
        }

        public  bool EnableSecurity
        {
            get
            {
                string implementSecurity =
                    System.Web.Configuration.WebConfigurationManager.AppSettings["ImplementSecurity"];
                if (string.IsNullOrEmpty(implementSecurity)) return false;
                var result = false;
                bool.TryParse(implementSecurity, out result);
                return result;
            }
        }

    
        
        public void SetCulture(string langName)
        {
            SetCulture(new System.Globalization.CultureInfo(langName));
        }

        public void SetCulture(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
        }

        #endregion

        public void Dispose()
        {
            if (UnitOfWork != null)
                UnitOfWork.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}