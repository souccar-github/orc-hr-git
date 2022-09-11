using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Models.GridModel
{
    public enum FieldType
    {
        Number,
        String,
        Boolean,
        Date,
        Complex,
        Image,
        File
    }

    [Serializable]
    public class Field
    {
        public Field()
        {
            this.ValidationRules = new Dictionary<string, IDictionary<string, object>>();
        }

        public string Type { get; set; }
        public string Name { get; set; }
        public bool Editable { get; set; }
        public object DefaultValue { get; set; }

        public bool CanView { get; set; }
        public bool CanEdit { get; set; }
        public bool CanInsert { get; set; }

        public IDictionary<string, IDictionary<string, object>> ValidationRules { get; private set; }
    }
}