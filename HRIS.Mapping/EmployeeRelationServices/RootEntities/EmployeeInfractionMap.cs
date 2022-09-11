#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 11/03/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference
using FluentNHibernate.Mapping;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
namespace HRIS.Mapping.EmployeeRelationServices.RootEntities
{
    public sealed class EmployeeInfractionMap : ClassMap<EmployeeInfraction>
    {
        public EmployeeInfractionMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            
         
            Map(x => x.CreationDate);
          
            Map(x => x.InfractionStatus);

            Map(x => x.PenaltyName);
            References(x => x.Employee);
            References(x => x.Infraction);
           
           
        }
    }
}