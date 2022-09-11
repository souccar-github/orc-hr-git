using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Models
{
    public class TreeViewModel
    {
        public TreeViewModel()
        {
            Tags=new List<string>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageName { get; set; }
        public bool HasChildren { get; set; }
        public List<string> Tags { get; set; }
        public IList<TreeViewModel> Items { get; set; }
    }
}