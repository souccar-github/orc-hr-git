//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using HRIS.Domain.JobDescription.Entities;
//using HRIS.Domain.PMS.Entities;
//using HRIS.Domain.PMS.Entities.Competency;
//using HRIS.Domain.PMS.Entities.JobDescription;
//using HRIS.Domain.PMS.Entities.objective;
//using HRIS.Domain.PMS.Entities.Organizational;
//using HRIS.Domain.PMS.RootEntities;
//using HRIS.Domain.Training.Entities;
//using HRIS.Domain.Training.Indexes;
//using HRIS.Domain.Training.RootEntities;
//using HRIS.Validation.MessageKeys;
//using  Project.Web.Mvc4.Helpers.DomainExtensions;
//using  Project.Web.Mvc4.Factories;
//using  Project.Web.Mvc4.Models.Appraisal;
//using Souccar.Domain.DomainModel;
//using Souccar.Domain.Workflow.Entities;
//using Souccar.Domain.Workflow.Enums;
//using Souccar.Domain.Workflow.RootEntities;
//using Souccar.Infrastructure.Core;
//using HRIS.Domain.Personnel.RootEntities;
//using Souccar.Core.Extensions;
//using Souccar.Infrastructure.Extenstions;

//using  Project.Web.Mvc4.Extensions;

//namespace Project.Web.Mvc4.Controllers
//{
//    public class AppraisalController : Controller
//    {
//        public ActionResult test()
//        {
//            var appraisal = new AppraisalViewModel()
//                                {
//                                    IsHiddenCompetanceSection = false,
//                                    IsHiddenJobDescriptionSection = false,
//                                    IsHiddenObjectiveSection = false,
//                                    JobDescriptionSection = getJDSection(),
//                                    CompetenceSection = getCompetenceSection(),
//                                    ObjectiveSection = getObjectiveSection(),
//                                    CustomSections = new List<CustomSectionViewModel>()
//                                                         {
//                                                             new CustomSectionViewModel()
//                                                                 {
//                                                                     Name = "Custom section1",
//                                                                     Id = 1,
//                                                                     Description = "hahaha",
//                                                                     AppraisalItems = new List<AppraisalSectionItemViewModel>()
//                                                                          {
//                                                                              new AppraisalSectionItemViewModel()
//                                                                                  {
//                                                                                      Id = 1,
//                                                                                      Name = "Responsibility1 FOR Custom section",
//                                                                                      Rate = 1,
//                                                                                      Weight = 1
//                                                                                  },
//                                                                              new AppraisalSectionItemViewModel()
//                                                                                  {
//                                                                                      Id = 2,
//                                                                                      Name = "Responsibility2 FOR Custom section",
//                                                                                      Rate = 2,
//                                                                                      Weight = 2
//                                                                                  }
//                                                                          }
//                                                                     //AppraisalItems =
//                                                                     //    {
//                                                                     //        new AppraisalSectionItemViewModel()
//                                                                     //            {
//                                                                     //                Id = 1,
//                                                                     //                Name = "a",
//                                                                     //                Rate = 1,
//                                                                     //                Weight = 1
//                                                                     //            },
//                                                                     //        new AppraisalSectionItemViewModel()
//                                                                     //            {
//                                                                     //                Id = 2,
//                                                                     //                Name = "b",
//                                                                     //                Rate = 2,
//                                                                     //                Weight = 2
//                                                                     //            }
//                                                                     //    }
//                                                                 },
//                                                             new CustomSectionViewModel()
//                                                                 {
//                                                                     Name = "Custom section2 FOR Custom section",
//                                                                     Id = 2,
//                                                                     Description = "hahaha FOR Custom section"
//                                                                 }
//                                                         }
//                                };
//            return Json(appraisal, JsonRequestBehavior.AllowGet);
//        }

