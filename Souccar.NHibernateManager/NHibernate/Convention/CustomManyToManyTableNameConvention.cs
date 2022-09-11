using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Inspections;

namespace NHibernateDBGenerator.NHibernate.Convention
{
    public class CustomManyToManyTableNameConvention : ManyToManyTableNameConvention
    {
        private string _manyToManyTableName = string.Empty;
        private const string Prefix = "G";
        private const int DbObjectMaximumLength = 30;

        protected override string GetBiDirectionalTableName(IManyToManyCollectionInspector collection, IManyToManyCollectionInspector otherSide)
        {
            return ConcatenateManyToManyTableName(collection.EntityType.Name, otherSide.EntityType.Name);
        }

        protected override string GetUniDirectionalTableName(IManyToManyCollectionInspector collection)
        {
            return ConcatenateManyToManyTableName(collection.EntityType.Name, collection.ChildType.Name);
        }

        protected string ConcatenateManyToManyTableName(string firstTableName, string secondTableName)
        {
            _manyToManyTableName = string.Empty;
            _manyToManyTableName = firstTableName + "To" + secondTableName;
            _manyToManyTableName = _manyToManyTableName.Length > DbObjectMaximumLength ?
                _manyToManyTableName.Substring(0, DbObjectMaximumLength - 1).Insert(0, Prefix) : _manyToManyTableName;

            return _manyToManyTableName;
        }
    }

}