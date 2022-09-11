using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.OrgChart.Entities;

namespace UI.Areas.PMSComprehensive.DTO.ViewModels
{
    public class GradeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PositionLevel { get; set; }
        public bool Checked { get; set; }

        public static GradeViewModel Create(Grade grade)
        {
            return new GradeViewModel { Id = grade.Id, Name = grade.Name.Name, PositionLevel = grade.Level.Name };
        }
    }
}