using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Souccar.Core.CustomAttribute
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class UserInterfaceParameterAttribute : Attribute
    {
        public string Group { get; set; }
        public bool IsHidden { get; set; }
        public bool IsNonEditable { get; set; }
        public int Width { get; set; }
        public int Order { get; set; }
        public bool IsReference { get; set; }
        public string ReferenceReadUrl { get; set; }
        public string CascadeFrom { get; set; }
        public int Step { get; set; }
        public bool IsImageColumn { get; set; }
        public bool IsFile { get; set; }
        public bool IsDateTime { get; set; }
        public bool IsTime { get; set; }
        /// <summary>
        /// File size by byte
        /// </summary>
        public int FileSize { get; set; }
        public string AcceptExtension{ get; set; }
        public string ImageColumnPath { get; set; }
        public string DefaultImageName { get; set; }
        public string DisplayFieldName { get; set; }
        public IList<int> ViewsIds { get; set; }
    }
}
