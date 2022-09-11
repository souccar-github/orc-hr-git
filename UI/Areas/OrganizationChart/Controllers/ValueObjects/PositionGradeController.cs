#region

using System.Linq;
using System.Web.Mvc;
using Infrastructure.Validation;
using HRIS.Domain.OrgChart.ValueObjects;
using HRIS.Domain.OrgChart.ValueObjects.AssignedGrade;
using UI.Areas.OrganizationChart.Controllers.EntitiesRoots;
using UI.Extensions;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.OrganizationChart.ValueObjects;

#endregion

namespace UI.Areas.OrganizationChart.Controllers.ValueObjects
{
    public class PositionGradeController : NodeAggregateController, IRule<PositionGrade>
    {
        #region Parents Chain

        #region Position

        public Position Position
        {
            get
            {
                if (GetMasterRecordValue(MasterRecordOrder.Second) != 0)
                {
                    return PositionService.LoadById(GetMasterRecordValue(MasterRecordOrder.Second));
                }

                return null;
            }
        }

        #endregion

        #region PositionGrade

        public PositionGrade PositionGrade
        {
            get { return Position.Grades.First(); }
        }

        #endregion

        #region PositionExist

        public bool PositionExist
        {
            get { return GetMasterRecordValue(MasterRecordOrder.Second) != 0; }
        }

        #endregion

        #endregion

        #region Overrides of NodeAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("GroupSalaryScale.Name");
        }

        #endregion

        #region Implementation of IRule<Grade>

        public ObjectRules<PositionGrade> Rules
        {
            get { return new PositionGradeRules(); }
        }

        #endregion

        #region CRUD

        #region Read

        public ActionResult Index()
        {
            SetMasterRecordValue(MasterRecordOrder.Third, PositionGrade.Id);

            PrePublish();

            SaveTabIndex(0);

            return PartialView("Select", PositionGrade);
        }

        #endregion

        #region Create

        [HttpPost]
        public ActionResult Save(PositionGrade positionGrade)
        {
            PrePublish();

            positionGrade.Position = PositionGrade.Position;
            positionGrade.Assets = PositionGrade.Assets;
            positionGrade.NonCashBenefits = PositionGrade.NonCashBenefits;
            positionGrade.CashBenefits = PositionGrade.CashBenefits;
            positionGrade.CashDeductions = PositionGrade.CashDeductions;
            positionGrade.Insurances = PositionGrade.Insurances;

            this.UpdateValueObject(positionGrade, PositionGrade);


            if ((Rules.GetBrokenRules(positionGrade).Count == 0) && (TryValidateModel(positionGrade)))
            {
                PositionService.Update(Position);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(positionGrade));

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("Edit", positionGrade)
                                });
            }

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml =
                            RenderPartialViewToString("Select",
                                                      Position.Grades
                                                          .First())
                            });
        }

        #endregion

        #region Update

        public ActionResult Edit()
        {
            return View("Edit", PositionGrade);
        }

        #endregion

        #region Delete

        public ActionResult Delete()
        {
            PrePublish();

            //Position position = Node.Positions.Single(p => p.Id == PositionGrade.Position.Id);

            Position.Grades.Remove(PositionGrade);

           //todo remove all grade id from position
            //position.GradeId = 0;

            PositionService.Update(Position);

            PrePublish();

            return RedirectToAction("Index", "Position");
        }

        #endregion

        #endregion
    }
}