using FluentNHibernate.Conventions;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.Grades.Entities;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.OrganizationChart.Configurations;
using HRIS.Domain.Personnel.RootEntities;
using  Project.Web.Mvc4.Extensions;
using  Project.Web.Mvc4.Helpers.DomainExtensions;
using  Project.Web.Mvc4.Models.Controls;
using NHibernate.Properties;
using Resources.Areas.Services.AssignEmployeeToPosition;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Extenstions;
using Souccar.Domain.Audit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.OrganizationChart.Indexes;
using HRIS.Domain.OrganizationChart.RootEntities;
using  Project.Web.Mvc4.Models;
using Souccar.Core.Extensions;
using Souccar.Domain.PersistenceSupport;
using Souccar.NHibernate;

using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using  Project.Web.Mvc4.Helpers.Resource;
using Souccar.Infrastructure.Core;
using Souccar.Domain.Audit;

namespace Project.Web.Mvc4.Areas.OrganizationChart.Controllers
{
    //test
    public class NodeController : Controller
    {
        private string _message = string.Empty;
        private bool _isSuccess;
        private List<ValidationResult> _validationResults;
        private Dictionary<string, string>  _errorsMessages;

        public ActionResult TreeView()
        {
            return PartialView();
        }
        public ActionResult OrgTreeView()
        {
            return PartialView();
        }
        public ActionResult OrgChartBasedOnPositionView()
        {
            return PartialView();
        }
        public ActionResult OrgChartBasedOnGradeView()
        {
            return PartialView();
        }

        public ActionResult ReorderNodesIndex()
        {
            return PartialView();
        }

        public ActionResult MergeTwoNodesIndex()
        {
            return PartialView();
        }

        public ActionResult NodeSeparationIndex()
        {
            return PartialView();
        }

        public JsonResult GetTreeNodes(int? id)
        {
            if (id == null)
            {
                var nodes = typeof(Node).GetAll<Node>().Where(x => x.Parent == null && !x.IsHistorical).
                    Select(x => new {x.Id, x.Name, HasChildren = x.Children.Where(child => !child.IsHistorical).Count() != 0 }).ToArray();

                return Json(nodes);
            }
            else
            {
                var nodes = (((Node)typeof(Node).GetById((int)id)).Children.Where(x => !x.IsHistorical).
                    Select(x => new {x.Id, x.Name, HasChildren = x.Children.Where(child => !child.IsHistorical).Count() != 0 }).ToArray());

                return Json(nodes);
            }
        }

