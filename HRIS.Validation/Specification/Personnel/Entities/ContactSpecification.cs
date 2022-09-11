//using HRIS.Domain.Personnel.Entities;
//using SpecExpress;

//namespace HRIS.Validation.Specification.Personnel.Entities
//{
//    public class ContactSpecification : Validates<Contact>
//    {
//        public ContactSpecification()
//        {
//            IsDefaultForType();

//            #region Primitive Types

//            Check(x => x.FirstContact).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
//            Check(x => x.Fax).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
//            Check(x => x.Address).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

//            Check(x => x.SecondContact).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength);
//            Check(x => x.PrimaryEMail).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength).And.Matches(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
//            Check(x => x.SecondaryEMail).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength).And.Matches(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
//            Check(x => x.POBox).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength);
//            Check(x => x.WebSite).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength).And.Matches(@"^([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$");
//            Check(x => x.Twitter)
//                .Optional()
//                .MaxLength(GlobalConstant.MultiLinesStringMaxLength)
//                .And.Matches(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
//            Check(x => x.Facebook).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength).And.Matches(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");

//            #endregion
//        }
//    }
//}
