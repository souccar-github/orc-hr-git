#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Recruitment.Entities;
using HRIS.Domain.Recruitment.Enums;
using HRIS.Domain.Recruitment.Helpers;
using HRIS.Domain.Recruitment.Indexes;
using HRIS.Domain.Recruitment.RootEntities;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Personnel.Helpers;
//using MaritalStatus = HRIS.Domain.Personnel.Indexes.MaritalStatus;
using Souccar.Infrastructure.Core;
using Souccar.NHibernate;
using LinqToExcel.Attributes;

#endregion

namespace HRIS.Domain.Recruitment.RootEntities
{
    [Module(ModulesNames.Recruitment)]
    [Order(1)]
    public class Applicant : EmployeeBase
    {
        public Applicant()
        {
            RChildren = new List<RChild>();
            RSpouse = new List<RSpouse>();
            REducations = new List<REducation>();
            RJobRelatedInfos = new List<RJobRelatedInfo>();
        }

        #region Spouse

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.Family, Order = 1)]
        public virtual IList<RSpouse> RSpouse { get; protected set; }

        public virtual void AddSpouse(RSpouse spouse)
        {
            spouse.Applicant = this;
            RSpouse.Add(spouse);
        }

        #endregion

        #region Children

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.Family, Order = 2)]
        public virtual IList<RChild> RChildren { get; protected set; }

        public virtual void AddChild(RChild child)
        {
            child.Applicant = this;
            RChildren.Add(child);
        }

        #endregion

        #region Education

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.Qualification, Order = 3)]
        public virtual IList<REducation> REducations { get; protected set; }

        public virtual void AddEducation(REducation education)
        {
            education.Applicant = this;
            REducations.Add(education);
        }

        #endregion

        #region JobRelatedInfos

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.General, Order = 4)]
        public virtual IList<RJobRelatedInfo> RJobRelatedInfos { get; protected set; }

        public virtual void AddJobRelatedInfo(RJobRelatedInfo jobRelatedInfo)
        {
            jobRelatedInfo.Applicant = this;
            RJobRelatedInfos.Add(jobRelatedInfo);
        }

        #endregion

    }
}