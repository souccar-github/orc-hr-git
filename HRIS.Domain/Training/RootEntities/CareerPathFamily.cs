using FluentNHibernate.Data;
using HRIS.Domain.OrgChart.RootEntities;
using Souccar.Core.CustomAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity = Souccar.Domain.DomainModel.Entity;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Training.Entities;
using HRIS.Domain.Global.Constant;

namespace HRIS.Domain.Training.RootEntities
{
    [Module(ModulesNames.Training)]
    [Order(5)]
    public class CareerPathFamily : Entity, IAggregateRoot
    {
        public CareerPathFamily()
        {
            CareerPathNodes = new List<CareerPathNode>();
            CreationDate = DateTime.Today;
        }

        /// <summary>
        /// اسم العائلة الوظيفية
        /// </summary>
        [UserInterfaceParameter(Order = 1)]
        public virtual string Name { get; set; }
        /// <summary>
        /// تاريخ الإنشاء
        /// </summary>
        [UserInterfaceParameter(Order = 2)]
        public virtual DateTime CreationDate { get; set; }
        /// <summary>
        /// العقدة- الفرع
        /// </summary>
        [UserInterfaceParameter(Order = 3, IsReference = true)]
        public virtual Node Node { get; set; }

        public virtual IList<CareerPathNode> CareerPathNodes { get; set; }
        public virtual void AddCareerPathJobDescription(CareerPathNode careerPathNode)
        {
            careerPathNode.CareerPathFamily = this;
            CareerPathNodes.Add(careerPathNode);
        }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown
        {
            get
            {
                return Name;
            }
        }
    }
}
