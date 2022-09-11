using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using FluentNHibernate.Conventions;
using  Project.Web.Mvc4.Models.GridModel;

using Souccar.Domain.DomainModel;
using  Project.Web.Mvc4.Models;
using Souccar.Infrastructure.Core;
using Souccar.Core.Extensions;
using Souccar.Infrastructure.Extenstions;
using System.Collections;
using System.Dynamic;


using DataTable = System.Data.DataTable;
using Souccar.Domain.Localization;

namespace Project.Web.Mvc4.Controllers
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class ExportController : Controller
    {
        //
        // GET: /Export/

        public ActionResult ExportCSV(int pageSize = 10, int skip = 0, bool serverPaging = true,
           IEnumerable<GridSort> sort = null, GridFilter filter = null, IEnumerable<GridGroup> group = null,
           RequestInformation requestInformation = null, GridViewModel gridModel = null)
        {


            DataSourceResult dataSourse = new DataSourceResult();
            if (gridModel.ViewModelTypeFullName != null)
            {
                var viewMode = (ViewModel)Activator.CreateInstance(gridModel.ViewModelTypeFullName.ToType());
                viewMode.BeforeRead(requestInformation);
            }

            var previous = requestInformation.NavigationInfo.Previous;
            var entityType = previous.Last().TypeName.ToType();
            CrudController.UpdateFilter(filter, entityType);
            IQueryable<IEntity> queryable = null;
            if (previous.Count == 1)
                queryable = CrudController.GetAllWithVertualDeleted(previous[0].TypeName.ToType());
            else
            {
                var selectedDetail = (Entity)previous[0].TypeName.ToType().GetById(previous[0].RowId);
                for (var i = 1; i < previous.Count; i++)
                {
                    var list = selectedDetail.GetPropertyValue(previous[i].Name);
                    if (i == previous.Count - 1)
                        queryable = (list as IEnumerable<IEntity>).AsQueryable();
                    else
                        selectedDetail =
                            (list as IEnumerable<Entity>).AsQueryable().SingleOrDefault(x => x.Id == previous[i].RowId);
                }
            }

            dataSourse = DataSourceResult.GetDataSourceResult(queryable, previous.Last().TypeName.ToType(), pageSize,
                skip, false, sort, filter, requestInformation);

            if (gridModel.ViewModelTypeFullName != null)
            {
                var viewMode = (ViewModel)Activator.CreateInstance(gridModel.ViewModelTypeFullName.ToType());
                viewMode.AfterRead(requestInformation, dataSourse, dataSourse.Data.Count(), skip);
            }





            var notInValu = new List<string> { "Id", "IsVertualDeleted", "NameForDropdown" };
            DataTable table = new DataTable();

            for (int i = 0; i < gridModel.Views[gridModel.CurrentViewId].Columns.Count; i++)
            {
                var fieldName = gridModel.Views[gridModel.CurrentViewId].Columns[i].FieldName;
                if (fieldName.IsNotAny(notInValu.ToArray()))
                    table.Columns.Add(gridModel.Views[gridModel.CurrentViewId].Columns[i].Title);
            }
            foreach (var item in dataSourse.Data)
            {
                var j = 0;
                var row = table.NewRow();

                foreach (var col in gridModel.Views[gridModel.CurrentViewId].Columns.OrderBy(x => x.Order))
                {


                    var value = item.GetPropertyValue(col.FieldName);

                    if (col.FieldName.IsNotAny(notInValu.ToArray()))
                    {
                        if (value == null)
                            row[col.Title] = "";
                        else if (value is IndexEntity)
                            row[col.Title] = ((IndexEntity)value).Name;
                        else if (value is Enum)
                        {
                            var title = ServiceFactory.LocalizationService.GetResource(value.GetType().FullName + "." + value.ToString());
                            row[col.Title] = string.IsNullOrEmpty(title) ? value.ToString() : title;

                        }
                        else if (value is ICollection)
                            row[col.Title] = (value as ICollection).
                                Count;
                        else if (value is DateTime)
                        {
                            if (col.IsTime)
                                row[col.Title] = ((DateTime)value).Hour + ":" + ((DateTime)value).Minute;
                            else if (col.IsDateTime)
                                row[col.Title] = value.ToString();
                            else
                                row[col.Title] = ((DateTime)value).Day + "/" + ((DateTime)value).Month + "/" + ((DateTime)value).Year;

                        }
                        else if (value is bool)
                        {
                            var activeLanguage = ServiceFactory.ORMService.All<Language>().FirstOrDefault(x => x.IsActive);
                            var culture=(LanguageCulture)activeLanguage.LanguageCulture;
                            var string_culture = culture.ToString();
                            if(string_culture.Contains("ar"))
                                row[col.Title] = (bool) value ? "نعم": "لا";
                            if(string_culture.Contains("en"))
                                row[col.Title] = (bool) value ? "yes": "no";
                        }
                        else if (value is Entity)
                        {
                            row[col.Title] = value.ToString();
                            try
                            {
                                var entityid = ((Entity)value).Id;
                                var typyName = value.GetType().BaseType.FullName;
                                var selectedDetail = (Entity)typyName.ToType().GetById(entityid);
                                foreach (var pro in value.GetType().GetProperties())
                                {
                                    if (pro.Name == "NameForDropdown")
                                    {
                                        var nameForDropdown = selectedDetail.GetPropertyValue("NameForDropdown");
                                        row[col.Title] = nameForDropdown;
                                    }

                                    else if (pro.Name == "Name")
                                    {
                                        var nameForDropdown = selectedDetail.GetPropertyValue("Name");
                                        row[col.Title] = nameForDropdown;

                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                var s = ex.Message;
                            }


                        }


                        else
                            row[col.Title] = value.ToString();
                        j++;
                    }
                }
                try
                {
                    table.Rows.Add(row);
                }
                catch (Exception)
                {

                    throw;
                }

            }


            //var test=    ConvertListToDataTable(listDic);


            GridView gv = new GridView();
            gv.DataSource = table;
            gv.DataBind();
            //count++;
            //sampleWorkbook.SaveAs("D:\\Data" + count + ".csv");
            //sampleWorkbook.Close();
            //xlApplication.Quit();
            //return Json(new { FileName = "D:\\Data" + count + ".csv " }, JsonRequestBehavior.AllowGet);
            string FileName = previous[0].Name;
            string  FileType = "xls";
            var FileKey = 2;




            Session["dataModels"] = gv;
            //Session["FileNameXls"]=
            //return File(output, "application/vnd.ms-excel", string.Format("{0}.csv", "aaa"));
            return Json(new { FileKey, FileType, FileName }, JsonRequestBehavior.AllowGet);


        }


        public ActionResult GetFile(string fileKey, string fileType, string fileName)
        {
            if (Session["dataModels"] != null)
            {
                return new DownloadFileActionResult((GridView)Session["dataModels"], fileName+"."+fileType);
            }
           
            return View();

        }

    }
}
