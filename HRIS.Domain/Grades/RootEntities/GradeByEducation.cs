using HRIS.Domain.Global.Constant;
using HRIS.Domain.Grades.Entities;
using HRIS.Domain.OrganizationChart.Enum;
using HRIS.Domain.OrganizationChart.Indexes;
using HRIS.Domain.Personnel.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Grades.RootEntities
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    [Order(2)]
    [Module(ModulesNames.Grades)]
    public class GradeByEducation : Entity, IAggregateRoot
    {
        public GradeByEducation()
        {
            GradeByEducationQualifications = new List<GradeByEducationQualification>();
        }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return Name; } }
        #region Basic Info.
        [UserInterfaceParameter(Order = 1)]
        public virtual string Name { get; set; }

        [UserInterfaceParameter(Order = 2)]
        public virtual int Order { get; set; }

        //[UserInterfaceParameter(Order = 3, Step = 1000, IsHidden = true)]
        [UserInterfaceParameter(Order = 3, Step = 1000)]
        public virtual float MinSalary { get; set; }

        //[UserInterfaceParameter(Order = 4, Step = 1000, IsHidden = true)]
        [UserInterfaceParameter(Order = 4, Step = 1000)]
        public virtual float MaxSalary { get; set; }

        //[UserInterfaceParameter(Order = 5, IsHidden = true)]
        [UserInterfaceParameter(Order = 5)]
        public virtual float MidSalary
        {
            get { return (MaxSalary + MinSalary) / 2; }
        }
       
        [UserInterfaceParameter(Order = 6)]
        public virtual CurrencyType CurrencyType { get; set; }

        [UserInterfaceParameter(Order = 7)]
        public virtual string Description { get; set; }

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
