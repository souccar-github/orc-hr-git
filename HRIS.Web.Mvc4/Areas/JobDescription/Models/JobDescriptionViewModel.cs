using System.Collections.Generic;
using System.Linq;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using System;
using HRIS.Domain.OrganizationChart.RootEntities;
using  Project.Web.Mvc4.Extensions;
using  Project.Web.Mvc4.Helpers.Resource;
using Souccar.Infrastructure.Extenstions;
using Souccar.Infrastructure.Core;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.JobDescription.Entities;
using Souccar.Domain.Validation;
using  Project.Web.Mvc4.Helpers;


namespace Project.Web.Mvc4.Areas.JobDescription.Models
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class JobDescriptionViewModel:ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            var temp= requestInformation.NavigationInfo.Next.SingleOrDefault(x => x.TypeName == "HRIS.Domain.JobDescription.Entities.Competence");
            if (temp != null)
            {
                temp.CSSClass = "detail-extra-space";
            }
        }
    }   
}