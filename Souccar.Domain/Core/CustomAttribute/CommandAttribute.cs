using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Souccar.Core.CustomAttribute
{
    /// <summary>
    /// Author: Yaseen Alrefaee dema dadad dfhydfhdfh
    /// </summary>
    [AttributeUsage(AttributeTargets.Class,AllowMultiple=true)]
    public class CommandAttribute:Attribute
    {
        private CommandAttribute()
        {
            GroupId = 1;
            Order = 1;
        }
        public CommandAttribute(string name):base()
        {
            Name = name;
            GroupId = 1;
            Order = 1;
        }
        public string Name { get; set; }
        public int  Order{ get; set; }
        public int GroupId { get; set; }
        public string StyleName{ get; set; }
        public bool IsToolbarCommand { get; set; }
        public bool IsAdditional { get; set; }

    }
}
