//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Web;
//using System.Web.Mvc;
//using HRIS.Domain.Personnel.RootEntities;
//using Souccar.Domain.Extensions;
//using Souccar.Domain.PersistenceSupport;

//namespace Project.Web.Mvc4.Controllers
//{
//    public class IndexBatchController : Controller
//    {
//        private Type getTypeByName(string className)
//        {
//            var assembly = Assembly.GetAssembly(typeof(Employee));
//            return assembly.GetTypes().SingleOrDefault(c => c.FullName == className);
//        }

//        [HttpPost]
//        public ActionResult Create(string name, List<Dictionary<string, object>> models)
//        {
//            var indexType = getTypeByName(name);
//            var generic = typeof(IRepository<>);
//            var repositoryType = generic.MakeGenericType(new System.Type[] { indexType });
//            var repository = DependencyResolver.Current.GetService(repositoryType);
//            repositoryType = repository.GetType();
//            var addMethod = repositoryType.GetMethod("Add", new Type[] { indexType });
//            var dbContext = (IDbContext)repositoryType.GetProperty("DbContext").GetValue(repository);
//            foreach (var item in models)
//            {
//                try
//                {
//                    var newIndex = Activator.CreateInstance(indexType);
//                    foreach (var key in item.Keys.Where(x => item[x] != null))
//                    {
//                        var propertyInfo = indexType.GetProperty(key);
//                        if (propertyInfo.CanWrite)
//                        {
//                            propertyInfo.SetValue(newIndex, Convert.ChangeType(item[key], propertyInfo.PropertyType));
//                        }
//                    }
//                    using (dbContext.BeginTransaction())
//                    {
//                        addMethod.Invoke(repository, new object[] { newIndex });
//                        dbContext.CommitTransaction();
//                    }
//                }
//                catch (Exception e)
//                {
//                    item.Add("Error", e.Message);
//                }
//            }
//            return Json(models);
//        }


//        [HttpPost]
//        public ActionResult Destroy(string name, List<Dictionary<string, object>> models)
//        {
//            var indexType = getTypeByName(name);
//            var generic = typeof(IRepository<>);
//            var repositoryType = generic.MakeGenericType(new System.Type[] { indexType });
//            var repository = DependencyResolver.Current.GetService(repositoryType);
//            repositoryType = repository.GetType();
//            var getMethod = repositoryType.GetMethod("GetById", new Type[] { typeof(int) });
//            var deleteMethod = repositoryType.GetMethod("Delete", new Type[] { indexType });
//            var dbContext = (IDbContext)repositoryType.GetProperty("DbContext").GetValue(repository);
//            foreach (var item in models)
//            {
//                var index = getMethod.Invoke(repository, new object[] { item["Id"] });
//                using (dbContext.BeginTransaction())
//                {
//                    try
//                    {
//                        deleteMethod.Invoke(repository, new object[] { index });
//                        dbContext.CommitTransaction();
//                    }
//                    catch (Exception e)
//                    {
//                        item.Add("Error", e.Message);
//                    }
//                }
//            }
//            return Json(models);
//        }

//        [HttpPost]
//        public ActionResult Update(string name, List<Dictionary<string, object>> models)
//        {
//            var indexType = getTypeByName(name);
//            var generic = typeof(IRepository<>);
//            var repositoryType = generic.MakeGenericType(new System.Type[] { indexType });
//            var repository = DependencyResolver.Current.GetService(repositoryType);
//            repositoryType = repository.GetType();
//            var getMethod = repositoryType.GetMethod("GetById", new Type[] { typeof(int) });
//            var updateMethod = repositoryType.GetMethod("Update", new Type[] { indexType });
//            var dbContext = (IDbContext)repositoryType.GetProperty("DbContext").GetValue(repository);
//            foreach (var item in models)
//            {
//                try
//                {
//                    var index = getMethod.Invoke(repository, new object[] { item["Id"] });
//                    foreach (var key in item.Keys.Where(x => item[x] != null))
//                    {
//                        var propertyInfo = indexType.GetProperty(key);
//                        if (propertyInfo.CanWrite)
//                        {
//                            propertyInfo.SetValue(index, Convert.ChangeType(item[key], propertyInfo.PropertyType));
//                        }
//                    }
//                    using (dbContext.BeginTransaction())
//                    {
//                        updateMethod.Invoke(repository, new object[] { index });
//                        dbContext.CommitTransaction();
//                    }
//                }
//                catch (Exception e)
//                {
//                    item.Add("Error", e.Message);
//                }
//            }
//            return Json(models);
//        }
//    }
//}
