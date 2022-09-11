#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//project manager:
//supervisor:
//author: Ammar Alziebak
//description:تطوير استمارة تقييم الأداء
//start date:
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion

#region

using System;
using System.Collections.Generic;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.PMS.Entities;
using HRIS.Domain.PMS.Entities.Competency;
using HRIS.Domain.PMS.Entities.JobDescription;
using HRIS.Domain.PMS.Entities.objective;
using HRIS.Domain.PMS.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.PMS.Helpers;

#endregion

namespace HRIS.Domain.PMS.RootEntities
{
    [Module(ModulesNames.PMS)]
    [Module(ModulesNames.Recruitment)]
    [Order(6)]
    public class AppraisalTemplate : Entity, IConfigurationRoot
    {
       public AppraisalTemplate()
       {
           TemplateSectionWeights = new List<TemplateSectionWeight>();
           CreationDate = DateTime.Now;
       }

        #region Basic Info
        
        [UserInterfaceParameter(Order = 1, Group = PMSGoupesNames.ResourceGroupName+"_"+PMSGoupesNames.AppraisalTemplateInformation)]
        public virtual string Name { get; set; }//اسم الاستمارة
        [UserInterfaceParameter(Order = 2, Group = PMSGoupesNames.ResourceGroupName + "_" + PMSGoupesNames.AppraisalTemplateInformation)]
        public virtual DateTime CreationDate { get; set; }//تاريخ الإنشاء
        [UserInterfaceParameter(Order = 3, Group = PMSGoupesNames.ResourceGroupName + "_" + PMSGoupesNames.AppraisalTemplateInformation)]
        public virtual TemplateType Type { get; set; }//نوع الاستمارة
        [UserInterfaceParameter(Order = 4, Group = PMSGoupesNames.ResourceGroupName + "_" + PMSGoupesNames.AppraisalTemplateInformation)]
        public virtual string Description { get; set; }//توصيف
        #endregion

        #region fixed Section
        [UserInterfaceParameter(Order = 5, Group = PMSGoupesNames.ResourceGroupName + "_" + PMSGoupesNames.AppraisalTemplateFixedSection)]
        public virtual bool Competency { get; set; }//قسم الكفاءات
        [UserInterfaceParameter(Order = 6, Group = PMSGoupesNames.ResourceGroupName + "_" + PMSGoupesNames.AppraisalTemplateFixedSection)]
        public virtual float CompetencyWeight { get; set; }//وزن قسم الكفاءات
        [UserInterfaceParameter(Order = 7, Group = PMSGoupesNames.ResourceGroupName + "_" + PMSGoupesNames.AppraisalTemplateFixedSection)]
        public virtual bool JobDescription { get; set; }//قسم الوصف الوظيفي
        [UserInterfaceParameter(Order = 8, Group = PMSGoupesNames.ResourceGroupName + "_" + PMSGoupesNames.AppraisalTemplateFixedSection)]
        public virtual float JobDescriptionWeight { get; set; }//وزن قسم الوصف الوظيفي
        [UserInterfaceParameter(Order = 9, Group = PMSGoupesNames.ResourceGroupName + "_" + PMSGoupesNames.AppraisalTemplateFixedSection)]
        public virtual bool Objective { get; set; }//قسم الأهداف
        [UserInterfaceParameter(Order = 10, Group = PMSGoupesNames.ResourceGroupName + "_" + PMSGoupesNames.AppraisalTemplateFixedSection)]
        public virtual float ObjectiveWeight { get; set; }//وزن قسم الأهداف
        public virtual float TotalCustomSectionWeight
        {
            get
            {
                return 100 - (CompetencyWeight + JobDescriptionWeight + ObjectiveWeight);
            }
        }
        #endregion

        #region TemplateSectionWeights

        public virtual IList<TemplateSectionWeight> TemplateSectionWeights { get; protected set; }
        public virtual void AddTemplateSectionWeight(TemplateSectionWeight templateSectionWeight)
        {
            templateSectionWeight.AppraisalTemplate = this;
            TemplateSectionWeights.Add(templateSectionWeight);
        }

        #endregion

        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown
        {
            get { return Name; }
        }
    }
}