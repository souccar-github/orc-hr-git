using HRIS.Domain.OrganizationChart.Configurations;
using SpecExpress;

namespace HRIS.Validation.Specification.OrganizationChart.Configurations
{
    public class NodeTypeSpecification : Validates<NodeType>
    {
        public NodeTypeSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.Code).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.Name).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.Order).Required().Between(GlobalConstant.MinimumNodeTypeOrderLengthn, GlobalConstant.MaximumNodeTypeOrderLength);
            
           

            #endregion

            #region Indexes

            

            #endregion
        }
    }
}
