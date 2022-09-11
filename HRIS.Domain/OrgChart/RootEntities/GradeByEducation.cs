using HRIS.Domain.Global.Constant;
using HRIS.Domain.OrgChart.Entities;
using HRIS.Domain.OrgChart.Enum;
using HRIS.Domain.OrgChart.Indexes;
using HRIS.Domain.Personnel.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.OrgChart.RootEntities
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    [Order(2)]
    [Module(ModulesNames.Grade)]
    public class GradeByEducation : Entity, IAggregateRoot
    {
        public GradeByEducation()
        {
            GradeByEducationQualifications = new List<GradeByEducationQualification>();
        }

        #region Basic Info.
        [UserInterfaceParameter(Order = 1)]
        public virtual string Name { get; set; }

        [UserInterfaceParameter(Order = 2)]
        public virtual int Order { get; set; }

        [UserInterfaceParameter(Order = 3, Step = 1000)]
        public virtual float MinSalary { get; set; }
        [UserInterfaceParameter(Order = 4, Step = 1000)]
        public virtual float MaxSalary { get; set; }

        [UserInterfaceParameter(Order = 5, Step = 1000)]
        public virtual float MinBenefitSalary { get; set; }
        [UserInterfaceParameter(Order = 6, Step = 1000)]
        public virtual float MaxBenefitSalary { get; set; }
       
        [UserInterfaceParameter(Order = 7)]
        public virtual CurrencyType CurrencyType { get; set; }

        [UserInterfaceParameter(Order = 8)]
        public virtual string Description { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return Name; } }

        #endregion

        #region Grade By Education Qualifications

        public virtual IList<GradeByEducationQualification> GradeByEducationQualifications { get; protected set; }

        public virtual void AddGradeByEducationQualification(GradeByEducationQualification gradeByEducationQualification)
        {
            gradeByEducationQualification.GradeByEducation = this;
            GradeByEducationQualifications.Add(gradeByEducationQualification);
        }

        #endregion
    }
}
