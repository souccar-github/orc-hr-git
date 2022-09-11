using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.JobDescription.Entities
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class PositionReporting:HistoryEntity
    {
        public PositionReporting()
        {
            FromDate = DateTime.Today;
        }
        public virtual Position Position { get; set; }
        public virtual Position ManagerPosition { get; set; }
        public virtual bool IsPrimary { get; set; }

    }
}
