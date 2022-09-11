#region

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using HRIS.Domain.JobDescription.RootEntities;
using HRIS.Domain.Objectives.RootEntities;
using HRIS.Domain.OrganizationChart.Configurations;
using HRIS.Domain.OrganizationChart.Indexes;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.PayrollSystem.Entities;
using Souccar.Domain.DomainModel;
using Souccar.Core.CustomAttribute;

#endregion

namespace HRIS.Domain.OrganizationChart.RootEntities
{
    [Module(ModulesNames.OrganizationChart)]
    //[Module(ModulesNames.PayrollSystem)]
    [Order(5)]
    public class Node : HistoryEntity, IAggregateRoot
    {

        public Node()
        {
            Children = new List<Node>();
            JobDescriptions = new List<HRIS.Domain.JobDescription.RootEntities.JobDescription>();
            NodeBenefitDetails = new List<NodeBenefitDetail>();
            NodeDeductionDetails = new List<NodeDeductionDetail>();
        }

        public override string ToString()
        {
            int i = -1;
            string toString = string.Empty;

            if (Children.Count == 0)
            {
                //Node Format
                //"{id:\"node02\", name:\"0.2\", code:\"0000\", data:{}, children:[{id:\"node13\", name:\"1.3\", code:\"0001\", data:{}, children:[]}]}"
                return string.Format("{{id:\"{0}\", name:\"{1}\", data:{{code:\"{2}\"}},children:[]}}", Id, Name, Code);
            }

            foreach (Node child in Children)
            {
                i++;
                if (i == 0)
                {
                    toString += string.Format("{{id:\"{0}\", name:\"{1}\", data:{{code:\"{2}\"}},children:[{3},]}}", Id,
                                              Name, Code, Children[i]);
                }
                else
                {
                    string temp = Children[i] + ",";
                    toString = toString.Insert(toString.LastIndexOf(",") + 1, temp);
                }
            }

            return toString;
        }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return Name; } }

        #region General

        public virtual string Name { get; set; }

        public virtual NodeType Type { get; set; }

        public virtual Node Parent { get; set; }

        public virtual string Code { get; set; }

        public virtual bool IsRoot
        {
            get { return Parent == null; }
        }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual bool IsHistorical { get; set; }

        #endregion

        #region JobDescriptions

        public virtual IList<HRIS.Domain.JobDescription.RootEntities.JobDescription> JobDescriptions { get; set; }

        public virtual void AddJobDescription(HRIS.Domain.JobDescription.RootEntities.JobDescription jobDescription)
        {
            jobDescription.Node = this;
            JobDescriptions.Add(jobDescription);
        }

        #endregion

        #region Children Nodes

        public virtual IList<Node> Children { get; set; }

        public virtual void AddChildNode(Node node)
        {
            node.Parent = this;
            Children.Add(node);
        }

        #endregion

        #region NodeBenefitDetails
        public virtual IList<NodeBenefitDetail> NodeBenefitDetails { get; set; } // ��������� ���� ���� ����� 
        public virtual void AddNodeBenefitDetail(NodeBenefitDetail nodeBenefitDetail)
        {
            NodeBenefitDetails.Add(nodeBenefitDetail);
            nodeBenefitDetail.Node = this;
        }

        #endregion

        #region NodeDeductionDetails
        public virtual IList<NodeDeductionDetail> NodeDeductionDetails { get; set; }
        public virtual void AddNodeDeductionDetail(NodeDeductionDetail nodeDeductionDetail)
        {
            NodeDeductionDetails.Add(nodeDeductionDetail);
            nodeDeductionDetail.Node = this;
        }

        #endregion
        

        //public IEnumerator GetEnumerator()
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}