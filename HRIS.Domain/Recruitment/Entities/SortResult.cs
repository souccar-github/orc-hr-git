using HRIS.Domain.Recruitment.Configurations;
using HRIS.Domain.Recruitment.Enums;
using HRIS.Domain.Recruitment.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Recruitment.Entities
{
    public class SortResult : Entity
    {




        [UserInterfaceParameter(Order = 1)]
        public virtual int SortOrder { get; set; }
        [UserInterfaceParameter(Order = 2)]
        public virtual SortBy SortBy { get; set; }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown
        {
            get
            {
                return SortOrder.ToString();
            }
        }
        public virtual EvaluationSettings EvaluationSettings { get; set; }

    }
}
