
using FluentNHibernate.Mapping;
using HRIS.Domain.Workflow;

namespace HRIS.Mapping.Workflow.RootEntities
{
      public sealed  class WorkflowItemsMap : ClassMap<WorkflowItems>
    {
        public WorkflowItemsMap()
        {
            #region Default

            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);

            #endregion
            References(x => x.Workitem);
        }
    }
}
