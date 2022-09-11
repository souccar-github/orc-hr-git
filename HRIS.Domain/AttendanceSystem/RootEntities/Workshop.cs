using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.AttendanceSystem.Entities;
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.AttendanceSystem.RootEntities
{
    [Order(1)]
    [Module(ModulesNames.AttendanceSystem)]
    public class Workshop : Entity, IAggregateRoot // وردية دوام
    {
        public Workshop()
        { 
            ParticularOvertimeShifts = new List<ParticularOvertimeShift>();
            TemporaryWorkshops = new List<TemporaryWorkshop>();
            NormalShifts = new List<NormalShift>();
        }
        [UserInterfaceParameter(Order = 1)]
        public virtual int Number { get; set; } // رقم الوردية

        [UserInterfaceParameter(Order = 1)]
        public virtual string Description { get; set; } // وصف الوردية

        [UserInterfaceParameter(Order = 1)]
        public virtual double TotalWorkHours
        {
            get
            {
                return NormalShifts == null ? 0 : NormalShifts.Sum(x => x.ExitTime.GetValueOrDefault() >= x.EntryTime.GetValueOrDefault() ? (x.ExitTime.GetValueOrDefault() - x.EntryTime.GetValueOrDefault()).TotalHours : (x.ExitTime.GetValueOrDefault() - x.EntryTime.GetValueOrDefault()).TotalHours + 24);
            }
        } // اجمالي ساعات العمل لكل الفترات وهو حقل للقراءة فقط

        [UserInterfaceParameter(Order = 1)]
        public virtual int TotalRestTime
        {
            get
            {
                return NormalShifts == null ? 0 : NormalShifts.Sum(x => x.RestPeriod);
            }
        } // اجمالي فترة الاستراحة لكامل الفترات وهو للقراءة فقط

        [UserInterfaceParameter(Order = 1)]
        public virtual IList<ParticularOvertimeShift> ParticularOvertimeShifts { get; set; } // الفترات الخاصة بالوردية - لها علاقة بالاضافي الخاص ويجب ان تكون ضمن الادنى والاعلى لاي فترة ولا يجوز تقاطعها  مع فترتين
        public virtual void AddParticularWorkshop(ParticularOvertimeShift particularOvertimeShift)
        {
            ParticularOvertimeShifts.Add(particularOvertimeShift);
            particularOvertimeShift.Workshop = this;
        }


        [UserInterfaceParameter(Order = 1)]
        public virtual IList<TemporaryWorkshop> TemporaryWorkshops { get; set; } // الورديات الاستثنائية للوردية الحالية
        public virtual void AddTemporaryWorkshop(TemporaryWorkshop temporaryWorkshop)
        {
            TemporaryWorkshops.Add(temporaryWorkshop);
            temporaryWorkshop.Workshop = this;
        }


        [UserInterfaceParameter(Order = 1)]
        public virtual IList<NormalShift> NormalShifts { get; set; } // فترات الوردية العادية
        public virtual void AddNormalShift(NormalShift normalShift)
        {
            NormalShifts.Add(normalShift);
            normalShift.Workshop = this;
        }
    }
}
