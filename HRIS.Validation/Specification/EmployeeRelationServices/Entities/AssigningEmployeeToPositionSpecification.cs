using System;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.EmployeeRelationServices.Entities
{
    public class AssigningEmployeeToPositionSpecification : Validates<AssigningEmployeeToPosition>
    {
        public AssigningEmployeeToPositionSpecification()
        {

            #region Primitive Types
            Check(x => x.Weight).Required().GreaterThanEqualTo(0).And.LessThanEqualTo(100);
            #endregion
            #region Indexes

            //Check(x => x.Position)
            //    .Required()
            //    .Expect((assigningEmployeeToPosition, position) => position.IsTransient() == false, "")
            //    .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion
            #region Document Info

            

            #endregion
            #region Document Info

          

            #endregion

        }
    }
}
