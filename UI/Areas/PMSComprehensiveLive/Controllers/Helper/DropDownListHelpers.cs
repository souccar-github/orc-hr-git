#region

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UI.Extensions;
using UI.Helpers.Cache;
using HRIS.Domain.PMS.Entities;
using HRIS.Domain.PMS.Annotations.Indexes;
using HRIS.Domain.PMS.RootEntities;

#endregion

namespace UI.Areas.PMSComprehensiveLive.Helpers
{
    public class DropDownListHelpers
    {
        #region Entities

        #endregion

        #region Value Objects

        #endregion

        #region Indexes

/*        public static SelectList ListOfAppraisalType
        {
            get
            {
                List<AppraisalType> types = CacheProvider.Get(PMSComprehensiveLiveCacheKeys.AppraisalType.ToString(),
                                                            () =>
                                                            new EntityService<AppraisalType>().GetList());

                return types.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }
        
        public static SelectList ListOfAppraisalPeriod
        {
            get
            {
                List<AppraisalPeriod> appraisalPeriods = CacheProvider.Get(PMSComprehensiveLiveCacheKeys.AppraisalPeriod.ToString(),
                                                            () =>
                                                            new EntityService<AppraisalPeriod>().GetList());

                return appraisalPeriods.SelectFromList(x => x.Id.ToString(), y => y.Period);
            }
        }*/





        #endregion
    }
}