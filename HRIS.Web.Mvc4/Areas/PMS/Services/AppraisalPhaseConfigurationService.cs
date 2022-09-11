using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.Objectives.Enums;

using HRIS.Domain.OrganizationChart.Indexes;
using HRIS.Domain.PMS.Entities;
using  Project.Web.Mvc4.Areas.Objectives.Models;
using  Project.Web.Mvc4.Extensions;
using  Project.Web.Mvc4.Models;
using NHibernate.Linq;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;


namespace Project.Web.Mvc4.Areas.PMS.Services
{
    //public static class AppraisalPhaseConfigurationService
    //{
    //    public static int AppraisalPhaseConfigurationWorkflowMaxOperationNo()
    //    {
    //        int maxOperationNo = 0;

    //        if (!typeof(AppraisalPhaseConfigurationWorkflow).GetAll<AppraisalPhaseConfigurationWorkflow>().Any())
    //        {
    //            maxOperationNo = 1;
    //        }
    //        else
    //        {
    //            maxOperationNo = ServiceFactory.ORMService.All<AppraisalPhaseConfigurationWorkflow>().Max(x => x.OperationNo) + 1;
    //        }

    //        return maxOperationNo;
    //    }

    //    public static int TemplateAppraisalPositionsMaxOperationNo()
    //    {
    //        int maxOperationNo = 0;

    //        if (!typeof(TemplateAppraisalPositions).GetAll<TemplateAppraisalPositions>().Any())
    //        {
    //            maxOperationNo = 1;
    //        }
    //        else
    //        {
    //            maxOperationNo = ServiceFactory.ORMService.All<TemplateAppraisalPositions>().Max(x => x.OperationNo) + 1;
    //        }

    //        return maxOperationNo;
    //    }

    //    public static List<WorkflowTreeViewModel> TemplateAppraisalPositionsViewTree()
    //    {
    //        //var groupedData = ServiceFactory.ORMService.All<TemplateAppraisalPositions>().ToList().Where(x => x.AppraisalTemplate.Id == id).GroupBy(x => x.OperationNo).Select(x => new { x, Count = x.Count() }).ToList();//for grouping tree according to OperationNo.
    //        var groupedData = ServiceFactory.ORMService.All<TemplateAppraisalPositions>().ToList().GroupBy(x => x.OperationNo).Select(x => new { x, Count = x.Count() }).ToList();//for grouping tree according to OperationNo.

    //        var organizationalTree = new List<WorkflowTreeViewModel>();

    //        foreach (var item in groupedData)
    //        {
    //            var requiredLevels = item.x.Select(x => x.Position.JobDescription.JobTitle.Grade.OrganizationalLevel).Distinct().ToList();

    //            foreach (OrganizationalLevel organizationalLevel in requiredLevels)//organizationalLevels for each parent node
    //            {
    //                var treeItem = new WorkflowTreeViewModel();
    //                treeItem.Id = organizationalLevel.Id;
    //                treeItem.LevelNumber = (int)WorkflowApplyFlag.OrganizationalLevel;
    //                treeItem.Name = organizationalLevel.Name;

    //                if (item.x.ElementAt(0).WorkflowApplyFlag == HRIS.Domain.Objectives.Enums.WorkflowApplyFlag.OrganizationalLevel)//To stop adding children items.
    //                {
    //                    string appraisalTemplateName = item.x.ToList().Select(x => x.AppraisalTemplate.Name).Distinct().FirstOrDefault();
    //                    if (!string.IsNullOrEmpty(appraisalTemplateName))
    //                        treeItem.Name += (" ( " + appraisalTemplateName + " )");
    //                    organizationalTree.Add(treeItem);
    //                    continue;
    //                }

    //                treeItem.HasChildren = true;
    //                treeItem.Items = GetGradeByOrganizationalLevel(organizationalLevel.Id, item.x.ToList());
    //                organizationalTree.Add(treeItem);
    //            }
    //        }
    //        return organizationalTree;
    //    }

    //    public static List<WorkflowTreeViewModel> AppraisalPhaseConfigurationViewTree(int id)
    //    {
    //        var groupedData =
    //            ServiceFactory.ORMService.All<AppraisalPhaseConfigurationWorkflow>().ToList().Where(
    //                x => x.AppraisalPhaseConfiguration.Id == id).GroupBy(x => x.OperationNo).Select(
    //                    x => new { x, Count = x.Count() }).ToList(); //for grouping tree according to OperationNo.

