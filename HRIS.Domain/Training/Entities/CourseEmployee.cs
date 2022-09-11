using System.Linq;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Training.Enums;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.Training.Entities
{
    public class CourseEmployee : Entity
    {
        public virtual Employee Employee { get; set; }
        public virtual Course Course { get; set; }

        public virtual CourseEmployeeType Type { get; set; }

        public virtual Position Position
        {
            get
            {
                return Employee.Positions.FirstOrDefault(x => x.IsPrimary) == null ? null:
                    Employee.Positions.FirstOrDefault(x => x.IsPrimary).Position != null?Employee.Positions.FirstOrDefault(x => x.IsPrimary).Position: null;
                    
            }
        }

        public virtual Node Node
        {
            get { return Position != null ? Position.JobDescription.Node : null; }
        }
    }
}
