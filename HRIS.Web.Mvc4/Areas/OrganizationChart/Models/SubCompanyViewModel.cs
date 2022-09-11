using HRIS.Domain.OrganizationChart.Configurations;
using HRIS.Domain.OrganizationChart.RootEntities;
using  Project.Web.Mvc4.Models;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using  Project.Web.Mvc4.Areas.Grades.Models;
using  Project.Web.Mvc4.Factories;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Extensions;
using  Project.Web.Mvc4.Models.GridModel;
using  Project.Web.Mvc4.Helpers.Resource;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using  Project.Web.Mvc4.Helpers;

namespace Project.Web.Mvc4.Areas.OrganizationChart.Models
{
    public class SubCompanyViewModel : ViewModel
    {
       public override void CustomizeGridModel(Mvc4.Models.GridModel.GridViewModel model, Type type, Mvc4.Models.RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(SubCompanyViewModel).FullName;
            if (requestInformation.NavigationInfo.Previous.Count == 2)
            {
                var col = model.Views[model.CurrentViewId].Columns.SingleOrDefault(x => x.FieldName == "OrganizationChart");
                var field = model.SchemaFields.SingleOrDefault(x => x.Name == "OrganizationChart");
                field.CanEdit = false;
                col.Editable = false;
            }
            model.Views[0].ViewHandler = "SubCompanyViewHandler";
        
    }
        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var subCompany = entity as SubCompany;

            var org = subCompany.Organization;
            var nodeOrg = ServiceFactory.ORMService.All<Node>().Where(x => x.Name == org.Name).First();

            var oldNodeType = ServiceFactory.ORMService.All<NodeType>().Where(x => x.Code == "SUB").FirstOrDefault();
            if (oldNodeType == null)
            {
                NodeType nodeType = new NodeType();
                nodeType.Name = "SUB";
                nodeType.Code = "SUB";
                nodeType.Order = 2;
                nodeType.Save();

                Node subCompanyNode = new Node();
                subCompanyNode.Name = subCompany.Name;
                subCompanyNode.Code = "SUB";
                subCompanyNode.Parent = nodeOrg;
                subCompanyNode.Type = nodeType;
                subCompanyNode.Save();
            }
            else
            {
                Node subCompanyNode = new Node();
                subCompanyNode.Name = subCompany.Name;
                subCompanyNode.Code = "SUB";
                subCompanyNode.Parent = nodeOrg;
                subCompanyNode.Type = oldNodeType;
                subCompanyNode.Save();
            }
        }

        public override void AfterUpdate(RequestInformation requestInformation, Souccar.Domain.DomainModel.Entity entity, IDictionary<string, object> originalState, string customInformation = null)
        {
            var subCompany = entity as SubCompany;

            var subCompNameString = originalState["Name"].ToString();
            var oldSubCompNode = ServiceFactory.ORMService.All<Node>().SingleOrDefault(x => x.Name.Contains(subCompNameString));

            oldSubCompNode.Name = subCompany.Name;
            oldSubCompNode.Type = oldSubCompNode.Type;
            oldSubCompNode.Save();

            subCompany.Save();
        }
        public override void AfterDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var subCompany = entity as SubCompany;

            var subCompanyNode = ServiceFactory.ORMService.All<Node>().Where(x => x.Name == subCompany.Name).First();
            subCompanyNode.Delete();
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var subCompany = entity as SubCompany;
            if (operationType == CrudOperationType.Insert)
            {
                Organization org = ServiceFactory.ORMService.All<Organization>().FirstOrDefault();

                if (org == null)
                {
                    validationResults.Add(new ValidationResult()
                    {
                        Message = string.Format(GlobalResource.OrganizationRequest),
                        Property = null
                    });

                }
                else
                {
                    subCompany.Organization = org;
                }

            }
        }

    }
}