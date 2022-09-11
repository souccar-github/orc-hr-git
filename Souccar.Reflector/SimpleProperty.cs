using System;
using System.Collections.Generic;
using System.Linq;

namespace Souccar.Reflector
{
    public class SimpleProperty : IEquatable<SimpleProperty>
    {
        /// <summary>
        /// Get or set the name of the property.
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// Get or set the short type name of the type containing this property.       
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// Get or set the type of this property.       
        /// </summary>
        public Type PropertyType { get; set; }

        /// <summary>
        /// return true if the property name is id.
        /// </summary>
        public bool IsPrimaryKey
        {
            get
            {
                return Name.Equals("id", StringComparison.CurrentCultureIgnoreCase);
            }
        }
        public bool Equals(SimpleProperty other)
        {
            return Name.Equals(other.Name) && ClassName.Equals(other.ClassName) && PropertyType.Name.Equals(other.PropertyType.Name);
        }
    }
}
