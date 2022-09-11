using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using Souccar.Core.CustomAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Personnel.RootEntities
{
    public partial class Employee
    {
        //public virtual string EmploymentPeriodDuration
        //{
        //    get
        //    {
        //        try
        //        {
        //            int years = 0, months = 0, days = 0;

        //            //todo please return to Khaled
        //            //var lastStartupInformation = StartupInformations.OrderByDescending(si => si.StartDate).First();

        //            //if (lastStartupInformation != null)
        //            //{
        //            //    DateTime date1 = DateTime.Today;
        //            //    DateTime date2 = lastStartupInformation.StartDate;

        //            //    int oldMonth = date2.Month;
        //            //    while (oldMonth == date2.Month)
        //            //    {
        //            //        date1 = date1.AddDays(-1);
        //            //        date2 = date2.AddDays(-1);
        //            //    }

        //            //    // getting number of years
        //            //    while (date2.CompareTo(date1) >= 0)
        //            //    {
        //            //        years++;
        //            //        date2 = date2.AddYears(-1);
        //            //    }
        //            //    date2 = date2.AddYears(1);
        //            //    years--;

        //            //    // getting number of months and days
        //            //    oldMonth = date2.Month;
        //            //    while (date2.CompareTo(date1) >= 0)
        //            //    {
        //            //        days++;
        //            //        date2 = date2.AddDays(-1);
        //            //        if ((date2.CompareTo(date1) >= 0) && (oldMonth != date2.Month))
        //            //        {
        //            //            months++;
        //            //            days = 0;
        //            //            oldMonth = date2.Month;
        //            //        }
        //            //    }
        //            //    date2 = date2.AddDays(1);
        //            //    days--;
        //            //}

        //            return years + " years, " + months + " months, " + days + " days.";

        //        }
        //        catch
        //        {
        //            return string.Empty;
        //        }

        //    }
        //}



    }
}
