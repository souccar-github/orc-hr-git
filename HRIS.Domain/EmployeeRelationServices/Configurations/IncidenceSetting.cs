using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.Personnel.Enums;
using Souccar.Domain.DomainModel;
using HRIS.Domain.EmployeeRelationServices.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Infrastructure.Core;

namespace HRIS.Domain.EmployeeRelationServices.Configurations
{
    public class IncidenceSetting:Entity,IConfigurationRoot
    {
        public virtual IncidenceType IncidenceType { get; set; }
        public virtual AdditionalIncidenceType AdditionalIncidenceType { get; set; }

        public virtual ContractType ContractType { get; set; }
        public virtual ServiceEndType ServiceEndType { get; set; }
        public virtual TransportType TransportType { get; set; }

        public virtual EmploymentStatus EmploymentStatus { get; set; }
        public virtual EmployeeStatus EmployeeStatus { get; set; }
        public virtual EmployeeCountStatus EmployeeCountStatus { get; set; }
        public virtual SalaryStatus SalaryStatus { get; set; }

        public virtual bool IsEmploymentStatusAffected { get; set; }
        public virtual bool IsEmployeeStatusAffected { get; set; }
        public virtual bool IsEmployeeCountStatusAffected { get; set; }
        public virtual bool IsSalaryStatusAffected { get; set; }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return ServiceFactory.LocalizationService.GetResource("HRIS.Domain.EmployeeRelationServices.Enums.IncidenceType." + Enum.GetName(typeof(IncidenceType), IncidenceType)); } }
    }
}
