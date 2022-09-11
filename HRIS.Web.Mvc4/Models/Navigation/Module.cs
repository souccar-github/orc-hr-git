using System.Web;
using Project.Web.Mvc4.Models.Controls;
using Microsoft.Ajax.Utilities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System.Collections.Generic;
using System.Linq;
using Souccar.Domain.Security;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Factories;
using Project.Web.Mvc4.Models.GridModel;

namespace Project.Web.Mvc4.Models.Navigation
{
    [Module("Navigation")]
    public class Module : NavigationItem, IAggregateRoot, IAuthorizable
    {
        public virtual string ModuleId { get; set; }

        public virtual IList<Dashboard> Dashboards { get; set; }
        public virtual IList<Aggregate> Aggregates { get; set; }
        public virtual IList<Index> Indexes { get; set; }
        public virtual IList<Service> Services { get; set; }
        public virtual IList<Report> Reports { get; set; }
        public virtual IList<Configuration> Configurations { get; set; }

        public Module()
        {
            Dashboards = new List<Dashboard>();
            Aggregates = new List<Aggregate>();
            Indexes = new List<Index>();
            Services = new List<Service>();
            Reports = new List<Report>();
            Configurations = new List<Configuration>();
        }
        public void UpdateReport()
        {
            Reports = ReportFactory.Create(ModuleId);
        }
        public IList<DualSelectListModel> GetAuthorizeAggregates
        {
            get
            {
                return Aggregates.Select(
                        x =>
                            new DualSelectListModel()
                            {
                                Group = ModuleId,
                                Value = x.SecurityId,
                                //Value = x.AggregateId,
                                Title = x.Title,
                                Dir = "Left"
                            }).ToList();
            }
        }

        public IList<DualSelectListModel> GetAuthorizeActionListCommands
        {
            get
            {
                var result = new List<DualSelectListModel>();
                foreach (var aggregate in Aggregates)
                {
                    foreach (var command in aggregate.ActionListCommands)
                    {
                        if (result.All(x => !x.Value.Equals(command.HandlerName)))
                        {
                            result.Add(getDualSelectListModelByActionListCommand(command, aggregate.SecurityId));
                        }
                    }
                    foreach (var detail in aggregate.Details)
                    {
                        getAllAuthorizeActionListCommands(detail, result);
                    }
                }

                foreach (var configuration in Configurations)
                {
                    foreach (var command in configuration.ActionListCommands)
                    {
                        if (result.All(x => !x.Value.Equals(command.HandlerName)))
                        {
                            result.Add(getDualSelectListModelByActionListCommand(command, configuration.SecurityId));
                        }
                    }
                    foreach (var detail in configuration.Details)
                    {
                        getAllAuthorizeActionListCommands(detail, result);
                    }
                }
                return result;
            }
        }

        private void getAllAuthorizeActionListCommands(Detail detail, IList<DualSelectListModel> result)
        {
            if (detail == null)
                return;
            foreach (var command in detail.ActionListCommands)
            {
                if (result.All(x => !x.Value.Equals(command.HandlerName)))
                {
                    result.Add(getDualSelectListModelByActionListCommand(command, detail.SecurityId));
                }
            }
            foreach (var d in detail.Details)
            {
                getAllAuthorizeActionListCommands(d, result);
            }
        }

        // Add new Parameter "ParentName" to send the to the client side
        private DualSelectListModel getDualSelectListModelByActionListCommand(ActionListCommand command, string ParentName)
        {
            return new DualSelectListModel()
            {
                Group = ModuleId,
                Parent = ParentName,
                //Value = detail.DetailId,
                Value = command.HandlerName,
                Title = command.Name,
                Dir = "Left"
            };
        }

        public IList<DualSelectListModel> GetAuthorizeDetails
        {
            get
            {
                var result = new List<DualSelectListModel>();
                foreach (var aggregate in Aggregates)
                {
                    foreach (var detail in aggregate.Details)
                    {
                        getAllAuthorizeDetails(detail, result, aggregate.SecurityId);
                    }
                }

                foreach (var configuration in Configurations)
                {
                    foreach (var detail in configuration.Details)
                    {
                        getAllAuthorizeDetails(detail, result, configuration.SecurityId);
                    }
                }
                return result;
            }
        }

        // Add new parameter "ParentName" to send detail parent to client side
        private void getAllAuthorizeDetails(Detail detail, IList<DualSelectListModel> result, string parentName)
        {
            if (detail == null)
                return;
            if (result.All(x => !x.Value.Equals(detail.DetailId)))
            {
                result.Add(new DualSelectListModel()
                {
                    Group = ModuleId,
                    Parent = parentName,
                    //Value = detail.DetailId,
                    Value = detail.SecurityId,
                    Title = detail.Title,
                    Dir = "Left"
                });
            }
            foreach (var d in detail.Details)
            {
                getAllAuthorizeDetails(d, result, detail.SecurityId);
            }
        }

        public IList<DualSelectListModel> GetAuthorizeDashboards
        {
            get
            {
                return Dashboards.Select(
                        x =>
                            new DualSelectListModel()
                            {
                                Group = ModuleId,
                                Value = x.SecurityId,
                                //Value = x.DashboardId,
                                Title = x.Title,
                                Dir = "Left"
                            }).ToList();
            }
        }

        public IList<DualSelectListModel> GetAuthorizeIndexs
        {
            get
            {
                return Indexes.Select(
                        x =>
                            new DualSelectListModel()
                            {
                                Group = ModuleId,
                                Value = x.SecurityId,
                                //Value = x.IndexId,
                                Title = x.Title,
                                Dir = "Left"
                            }).ToList();
            }
        }

        public IList<DualSelectListModel> GetAuthorizeServices
        {
            get
            {
                return Services.Select(
                        x =>
                            new DualSelectListModel()
                            {
                                Group = ModuleId,
                                Value = x.SecurityId,
                                Title = x.Title,
                                Dir = "Left"
                            }).ToList();
            }
        }

        public IList<DualSelectListModel> GetAuthorizeConfigurations
        {
            get
            {
                return Configurations.Select(
                        x =>
                            new DualSelectListModel()
                            {
                                Group = ModuleId,
                                //Value = x.ConfigurationId,
                                Value = x.SecurityId,
                                Title = x.Title,
                                Dir = "Left"
                            }).ToList();
            }
        }

        public IList<DualSelectListModel> GetAuthorizeReports
        {
            get
            {
                return Reports.Select(
                        x =>
                            new DualSelectListModel()
                            {
                                Group = ModuleId,
                                //Value = x.ReportId,
                                Value = x.SecurityId,
                                Title = ServiceFactory.LocalizationService.GetResource(x.FileName) ?? x.FileName,
                                Dir = "Left"
                            }).ToList();
            }
        }

        public Aggregate GetAggregate(string aggregateId)
        {
            return Aggregates.SingleOrDefault(a => a.AggregateId == aggregateId);
        }
        public Service GetService(string serviceId)
        {
            return Services.SingleOrDefault(a => a.ServiceId == serviceId);
        }
        public List<Index> GetIndexes(string indexId)
        {
            return Indexes.Where(a => a.IndexId.Contains(indexId)).ToList();
        }
        public Configuration GetConfiguration(string configurationId)
        {
            return Configurations.SingleOrDefault(a => a.ConfigurationId == configurationId);
        }

        public Dashboard GetDashboard(string dashboardId)
        {
            return Dashboards.SingleOrDefault(a => a.DashboardId == dashboardId);
        }
        public Report GetReport(string reportId)
        {
            return Reports.SingleOrDefault(a => a.ReportId == reportId);
        }
    }
}
