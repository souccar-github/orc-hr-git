using System;
using System.Collections.Generic;
using System.Linq;

using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Reflector;
using Souccar.Domain.Extensions;


using Souccar.Domain.Validation;
using Project.Web.Mvc4.Extensions;

using Souccar.Core.Extensions;
using Souccar.Infrastructure.Extenstions;

using Souccar.Infrastructure.Helpers;
using Project.Web.Mvc4.Areas;
using Project.Web.Mvc4.ProjectModels;
using Project.Web.Mvc4.ProjectModels;
using static Project.Web.Mvc4.Models.RequestInformation.Navigation;

namespace Project.Web.Mvc4.Factories
{
    public class GridViewModelFactory
    {
        //After Apply Master Detail Feature
        public static int DefaultColumnWidth = 165;
        public static int ControlsCountForDefaultWindow = 8;

        /// <summary>
        /// Update:Yaseen Alrefaee
        /// Add order Localization security
        /// </summary>
        /// <param name="type"></param>
        /// <param name="requestInformation"></param>
        /// <returns></returns>
        public static GridViewModel Create(Type type, RequestInformation requestInformation)
        {

            var modelName = string.Empty;
            var orginalModelName = string.Empty;
            var detailname = string.Empty;
            var isdetail = false;
            var _Previous = new Step();
            if (requestInformation != null)
            {

                var count = requestInformation.NavigationInfo.Previous.Count;
                if (count > 1)
                    if (requestInformation.NavigationInfo.Previous[count - 1] != null && requestInformation.NavigationInfo.Previous[count - 1].Name != null)
                    {
                        detailname = requestInformation.NavigationInfo.Previous[count - 1].Name;
                        _Previous = requestInformation.NavigationInfo.Previous[count - 1];
                        _Previous.IsMasterContainsThisDetail = type.GetIsDetailHidden() == false ? true : false;
                        isdetail = true;
                    }

                string[] tokens = type.FullName.Split('.');
                orginalModelName = tokens[2];
                modelName = requestInformation.NavigationInfo.Module.Name;
                var details = requestInformation.NavigationInfo.Next;
                foreach (var detail in details)
                {
                    try
                    {
                        var detailName = detail.Name;
                        if (detailname != null)
                        {
                            var _detail = type.GetProperty(detailName);
                            var typeOfDetail = _detail.PropertyType;
                            var fullName = typeOfDetail.FullName;
                            var _strings = fullName.Split('[');
                            var __strings = _strings[2].Split(',');
                            var _type = Type.GetType(__strings[0] + "," + __strings[1]);
                            if (!_type.GetIsDetailHidden())
                            {
                                requestInformation.NavigationInfo.Previous.Last().IsMasterContainsDetailsWithSameInterface = true;
                                continue;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }

                }

            }
            var classTree = ClassTreeFactory.Create(type);

            #region View

            var view1 = new View()
            {
                Id = 0,
                Title = GlobalResource.DefaultViewTitle,
                Type = ViewType.GridView,
                IsDetailOutSideGrid = classTree.Type.GetIsDetailOutSideGrid(),
            };

            var view2 = new View()
            {
                Id = 1,
                Title = GlobalResource.SimpleViewTitle,
                Type = ViewType.GridView,
                IsDetailOutSideGrid = classTree.Type.GetIsDetailOutSideGrid()
            };

            #endregion

            var model = new GridViewModel() { TypeFullName = type.FullName };
            model.EntityTitle = type.GetLocalized();
            model.Views.Add(view1);
            //model.Views.Add(view2);
            model.IsFromMasterDetail = requestInformation != null ? requestInformation.NavigationInfo
                .Previous[requestInformation.NavigationInfo.Previous.Count - 1].IsFromMasterDetail : false;
            UpdateToolbarAndActionList(model, type, _Previous);

            List<ValidationRules> validationRules = type.GetValidators();

            #region Simple Properties

            foreach (var property in classTree.SimpleProperties)
            {
                var prop = type.GetProperty(property.Name);
                var roleEditable = SecurityHelper.IsAuthorizableRolEdit(property.Name, isdetail, type.FullName, modelName);
                var field = new Field()
                {
                    Name = property.Name,
                    Type = GridViewModelHelper.GetFieldTypeName(property.PropertyType, prop).ToString().ToLower(),
                    Editable = prop.CanWrite && !prop.GetIsNonEditable() && roleEditable
                };

                //if (columnOrder == 1)
                //    view2.SortFields.Add(field.Name, GridSortDirection.Desc.ToString().ToLower());
                //else if (columnOrder == 2)
                //    view2.SortFields.Add(field.Name, GridSortDirection.Asc.ToString().ToLower());

                var title = property.GetLocalized(classTree.Type);

                var nativeValidationRules = validationRules.SingleOrDefault(x => x.PropertyName == property.Name);
                var isRequired = nativeValidationRules != null &&
                                 nativeValidationRules.Validators.Any(x => x.ValidatorType == ValidatorType.Required) && roleEditable;
                generateValidationRules(field, title, nativeValidationRules);
                model.SchemaFields.Add(field);

                var columnType = ColumnType.Simple;
                if (nativeValidationRules != null)
                {
                    var validator =
                        nativeValidationRules.Validators.SingleOrDefault(v => v.ValidatorType == ValidatorType.MaxLength);
                    if (validator != null)
                    {
                        var max = Convert.ToInt32(validator.ValidatorRules.First().Parameters.First());
                        if (max == GlobalResorce.GetValidationMultiLinesStringMaxLength())
                            columnType = ColumnType.TextArea;
                    }
                }

                var column = new Column()
                {
                    Width =
                        prop.GetWidth() != 0 ? prop.GetWidth() : DefaultColumnWidth,
                    Title = title,
                    Order = prop.GetOrder(),
                    GroupName = prop.GetGroupName(),
                    FieldName = property.Name,
                    Type = columnType.ToString(),
                    ShowCommaSeparator = GridViewModelHelper.ShowCommaSeparatorForType(prop),
                    Step = prop.GetStep(),
                    Sortable = prop.CanWrite,
                    Filterable = prop.CanWrite,
                    IsRequired = isRequired,
                    Editable = prop.CanWrite && !prop.GetIsNonEditable(),
                    Hidden = prop.GetIsHidden() || !roleEditable,
                    IsFile = prop.GetIsFile(),
                    IsDateTime = prop.GetIsDateTime(),
                    IsTime = prop.GetIsTime(),
                    FileAcceptExtension = prop.GetAcceptExtension(),
                    FileSize = prop.GetFileSize(),
                    ImagePath = prop.GetImagePath(),
                    DetailName = detailname,
                    DefaultImageName = prop.GetDefaultImageName()
                };

                view1.Columns.Add(column);
                view2.Columns.Add(column);
                view2.OrderColumns();
            }

            #region Nullabel Date

            foreach (
                var property in classTree.ReferencesProperties.Where(p => p.PropertyType == typeof(DateTime?)).ToList()
                )
            {
                var prop = type.GetProperty(property.Name);
                var roleEditable = SecurityHelper.IsAuthorizableRolEdit(property.Name, isdetail, type.FullName, modelName);
                var field = new Field()
                {
                    Name = property.Name,
                    Type = GridViewModelHelper.GetFieldTypeName(property.PropertyType, prop).ToString().ToLower(),
                    Editable = prop.CanWrite && !prop.GetIsNonEditable() && roleEditable
                };

                //if (columnOrder == 1)
                //    view2.SortFields.Add(field.Name, GridSortDirection.Desc.ToString().ToLower());
                //else if (columnOrder == 2)
                //    view2.SortFields.Add(field.Name, GridSortDirection.Asc.ToString().ToLower());

                var title = property.GetLocalized(classTree.Type);

                var nativeValidationRules = validationRules.SingleOrDefault(x => x.PropertyName == property.Name);
                var isRequired = nativeValidationRules != null && nativeValidationRules.Validators.Any(x => x.ValidatorType == ValidatorType.Required) && roleEditable;
                generateValidationRules(field, title, nativeValidationRules);
                model.SchemaFields.Add(field);

                var columnType = ColumnType.Simple;
                if (nativeValidationRules != null)
                {
                    var validator =
                        nativeValidationRules.Validators.SingleOrDefault(v => v.ValidatorType == ValidatorType.MaxLength);
                    if (validator != null)
                    {
                        var max = Convert.ToInt32(validator.ValidatorRules.First().Parameters.First());
                        if (max == GlobalResorce.GetValidationMultiLinesStringMaxLength())
                            columnType = ColumnType.TextArea;
                    }
                }



                var column = new Column()
                {
                    Width =
                        prop.GetWidth() != 0 ? prop.GetWidth() : DefaultColumnWidth,
                    Title = title,
                    Order = prop.GetOrder(),
                    GroupName = prop.GetGroupName(),
                    FieldName = property.Name,
                    Type = columnType.ToString(),
                    ShowCommaSeparator = GridViewModelHelper.ShowCommaSeparatorForType(prop),
                    Step = prop.GetStep(),
                    Sortable = prop.CanWrite,
                    Filterable = prop.CanWrite,
                    IsRequired = isRequired,
                    Editable = prop.CanWrite && !prop.GetIsNonEditable(),
                    Hidden = prop.GetIsHidden() || !roleEditable,
                    IsFile = prop.GetIsFile(),
                    IsDateTime = prop.GetIsDateTime(),
                    IsTime = prop.GetIsTime(),
                    FileAcceptExtension = prop.GetAcceptExtension(),
                    FileSize = prop.GetFileSize(),
                    ImagePath = prop.GetImagePath(),
                    DetailName = detailname,
                    DefaultImageName = prop.GetDefaultImageName()
                };

                view1.Columns.Add(column);
                view2.Columns.Add(column);
                view2.OrderColumns();

            }

            #endregion Nullabel Date

            #endregion

            #region Index Prorerty

            foreach (var property in classTree.ReferencesProperties.Where(p => p.PropertyType.IsIndex()).ToList())
            {
                var roleEditable = SecurityHelper.IsAuthorizableRolEdit(property.Name, isdetail, type.FullName, modelName);
                var prop = type.GetProperty(property.Name);
                var field = new Field()
                {
                    Name = property.Name,
                    Type = FieldType.Complex.ToString().ToLower(),
                    Editable = prop.CanWrite && !prop.GetIsNonEditable() && roleEditable
                };

                var title = property.GetLocalized(classTree.Type);
                var nativeValidationRules = validationRules.SingleOrDefault(x => x.PropertyName == property.Name);
                var isRequired = nativeValidationRules != null &&
                                 nativeValidationRules.Validators.Any(x => x.ValidatorType == ValidatorType.Required) && roleEditable;

                generateValidationRules(field, title, nativeValidationRules);
                model.SchemaFields.Add(field);

                var columnType = ColumnType.DropDown;
                //if (referencesProperty.PropertyType == typeof(Nationality))
                //{
                //    columnType = ColumnType.AutoComplete;                    
                //}
                var readUrlForIndex = prop.GetReferenceReadUrl();
                if (string.IsNullOrEmpty(readUrlForIndex))
                    readUrlForIndex = "Index/ReadToList/";

                var column = new Column()
                {
                    Width =
                        prop.GetWidth() != 0 ? prop.GetWidth() : DefaultColumnWidth,
                    Title = title,
                    Order = prop.GetOrder(),
                    GroupName = prop.GetGroupName(),
                    FieldName = property.Name,
                    Type = columnType.ToString(),
                    IndexName = property.PropertyType.Name,
                    TypeFullName = property.PropertyType.FullName,
                    CreateUrl = "Index/CreateSingle/",
                    ReadUrl = readUrlForIndex,
                    ShowAddButton = prop.CanWrite && SecurityHelper.ShowAddButton(property.PropertyType.FullName),
                    Sortable = false,
                    Filterable = prop.CanWrite,
                    IsRequired = isRequired,
                    Hidden = prop.GetIsHidden() || !roleEditable,
                    Editable = prop.CanWrite && !prop.GetIsNonEditable(),
                    DetailName = detailname
                };

                column.ValueField = "Id";
                //var simpleProperty = property.ClassTree.SimpleProperties.FirstOrDefault(s => s.IsPrimaryKey);
                //if (simpleProperty != null)
                //{
                //    column.ValueField = simpleProperty.Name;
                //}

                column.TextField = "Name";
                //simpleProperty = property.ClassTree.SimpleProperties.FirstOrDefault(s => !s.IsPrimaryKey);
                //if (simpleProperty != null)
                //{
                //    column.TextField = simpleProperty.Name;
                //}

                view1.Columns.Add(column);
            }

            #endregion

            #region Enum Property

            foreach (var property in classTree.ReferencesProperties.Where(p => p.PropertyType.IsEnum()).ToList())
            {
                var roleEditable = SecurityHelper.IsAuthorizableRolEdit(property.Name, isdetail, type.FullName, modelName);
                var prop = type.GetProperty(property.Name);
                var field = new Field()
                {
                    Name = property.Name,
                    Type = FieldType.Complex.ToString().ToLower(),
                    Editable = prop.CanWrite && !prop.GetIsNonEditable() && roleEditable
                };

                var title = property.GetLocalized(classTree.Type);
                var nativeValidationRules = validationRules.SingleOrDefault(x => x.PropertyName == property.Name);
                var isRequired = nativeValidationRules != null &&
                                 nativeValidationRules.Validators.Any(x => x.ValidatorType == ValidatorType.Required) && roleEditable;

                generateValidationRules(field, title, nativeValidationRules);
                model.SchemaFields.Add(field);
                var readUrlForEnum = prop.GetReferenceReadUrl();
                if (string.IsNullOrEmpty(readUrlForEnum))
                    readUrlForEnum = "Enum/ReadToList/";

                var column = new Column()
                {
                    Width = prop.GetWidth() != 0 ? prop.GetWidth() : DefaultColumnWidth,
                    Title = title,
                    Order = prop.GetOrder(),
                    GroupName = prop.GetGroupName(),
                    ValueField = "Id",
                    TextField = "Name",
                    FieldName = property.Name,
                    Type = ColumnType.DropDown.ToString(),
                    IndexName = property.PropertyType.Name,
                    TypeFullName = property.PropertyType.FullName,
                    ReadUrl = readUrlForEnum,
                    ShowAddButton = false,
                    Sortable = prop.CanWrite,
                    Filterable = prop.CanWrite,
                    IsRequired = isRequired,
                    Hidden = type.GetProperty(property.Name).GetIsHidden() || !roleEditable,
                    Editable = prop.CanWrite && !prop.GetIsNonEditable(),
                    DetailName = detailname

                };

                view1.Columns.Add(column);
            }

            #endregion

            #region Reference Property

            foreach (
                var property in
                    classTree.ReferencesProperties.Where(
                        p =>
                            p.PropertyType != typeof(DateTime?) && !p.PropertyType.IsEnum() &&
                            !p.PropertyType.IsIndex()).ToList())
            {
                var roleEditable = SecurityHelper.IsAuthorizableRolEdit(property.Name, isdetail, type.FullName, modelName);
                var prop = type.GetProperty(property.Name);

                if (!type.GetProperty(property.Name).GetIsReference())
                    continue;
                var url = prop.GetReferenceReadUrl();
                var cascadeFrom = prop.GetCascadeFrom();
                if (string.IsNullOrEmpty(url))
                {
                    AddRefFieldWithCascade(model, property.Name, cascadeFrom, roleEditable, detailname);
                }
                else
                {
                    AddRefFieldWithCascade(model, property.Name, url, cascadeFrom, roleEditable, detailname);
                }

            }

            #endregion

            foreach (var view in model.Views)
            {
                view.ShowTwoColumns = view.Columns.Count(x => !x.Hidden) >= ControlsCountForDefaultWindow;
            }

            foreach (var column in model.Views.SelectMany(view => view.Columns))
            {
                var field = model.SchemaFields.SingleOrDefault(x => x.Name == column.FieldName);
                field.Editable = field.Editable && !column.Hidden;
                column.Editable = column.Editable && !column.Hidden;
            }

            if (requestInformation != null)
            {
                var modelAdjustment = FactoryModelAdjustment.Create(orginalModelName);
                var viewmodel = modelAdjustment.AdjustGridModel(type.Name);
                if (_Previous.IsFromMasterDetail)
                    viewmodel.CustomizeDetailGridModelForMasterDetail(model, type, requestInformation);
                else
                    viewmodel.CustomizeGridModel(model, type, requestInformation);


            }

            model.ActionList.OrderCommands();
            view1.OrderColumns();

            return model;
        }

        public static void UpdateToolbarAndActionList(GridViewModel model, Type type, Step _Previous)
        {

            #region ToolbarCommand
            model.ToolbarCommands.Clear();
            if ((model.AuthorizedToAdd && type.GetIsDetailHidden()) || (model.AuthorizedToAdd && !type.GetIsDetailHidden() && model.IsFromMasterDetail))
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

            model.ToolbarCommands.Add(new ToolbarCommand()
            {
                Text = model.ExportToCSV,
                Handler = "exportDateToCSV",
                Additional = true
            });

            model.ToolbarCommands.Add(new ToolbarCommand()
            {
                Template = "GridViewSelector"
            });

            model.ToolbarCommands.Add(new ToolbarCommand()
            {
                Template = "AdditionalToolbarCommands"
            });
            #endregion

            #region ActionListCommand
            model.ActionList = new ActionList();
            if (_Previous.IsFromMasterDetail)
            {
                model.ActionList.Commands.Add(new ActionListCommand()
                {
                    GroupId = 2,
                    StyleClass = "action-list-view",
                    ImageClass = "action-list-img-view",
                    Order = 3,
                    HandlerName = "showDetailInformation",
                    Name = model.View,
                    ShowCommand = true
                });
                if (model.AuthorizedToDelete)
                {
                    model.ActionList.Commands.Add(new ActionListCommand()
                    {
                        GroupId = 2,
                        StyleClass = "action-list-destroy",
                        ImageClass = "action-list-img-destroy",
                        Order = 2,
                        HandlerName = "destroyDetail",
                        Name = model.Delete,
                        ShowCommand = true
                    });
                }
                if (model.AuthorizedToEdit)
                {
                    model.ActionList.Commands.Add(new ActionListCommand()
                    {
                        GroupId = 2,
                        StyleClass = "action-list-update",
                        ImageClass = "action-list-img-update",
                        Order = 1,
                        HandlerName = "updateDetail",
                        Name = model.Edit,
                        ShowCommand = true
                    });
                }
            }
            else
            {
                model.ActionList.Commands.Add(new ActionListCommand()
                {
                    GroupId = 2,
                    StyleClass = "action-list-view",
                    ImageClass = "action-list-img-view",
                    Order = 3,
                    HandlerName = "showInformation",
                    Name = model.View,
                    ShowCommand = true
                });
                if ((model.AuthorizedToDelete && type.GetIsDetailHidden()) || (model.AuthorizedToDelete && !type.GetIsDetailHidden() && model.IsFromMasterDetail))
                {
                    model.ActionList.Commands.Add(new ActionListCommand()
                    {
                        GroupId = 2,
                        StyleClass = "action-list-destroy",
                        ImageClass = "action-list-img-destroy",
                        Order = 2,
                        HandlerName = "destroy",
                        Name = model.Delete,
                        ShowCommand = true
                    });
                }
                if ((model.AuthorizedToEdit && type.GetIsDetailHidden()) || (model.AuthorizedToEdit && !type.GetIsDetailHidden() && model.IsFromMasterDetail))
                {
                    model.ActionList.Commands.Add(new ActionListCommand()
                    {
                        GroupId = 2,
                        StyleClass = "action-list-update",
                        ImageClass = "action-list-img-update",
                        Order = 1,
                        HandlerName = "update",
                        Name = model.Edit,
                        ShowCommand = true
                    });
                }
            }

            foreach (var command in CommandFactory.GetActionListCommands(type))
            {
                if (command.Authorized)
                    model.ActionList.Commands.Add(command);
            }

            model.ActionList.GroupsCount = 2;
            #endregion

        }


        /// <summary>
        /// Author: Yaseen Alrefaee
        /// </summary>
        /// <param name="gridViewModel"></param>
        /// <param name="propName"></param>
        private static void AddRefField(GridViewModel gridViewModel, string propName)
        {
            AddRefField(gridViewModel, propName, "Reference/ReadToList/");
        }



        /// <summary>
        /// Author :Yaseen Alrefaee
        /// </summary>
        /// <param name="gridViewModel"></param>
        /// <param name="propName"></param>
        /// <param name="readUrl"></param>
        public static void AddRefField(GridViewModel gridViewModel, string propName, string readUrl)
        {
            var prop = gridViewModel.TypeFullName.ToType().GetProperty(propName);
            AddRefField(gridViewModel, new List<int>() { 0 }, propName, prop.GetTitle(), prop.PropertyType.FullName, DefaultColumnWidth, prop.GetOrder(), readUrl, "");
        }

        /// <summary>
        /// Author: Yaseen Alrefaee 
        /// </summary>
        /// <param name="gridViewModel"></param>
        /// <param name="propName"></param>
        /// <param name="cascadeFrom"></param>
        private static void AddRefFieldWithCascade(GridViewModel gridViewModel, string propName, string cascadeFrom, bool roleEditable, string detailname)
        {
            AddRefFieldWithCascade(gridViewModel, propName, "Reference/ReadToList/", cascadeFrom, roleEditable, detailname);
        }

        /// <summary>
        /// Author: Yaseen Alrefaee 
        /// </summary>
        /// <param name="gridViewModel"></param>
        /// <param name="propName"></param>
        /// <param name="readUrl"></param>
        private static void AddRefFieldWithCascade(GridViewModel gridViewModel, string propName, string readUrl, string cascadeFrom, bool roleEditable, string detailname)
        {
            var prop = gridViewModel.TypeFullName.ToType().GetProperty(propName);

            AddRefField(gridViewModel, new List<int>() { 0 }, propName, prop.GetTitle(), prop.PropertyType.FullName,
                DefaultColumnWidth, prop.GetOrder(), readUrl, cascadeFrom, detailname, roleEditable);
        }



        /// <summary>
        /// Author: Yaseen Alrefaee 
        /// </summary>
        /// <param name="gridViewModel"></param>
        /// <param name="viewsIds"></param>
        /// <param name="columnName"></param>
        /// <param name="columnTitlte"></param>
        /// <param name="typeFullName"></param>
        /// <param name="columnWidth"></param>
        /// <param name="columnOrder"></param>
        /// <param name="readUrl"></param>
        private static void AddRefField(GridViewModel gridViewModel, List<int> viewsIds, string columnName, string columnTitle, string typeFullName, int columnWidth, int columnOrder, string readUrl, string cascadeFrom, string detailname = null, bool roleEditable = true)
        {
            var prop = gridViewModel.TypeFullName.ToType().GetProperty(columnName);
            if (!roleEditable)
            {
                readUrl = "";
                cascadeFrom = "";
            }
            var field = new Field()
            {
                Name = columnName,
                Type = FieldType.Complex.ToString().ToLower(),
                Editable = prop.CanWrite && !prop.GetIsNonEditable() && roleEditable
            };
            var validationRules = gridViewModel.TypeFullName.ToType().GetValidators();
            var nativeValidationRules = validationRules.SingleOrDefault(x => x.PropertyName == columnName);
            var isRequired = nativeValidationRules != null && nativeValidationRules.Validators.Any(x => x.ValidatorType == ValidatorType.Required) && roleEditable;

            generateValidationRules(field, columnTitle, nativeValidationRules);

            gridViewModel.SchemaFields.Add(field);

            //if (requestInformation.NavigationInfo.Previous.Count > 1)
            //    if (requestInformation.NavigationInfo.Previous[1].Name != null)
            //        detailname = requestInformation.NavigationInfo.Previous[1].Name;
            var column = new Column()
            {
                Width = columnWidth,
                Title = columnTitle,
                Order = columnOrder,
                FieldName = columnName,
                GroupName = prop.GetGroupName(),
                Type = ColumnType.DropDown.ToString(),
                IndexName = typeFullName,
                TypeFullName = typeFullName,
                CreateUrl = "",
                ReadUrl = readUrl,
                ShowAddButton = false,
                ShowInfoButton = true,
                Sortable = false,
                Filterable = prop.CanWrite,
                CascadeFrom = cascadeFrom,
                HasParent = !string.IsNullOrEmpty(cascadeFrom),
                IsRequired = isRequired,
                Hidden = !roleEditable,
                Editable = prop.CanWrite && !prop.GetIsNonEditable(),
                DetailName = detailname

            };
            column.ValueField = "Id";
            column.TextField = "Name";
            foreach (var id in viewsIds)
            {
                gridViewModel.Views[id].Columns.Add(column);
            }
        }


        private static void generateReportAdditionalFields(GridViewModel model, int columnOrder)
        {
            generateSingleField(model, columnOrder++, typeof(bool), "ShowDateTime", ColumnType.Simple);
            generateSingleField(model, columnOrder++, typeof(bool), "ShowUserName", ColumnType.Simple);
            generateSingleField(model, columnOrder++, typeof(bool), "ShowPageNumber", ColumnType.Simple);
            generateSingleField(model, columnOrder++, typeof(bool), "ShowHeader", ColumnType.Simple);
            generateSingleField(model, columnOrder++, typeof(bool), "ShowFooter", ColumnType.Simple);
        }

        private static void generateSingleField(GridViewModel model, int columnOrder, Type type, string fieldName, ColumnType columnType)
        {

            var field = new Field()
            {
                Name = fieldName,
                Type = GridViewModelHelper.GetFieldTypeName(type).ToString().ToLower(),
                Editable = true
            };

            model.SchemaFields.Add(field);

            var column1 = new Column()
            {
                Title = fieldName,
                Type = columnType.ToString(),
                Order = columnOrder,
                FieldName = fieldName,
                Sortable = true,
                Filterable = true,
                Hidden = false
            };

            model.Views[0].Columns.Add(column1);
            //model.Views[1].Columns.Add(column1);
        }

        private static void generateValidationRules(Field field, string fieldTitle, ValidationRules rules)
        {
            if (rules == null)
                return;
#warning يجب الحذف بعد معالجة المشكلة في الواجهة بعد تغيير اللغة
            if (field.Type.Equals(FieldType.Date.ToString().ToLower()))
                return;
            foreach (var validator in rules.Validators)
            {
                Dictionary<string, object> values = null;
                switch (validator.ValidatorType)
                {
                    case ValidatorType.Required:
                        values = new Dictionary<string, object>();
                        values["message"] = validator.ValidatorRules[0].Message.Replace("{PropertyName}", fieldTitle);
                        field.ValidationRules.Add(GridViewModelHelper.GetValidationTypeName(validator.ValidatorType), values);
                        continue;
                    case ValidatorType.MaxLength:
                    case ValidatorType.LessThanEqualTo:
                    case ValidatorType.GreaterThanEqualTo:
                        foreach (var validatorRule in validator.ValidatorRules)
                        {
                            if (!validatorRule.IsValue)
                                continue;

                            values = new Dictionary<string, object>();
                            values["value"] = validatorRule.Parameters[0];
                            values["message"] = validatorRule.Message.Replace("{PropertyName}", fieldTitle);
                            field.ValidationRules.Add(GridViewModelHelper.GetValidationTypeName(validator.ValidatorType), values);
                        }
                        continue;
                    case ValidatorType.LessThan:
                        foreach (var validatorRule in validator.ValidatorRules)
                        {
                            if (!validatorRule.IsValue)
                                continue;

                            values = new Dictionary<string, object>();

                            if (field.Type == FieldType.Number.ToString().ToLower())
                                values["value"] = int.Parse(validatorRule.Parameters[0].ToString()) - 1;
                            else if (field.Type == FieldType.Date.ToString().ToLower())
                                values["value"] = validatorRule.Parameters[0].ToString().ParseDateTime().AddDays(-1).ToString();

                            values["message"] = validatorRule.Message.Replace("{PropertyName}", fieldTitle);
                            field.ValidationRules.Add(GridViewModelHelper.GetValidationTypeName(validator.ValidatorType), values);
                        }
                        continue;
                    case ValidatorType.GreaterThan:
                        foreach (var validatorRule in validator.ValidatorRules)
                        {
                            if (!validatorRule.IsValue)
                                continue;

                            values = new Dictionary<string, object>();

                            if (field.Type == FieldType.Number.ToString().ToLower())
                                values["value"] = int.Parse(validatorRule.Parameters[0].ToString()) + 1;
                            else if (field.Type == FieldType.Date.ToString().ToLower())
                                values["value"] = validatorRule.Parameters[0].ToString().ParseDateTime().AddDays(1).ToString();

                            values["message"] = validatorRule.Message.Replace("{PropertyName}", fieldTitle);
                            field.ValidationRules.Add(GridViewModelHelper.GetValidationTypeName(validator.ValidatorType), values);
                        }
                        continue;
                    case ValidatorType.Between:
                        foreach (var validatorRule in validator.ValidatorRules)
                        {
                            if ((!validatorRule.IsValue) || (validatorRule.Parameters.Count != 2))
                                continue;

                            values = new Dictionary<string, object>();
                            values["value"] = validatorRule.Parameters[0];
                            values["message"] = validatorRule.Message.Replace("{PropertyName}", fieldTitle);
                            field.ValidationRules.Add(GridViewModelHelper.GetValidationTypeName(ValidatorType.GreaterThanEqualTo), values);

                            values = new Dictionary<string, object>();
                            values["value"] = validatorRule.Parameters[1];
                            values["message"] = validatorRule.Message.Replace("{PropertyName}", fieldTitle);
                            field.ValidationRules.Add(GridViewModelHelper.GetValidationTypeName(ValidatorType.LessThanEqualTo), values);
                        }
                        continue;
                }
            }
        }
    }


}