        public bool GetOrgChildren(int id)
        {
            return typeof(Position).GetAll<Position>().Where(y => y.Manager.Id == id).ToArray().Count() != 0;
        }
        public string getImg(int id)
        {
            var data = typeof(Position).GetAll<Position>().Where(y => y.Id == id).ToArray();
            Employee employee = null;
            if (data.First().AssigningEmployeeToPosition != null)
                employee =
                     (Employee)
                                 typeof(Employee).GetById(data.First().AssigningEmployeeToPosition.Employee.Id);
            
            var themingName = UserExtensions.CurrentUserTheming;

            if (employee == null)
                return "../Content/images/theme-" + themingName + "/placeholder.jpg";

            if (string.IsNullOrEmpty(employee.PhotoId))
                return "../Content/images/theme-" + themingName + "/placeholder.jpg";
            return "../Content/EmployeesPhoto/" + employee.PhotoId;
        }
        public JsonResult GetOrgTreeNodes(int? id)
        {
            if (id == null)
            {

                
                var nodes = typeof(Position).GetAll<Position>().Where(x => x.Manager == null);
                foreach (var node in nodes)
                {

                    Employee temp =null;
                    if (node.AssigningEmployeeToPosition != null)
                        temp =
                             (Employee)
                                         typeof(Employee).GetById(node.AssigningEmployeeToPosition.Employee.Id);
                    
                    if (temp != null)
                    {
                        if (temp.EmployeeCard != null)
                        {
                            temp.EmployeeCard = (EmployeeCard)typeof(EmployeeCard).GetById(temp.EmployeeCard.Id);
                            if (temp.EmployeeCard.CardStatus == EmployeeCardStatus.OnHeadOfHisWork)
                                node.AssigningEmployeeToPosition.Employee = temp;
                        }
                    }
                  
                    node.JobDescription.Node = (Node)typeof(Node).GetById(node.JobDescription.Node.Id);
                    node.JobDescription.JobTitle = (JobTitle)typeof(JobTitle).GetById(node.JobDescription.JobTitle.Id);
                }


                var data = nodes.
                    Select(x => new
                    {
                        x.Id,
                        Name = x.AssigningEmployeeToPosition.Employee.FirstName + ' ' + x.AssigningEmployeeToPosition.Employee.LastName
                        ,
                        x.Code,
                        Node = x.JobDescription.Node.Name,
                        JobTit = x.JobDescription.JobTitle.Name,
                        JobDesc = x.JobDescription.Name,
                        imageUrl =  getImg(x.Id),
                        HasChildren = GetOrgChildren(x.Id)
                    }).ToArray();

                

                return Json(data);
            }
            else
            {
                var nodes = typeof(Position).GetAll<Position>().Where(x => x.Manager.Id == (int)id);    
                foreach (var node in nodes)
                {
                    
                    Employee temp=null;
                    if(node.AssigningEmployeeToPosition!=null)
                    temp=
                         (Employee)
                                     typeof(Employee).GetById(node.AssigningEmployeeToPosition.Employee.Id);
                    if (temp != null)
                    {
                        if (temp.EmployeeCard != null)
                        {
                            temp.EmployeeCard = (EmployeeCard)typeof(EmployeeCard).GetById(temp.EmployeeCard.Id);
                            if (temp.EmployeeCard.CardStatus == EmployeeCardStatus.OnHeadOfHisWork)
                                node.AssigningEmployeeToPosition.Employee = temp;
                        }
                    }
                   
                    //node.AssigningEmployeeToPosition.Employee.EmployeeCard.CardStatus = 1;
                    node.JobDescription.Node = (Node)typeof(Node).GetById(node.JobDescription.Node.Id);
                    node.JobDescription.JobTitle = (JobTitle)typeof(JobTitle).GetById(node.JobDescription.JobTitle.Id);
                    
                }

                var data = nodes.
                    Select(x => new
                    {
                        x.Id,
                        Name = x.AssigningEmployeeToPosition.Employee.FirstName + ' ' + x.AssigningEmployeeToPosition.Employee.LastName
                        ,
                        x.Code,
                        Node = x.JobDescription.Node.Name,
                        JobTit = x.JobDescription.JobTitle.Name,
                        JobDesc = x.JobDescription.Name,
                        imageUrl =  getImg(x.Id),
                        HasChildren = GetOrgChildren(x.Id)
                    }).ToArray();

             
                return Json(data);
            }

        }
        public JsonResult GetOrgChartBasedOnPositionTreeNodes(int? id)
        {
            if (id == null)
            {


                var nodes = typeof(Position).GetAll<Position>().Where(x => x.Manager == null);
                foreach (var node in nodes)
                {
                    Employee temp = null;
                    if (node.AssigningEmployeeToPosition != null)
                        temp =
                             (Employee)
                                         typeof(Employee).GetById(node.AssigningEmployeeToPosition.Employee.Id);
                    if (temp != null)
                    {
                        if (temp.EmployeeCard != null)
                        {
                            temp.EmployeeCard = (EmployeeCard)typeof(EmployeeCard).GetById(temp.EmployeeCard.Id);
                            if (temp.EmployeeCard.CardStatus == EmployeeCardStatus.OnHeadOfHisWork)
                                node.AssigningEmployeeToPosition.Employee = temp;
                        }
                    }
                   
                    node.JobDescription.Node = (Node)typeof(Node).GetById(node.JobDescription.Node.Id);
                    
                }

                var data = nodes.
                    Select(x => new
                    {
                        x.Id,
                        Name = x.AssigningEmployeeToPosition.Employee.FirstName + ' ' + x.AssigningEmployeeToPosition.Employee.LastName
                           , JobDesc = x.JobDescription.Name,
                        Node=x.JobDescription.Node.Name,
                        x.Code,
                        HasChildren = GetOrgChildren(x.Id)
                    }).ToArray();




                return Json(data);
            }
            else
            {
                var nodes = typeof(Position).GetAll<Position>().Where(x => x.Manager.Id == (int)id);
                foreach (var node in nodes)
                {
                    Employee temp = null;
                    if (node.AssigningEmployeeToPosition != null)
                        temp =
                             (Employee)
                                         typeof(Employee).GetById(node.AssigningEmployeeToPosition.Employee.Id);
                    if (temp != null)
                    {
                        if (temp.EmployeeCard != null)
                        {
                            temp.EmployeeCard = (EmployeeCard)typeof(EmployeeCard).GetById(temp.EmployeeCard.Id);
                            if (temp.EmployeeCard.CardStatus == EmployeeCardStatus.OnHeadOfHisWork)
                                node.AssigningEmployeeToPosition.Employee = temp;
                        }
                    }
                   
                    //node.AssigningEmployeeToPosition.Employee.EmployeeCard.CardStatus = 1;
                    node.JobDescription.Node = (Node)typeof(Node).GetById(node.JobDescription.Node.Id);
                  
                }
                var data = nodes.
                   Select(x => new
                   {
                       x.Id,
                       Name = x.AssigningEmployeeToPosition.Employee.FirstName + ' ' + x.AssigningEmployeeToPosition.Employee.LastName
                          ,
                       JobDesc = x.JobDescription.Name,
                       Node = x.JobDescription.Node.Name, x.Code,
                       HasChildren = GetOrgChildren(x.Id)
                   }).ToArray();


                return Json(data);
            }

        }
        public JsonResult GetOrgChartBasedOnGradeTreeNodes(int? id)
        {
            if (id == null)
            {


                var nodes = typeof(Position).GetAll<Position>().Where(x => x.Manager == null);
                foreach (var node in nodes)
                {
                    Employee temp = null;
                    if (node.AssigningEmployeeToPosition != null)
                        temp =
                             (Employee)
                                         typeof(Employee).GetById(node.AssigningEmployeeToPosition.Employee.Id);
                    if (temp != null)
                    {
                        if (temp.EmployeeCard != null)
                        {
                            temp.EmployeeCard = (EmployeeCard)typeof(EmployeeCard).GetById(temp.EmployeeCard.Id);
                            if (temp.EmployeeCard.CardStatus == EmployeeCardStatus.OnHeadOfHisWork)
                                node.AssigningEmployeeToPosition.Employee = temp;
                        }
                    }
                    
                    node.JobDescription.Node = (Node)typeof(Node).GetById(node.JobDescription.Node.Id);
                    node.JobDescription.JobTitle = (JobTitle)typeof(JobTitle).GetById(node.JobDescription.JobTitle.Id);
                    node.JobDescription.JobTitle.Grade = (HRIS.Domain.Grades.RootEntities.Grade)typeof(HRIS.Domain.Grades.RootEntities.Grade).GetById(node.JobDescription.JobTitle.Grade.Id);

                }

                var data = nodes.
                    Select(x => new
                    {
                        x.Id,
                        Name = x.AssigningEmployeeToPosition.Employee.FirstName + ' ' + x.AssigningEmployeeToPosition.Employee.LastName
                            ,
                        Grade = x.JobDescription.JobTitle.Grade.Name,
                        JobDesc = x.JobDescription.Name,
                        Node = x.JobDescription.Node.Name,
                        x.Code,
                        HasChildren = GetOrgChildren(x.Id)
                    }).ToArray();




                return Json(data);
            }
            else
            {
                var nodes = typeof(Position).GetAll<Position>().Where(x => x.Manager.Id == (int)id);
                foreach (var node in nodes)
                {
                    Employee temp = null;
                    if (node.AssigningEmployeeToPosition != null)
                        temp =
                             (Employee)
                                         typeof(Employee).GetById(node.AssigningEmployeeToPosition.Employee.Id);
                    if (temp != null)
                    {
                        if (temp.EmployeeCard != null)
                        {
                            temp.EmployeeCard = (EmployeeCard)typeof(EmployeeCard).GetById(temp.EmployeeCard.Id);
                            if (temp.EmployeeCard.CardStatus == EmployeeCardStatus.OnHeadOfHisWork)
                                node.AssigningEmployeeToPosition.Employee = temp;
                        }
                    }
                    
                    //node.AssigningEmployeeToPosition.Employee.EmployeeCard.CardStatus = 1;
                    node.JobDescription.Node = (Node)typeof(Node).GetById(node.JobDescription.Node.Id);
                    node.JobDescription.JobTitle = (JobTitle)typeof(JobTitle).GetById(node.JobDescription.JobTitle.Id);
                    node.JobDescription.JobTitle.Grade = (HRIS.Domain.Grades.RootEntities.Grade)typeof(HRIS.Domain.Grades.RootEntities.Grade).GetById(node.JobDescription.JobTitle.Grade.Id);

                }
                var data = nodes.
                     Select(x => new
                     {
                         x.Id,
                         Name = x.AssigningEmployeeToPosition.Employee.FirstName + ' ' + x.AssigningEmployeeToPosition.Employee.LastName
                             ,
                         Grade = x.JobDescription.JobTitle.Grade.Name,
                         JobDesc = x.JobDescription.Name,
                         Node = x.JobDescription.Node.Name,
                         x.Code,
                         HasChildren = GetOrgChildren(x.Id)
                     }).ToArray();


                return Json(data);
            }

        }
        

