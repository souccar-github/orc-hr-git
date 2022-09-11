using System;
using System.Collections.Generic;
using System.Web;
using  Project.Web.Mvc4.Factories;
using Souccar.Core.Extensions;
using Souccar.Domain.Security;
using Souccar.Infrastructure.Core;
using System.Linq;
using  Project.Web.Mvc4.Helpers;
using  Project.Web.Mvc4.ProjectModels;

namespace Project.Web.Mvc4.Models.GridModel
{
    public enum GridEditorMode
    {
        Batch,
        Popup,
        Inline
    }

    [Serializable]
    public class GridViewModel
    {
        public GridViewModel()
        {
            Height = 380;
            Name = "grid";

            DataFieldName = "Data";
            ErrorFieldName = "Errors";
            TotalCountFieldName = "TotalCount";

            SchemaFields = new List<Field>();

            CurrentViewId = 0;
            Views = new List<View>();
            IsEditable = true;
            IsAddable = true;
            IsDeleteable = true;
            ActionListHandler = "";
            ActionList = new ActionList();
            ToolbarCommands = new List<ToolbarCommand>();
        }
        public const int DefaultPageSize = 10;
        public int Height { get; set; }
        public string Name { get; set; }
        public string TypeFullName { get; set; }
        public string ViewModelTypeFullName { get; set; }

        public string DataFieldName { get; set; }
        public string ErrorFieldName { get; set; }
        public string TotalCountFieldName { get; set; }

        public IList<Field> SchemaFields { get; private set; }

        public int CurrentViewId { get; set; }
        public IList<View> Views { get; private set; }

        public ActionList ActionList { get; set; }
        public string ActionListHandler { get; set; }

        public IList<ToolbarCommand> ToolbarCommands { get; set; }
        public bool IsFromMasterDetail { get; set; }

        #region View
        public IList<string> ViewRoles
        {
            get
            {
                return
                    ServiceFactory.SecurityService.GetAuthorizeTypeRolesForElement(TypeFullName).Where(
                        x => x.AuthorizeType == AuthorizeType.Visible).Select(x => x.Role.Name).ToList();
            }
        }

        public bool AuthorizedToView
        {
            get
            {
                var userRole = ServiceFactory.SecurityService.RolesForUser(HttpContext.Current.User.Identity.Name);
                return (HttpContext.Current.User.Identity.IsAuthenticated && userRole != null &&
                       ViewRoles.Intersect(userRole.Select(x => x.Name)).Any());
            }
        }
        #endregion
      
        #region Edit
        public bool IsEditable { get; set; }
        public bool AuthorizedToEdit
        {
            get
            {
                var userRole = ServiceFactory.SecurityService.RolesForUser(HttpContext.Current.User.Identity.Name);
                return (HttpContext.Current.User.Identity.IsAuthenticated && userRole != null && IsEditable &&
                        EditRoles.Intersect(userRole.Select(x => x.Name)).Any());
            }
        }
        public IList<string> EditRoles
        {
            get
            {
                return
                    ServiceFactory.SecurityService.GetAuthorizeTypeRolesForElement(TypeFullName).Where(
                        x => x.AuthorizeType == AuthorizeType.Editable).Select(x => x.Role.Name).ToList();
            }
        }
        #endregion
        
        #region Add
        public bool IsAddable { get; set; }
        public IList<string> AddRoles
        {
            get
            {
                return
                    ServiceFactory.SecurityService.GetAuthorizeTypeRolesForElement(TypeFullName).Where(
                        x => x.AuthorizeType == AuthorizeType.Addable).Select(x => x.Role.Name).ToList();
            }
        }

        public bool AuthorizedToAdd
        {
            get
            {
                var userRole = ServiceFactory.SecurityService.RolesForUser(HttpContext.Current.User.Identity.Name);
                return (HttpContext.Current.User.Identity.IsAuthenticated && userRole != null && IsAddable &&
                       AddRoles.Intersect(userRole.Select(x => x.Name)).Any());
            }
        }
        #endregion

        #region Delete
        public bool IsDeleteable { get; set; }
        public IList<string> DeleteRoles
        {
            get
            {
                return
                    ServiceFactory.SecurityService.GetAuthorizeTypeRolesForElement(TypeFullName).Where(
                        x => x.AuthorizeType == AuthorizeType.Deleteable).Select(x => x.Role.Name).ToList();
            }
        }
        public bool AuthorizedToDelete
        {
            get
            {
                var userRole = ServiceFactory.SecurityService.RolesForUser(HttpContext.Current.User.Identity.Name);
                return (HttpContext.Current.User.Identity.IsAuthenticated && userRole != null && IsDeleteable &&
                       DeleteRoles.Intersect(userRole.Select(x => x.Name)).Any());
            }
        }
        #endregion
      
        #region Localized property

        public string EntityTitle { get; set; }


        public string FilterDropdownLabel
        {
            get { return GlobalResource.Select; }
        }


        public string Actions
        {
            get { return getLocalized(GridModelLocalizationConst.Actions); }
        }

        public string Add
        {
            get { return getLocalized(GridModelLocalizationConst.Add); }
        }

        public string Edit
        {
            get { return getLocalized(GridModelLocalizationConst.Edit); }
        }

        public string Create
        {
            get { return getLocalized(GridModelLocalizationConst.Create); }
        }

        public string Delete
        {
            get { return getLocalized(GridModelLocalizationConst.Delete); }
        }

        public string Update
        {
            get { return getLocalized(GridModelLocalizationConst.Update); }
        }

