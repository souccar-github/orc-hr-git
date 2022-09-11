using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Models.GridModel
{
    public enum BuiltinCommand
    {
        Create,
        Cancel,
        Save
    }

    public class ToolbarCommand
    {
        public string Text { get; set; }
        public string Name { get; set; }
        public string Template { get; set; }
        public string ImageClass { get; set; }
        public string ClassName { get; set; }

        public bool Additional { get; set; }
        public string Handler { get; set; }
    }
}