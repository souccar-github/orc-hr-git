using System;
using System.Collections.Generic;
using System.Linq;


namespace Souccar.Reflector
{
    public class ClassTree : IEquatable<ClassTree>
    {
        /// <summary>
        /// Get or Set the list of primitive, string and datetime properties  which are found in the type represented by this class tree.
        /// </summary>
        public List<SimpleProperty> SimpleProperties { get; set; }
        /// <summary>
        /// Get or Set the list of details properties  which are found in the type represented by this class tree.
        /// </summary>
        public List<ReferenceProperty> ReferencedByProperties { get; set; }
        /// <summary>
        /// Get or Set the list of reference properties  which are found in the type represented by this class tree.
        /// </summary>
        public List<ReferenceProperty> ReferencesProperties { get; set; }
        /// <summary>
        /// Get or Set the short name of the type represented by this class tree.
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// Get or Set the type represented by this class tree.
        /// </summary>
        public Type Type { get; set; }

        public ClassTree()
        {
            SimpleProperties = new List<SimpleProperty>();
            ReferencedByProperties = new List<ReferenceProperty>();
            ReferencesProperties = new List<ReferenceProperty>();
        }

        public void AddSimpleProperty(SimpleProperty simpleProperty)
        {
            SimpleProperties.Add(simpleProperty);
        }

        public void AddCollectionProperty(ReferenceProperty collectionProperty)
        {
            ReferencedByProperties.Add(collectionProperty);
        }

        public void AddReferenceProperty(ReferenceProperty referenceProperty)
        {
            ReferencesProperties.Add(referenceProperty);
        }

        public bool Equals(ClassTree other)
        {
            return Name.Equals(other.Name) && SimpleProperties.SequenceEqual(other.SimpleProperties) && ReferencedByProperties.SequenceEqual(other.ReferencedByProperties)
            && ReferencesProperties.SequenceEqual(other.ReferencesProperties);
        }
    }
}
