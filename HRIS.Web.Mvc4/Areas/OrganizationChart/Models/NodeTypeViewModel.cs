
using HRIS.Domain.OrganizationChart.Configurations;
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
using Souccar.Infrastructure.Extenstions;

namespace Project.Web.Mvc4.Areas.OrganizationChart.Models
{
    public class NodeTypeViewModel : ViewModel
    {
       public override void CustomizeGridModel(Mvc4.Models.GridModel.GridViewModel model, Type type, Mvc4.Models.RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(NodeTypeViewModel).FullName;
        }



        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var nodeType = entity as NodeType;
            NodeType oldnodeType = ServiceFactory.ORMService.All<NodeType>().FirstOrDefault(x => x.Name == nodeType.Name || x.Code == nodeType.Code);

            if (oldnodeType != null && oldnodeType.Id != nodeType.Id)
            {
                if (oldnodeType.Name == nodeType.Name)
                {
                    var prop = typeof(NodeType).GetProperty("Name");
                    validationResults.Add(
                        new ValidationResult()
                        {
                            Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.AlreadyexistMessage),
                            Property = prop
                        });
                }
                if (oldnodeType.Code == nodeType.Code)
                {
                    var prop = typeof(NodeType).GetProperty("Code");
                    validationResults.Add(
                        new ValidationResult()
                        {
                            Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.AlreadyexistMessage),
                            Property = prop
                        });
                }
            }
        }
        
    }
}