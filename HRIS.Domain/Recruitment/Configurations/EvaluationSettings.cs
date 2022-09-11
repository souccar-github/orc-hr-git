using System;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Grades.RootEntities;
using HRIS.Domain.Recruitment.Enums;
using HRIS.Domain.Recruitment.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.OrganizationChart.RootEntities;

namespace HRIS.Domain.Recruitment.Configurations
{
    [Module(ModulesNames.Recruitment)]
    [Order(2)]
    public class EvaluationSettings : Entity, IConfigurationRoot
    {

        #region Basic Info

        [UserInterfaceParameter(Order = 1)]
        public virtual RecruitmentType RecruitmentType { get; set; }

        [UserInterfaceParameter(IsReference = true, Order = 2)]
        public virtual Grade Grade { get; set; }

        [UserInterfaceParameter(Order = 3)]
        public virtual int FullSuccessMark { get; set; }

        [UserInterfaceParameter(Order = 4)]
        public virtual int MinSuccessMark { get; set; }

        [UserInterfaceParameter(Order = 5)]
        public virtual int WrittenWeightFactor { get; set; }

        [UserInterfaceParameter(Order = 6)]
        public virtual int MinWrittenMark { get; set; }

        [UserInterfaceParameter(Order = 7)]
        public virtual int OralWeightFactor { get; set; }

        [UserInterfaceParameter(Order = 8)]
        public virtual int MinOralMark { get; set; }

        [UserInterfaceParameter(Order = 9)]
        public virtual int OldnessWeightFactor { get; set; }

        [UserInterfaceParameter(Order = 10)]
        public virtual DateTime LaborOfficeStartingDate { get; set; }

        [UserInterfaceParameter(Order = 11)]
        public virtual RoundType RoundType { get; set; }

        [UserInterfaceParameter(Order = 12)]
        public virtual int MartyrSonFactor { get; set; }

        #endregion

    }
}
