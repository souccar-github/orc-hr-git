

using System;
using Souccar.Domain.DomainModel;

namespace Souccar.ReportGenerator.Domain.QueryBuilder
{
    [Serializable]
    public class SortDescriptor : ValueObject
    {
        /// <summary>
        /// Get or set the <see cref="ListSortDirection"/> of this <see cref="SortDescriptor"/>.
        /// </summary>
        public ListSortDirection SortDirection { get; set; }

        /// <summary>
        /// Get or Set the order of the column sorting represented by this <see cref="SortDescriptor"/>.
        /// If the value is zero then the column is not selected for sorting.
        /// </summary>
        public int SortOrder { get; set; }
    }
}
