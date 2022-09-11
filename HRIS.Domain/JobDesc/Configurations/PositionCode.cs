#region

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.JobDescription.Indexes;
using HRIS.Domain.OrganizationChart.Indexes;
using HRIS.Domain.OrganizationChart.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

using HRIS.Domain.Global.Entities;

#endregion

namespace HRIS.Domain.JobDescription.Configurations
{
    [Module(ModulesNames.JobDescription)]
    [Order(3)]
    public class PositionCode : CodeSetting, IConfigurationRoot
    {
    }
}
