using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.OrganizationChart.Configurations;
using HRIS.Domain.OrganizationChart.RootEntities;
using Project.Web.Mvc4.Areas.OrganizationChart.Models;
using Project.Web.Mvc4.Factories;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Project.Web.Mvc4.Models.Navigation;
using Project.Web.Mvc4.Helpers.Resource;
using Souccar.Core.Fasterflect;
using Node = Castle.MicroKernel.Registration.Node;
using Project.Web.Mvc4.Helpers;

namespace Project.Web.Mvc4.Areas.OrganizationChart.Models
{
    public class OrganizationChartAdjustment: ModelAdjustment
    {
        private static Dictionary<string, ViewModel> parent = new Dictionary<string, ViewModel>();
        public override void AdjustModule(Module module)
        {
            //if (module.ModuleId.Equals(ModulesNames.OrganizationChart))
            //{
            //    var nodeDetails = new List<string>()
            //    {
            //        "NodeBenefitDetails","NodeDeductionDetails"
            //    };

            //    module.Aggregates.SingleOrDefault(x => x.TypeFullName == (typeof(HRIS.Domain.OrganizationChart.RootEntities.Node).FullName))
            //           .Details = module.Aggregates.SingleOrDefault(x => x.TypeFullName == (typeof(HRIS.Domain.OrganizationChart.RootEntities.Node).FullName))
            //               .Details
            //               .Where(x => nodeDetails.Contains(x.DetailId))
            //               .ToList();               
            //}

            //module.Aggregates.Add(new Aggregate()
            //{
            //    Title = OrganizationChartLocalizationHelper.GetResource(OrganizationChartLocalizationHelper.OrganizationCaption),
            //    Controller = "OrganizationChart/Organization",
            //    Action = "Organization",
            //    AggregateId = "Organization",
            //    TypeFullName = "Organization",
            //    SecurityId = "Organization"
            //});

            module.Aggregates.Add(new Aggregate()
            {
                Title = OrganizationChartLocalizationHelper.GetResource(OrganizationChartLocalizationHelper.PrivateOrganizationCaption),
                Controller = "OrganizationChart/Organization",
                Action = "PrivateOrganization",
                AggregateId = "PrivateOrganization",
                TypeFullName = "Organization",
                SecurityId = "PrivateOrganization"
            });

            module.Aggregates.Add(new Aggregate()
            {
                Title = OrganizationChartLocalizationHelper.GetResource(OrganizationChartLocalizationHelper.OrganizationTreeCaption),
                Controller = "OrganizationChart/Node",
                Action = "TreeView",
                AggregateId = "TreeView",
                TypeFullName = "TreeView",
                SecurityId = "TreeView",
                Order = 6
            });

            module.Dashboards.Add(new Dashboard()
            {
                Title = GlobalResource.Dashboard,
                Controller = "OrganizationChart/Dashboard",
                Action = "OrgChartDashboard",
                DashboardId = "OrganizationChart",
                SecurityId = "OrganizationChartDashboard"
            });

            module.Dashboards.Add(new Dashboard()
            {
                Title = GlobalResource.OrganizationChartTree,
                Controller = "OrganizationChart/Dashboard",
                Action = "OrgChartDashboardTree",
                DashboardId = "OrganizationChartTree",
                SecurityId = "OrganizationChartTreeDashboard"
            });

            //module.Aggregates.Add(new Aggregate()
            //{
            //    Title = OrganizationChartLocalizationHelper.GetResource(OrganizationChartLocalizationHelper.NodesCaption),
            //    Controller = "OrganizationChart/Node",
            //    Action = "TreeView",
            //    AggregateId = "TreeView",
            //    TypeFullName = "TreeView",
            //    SecurityId = "TreeView",
            //    Order = 5
            //});

            if (module.ModuleId.Equals(ModulesNames.OrganizationChart))
            {
                var nodeDetails = new List<string>()
                {
                    "NodeBenefitDetails", "NodeDeductionDetails"
                };
                var node = module.Aggregates.SingleOrDefault(x => x.TypeFullName == typeof(HRIS.Domain.OrganizationChart.RootEntities.Node).FullName);
                if (node != null)
                {
                    node.Details = DetailFactory.Create(typeof(HRIS.Domain.OrganizationChart.RootEntities.Node));
                    node.Details = node.Details.Where(x => nodeDetails.Contains(x.DetailId))
                            .ToList();
                }
                
            }

            module.Services = new List<Service>()
                                      {
                                          new Service()
                                              {
                                                  Title =OrganizationChartLocalizationHelper.GetResource(OrganizationChartLocalizationHelper.MergeNodesCaption),
                                                  Controller = "OrganizationChart/Node",
                                                  Action = "MergeTwoNodesIndex",
                                                  ServiceId = "MergeTwoNodes",
                                                  SecurityId = "MergeTwoNodes",
                                              },
                                          new Service()
                                              {
                                                  Title = OrganizationChartLocalizationHelper.GetResource(OrganizationChartLocalizationHelper.NodeSeparationCaption),
                                                  Controller = "OrganizationChart/Node",
                                                  Action = "NodeSeparationIndex",
                                                  ServiceId = "NodeSeparation",
                                                  SecurityId = "NodeSeparation"
                                              },
                                          new Service()
                                              {
                                                  Title =OrganizationChartLocalizationHelper.GetResource(OrganizationChartLocalizationHelper.ReorderNodesCaption),
                                                  Controller = "OrganizationChart/Node",
                                                  Action = "ReorderNodesIndex",
                                                  ServiceId = "ReorderNodes",
                                                  SecurityId = "ReorderNodes"
                                              },
                                              new Service()
                                              {
                                                  Title =OrganizationChartLocalizationHelper.GetResource(OrganizationChartLocalizationHelper.OverallOrgChart),
                                                  Controller = "OrganizationChart/Node",
                                                  Action = "OrgTreeView",
                                                  ServiceId = "OrgTreeView",
                                                  SecurityId = "OrgTreeView"
                                              },
                                              new Service()
                                              {
                                                  Title =OrganizationChartLocalizationHelper.GetResource(OrganizationChartLocalizationHelper.OrgChartBasedOnPosition),
                                                  Controller = "OrganizationChart/Node",
                                                  Action = "OrgChartBasedOnPositionView",
                                                  ServiceId = "OrgChartBasedOnPositionView",
                                                  SecurityId = "OrgChartBasedOnPositionView"
                                              },
                                              new Service()
                                              {
                                                  Title =OrganizationChartLocalizationHelper.GetResource(OrganizationChartLocalizationHelper.OrgChartBasedOnGrade),
                                                  Controller = "OrganizationChart/Node",
                                                  Action = "OrgChartBasedOnGradeView",
                                                  ServiceId = "OrgChartBasedOnGradeView",
                                                  SecurityId = "OrgChartBasedOnGradeView"
                                              }
                                          
                                      };
        }

        public override ViewModel AdjustGridModel(string type)
        {
            if (parent.Count == 0)
            {
                parent.Add("SubCompany", new SubCompanyViewModel());
                parent.Add("NodeType", new NodeTypeViewModel());
                parent.Add("Node", new NodeViewModel());



            }
            try
            {
                return parent[type];
            }
            catch
            {

                return new ViewModel();
            }
            //if (type == typeof(Domain.OrganizationChart.RootEntities.Node))
            //{
            //    model.ActionListHandler = "Node_ActionListHandler";
            //    model.Views[0].ViewHandler = "NodeViewHandler";
            //    model.ToolbarCommands.RemoveAt(0);             
            //}





        }
    }
}