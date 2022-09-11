using HRIS.Domain.Global.Constant;
using HRIS.Domain.Objectives.Enums;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Recruitment.Enums;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace HRIS.Domain.TaskManagement.RootEntities
{
    [Module(ModulesNames.TaskManagement)]
    public class Task : Entity, IAggregateRoot
    {
        public Task()
        {
            CreationDate = DateTime.Now;
            DailyWorks = new List<DailyWork>();
        }
        [UserInterfaceParameter(Order = 10, IsNonEditable = true)]
        public virtual DateTime CreationDate { get; set; }

        [UserInterfaceParameter(Order = 15, IsNonEditable = true)]
        public virtual int WeekNumber
        {
            get
            {
                DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                return dfi.Calendar.GetWeekOfYear(CreationDate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
            }
        }

        [UserInterfaceParameter(Order = 17)]
        public virtual string Title { get; set; }

        [UserInterfaceParameter(Order = 20)]
        public virtual string Description { get; set; }

        [UserInterfaceParameter(Order = 30)]
        public virtual Priority Priority { get; set; }

        [UserInterfaceParameter(Order = 40)]
        public virtual TaskStatus Status { get; set; }

        [UserInterfaceParameter(Order = 45)]
        public virtual double Progress
        {
            get
            {
                return DailyWorks.Sum(x => x.Progress);
            }
        }

        [UserInterfaceParameter(Order = 50)]
        public virtual DateTime PlanningStartDate { get; set; }

        [UserInterfaceParameter(Order = 60)]
        public virtual DateTime PlanningEndDate { get; set; }

        [UserInterfaceParameter(Order = 70)]
        public virtual DateTime ActuallyStartDate
        {
            get
            {
                return DailyWorks.Count == 0 ? PlanningStartDate : DailyWorks.Min(x => x.Date);

            }
        }

        [UserInterfaceParameter(Order = 80)]
        public virtual DateTime ActuallyEndDate
        {
            get
            {
                return DailyWorks.Count == 0 ? PlanningEndDate : DailyWorks.Max(x => x.Date);
            }
        }

        [UserInterfaceParameter(Order = 90)]
        public virtual string Comment { get; set; }


        [UserInterfaceParameter(Order = 95)]
        public virtual double Evaluation { get; set; }


        [UserInterfaceParameter(Order = 100, IsNonEditable = true)]
        public virtual Employee Employee { get; set; }

        public virtual IList<DailyWork> DailyWorks { get; set; }
        public virtual void AddDailyWork(DailyWork dailyWork)
        {
            dailyWork.Task = this;
            DailyWorks.Add(dailyWork);
        }

    }
}
