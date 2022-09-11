using System;
using Souccar.Domain.DomainModel;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Recruitment.Helpers;
using HRIS.Domain.Recruitment.Enums;
using HRIS.Domain.Recruitment.Indexes;
using HRIS.Domain.JobDescription.Entities;
using Souccar.Domain.Security;
using HRIS.Domain.Recruitment.Entities;
using System.Collections.Generic;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.JobDescription.Indexes;

namespace HRIS.Domain.Recruitment.RootEntities
{
    [Module(ModulesNames.Recruitment)]
    [Command(CommandsNames.SetRecruitmentRequestStatus)]
    [Order(1)]
    public class RecruitmentRequest : Entity, IAggregateRoot
    {
        public RecruitmentRequest()
        {
            RecruitmentRequestAttachments = new List<RecruitmentRequestAttachment>();
        }

        #region Request Information

        [UserInterfaceParameter(Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.RequestInformation, Order = 5)]
        public virtual DateTime RequestDate { get; set; }

        [UserInterfaceParameter(Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.RequestInformation, Order = 10)]
        public virtual PositionBudget PositionBudget { get; set; }

        [UserInterfaceParameter(Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.RequestInformation, Order = 15)]
        public virtual double SalaryRange { get; set; }

        [UserInterfaceParameter(Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.RequestInformation, Order = 20)]
        public virtual DateTime ExpectedHiringDate { get; set; }

        [UserInterfaceParameter(Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.RequestInformation, Order = 25)]
        public virtual string DurationToFillPosition { get; set; }

        [UserInterfaceParameter(Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.RequestInformation, Order = 30)]
        public virtual RequestType RequestType { get; set; }

        [UserInterfaceParameter(Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.RequestInformation, Order = 35)]
        public virtual VacancyReason VacancyReason { get; set; }

        [UserInterfaceParameter(Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.RequestInformation, Order = 40)]
        public virtual JobType JobType { get; set; }

        #endregion

        #region Job Description Info

        [UserInterfaceParameter(Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.JobDescriptionInfo, IsReference = true
            , ReferenceReadUrl = "Recruitment/Reference/GetPositions/", Order = 45)]
        public virtual Position RequestedPosition { get; set; }

        [UserInterfaceParameter(Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.JobDescriptionInfo, Order = 50)]
        public virtual string Node
        {
            get
            {
                return RequestedPosition.JobDescription != null ?
                    (RequestedPosition.JobDescription.Node != null ? RequestedPosition.JobDescription.Node.Name : string.Empty)
                    : string.Empty;
            }
        }

        [UserInterfaceParameter(Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.JobDescriptionInfo, Order = 55)]
        public virtual string NodeType
        {
            get
            {
                return RequestedPosition.JobDescription != null ?
                    (RequestedPosition.JobDescription.Node != null ? RequestedPosition.JobDescription.Node.Type.Name : string.Empty)
                    : string.Empty;
            }
        }

        [UserInterfaceParameter(Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.JobDescriptionInfo, Order = 60)]
        public virtual string PositionGrade
        {
            get
            {
                return RequestedPosition.JobDescription != null ?
                    (RequestedPosition.JobDescription.JobTitle != null ?
                        (RequestedPosition.JobDescription.JobTitle.Grade != null ? RequestedPosition.JobDescription.JobTitle.Grade.Name : string.Empty)
                        : string.Empty)
                    : string.Empty;
            }
        }

        [UserInterfaceParameter(Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.JobDescriptionInfo, Order = 65)]
        public virtual string PositionType
        {
            get
            {
                return RequestedPosition != null ? (RequestedPosition.Type != null ? RequestedPosition.Type.Name : string.Empty) : string.Empty;
            }
        }


        [UserInterfaceParameter(Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.JobDescriptionInfo, Order = 70)]
        public virtual string PositionCode
        {
            get
            {
                return RequestedPosition != null ? RequestedPosition.Code : string.Empty;
            }
        }

        [UserInterfaceParameter(Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.JobDescriptionInfo, Order = 75)]
        public virtual string PositionLevel
        {
            get
            {
                return RequestedPosition.JobDescription != null ?
                    (RequestedPosition.JobDescription.JobTitle != null ?
                        (RequestedPosition.JobDescription.JobTitle.Grade != null ? (RequestedPosition.JobDescription.JobTitle.Grade.OrganizationalLevel != null ? RequestedPosition.JobDescription.JobTitle.Grade.OrganizationalLevel.Name : string.Empty) : string.Empty)
                        : string.Empty)
                    : string.Empty;
            }
        }

        #endregion

        #region Other Info

        [UserInterfaceParameter(Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.RequestInformation, IsNonEditable = true, Order = 80)]
        public virtual string RequestCode { get; set; }

        [UserInterfaceParameter(Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.RequestInformation, IsNonEditable = true, Order = 85)]
        public virtual RequestStatus RequestStatus { get; set; }

        [UserInterfaceParameter(Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.RequestInformation, IsReference = true, IsNonEditable = true, Order = 90)]
        public virtual User Requester { get; set; }

        [UserInterfaceParameter(Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.RequestInformation, IsReference = true, IsNonEditable = true, Order = 95)]
        public virtual Position RequesterPosition { get; set; }

        #endregion

        #region Lists

        public virtual IList<RecruitmentRequestAttachment> RecruitmentRequestAttachments { get; set; }
        public virtual void AddRecruitmentRequestAttachment(RecruitmentRequestAttachment recruitmentRequestAttachment)
        {
            this.RecruitmentRequestAttachments.Add(recruitmentRequestAttachment);
            recruitmentRequestAttachment.RecruitmentRequest = this;
        }

        #endregion

        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown
        {
            get
            {
                return RequestCode;
            }
        }
    }
}
