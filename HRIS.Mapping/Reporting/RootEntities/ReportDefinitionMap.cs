using FluentNHibernate.Mapping;
using Souccar.Domain.Reporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Mapping.Report.RootEntities
{
     public sealed class ReportDefinitionMap : ClassMap<ReportDefinition>
     {
         public ReportDefinitionMap()
         {
             #region Default
             DynamicUpdate();
             DynamicInsert();
             Id(x => x.Id);
             Map(x => x.IsVertualDeleted);
             #endregion

             Map(x => x.Title).Not.Nullable();
             Map(x => x.Description);
             Map(x => x.ModuleName);
             Map(x => x.CreationDate);
             Map(x => x.FileName);
             References(x => x.CreatedBy);
         }
     }
}
