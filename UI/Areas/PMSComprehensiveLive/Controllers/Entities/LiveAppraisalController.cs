#region

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using FizzWare.NBuilder;
using HRIS.Domain.JobDesc.Entities;
using HRIS.Domain.JobDesc.ValueObjects;
using HRIS.Domain.Objectives.Entities;
using HRIS.Domain.Objectives.Indexes;
using HRIS.Domain.OrgChart.ValueObjects;
using HRIS.Domain.PMS.Entities.Organizational;
using HRIS.Domain.PMS.Entities.Template;
using HRIS.Domain.PMS.Enums;
using HRIS.Domain.PMS.RootEntities;
using HRIS.Domain.Personnel.Indexes;

using Infrastructure.Validation;
using HRIS.Domain.JobDesc.Indexes;
using HRIS.Domain.PMS.Entities;
using HRIS.Domain.PMS.Entities.Competency;
using HRIS.Domain.PMS.Entities.Development;
using HRIS.Domain.PMS.Entities.JobDescription;
using HRIS.Domain.PMS.Entities.Objective;
using Repository.UnitOfWork;
using Service;
using Service.PMSComprehensive;
using Service.OrgChart;
using Service.Personnel;
using Telerik.Web.Mvc;
using UI.Areas.PMSComprehensiveLive.Controllers.EntitiesRoots;
using UI.Areas.PMSComprehensiveLive.Helpers;
using UI.Extensions;
using UI.Helpers.Cache;
using UI.Helpers.Model;
using Validation.PMSComprehensive.Entities;
using Service.PMSComprehensive;

#endregion

namespace UI.Areas.PMSComprehensiveLive.Controllers.Entities
{
    public class LiveAppraisalController : LiveAppraisalAggregateController, IRule<Appraisal>
    {
        #region IRule<Appraisal> Members

        public ObjectRules<Appraisal> Rules
        {
            get { return new AppraisalRules(); }
        }

        #endregion

        #region Overrides of ApprasialAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("Type.Name");
            ModelState.Remove("Period.Period");
            ModelState.Remove("Employee.LastName");
        }

        #endregion

        public ActionResult Index()
        {
            #region Self Assesment

            // var employee = EmployeeHelpers.GetByLoginName(User.Identity.Name);

            //   var newAppraisal = AppraisalHelper.CreateAppraisalForEmployee(employee);

            #region Intialize Appraisal
            var newAppraisal = IntializeAppraisal();
            newAppraisal.Id = 555;
            #endregion


            TempData["MyAppraisal"] = newAppraisal;

            return View("Index", newAppraisal);

            #endregion
        }

        public ActionResult LoadSections()
        {
            return View("SectionsMenu");
        }


        #region Competency Section
        public ActionResult CompetencySection(int appraisalId)
        {
            //  var appraisal = Service.LoadById(appraisalId);
            var appraisal = IntializeAppraisal();
            appraisal.Id = 555;
            // ViewData["CompetencySection"] = appraisal.CompetencySections;

            var competencyTypes = GetDistinctCompetencyTypes(appraisal.CompetencySections.SingleOrDefault());

            ViewData["CompetencyTypes"] = competencyTypes;

            return Json(new
            {
                Success = true,
                PartialViewHtml =
            RenderPartialViewToString("CompetencySection", appraisal.CompetencySections.SingleOrDefault())
            });
        }

        //todo test Save
        [HttpPost]
        public ActionResult SaveCompetencySection(CompetencySection competencySection)
        {
            Appraisal appraisal;
            try
            {
                competencySection.SetTotalRate();
                appraisal = Service.GetById(competencySection.Appraisal.Id);

                this.UpdateValueObject(competencySection,
                                       appraisal.CompetencySections.SingleOrDefault());
                for (var i = 0; i < competencySection.Items.Count; i++)
                {
                    this.UpdateValueObject(
                        competencySection.Items[i],
                        appraisal.CompetencySections.SingleOrDefault().Items[i]);
                    appraisal.CompetencySections.SingleOrDefault().Items[i].Section =
                        appraisal.CompetencySections.SingleOrDefault();
                }
                Service.Update(appraisal);
            }
            catch (Exception)
            {
                throw;
            }

            ViewData["CompetencyTypes"] =
                GetDistinctCompetencyTypes(appraisal.CompetencySections.SingleOrDefault());
            return Json(new
            {
                Success = true,
                PartialViewHtml =
            RenderPartialViewToString("CompetencySection",
                                      appraisal.CompetencySections.SingleOrDefault())
            });
        }

