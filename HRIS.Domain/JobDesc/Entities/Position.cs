using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Conventions;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.Grades.Indexes;
using HRIS.Domain.JobDescription.Enum;
using HRIS.Domain.JobDescription.Indexes;
using HRIS.Domain.JobDescription.RootEntities;

using HRIS.Domain.OrganizationChart.Indexes;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Grades.Entities;

namespace HRIS.Domain.JobDescription.Entities
{

    [Module(ModulesNames.JobDescription)]
    //[Module(ModulesNames.PayrollSystem)]
    [Order(45)]
    //[Command(CommandsNames.ManageDelegatePosition, Order = 1)]
    public class Position : Entity, IAggregateRoot
    {
        public Position()
        {
            Status = new List<PositionStatus>();
            ManagerTo = new List<PositionReporting>();
            ReportingsTo = new List<PositionReporting>();

            Delegates = new List<PositionDelegate>();
            DelegatesTo = new List<PositionDelegate>();

            PositionBenefitDetails = new List<PositionBenefitDetail>();
            PositionDeductionDetails = new List<PositionDeductionDetail>();

        }

        public override string ToString()
        {
            return string.Format("Position: {0}", NameForDropdown);
        }

        [UserInterfaceParameter(IsImageColumn = true, ImageColumnPath = "Content/EmployeesPhoto/", DefaultImageName = "placeholder.jpg", Order = 1, Width = 60, IsHidden = false)]
        public virtual string PhotoPath
        {
            get {
                if (Employee == null)
                    return "placeholder.jpg";
                return Employee.PhotoId;
            }
        }

        [UserInterfaceParameter(Order = 1, IsReference = true)]
        public virtual HRIS.Domain.JobDescription.RootEntities.JobDescription JobDescription { get; set; }
        [UserInterfaceParameter(Order = 2, IsHidden = false)]
        public virtual string NameForDropdown
        {
            get
            {
                if (JobDescription == null && Employee == null)
                    return Code;

                if (JobDescription == null && Employee != null)
                    return Employee.FullName;

                if (JobDescription != null && Employee == null)
                    return string.Format("{0}={1}", JobDescription.Name, Code);

                if (JobDescription != null && Employee != null)
                    return string.Format("{0}={1}", JobDescription.Name, Employee.FullName);

                return Code;
            }
        }

        [UserInterfaceParameter(Order = 4)]
        public virtual PositionType Type { get; set; }

        [UserInterfaceParameter(Order = 5,IsNonEditable = true)]
        public virtual PositionStatusType PositionStatusType
        {
            get
            {
                if (Status.IsEmpty())
                    AddPositionStatus(PositionStatusType.New);
                var status = Status.Where(x => x.IsActive).OrderByDescending(x=> x.FromDate).FirstOrDefault();
                if (status != null)
                    return status.PositionStatusType;
                return Status.FirstOrDefault(x => x.ExpireDate == Status.Max(y => y.ExpireDate)).PositionStatusType;
            }
            set
            {
                AddPositionStatus(value);
            }
        }
        //[UserInterfaceParameter(Order = 3)]
        //public virtual bool IsAssign
        //{
        //    get
        //    {
        //        return AssigningEmployeeToPosition != null;
        //    }
        //}
        [UserInterfaceParameter(Order = 6)]
        public virtual CostCenter CostCenter { get; set; }

        [UserInterfaceParameter(Order = 7,IsNonEditable = true)]
        public virtual string Code { get; set; }

        [UserInterfaceParameter(Order = 8)]
        public virtual float WorkingHours { get; set; }

        [UserInterfaceParameter(Order = 9)]
        public virtual bool DisabilityStatus { get; set; }

        [UserInterfaceParameter(Order = 10)]
        public virtual TimeInterval Per { get; set; }

        [UserInterfaceParameter(Order = 11)]
        public virtual float Budget { get; set; }

        [UserInterfaceParameter(Order = 12)]
        public virtual CurrencyType CurrencyType { get; set; }

        [UserInterfaceParameter(Order = 13)]
        public virtual bool ManagerIsMandatory { get; set; }

        [UserInterfaceParameter(Order = 14, IsReference = true)]
        public virtual JobTitle ManagerJobTitle { get; set; }

        [UserInterfaceParameter(Order = 15, IsReference = true, ReferenceReadUrl = "JobDescription/Reference/ReadPositionCascadeJobTitle", CascadeFrom = "ManagerJobTitle")]
        public virtual Position Manager { get; set; }
        

    

        [UserInterfaceParameter(Order = 16, IsReference = true, ReferenceReadUrl = "JobDescription/Position/GetGradeStep/")]
        public virtual GradeStep Step { get; set; }

        //public virtual Position Manager
        //{
        //    get
        //    {
        //        var primaryManager = ReportingsTo.SingleOrDefault(x => x.IsPrimary);
        //        return primaryManager!= null ? ReportingsTo.SingleOrDefault(x => x.IsPrimary).ManagerPosition : null;
        //    }
        //}

        public virtual Employee Employee
        {
            get
            {
                return AssigningEmployeeToPosition != null ? AssigningEmployeeToPosition.Employee : null;
            }
        }