    //        var organizationalTree = new List<WorkflowTreeViewModel>();

    //        foreach (var item in groupedData)
    //        {
    //            var requiredLevels =
    //                item.x.Where(x => x.AppraisalPhaseConfiguration.Id == id).Select(
    //                    x => x.FirstPosition.JobDescription.JobTitle.Grade.OrganizationalLevel).Distinct().ToList();

    //            foreach (OrganizationalLevel organizationalLevel in requiredLevels)
    //            //organizationalLevels for each parent node
    //            {
    //                var treeItem = new WorkflowTreeViewModel();
    //                treeItem.Id = organizationalLevel.Id;
    //                treeItem.LevelNumber = (int)WorkflowApplyFlag.OrganizationalLevel;
    //                treeItem.Name = organizationalLevel.Name;

    //                if (item.x.ElementAt(0).WorkflowApplyFlag ==
    //                    HRIS.Domain.Objectives.Enums.WorkflowApplyFlag.OrganizationalLevel)
    //                //To stop adding children items.
    //                {
    //                    string stepCount =
    //                        item.x.ToList().Select(x => x.StepCount.ToString()).Distinct().FirstOrDefault();
    //                    if (!string.IsNullOrEmpty(stepCount))
    //                        treeItem.Name += (" ( " + stepCount + " )");
    //                    organizationalTree.Add(treeItem);
    //                    continue;
    //                }

    //                treeItem.HasChildren = true;
    //                treeItem.Items = GetGradeByOrganizationalLevel(organizationalLevel.Id, id, item.x.ToList());
    //                organizationalTree.Add(treeItem);
    //            }
    //        }
    //        return organizationalTree;
    //    }

    //    private static List<WorkflowTreeViewModel> GetGradeByOrganizationalLevel(int organizationalLevelId, int id, List<AppraisalPhaseConfigurationWorkflow> appraisalPhaseConfigurationWorkflows)// method to get child nodes
    //    {
    //        var requiredGrades = appraisalPhaseConfigurationWorkflows.Where(x => x.AppraisalPhaseConfiguration.Id == id).Select(x => x.FirstPosition.JobDescription.JobTitle.Grade).Distinct().ToList();
    //        var gradeTree = new List<WorkflowTreeViewModel>();

    //        foreach (HRIS.Domain.Grades.RootEntities.Grade grade in requiredGrades)
    //        {
    //            var treeItem = new WorkflowTreeViewModel();
    //            if (grade.OrganizationalLevel.Id == organizationalLevelId)//may be
    //            {
    //                treeItem.Id = grade.Id;
    //                treeItem.Name = grade.Name;
    //                treeItem.LevelNumber = (int)WorkflowApplyFlag.Grade;

    //                if (appraisalPhaseConfigurationWorkflows.ElementAt(0).WorkflowApplyFlag == HRIS.Domain.Objectives.Enums.WorkflowApplyFlag.Grade)//To stop adding children items.
    //                {
    //                    string stepCount = appraisalPhaseConfigurationWorkflows.Select(x => x.StepCount.ToString()).Distinct().FirstOrDefault();
    //                    if (!string.IsNullOrEmpty(stepCount))
    //                        treeItem.Name += (" ( " + stepCount + " )");
    //                    gradeTree.Add(treeItem);
    //                    continue;
    //                }

    //                treeItem.HasChildren = true;
    //                treeItem.Items = GetJobTitleByGrade(grade.Id, id, appraisalPhaseConfigurationWorkflows);
    //                gradeTree.Add(treeItem);
    //            }
    //        }
    //        return gradeTree;
    //    }

    //    private static List<WorkflowTreeViewModel> GetJobTitleByGrade(int gradeId, int id, List<AppraisalPhaseConfigurationWorkflow> appraisalPhaseConfigurationWorkflows)// method to get child nodes
    //    {
    //        var jobTitleTree = new List<WorkflowTreeViewModel>();
    //        var requiredJobTitles = appraisalPhaseConfigurationWorkflows.Where(x => x.AppraisalPhaseConfiguration.Id == id).Select(x => x.FirstPosition.JobDescription.JobTitle).Distinct().ToList();

    //        foreach (JobTitle jobTitle in requiredJobTitles)
    //        {
    //            var treeItem = new WorkflowTreeViewModel();
    //            if (jobTitle.Grade.Id == gradeId)//may be
    //            {
    //                treeItem.Id = jobTitle.Id;
    //                treeItem.Name = jobTitle.Name;
    //                treeItem.LevelNumber = (int)WorkflowApplyFlag.JobTitle;

