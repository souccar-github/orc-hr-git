using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Office.Interop.Excel;
using Souccar.Domain.DomainModel;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;

namespace project.Web.Mvc4.Areas.ReportGenerator.Models
{
    public class ReportTemplateViewModel : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            //add
            model.ViewModelTypeFullName = typeof(ReportTemplateViewModel).FullName;
            //model.Views[0].EditorTemplate = "ReportTemplateEditor";
            model.Views[0].EditHandler = "initializeReportTemplateEditor";

        //    foreach (var view in model.Views)
        //    {
        //        view.EditorMode = GridEditorMode.Popup.ToString().ToLower();
        //        view.EditorTemplate = "ReportTemplateEditor";
        //        view.EditHandler = "initializeReportTemplateEditor";
        //        view.CreateUrl = "ReportTemplate/Create";
        //        view.UpdateUrl = "ReportTemplate/Update";
        //        view.DestroyUrl = "ReportTemplate/Delete";
        //        // generateReportAdditionalFields(model, columnOrder);
        //    }
        //    generateReportAdditionalFields(model, 0);
        }
        
        public override void BeforeInsert(RequestInformation requestInformation, Entity entity,
            string customInformation = null)
        {
        }
        //private static void generateReportAdditionalFields(GridViewModel model, int columnOrder)
        //{
        //    generateSingleField(model, columnOrder++, typeof(bool), "ShowDateTime", ColumnType.Simple);
        //    generateSingleField(model, columnOrder++, typeof(bool), "ShowUserName", ColumnType.Simple);
        //    generateSingleField(model, columnOrder++, typeof(bool), "ShowPageNumber", ColumnType.Simple);
        //    generateSingleField(model, columnOrder++, typeof(bool), "ShowHeader", ColumnType.Simple);
        //    generateSingleField(model, columnOrder++, typeof(bool), "ShowFooter", ColumnType.Simple);
        //}
        //private static void generateSingleField(GridViewModel model, int columnOrder, Type type, string fieldName, ColumnType columnType)
        //{
        //    var field = new Field()
        //    {
        //        Name = fieldName,
        //        Type = GridViewModelHelper.GetFieldTypeName(type).ToString().ToLower(),
        //        Editable = true
        //    };

        //    model.SchemaFields.Add(field);

        //    var column1 = new Column()
        //    {
        //        Title = fieldName,
        //        Type = columnType.ToString(),
        //        Order = columnOrder,
        //        FieldName = fieldName,
        //        Sortable = true,
        //        Filterable = true,
        //        Hidden = false
        //    };

        //    model.Views[0].Columns.Add(column1);
        //    model.Views[1].Columns.Add(column1);
        //}

    }
}