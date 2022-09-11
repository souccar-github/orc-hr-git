using System;
using Souccar.Domain.Extensions;
using Souccar.Reflector;

namespace Souccar.ReportGenerator.Domain.QueryBuilder
{
    public class QueryTreeFactory
    {
        /// <summary>
        ///     Create the query tree matching that represents the type.
        /// </summary>
        /// <param name="type">The type to be reflected</param>
        /// <returns>The query tree matching the type.</returns>
        public static QueryTree Create(Type type)
        {
            ClassTree classTree = ClassTreeFactory.Create(type);
            return Create(classTree);
        }

        /// <summary>
        ///     Create a query tree from the provided ClassTree.
        ///     Only the properties with setter properties will be added to the query tree.
        ///     The primarykey property will not be added.
        /// </summary>
        /// <param name="typeClassTree">The ClassTree to transform.</param>
        /// <param name="currentFullClassPath">The FullClassPath of the parent node.</param>
        /// <param name="parentType">The type of the parent node.</param>
        /// <param name="selectedOrder">The select order of the query tree.</param>
        /// <returns>The query tree matching the provided ClassTree.</returns>
        private static QueryTree Create(ClassTree typeClassTree, string currentFullClassPath = "",
            Type parentType = null, int selectedOrder = 0)
        {
            var result = new QueryTree
            {
                FullClassName = typeClassTree.Type.FullName,
                Type = typeClassTree.Type,
                FullClassPath =
                    currentFullClassPath != String.Empty
                        ? currentFullClassPath
                        : typeClassTree.Type.Name,
                DefiningType = parentType,
                SelectOrder = selectedOrder
            };
            foreach (SimpleProperty simpleProperty in typeClassTree.SimpleProperties)
                if (typeClassTree.Type.GetProperty(simpleProperty.Name).CanWrite && !simpleProperty.IsPrimaryKey)
                    result.Leaves.Add(new QueryLeaf
                    {
                        IsReference = false,
                        IsPrimitive = true,
                        PropertyType = simpleProperty.PropertyType,
                        ParentType = typeClassTree.Type,
                        PropertyName = simpleProperty.Name,
                        DefiningType = typeClassTree.Type,
                        PropertyFullPath =
                            String.Format("{0}.{1}", result.FullClassPath, simpleProperty.Name)
                    });
            int i = 1;
            foreach (ReferenceProperty referencesProperty in typeClassTree.ReferencesProperties)
            {
                if (typeClassTree.Type.GetProperty(referencesProperty.Name).CanWrite)
                    if (!referencesProperty.PropertyType.IsEntity())
                    {
                        foreach (SimpleProperty simpleProperty in referencesProperty.ClassTree.SimpleProperties)
                            if (referencesProperty.PropertyType.GetProperty(simpleProperty.Name).CanWrite &&
                                !simpleProperty.IsPrimaryKey)
                                result.Leaves.Add(new QueryLeaf
                                {
                                    IsReference = true,
                                    IsPrimitive = true,
                                    PropertyType = simpleProperty.PropertyType,
                                    ParentType = referencesProperty.PropertyType,
                                    PropertyName =
                                        String.Format("{0}.{1}", referencesProperty.Name,
                                            simpleProperty.Name),
                                    DefiningType = typeClassTree.Type,
                                    PropertyFullPath =
                                        String.Format("{0}.{1}.{2}", result.FullClassPath,
                                            referencesProperty.Name, simpleProperty.Name)
                                });
                    }
                    else
                    {
                        string fullClassPathtemp = currentFullClassPath != string.Empty
                    ? String.Format("{0}.{1}", currentFullClassPath, referencesProperty.Name)
                    : String.Format("{0}.{1}", typeClassTree.Name, referencesProperty.Name);
                        result.Nodes.Add(Create(referencesProperty.ClassTree, fullClassPathtemp, typeClassTree.Type, i++));
                    }
            }
            foreach (ReferenceProperty referencedByProperty in typeClassTree.ReferencedByProperties)
            {
                string fullClassPathtemp = currentFullClassPath != string.Empty
                    ? String.Format("{0}.{1}", currentFullClassPath, referencedByProperty.Name)
                    : String.Format("{0}.{1}", typeClassTree.Name, referencedByProperty.Name);
                result.Nodes.Add(Create(referencedByProperty.ClassTree, fullClassPathtemp, typeClassTree.Type, i++));
            }
            return result;
        }
    }
}