        private IList<string> GetDistinctCompetencyTypes(CompetencySection competencySection)
        {
            var distinctTypes =
                (from competencySectionItem in competencySection.Items
                 select competencySectionItem.Type).Distinct();
            return distinctTypes.ToList();
        }

        #endregion


        #region Customized Section

        public ActionResult CustomizedSection(int appraisalId, int sectionID)
        {
            // var appraisal = Service.LoadById(appraisalId);
            var appraisal = IntializeAppraisal();
            var customizedSection = appraisal.OrganizationalSections.Where(x => x.Id == sectionID).SingleOrDefault();
            return Json(new
            {
                Success = true,
                PartialViewHtml =
            RenderPartialViewToString("CustomizedSection",
                                      customizedSection)
            });
        }

        public ActionResult SaveCutomizedSection(OrganizationalSection customizedSection)
        {
            Appraisal appraisal;
            try
            {
                // CustomizedSectionHelper.CalulateCustomizedSectionRate(customizedSection);
                customizedSection.SetTotalRate();
                appraisal = Service.LoadById(customizedSection.Appraisal.Id);
                this.UpdateValueObject(customizedSection,
                                 appraisal.OrganizationalSections.SingleOrDefault(x => x.Id == customizedSection.Id));
                for (var i = 0; i < customizedSection.Items.Count; i++)
                {
                    this.UpdateValueObject(
                     customizedSection.Items[i],
                     appraisal.OrganizationalSections.SingleOrDefault(x => x.Id == customizedSection.Id).Items[i]);
                    appraisal.OrganizationalSections.SingleOrDefault(x => x.Id == customizedSection.Id).Items[i].Section =
                     appraisal.OrganizationalSections.SingleOrDefault(x => x.Id == customizedSection.Id);
                }
                Service.Update(appraisal);

            }
            catch (Exception)
            {

                throw;
            }
            appraisal = Service.LoadById(customizedSection.Appraisal.Id);
            return Json(new
            {
                Success = true,
                PartialViewHtml =
            RenderPartialViewToString("CustomizedSection",
                                      appraisal.OrganizationalSections.SingleOrDefault(x => x.Id == customizedSection.Id))
            });
        }


/*        public ActionResult GetCustomizedKPIList(int sectionId, int sectionItemId)
        {
            ViewData["CustomizedKPIList"] = CustomizedSectionHelper.GetKPI(sectionId, sectionItemId);
            return Json(new
            {
                Success = true,
                PartialViewHtml =
            RenderPartialViewToString("CustomizedSectionItemKPI",
                                      ViewData["CustomizedKPIList"])
            });
        }*/
        #endregion


        #region Objective Section

        public ActionResult ObjectiveSection(int appraisalId)
        {
            // var appraisal = Service.LoadById(appraisalId);
            var appraisal = IntializeAppraisal();
            appraisal.Id = 555;
            var objectiveSection = appraisal.ObjectiveSections.SingleOrDefault();
            // objectiveSection.ObjectiveSectionItems.OrderBy(y => y.IsShared);
            //var result = from o in objectiveSection.ObjectiveSectionItems
            //             group o by  o.IsShared
            //             into gro
            //             select gro;
            //ViewData["ObjectiveSection"] = result.ToList();

            //            foreach (var VARIABLE in result)
            //            {
            //                foreach (var objectiveSectionItem in VARIABLE)
            //                {

            //                }
            //            }
            return Json(new
            {
                Success = true,
                PartialViewHtml =
            RenderPartialViewToString("ObjectiveSection",
                                      objectiveSection)
            });
        }