        public virtual AssigningEmployeeToPosition AssigningEmployeeToPosition { get; set; }



        #region Status
        public virtual IList<PositionStatus> Status { get; set; }
        public virtual void AddPositionStatus(PositionStatusType positionStatusType)
        {
            var positionStatus = new PositionStatus(this, positionStatusType);
            if (Status == null || Status.Count == 0)
            {
                Status.Add(positionStatus);
                return;
            }
            var temp = Status.OrderByDescending(x=>x.Id).FirstOrDefault(x => x.IsActive);
            if (temp == null)
            {
                Status.Add(positionStatus);
                return;
            }
            if (temp.PositionStatusType != positionStatusType)
            {
                temp.ExpireDate = DateTime.Now;
                Status.Add(positionStatus);
            }
        }
        #endregion
        #region Delegate Authorities
        public virtual IList<DelegateAuthoritiesToPosition> DelegateAuthoritiesFromPositions { get; set; }
        public virtual IList<DelegateAuthoritiesToPosition> DelegateAuthoritiesToPositions { get; set; }
        public virtual void AddPositionDelegateAuthority(DelegateAuthoritiesToPosition delegateAuthoritiesToPosition)
        {
            DelegateAuthoritiesFromPositions.Add(delegateAuthoritiesToPosition);
            delegateAuthoritiesToPosition.SourcePosition = this;

            delegateAuthoritiesToPosition.DestinationPosition.DelegateAuthoritiesToPositions.Add(delegateAuthoritiesToPosition);
        }
        #endregion

        #region Delegate Roles
        public virtual IList<DelegateRolesToPosition> DelegateRolesToPositions { get; set; }
        public virtual IList<DelegateRolesToPosition> DelegateRolesFromPositions { get; set; }
        public virtual IList<DelegateRolesToPosition> DelegateRolesAsSuperiorPositions { get; set; }
        public virtual void AddPositionDelegateRole(DelegateRolesToPosition delegateRolesToPosition)
        {
            DelegateRolesFromPositions.Add(delegateRolesToPosition);
            delegateRolesToPosition.SourcePosition = this;

            delegateRolesToPosition.DestinationPosition.DelegateRolesToPositions.Add(delegateRolesToPosition);
            delegateRolesToPosition.Superior.DelegateRolesAsSuperiorPositions.Add(delegateRolesToPosition);
        }
        #endregion



        #region Delegate
        [UserInterfaceParameter(IsHidden = true)]

        public virtual IList<PositionDelegate> Delegates { get; set; }
        public virtual void AddDelegate(Position SecondaryPosition, AuthorityType authorityType)
        {
            var @delegate = new PositionDelegate()
            {
                PrimaryPosition = this,
                SecondaryPosition = SecondaryPosition,
                AuthorityType = authorityType
            };
            Delegates.Add(@delegate);
            SecondaryPosition.DelegatesTo.Add(@delegate);
        }
      
        [UserInterfaceParameter(IsHidden = true)]
        public virtual IList<PositionDelegate> DelegatesTo { get; set; }

        public virtual void AddDelegateTo(Position SecondaryPosition, AuthorityType authorityType)
        {
            var @delegate = new PositionDelegate()
            {
                SecondaryPosition = SecondaryPosition,
                PrimaryPosition = this,
                AuthorityType = authorityType
            };
            Delegates.Add(@delegate);
            //SecondaryPosition.DelegatesTo.Add(@delegate);
        }

        #endregion

        #region Reporting

        public virtual IList<PositionReporting> ManagerTo { get; set; }
        public virtual void AddManagerTo(Position position, bool isPrimary = false)
        {
            var positionReporting = new PositionReporting()
            {
                ManagerPosition = this,
                Position = position,
                IsPrimary = isPrimary
            };
            ManagerTo.Add(positionReporting);
            position.ReportingsTo.Add(positionReporting);
        }

        public virtual IList<PositionReporting> ReportingsTo { get; set; }

        public virtual void AddReportingTo(Position managerPosition, bool isPrimary = false)
        {
            var positionReporting = new PositionReporting()
            {
                ManagerPosition = managerPosition,
                Position = this,
                IsPrimary = isPrimary
            };
            ReportingsTo.Add(positionReporting);
            managerPosition.ManagerTo.Add(positionReporting);
        }

        #endregion

        #region PositionBenefitDetails

        public virtual IList<PositionBenefitDetail> PositionBenefitDetails { get; set; } // التعويضات التي سيتم منحها 
        public virtual void AddPositionBenefitDetail(PositionBenefitDetail positionBenefitDetail)
        {
            PositionBenefitDetails.Add(positionBenefitDetail);
            positionBenefitDetail.Position = this;
        }

        #endregion

        #region PositionDeductionDetails

        public virtual IList<PositionDeductionDetail> PositionDeductionDetails { get; set; }
        public virtual void AddPositionDeductionDetail(PositionDeductionDetail positionDeductionDetail)
        {
            PositionDeductionDetails.Add(positionDeductionDetail);
            positionDeductionDetail.Position = this;
        }

        #endregion
    }
}