    //                if (appraisalPhaseConfigurationWorkflows.ElementAt(0).WorkflowApplyFlag == HRIS.Domain.Objectives.Enums.WorkflowApplyFlag.JobTitle)//To stop adding children items.
    //                {
    //                    string stepCount = appraisalPhaseConfigurationWorkflows.Select(x => x.StepCount.ToString()).Distinct().FirstOrDefault();
    //                    if (!string.IsNullOrEmpty(stepCount))
    //                        treeItem.Name += (" ( " + stepCount + " )");
    //                    jobTitleTree.Add(treeItem);
    //                    continue;
    //                }

    //                treeItem.HasChildren = true;
    //                treeItem.Items = GetJobDescriptionByJobTitle(jobTitle.Id, id, appraisalPhaseConfigurationWorkflows);
    //                jobTitleTree.Add(treeItem);
    //            }
    //        }
    //        return jobTitleTree;
    //    }

    //    private static List<WorkflowTreeViewModel> GetJobDescriptionByJobTitle(int jobTitleId, int id, List<AppraisalPhaseConfigurationWorkflow> appraisalPhaseConfigurationWorkflows)// method to get child nodes
    //    {
    //        var requiredJobDescriptions = appraisalPhaseConfigurationWorkflows.Where(x => x.AppraisalPhaseConfiguration.Id == id).Select(x => x.FirstPosition.JobDescription).Distinct().ToList();
    //        var jobDescriptionTree = new List<WorkflowTreeViewModel>();

    //        foreach (HRIS.Domain.JobDescription.RootEntities.JobDescription jobDescription in requiredJobDescriptions)
    //        {
    //            var treeItem = new WorkflowTreeViewModel();
    //            if (jobDescription.JobTitle.Id == jobTitleId)
    //            {
    //                treeItem.Id = jobDescription.Id;
    //                treeItem.Name = jobDescription.Name;
    //                treeItem.LevelNumber = (int)WorkflowApplyFlag.JobDescription;

    //                if (appraisalPhaseConfigurationWorkflows.ElementAt(0).WorkflowApplyFlag == HRIS.Domain.Objectives.Enums.WorkflowApplyFlag.JobDescription)//To stop adding children items.
    //                {
    //                    string stepCount = appraisalPhaseConfigurationWorkflows.Select(x => x.StepCount.ToString()).Distinct().FirstOrDefault();
    //                    if (!string.IsNullOrEmpty(stepCount))
    //                        treeItem.Name += (" ( " + stepCount + " )");
    //                    jobDescriptionTree.Add(treeItem);
    //                    continue;
    //                }

    //                treeItem.HasChildren = true;
    //                treeItem.Items = GetPositionByJobDescription(jobDescription.Id, id, appraisalPhaseConfigurationWorkflows);
    //                jobDescriptionTree.Add(treeItem);
    //            }
    //        }
    //        return jobDescriptionTree;
    //    }

    //    private static List<WorkflowTreeViewModel> GetPositionByJobDescription(int jobDescriptionId, int id, List<AppraisalPhaseConfigurationWorkflow> groupedPhaseConfigurationWorkflows)// method to get child nodes
    //    {
    //        var requiredPositions = groupedPhaseConfigurationWorkflows.Where(x => x.AppraisalPhaseConfiguration.Id == id).Select(x => x.FirstPosition).Distinct().ToList();
    //        var positionTree = new List<WorkflowTreeViewModel>();

    //        foreach (Position position in requiredPositions)
    //        {
    //            var treeItem = new WorkflowTreeViewModel();
    //            if (position.JobDescription.Id == jobDescriptionId)
    //            {
    //                treeItem.Id = position.Id;
    //                treeItem.LevelNumber = (int)WorkflowApplyFlag.Position;
    //                treeItem.Name = position.NameForDropdown;
    //                string stepCount = groupedPhaseConfigurationWorkflows.Select(x => x.StepCount.ToString()).Distinct().FirstOrDefault();
    //                if (!string.IsNullOrEmpty(stepCount))
    //                    treeItem.Name += (" ( " + stepCount + " )");

    //                positionTree.Add(treeItem);
    //            }
    //        }
    //        return positionTree;
    //    }
        
    //    #region Assistant Methods

