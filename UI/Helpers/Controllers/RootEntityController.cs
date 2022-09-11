#region

using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.SessionState;
using UI.Filters;
using UI.Helpers.Session;
using UI.Utilities;

#endregion

namespace UI.Helpers.Controllers
{
    [Authorize]
    //[Localization]
    [SessionExpireFilter]
    [SessionState(SessionStateBehavior.Default)]
    public class RootEntityController : MvcPathNavigator
    {
        #region Session Keys

        #region Properties

        public int LatestSectionPartial
        {
            get
            {
                int id = 0;
                if (Session["LatestSectionPartial"] != null)
                {
                    int.TryParse(Session["LatestSectionPartial"].ToString(), out id);
                }

                return id;
            }

            set { Session["LatestSectionPartial"] = value != 0 ? value : 0; }
        }

        #endregion

        #region Master Records Keys

        public bool CurrentlyInFirstLevel
        {
            set { Session["CurrentlyInFirstLevel"] = value; }
        }

        public int CurrentlyInSecondLevel
        {
            set
            {
                if (value != 0)
                {
                    Session["CurrentSecondLevelRowId"] = value;
                }
                else
                {
                    Session["CurrentSecondLevelRowId"] = null;
                }
            }
        }

        public int SetMasterRecordValue(MasterRecordOrder masterRecordOrder, int value)
        {
            Dictionary<MasterRecordOrder, int> output;

            if (value > int.MaxValue | value < int.MinValue)
            {
                value = 0;
            }

            if (Session["MasterRecords"] != null)
            {
                output = (Dictionary<MasterRecordOrder, int>) Session["MasterRecords"];
            }
            else
            {
                output = new Dictionary<MasterRecordOrder, int>();
            }

            output[masterRecordOrder] = value;

            Session["MasterRecords"] = output;

            return output[masterRecordOrder];
        }

        public int GetMasterRecordValue(MasterRecordOrder masterRecordOrder)
        {
            Dictionary<MasterRecordOrder, int> output = null;

            if (Session != null && Session["MasterRecords"] != null)
            {
                output = (Dictionary<MasterRecordOrder, int>) Session["MasterRecords"];
            }

            if (output != null && output.ContainsKey(masterRecordOrder))
            {
                return output[masterRecordOrder];
            }

            return 0;
        }

        public void ClearMasterRecords()
        {
            Session["MasterRecords"] = null;
            CurrentlyInSecondLevel = 0;
            CurrentlyInFirstLevel = false;
        }

        #endregion

        #region Tabs

        [HttpPost]
        public virtual void SaveTabIndex(int selectedIndex)
        {
            Session["SelectedTabIndex"] = selectedIndex;
        }

        [HttpPost]
        public virtual void SaveTabIndexSecondLevel(int selectedIndex)
        {
            Session["SelectedTabIndexSecondLevel"] = selectedIndex;
        }

        public void ClearTabIndex()
        {
            Session["SelectedTabIndex"] = null;
            Session["SelectedTabIndexSecondLevel"] = null;
        }

        #endregion

        #region Core Entity Relations

        #region Related Node

        public int RelatedNode
        {
            get
            {
                int id = 0;
                if (Session["RelatedNode"] != null)
                {
                    int.TryParse(Session["RelatedNode"].ToString(), out id);
                }

                return id;
            }

            private set { Session["RelatedNode"] = value != 0 ? value : 0; }
        }

        public void SetRelatedNodeToTheSession(int nodeId)
        {
            RelatedNode = nodeId;
            RelatedPosition = 0;
            RelatedEmployee = 0;
        }

        #endregion

        #region Related Position

        public int RelatedPosition
        {
            get
            {
                int id = 0;
                if (Session["RelatedPosition"] != null)
                {
                    int.TryParse(Session["RelatedPosition"].ToString(), out id);
                }

                return id;
            }

            private set { Session["RelatedPosition"] = value != 0 ? value : 0; }
        }

        public void SetRelatedPositionToTheSession(int positionId)
        {
            RelatedPosition = positionId;
            RelatedEmployee = 0;
        }

        #endregion

        #region Related Employee

        public int RelatedEmployee
        {
            get
            {
                int id = 0;
                if (Session["RelatedEmployee"] != null)
                {
                    int.TryParse(Session["RelatedEmployee"].ToString(), out id);
                }

                return id;
            }

            private set { Session["RelatedEmployee"] = value != 0 ? value : 0; }
        }

        public void SetRelatedEmployeeToTheSession(int employeeId)
        {
            RelatedEmployee = employeeId;
        }

        #endregion

        #endregion

        #endregion

        #region Utilities

        public PartialViewResult ErrorPartialMessage(string message)
        {
            //TempData["Error"] = message;
            TempData["GlobalError"] = message;

            return PartialView("Error");
        }

        public void SetGlobalErrorMessage(string errorMessage)
        {
            TempData["GlobalError"] = errorMessage;
        }

        #endregion

        #region Overrides of Mvc Path Navigator

        protected override PathStep PrepareStep(MasterRecordOrder masterRecordOrder,
                                                RibbonLevels level,
                                                string actionName,
                                                string controllerName,
                                                string areaName,
                                                string nodeName,
                                                int stepId)
        {
            MasterRecordOrder localMasterRecordOrder = MasterRecordOrder.First;
            if (masterRecordOrder != MasterRecordOrder.First)
            {
                localMasterRecordOrder = masterRecordOrder;
            }

            RibbonLevels localLevel = RibbonLevels.A;
            if (level != RibbonLevels.A)
            {
                localLevel = level;
            }

            string localActionName = "Index";
            if (!string.IsNullOrEmpty(actionName))
            {
                localActionName = actionName;
            }

            string localControllerName =
                ControllerContext.Controller.ValueProvider.GetValue("controller").RawValue.ToString();
            if (!string.IsNullOrEmpty(controllerName))
            {
                localControllerName = controllerName;
            }

            string localNodeName = localControllerName;
            if (!string.IsNullOrEmpty(nodeName))
            {
                localNodeName = nodeName;
            }

            int localStepId = GetMasterRecordValue(localMasterRecordOrder);
            if (stepId != 0)
            {
                localStepId = stepId;
            }

            return new PathStep
                       {
                           MasterRecordOrder = localMasterRecordOrder,
                           Level = localLevel,
                           ActionName = localActionName,
                           ContextName = localControllerName,
                           StepId = localStepId,
                           StepName = localNodeName,
                           AreaName = areaName
                       };
        }

        public override void LoadStepsList()
        {
            ViewData["PathStepsList"] = PathStepsList;
        }

        public override dynamic GoBackward(int stepOrder, int selectedTabOrder = 0)
        {
            PathStep step = GetStep(stepOrder);

            return step != null
                       ? RedirectToAction(step.ActionName, step.ContextName,
                                          new {area = step.AreaName, id = step.StepId, selectedTabOrder})
                       : RedirectToAction("Index", "Home", new { area = "" });//RedirectToAction("LogOn", "Account", new {area = ""});
        }

        #endregion
    }
}