        [HttpPost]  
        public ActionResult SaveNode(IDictionary<string, object> model)
        {
            InitialzeDefaultValues();
            var node = new Node();
            var isInsert = true;
            try
            {
                var check = true;
                NodeType parenttype;
                Node parentNode = null;
              
                var type = (NodeType) typeof (NodeType).GetById((int) model["Type"].To(typeof (int)));

                if (model.ContainsKey("NodeId") && model["NodeId"] != null && model.ContainsKey("ParentId") )
                {
                    isInsert = false;
                    var updatedNode = (Node) typeof (Node).GetById((int) model["NodeId"].To(typeof (int)));
                    if (model["ParentId"].ToString() != "" )
                        parentNode = (Node)typeof(Node).GetById((int)model["ParentId"].To(typeof(int)));
                    updatedNode.Name = (string) model["Name"];
                    updatedNode.Code = (string) model["Code"];
                    updatedNode.Type = type;
                    var nodes = typeof(Node).GetAll<Node>().Where(x => !x.IsVertualDeleted);
                    if (nodes.Where(x => x.Id != updatedNode.Id && !x.IsHistorical).Any(x => x.Name == updatedNode.Name))
                    {
                        check = false;
                        _validationResults = new List<ValidationResult>();
                        _message = OrganizationChartLocalizationHelper.ThisNodeNameIsAlreadyExist;
                    }
                    else if(nodes.Where(x => x.Id != updatedNode.Id && x.IsHistorical).Any(x => x.Name == updatedNode.Name))
                    {
                        check = false;
                        _validationResults = new List<ValidationResult>();
                        _message = OrganizationChartLocalizationHelper.ThisNodeNameIsAlreadyExistAndTheNodeIsDeleted;
                    }
                    else if(updatedNode.Children.Where(child => !child.IsHistorical).Select(child => (NodeType)typeof(NodeType).GetById(child.Type.Id)).Any(childtype => childtype.Order <= updatedNode.Type.Order))
                    {
                        check = false;
                        _validationResults = new List<ValidationResult>();
                        _message = OrganizationChartLocalizationHelper.OrderingChildrenFailerMessage;
                    }
                    else if (parentNode != null && check)
                    {
                        parenttype = (NodeType)typeof(NodeType).GetById(parentNode.Type.Id);
                        if (parenttype.Order < updatedNode.Type.Order)
                        {
                            node = updatedNode;
                            _validationResults = (List<ValidationResult>)updatedNode.Save();
                        }
                        else
                        {
                            check = false;
                            _validationResults = new List<ValidationResult>();
                            _message = OrganizationChartLocalizationHelper.OrderingFailerMessage;
                        }
                    }
                    else
                    {
                        if (check)
                        {
                            node = updatedNode;
                            _validationResults = (List<ValidationResult>)updatedNode.Save();
                        }
                    }
                    
                }
                else
                {
                    if (model.ContainsKey("ParentId") && (model["ParentId"].ToString() != "" || model["ParentId"] != null))
                        parentNode = (Node) typeof (Node).GetById((int) model["ParentId"].To(typeof (int)));

                    var newNode = new Node()
                                  {
                                      Name = (string) model["Name"],
                                      Code = (string) model["Code"],
                                      Type = type
                                  };
                    var nodes = typeof(Node).GetAll<Node>().Where(x => !x.IsVertualDeleted);
                    if (nodes.Where(x => !x.IsHistorical).Any(x => x.Name == newNode.Name))
                    {
                        check = false;
                        _validationResults = new List<ValidationResult>();
                        _message = OrganizationChartLocalizationHelper.ThisNodeNameIsAlreadyExist;
                    }
                    else if (nodes.Where(x => x.IsHistorical).Any(x => x.Name == newNode.Name))
                    {
                        check = false;
                        _validationResults = new List<ValidationResult>();
                        _message = OrganizationChartLocalizationHelper.ThisNodeNameIsAlreadyExistAndTheNodeIsDeleted;
                    }
                    else if (parentNode != null)
                    {
                        parenttype = (NodeType)typeof(NodeType).GetById(parentNode.Type.Id);
                        if (parenttype.Order < newNode.Type.Order)
                        {
                            newNode.Parent = parentNode;
                            node = newNode;
                            _validationResults = (List<ValidationResult>)newNode.Save();
                        }
                        else
                        {
                            check = false;
                            _validationResults = new List<ValidationResult>();
                            _message = OrganizationChartLocalizationHelper.OrderingFailerMessage;
                        }
                    }
                    else
                    {
                        node = newNode;
                        _validationResults = (List<ValidationResult>)newNode.Save();
                    }
                   
                }

                if (!AnyValidationResults())
                {
                    if (check)
                    {
                        _isSuccess = true;
                        _message = Helpers.GlobalResource.DoneMessage;
                        var NodeObj = new Node();
                        ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { }, UserExtensions.CurrentUser, node, isInsert ? OperationType.Insert : OperationType.Update);

                    }

                }
            }
            catch(Exception ex)
            {
                _errorsMessages = new Dictionary<string, string> { { "Exception", ex.Message } };
            }

