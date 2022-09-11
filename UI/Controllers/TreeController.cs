#region

using System.Linq;
using System.Web.Mvc;
using Infrastructure.Entities;
using HRIS.Domain.OrgChart.Entities;
using HRIS.Domain.OrgChart.ValueObjects;
using Service;
using StructureMap;
using UI.Helpers.Controllers;
using UI.Utilities;

#endregion

namespace UI.Controllers
{
    public class TreeController : RootEntityController
    {
        #region Properties

        #region Organization

        private EntityService<Organization> _service;

        public EntityServiceBase<Organization> Service
        {
            get { return _service ?? (_service = ObjectFactory.GetInstance<EntityService<Organization>>()); }
        }

        #endregion

        #region Node Service

        private EntityServiceBase<Node> _nodeService;

        public EntityServiceBase<Node> NodeService
        {
            get { return _nodeService ?? (_nodeService = ObjectFactory.GetInstance<EntityService<Node>>()); }
        }

        #endregion

        #endregion

        #region Render Tree

        public ActionResult NodeToJson()
        {
            Node node = GetMasterRecordValue(MasterRecordOrder.First) != 0
                            ? NodeService.LoadById(GetMasterRecordValue(MasterRecordOrder.First))
                            : Service.GetAll().Single().RootNode.Single();

            string result = node.ToString();

            return Json(new
                            {
                                Success = true,
                                NodeId = node.Id,
                                NodeCode = node.Code,
                                Message = result
                            });
        }

        #endregion
    }
}