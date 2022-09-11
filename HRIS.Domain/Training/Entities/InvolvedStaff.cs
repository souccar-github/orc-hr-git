//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//project manager:
//supervisor:
//author: Yaseen Alrefaee
//description:
//start date:
//end date:
//last update:
//update by: Ammar Alziebak
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
using HRIS.Domain.Training.RootEntities;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Personnel.RootEntities;

namespace HRIS.Domain.Training.Entities
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class InvolvedStaff:Entity
    {
        public virtual Employee Employee { get; set; }
        public virtual Course Course { get; set; }
        //public virtual NewCourse NewCourse { get; set; }
    }
}
