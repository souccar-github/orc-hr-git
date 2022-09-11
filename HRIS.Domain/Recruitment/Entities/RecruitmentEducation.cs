using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.Recruitment.RootEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Recruitment.Entities
{
    public class RecruitmentEducation: EducationBase
    {
        public virtual JobApplication JobApplication { get; set; }
    }
}
