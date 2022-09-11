using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using HRIS.Domain.Personnel.Entities;
using Service;
using Souccar.ReportGenerator.Domain.Classification;
using UI.Extensions;
using Souccar.Domain.Extensions;

namespace UI.Areas.Reporting.Helpers
{
    public class ReportingDropDownListHelpers
    {
        #region Entities

        public static SelectList ListOfReportTemplates
        {
            get
            {
                List<ReportTemplate> reportTemplates = new EntityService<ReportTemplate>().GetList();

                return reportTemplates.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        #endregion

        #region Value Objects

        #endregion

        #region Indexes

        public static SelectList ListOfAggregateClasses
        {
            get
            {
                return
                    Assembly.GetAssembly(typeof (Employee)).GetAggregateClasses().ToList().SelectFromList(
                        x => x.Key.FullName, y => y.Value);
            }
        }

        #endregion
    }
}