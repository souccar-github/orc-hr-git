using HRIS.Domain.Global.Enums;
using HRIS.Domain.Workflow;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Extensions;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;

using Souccar.Domain.Extensions;
using Souccar.Infrastructure.Extenstions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Domain.DomainModel;
using Souccar.Core.Extensions;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Areas.Workflow.Models.Models
{
    public class PhasePeriodViewModel : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            if (type.BaseType == typeof(PhasePeriod))
            {
                model.ViewModelTypeFullName = typeof(PhasePeriodViewModel).FullName;
                model.Views[0].EditHandler = "PhasePeriodEditHandler";
            }
        }
     
       
        public override void BeforeValidation(RequestInformation requestInformation, Souccar.Domain.DomainModel.Entity entity, IDictionary<string, object> originalState, CrudOperationType operationType, string customInformation = null)
        {
            base.BeforeValidation(requestInformation, entity, originalState, operationType, customInformation);
            var phasePeriod = entity as PhasePeriod;
            if ((phasePeriod.Period == Period.Monthly && phasePeriod.Year != 0 && (phasePeriod.Month != 0 && phasePeriod.Month != Month.Nothing)) || 
                (phasePeriod.Period == Period.Annual && phasePeriod.Year != 0) ||
                (phasePeriod.Period == Period.Quarterly && phasePeriod.Year != 0 && (phasePeriod.Quarter != 0 && phasePeriod.Quarter != Quarter.Nothing)) || 
                (phasePeriod.Period == Period.SemiAnnual && phasePeriod.Year != 0 && (phasePeriod.SemiAnnual != 0 && phasePeriod.SemiAnnual != SemiAnnual.Nothing)) ||
                phasePeriod.Period == Period.Custom)
                updateStartDateAndEndDate(phasePeriod);
        }
        public override void AfterValidation(RequestInformation requestInformation, Souccar.Domain.DomainModel.Entity entity, IDictionary<string, object> originalState, IList<Souccar.Domain.Validation.ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var phasePeriod = entity as PhasePeriod;
            if (phasePeriod.Period != Period.Custom && ((phasePeriod.Period == Period.Monthly && (phasePeriod.Month == 0 || phasePeriod.Month == Month.Nothing)) ||
                (phasePeriod.Year == 0) ||
                (phasePeriod.Period == Period.Quarterly && (phasePeriod.Quarter == 0 || phasePeriod.Quarter == Quarter.Nothing)) ||
                (phasePeriod.Period == Period.SemiAnnual && (phasePeriod.SemiAnnual == 0 || phasePeriod.SemiAnnual == SemiAnnual.Nothing))))
            {
                if (phasePeriod.Year == 0)
                {
                    var prop = phasePeriod.GetType().GetProperty("Year");
                    validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                    {
                        Property = prop,
                        Message = GlobalResource.RequiredMessage
                    });
                }
                else if (phasePeriod.Period == Period.SemiAnnual && (phasePeriod.SemiAnnual == 0 || phasePeriod.SemiAnnual == SemiAnnual.Nothing))
                {
                    var prop = phasePeriod.GetType().GetProperty("SemiAnnual");
                    validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                    {
                        Property = prop,
                        Message = GlobalResource.RequiredMessage
                    });
                }
                else if (phasePeriod.Period == Period.Quarterly && (phasePeriod.Quarter == 0 || phasePeriod.Quarter == Quarter.Nothing))
                {
                    var prop = phasePeriod.GetType().GetProperty("Quarter");
                    validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                    {
                        Property = prop,
                        Message = GlobalResource.RequiredMessage
                    });
                }
                else if (phasePeriod.Period == Period.Monthly && (phasePeriod.Month == 0 || phasePeriod.Month == Month.Nothing))
                {
                    var prop = phasePeriod.GetType().GetProperty("Month");
                    validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                    {
                        Property = prop,
                        Message = GlobalResource.RequiredMessage
                    });
                }
            }
                if (phasePeriod.Period != Period.Custom && phasePeriod.Year < 1900)
            {
                var prop = phasePeriod.GetType().GetProperty("Year");
                validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                {
                    Property = prop,
                    Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.GreaterThanEqMessage, 1900)
                });
            }

            if (phasePeriod.Period == Period.Custom && phasePeriod.StartDate >= phasePeriod.EndDate)
            {

                var prop = phasePeriod.GetType().GetProperty("EndDate");
                validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                {
                    Property = prop,
                    Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.GreaterThanMessage, phasePeriod.StartDate)
                });


            }

          }
        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var currentPhase = entity as PhasePeriod;
            currentPhase.Name = currentPhase.getPhaseName();
        }

        protected void updateStartDateAndEndDate(PhasePeriod phasePeriod)
        {
            if ((phasePeriod.Year <= 0) && (phasePeriod.Period != HRIS.Domain.Global.Enums.Period.Custom))
                return;
            switch (phasePeriod.Period)
            {
                case HRIS.Domain.Global.Enums.Period.Annual:
                    phasePeriod.StartDate = new DateTime(phasePeriod.Year, 1, 1);
                    phasePeriod.EndDate = new DateTime(phasePeriod.Year, 12, 31);
                    phasePeriod.SemiAnnual = SemiAnnual.Nothing;
                    phasePeriod.Quarter = Quarter.Nothing;
                    phasePeriod.Month = Month.Nothing;

                    break;
                case HRIS.Domain.Global.Enums.Period.Custom:
                    phasePeriod.Year = phasePeriod.StartDate.Year;
                    phasePeriod.SemiAnnual = SemiAnnual.Nothing;//.H1;
                    phasePeriod.Quarter = Quarter.Nothing;//.Q1;
                    phasePeriod.Month = Month.Nothing;//.January;
                    break;
                case HRIS.Domain.Global.Enums.Period.Monthly:
                    phasePeriod.StartDate = new DateTime(phasePeriod.Year, (int)phasePeriod.Month, 1);
                    phasePeriod.EndDate = new DateTime(phasePeriod.Year, (int)phasePeriod.Month, DateTime.DaysInMonth(phasePeriod.Year, (int)phasePeriod.Month));
                    if (phasePeriod.Month >= Month.January && phasePeriod.Month <= Month.June)
                    {
                        phasePeriod.SemiAnnual = SemiAnnual.Nothing;//.H1;
                        if (phasePeriod.Month >= Month.January && phasePeriod.Month <= Month.March)
                        {
                            phasePeriod.Quarter = Quarter.Nothing;//.Q1;
                        }
                        else
                        {
                            phasePeriod.Quarter = Quarter.Nothing;//.Q2;
                        }
                    }
                    else
                    {
                        phasePeriod.SemiAnnual = SemiAnnual.Nothing;//.H2;
                        if (phasePeriod.Month >= Month.July && phasePeriod.Month <= Month.August)
                        {
                            phasePeriod.Quarter = Quarter.Nothing;//.Q3;
                        }
                        else
                        {
                            phasePeriod.Quarter = Quarter.Nothing;//.Q4;
                        }

                    }
                    break;
                case HRIS.Domain.Global.Enums.Period.Quarterly:
                    switch (phasePeriod.Quarter)
                    {
                        case HRIS.Domain.Global.Enums.Quarter.Q1:
                            phasePeriod.StartDate = new DateTime(phasePeriod.Year, 1, 1);
                            phasePeriod.EndDate = new DateTime(phasePeriod.Year, 3, DateTime.DaysInMonth(phasePeriod.Year, 3));
                            phasePeriod.SemiAnnual = SemiAnnual.Nothing;//.H1;
                            phasePeriod.Month = Month.Nothing;//.January;

                            break;
                        case HRIS.Domain.Global.Enums.Quarter.Q2:
                            phasePeriod.StartDate = new DateTime(phasePeriod.Year, 4, 1);
                            phasePeriod.EndDate = new DateTime(phasePeriod.Year, 6, DateTime.DaysInMonth(phasePeriod.Year, 6));
                            phasePeriod.SemiAnnual = SemiAnnual.Nothing;//.H1;
                            phasePeriod.Month = Month.Nothing;//.April;

                            break;
                        case HRIS.Domain.Global.Enums.Quarter.Q3:
                            phasePeriod.StartDate = new DateTime(phasePeriod.Year, 7, 1);
                            phasePeriod.EndDate = new DateTime(phasePeriod.Year, 9, DateTime.DaysInMonth(phasePeriod.Year, 9));
                            phasePeriod.SemiAnnual = SemiAnnual.Nothing;//.H2;
                            phasePeriod.Month = Month.Nothing;//.July;

                            break;
                        case HRIS.Domain.Global.Enums.Quarter.Q4:
                            phasePeriod.StartDate = new DateTime(phasePeriod.Year, 10, 1);
                            phasePeriod.EndDate = new DateTime(phasePeriod.Year, 12, DateTime.DaysInMonth(phasePeriod.Year, 12));
                            phasePeriod.SemiAnnual = SemiAnnual.Nothing;//.H2;
                            phasePeriod.Month = Month.Nothing;//.October;
                            break;
                        default:
                            break;
                    }
                    break;
                case HRIS.Domain.Global.Enums.Period.SemiAnnual:
                    switch (phasePeriod.SemiAnnual)
                    {
                        case HRIS.Domain.Global.Enums.SemiAnnual.H1:
                            phasePeriod.StartDate = new DateTime(phasePeriod.Year, 1, 1);
                            phasePeriod.EndDate = new DateTime(phasePeriod.Year, 6, DateTime.DaysInMonth(phasePeriod.Year, 6));
                            phasePeriod.Quarter = Quarter.Nothing;//.Q1;
                            phasePeriod.Month = Month.Nothing;//.January;
                            break;
                        case HRIS.Domain.Global.Enums.SemiAnnual.H2:
                            phasePeriod.StartDate = new DateTime(phasePeriod.Year, 7, 1);
                            phasePeriod.EndDate = new DateTime(phasePeriod.Year, 12, DateTime.DaysInMonth(phasePeriod.Year, 12));
                            phasePeriod.Quarter = Quarter.Nothing;//.Q2;
                            phasePeriod.Month = Month.Nothing;//.June;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}