        public string Cancel
        {
            get { return getLocalized(GridModelLocalizationConst.Cancel); }
        }

        public string SaveAndNew
        {
            get { return getLocalized(GridModelLocalizationConst.SaveAndNew); }
        }

        public string SaveAndCopy
        {
            get { return getLocalized(GridModelLocalizationConst.SaveAndCopy); }
        }
        public string ViewIntormation
        {
            get { return getLocalized(GridModelLocalizationConst.ViewIntormation); }
        }

        public string EQ
        {
            get { return getLocalized(GridModelLocalizationConst.EQ); }//IsEqualTo
        }

        public string NET
        {
            get { return getLocalized(GridModelLocalizationConst.NET); }//NotEqualTo
        }

        public string GTE
        {
            get { return getLocalized(GridModelLocalizationConst.GTE); }//"Is greater than or equal to"
        }

        public string GT
        {
            get { return getLocalized(GridModelLocalizationConst.GT); }//"Is greater than"
        }

        public string LTE
        {
            get { return getLocalized(GridModelLocalizationConst.LTE); }//"Is less than or equal to"
        }

        public string LT
        {
            get { return getLocalized(GridModelLocalizationConst.LT); }//"Is less than"
        }

        public string Display
        {
            get { return getLocalized(GridModelLocalizationConst.Display); }//"{0} - {1} of {2} items"
        }

        public string Empty
        {
            get { return getLocalized(GridModelLocalizationConst.Empty); }//"No items to display"
        }

        public string Page
        {
            get { return getLocalized(GridModelLocalizationConst.Page); }//"Page"
        }

        public string Of
        {
            get { return getLocalized(GridModelLocalizationConst.Of); }//""of {0}""
        }

        public string ItemsPerPage
        {
            get { return getLocalized(GridModelLocalizationConst.ItemsPerPage); }//ItemsPerPage
        }

        public string First
        {
            get { return getLocalized(GridModelLocalizationConst.First); }//"First
        }

        public string Last
        {
            get { return getLocalized(GridModelLocalizationConst.Last); }//"Last
        }

        public string Next
        {
            get { return getLocalized(GridModelLocalizationConst.Next); }//"Next
        }

        public string Previous
        {
            get { return getLocalized(GridModelLocalizationConst.Previous); }//"Previous
        }

        public string Refresh
        {
            get { return getLocalized(GridModelLocalizationConst.Refresh); }//"Refresh
        }

        public string Confirmation
        {
            get { return getLocalized(GridModelLocalizationConst.AreYouSureYouWantToDeleteThisRecord); }//Are you sure you want to delete this record?
        }

        public string Info
        {
            get { return getLocalized(GridModelLocalizationConst.Information); }//"Show items with value that:"
        }

        public string Filter
        {
            get { return getLocalized(GridModelLocalizationConst.Filter); }//Filter
        }
        public string FilterBy
        {
            get { return getLocalized(GridModelLocalizationConst.FilterBy); }//Filter
        }
        public string Clear
        {
            get { return getLocalized(GridModelLocalizationConst.Clear); }//Clear
        }

        public string IsTrue
        {
            get { return getLocalized(GridModelLocalizationConst.IsTrue); }//IsTrue
        }

        public string IsFalse
        {
            get { return getLocalized(GridModelLocalizationConst.IsFalse); }//IsFalse
        }

        public string And
        {
            get { return getLocalized(GridModelLocalizationConst.And); }//and
        }

        public string Or
        {
            get { return getLocalized(GridModelLocalizationConst.Or); }//or
        }

        public string StartsWith
        {
            get { return getLocalized(GridModelLocalizationConst.StartsWith); }//StartsWith
        }

        public string EndsWith
        {
            get { return getLocalized(GridModelLocalizationConst.EndsWith); }//EndsWith
        }

        public string Contains
        {
            get { return getLocalized(GridModelLocalizationConst.Contains); }//Contains
        }

        public string EditWindowTitle
        {
            get { return getLocalized(GridModelLocalizationConst.EditWindowTitle); }//
        }

        public string AddTitle
        {
            get { return getLocalized(GridModelLocalizationConst.AddTitle); }//
        }

        public string ClearFiltering
        {
            get { return getLocalized(GridModelLocalizationConst.ClearFiltering); }//
        }
        public string ExportToCSV
        {
            get { return getLocalized(GridModelLocalizationConst.ExportToCSV); }//
        }

        public string ClearSorting
        {
            get { return getLocalized(GridModelLocalizationConst.ClearSorting); }//
        }

        public string View
        {
            get { return getLocalized(GridModelLocalizationConst.View); }//
        }
        public string NameTitle
        {
            get { return getLocalized(GridModelLocalizationConst.NameTitle); }//
        }
        public string ViewsTitle
        {
            get { return GlobalResource.ViewsTitle; }//
        }
        public string InvalidDateMessage
        {
            get { return GlobalResource.InvalidDateMessage; }//
        }
        public string InvalidTimeMessage
        {
            get { return GlobalResource.InvalidTimeMessage; }//
        }
        public string ResError
        {
            get { return GlobalResource.Error; }//
        }

        public string ExceptionMessage
        {
            get { return GlobalResource.ExceptionMessage; }//
        }

        public string ResOk
        {
            get { return GlobalResource.Ok; }//
        }

        public string Select
        {
            get { return GlobalResource.Select; }//
        }
        private string getLocalized(string key)
        {
            return ServiceFactory.LocalizationService.GetResource(GridModelLocalizationConst.ResourceGroupName + "_" + key) ?? key.ToCapitalLetters();
        }

        #endregion
    }
}
