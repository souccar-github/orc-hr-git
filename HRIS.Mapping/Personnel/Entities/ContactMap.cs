//#region

//using FluentNHibernate.Mapping;
//using HRIS.Domain.Personnel.Entities;

//#endregion

//namespace HRIS.Mapping.Personnel.Entities
//{
//    public sealed class ContactMap : ClassMap<Contact>
//    {
//        public ContactMap()
//        {
//            DynamicUpdate();
//            DynamicInsert();

//            Id(x => x.Id);

//            Map(x => x.FirstContact).Length(50);
//            Map(x => x.SecondContact).Length(50);
//            Map(x => x.Fax).Length(50);
//            Map(x => x.PrimaryEMail).Length(50);
//            Map(x => x.SecondaryEMail).Length(50);
//            Map(x => x.Address);
//            Map(x => x.POBox).Length(50);
//            Map(x => x.WebSite).Length(50);
//            Map(x => x.Twitter).Length(50);
//            Map(x => x.Facebook).Length(50);

//            References(x => x.Employee);
//        }
//    }
//}

#warning to delete