    //    private static List<WorkflowTreeViewModel> GetGradeByOrganizationalLevel(int organizationalLevelId, List<TemplateAppraisalPositions> templateAppraisalPositions)// method to get child nodes
    //    {
    //        var requiredGrades = templateAppraisalPositions.Select(x => x.Position.JobDescription.JobTitle.Grade).Distinct().ToList();
    //        var gradeTree = new List<WorkflowTreeViewModel>();

    //        foreach (HRIS.Domain.Grades.RootEntities.Grade grade in requiredGrades)
    //        {
    //            var treeItem = new WorkflowTreeViewModel();
    //            if (grade.OrganizationalLevel.Id == organizationalLevelId)//may be
    //            {
    //                treeItem.Id = grade.Id;
    //                treeItem.Name = grade.Name;
    //                treeItem.LevelNumber = (int)WorkflowApplyFlag.Grade;

    //                if (templateAppraisalPositions.ElementAt(0).WorkflowApplyFlag == HRIS.Domain.Objectives.Enums.WorkflowApplyFlag.Grade)//To stop adding children items.
    //                {
    //                    string appraisalTemplateName = templateAppraisalPositions.Select(x => x.AppraisalTemplate.Name).Distinct().FirstOrDefault();
    //                    if (!string.IsNullOrEmpty(appraisalTemplateName))
    //                        treeItem.Name += (" ( " + appraisalTemplateName + " )");
    //                    gradeTree.Add(treeItem);
    //                    continue;
    //                }

    //                treeItem.HasChildren = true;
    //                treeItem.Items = GetJobTitleByGrade(grade.Id, templateAppraisalPositions);
    //                gradeTree.Add(treeItem);
    //            }
    //        }
    //        return gradeTree;
    //    }

    //    private static List<WorkflowTreeViewModel> GetJobTitleByGrade(int gradeId, List<TemplateAppraisalPositions> templateAppraisalPositions)// method to get child nodes
    //    {
    //        var jobTitleTree = new List<WorkflowTreeViewModel>();
    //        var requiredJobTitles = templateAppraisalPositions.Select(x => x.Position.JobDescription.JobTitle).Distinct().ToList();

    //        foreach (JobTitle jobTitle in requiredJobTitles)
    //        {
    //            var treeItem = new WorkflowTreeViewModel();
    //            if (jobTitle.Grade.Id == gradeId)//may be
    //            {
    //                treeItem.Id = jobTitle.Id;
    //                treeItem.Name = jobTitle.Name;
    //                treeItem.LevelNumber = (int)WorkflowApplyFlag.JobTitle;

    //                if (templateAppraisalPositions.ElementAt(0).WorkflowApplyFlag == HRIS.Domain.Objectives.Enums.WorkflowApplyFlag.JobTitle)//To stop adding children items.
    //                {
    //                    string appraisalTemplateName = templateAppraisalPositions.Select(x => x.AppraisalTemplate.Name).Distinct().FirstOrDefault();
    //                    if (!string.IsNullOrEmpty(appraisalTemplateName))
    //                        treeItem.Name += (" ( " + appraisalTemplateName + " )");
    //                    jobTitleTree.Add(treeItem);
    //                    continue;
    //                }

    //                treeItem.HasChildren = true;
    //                treeItem.Items = GetJobDescriptionByJobTitle(jobTitle.Id, templateAppraisalPositions);
    //                jobTitleTree.Add(treeItem);
    //            }
    //        }
    //        return jobTitleTree;
    //    }

    //    private static List<WorkflowTreeViewModel> GetJobDescriptionByJobTitle(int jobTitleId, List<TemplateAppraisalPositions> templateAppraisalPositions)// method to get child nodes
    //    {
    //        var requiredJobDescriptions = templateAppraisalPositions.Select(x => x.Position.JobDescription).Distinct().ToList();
    //        var jobDescriptionTree = new List<WorkflowTreeViewModel>();

    //        foreach (HRIS.Domain.JobDescription.RootEntities.JobDescription jobDescription in requiredJobDescriptions)
    //        {
    //            var treeItem = new WorkflowTreeViewModel();
    //            if (jobDescription.JobTitle.Id == jobTitleId)
    //            {
    //                treeItem.Id = jobDescription.Id;
    //                treeItem.Name = jobDescription.Name;
    //                treeItem.LevelNumber = (int)WorkflowApplyFlag.JobDescription;

