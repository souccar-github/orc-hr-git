using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.PMS.Configurations;
using HRIS.Domain.PMS.RootEntities;
using HRIS.Domain.Recruitment.Helpers;
using HRIS.Domain.Recruitment.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Workflow.RootEntities;
using HRIS.Domain.Recruitment.Entities.Evaluations;

namespace HRIS.Domain.Recruitment.RootEntities
{
    [Command(CommandsNames.SetJobApplicationStatus)]
    [Module(ModulesNames.Recruitment)]
    [Order(3)]
    public class Interview : Entity, IAggregateRoot
    {
        public Interview()
        {
            Evaluators = new List<Evaluator>();
        }
        #region Main Interview Information

        [UserInterfaceParameter(Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.MainInterviewInformation, Order = 5)]
        public virtual DateTime InterviewDate { get; set; }

        [UserInterfaceParameter(Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.MainInterviewInformation, IsTime = true, Order = 10)]
        public virtual DateTime InterviewStartingTime { get; set; }

        [UserInterfaceParameter(Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.MainInterviewInformation, IsTime = true, Order = 15)]
        public virtual DateTime InterviewEndTime { get; set; }

        //[UserInterfaceParameter(Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.MainInterviewInformation, Order = 20)]
        //public virtual string SubTopic { get; set; }

        [UserInterfaceParameter(Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.MainInterviewInformation, Order = 25)]
        public virtual InterviewType InterviewType { get; set; }

        [UserInterfaceParameter(Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.MainInterviewInformation, IsReference = true, Order = 28)]
        public virtual JobApplication JobApplication { get; set; }

        [UserInterfaceParameter(Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.MainInterviewInformation, Order = 30)]
        public virtual string InterviewGuidelines { get; set; }

        #endregion

        #region Appraisal Info

        [UserInterfaceParameter(Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.AppraisalInfo, IsReference = true, Order = 35)]
        public virtual AppraisalPhaseSetting InterviewAppraisalSetting { get; set; }

        [UserInterfaceParameter(Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.AppraisalInfo, IsReference = true, Order = 40)]
        public virtual AppraisalTemplate InterviewAppraisalTemplate { get; set; }

        #endregion

        [UserInterfaceParameter(IsHidden = true)]
        public virtual float FinalMark { get; set; }

        public virtual WorkflowItem WorkflowItem { get; set; }


        public virtual IList<Evaluator> Evaluators { get; set; }

        public virtual void AddEvaluator(Evaluator evaluator)
        {
            Evaluators.Add(evaluator);
            evaluator.Interview = this;
        }

        public virtual void CalculateFinalMark()
        {
            var evaluators = Evaluators.Where(x => x.Mark != 0).ToList();

            FinalMark = evaluators.Count == 0 ? 0 : evaluators.Sum(x => x.Mark) / evaluators.Count;
        }

    }
}
