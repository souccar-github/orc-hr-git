#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.JobDesc.Entities;
using HRIS.Domain.OrgChart.Entities;
using HRIS.Domain.OrgChart.Indexes;
using HRIS.Domain.OrgChart.ValueObjects;
using Service;
using UI.Extensions;
using UI.Helpers.Cache;

#endregion

namespace UI.Areas.OrganizationChart.Helpers
{
    public class DropDownListHelpers
    {
        #region Entities

        #endregion

        #region Indexes

        public static SelectList ListOfJobGroups
        {
            get
            {
                List<JobGroup> jobGroups =
                    CacheProvider.Get(OrganizationChartCacheKeys.JobGroup.ToString(),
                                      () => new EntityService<JobGroup>().GetList());

                return jobGroups.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfInsuranceCompanies
        {
            get
            {
                List<InsuranceCompany> insuranceCompany =
                    CacheProvider.Get(OrganizationChartCacheKeys.InsuranceCompany.ToString(),
                                      () => new EntityService<InsuranceCompany>().GetList());

                return insuranceCompany.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfInsuranceTypes
        {
            get
            {
                List<InsuranceType> insuranceType =
                    CacheProvider.Get(OrganizationChartCacheKeys.InsuranceType.ToString(),
                                      () => new EntityService<InsuranceType>().GetList());

                return insuranceType.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfCashBenefitTypes
        {
            get
            {
                List<CashBenefitType> cashBenefitType =
                    CacheProvider.Get(OrganizationChartCacheKeys.CashBenefitsType.ToString(),
                                      () => new EntityService<CashBenefitType>().GetList());

                return cashBenefitType.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfNonCashBenefitTypes
        {
            get
            {
                List<NoneCashBenefitType> nonCashBenefitType =
                    CacheProvider.Get(OrganizationChartCacheKeys.NonCashBenefitType.ToString(),
                                      () => new EntityService<NoneCashBenefitType>().GetList());

                return nonCashBenefitType.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfCashDeductionTypes
        {
            get
            {
                List<CashDeductionType> cashDeductionType =
                    CacheProvider.Get(OrganizationChartCacheKeys.CashDeductionType.ToString(),
                                      () => new EntityService<CashDeductionType>().GetList());

                return cashDeductionType.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfTimeIntervals
        {
            get
            {
                List<TimeInterval> timeInterval =
                    CacheProvider.Get(OrganizationChartCacheKeys.TimeInterval.ToString(),
                                      () => new EntityService<TimeInterval>().GetList());

                return timeInterval.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        //TODO refactor this method
        public static SelectList ListOfContractType
        {
            get
            {
                List<ContractType> contractType =
                    CacheProvider.Get(OrganizationChartCacheKeys.ContractType.ToString(),
                                      () => new EntityService<ContractType>().GetList());

                return contractType.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        //TODO refactor this method
        public static SelectList ListOfEmployeeType
        {
            get
            {
                List<EmployeeType> employeeType =
                    CacheProvider.Get(OrganizationChartCacheKeys.EmployeeType.ToString(),
                                      () => new EntityService<EmployeeType>().GetList());

                return employeeType.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfAssetStatus
        {
            get
            {
                List<AssetStatus> assetStatus =
                    CacheProvider.Get(OrganizationChartCacheKeys.AssetStatus.ToString(),
                                      () => new EntityService<AssetStatus>().GetList());

                return assetStatus.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfAssetType
        {
            get
            {
                List<AssetType> assetStatus =
                    CacheProvider.Get(OrganizationChartCacheKeys.AssetType.ToString(),
                                      () => new EntityService<AssetType>().GetList());

                return assetStatus.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfCurrencyType
        {
            get
            {
                List<CurrencyType> currencyType =
                    CacheProvider.Get(OrganizationChartCacheKeys.CurrencyType.ToString(),
                                      () => new EntityService<CurrencyType>().GetList());

                return currencyType.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfCostCenter
        {
            get
            {
                List<CostCenter> costCenter =
                    CacheProvider.Get(OrganizationChartCacheKeys.CostCenter.ToString(),
                                      () => new EntityService<CostCenter>().GetList());

                return costCenter.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfJobTitle
        {
            get
            {
                List<JobTitle> jobTitle =
                     CacheProvider.Get(OrganizationChartCacheKeys.JobTitle.ToString(),
                                       () => new EntityService<JobTitle>().GetList());

                return jobTitle.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfPositionJobTitles()
        {
            IQueryable<JobTitle> result = new EntityService<JobTitle>().GetAll();

            //IQueryable<Position> position = new EntityService<Position>().GetAll();
            //result = result.Where(x => !position.Any(p => (p.JobTitle.Id == x.Id && p.Id != positionId)));

            return result.ToList().SelectFromList(x => x.Id.ToString(), y => y.Name);

        }

        public static SelectList ListOfJobDescJobTitles(int jobDescriptionId)
        {
            IQueryable<JobTitle> result = new EntityService<JobTitle>().GetAll();

            IQueryable<JobDescription> jobDescription = new EntityService<JobDescription>().GetAll();
            result = result.Where(x => !jobDescription.Any(p => (p.JobTitle.Id == x.Id && p.Id != jobDescriptionId)));

            return result.ToList().SelectFromList(x => x.Id.ToString(), y => y.Name);
        }

        //public static SelectList ListOfJobRole
        //{
        //    get
        //    {
        //        List<JobRole> jobRole =
        //            CacheProvider.Get(OrganizationChartCacheKeys.JobRole.ToString(),
        //                              () => new EntityService<JobRole>().GetList());

        //        return jobRole.SelectFromList(x => x.Id.ToString(), y => y.Name);
        //    }
        //}

        public static SelectList ListOfPositionType
        {
            get
            {
                List<PositionType> positionType =
                    CacheProvider.Get(OrganizationChartCacheKeys.PositionType.ToString(),
                                      () => new EntityService<PositionType>().GetList());

                return positionType.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfPositionLevel
        {
            get
            {
                List<PositionLevel> positionLevel =
                    CacheProvider.Get(OrganizationChartCacheKeys.PositionLevel.ToString(),
                                      () => new EntityService<PositionLevel>().GetList());

                return positionLevel.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfOrganizationalLevel
        {
            get
            {
                var list =
                    CacheProvider.Get(OrganizationChartCacheKeys.OrganizationalLevel.ToString(),
                                      () => new EntityService<OrganizationalLevel>().GetAll().OrderBy(x=>x.Order).ToList());

                return list.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfGradeName
        {
            get
            {
                var list =
                    CacheProvider.Get(OrganizationChartCacheKeys.GradeName.ToString(),
                                      () => new EntityService<GradeName>().GetAll().OrderBy(x => x.Order).ToList());

                return list.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }
        public static SelectList ListOfGradeStepName
        {
            get
            {
                var list =
                    CacheProvider.Get(OrganizationChartCacheKeys.GradeName.ToString(),
                                      () => new EntityService<GradeStepName>().GetAll().OrderBy(x => x.Order).ToList());

                return list.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        //public static SelectList ListOfPositionStatus
        //{
        //    get
        //    {
        //        List<PositionStatus> positionStatus =
        //            CacheProvider.Get(OrganizationChartCacheKeys.PositionStatus.ToString(),
        //                              () => new EntityService<PositionStatus>().GetList());

        //        return positionStatus.SelectFromList(x => x.Id.ToString(), y => y.Name);
        //    }
        //}

        public static SelectList ListOfNodeTypes
        {
            get
            {
                List<NodeType> nodeType =
                    CacheProvider.Get(OrganizationChartCacheKeys.NodeType.ToString(),
                                      () => new EntityService<NodeType>().GetAll().OrderBy(x=>x.NodeOrder).ToList());

                return nodeType.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfGradeSystems
        {
            get
            {
                List<Grade> grade = new EntityService<Grade>().GetAll().OrderBy(x => x.Name).ToList();

                return grade.SelectFromList(x => x.Id.ToString(), y => y.ToString());
            }
        }

        public static SelectList ListOfGrades
        {
            get
            {
                List<GradeName> grade = new EntityService<GradeName>().GetAll().OrderBy(x => x.Name).ToList();

                return grade.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfPositions
        {
            get
            {
                List<Position> position = new EntityService<Position>().GetList();

                return position.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfPositionsOfParentNode(int nodeId)
        {
            var nodeService = new EntityService<Node>();
            var node = nodeService.LoadById(nodeId);
            if (node==null)
            {
                throw new ApplicationException("node must be define");
            }
            List<Position> positions = node.Positions.ToList();
            if (node.Parent!=null)
            {
                if (node.Parent.PositionNodes!=null)
                    positions.AddRange(node.Parent.Positions);
            }
            return positions.SelectFromList(x => x.Id.ToString(), y => y.Name );
        }

        public static SelectList ListOfDisabilityStatus
        {
            get
            {
                List<DisabilityStatus> disabilityStatus =
                    CacheProvider.Get(OrganizationChartCacheKeys.DisabilityStatus.ToString(),
                                      () => new EntityService<DisabilityStatus>().GetList());

                return disabilityStatus.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfGroupSalaryScale
        {
            get
            {
                List<GroupSalaryScale> groupSalaryScale =
                    CacheProvider.Get(OrganizationChartCacheKeys.GroupSalaryScale.ToString(),
                                      () => new EntityService<GroupSalaryScale>().GetList());

                return groupSalaryScale.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
        }

        public static SelectList ListOfNodeTypesOrders(int parentId)
        {
            List<NodeType> nodeType =
                CacheProvider.Get(OrganizationChartCacheKeys.NodeType.ToString(),
                                  () => new EntityService<NodeType>().GetList());

            nodeType = nodeType.FindAll(x => x.Id > parentId);

            return nodeType.SelectFromList(x => x.Id.ToString(), y => y.Name);
        }

        #endregion

        #region Services

        #endregion
    }
}