            return Json(new
            {
                Success = _isSuccess,
                Msg = _message,
                Errors = _errorsMessages
            });
        }

        private Node GetNodeByCode(string code)
        {
            return typeof(Node).GetAll<Node>().First(x => x.Code == code);
        }

        [HttpPost]
        public ActionResult DeleteNode(string nodeId)
        {
            InitialzeDefaultValues();

            try
            {
                var selectedNode = ServiceFactory.ORMService.All < Node>().Where(x=>x.Id == int.Parse(nodeId)).FirstOrDefault();
               var relatedJobDescription= ServiceFactory.ORMService.All<HRIS.Domain.JobDescription.RootEntities.JobDescription>().Where(x => x.Node.Id == selectedNode.Id).ToList();

                var info = OrganizationChartLocalizationHelper.GetResource(OrganizationChartLocalizationHelper.DeleteNode);
               
                ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { }, UserExtensions.CurrentUser, selectedNode, OperationType.Delete);



                if ( selectedNode.Children.Where(child => child.IsHistorical==false).Count()>0)
                {
                    _isSuccess = false;
                    _message = Helpers.GlobalResource.CannotDeleteBecauseItHasChildrenMessage;
                }
              else  if (relatedJobDescription.Count() > 0)
                {
                    _isSuccess = false;
                    _message = Helpers.GlobalResource.CannotDeleteBecauseItRelatedToJobDescription;
                }
              
                else
                {
                    selectedNode.IsHistorical=true;
                    _isSuccess = true;
                    _message = Helpers.GlobalResource.DoneMessage;
                    selectedNode.Save();
                }
            }
            catch
            {
                
            }

