using HRIS.Domain.Payroll.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Personnel.RootEntities
{
    public partial class Employee
    {

        #region Payroll
        // todo Mhd Alsaadi: أظن انه تم اضافته من سليمان وسيتم نقاشه
        #region EmployeeBenefit

        public virtual IList<EmployeeBenefit> EmployeeBenefits { get; protected set; }

        public virtual void AddEmployeeBenefit(EmployeeBenefit employeeBenefit)
        {
            employeeBenefit.Employee = this;
            EmployeeBenefits.Add(employeeBenefit);
        }
        #endregion

        #region EmployeeDeduction

        public virtual IList<EmployeeDeduction> EmployeeDeduction { get; protected set; }

        public virtual void AddEmployeeDeduction(EmployeeDeduction employeeDeduction)
        {
            employeeDeduction.Employee = this;
            EmployeeDeduction.Add(employeeDeduction);
        }
        #endregion

        #region Recantation

        public virtual IList<Recantation> Recantation { get; protected set; }

        public virtual void AddRecantation(Recantation recantation)
        {
            recantation.Employee = this;
            Recantation.Add(recantation);
        }
        #endregion

        #region Retrieval

        public virtual IList<Retrieval> Retrieval { get; protected set; }

        public virtual void AddRetrieval(Retrieval retrieval)
        {
            retrieval.Employee = this;
            Retrieval.Add(retrieval);
        }
        #endregion

        #region Loan

        public virtual IList<Loan> Loan { get; protected set; }

        public virtual void AddLoan(Loan loan)
        {
            loan.Employee = this;
            Loan.Add(loan);
        }
        #endregion

        #region Bonuse

        public virtual IList<Bonuse> Bonuse { get; protected set; }

        public virtual void AddBonuse(Bonuse bonuse)
        {
            bonuse.Employee = this;
            Bonuse.Add(bonuse);
        }
        #endregion

        #endregion

    }
}
