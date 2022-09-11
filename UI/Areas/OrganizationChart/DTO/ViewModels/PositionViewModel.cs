using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.OrgChart.ValueObjects;
using UI.Areas.OrganizationChart.Helpers;
using UI.Areas.PMSComprehensive.DTO.ViewModels;

namespace UI.Areas.OrganizationChart.DTO.ViewModels
{
    public class PositionViewModel
    {
        //public PositionViewModel()
        //{
        //    Grades = new List<GradeViewModel>();
        //    PositionLevels = new List<PositionLevelViewModel>();
        //}

        //public Position Position { get; set; }
        //public List<GradeViewModel> Grades { get; set; }
        //public List<PositionLevelViewModel> PositionLevels { get; set; }

        public Position Position { get; set; }
        public int NodeId { get; set; }
        public int? GradeId { get; set; }
        public int? ParentPositionId { get; set; }
        //public SelectList ListOfParentPositions
        //{
        //    get
        //    {   if (NodeId==0)
        //        {
        //            throw new ArgumentException("NodeId");
        //        }
        //        return DropDownListHelpers.ListOfPositionsOfParentNode(NodeId);
        //    }
        //}
        //public SelectList ListOfGrades
        //{
        //    get { return DropDownListHelpers.ListOfGrades; }
        //}
        //public SelectList ListOfJobTitles {
        //    get { return DropDownListHelpers.ListOfJobTitle; }
        //}
        //public SelectList ListOfPositionType
        //{
        //    get { return DropDownListHelpers.ListOfPositionType; }
        //}
        //public SelectList ListOfPositionLevel
        //{
        //    get { return DropDownListHelpers.ListOfPositionLevel; }
        //}
        //public SelectList ListOfCostCenter
        //{
        //    get { return DropDownListHelpers.ListOfCostCenter; }
        //}
        //public SelectList ListOfTimeIntervals
        //{
        //    get { return DropDownListHelpers.ListOfTimeIntervals; }
        //}
        //public SelectList ListOfDisabilityStatus
        //{
        //    get { return DropDownListHelpers.ListOfDisabilityStatus; }
        //}
        
    }
}