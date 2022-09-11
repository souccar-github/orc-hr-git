using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Training1.RootEntities;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Training1.Entities
{
    public class AppraisalTrainee :Entity
    {
        public virtual Employee Employee { get; set; }
        public virtual int GroupNumber { get; set; }
        public virtual int Attendance { get; set; }
        public virtual DateTime ExamDate { get; set; }
        public virtual int Score { get; set; }
        public virtual int Level { get; set; }
        public virtual string AbsenteeismCause { get; set; }
        public virtual string TrainerNote { get; set; }
        public virtual string AttachmentDocumentPath { get; set; }

        public virtual Course Course { get; set; }
    }
}