            return Json(new
            {
                Success = _isSuccess,
                Msg = _message
            });

        }

        [HttpPost]
        public ActionResult GetNodeTypesList()
        {
            var nodeTypes = GetAllNodeTypes();

            var result = new List<Dictionary<string, object>>();
            foreach (var item in nodeTypes)
            {
                var temp = new Dictionary<string, object>();
                temp["Name"] = item.Name;
                temp["Id"] = item.Id;
                result.Add(temp);
            }

            return Json(result);
        }

        private IEnumerable<NodeType> GetAllNodeTypes()
        {
            return typeof(NodeType).GetAll<NodeType>();
        }

        [HttpPost]
        public ActionResult GetNodeInformation(int id = -1)
        {
            var node = GetNodeById(id);

            var result = new Dictionary<string, object>();

            if (node != null)
            {
                result["ParentId"] = node.Parent == null ? string.Empty : node.Parent.Id.ToString();
                result["ParentName"] = node.Parent == null ? string.Empty : node.Parent.Name;
                result["Name"] = node.Name;
                result["Code"] = node.Code;
                result["Type"] = node.Type == null ? string.Empty : node.Type.Id.ToString();
                result["NodeId"] = node.Id;
            }
            else
            {
                result["ParentId"] = string.Empty;
                result["ParentName"] = string.Empty;
                result["Name"] = string.Empty;
                result["Code"] = string.Empty;
                result["Type"] = string.Empty;
            }

            return Json(result);
        }
        [HttpPost]
        public ActionResult GetNodeInformationByCode(string code = "")
        {
            var node = GetNodeByCode(code);

            var result = new Dictionary<string, object>();

            if (node != null)
            {
                result["ParentId"] = node.Parent == null ? string.Empty : node.Parent.Id.ToString();
                result["ParentName"] = node.Parent == null ? string.Empty : node.Parent.Name;
                result["Name"] = node.Name;
                result["Code"] = node.Code;
                result["Type"] = node.Type == null ? string.Empty : node.Type.Id.ToString();
                result["NodeId"] = node.Id;
            }
            else
            {
                result["ParentId"] = string.Empty;
                result["ParentName"] = string.Empty;
                result["Name"] = string.Empty;
                result["Code"] = string.Empty;
                result["Type"] = string.Empty;
            }

            return Json(result);
        }

        private Node GetNodeById(int id)
        {
            return (Node)typeof(Node).GetById(id);
        }

        private void InitialzeDefaultValues()
        {
            _isSuccess = false;
            _message = Helpers.GlobalResource.FailMessage;
            _errorsMessages = null;
        }

        [HttpPost]
        public ActionResult IsAllowedToReorderThisNode(int sourceNodeId, int targetNodeId)
        {
            InitialzeDefaultValues();

            try
            {
                var sourceNode = GetNodeById(sourceNodeId);
                var targetNode = GetNodeById(targetNodeId);

                object[] messageParameters = new object[4];
                messageParameters[0] = sourceNode.Name;
                messageParameters[1] = sourceNode.Type.Order;
                messageParameters[2] = targetNode.Name;
                messageParameters[3] = targetNode.Type.Order;

                if (sourceNode.Type.Order < targetNode.Type.Order)
                {
                    _message = string.Format(OrganizationChartLocalizationHelper.MsgCanNotMoveNode, messageParameters);
                }
                else
                {
                    _isSuccess = true;
                    _message = string.Format(OrganizationChartLocalizationHelper.MsgAreYouSureYouWantMoveNode, messageParameters);
                }   
            }
            catch
            {
            }

            return Json(new
            {
                Success = _isSuccess,
                Msg = _message
            });

        }

        [HttpPost]
        public ActionResult ReorderNode(int nodeId, int targetParentId)
        {
            DateTime startTimeOfProcess = DateTime.Now;
            InitialzeDefaultValues();

            try
            {
                _message = Helpers.GlobalResource.FailMessage;
                Node node = GetNodeById(nodeId);
                if ( nodeId != targetParentId)
                {
                   
                    var oldParent = node.Parent;
                    var parentNode = GetNodeById(targetParentId);
                    var tempparent = parentNode;
                    bool flag = false;
                    while (tempparent.Parent != null)
                    {
                        if (tempparent.Parent == node)
                        {
                            flag = true;
                            break;
                        }
                        tempparent = tempparent.Parent;
                    }
                    if (!flag)
                    {
                        if (((NodeType)typeof(NodeType).GetById(parentNode.Type.Id)).Order < ((NodeType)typeof(NodeType).GetById(node.Type.Id)).Order)
                        {
                            node.Parent = parentNode;
                            var info = OrganizationChartLocalizationHelper.GetResource(OrganizationChartLocalizationHelper.ReorderNodesCaption);
                            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { node }, UserExtensions.CurrentUser, node, OperationType.Update, info, startTimeOfProcess, new List<Entity>() { node, oldParent, parentNode });

                            _isSuccess = true;
                            _message = Helpers.GlobalResource.DoneMessage;
                        }
                        else
                            _message = OrganizationChartLocalizationHelper.OrderingFailerMessage;
                    }
                    else
                    {
                        _message = OrganizationChartLocalizationHelper.OrderingParentFailerMessage;
                    }


                }


            }
            catch
            {

            }

            return Json(new
            {
                Success = _isSuccess,
                Msg = _message
            });
        }

        [HttpPost]
        public ActionResult MergeTwoNodes(IDictionary<string, object> model, int firstNodeId, int secondNodeId)
        {
            InitialzeDefaultValues();

            try
            {
                DateTime startTimeOfProcess = DateTime.Now;
                var entities = new List<IAggregateRoot>();
                var type = (NodeType)typeof(NodeType).GetById((int)model["Type"].To(typeof(int)));
                var parentNode = (Node)typeof(Node).GetById((int)model["Parent"]);
                if (type.Order > parentNode.Type.Order)
                {
                    var newNode = new Node()
                    {
                        Name = (string) model["Name"],
                        Code = (string) model["Code"],
                        Type = type,
                        Parent = parentNode
                    };
                    entities.Add(newNode);
                    //_validationResults = (List<ValidationResult>)newNode.Save();

                    //if (!AnyValidationResults())
                    //{
                    var jobDescriptions = typeof (HRIS.Domain.JobDescription.RootEntities.JobDescription)
                        .GetAll<HRIS.Domain.JobDescription.RootEntities.JobDescription>().
                        Where(jd => jd.Node.Id == firstNodeId || jd.Node.Id == secondNodeId);

                    foreach (var jobDesc in jobDescriptions)
                    {
                        jobDesc.Node = newNode;
                        entities.Add(jobDesc);
                    }

                    var childrenNodes =
                        typeof (Node).GetAll<Node>()
                            .Where(nd => nd.Parent.Id == firstNodeId || nd.Parent.Id == secondNodeId);

                    foreach (var childNode in childrenNodes)
                    {
                        childNode.Parent = newNode;
                        entities.Add(childNode);
                    }

                    var firstNode = (Node) typeof (Node).GetById(firstNodeId);
                    firstNode.IsHistorical = true;
                    entities.Add(firstNode);

                    var secondNode = (Node) typeof (Node).GetById(secondNodeId);
                    secondNode.IsHistorical = true;
                   
                    var info = OrganizationChartLocalizationHelper.GetResource(OrganizationChartLocalizationHelper.MergeTwoNodes);
                    entities.Add(secondNode);
                    ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, newNode, OperationType.Update, info, startTimeOfProcess, new List<Entity>() { firstNode, secondNode, newNode });
                    _isSuccess = true;
                    _message = Helpers.GlobalResource.DoneMessage;
                    //}
                }
                else
                {
                    _isSuccess = false;
                    _message =
                        Helpers.Resource.OrganizationChartLocalizationHelper
                            .MsgTheNewNodeTypeMustBeLowerThanParentNodeType;
                }
            }
            catch (Exception ex)
            {
                _errorsMessages = new Dictionary<string, string> { { "Exception", ex.Message } };
            }

            return Json(new
            {
                Success = _isSuccess,
                Msg = _message,
                Errors = _errorsMessages
            });
        }

        [HttpPost]
        public ActionResult checkParentTypeNode(int newParentNodeID, int newNodeTypeID)
        {
            var Node = ServiceFactory.ORMService.GetById<Node>(newParentNodeID);
            var NodeType = ServiceFactory.ORMService.GetById<NodeType>(newNodeTypeID);
            //var TypeForNewNode = ServiceFactory.ORMService.GetById<NodeType>(Node.Type.Id);

            if (Node.Type.Order >= NodeType.Order)
            {
                _isSuccess = false;
                _message = Helpers.GlobalResource.NodeTypeMustBeMoreThanSelectedParentType;
            }
            else
            {
                _isSuccess = true;
                _message = Helpers.GlobalResource.DoneMessage;
            }

            return Json(new
            {
                Success = _isSuccess,
                Msg = _message,
                Errors = _errorsMessages
            });
        }

        [HttpPost]
        public ActionResult SeperateNode(IDictionary<string, object> model, 
            IList<DualSelectListValues> jobDescriptionSelectListValues, IList<DualSelectListValues> nodeSelectListValues)
        {
            InitialzeDefaultValues();
            DateTime startTimeOfProcess = DateTime.Now;
            try
            {
                var entities = new List<IAggregateRoot>();
                var firstType = (NodeType) typeof (NodeType).GetById(int.Parse(model["FirstType"].ToString()));
                var secondType = (NodeType) typeof (NodeType).GetById(int.Parse(model["SecondType"].ToString()));
                var parentNode = ((Node) typeof (Node).GetById((int) model["Parent"])).Parent;
                var parentType = (NodeType) typeof (NodeType).GetById(parentNode.Type.Id);
                if (firstType.Order <= parentType.Order)
                {
                    _isSuccess = false;
                    _message = OrganizationChartLocalizationHelper.TheOrderOfTypeOfFirstChildNodeMustGreaterThanTheOrderOfTheParent;
                }
                else if (secondType.Order <= parentType.Order)
                {
                    _isSuccess = false;
                    _message = OrganizationChartLocalizationHelper.TheOrderOfTypeOfSecondChildNodeMustGreaterThanTheOrderOfTheParent;
                }
                else
                {
                    var firstNode = new Node()
                    {
                        Name = (string) model["FirstName"],
                        Code = (string) model["FirstCode"],
                        Type = firstType,
                        Parent = parentNode
                    };

                    if (
                        !typeof (Node).GetAll<Node>()
                            .Any(
                                nd =>
                                    nd.Name == (string) model["FirstName"] && nd.Parent == parentNode &&
                                    !nd.IsHistorical))
                        entities.Add(firstNode);
                    else
                    {
                        _validationResults = new List<ValidationResult>();
                        firstNode = typeof (Node).GetAll<Node>().FirstOrDefault(
                            nd => nd.Name == (string) model["FirstName"] && nd.Parent == parentNode && !nd.IsHistorical);
                    }
                    var secondNode = new Node()
                    {
                        Name = (string) model["SecondName"],
                        Code = (string) model["SecondCode"],
                        Type = secondType,
                        Parent = parentNode
                    };

                    entities.Add(secondNode);
                    if (jobDescriptionSelectListValues != null && jobDescriptionSelectListValues.Count > 0)
                    {
                        foreach (var jobDescriptionItem in jobDescriptionSelectListValues)
                        {
                            var jobDescription =
                                (HRIS.Domain.JobDescription.RootEntities.JobDescription)
                                    typeof (HRIS.Domain.JobDescription.RootEntities.JobDescription).
                                        GetById(jobDescriptionItem.Value);
                            jobDescription.Node = jobDescriptionItem.Dir == "Left" ? firstNode : secondNode;
                            entities.Add(jobDescription);
                        }
                    }

                    if (nodeSelectListValues != null && nodeSelectListValues.Count > 0)
                    {
                        foreach (var nodeItem in nodeSelectListValues)
                        {
                            var node = (Node) typeof (Node).GetById(nodeItem.Value);
                            node.Parent = nodeItem.Dir == "Left" ? firstNode : secondNode;
                            entities.Add(node);
                        }
                    }

                    var seperatedNode = (Node) typeof (Node).GetById((int) model["Parent"]);
                    seperatedNode.IsHistorical = true;
                    entities.Add(seperatedNode);
                    var info = OrganizationChartLocalizationHelper.GetResource(OrganizationChartLocalizationHelper.SeperateNode);
                    ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, seperatedNode, OperationType.Update, info, startTimeOfProcess, new List<Entity>() { seperatedNode, firstNode, secondNode, });
                    _isSuccess = true;
                    _message = Helpers.GlobalResource.DoneMessage;
                }
            }
            catch (Exception ex)
            {
                _errorsMessages = new Dictionary<string, string> {{"Exception", ex.Message}};
            }

            return Json(new
            {
                Success = _isSuccess,
                Msg = _message,
                Errors = _errorsMessages
            });
        }

        public ActionResult GetJobDescriptionsByNode(int nodeId)
        {
            var jobDescriptions = typeof(HRIS.Domain.JobDescription.RootEntities.JobDescription).GetAll<HRIS.Domain.JobDescription.RootEntities.JobDescription>()
                .Where(s => s.Node.Id == nodeId).Select(x => new { Value = x.Id, Title = x.Name, Dir = "Left" }).ToList();
            return Json(jobDescriptions, JsonRequestBehavior.AllowGet);
        
        }

        public ActionResult GetChildrenByParentNode(int nodeId)
        {
            var nodes = typeof(Node).GetAll<Node>().Where(s => s.Parent.Id == nodeId).
                Select(x => new { Value = x.Id, Title = x.Name, Dir = "Left" }).ToList();
            return Json(nodes, JsonRequestBehavior.AllowGet);

        }

        protected bool AnyValidationResults(string controlPrefixName)
        {
            if (_validationResults.Any())
            {
                _errorsMessages = new Dictionary<string, string>();

                foreach (var error in _validationResults)
                {
                    if (_errorsMessages.Keys.All(x => x != error.Property.Name))
                        _errorsMessages.Add(controlPrefixName + error.Property.Name, error.Message);
                }

                return true;
            }

            return false;
        }

        protected bool AnyValidationResults()
        {
            return AnyValidationResults(string.Empty);
        }

    }

    public class DualSelectListValues
    {
        public int  Value { get; set; }
        public string Title { get; set; }
        public string Dir { get; set; }
    }
}
