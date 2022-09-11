using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Workflow;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.PMS.Helpers;

namespace HRIS.Domain.PMS.Configurations
{
    [Module(ModulesNames.PMS)]
    [Module(ModulesNames.Recruitment)]
    [Order(3)]
    public class AppraisalPhaseSetting : Entity, IConfigurationRoot
    {

        [UserInterfaceParameter(Order = 2, Group = PMSGoupesNames.ResourceGroupName + "_" + PMSGoupesNames.Workflow,
            ReferenceReadUrl = "PMS/Reference/ReadWorkflowSetting", IsReference = true, IsHidden = false)]
        public virtual WorkflowSetting WorkflowSetting { get; set; }
        [UserInterfaceParameter(Order = 1, Group = PMSGoupesNames.ResourceGroupName + "_" + PMSGoupesNames.Workflow)]
        public virtual string Title { get; set; }

        #region Scale Details
        #region range
        [UserInterfaceParameter(Order = 2, Group = PMSGoupesNames.ResourceGroupName + "_" + PMSGoupesNames.MarkRange)]
        public virtual float FromMark { get; set; }//من مجال حدود علامات التقييم
        public virtual float FromMarkPercentage
        {
            get
            {
                if (FullMark > 0)
                {
                    return (FromMark * 100) / FullMark;
                }
                else
                {
                    return 0;
                }
            }
        }
        [UserInterfaceParameter(Order = 3, Group = PMSGoupesNames.ResourceGroupName + "_" + PMSGoupesNames.MarkRange)]
        public virtual float ToMark { get; set; }//إلى مجال حدود علامات التقييم
        public virtual float ToMarkPercentage
        {
            get
            {
                if (FullMark > 0)
                {
                    return (ToMark * 100) / FullMark;
                }
                else
                {
                    return 0;
                }
            }
        }
        #endregion
        #region Mark
        [UserInterfaceParameter(Order = 4, Group = PMSGoupesNames.ResourceGroupName + "_" + PMSGoupesNames.Mark)]
        public virtual float FullMark { get; set; }//العلامة الكاملة
        public virtual float FullMarkPercentage
        {
            get
            {
                if (FullMark > 0)
                {
                    return (FullMark * 100) / FullMark;
                }
                else
                {
                    return 0;
                }
            }
        }
        [UserInterfaceParameter(Order = 5, Group = PMSGoupesNames.ResourceGroupName + "_" + PMSGoupesNames.Mark)]
        public virtual float MarkStep { get; set; }//خطوة العلامة
        public virtual float MarkStepPercentage
        {
            get
            {
                if (FullMark > 0)
                {
                    return (MarkStep * 100) / FullMark;
                }
                else
                {
                    return 0;
                }
            }
        }
        #endregion
        #region MarkBelowExpected
        [UserInterfaceParameter(Order = 6, Group = PMSGoupesNames.ResourceGroupName + "_" + PMSGoupesNames.MarkRange)]
        public virtual float FromMarkBelowExpected { get; set; }//من المستوى تحت التوقعات
        public virtual float FromMarkBelowExpectedPercentage
        {
            get
            {
                if (FullMark > 0)
                {
                    return (FromMarkBelowExpected * 100) / FullMark;
                }
                else
                {
                    return 0;
                }
            }
        }
        [UserInterfaceParameter(Order = 7, Group = PMSGoupesNames.ResourceGroupName + "_" + PMSGoupesNames.MarkRange)]
        public virtual float ToMarkBelowExpected { get; set; }//إلى المستوى تحت التوقعات
        public virtual float ToMarkBelowExpectedPercentage
        {
            get
            {
                if (FullMark > 0)
                {
                    return (ToMarkBelowExpected * 100) / FullMark;
                }
                else
                {
                    return 0;
                }
            }
        }
        #endregion
        #region MarkNeedTraining
        [UserInterfaceParameter(Order = 9, Group = PMSGoupesNames.ResourceGroupName + "_" + PMSGoupesNames.MarkRange)]
        public virtual float FromMarkNeedTraining { get; set; }//من المستوى يحتاج لتدريب
        public virtual float FromMarkNeedTrainingPercentage
        {
            get
            {
                if (FullMark > 0)
                {
                    return (FromMarkNeedTraining * 100) / FullMark;
                }
                else
                {
                    return 0;
                }
            }
        }
        [UserInterfaceParameter(Order = 10, Group = PMSGoupesNames.ResourceGroupName + "_" + PMSGoupesNames.MarkRange)]
        public virtual float ToMarkNeedTraining { get; set; }//إلى المستوى يحتاج لتدريب
        public virtual float ToMarkNeedTrainingPercentage
        {
            get
            {
                if (FullMark > 0)
                {
                    return (ToMarkNeedTraining * 100) / FullMark;
                }
                else
                {
                    return 0;
                }
            }
        }
        #endregion
        #region MarkExpected
        [UserInterfaceParameter(Order = 12, Group = PMSGoupesNames.ResourceGroupName + "_" + PMSGoupesNames.MarkRange)]
        public virtual float FromMarkExpected { get; set; }//من المستوى ضمن التوقعات
        public virtual float FromMarkExpectedPercentage
        {
            get
            {
                if (FullMark > 0)
                {
                    return (FromMarkExpected * 100) / FullMark;
                }
                else
                {
                    return 0;
                }
            }
        }
        [UserInterfaceParameter(Order = 13, Group = PMSGoupesNames.ResourceGroupName + "_" + PMSGoupesNames.MarkRange)]
        public virtual float ToMarkExpected { get; set; }//إلى المستوى ضمن التوقعات
        public virtual float ToMarkExpectedPercentage
        {
            get
            {
                if (FullMark > 0)
                {
                    return (ToMarkExpected * 100) / FullMark;
                }
                else
                {
                    return 0;
                }
            }
        }
        #endregion
        #region MarkUpExpected
        [UserInterfaceParameter(Order = 15, Group = PMSGoupesNames.ResourceGroupName + "_" + PMSGoupesNames.MarkRange)]
        public virtual float FromMarkUpExpected { get; set; }//من المستوى فوق التوقعات
        public virtual float FromMarkUpExpectedPercentage
        {
            get
            {
                if (FullMark > 0)
                {
                    return (FromMarkUpExpected * 100) / FullMark;
                }
                else
                {
                    return 0;
                }
            }
        }
        [UserInterfaceParameter(Order = 16, Group = PMSGoupesNames.ResourceGroupName + "_" + PMSGoupesNames.MarkRange)]
        public virtual float ToMarkUpExpected { get; set; }//إلى المستوى فوق التوقعات
        public virtual float ToMarkUpExpectedPercentage
        {
            get
            {
                if (FullMark > 0)
                {
                    return (ToMarkUpExpected * 100) / FullMark;
                }
                else
                {
                    return 0;
                }
            }
        }
        #endregion
        #region Distinct
        [UserInterfaceParameter(Order = 18, Group = PMSGoupesNames.ResourceGroupName + "_" + PMSGoupesNames.MarkRange)]
        public virtual float FromMarkDistinct { get; set; }//من المستوى متميز 
        public virtual float FromMarkDistinctPercentage
        {
            get
            {
                if (FullMark > 0)
                {
                    return (FromMarkDistinct * 100) / FullMark;
                }
                else
                {
                    return 0;
                }
            }
        }
        [UserInterfaceParameter(Order = 19, Group = PMSGoupesNames.ResourceGroupName + "_" + PMSGoupesNames.MarkRange)]
        public virtual float ToMarkDistinct { get; set; }//إلى المستوى متميز
        public virtual float ToMarkDistinctPercentage
        {
            get
            {
                if (FullMark > 0)
                {
                    return (ToMarkDistinct * 100) / FullMark;
                }
                else
                {
                    return 0;
                }
            }
        }
        #endregion
        #region Gap
        [UserInterfaceParameter(Order = 21, Group = PMSGoupesNames.ResourceGroupName + "_" + PMSGoupesNames.GapSkillThreshold)]
        public virtual float GapThreshold { get; set; }//عتبة نقاط الضعف
        public virtual float GapThresholdPercentage
        {
            get
            {
                if (FullMark > 0)
                {
                    return (GapThreshold * 100) / FullMark;
                }
                else
                {
                    return 0;
                }
            }
        }
        #endregion
        #region Skill
        [UserInterfaceParameter(Order = 22, Group = PMSGoupesNames.ResourceGroupName + "_" + PMSGoupesNames.GapSkillThreshold)]
        public virtual float SkillThreshold { get; set; }//من عتبة نقاط القوة
        public virtual float SkillThresholdPercentage
        {
            get
            {
                if (FullMark > 0)
                {
                    return (SkillThreshold * 100) / FullMark;
                }
                else
                {
                    return 0;
                }
            }
        }
        #endregion
        #endregion

        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown
        {
            get { return Title; }
        }
    }
}
