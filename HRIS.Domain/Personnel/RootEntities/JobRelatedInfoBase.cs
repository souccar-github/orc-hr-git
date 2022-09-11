using HRIS.Domain.EmployeeRelationServices.Indexes;
using HRIS.Domain.Personnel.Helpers;
using HRIS.Domain.Personnel.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Personnel.RootEntities
{
    public class JobRelatedInfoBase: Entity, IAggregateRoot
    {
        public JobRelatedInfoBase()
        {
            WorkSideAgreementDate = DateTime.Today;
            DocumentDate = DateTime.Today;
        }

        [UserInterfaceParameter(IsNonEditable = true, Order = 1)]
        public virtual int ApplicationNumber { get; set; }

        [UserInterfaceParameter(Order = 2, Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.General)]
        public virtual DateTime ApplicationDate { get; set; }

        [UserInterfaceParameter(Order = 3, Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.General)]
        public virtual City City { get; set; }

        [UserInterfaceParameter(Order = 4, Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.General)]
        public virtual DateTime LaborOfficeRegistrationDate { get; set; }

        [UserInterfaceParameter(Order = 5, Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.General)]
        public virtual string WorkIdentificationNumber { get; set; }

        [UserInterfaceParameter(Order = 6, Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.General)]
        public virtual DateTime WorkIdentificationDate { get; set; }

        [UserInterfaceParameter(Order = 7, Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.JobInfo)]
        public virtual bool IsWorkPreviously { get; set; }

        [UserInterfaceParameter(Order = 8, Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.JobInfo)]
        public virtual WorkSide WorkSide { get; set; }

        [UserInterfaceParameter(Order = 9, Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.JobInfo)]
        public virtual string WorkSideAgreementNumber { get; set; }

        [UserInterfaceParameter(Order = 10, Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.JobInfo)]
        public virtual DateTime WorkSideAgreementDate { get; set; }

        [UserInterfaceParameter(Order = 11, Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.JobInfo, 
            IsFile = true, AcceptExtension = ".rar,.zip,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.jpg,.png,.txt,.pdf", FileSize = 5000000)]
        public virtual string WorkSideAgreementFileName { get; set; }

        [UserInterfaceParameter(Order = 12, Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.ChildrenOfMartyrs)]
        public virtual bool IsFamiliesMartyrs { get; set; }

        [UserInterfaceParameter(Order = 13, Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.ChildrenOfMartyrs)]
        public virtual KinshipType KinshipType { get; set; }

        [UserInterfaceParameter(Order = 14, Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.ChildrenOfMartyrs)]
        public virtual string DocumentNumber { get; set; }

        [UserInterfaceParameter(Order = 15, Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.ChildrenOfMartyrs)]
        public virtual DateTime DocumentDate { get; set; }

        [UserInterfaceParameter(Order = 16, Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.ChildrenOfMartyrs)]
        public virtual WorkSide IssuedBy { get; set; }
    }
}
