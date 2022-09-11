using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using  Project.Web.Mvc4.Models.Controls;

namespace Project.Web.Mvc4.Areas.Security.Models
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class ManageRoleViewModel
    {
        public ManageRoleViewModel()
        {
            AuthorizableModules = new List<DualSelectListModel>();
            AuthorizableDashboards = new List<DualSelectListModel>();
            AuthorizableAggregates = new List<DualSelectListModel>();
            AuthorizableIndexs = new List<DualSelectListModel>();
            AuthorizableServices = new List<DualSelectListModel>();
            AuthorizableReports = new List<DualSelectListModel>();
            AuthorizableConfigurations = new List<DualSelectListModel>();
            AuthorizableDetails = new List<DualSelectListModel>();
            AuthorizableActionLists = new List<DualSelectListModel>();
            AuthorizableConfigurationField=new List<DualSelectListModel>();
            AuthorizableAggregateField=new List<DualSelectListModel>();
            AuthorizableDetailField = new List<DualSelectListModel>();

        }
        public int RoleId { get; set; }
        public IList<DualSelectListModel> AuthorizableModules { get; set; }

        public IList<DualSelectListModel> AuthorizableDashboards { get; set; }

        public IList<DualSelectListModel> AuthorizableAggregates { get; set; }

        public IList<DualSelectListModel> AuthorizableIndexs { get; set; }

        public IList<DualSelectListModel> AuthorizableServices { get; set; }

        public IList<DualSelectListModel> AuthorizableReports { get; set; }

        public IList<DualSelectListModel> AuthorizableConfigurations { get; set; }

        public IList<DualSelectListModel> AuthorizableDetails { get; set; }
        public IList<DualSelectListModel> AuthorizableActionLists { get; set; }
        public IList<DualSelectListModel> AuthorizableConfigurationField { get; set; }
        public IList<DualSelectListModel> AuthorizableAggregateField { get; set; }
        public IList<DualSelectListModel> AuthorizableDetailField { get; set; }

       

    }
}