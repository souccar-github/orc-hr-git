using System;
using AutoMapper;
using HRIS.Domain.AttendanceSystem.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.AttendanceSystem.Configurations;

namespace HRIS.Domain.AttendanceSystem.Entities
{
    [Details(IsDetailHidden = false)]
    [Order(2)]
    public class ParticularOvertimeShift : Entity // الفترات الخاصة للوردية
    {
        [UserInterfaceParameter(Order = 1, IsTime = true)]
        public virtual DateTime StartTime { get; set; } // توقيت البداية ويكون ساعات ودقائق

        [UserInterfaceParameter(Order = 1, IsTime = true)]
        public virtual DateTime EndTime { get; set; } // توقيت النهاية ويكون ساعات ودقائق

        [UserInterfaceParameter(Order = 1)]
        public virtual Workshop Workshop { get; set; } // الوردية الاب لهذه الفترة الخاصة


        public virtual ParticularOvertimeShift Prepare(DateTime date)
        {
            var result = new ParticularOvertimeShift();

            Mapper.Map(this, result);
            var tempDate = date.Date;
            result.StartTime = tempDate.Date.AddHours(result.StartTime.Hour).AddMinutes(result.StartTime.Minute);
            result.EndTime = tempDate.Date.AddHours(result.EndTime.Hour).AddMinutes(result.EndTime.Minute);

            if (result.EndTime < result.StartTime)
            {
                result.EndTime = result.EndTime.AddDays(1);
            }
            return result;
        }
    }
}
