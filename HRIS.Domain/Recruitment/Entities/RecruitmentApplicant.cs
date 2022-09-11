
using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.Recruitment.Configurations;
using HRIS.Domain.Recruitment.Enums;
using HRIS.Domain.Recruitment.Helpers;
using HRIS.Domain.Recruitment.Indexes;
using HRIS.Domain.Recruitment.RootEntities;
using LinqToExcel.Attributes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;

namespace HRIS.Domain.Recruitment.Entities
{

    public class RecruitmentApplicant : Entity, IAggregateRoot
    {

        #region Basic Info

        [ExcelColumn("id")]
        [UserInterfaceParameter(IsNonEditable = true, Order = 1)]
        public virtual int ApplicantNumber { get; set; }

        [UserInterfaceParameter(IsReference = true, Order = 2)]
        public virtual Applicant Applicant { get; set; }

        [UserInterfaceParameter(IsNonEditable = true, Order = 3)]
        public virtual bool IsAccepted { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual RejectionReason RejectionReason { get; set; }

        [ExcelColumn("score")]
        [UserInterfaceParameter(IsNonEditable = true, Order = 4)]
        public virtual int WrittenDeservedMark { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual bool IsAttendedWritten { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual bool IsPassedWritten
        {
            get
            {
                var minWrittenMark = GetMinWrittenMark();

                if (minWrittenMark != -1)
                    if (WrittenDeservedMark >= minWrittenMark)
                        return true;

                return false;
            }

        }

        [UserInterfaceParameter(IsNonEditable = true, Order = 5)]
        public virtual int OralDeservedMark { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual bool IsAttendedOral { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual bool IsPassedOral
        {
            get
            {
                var minOralMark = GetMinOralMark();

                if (minOralMark != -1)
                    if (OralDeservedMark >= minOralMark)
                        return true;

                return false;
            }
        }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual double OldnessLaborOfficeMark
        {
            get
            {
                return GetOldnessLaborOfficeMark();
            }
        }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual int MartyrSonMark
        {
            get
            {
                return GetMartyrSonMark();
            }
        }

        [UserInterfaceParameter(IsNonEditable = true, Order = 6)]
        public virtual double FinalMark
        {
            get
            {
                return GetFinalMark();
            }
        }

        [UserInterfaceParameter(IsNonEditable = true, Order = 7)]
        public virtual bool IsPassed
        {
            get
            {
                return GetFinalResult();
            }
        }

        public virtual RecruitmentInformation RecruitmentInformation { get; set; }

        #region Methods

        private EvaluationSettings GetEvaluationSettingByRecruitmentType()
        {
            var evaluationSettings = ServiceFactory.ORMService.All<EvaluationSettings>();
            var evaluationSetting = evaluationSettings.SingleOrDefault(es => es.RecruitmentType == this.RecruitmentInformation.Advertisement.RecruitmentType);

            return evaluationSetting;
        }

        private int GetMinWrittenMark()
        {
            try
            {
                var evaluationSetting = GetEvaluationSettingByRecruitmentType();

                if (evaluationSetting != null && evaluationSetting.MinWrittenMark != -1)
                    return evaluationSetting.MinWrittenMark;
            }
            catch
            {
                return -1;
            }

            return -1;
        }

        private int GetMinOralMark()
        {
            try
            {
                var evaluationSetting = GetEvaluationSettingByRecruitmentType();

                if (evaluationSetting != null && evaluationSetting.MinOralMark != -1)
                    return evaluationSetting.MinOralMark;
            }
            catch
            {
                return -1;
            }

            return -1;
        }

        private double GetOldnessLaborOfficeMark()
        {
            try
            {
                var evaluationSetting = GetEvaluationSettingByRecruitmentType();

                var jobRelatedInfo = this.Applicant.RJobRelatedInfos.FirstOrDefault();

                if (jobRelatedInfo != null)
                {
                    double laborOfficeRegistrationYearsCount = 0;
                    double laborOfficeWorkingYearsCount = 0;
                    double laborOfficeRegistrationMonthsCount = 0;
                    double laborOfficeWorkingMonthsCount = 0;

                    if (evaluationSetting.RoundType == RoundType.PerYear)
                    {
                        var dateDiff = new DateDiff(jobRelatedInfo.LaborOfficeRegistrationDate, this.RecruitmentInformation.Advertisement.Date);
                        laborOfficeRegistrationYearsCount = dateDiff.YearsDiff;
                        if (dateDiff.MonthsDiff >= 6)
                            laborOfficeRegistrationYearsCount++;

                        dateDiff = new DateDiff(evaluationSetting.LaborOfficeStartingDate, this.RecruitmentInformation.Advertisement.Date);
                        laborOfficeWorkingYearsCount = dateDiff.YearsDiff;
                        if (dateDiff.MonthsDiff >= 6)
                            laborOfficeWorkingYearsCount++;

                    }
                    else
                    {
                        var dateDiff = new DateDiff(jobRelatedInfo.LaborOfficeRegistrationDate, this.RecruitmentInformation.Advertisement.Date);
                        laborOfficeRegistrationMonthsCount = (dateDiff.YearsDiff * 12) + dateDiff.MonthsDiff;
                        if (dateDiff.DaysDiff >= 15)
                            laborOfficeRegistrationMonthsCount++;

                        dateDiff = new DateDiff(evaluationSetting.LaborOfficeStartingDate, this.RecruitmentInformation.Advertisement.Date);
                        laborOfficeWorkingMonthsCount = (dateDiff.YearsDiff * 12) + dateDiff.MonthsDiff;
                        if (dateDiff.DaysDiff >= 15)
                            laborOfficeWorkingMonthsCount++;
                    }


                    double oldnessLaborOfficeMark = -1;
                    if (jobRelatedInfo.IsFamiliesMartyrs)
                    {
                        if (evaluationSetting.RoundType == RoundType.PerYear)
                            oldnessLaborOfficeMark =
                                Math.Round(
                                    ((laborOfficeRegistrationYearsCount * 12) / (laborOfficeWorkingYearsCount * 12)) *
                                    (evaluationSetting.OldnessWeightFactor));
                        else
                            oldnessLaborOfficeMark =
                                Math.Round(
                                    (laborOfficeRegistrationMonthsCount / laborOfficeWorkingMonthsCount) *
                                    (evaluationSetting.OldnessWeightFactor));
                    }
                    else
                    {
                        if (evaluationSetting.RoundType == RoundType.PerYear)
                            oldnessLaborOfficeMark =
                                Math.Round(
                                    ((laborOfficeRegistrationYearsCount * 12) / (laborOfficeWorkingYearsCount * 12)) *
                                    (evaluationSetting.OldnessWeightFactor + evaluationSetting.MartyrSonFactor));
                        else
                            oldnessLaborOfficeMark =
                                Math.Round(
                                    (laborOfficeRegistrationMonthsCount / laborOfficeWorkingMonthsCount) *
                                    (evaluationSetting.OldnessWeightFactor + evaluationSetting.MartyrSonFactor));
                    }


                    return oldnessLaborOfficeMark;
                }
            }
            catch
            {
                return -1;
            }

            return -1;
        }

        private int GetMartyrSonMark()
        {
            try
            {
                var jobRelatedInfo = this.Applicant.RJobRelatedInfos.FirstOrDefault();

                if (jobRelatedInfo != null)
                {
                    if (jobRelatedInfo.IsFamiliesMartyrs)
                    {
                        var evaluationSetting = GetEvaluationSettingByRecruitmentType();

                        if (evaluationSetting != null && evaluationSetting.MartyrSonFactor != -1)
                            return evaluationSetting.MartyrSonFactor;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch
            {
                return -1;
            }

            return -1;
        }

        private double GetFinalMark()
        {
            try
            {
                double finalMark = -1;
                var jobRelatedInfo = this.Applicant.RJobRelatedInfos.FirstOrDefault();

                if (jobRelatedInfo != null)
                {
                    if (jobRelatedInfo.IsFamiliesMartyrs)
                    {
                        if (this.RecruitmentInformation.Advertisement.RecruitmentType == RecruitmentType.Quiz)
                            finalMark = WrittenDeservedMark + OralDeservedMark + OldnessLaborOfficeMark + MartyrSonMark;
                        else
                            finalMark = WrittenDeservedMark + OldnessLaborOfficeMark + MartyrSonMark;
                    }

                    else
                    {
                        if (this.RecruitmentInformation.Advertisement.RecruitmentType == RecruitmentType.Quiz)
                            finalMark = WrittenDeservedMark + OralDeservedMark + OldnessLaborOfficeMark;
                        else
                            finalMark = WrittenDeservedMark + OldnessLaborOfficeMark;
                    }

                }


                return finalMark;
            }
            catch
            {

            }

            return -1;
        }

        private bool GetFinalResult()
        {
            try
            {
                if (IsPassedWritten && IsPassedOral)
                {
                    var evaluationSetting = GetEvaluationSettingByRecruitmentType();
                    var finalMark = GetFinalMark();

                    if (finalMark >= evaluationSetting.MinSuccessMark)
                        return true;
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        #endregion

        #endregion

    }
}
