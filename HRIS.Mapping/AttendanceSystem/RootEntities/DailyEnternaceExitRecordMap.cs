using FluentNHibernate.Mapping;
using HRIS.Domain.AttendanceSystem.RootEntities;
using Souccar.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Mapping.AttendanceSystem.RootEntities
{
    public class DailyEnternaceExitRecordMap : ClassMap<DailyEnternaceExitRecord>
    {
        public DailyEnternaceExitRecordMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            Map(x => x.Date);
            Map(x => x.LoginDateTime);
            Map(x => x.LoginTime);
            Map(x => x.LoginDate);
            Map(x => x.LogoutDate);

            Map(x => x.SecondLoginDateTime);
            Map(x => x.SecondLoginTime);
            Map(x => x.SecondLoginDate);
            Map(x => x.SecondLogoutDate);

            Map(x => x.ThirdLoginDateTime);
            Map(x => x.ThirdLoginTime);
            Map(x => x.ThirdLoginDate);
            Map(x => x.ThirdLogoutDate);

            Map(x => x.LogoutTime);
            Map(x => x.LogoutDateTime);

            Map(x => x.SecondLogoutTime);
            Map(x => x.SecondLogoutDateTime);

            Map(x => x.ThirdLogoutTime);
            Map(x => x.ThirdLogoutDateTime);
            Map(x => x.AbsenseType);
            Map(x => x.LateType);
            Map(x => x.Day);
            Map(x => x.HasVacation);
            Map(x => x.VacationValue);
            Map(x => x.HasMission);
            Map(x => x.MissionValue);
            Map(x => x.Status);
            Map(x => x.RequiredWorkHours);
            Map(x => x.WorkHoursValue);
            Map(x => x.LateHoursValue);
            Map(x => x.AbsentHoursValue);
            Map(x => x.OvertimeHoursValue);
            Map(x => x.HolidayOvertimeHoursValue);
            Map(x => x.UpdateReason).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.Note).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.InsertSource);
            Map(x => x.IsCalculated);
            Map(x => x.IsClosed);

            References(x => x.Employee);
            References(x => x.Node);
        }
        }
    }
