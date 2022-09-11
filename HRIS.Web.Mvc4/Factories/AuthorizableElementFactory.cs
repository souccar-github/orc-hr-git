using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.ReportServer.ServiceModel.ConnectionProviders;
using DevExpress.XtraRichEdit.Import.Html;
using  Project.Web.Mvc4.Models.Controls;
using  Project.Web.Mvc4.Models.Navigation;
using NHibernate.Cfg;
using Souccar.Core.Fasterflect;
using Souccar.Domain.Extensions;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;
using Souccar.Reflector;
using Souccar.Core.Extensions;
using SpecExpress.Util;
using System.Data.EntityClient;
using System.Data.SqlClient;
using Souccar.NHibernate;
using  Project.Web.Mvc4.ProjectModels;

namespace Project.Web.Mvc4.Factories
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class AuthorizableElementFactory
    {
        public static IList<DualSelectListModel> AuthorizableModules()
        {
            var result = new List<DualSelectListModel>();
            foreach (var tab in BuildNavigation.Tabs)
            {
                foreach (var module in tab.Modules)
                {
                    result.Add(new DualSelectListModel()
                    {
                        Group =module.ModuleId,
                        //Value = module.ModuleId,
                        Value = module.SecurityId,
                        Title = module.Title,
                        Dir = "Left"
                    });
                }
            }
            return result;
        }

        public static IList<DualSelectListModel> AuthorizableDashboards()
        {
            var result = new List<DualSelectListModel>();
            foreach (var tab in BuildNavigation.Tabs)
            {
                foreach (var module in tab.Modules)
                {
                    result.AddRange(module.GetAuthorizeDashboards );
                }
            }
            return result;
        }

        public static IList<DualSelectListModel> AuthorizableAggregates()
        {
            var result=new List<DualSelectListModel>();
            foreach (var tab in BuildNavigation.Tabs)
            {
                foreach (var module in tab.Modules)
                {
                    result.AddRange( module.GetAuthorizeAggregates);
                }
            }
            return result;
        }

        public static IList<DualSelectListModel> AuthorizableServices()
        {
            var result = new List<DualSelectListModel>();
            foreach (var tab in BuildNavigation.Tabs)
            {
                foreach (var module in tab.Modules)
                {
                    result.AddRange(module.GetAuthorizeServices);
                }
            }
            return result;
        }

        public static IList<DualSelectListModel> AuthorizableIndexs()
        {
            var result = new List<DualSelectListModel>();
            foreach (var tab in BuildNavigation.Tabs)
            {
                foreach (var module in tab.Modules)
                {
                    result.AddRange(module.GetAuthorizeIndexs);
                }
            }
            return result;
        }

        public static IList<DualSelectListModel> AuthorizableReports()
        {
            var result = new List<DualSelectListModel>();
            foreach (var tab in BuildNavigation.Tabs)
            {
                foreach (var module in tab.Modules)
                {
                    result.AddRange(module.GetAuthorizeReports);
                }
            }
            return result;
        }

        public static IList<DualSelectListModel> AuthorizableDetails()
        {
            var result = new List<DualSelectListModel>();
            foreach (var tab in BuildNavigation.Tabs)
            {
                foreach (var module in tab.Modules)
                {
                    result.AddRange(module.GetAuthorizeDetails);
                }
            }
            return result;
        }

        public static IList<DualSelectListModel> AuthorizableActionLists()
        {
            var result = new List<DualSelectListModel>();
            foreach (var tab in BuildNavigation.Tabs)
            {
                foreach (var module in tab.Modules)
                {
                    result.AddRange(module.GetAuthorizeActionListCommands);
                }
            }
            return result;
        }


        public static IList<DualSelectListModel> AuthorizableConfigurations()
        {
            var result = new List<DualSelectListModel>();
            foreach (var tab in BuildNavigation.Tabs)
            {
                foreach (var module in tab.Modules)
                {
                    result.AddRange(module.GetAuthorizeConfigurations);
                }
            }
            return result;
        }

        public static IList<DualSelectListModel> AuthorizableField(IList<DualSelectListModel> authorizableFields)
        {
     
            var result = new List<DualSelectListModel>();
            foreach (var authorizableField in authorizableFields)
            {
                var type = authorizableField.Value.ToType();

                if (type == null)
                    continue;

                var defaultSessionFactory = NHibernateSession.GetDefaultSessionFactory();
                var classmMetaData = defaultSessionFactory.GetClassMetadata(type);
                var classTree = ClassTreeFactory.Create(type);
               
                foreach (var property in classTree.SimpleProperties)
                
                {
                    var prop = type.GetProperty(property.Name);
                    if (prop.PropertyType == typeof(DateTime))
                        continue;

                    var index = Array.FindIndex(classmMetaData.PropertyNames, row => row.Contains(property.Name));
                    if (index > 0)
                        if (classmMetaData.PropertyNullability[index] == false)
                      continue;
                      if (prop.CanWrite && !prop.GetIsNonEditable() && !prop.GetIsHidden() && prop.Name != "Id" && prop.Name != "IsVertualDeleted")
                    {
                        result.Add(new DualSelectListModel()
                        {
                           Description = authorizableField.Group,
                            Group = type.FullName,
                            Value = prop.Name,
                            Title = prop.GetTitle(),
                            Dir = "Left"
                        });

                    }

                }
                foreach (var property in classTree.ReferencesProperties)
                {
                    var prop = type.GetProperty(property.Name);
                    if ((prop.PropertyType.IsEnum) || !(prop.GetIsReference() || prop.PropertyType.IsIndex() || prop.PropertyType == typeof(DateTime?)))
                        continue;

               
                    var index = Array.FindIndex(classmMetaData.PropertyNames, row => row.Contains(property.Name));
                    if (index > 0)
                        if (classmMetaData.PropertyNullability[index] == false)
                            continue;
                    if (prop.CanWrite && !prop.GetIsNonEditable() && !prop.GetIsHidden())
                    {
                        
                        result.Add(new DualSelectListModel()
                        {
                            Description = authorizableField.Group,
                            Group = type.FullName,
                            Value = prop.Name,
                            Title = prop.GetTitle(),
                            Dir = "Left"
                        });

                    }

                }





            }
            return result;
        }


       

    }
}