        public ActionResult SaveObjectiveSection(ObjectiveSection objectiveSection)
        {
            Appraisal appraisal;
            try
            {
                objectiveSection.SetTotalRate();
                appraisal = Service.LoadById(objectiveSection.Appraisal.Id);
                this.UpdateValueObject(objectiveSection,
                                      appraisal.ObjectiveSections.SingleOrDefault());
                for (var i = 0; i < objectiveSection.Items.Count; i++)
                {
                    this.UpdateValueObject(
                     objectiveSection.Items[i],
                     appraisal.ObjectiveSections.SingleOrDefault().Items[i]);
                    appraisal.ObjectiveSections.SingleOrDefault().Items[i].Section =
                     appraisal.ObjectiveSections.SingleOrDefault();
                }
                Service.Update(appraisal);

            }
            catch (Exception)
            {

                throw;
            }
            appraisal = Service.LoadById(objectiveSection.Appraisal.Id);
            return Json(new
            {
                Success = true,
                PartialViewHtml =
            RenderPartialViewToString("ObjectiveSection",
                                      appraisal.ObjectiveSections.SingleOrDefault())
            });
        }

        //public ActionResult GetObjectiveKPIList(int sectionID)
        //{
        //    ViewData["KPIList"] = ObjectiveSectionHelper.GetKPI(sectionID);
        //    return Json(new
        //    {
        //        Success = true,
        //        PartialViewHtml =
        //    RenderPartialViewToString("ObjectiveSectionItemKPI",
        //                              ViewData["KPIList"])
        //    });
        //}

        #endregion


        #region JobDescription Section

        public ActionResult JobDescriptionSection(int appraisalId)
        {
            //var appraisal = Service.LoadById(appraisalId);
            var appraisal = IntializeAppraisal();
            appraisal.Id = 555;
            //ViewData["JobDescriptionSection"] = appraisal.JobDescriptionSections;
            /*            var query = from obj in appraisal.JobDescriptionSections.SingleOrDefault().JobDescriptionSectionItems
                                    from item in obj.JobDescriptionSectionTasks
                                    group item by {new item. }
                                    select item;*/

            return Json(new
            {
                Success = true,
                PartialViewHtml =
            RenderPartialViewToString("JobDescriptionSection",
                                      appraisal.JobDescriptionSections.SingleOrDefault())
            });
        }

        /*       public ActionResult SaveJobDescriptionSection(JobDescriptionSection jobDescriptionSection)
               {
                   Appraisal appraisal;
                   //var uow = new UnitOfWork();
                 //  var serAppraisal = new EntityService<Appraisal>(uow);
                   try
                   {
                       //JobDescriptionSectionHelper.CalulateJobDescriptionRate(jobDescriptionSection);

                       appraisal = Service.LoadById(jobDescriptionSection.Appraisal.Id);
                       jobDescriptionSection.SetTotalRate();
                       this.UpdateValueObject(jobDescriptionSection,
                                             appraisal.JobDescriptionSections.SingleOrDefault());  
                       for (var i = 0; i < jobDescriptionSection.JobDescriptionSectionItems.Count; i++)
                       {
                           for (int j = 0; j < jobDescriptionSection.JobDescriptionSectionItems[i].JobDescriptionSectionTasks.Count; j++)
                           {
                               this.UpdateValueObject(
            jobDescriptionSection.JobDescriptionSectionItems[i].JobDescriptionSectionTasks[j],
            appraisal.JobDescriptionSections.SingleOrDefault().JobDescriptionSectionItems[i].JobDescriptionSectionTasks[j]);
                               appraisal.JobDescriptionSections.SingleOrDefault().JobDescriptionSectionItems[i].
                                   JobDescriptionSectionTasks[j].JobDescriptionSectionItem =
                                   appraisal.JobDescriptionSections.SingleOrDefault().JobDescriptionSectionItems[i];
                           }


                       }
               

                       Service.Update(appraisal);
               
                

                   }
                   catch (Exception)
                   {
               
                       throw;
                   }
                   appraisal = Service.LoadById(jobDescriptionSection.Appraisal.Id);
                
                   return Json(new
                   {
                       Success = true,
                       PartialViewHtml =
                   RenderPartialViewToString("JobDescriptionSection",
                                             appraisal.JobDescriptionSections.SingleOrDefault())
                   });
               }*/


