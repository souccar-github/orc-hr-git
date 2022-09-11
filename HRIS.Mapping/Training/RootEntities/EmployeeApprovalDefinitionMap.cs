//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//project manager:
//supervisor:
//author: Ammar Alziebak
//description:
//start date:
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
using FluentNHibernate.Mapping;
using HRIS.Domain.Training.RootEntities;

namespace HRIS.Mapping.Training.RootEntities
{

    public sealed class EmployeeApprovalDefinitionMap : ClassMap<EmployeeApprovalDefinition>
    {
        public EmployeeApprovalDefinitionMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.DocumentType);
            Map(x => x.DocumentNumber);
            Map(x => x.DocumentDate);
            Map(x => x.Description);

        }
    }

}
