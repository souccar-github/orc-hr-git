#region

using System.ComponentModel.DataAnnotations;
using HRIS.Domain.JobDescription.RootEntities;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Recruitment.Enums;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.JobDescription.Entities
{
    public class JEducation : Entity
    {
        
        /// <summary>
        ///‰Ê⁄ «·«Œ ’«’
        /// </summary>
        [UserInterfaceParameter(Order = 1)]
        public virtual MajorType Type { get; set; }
        
        /// <summary>
        /// «·«Œ ’«’
        /// </summary>
        [UserInterfaceParameter(Order = 2)]
        public virtual Major Major { get; set; }
       
        /// <summary>
        /// „” ÊÏ «·„ƒÂ· «·⁄·„Ì
        /// </summary>
        [UserInterfaceParameter(Order = 3)]
        public virtual GraduationLevel Rank { get; set; }

        [UserInterfaceParameter(Order = 5)]
        public virtual float Score { get; set; }

        [UserInterfaceParameter(Order = 6)]
        public virtual float Weight { get; set; }

        [UserInterfaceParameter(Order = 7)]
        public virtual bool Required { get; set; }

        public virtual HRIS.Domain.JobDescription.RootEntities.JobDescription JobDescription { get; set; }
    }
}