﻿using System.Web.Mvc;

namespace Project.Web.Mvc4.Areas.Recruitment
{
    public class RecruitmentAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Recruitment";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Recruitment_default",
                "Recruitment/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
