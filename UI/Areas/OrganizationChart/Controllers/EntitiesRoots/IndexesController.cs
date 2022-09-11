#region

using System.Web.Mvc;
using Infrastructure.Entities;
using HRIS.Domain.OrgChart.ValueObjects;
using Service;
using Souccar.Domain.DomainModel;
using StructureMap;
using UI.Helpers.Controllers;
using UI.Helpers.Security;

#endregion

namespace UI.Areas.OrganizationChart.Controllers.EntitiesRoots
{
    public class IndexesControllerOrgChart<T> : PartialViewToStringController where T : class, IAggregateRoot
    {
        public void Permissions()
        {
            if (Key.Status())
            {
                ViewData["CanCreate"] = User.IsInRole("Org_Create");
                ViewData["CanRead"] = User.IsInRole("Org_Read");
                ViewData["CanUpdate"] = User.IsInRole("Org_Update");
                ViewData["CanDelete"] = User.IsInRole("Org_Delete");
            }
            else
            {
                ViewData["CanCreate"] = true;
                ViewData["CanRead"] = true;
                ViewData["CanUpdate"] = true;
                ViewData["CanDelete"] = true;
            }
        }

        public void PrePublish()
        {
            Permissions();
        }

        public PartialViewResult ErrorPartialMessage(string message)
        {
            ViewData["Error"] = message;

            return PartialView("Error");
        }

        #region Node Type Service

        private EntityServiceBase<T> _service;

        public EntityServiceBase<T> Service
        {
            get { return _service ?? (_service = ObjectFactory.GetInstance<EntityService<T>>()); }
        }

        #endregion

        #region Node Service

        private EntityServiceBase<Node> _nodeService;

        public EntityServiceBase<Node> NodeService
        {
            get { return _nodeService ?? (_nodeService = ObjectFactory.GetInstance<EntityService<Node>>()); }
        }

        #endregion
    }
}