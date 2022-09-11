//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using FluentNHibernate.Mapping;
//using HRIS.Domain.AttendanceSystem.RootEntities;

//namespace HRIS.Mapping.AttendanceSystem.RootEntities
//{
//    public class EmployeeAttendanceCardMap : ClassMap<EmployeeAttendanceCard>
//    {
//        public EmployeeAttendanceCardMap()
//        {
//            #region Default
//            DynamicUpdate();
//            DynamicInsert();
//            Id(x => x.Id);
//            Map(x => x.IsVertualDeleted);
//            #endregion
//            Map(x => x.EmployeeMachineCode);
//            Map(x => x.AttendanceDemand);
//            Map(x => x.AbsenceCounterRecurrence);
//            Map(x => x.LatenessCounterRecurrence);

//            References(x => x.Employee);
//            References(x => x.AttendanceForm);
//            References(x => x.LatenessForm);
//            References(x => x.AbsenceForm);
//            References(x => x.OvertimeForm);

//            HasMany(x => x.TemporaryWorkshops).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

//        }
//    }
//}
