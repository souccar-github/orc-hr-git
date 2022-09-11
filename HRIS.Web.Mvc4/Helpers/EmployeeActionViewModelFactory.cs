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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.JobDescription.Entities;
using  Project.Web.Mvc4.Models.Services;
using  Project.Web.Mvc4.Helpers.DomainExtensions;
using HRIS.Domain.Personnel.RootEntities;
#endregion
namespace Project.Web.Mvc4.Helpers
{
    public class EmployeeActionViewModelFactory
    {
        public static EmployeeActionViewModel Create(Position position, string actionTitle)
        {
            var result = new EmployeeActionViewModel();

            result.NodeName = (position.JobDescription.Node != null) ? position.JobDescription.Node.Name : string.Empty;
            result.JobTitleName = (position.JobDescription.JobTitle != null) ? position.JobDescription.JobTitle.Name : string.Empty;
            result.JobDescriptionName = (position != null) ? position.JobDescription.Name : string.Empty;
            result.PositionName = (position != null) ? position.NameForDropdown : string.Empty;
            result.EmployeeId = (position.Employee != null) ? position.Employee.Id : 0;
            result.PositionId = (position != null) ? position.Id : 0;
            result.FullName = (position.Employee != null) ? position.Employee.FullName : position.NameForDropdown;
            result.ActionTitle = actionTitle;

            return result;
        }

        public static EmployeeActionViewModel Create(Employee employee, string actionTitle)
        {
            var result = new EmployeeActionViewModel();

            var position = employee.PrimaryPosition();
            result.NodeName = (position != null && position.JobDescription.Node != null) ? position.JobDescription.Node.Name : string.Empty;
            result.JobTitleName = (position != null && position.JobDescription.JobTitle != null) ? position.JobDescription.JobTitle.Name : string.Empty;
            result.JobDescriptionName = (position != null) ? position.JobDescription.Name : string.Empty;
            result.PositionName = (position != null) ? position.NameForDropdown : string.Empty;
            result.EmployeeId = employee.Id;
            result.PositionId = (position != null) ? position.Id : 0;
            result.FullName = employee.FullName;
            result.ActionTitle = actionTitle;
            return result;

            //var lastAssigningEmployeeToPosition = employee.EmployeeCard.Assignments.LastOrDefault();
            //var position = lastAssigningEmployeeToPosition != null ? lastAssigningEmployeeToPosition.Position : null;
            //result.NodeName = (position != null && position.JobDescription.Node != null) ? position.JobDescription.Node.Name : string.Empty;
            //result.JobTitleName = (position != null && position.JobDescription.JobTitle != null) ? position.JobDescription.JobTitle.Name : string.Empty;
            //result.JobDescriptionName = (position != null) ? position.JobDescription.Name : string.Empty;
            //result.PositionName = (position != null) ? position.NameForDropdown : string.Empty;
            //result.EmployeeId = employee.Id;
            //result.PositionId = (position != null) ? position.Id : 0;
            //result.FullName = employee.FullName;
            //result.ActionTitle = actionTitle;
        }
    }
}