        //public ActionResult GetJobDescriptionKPIList(int sectionItemId, int taskId)
        //{

        //    ViewData["JDKPIList"] = JobDescriptionSectionHelper.GetKPI(sectionItemId, taskId);
        //    return Json(new
        //    {
        //        Success = true,
        //        PartialViewHtml =
        //    RenderPartialViewToString("JobDescriptionSectionItemKPI",
        //                              ViewData["JDKPIList"])
        //    });
        //}

        #endregion


        #region Project Section
        public ActionResult ProjectSection(int appraisalId)
        {
            return null;
        }
        #endregion

        private Appraisal IntializeAppraisal()
        {
            Guid _appraisalProcess_id = new Guid();
            const float _sharedWithPercent = 20;
            string _sectionName = "Values Section";
            const decimal _competencySectionWeight = 30;
            const decimal _jobDescriptionSectionWeight = 30;
            const decimal _objectiveSectionWeight = 30;
            const decimal _sectionWeight = 10;

            Appraisal _expected;
            Appraisal _actual;
            const int sectionWeight = 10;
            var postion = new Position() { JobDescription = new JobDescription() };

            #region Create JobDescription & competency

            var jobDescription = postion.JobDescription;

            // JobDescription jobDescription = new JobDescription();
            jobDescription.AddSpecification(new Specification());
            jobDescription.Specification[0].AddCompetency(new Competency()
            {
                Name = "Comp1",
                Level = new Level() { Name = "Level1" },
                Description = "Comp1Desc",
                Type = new CompetencyType() { Name = "Type1" },
                Weight = 30
            });

            jobDescription.Specification[0].AddCompetency(new Competency()
            {
                Name = "Comp2",
                Level = new Level() { Name = "Level2" },
                Description = "Comp2Desc",
                Type = new CompetencyType() { Name = "Type2" },
                Weight = 40
            });

            jobDescription.AddRole(new Role()
            {
                Name = "Role1",
                Summary = "summary",
                Weight = 30,
            });


            jobDescription.Roles[0].AddResponsibility(new Responsibility()
            {
                Description = "Res1",
                Weight = 20
            });

            jobDescription.Roles[0].Responsibilities[0].AddKpi(new ResponsibilityKpi()
            {
                Description = "Res1Kpi1",
                Value = 10
            });

            jobDescription.AddRole(new Role()
            {
                Name = "Role2",
                Summary = "summary2",
                Weight = 30,
            });


            jobDescription.Roles[1].AddResponsibility(new Responsibility()
            {
                Description = "Res2",
                Weight = 20
            });

            jobDescription.Roles[1].Responsibilities[0].AddKpi(new ResponsibilityKpi()
            {
                Description = "Res2Kpi2",
                Value = 20
            });
            #endregion

            #region Create Objective
            var objectiveList = new List<HRIS.Domain.Objectives.RootEntities.Objective>();
            var objective1 = new HRIS.Domain.Objectives.RootEntities.Objective()
            {
                Name = "objective1",
                Description = "desc1",
                Weight = 30,
                SharedWiths = new List<SharedWith>()
            };
            objective1.SharedWiths.Add(new SharedWith() { Percentage = _sharedWithPercent });

            var kpi1 = new ObjectiveKpi()
            {
                Value = 30,
                Description = "Desc1",
                Type = new ObjectiveKpiType() { Name = "Type1" },
            };

            objective1.AddKpi(kpi1);
            objectiveList.Add(objective1);
            #endregion

            #region Create Organizational Section
            var appraisalTemplate = new AppraisalTemplate();

            appraisalTemplate.AddSectionWeight(_sectionName, _sectionWeight);
            appraisalTemplate.AddSectionWeight(TemplateSectionName.Competency.ToString(), _competencySectionWeight);
            appraisalTemplate.AddSectionWeight(TemplateSectionName.JobDescription.ToString(), _jobDescriptionSectionWeight);
            appraisalTemplate.AddSectionWeight(TemplateSectionName.Objective.ToString(), _objectiveSectionWeight);

            var appraisalSectionList = new List<AppraisalSection>();
            var appraisalSection = new AppraisalSection() { Name = _sectionName };
            appraisalSection.AddSectionItem(new AppraisalSectionItem() { Name = "Section1", Weight = 10, Description = "desc1" });
            appraisalSection.Items[0].AddKpi(new AppraisalSectionItemKpi() { Description = "descKpi1", Value = 40 });
            appraisalSectionList.Add(appraisalSection);



            appraisalSection.AddSectionItem(new AppraisalSectionItem() { Name = "Section2", Weight = 10, Description = "desc2" });
            appraisalSection.Items[1].AddKpi(new AppraisalSectionItemKpi() { Description = "descKpi2", Value = 50 });
            #endregion


            /*            _expected = new Appraisal(_appraisalProcess_id);
            _expected.AddCompetencySection(new CompetencySection() { Weight = _competencySectionWeight, Name = "Competency Section" });
            _expected.CompetencySections[0].AddItem(new CompetencySectionItem() { Description = "Comp1Desc", Level = "Level1", Name = "Comp1", Type = "Type1", Weight = 30 });
            _expected.CompetencySections[0].AddItem(new CompetencySectionItem() { Description = "Comp2Desc", Level = "Level2", Name = "Comp2", Type = "Type2", Weight = 40 });


            _expected.AddJobDescriptionSection(new JobDescriptionSection() { Weight = _jobDescriptionSectionWeight });
            _expected.JobDescriptionSections[0].AddItem(new JobDescriptionSectionItem() { RoleName = "Role1", JobTask = "Res1" });
            _expected.JobDescriptionSections[0].AddItem(new JobDescriptionSectionItem() { RoleName = "Role2", JobTask = "Res2" });


            _expected.JobDescriptionSections[0].Items[0].AddKpi(new JobDescriptionSectionItemKpi() { Value = 10, Description = "Res1Kpi1" });
            _expected.JobDescriptionSections[0].Items[1].AddKpi(new JobDescriptionSectionItemKpi() { Value = 20, Description = "Res2Kpi2" });


            _expected.AddObjectiveSection(new ObjectiveSection() { Weight = _objectiveSectionWeight });
            _expected.ObjectiveSections[0].AddItems(new ObjectiveSectionItem() { Name = "objective1", Description = "desc1", Weight = 30, SharedWithPercentage = 0 });
            _expected.ObjectiveSections[0].Items[0].AddKpi(new ObjectiveSectionItemKpi() { Value = 30, Description = "Desc1" });



            _expected.AddOrganizationalSection(new OrganizationalSection() { Name = _sectionName, Weight = _sectionWeight });
            _expected.OrganizationalSections[0].AddItem(new OrganizationalSectionItem() { Name = "Section1", Weight = 10, Description = "desc1" });
            _expected.OrganizationalSections[0].Items[0].AddKpi(new OrganizationalSectionItemKpi() { Description = "descKpi1", Value = 40 });*/

            /*            _expected.OrganizationalSections[1].AddItem(new OrganizationalSectionItem() { Name = "Section2", Weight = 10, Description = "desc2" });
                        _expected.OrganizationalSections[1].Items[0].AddKpi(new OrganizationalSectionItemKpi() { Description = "descKpi2", Value = 50 });*/


            return AppraisalFactory.Create(_appraisalProcess_id, postion, appraisalTemplate, objectiveList, appraisalSectionList);

        }





    }
}