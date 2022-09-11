//using HRIS.Domain.PayrollSystem.RootEntities;
//using HRIS.Validation.MessageKeys;
//using Souccar.Infrastructure.Extenstions;
//using SpecExpress;

//namespace HRIS.Validation.Specification.PayrollSystem.RootEntities
//{
//    public class NodeBenefitMasterSpecification : Validates<NodeBenefitMaster>
//    {
//        public NodeBenefitMasterSpecification()
//        {
//            IsDefaultForType();

//            Check(x => x.Node, y => typeof(NodeBenefitMaster).GetProperty("Node").GetTitle())
//                .Required()
//                .Expect((nodeBenefitMaster, node) => node.IsTransient() == false, "")
//                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
//        }
//    }

//}
