using HRIS.Domain.Objectives.Enums;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.TaskManagement.RootEntities;
using HRIS.Validation;
using Souccar.Domain.DomainModel;
using SpecExpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Map.TaskManagement.RootEntities
{
    
    public class TaskSpecification : Validates<Task>
    {
        public TaskSpecification()
        {

            IsDefaultForType();

            #region Primitive Types

            Check(x => x.Description).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.Title).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.Comment).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.PlanningStartDate).Required().LessThanEqualTo(x => x.PlanningEndDate);
            Check(x => x.Comment).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            Check(x => x.Progress).Optional().Between(0, 100);

            #endregion

            #region Indexes

            #endregion
        }
    }
}
