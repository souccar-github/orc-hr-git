using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HRIS.Domain.Grades.Entities;
using SpecExpress;

namespace HRIS.Validation.Specification.Grade.Entities
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class JobTitleSpecitication : Validates<JobTitle>
    {
        public JobTitleSpecitication()
        {
            IsDefaultForType();

            #region Primitive types


            Check(x => x.Name).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.EmployeeCount).Required().GreaterThanEqualTo(0);
            Check(x => x.Order).Required();

            #endregion


        }
    }
}
