using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Domain.Seedwork;
using Souccar.Core.UI.Initializers;
using Souccar.Security.Domain;
using StructureMap;
using UI.Filters;
using Infrastructure.Validation;

namespace UI.Initializers
{
    public class SecurityInitializer : IAppInitializer
    {

        private readonly IPermissionRepository permissionRepository;
        private readonly IPermissionSetRepository permissionSetRepository;
        private readonly IUnitOfWork unitOfWork;

        //public SecurityInitializer(IUnitOfWork unitOfWork, IPermissionRepository permissionRepository, IPermissionSetRepository permissionSetRepository)
        //{
        //    this.permissionRepository = permissionRepository;
        //    this.permissionSetRepository = permissionSetRepository;
        //    this.unitOfWork = unitOfWork;
        //}

        public SecurityInitializer()
        {
            this.unitOfWork = ObjectFactory.GetInstance<IUnitOfWork>();
            this.permissionRepository = ObjectFactory.With(unitOfWork).GetInstance<IPermissionRepository>();

            this.permissionSetRepository = ObjectFactory.With(unitOfWork).GetInstance<IPermissionSetRepository>();
        }
        public void Init()
        {
            try
            {
                //When using LoadFile will cause could not get CustomAttributes!
                var assembly = this.GetType().Assembly;
                var asmname = assembly.GetName();
                Type[] types = assembly.GetTypes();
                var controllers = from c in types

                                  where c.Name.Contains("Controller")
                                  select c;

                Dictionary<string, string> added = new Dictionary<string, string>();

                foreach (Type controller in controllers)
                {
                    var methods = controller.GetMethods(BindingFlags.Public | BindingFlags.Instance);
                    var actions = from MethodInfo method in methods
                                  where (method.GetCustomAttributes(typeof(SecurityActionAttribute), true).Length > 0)
                                  select method;

                    foreach (MethodInfo action in actions)
                    {
                        SecurityActionAttribute attr = (SecurityActionAttribute)Attribute.GetCustomAttribute(action, typeof(SecurityActionAttribute));

                        //var instance = permissionRepository.GetAll().Where(p => (p.Action.Equals(action.Name, StringComparison.OrdinalIgnoreCase)) &&
                        //               (p.Assembly.Equals(asmname.Name, StringComparison.OrdinalIgnoreCase)) &&
                        //               (p.Controller.Equals(controller.FullName, StringComparison.OrdinalIgnoreCase)) &&
                        //               (p.Title.Equals(attr.Title, StringComparison.OrdinalIgnoreCase)));
                        var instance =
                            permissionRepository.GetAll().Where(
                                p => (p.Action == action.Name) &&
                                     (p.Assembly == asmname.Name) &&
                                     (p.Controller == controller.FullName));


                        if (instance.Count() > 0)
                            continue;

                        string _key = asmname.Name + "_" + controller.FullName + "_" + action.Name;
                        //if (added.ContainsKey(_key))
                        //{
                        //    if (added[_key] == attr.Title)
                        //        continue;
                        //}
                        //else
                        //    added.Add(_key, attr.Title);

                        Permission permission = new Permission()
                        {
                            Action = action.Name,
                            Assembly = asmname.Name,
                            Controller = controller.FullName,
                            Title = attr.Title,
                            Description = attr.Description
                        };

                        PermissionSet pset = null;
                        //if (!string.IsNullOrEmpty(attr.PermssionSet))
                        pset = permissionSetRepository.GetByName(attr.PermssionSet);

                        //var _updateCount = 0;

                        if (pset == null)
                        {
                            pset = new PermissionSet();
                            pset.Name = attr.PermssionSet;
                            pset.ResbaseName = attr.ResBaseName;
                            pset.TitleResName = attr.PermssionSetResName;
                            permissionSetRepository.AddEntity(pset);
                            //_updateCount=context.SaveChanges();
                        }

                        permission.PermissionSet = pset;
                        permissionRepository.AddEntity(permission);
                        unitOfWork.Commit();
                    }
                }
            }
            catch (Exception e) { }
        }
    }
}