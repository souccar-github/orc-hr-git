using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.Grades.Entities;
using Souccar.Core;

namespace HRIS.Mapping.Grade.Entities
{
    
    public sealed class GradeStepMap : ClassMap<GradeStep>
    {
        public GradeStepMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Name).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Order).Column("Step_Order");
          
            Map(x => x.MinSalary);
            Map(x => x.MaxSalary);

            References(x => x.CurrencyType);
            Map(x => x.Description);

            References(x => x.Grade);

        }
    }
}
