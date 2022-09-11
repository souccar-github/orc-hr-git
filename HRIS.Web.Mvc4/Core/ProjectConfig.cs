//  Copyright (c) 2011 Ray Liang (http://www.dotnetage.com)
//  Licensed MIT: http://www.opensource.org/licenses/mit-license.php

using System;
using System.Web;
using System.Web.Configuration;
using System.Configuration;
using System.Web.Security;
using System.Xml;

namespace Project.Web.Mvc4.Core
{
    public static class ProjectConfig
    {
        private static string dnaHashStr = null;

        public static bool TestDomainIsChanged(HttpContextBase context)
        {
            var hashCode = GenerateDomainHash(context);
            return hashCode != DnaHash;
        }

        public static string RuntimeVersion
        {
            get
            {
                return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public static int GenerateDomainHash(HttpContextBase context)
        {
            var request = context.Request;
            string schemeAndAuthority = request.Url.Scheme + "://" + request.Url.Authority + request.ApplicationPath;
            var hashCode = schemeAndAuthority.ToLower().GetHashCode();
            return hashCode;
        }

        internal static void Restart()
        {
            System.Web.HttpRuntime.UnloadAppDomain();
        }

        internal static bool IsHttpContextAvaildable
        {
            get
            {
                try
                {
                    var appPath = HttpContext.Current.Request.ApplicationPath;
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }

            }
        }

        internal static bool IsNeedInstallCheck
        {
            get
            {
                var key="dna_runtime_health";
                if (IsHttpContextAvaildable)
                {
                    DnaRuntimeHealth health = null;
                    if (HttpContext.Current.Cache[key] != null)
                        health = HttpContext.Current.Cache[key] as DnaRuntimeHealth;
                    else
                    {
                        health = new DnaRuntimeHealth();
                        health.Check();
                        HttpContext.Current.Cache.Add(key, health, null, DateTime.Now.AddMinutes(20), System.Web.Caching.Cache.NoSlidingExpiration,
                            System.Web.Caching.CacheItemPriority.Default, null);
                    }
                    return !health.IsFine;
                }
                else
                {
                    var health = new DnaRuntimeHealth();
                    health.Check(false);
                    return !health.IsFine;
                }
            }
        }

        internal static void SaveDomain(HttpContextBase context)
        {
            var hashCode = GenerateDomainHash(context).ToString();
            UpdateWebConfig("configuration/appSettings/add[@key='DnaHash']", "value", hashCode);
        }

        internal static void UpdateWebConfig(string match, string key, string value, string configuationFile="web.config")
        {
            var config = new XmlDocument();
            string configPath = HttpContext.Current.Server.MapPath("~/" + configuationFile);
            config.Load(configPath);

            if (config == null)
                throw new Exception("The configuation file not found.");

            var sectionNode = config.SelectSingleNode(match);
            sectionNode.Attributes[key].Value = value;
            config.Save(configPath);
        }

        private static Configuration config = null;

        public static Configuration Configuration
        {
            get
            {
                if (config == null)
                    config = WebConfigurationManager.OpenWebConfiguration("~/web.config");
                return config;
            }
        }

        public static int DnaHash
        {
            get
            {
                if (string.IsNullOrEmpty(dnaHashStr))
                {
                    if (Configuration.AppSettings.Settings["DnaHash"] != null)
                        dnaHashStr = config.AppSettings.Settings["DnaHash"].Value;
                }

                if (!string.IsNullOrEmpty(dnaHashStr))
                {
                    int hash = 0;
                    int.TryParse(dnaHashStr, out hash);
                    return hash;
                }

                return 0;
            }
        }
    }

    internal class DnaRuntimeHealth
    {
        internal bool IsFine
        {
            get { return IsAspnetDBHealth && IsCoreDbHealth && !IsDomainChanged; }
        }

        internal void Check(bool httpAvailable = true)
        {
            IsAspnetDBHealth = _IsAspnetDBHealth;

            if (httpAvailable)
            {
                IsDomainChanged = _IsDomainNameChanged;
                IsCoreDbHealth = _IsCoreDbExists && _IsTopSiteExists;
            }
            else
            {
                IsDomainChanged = ProjectConfig.DnaHash == 0;
                //IsCoreDbHealth = _IsCoreDbExists && _TryGetTopSite();
            }
        }

        internal bool IsAspnetDBHealth { get; set; }

        internal bool IsCoreDbHealth { get; set; }

        internal bool IsDomainChanged { get; set; }

        /// <summary>
        /// check aspnet db
        /// </summary>
        /// <returns></returns>
        private bool _IsAspnetDBHealth
        {
            get
            {
                try
                {
                    if (Roles.RoleExists("administrators"))
                        return true;
                }
                catch (Exception e)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Check core database is exists and top site is exists.
        /// </summary>
        /// <returns></returns>
        private bool _IsCoreDbExists
        {
            get
            {
                try
                {
                    //return Database.Exists("DNADB");
                    return false;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        private bool _IsTopSiteExists
        {
            get
            {
                try
                {
                   // return WebSiteContext.Current.RootWeb != null;
                    return false;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        private bool _IsDomainNameChanged
        {
            get
            {
                if (ProjectConfig.DnaHash == 0)
                    return true;
                return ProjectConfig.TestDomainIsChanged(new HttpContextWrapper(HttpContext.Current));
            }
        }

        
    }
}