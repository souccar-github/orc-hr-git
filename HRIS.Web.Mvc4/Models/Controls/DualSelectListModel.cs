using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Models.Controls
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class DualSelectListModel
    {
        public DualSelectListModel()
        {
            this.Metadata = new List<MetadataItem>();
        }
        public string Value { get; set; }
        public int Order { get; set; }
        public string Title { get; set; }
        public string Group { get; set; }
        public string Parent { get; set; }
        public string Description { get; set; }
        public string Dir { get; set; }
        public IList<MetadataItem> Metadata { get; set; }
    }

    public class MetadataItem
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }
    public enum DualSelectListDirection
    {
        Left,
        Right
    }
}