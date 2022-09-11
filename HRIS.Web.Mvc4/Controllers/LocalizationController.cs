
using System.Web.Mvc;
using Souccar.Domain.Localization;
using Souccar.Domain.PersistenceSupport;
using Souccar.NHibernate;

namespace Project.Web.Mvc4.Controllers
{
    public class LocalizationController : Controller
    {
        private IRepository<Language> _repository;
        private IRepository<ResourceGroup> _resourceGroupRepository;

        public LocalizationController()
        {
            _repository = new NHibernateRepository<Language>();
            _resourceGroupRepository = new NHibernateRepository<ResourceGroup>();
        }

        

        //public ActionResult ImportFromCSV()
        //{
        //    var rep = new NHibernateRepository<Language>();
        //    var reader = new StreamReader(System.IO.File.OpenRead(@"d:\45.csv"), Encoding.UTF8);
        //    var ar_lan = rep.GetAll().SingleOrDefault(x => x.LanguageCulture.ToLower().StartsWith("ar"));
        //    if (ar_lan != null)
        //    {
        //        if (!reader.EndOfStream)
        //            reader.ReadLine();
        //        while (!reader.EndOfStream)
        //        {
        //            var line = reader.ReadLine();
        //            var values = line.Split(',');
        //            if (values[6] == "1")
        //            {
        //                var temp = ar_lan.LocaleStringResources.SingleOrDefault(x => x.ResourceName.Equals(values[1]));
        //                if (temp != null)
        //                    temp.ResourceValue = values[2];
        //            }
        //        }
        //    }
        //    CommitTransaction(rep.DbContext);
        //    return Content("Done");
        //}

        //public ActionResult ImportFromNewCSV(string id)
        //{
        //    var rep = new NHibernateRepository<Language>();
        //    var reader = new StreamReader(System.IO.File.OpenRead(@"d:\" + id + ".csv"), Encoding.UTF8);
        //    var ar_lan = rep.GetAll().SingleOrDefault(x => x.LanguageCulture.ToLower().StartsWith("ar"));
        //    if (ar_lan != null)
        //    {
        //        if (!reader.EndOfStream)
        //            reader.ReadLine();
        //        while (!reader.EndOfStream)
        //        {
        //            var line = reader.ReadLine();
        //            var values = line.Split(',');
        //            var temp = ar_lan.LocaleStringResources.SingleOrDefault(x => x.ResourceName.Equals(values[0]));
        //            if (temp != null)
        //                temp.ResourceValue = values[1];
        //        }

        //    }
        //    CommitTransaction(rep.DbContext);
        //    return Content("Done");
        //}
        
        //public ActionResult ImportFromExel()
        //{
        //    string con =
        //        @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\45.xls;Extended Properties='Excel 8.0;HDR=Yes;'";
        //    using (OleDbConnection connection = new OleDbConnection(con))
        //    {
        //        connection.Open();
        //        var comString = "select * from [Sheet1$]";
        //        OleDbCommand command = new OleDbCommand(comString, connection);
        //        using (OleDbDataReader dr = command.ExecuteReader())
        //        {
        //            while (dr.Read())
        //            {
        //                var row1Col0 = dr[0];
        //                Console.WriteLine(row1Col0);
        //            }
        //        }
        //    }
        //    return Content("Done");
        //}


        private void CommitTransaction(IDbContext dbContext)
        {
            using (dbContext.BeginTransaction())
            {
                dbContext.CommitTransaction();
            }
        }

    }
}
