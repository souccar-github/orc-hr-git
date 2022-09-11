using System;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using Souccar.ReportGenerator.Domain.Classification;
using System.Collections.Generic;


namespace Souccar.ReportGenerator.Domain.QueryBuilder
{
    [Command("DisplayReport", Order = 1)]
    [Module("ReportGenerator")]
    public class Report : Entity, IAggregateRoot
    {
        public Report()
        {
            QueryTreesList = new List<QueryTree>();
        }
        public virtual String Name { get; set; }
        public virtual IList<QueryTree> QueryTreesList { get; set; }
        public virtual void AddQuery(QueryTree queryTree)
        {
            queryTree.Report = this;
            QueryTreesList.Add(queryTree);
        }
        [UserInterfaceParameter(Order = 1, IsReference = true)]
        public virtual ReportTemplate Template { get; set; }
        //public virtual ReportType ReportType { get; set; }
        public virtual string ReportResourceName { get; set; }
    }
}