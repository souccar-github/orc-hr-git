#region

using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using UI.Controllers;
using UI.Helpers.Cache;

#endregion

namespace UI.Utilities
{
    public abstract class MvcPathNavigator : AccountController
    {
        #region Properties

        public List<PathStep> PathStepsList
        {
            get
            {
                return CacheProvider.Get("PathStepsList" + User.Identity.Name, () => new List<PathStep>());
            }
        }

        #endregion

        public void AddToPath(MasterRecordOrder masterRecordOrder = MasterRecordOrder.First,
                              RibbonLevels level = RibbonLevels.A,
                              string actionName = null,
                              string controllerName = null,
                              string nodeName = null,
                              int stepId = 0,
                              string areaName = null)
        {
            PathStep step = PrepareStep(masterRecordOrder, level, actionName, controllerName, areaName, nodeName, stepId);

            AddToPath(step);
        }

        public void AddToPath(PathStep pathStep)
        {
            var pathStepsList = PathStepsList;

            if (pathStep.MasterRecordOrder == MasterRecordOrder.First & pathStepsList.Count > 0)
            {
                pathStepsList.Clear();
            }

            bool exist = false;
            foreach (PathStep step in pathStepsList)
            {
                if (step.StepName == pathStep.StepName)
                {
                    exist = true;

                    pathStep.StepOrder = step.StepOrder;

                    Mapper.DynamicMap(pathStep, step);
                }
            }

            if (!exist)
            {
                pathStepsList.Add(pathStep);
                pathStep.StepOrder = pathStepsList.Count;
            }

            CacheProvider.Set("PathStepsStack" + User.Identity.Name, pathStepsList);
        }

        protected dynamic GetStep(int stepOrder)
        {
            PathStep step = null;

            if (stepOrder != -1)
            {
                var pathStepsList = PathStepsList;

                step = pathStepsList.SingleOrDefault(s => s.StepOrder == stepOrder);

                if (step != null)
                {
                    if (step.Level != RibbonLevels.Root)
                    {
                        pathStepsList.RemoveAll(s => s.StepOrder >= stepOrder);
                    }
                    else
                    {
                        pathStepsList.RemoveAll(s => s.StepOrder > stepOrder);
                    }

                    CacheProvider.Set("PathStepsStack" + User.Identity.Name, pathStepsList);
                }
                else
                {
                    RedirectToAction("Index", "Home", new { area = "" });
                    //RedirectToAction("LogOn", "Account", new {area = ""});
                }
            }
            else
            {
                if (PathStepsList.Count > 0)
                {
                    var pathStepsList = PathStepsList;

                    step = pathStepsList[pathStepsList.Count - 1];
                }
            }

            return step;
        }

        public dynamic GoToTheLatestPage()
        {
            PathStep step = GetStep(-1);

            return step == null
                       ? RedirectToAction("Index", "JobDescEntity", new { area = "JobDesc" })
                       : RedirectToAction(step.ActionName, step.ContextName,
                                          new { id = step.StepId, area = step.AreaName });
        }

        #region abstracts

        protected abstract PathStep PrepareStep(MasterRecordOrder masterRecordOrder,
                                                RibbonLevels level,
                                                string actionName,
                                                string controllerName,
                                                string areaName,
                                                string nodeName,
                                                int stepId);

        public abstract dynamic GoBackward(int stepOrder, int selectedTabOrder = 0);

        public abstract void LoadStepsList();

        #endregion
    }
}