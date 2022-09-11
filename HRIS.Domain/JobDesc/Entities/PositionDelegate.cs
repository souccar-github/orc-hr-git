using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.JobDescription.Indexes;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.JobDescription.Entities
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class PositionDelegate : HistoryEntity
    {
        public PositionDelegate()
        {
            FromDate = DateTime.Today;
        }
        public virtual AuthorityType AuthorityType { get; set; }

        public virtual Position PrimaryPosition { get; set; }
        public virtual Position SecondaryPosition { get; set; }
    }
}