//        public JobDescriptionSectionViewModel getJDSection()
//        {
//            var jd = new JobDescriptionSectionViewModel()
//                         {
//                             Id = 1,
//                             Name = "Name1",
//                             Description = "Description1",
//                             JobTitle = "JobTitle1",
//                             SectionWeight = 1,
//                             Roles = new List<AppraisalRoleViewModel>()
//                                         {
//                                             new AppraisalRoleViewModel()
//                                                 {
//                                                     Id = 1,
//                                                     Name = "Role1 FOR JD",
//                                                     Description = "Description1 FOR JD",
//                                                     Weight = 1,
//                                                     AppraisalItems = new List<AppraisalResponsibilityViewModel>()
//                                                                          {
//                                                                              new AppraisalResponsibilityViewModel()
//                                                                                  {
//                                                                                      Id = 1,
//                                                                                      Name = "Responsibility1 FOR JD",
//                                                                                      Rate = 1,
//                                                                                      Weight = 1
//                                                                                  },
//                                                                              new AppraisalResponsibilityViewModel()
//                                                                                  {
//                                                                                      Id = 2,
//                                                                                      Name = "Responsibility2 FOR JD",
//                                                                                      Rate = 2,
//                                                                                      Weight = 2
//                                                                                  }
//                                                                          }
//                                                 },
//                                             new AppraisalRoleViewModel()
//                                                 {
//                                                     Id = 2,
//                                                     Name = "Role2 FOR JD",
//                                                     Description = "Description2 FOR JD",
//                                                     Weight = 2,
//                                                     AppraisalItems = new List<AppraisalResponsibilityViewModel>()
//                                                                          {
//                                                                              new AppraisalResponsibilityViewModel()
//                                                                                  {
//                                                                                      Id = 3,
//                                                                                      Name = "Responsibility3 FOR JD",
//                                                                                      Rate = 3,
//                                                                                      Weight = 3
//                                                                                  },
//                                                                              new AppraisalResponsibilityViewModel()
//                                                                                  {
//                                                                                      Id = 4,
//                                                                                      Name = "Responsibility4 FOR JD",
//                                                                                      Rate = 4,
//                                                                                      Weight = 4
//                                                                                  }
//                                                                          }
//                                                 }
//                                         }
//                         };
//            return jd;
//        }

//        public CompetenceSectionViewModel getCompetenceSection()
//        {
//            var jd = new CompetenceSectionViewModel()
//                         {
//                             Id = 1,
//                             JobDescriptionName = "JobDescriptionName1 FOR Competence",
//                             JobDescriptionDescription = "JobDescriptionDescription1 FOR Competence",
//                             JobTitle = "JobTitle1",
//                             SectionWeight = 1,
//                             AppraisalItems = new List<AppraisalCompetenceViewModel>()
//                                                  {
//                                                      new AppraisalCompetenceViewModel()
//                                                          {
//                                                              Id = 1,
//                                                              Name = "AppraisalItems1 FOR Competence",
//                                                              Rate = 1,
//                                                              Weight = 1
//                                                          },
//                                                      new AppraisalCompetenceViewModel()
//                                                          {
//                                                              Id = 2,
//                                                              Name = "AppraisalItems2 FOR Competence",
//                                                              Rate = 2,
//                                                              Weight = 2
//                                                          },
//                                                      new AppraisalCompetenceViewModel()
//                                                          {
//                                                              Id = 3,
//                                                              Name = "AppraisalItems3 FOR Competence",
//                                                              Rate = 3,
//                                                              Weight = 3
//                                                          }
//                                                  }
//                         };
//            return jd;
//        }

//        public ObjectiveSectionViewModel getObjectiveSection()
//        {
//            var jd = new ObjectiveSectionViewModel()
//            {
//                Description = "Description1 FOR Objective",
//                SectionWeight = 1,
//                AppraisalItems = new List<AppraisalObjectiveViewModel>()
//                {
//                    new AppraisalObjectiveViewModel()
//                        {
//                            Id = 1,
//                            Description="Description1 FOR Objective",
//                            Name = "AppraisalItems1 FOR Objective",
//                            Rate = 1,
//                            Weight = 1
//                        },
//                    new AppraisalObjectiveViewModel()
//                        {
//                            Id = 2,
//                            Description="Description2 FOR Objective",
//                            Name = "AppraisalItems2 FOR Objective",
//                            Rate = 2,
//                            Weight = 2
//                        },
//                    new AppraisalObjectiveViewModel()
//                        {
//                            Id = 3,
//                            Description="Description3 FOR Objective",
//                            Name = "AppraisalItems3 FOR Objective",
//                            Rate = 3,
//                            Weight = 3
//                        }
//                }
//            };
//            return jd;
//        }
//    }

//}
