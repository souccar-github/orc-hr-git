#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 23/03/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference
using HRIS.Domain.JobDescription.RootEntities;

using HRIS.Domain.OrganizationChart.RootEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#endregion
namespace Project.Web.Mvc4.Models.Services
{
    public class EmployeeActionViewModel
    {
        public string NodeName { get; set; }
        public string JobTitleName { get; set; }
        public string JobDescriptionName { get; set; }
        public string PositionName { get; set; }
        public int EmployeeId { get; set; }
        public int PositionId { get; set; }
        public string FullName { get; set; }
        public string ActionTitle { get; set; }
    }
}