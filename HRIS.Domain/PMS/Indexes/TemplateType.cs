#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//project manager:
//supervisor:
//author: Ammar Alziebak
//description:
//start date:
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion

#region

using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.PMS.Indexes
{
    /// <summary>
    /// نوع الاستمارة
    /// </summary>
    [Module("PMS")]
    public class TemplateType : IndexEntity, IAggregateRoot
    {
    }
}