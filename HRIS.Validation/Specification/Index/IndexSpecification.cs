using Souccar.Domain.DomainModel;
using SpecExpress;

namespace HRIS.Validation.Specification.Index
{
    public class IndexSpecification : Validates<IIndex>
    {
        public IndexSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.Name).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);

            #endregion
        }



        public  static bool IsTransient(Souccar.Domain.DomainModel.Entity obj, Souccar.Domain.DomainModel.IndexEntity prop)
        {
            return !prop.IsTransient();
        }
    }


}
