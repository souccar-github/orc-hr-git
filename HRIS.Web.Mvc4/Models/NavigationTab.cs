using  Project.Web.Mvc4.Models.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Models
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class NavigationTab
    {
        public NavigationTab()
        {
            this.Modules = new List<Module>();
        }
        public string Name { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
        public IList<Module> Modules { get; set; }
    }
    public class Tab
    {
        public Tab()
        {
            Modules = new List<TabModule>();
        }
        public string Name { get; set; }
        public int Order { get; set; }
        public IList<TabModule> Modules { get; set; }
    }
    public class TabModule
    {
        public string Name { get; set; }
        public int Order { get; set; }
    }
}