    //                if (templateAppraisalPositions.ElementAt(0).WorkflowApplyFlag == HRIS.Domain.Objectives.Enums.WorkflowApplyFlag.JobDescription)//To stop adding children items.
    //                {
    //                    string appraisalTemplateName = templateAppraisalPositions.Select(x => x.AppraisalTemplate.Name).Distinct().FirstOrDefault();
    //                    if (!string.IsNullOrEmpty(appraisalTemplateName))
    //                        treeItem.Name += (" ( " + appraisalTemplateName + " )");
    //                    jobDescriptionTree.Add(treeItem);
    //                    continue;
    //                }

    //                treeItem.HasChildren = true;
    //                treeItem.Items = GetPositionByJobDescription(jobDescription.Id, templateAppraisalPositions);
    //                jobDescriptionTree.Add(treeItem);
    //            }
    //        }
    //        return jobDescriptionTree;
    //    }

    //    private static List<WorkflowTreeViewModel> GetPositionByJobDescription(int jobDescriptionId, List<TemplateAppraisalPositions> templateAppraisalPositions)// method to get child nodes
    //    {
    //        var requiredPositions = templateAppraisalPositions.Select(x => x.Position).Distinct().ToList();
    //        var positionTree = new List<WorkflowTreeViewModel>();

    //        foreach (Position position in requiredPositions)
    //        {
    //            var treeItem = new WorkflowTreeViewModel();
    //            if (position.JobDescription.Id == jobDescriptionId)
    //            {
    //                treeItem.Id = position.Id;
    //                treeItem.LevelNumber = (int)WorkflowApplyFlag.Position;
    //                treeItem.Name = position.NameForDropdown;
    //                string appraisalTemplateName = templateAppraisalPositions.Select(x => x.AppraisalTemplate.Name).Distinct().FirstOrDefault();
    //                if (!string.IsNullOrEmpty(appraisalTemplateName))
    //                    treeItem.Name += (" ( " + appraisalTemplateName + " )");

    //                positionTree.Add(treeItem);
    //            }
    //        }
    //        return positionTree;
    //    }

    //    public static int DeleteTreeNode(int nodeId, int levelNumber)
    //    {
    //        int deletedRowsNumber = 0;
    //        List<TemplateAppraisalPositions> templateAppraisalPositions = new List<TemplateAppraisalPositions>();
    //        switch (levelNumber)
    //        {
    //            case 0://Org Level
    //                templateAppraisalPositions = ServiceFactory.ORMService.All<TemplateAppraisalPositions>().Where(x => x.Position.JobDescription.JobTitle.Grade.OrganizationalLevel.Id == nodeId ).ToList();
    //                deletedRowsNumber = DeleteWorkflow(templateAppraisalPositions);
    //                break;
    //            case 1://Grade
    //                templateAppraisalPositions = ServiceFactory.ORMService.All<TemplateAppraisalPositions>().Where(x => x.Position.JobDescription.JobTitle.Grade.Id == nodeId ).ToList();
    //                deletedRowsNumber = DeleteWorkflow(templateAppraisalPositions);
    //                break;
    //            case 2://J.T
    //                templateAppraisalPositions = ServiceFactory.ORMService.All<TemplateAppraisalPositions>().Where(x => x.Position.JobDescription.JobTitle.Id == nodeId ).ToList();
    //                deletedRowsNumber = DeleteWorkflow(templateAppraisalPositions);
    //                break;
    //            case 3://J.D
    //                templateAppraisalPositions = ServiceFactory.ORMService.All<TemplateAppraisalPositions>().Where(x => x.Position.JobDescription.Id == nodeId ).ToList();
    //                deletedRowsNumber = DeleteWorkflow(templateAppraisalPositions);
    //                break;
    //            case 4://Position
    //                templateAppraisalPositions = ServiceFactory.ORMService.All<TemplateAppraisalPositions>().Where(x => x.Position.Id == nodeId).ToList();
    //                deletedRowsNumber = DeleteWorkflow(templateAppraisalPositions);
    //                break;
    //        }
    //        return deletedRowsNumber;
    //    }

    //    private static int DeleteWorkflow(List<TemplateAppraisalPositions> templateAppraisalPositions)
    //    {
    //        int deletedRowsNumber = 0;
    //        foreach (var item in templateAppraisalPositions)
    //        {
    //            ServiceFactory.ORMService.Delete<TemplateAppraisalPositions>(item);
    //            deletedRowsNumber++;
    //        }
    //        return deletedRowsNumber;
    //    }//Should to handle (delete internal node belonged to applied master node).

    //    #endregion
    //}
}