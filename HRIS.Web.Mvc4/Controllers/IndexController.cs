using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Project.Web.Mvc4.Factories;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Project.Web.Mvc4.Models;
using Souccar.Domain.Extensions;
using Project.Web.Mvc4.Extensions;
using Souccar.Core.Extensions;
using Souccar.Infrastructure.Extenstions;

using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Models.Navigation;
using Project.Web.Mvc4.ProjectModels;
using Project.Web.Mvc4.ProjectModels;
using Souccar.Core.Fasterflect;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Souccar.Domain.Audit;

namespace Project.Web.Mvc4.Controllers
{
    public class IndexController : Controller
    {
        

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetGridModel(string name = null)
        {
            var model = GridViewModelFactory.Create(name.ToType(), null);

            model.ToolbarCommands.Clear();
           
            if (model.AuthorizedToAdd)
            {
                model.ToolbarCommands.Add(new ToolbarCommand()
                {
                    Name = BuiltinCommand.Create.ToString().ToLower(),
                    Text = model.Create
                });
            }
            model.ToolbarCommands.Add(new ToolbarCommand()
            {
                Text = model.ClearFiltering,
                ClassName = "k-grid-clear-filters",
                ImageClass = "k-icon k-clear-filter",
                Additional = false
            });

            model.ToolbarCommands.Add(new ToolbarCommand()
            {
                Text = model.ClearSorting,
                ClassName = "k-grid-clear-sorting",
                ImageClass = "k-icon k-delete",
                Additional = false
            });

            model.ActionList = new ActionList();
            foreach (var view in model.Views)
            {
                view.EditorMode = GridEditorMode.Inline.ToString().ToLower();

                view.ReadUrl = "Index/Read";
                view.CreateUrl = "Index/Create";
                view.UpdateUrl = "Index/Update";
                view.DestroyUrl = "Index/Delete";
            }
            return Json(model);
        }
        /// <summary>
        /// Author: Yaseen Alrefaee
        /// </summary>
        /// <returns></returns>
        //public ActionResult IndexesName()
        //{
        //    return Json(Assembly.GetAssembly(typeof(Employee)).GetIndexClasses().Select(x=>new {Value=x.Key.FullName,Text=x.Value}).ToList(),JsonRequestBehavior.AllowGet);
        //}

        /// <summary>
        /// Author: Yaseen Alrefaee
        /// </summary>
        /// <param name="moduleName"></param>
        /// <returns></returns>
        public ActionResult IndexesNameForModule(string moduleName)
        {
            Type chooseType = null;
           var module= BuildNavigation.GetModule(moduleName);
         var agg=   module.Aggregates.FirstOrDefault();


            var objectType = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.GetName().GetPublicKey().Length==0);
            foreach (var assembly in objectType)
            {
                var type = assembly.GetTypes().FirstOrDefault(x => x.IsClass && x.Name == agg.AggregateId);
                if (type != null)
                {
                    chooseType = type;
                    break;
                }

            }


            return Json(Assembly.GetAssembly(chooseType).GetIndexClassesByModule(moduleName).Select(x => new { Value = x.Key.FullName, Text = x.Key.GetLocalized() }).ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Author: Yaseen Alrefaee
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pageSize"></param>
        /// <param name="skip"></param>
        /// <param name="sort"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Read(string name, int pageSize = 10, int skip = 0, IEnumerable<GridSort> sort = null, GridFilter filter = null)
        {
            var type = name.ToType();
            var query = IndexHelper.GetAllIndexItems(type);
            CrudController.UpdateFilter(filter, type);

            var dataSourse = DataSourceResult.GetDataSourceResult(query, name.ToType(), pageSize, skip, true, sort, filter);
            return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new { Data = name.ToType().ToDynamicData(dataSourse.Data), TotalCount = dataSourse.Total });
        }

        /// <summary>
        /// Author: Yaseen Alrefaee
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[HttpGet]
        public ActionResult ReadToList(string typeName, RequestInformation requestInformation)
        {
            var type = typeName.ToType();
            
            var data = CrudController.GetAll(type);
            var result = data.ToList().Cast<IndexEntity>().OrderBy(x => x.Order).Select(x => new DropdownItemViewModel { Id = x.Id, Name = x.Name });
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Author: Yaseen Alrefaee
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(string name, Dictionary<string, object> data)
        {
            var indexValue = data["Name"].ToString().Trim();
            var index = (IndexEntity)name.ToType().GetById((int)data["Id"]);
            var errors = IndexHelper.GetIndexValidationResult(name, indexValue, (int)data["Id"]);
            if (errors.Any())
                return Json(new { Data = index, Errors = errors });
            try
            {
                var indexEntity = IndexHelper.GeDeletedIndexItemByName(indexValue, name.ToType());
                if (indexEntity != null)
                {
                    index.IsVertualDeleted = true;
                    indexEntity.IsVertualDeleted = false;
                    indexEntity.Order = int.Parse(data["Order"].ToString());
                    ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { index, indexEntity }, UserExtensions.CurrentUser, indexEntity, OperationType.Update);
                    return Json(new { Data = index });
                }
                else
                {
                    index.Name = indexValue;
                    index.Order = int.Parse(data["Order"].ToString());
                    ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { index }, UserExtensions.CurrentUser, index, OperationType.Update);
                    return Json(new { Data = index });
                }
            }
            catch (Exception e)
            {
                return Json(new { Data = index, Errors = new { Exception = e.Message } });
            }
            return Json(new { Data = index });
        }

