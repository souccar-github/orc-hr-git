#region

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.OrgChart.Indexes;
using Service;
using UI.Extensions;
using UI.Helpers.Cache;
using HRIS.Domain.PMS.Indexes;


#endregion

namespace UI.Areas.PMSComprehensive.Helpers
{
    public class DropDownListHelpers
    {
        #region Entities

        #endregion

        #region Value Objects

        #endregion

        #region Indexes

        

        public static SelectList ListOfTemplateType
        {
            get
            {
                List<TemplateType> templateTypes = CacheProvider.Get(PMSComprehensiveCacheKeys.TemplateType.ToString(),
                                                            () =>
                                                            new EntityService<TemplateType>().GetList());

                return templateTypes.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfOrganizationalLevel
        {
            get
            {
                List<OrganizationalLevel> organizationalLevels = CacheProvider.Get(PMSComprehensiveCacheKeys.OrganizationalLevel.ToString(),
                                                            () =>
                                                            new EntityService<OrganizationalLevel>().GetAll().OrderBy(x=>x.Order).ToList());

                return organizationalLevels.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        

        #endregion
    }
}