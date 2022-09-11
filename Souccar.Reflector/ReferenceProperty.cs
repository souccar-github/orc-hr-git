using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Souccar.Reflector;

namespace Souccar.Reflector
{
    public class ReferenceProperty : IEquatable<ReferenceProperty>
    {
        /// <summary>
        /// Get or set The name of the property.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Get or set the classtree representing the type of this property.
        /// </summary>
        public ClassTree ClassTree { get; set; }
        /// <summary>
        /// Get or set the short type name of the type containing this property.       
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// Get or set the type of this property.
        /// </summary>
        public Type PropertyType { get; set; }

        public bool Equals(ReferenceProperty other)
        {
            return Name.Equals(other.Name) && ClassName.Equals(other.ClassName) && ClassTree.Equals(other.ClassTree)&& PropertyType.Name.Equals(other.PropertyType.Name);
        }
    }
}