        /// <summary>
        /// Author: Yaseen Alrefaee
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(string name, Dictionary<string, object> data)
        {
            var index = (IndexEntity)name.ToType().GetById((int)data["Id"]);
            try
            {
                index.IsVertualDeleted=true;
                ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { index }, UserExtensions.CurrentUser, index, OperationType.Delete);
            }
            catch (Exception e)
            {
                return Json(new { Data = data, Errors = e.Message });
            }

            return Json(new { Data = data });
        }

        /// <summary>
        /// Author: Yaseen Alrefaee
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Destroy(string name, Dictionary<string, object> data)
        {
            var index = (IndexEntity)name.ToType().GetById((int)data["Id"]);
            try
            {
                index.Delete();
            }
            catch (Exception e)
            {
                return Json(new { Data = data, Errors = e.Message });
            }

            return Json(new { Data = data });
        }

        /// <summary>
        /// Author: Yaseen Alrefaee
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(string name, Dictionary<string, object> data)
        {
            var indexValue = data["Name"].ToString().Trim();
            var newIndex = (IndexEntity)Activator.CreateInstance(name.ToType());
            var errors = IndexHelper.GetIndexValidationResult(name, indexValue, (int)data["Id"]);
            if (errors.Any())
                return Json(new { Data = newIndex, Errors = errors });
            try
            {
                var indexEntity = IndexHelper.GeDeletedIndexItemByName(indexValue, name.ToType());
                if (indexEntity != null)
                {
                    indexEntity.IsVertualDeleted = false;
                    indexEntity.Order = int.Parse(data["Order"].ToString());
                    ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { indexEntity }, UserExtensions.CurrentUser, indexEntity, OperationType.Insert);
                    return Json(new { Data = indexEntity });
                }
                else
                {
                    newIndex.Name = indexValue;
                    newIndex.Order = int.Parse(data["Order"].ToString());
                    ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { newIndex }, UserExtensions.CurrentUser, newIndex, OperationType.Insert);
                    return Json(new { Data = newIndex });
                }
            }
            catch (Exception e)
            {
                return Json(new { Data = newIndex, Errors = new { Exception = e.Message } });
            }
            return Json(new { Data = newIndex });
        }

        /// <summary>
        /// Author: Yaseen Alrefaee
        /// 
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateSingle(string indexName, string value)
        {
            value = value.Trim();
            var errors = IndexHelper.GetIndexValidationResult(indexName, value,0);
            if (errors.Any())
                return Json(new { Data = new{Name=value}, Errors = errors });
            var newIndex = (IndexEntity)Activator.CreateInstance(indexName.ToType());
            try
            {
                newIndex.Name = value;
                newIndex.Save();
            }
            catch (Exception e)
            {
                return Json(new { Data = newIndex, Errors = new { Exception = e.Message } });
            }
          
            return Json(new { Data = newIndex });
        }

        

      
    }

    public class IndexHelper
    {
        /// <summary>
        /// Author: Yaseen Alrefaee
        /// 
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<object> GetIndexValidationResult(string indexName, string value,int id)
        {
            var errors = new List<object>();
            var index = IndexHelper.GeIndexItemByName(value, indexName.ToType());
            if (string.IsNullOrEmpty(value))
                errors.Add(new { Name = "Name", Message = IndexLocalizationHelper.GetResource(IndexLocalizationHelper.NameRequired) });
            else if ( index!= null&&index.Id!=id)
                errors.Add(new { Name = "Name", Message = IndexLocalizationHelper.GetResource(IndexLocalizationHelper.NameAlreadyExists) });
            else if (value.Length > GlobalResorce.GetValidationSimpleStringMaxLength())
                errors.Add(
                    new
                    {
                        Name = "Name",
                        Message = string.Format(IndexLocalizationHelper.GetResource(IndexLocalizationHelper.NameMustBeLessThanCharacters),
                             GlobalResorce.GetValidationSimpleStringMaxLength())
                    });
            return errors;
        }

        /// <summary>
        /// Author: Yaseen Alrefaee
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static IQueryable<IndexEntity> GetAllIndexItems(Type t )
        {
            var method = typeof(Souccar.Infrastructure.Extenstions.TypeExtensions).GetMethod("GetAllWithVertualDeleted");
            method = method.MakeGenericMethod(new Type[] { t });
            return (IQueryable<IndexEntity>)method.Invoke(typeof(Souccar.Infrastructure.Extenstions.TypeExtensions), new[] { typeof(Souccar.Infrastructure.Extenstions.TypeExtensions), Activator.CreateInstance(t) });
        }

        public static IndexEntity GeDeletedIndexItemByName(string name, Type t)
        {
            var query = GetAllIndexItems(t);
            return !query.Any() ? null : Enumerable.FirstOrDefault(query, entity => entity.Name.ToLower().Equals(name.ToLower()) && entity.IsVertualDeleted);
        }
        /// <summary>
        /// Author: Yaseen Alrefaee
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static IndexEntity GeIndexItemByName(string name, Type t)
        {
            var query = GetAllIndexItems(t);
            return !query.Any() ? null : Enumerable.FirstOrDefault(query, entity => entity.Name.ToLower().Equals(name.ToLower()) && !entity.IsVertualDeleted);
        }

    }

}
