using System;
using AutoMapper;
using HRIS.Domain.AttendanceSystem.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.AttendanceSystem.Configurations;

namespace HRIS.Domain.AttendanceSystem.Entities
{//todo : Mhd Update changeset no.1

    [Details(IsDetailHidden = false)]
    [Order(1)]
    public class NormalShift : Entity,IAggregateRoot // الفترات العادية للوردية
    {


        [UserInterfaceParameter(Order = 5)]
        public virtual int NormalShiftOrder { get; set; } // ترتيب فترات الوردية اي الفترة الحالية مثلا هي الفترة الاولى او الثانية

        [UserInterfaceParameter(Order = 10, IsTime = true)]
        public virtual DateTime EntryTime { get; set; } // وقت الدخول

        [UserInterfaceParameter(Order = 15, IsTime = true)]
        public virtual DateTime ExitTime { get; set; } // وقت الخروج

        [UserInterfaceParameter(Order = 20, IsTime = true)]
        public virtual DateTime ShiftRangeStartTime { get; set; } // الحد الادنى للدخول - وأي دخول خارج هذا المجال سيتم اهماله وكأنه غير موجود مفيد لتقييد الاضافي 

        [UserInterfaceParameter(Order = 25, IsTime = true)]
        public virtual DateTime ShiftRangeEndTime { get; set; } // الحد الاقصى للخروج - وأي خروج خارج هذا المجال سيتم اهماله وكأنه غير موجود مفيد لتقييد الاضافي

        [UserInterfaceParameter(Order = 30, IsHidden = true)]
        public virtual bool ShiftRelatedToNextDay { get; set; } // الفترة الممتدة على يومين تكون تابعة لليوم التالي

        [UserInterfaceParameter(Order = 35)]
        public virtual int IgnoredPeriodBeforeEntryTime { get; set; } // مدة مسامحة الدخول بالدقائق - القيمة المهملة قبل وقت البدء للفترة مفيد لتقييد الاضافي

        [UserInterfaceParameter(Order = 40)]
        public virtual int IgnoredPeriodAfterEntryTime { get; set; } //  مدة مسامحة الدخول بالدقائق - القيمة المهملة بعد وقت بدء الفترة بالتالي لا تعتبر تأخر

        [UserInterfaceParameter(Order = 45)]
        public virtual int IgnoredPeriodBeforeExitTime { get; set; } // مدة المسامحة للخروج بالدقائق - القيمة المهملة قبل وقت الخروج للفترة مفيدة لعدم اعتباره خروج مبكر اي غياب غير مبرر

        [UserInterfaceParameter(Order = 50)]
        public virtual int IgnoredPeriodAfterExitTime { get; set; } // مدة المسامحة للخروج بالدقائق  - القيمة المهملة بعد وقت الخروج للفترة مفيدة لتقييد الاضافي بعد الدوام

        [UserInterfaceParameter(Order = 55)]
        public virtual int RestPeriod { get; set; } // مدة الاستراحة بالدقائق

        [UserInterfaceParameter(Order = 60, IsTime = true)]
        public virtual DateTime? RestRangeStartTime { get; set; } // وقت بدء  مجال الاستراحة بالدقائق والساعات

        [UserInterfaceParameter(Order = 65, IsTime = true)]
        public virtual DateTime? RestRangeEndTime { get; set; } // وقت انتهاء مجال الاستراحة بالدقائق والساعات

        public virtual Workshop Workshop { get; set; } //  الوردية التي سترتبط بها الفترات العادية للدوام


        public virtual NormalShift Prepare(DateTime date)
        {
            var result = new NormalShift();
           

            Mapper.Map(this, result);    
            var tempDate = date.Date;
            result.ShiftRangeStartTime = tempDate.Date.AddHours(result.ShiftRangeStartTime.Hour).AddMinutes(result.ShiftRangeStartTime.Minute);
            result.EntryTime = tempDate.Date.AddHours(result.EntryTime.Hour).AddMinutes(result.EntryTime.Minute);
            result.RestRangeStartTime = tempDate.Date.AddHours(result.RestRangeStartTime.Value.Hour).AddMinutes(result.RestRangeStartTime.Value.Minute);
            result.RestRangeEndTime = tempDate.Date.AddHours(result.RestRangeEndTime.Value.Hour).AddMinutes(result.RestRangeEndTime.Value.Minute);
            result.ExitTime = tempDate.Date.AddHours(result.ExitTime.Hour).AddMinutes(result.ExitTime.Minute);
            result.ShiftRangeEndTime = tempDate.Date.AddHours(result.ShiftRangeEndTime.Hour).AddMinutes(result.ShiftRangeEndTime.Minute);

            if (result.EntryTime.Hour < 12 && result.ShiftRangeStartTime.Hour > 12 && result.EntryTime < result.ShiftRangeStartTime)
              {
                result.EntryTime = result.EntryTime.AddDays(1);
            }
            if (result.RestRangeStartTime < result.EntryTime)
            {
                result.RestRangeStartTime = result.RestRangeStartTime.Value.AddDays(1);
            }
            if (result.RestRangeEndTime < result.RestRangeStartTime)
            {
                result.RestRangeEndTime = result.RestRangeEndTime.Value.AddDays(1);
            }
            if (result.ShiftRangeEndTime < result.ShiftRangeStartTime)
            {
                result.ShiftRangeEndTime = result.ShiftRangeEndTime.AddDays(1);
            }
            if (result.ExitTime < result.EntryTime)
            {
                result.ExitTime = result.ExitTime.AddDays(1);
            }
            return result;
        }
    }


}

