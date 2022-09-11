//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//project manager:
//supervisor:
//author: Ammar Alziebak
//description:
//start date:
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
using HRIS.Domain.Training.Indexes;
using HRIS.Domain.Training.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.Attachment.Entities;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Report;
using Souccar.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Training.Entities
{
    [Module("Training")]
    public class CourseAppraisalTraineeAttachment :Entity 
    {
        public CourseAppraisalTraineeAttachment()
        {
            CreationDate = DateTime.Now;
        }
        [UserInterfaceParameter(Order = 1)]
        public virtual string Title { get; set; }

        [UserInterfaceParameter(Order = 2)]
        public virtual string Description { get; set; }

        [UserInterfaceParameter(Order = 3, IsNonEditable = true)]
        public virtual DateTime CreationDate { get; set; }

        [UserInterfaceParameter(Order = 4)]
        public virtual AppraisalTrainee AppraisalTrainee { get; set; }

        [UserInterfaceParameter(IsFile = true, AcceptExtension = ".rar,.zip,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.jpg,.png,.txt,.pdf",FileSize = 5000000)]
        public virtual string FileName { get; set; }
        [UserInterfaceParameter(Order = 200, IsHidden = true)]
        public virtual string NameForDropdown
        {
            get
            {
                return Title;
            }
        }

    }
}
