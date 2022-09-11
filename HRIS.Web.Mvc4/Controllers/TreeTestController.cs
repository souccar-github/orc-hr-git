//using System;
//using System.Collections.Generic;
//using System.Data.OleDb;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using HRIS.Domain.JobDescription.Indexes;
//using HRIS.Domain.Personnel.Indexes;
//using  Project.Web.Mvc4.Extensions;
//using Souccar.Domain.DomainModel;
//using Souccar.Domain.PersistenceSupport;
//using HRIS.Domain.OrganizationChart.RootEntities;
//using  Project.Web.Mvc4.Extensions;
//using Souccar.NHibernate;
//using  Project.Web.Mvc4.Models;
//using HRIS.Domain.OrganizationChart.Indexes;
//using WebMatrix.WebData;
//using Souccar.Core.Extensions;
//using Souccar.Infrastructure.Core;
//using Souccar.Infrastructure.Exceptions;
//using Souccar.Infrastructure.Extenstions;
//using  Project.Web.Mvc4.Helpers.DomainExtensions;
//using HRIS.Domain.OrganizationChart.Configurations;


//namespace Project.Web.Mvc4.Controllers
//{
//    public class TreeTestController : Controller
//    {
//        //
//        // GET: /TreeTest/

//        public ActionResult ForAlaa()
//        {
//            return View();
//        }

//        public ActionResult Index()
//        {
           
//            var e1 = new Nationality() {Name = "t1", Order = 1};
//            var e2 = new CareerLevel() {Name = "t2", Order = 1};
//            try
//            {
//                ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { e1, e2 }, UserExtensions.CurrentUser);
//            }
//            catch (ORMException e)
//            {

//            }
            
            




//            return View();
//        }

//        [HttpPost]
//        public ActionResult GetData(int? Id=null)
//        {
//            var repository = new NHibernateRepository<Node>();
//            if (Id == null)
//            {
//                var nodes = repository.GetAll().Where(x=>x.Parent==null).Select(x => new { Id = x.Id, Name = x.Name, HasChildren = x.Children.Count == 0 ? false : true }).ToArray();
//                return Json(nodes);
//            }
//            else
//            {
//                var nodes = repository.GetById((int)Id).Children.Select(x => new { Id = x.Id, Name = x.Name, HasChildren = x.Children.Count==0?false:true }).ToArray();
//                return Json(nodes);
//            }
//        }
     
//        [HttpPost]
//        public ActionResult GetAllData(int? Id = null)
//        {
//            var repository = new NHibernateRepository<Node>();
//            var nodes = getTreeViewModel(repository.GetAll().Where(x => x.Parent == null).ToList());
//            return Json(nodes);
          
//        }
//        public List<TreeViewModel> getTreeViewModel(IList<Node> nodes)
//        {
//            if(nodes==null||nodes.Count==0)
//                return new List<TreeViewModel>();
//            return
//                nodes.Select(x => new TreeViewModel() {Id = x.Id, Name = x.Name,HasChildren=x.Children.Count==0?false:true, Items  = getTreeViewModel(x.Children)}).
//                    ToList();
//        }
  
//        [HttpPost]
//        public ActionResult SaveNode(IDictionary<string,object> model)
//        {
//            var repository = new NHibernateRepository<Node>();
//            var indexRep = new NHibernateRepository<NodeType>();
//            var type = indexRep.GetById(int.Parse(model["Type"].ToString()));
//            var parent = repository.GetById(int.Parse(model["ParentId"].ToString()));
//            var n = new Node();
//            n.Name = (string) model["Name"];
//            n.Code = (string) model["Code"];
//            n.Comment = (string) model["Comment"];
//            n.FromDate = DateTime.Parse( model["FromDate"].ToString());
//            n.Type = type;
//            //parent.AddChildNode(n);
//            repository.Add(n);
//            var dbContext = (IDbContext) repository.GetPropertyValue("DbContext");
//            using (dbContext.BeginTransaction())
//            {
//                dbContext.CommitTransaction();
//            }
//            return Json("Yaseen");
//        }
    

//        public ActionResult ReportTest()
//        {
//            return View();
//        }

//        public ActionResult aggregate()
//        {
//            return View();
//        }

//        public ActionResult Read()
//        {
//            return Json(new object[]{new {firstName= "Jane", lastName= "Doe", age= 30},new{firstName= "John", lastName= "Doe", age=  33}});
//        }
//    }
//}
