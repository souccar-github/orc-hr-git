using System;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Souccar.Core.Utilities
{
    /// <summary>
    /// Provides a method for performing a deep copy of an object.
    /// Binary Serialization is used to perform the copy.
    /// </summary>
    public static class ObjectCopier
    {
        /// <summary>
        /// Perform a deep Copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        // <summary>
        /// Copies the data of one object to another. The target object gets properties of the first. 
        /// Any matching properties (by name) are written to the target.
        /// </summary>
        /// <param name="source">The source object to copy from</param>
        /// <param name="target">The target object to copy to</param>
        public static T Copy<T>(this T source) where T:new()
        {
            var target = new T();

            CopyObjectData(source, target, String.Empty, BindingFlags.Public | BindingFlags.Instance);
            return target;
        }

        public static byte[] ToArray<T>(this T source) where T : new()
        {
            IFormatter formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return stream.ToArray();
            }
        }

        // <summary>
        /// Copies the data of one object to another. The target object gets properties of the first. 
        /// Any matching properties (by name) are written to the target.
        /// </summary>
        /// <param name="source">The source object to copy from</param>
        /// <param name="target">The target object to copy to</param>
        public static void CopyObjectData(object source, object target)
        {
            CopyObjectData(source, target, String.Empty, BindingFlags.Public | BindingFlags.Instance);
        }

        /// <summary>
        /// Copies the data of one object to another. The target object gets properties of the first. 
        /// Any matching properties (by name) are written to the target.
        /// </summary>
        /// <param name="source">The source object to copy from</param>
        /// <param name="target">The target object to copy to</param>
        /// <param name="excludedProperties">A comma delimited list of properties that should not be copied</param>
        /// <param name="memberAccess">Reflection binding access</param>
        public static void CopyObjectData(object source, object target, string excludedProperties, BindingFlags memberAccess)
        {
            string[] excluded = null;
            if (!string.IsNullOrEmpty(excludedProperties))
            {
                excluded = excludedProperties.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }

            var miT = target.GetType().GetMembers(memberAccess);
            foreach (var field in miT)
            {
                string name = field.Name;

                // Skip over excluded properties
                if (excluded!=null && excluded.Contains(name))
                {
                    continue;
                }


                if (field.MemberType == MemberTypes.Field)
                {
                    var sourcefield = source.GetType().GetField(name);
                    if (sourcefield == null) { continue; }

                    var sourceValue = sourcefield.GetValue(source);
                    ((FieldInfo)field).SetValue(target, sourceValue);
                }
                else if (field.MemberType == MemberTypes.Property)
                {
                    var piTarget = field as PropertyInfo;
                    var sourceField = source.GetType().GetProperty(name, memberAccess);
                    if (sourceField == null) { continue; }

                    if (piTarget!=null && piTarget.CanWrite && sourceField.CanRead)
                    {
                        object targetValue = piTarget.GetValue(target, null);
                        object sourceValue = sourceField.GetValue(source, null);

                        if (sourceValue == null) { continue; }

                        if (sourceField.PropertyType.IsArray && piTarget.PropertyType.IsArray)
                        {
                            CopyArray(source, target, memberAccess, piTarget, sourceField, sourceValue);
                        }
                        else
                        {
                            CopySingleData(source, target, memberAccess, piTarget, sourceField, targetValue, sourceValue);
                        }
                    }
                }
            }
        }

        private static void CopySingleData(object source, object target, BindingFlags memberAccess, PropertyInfo piTarget, PropertyInfo sourceField, object targetValue, object sourceValue)
        {
            //instantiate target if needed
            if (targetValue == null
                && piTarget.PropertyType.IsValueType == false
                && piTarget.PropertyType != typeof(string))
            {
                targetValue = Activator.CreateInstance(piTarget.PropertyType.IsArray ? piTarget.PropertyType.GetElementType() : piTarget.PropertyType);
            }

            if (piTarget.PropertyType.IsValueType == false
                && piTarget.PropertyType != typeof(string))
            {
                CopyObjectData(sourceValue, targetValue, "", memberAccess);
                piTarget.SetValue(target, targetValue, null);
            }
            else
            {
                if (piTarget.PropertyType.FullName == sourceField.PropertyType.FullName)
                {
                    object tempSourceValue = sourceField.GetValue(source, null);
                    piTarget.SetValue(target, tempSourceValue, null);
                }
                else
                {
                    CopyObjectData(piTarget, target, "", memberAccess);
                }
            }
        }

        private static void CopyArray(object source, object target, BindingFlags memberAccess, PropertyInfo piTarget, PropertyInfo sourceField, object sourceValue)
        {
            var sourceLength = (int)sourceValue.GetType().InvokeMember("Length", BindingFlags.GetProperty, null, sourceValue, null);
            var targetArray = Array.CreateInstance(piTarget.PropertyType.GetElementType(), sourceLength);
            var array = (Array)sourceField.GetValue(source, null);

            for (var i = 0; i < array.Length; i++)
            {
                var o = array.GetValue(i);
                var tempTarget = Activator.CreateInstance(piTarget.PropertyType.GetElementType());
                CopyObjectData(o, tempTarget, "", memberAccess);
                targetArray.SetValue(tempTarget, i);
            }
            piTarget.SetValue(target, targetArray, null);
        }

    }
}