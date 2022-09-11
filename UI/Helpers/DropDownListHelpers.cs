#region

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.Objectives.Enums;
using HRIS.Domain.Objectives.Indexes;
using HRIS.Domain.Objectives.Repositories;
using HRIS.Domain.Objectives.RootEntities;
using HRIS.Domain.Services;
using HRIS.Domain.OrgChart.ValueObjects;
using HRIS.Domain.Personnel.Entities;
using Service;
using Service.OrgChart;
using Souccar.Domain.DomainModel;
using Telerik.Web.Mvc.UI;
using UI.Areas.OrganizationChart.Helpers;
using UI.Extensions;
using UI.Helpers.Cache;
using Souccar.Core.Extensions;

#endregion

namespace UI.Helpers
{
    public class DropDownListHelpers
    {
        public static SelectList ListOfPositions
        {
            get
            {
                List<Position> node = new EntityService<Position>().GetList();
                CacheProvider.Get(OrganizationChartCacheKeys.Position.ToString(),
                                  () => new EntityService<Position>().GetList());

                return node.SelectFromList(x => x.Id.ToString(),
                                           y =>
                                               string.Format(Resources.Shared.Messages.DropDownListHelpers.Position, y.Code, y.Node.Name, y.Node.Type.Name));
                //"Position Code: " + y.Code + ", Inside : " + y.Node.Name + " " +
                //y.Node.Type.Name);
            }
        }

        #region Organization Nodes

        public static SelectList ListOfSelectedNodePosition(int nodeId)
        {
            var positions = new List<Position>();

            if (nodeId != 0)
            {
                positions =
                    CacheProvider.GetFromDataSource(OrganizationChartCacheKeys.SelectedNodePositions.ToString(),
                                                    () =>
                                                    new EntityService<Node>().GetById(nodeId).Positions.ToList());
                //positions.AddRange(new EntityService<Node>().GetById(id).Positions.ToList());

                CacheProvider.GetFromDataSource(OrganizationChartCacheKeys.SelectedNodePositions.ToString(),
                                                () => positions);
            }

            else
            {
                positions = CacheProvider.Get(OrganizationChartCacheKeys.SelectedNodePositions.ToString(),
                                              () =>
                                              new List<Position>());
            }

            return positions.SelectFromList(x => x.Id.ToString(),
                                            y => Resources.Shared.Messages.DropDownListHelpers.PositionJobTitle + y.JobTitle.Name);
        }

        /*        public static SelectList ListOfNodes()
                {
                    List<Node> nodes = new EntityService<Node>().GetList();

                    return nodes.SelectFromList(x => x.Id.ToString(), y => y.Name);
                }*/

        public static SelectList ListOfNodes(int id = 0)
        {
            if (id == 0)
            {

                List<Node> nodes = new EntityService<Node>().GetList();

                return nodes.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }
            else
            {

                var temp = new EntityService<Node>().GetById(id);
                var nodes = new List<Node> { temp };
                return nodes.SelectFromList(x => x.Id.ToString(), y => y.Name);
            }


        }

        #endregion

        #region Employees

        public static SelectList ListOfSelectedPositionEmployees(int id)
        {
            var employees = new List<Employee>();

            if (id > 0)
            {
                //TODO  ListOfSelectedPositionEmployees -- Need Test
                employees.AddRange(PositionHelpers.GetEmployees(id));

                CacheProvider.GetFromDataSource(CacheKeys.SelectedPositionEmployees.ToString(),
                                                () => employees);
            }
            else if (id == 0)
            {
                employees = CacheProvider.Get(CacheKeys.SelectedPositionEmployees.ToString(),
                                              () =>
                                              new List<Employee>());
            }
            else if (id == -1)
            {
                CacheProvider.GetFromDataSource(CacheKeys.SelectedPositionEmployees.ToString(),
                                                () => new List<Employee>());
            }

            return employees.SelectFromList(x => x.Id.ToString(), y => y.FirstName + " " + y.LastName);
        }

        #endregion

        #region Assign Employee To Position

        //public static SelectList ListOfDirectPositionManager(int positionId)
        //{
        //    Position positionItem = new EntityService<Position>().GetById(positionId);

        //    IQueryable<EmployeeToPosition> assignedEmployeeToPosition = new EntityService<EmployeeToPosition>().GetAll().Where(
        //        e => e.Position.Id == positionItem.DirectReportingTo.Id);

        //    IQueryable<Employee> employees = assignedEmployeeToPosition.Select(x => x.Employee).Distinct();

        //    return employees.ToList().SelectFromList(x => x.Id.ToString(), y => (y.FirstName + " " + y.LastName));

        //    //List<SelectListItem> directManagers = (from item in employees
        //    //                                       select
        //    //                                           new SelectListItem()
        //    //                                               {
        //    //                                                   Value = item.Id.ToString(),
        //    //                                                   Text = item.FirstName + " " + item.LastName
        //    //                                               }).ToList();
        //}

        //TODO Apply to all Indexes helper
        //        public static SelectList ListOfIndexes<T>() where T : IndexEntity
        //        {
        //            List<T> types = CacheProvider.Get(typeof(T).Name, () => new EntityService<T>().GetList());
        //            return types.SelectFromList(x => x.Id.ToString(), y => y.Name);
        //        }

        #endregion
    }
}