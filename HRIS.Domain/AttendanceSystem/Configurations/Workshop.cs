using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using FluentNHibernate.Automapping;
using HRIS.Domain.AttendanceSystem.DTO;
using HRIS.Domain.AttendanceSystem.Entities;
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Core.Utilities;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.AttendanceSystem.Configurations
{//todo : Mhd Update changeset no.1
    [Order(25)]
    [Module(ModulesNames.AttendanceSystem)]
    public class Workshop : Entity, IConfigurationRoot // وردية دوام
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

        [UserInterfaceParameter(Order = 1, IsHidden = true)]//
        public virtual double TotalWorkHours
        {
            get
            {
                return NormalShifts == null ? 0 : NormalShifts.Sum(x => x.ExitTime >= x.EntryTime ? (x.ExitTime - x.EntryTime).TotalHours : (x.ExitTime - x.EntryTime).TotalHours + 24);
            }
        } // اجمالي ساعات العمل لكل الفترات وهو حقل للقراءة فقط

        [UserInterfaceParameter(Order = 11)]
        public virtual string TotalWorkHoursFormatedValue
        {
            get
            {
                var result = NormalShifts.Sum(x =>
                {
                    var x1 = x.Prepare(DateTime.Now);
                    return (x1.ExitTime - x1.EntryTime).TotalMinutes;
                }) / 60;
                return DateTimeFormatter.ConvertDoubleToTimeFormat(result);
            }
        } // الاضافي الاجمالي المحتسب العطل

        [UserInterfaceParameter(Order = 1, IsHidden = true)]//
        public virtual int TotalRestTime
        {
            get
            {
                return NormalShifts == null ? 0 : NormalShifts.Sum(x => x.RestPeriod);
            }
        } // اجمالي فترة الاستراحة لكامل الفترات وهو للقراءة فقط

        [UserInterfaceParameter(Order = 1)]
        public virtual string TotalRestTimeFormatedValue
        {
            get
            {
                var result = (double)NormalShifts.Sum(x => x.RestPeriod) / 60.00;
                return DateTimeFormatter.ConvertDoubleToTimeFormat(result);
            }
        } // اجمالي فترة الاستراحة لكامل الفترات وهو للقراءة فقط

        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown
        {
            get { return Description; }
        }
        [UserInterfaceParameter(Order = 1)]
        public virtual IList<NormalShift> NormalShifts { get; set; } // فترات الوردية العادية
        public virtual void AddNormalShift(NormalShift normalShift)
        {
            normalShift.Workshop = this;
            NormalShifts.Add(normalShift);
        }

        [UserInterfaceParameter(Order = 2)]
        public virtual IList<ParticularOvertimeShift> ParticularOvertimeShifts { get; set; } // الفترات الخاصة بالوردية - لها علاقة بالاضافي الخاص ويجب ان تكون ضمن الادنى والاعلى لاي فترة ولا يجوز تقاطعها  مع فترتين
        public virtual void AddParticularWorkshop(ParticularOvertimeShift particularOvertimeShift)
        {
            particularOvertimeShift.Workshop = this;
            ParticularOvertimeShifts.Add(particularOvertimeShift);
        }


        [UserInterfaceParameter(Order = 3)]
        public virtual IList<TemporaryWorkshop> TemporaryWorkshops { get; set; } // الورديات الاستثنائية للوردية الحالية
        public virtual void AddTemporaryWorkshop(TemporaryWorkshop temporaryWorkshop)
        {
            temporaryWorkshop.Workshop = this;
            TemporaryWorkshops.Add(temporaryWorkshop);
        }


       
        public virtual Workshop Prepare(DateTime date)
        {
            var result = new Workshop();

            Mapper.Map(this, result);
            result.NormalShifts = new List<NormalShift>();
            result.ParticularOvertimeShifts = new List<ParticularOvertimeShift>();
            var tempDate = date.Date;
            for (var i = 0; i < NormalShifts.OrderBy(x => x.NormalShiftOrder).Count(); i++)
            {
                var preparedShift = NormalShifts[i].Prepare(tempDate);
                result.NormalShifts.Add(preparedShift);
                //result.NormalShifts[i].ShiftRangeStartTime = tempDate.Date.AddHours(result.NormalShifts[i].ShiftRangeStartTime.Value.Hour).AddMinutes(result.NormalShifts[i].ShiftRangeStartTime.Value.Minute);
                //result.NormalShifts[i].EntryTime = tempDate.Date.AddHours(result.NormalShifts[i].EntryTime.Value.Hour).AddMinutes(result.NormalShifts[i].EntryTime.Value.Minute);
                //result.NormalShifts[i].RestRangeStartTime = tempDate.Date.AddHours(result.NormalShifts[i].RestRangeStartTime.Value.Hour).AddMinutes(result.NormalShifts[i].RestRangeStartTime.Value.Minute);
                //result.NormalShifts[i].RestRangeEndTime = tempDate.Date.AddHours(result.NormalShifts[i].RestRangeEndTime.Value.Hour).AddMinutes(result.NormalShifts[i].RestRangeEndTime.Value.Minute);
                //result.NormalShifts[i].ExitTime = tempDate.Date.AddHours(result.NormalShifts[i].ExitTime.Value.Hour).AddMinutes(result.NormalShifts[i].ExitTime.Value.Minute);
                //result.NormalShifts[i].ShiftRangeEndTime = tempDate.Date.AddHours(result.NormalShifts[i].ShiftRangeEndTime.Value.Hour).AddMinutes(result.NormalShifts[i].ShiftRangeEndTime.Value.Minute);

                //if (result.NormalShifts[i].EntryTime < result.NormalShifts[i].ShiftRangeStartTime)
                //{
                //    result.NormalShifts[i].EntryTime = result.NormalShifts[i].EntryTime.Value.AddDays(1);
                //}
                //if (result.NormalShifts[i].RestRangeStartTime < result.NormalShifts[i].EntryTime)
                //{
                //    result.NormalShifts[i].RestRangeStartTime = result.NormalShifts[i].RestRangeStartTime.Value.AddDays(1);
                //}
                //if (result.NormalShifts[i].RestRangeEndTime < result.NormalShifts[i].RestRangeStartTime)
                //{
                //    result.NormalShifts[i].RestRangeEndTime = result.NormalShifts[i].RestRangeEndTime.Value.AddDays(1);
                //}
                //if (result.NormalShifts[i].ExitTime < result.NormalShifts[i].RestRangeEndTime)
                //{
                //    result.NormalShifts[i].ExitTime = result.NormalShifts[i].ExitTime.Value.AddDays(1);
                //}
                //if (result.NormalShifts[i].ShiftRangeEndTime < result.NormalShifts[i].ExitTime)
                //{
                //    result.NormalShifts[i].ShiftRangeEndTime = result.NormalShifts[i].ShiftRangeEndTime.Value.AddDays(1);
                //}
                //tempDate = result.NormalShifts[i].ShiftRangeEndTime.Value.Date;
                tempDate = preparedShift.ShiftRangeEndTime.Date;
            }
            tempDate = date.Date;
            for (var i = 0; i < ParticularOvertimeShifts.Count(); i++)
            {
                var preparedShift = ParticularOvertimeShifts[i].Prepare(tempDate);
                result.ParticularOvertimeShifts.Add(preparedShift);
                //result.ParticularOvertimeShifts[i].StartTime = tempDate.Date.AddHours(result.ParticularOvertimeShifts[i].StartTime.Value.Hour).AddMinutes(result.ParticularOvertimeShifts[i].StartTime.Value.Minute);
                //result.ParticularOvertimeShifts[i].EndTime = tempDate.Date.AddHours(result.ParticularOvertimeShifts[i].EndTime.Value.Hour).AddMinutes(result.ParticularOvertimeShifts[i].EndTime.Value.Minute);

                //if (result.ParticularOvertimeShifts[i].EndTime < result.ParticularOvertimeShifts[i].StartTime)
                //{
                //    result.ParticularOvertimeShifts[i].EndTime = result.ParticularOvertimeShifts[i].EndTime.Value.AddDays(1);
                //}
                //tempDate = result.ParticularOvertimeShifts[i].EndTime.Value.Date;
                tempDate = preparedShift.EndTime.Date;
            }

            return result;
        }

    }
}
