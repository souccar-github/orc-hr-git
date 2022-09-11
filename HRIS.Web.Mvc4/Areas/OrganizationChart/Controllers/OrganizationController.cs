using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.OrganizationChart.Configurations;
using HRIS.Domain.OrganizationChart.Indexes;
using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.Personnel.Indexes;
using Project.Web.Mvc4.Extensions;
using Project.Web.Mvc4.Models;
using Souccar.Domain.PersistenceSupport;
using Souccar.Infrastructure.Core;
using Souccar.NHibernate;
using Souccar.Domain.Validation;
using Project.Web.Mvc4.Helpers.Resource;
using System.Text.RegularExpressions;
using Souccar.Domain.DomainModel;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Souccar.Domain.Audit;

namespace Project.Web.Mvc4.Areas.OrganizationChart.Controllers
{
    public class OrganizationController : Controller
    {
        private string _message = string.Empty;
        private bool _isSuccess;
        private List<ValidationResult> _validationResults;
        private Dictionary<string, string> _errorsMessages;

        public ActionResult Organization()
        {
            return PartialView();
        }
        public ActionResult PrivateOrganization()
        {
            return PartialView();
        }

        public ActionResult GetOrgnizationForView()
        {
            var org = ServiceFactory.ORMService.All<Organization>().FirstOrDefault();
            if (org == null)
            {
                org=new Organization();
                org.Save();
            }
            else
            {
                org.Address = org.Address ?? string.Empty;
                org.Facebook = org.Facebook ?? string.Empty;
                org.Fax = org.Fax ?? string.Empty;
                org.Mobile = org.Mobile ?? string.Empty;
                org.Name = org.Name ?? string.Empty;
                org.Phone = org.Phone ?? string.Empty;
                org.POBox = org.POBox ?? string.Empty;
                org.WebSite = org.WebSite ?? string.Empty;

            }
            return Json(org.ToDynamicObj(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveOrgnization(Organization organization,int locationId,int sizeId)
        {
            if (IsValidOrgnizationInfo(organization, locationId, sizeId))
            {
                var org = ServiceFactory.ORMService.All<Organization>().FirstOrDefault();
                if (org == null)
                {
                    org = new Organization();
                    org.Save();
                }
                org.Location = ServiceFactory.ORMService.GetById<Country>(locationId);
                org.Size = ServiceFactory.ORMService.GetById<OrganizationSize>(sizeId);
                org.NumberOfEmployees = organization.NumberOfEmployees;
                org.Name = organization.Name ?? string.Empty;
                org.Phone = organization.Phone ?? string.Empty;
                org.Fax = organization.Fax ?? string.Empty;
                org.Facebook = organization.Facebook ?? string.Empty;
                org.Mobile = organization.Mobile ?? string.Empty;
                org.Address = organization.Address ?? string.Empty;
                org.POBox = organization.POBox ?? string.Empty;
                org.WebSite = organization.WebSite ?? string.Empty;
                var result = org.Save();
                ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() {  }, UserExtensions.CurrentUser, organization, OperationType.Insert);

                _isSuccess = true;


                SaveNewRootNodeForOrgnization(organization);
                //_message = OrganizationChartLocalizationHelper.GetResource(OrganizationChartLocalizationHelper.NewOrganizationRootNodeAdded);

                return Json(new { Success = true , Error = result, Org = org }, JsonRequestBehavior.AllowGet);
            }
         
            return Json(new
            {
                Success = false,
                Msg = _message
            }, JsonRequestBehavior.AllowGet);
        }
        public void SaveNewRootNodeForOrgnization (Organization organization)
        {
            var existNodeType = ServiceFactory.ORMService.All<NodeType>().Where(x => x.Code == "ORG").FirstOrDefault();
            if (existNodeType == null)
            {
                NodeType nodeType = new NodeType();
                nodeType.Name = organization.Name;
                nodeType.Code = "ORG";
                nodeType.Order = 1;
                nodeType.Save();

                var existOrgNode = ServiceFactory.ORMService.All<Node>().Where(x => x.Code == "ORG").FirstOrDefault();
                if (existOrgNode == null)
                {
                    Node orgNode = new Node();
                    orgNode.Name = organization.Name;
                    orgNode.Code = "ORG";
                    orgNode.Type = nodeType;
                   // orgNode.Save();
                    ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { orgNode }, UserExtensions.CurrentUser, organization, OperationType.Insert);

                }
            }
            else 
                if (existNodeType != null)
                {
                    existNodeType.Name = organization.Name;
                    existNodeType.Save();

                    var existOrgNode = ServiceFactory.ORMService.All<Node>().Where(x => x.Code == "ORG").FirstOrDefault();
                    if (existOrgNode != null)
                    {
                        existOrgNode.Name = organization.Name;
                        //.Save();
                    ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { existOrgNode }, UserExtensions.CurrentUser, organization, OperationType.Update);

                }
            } 
        }
        private bool IsValidOrgnizationInfo(Organization organization, int locationId, int sizeId)
        {
            var phone = organization.Phone;
            var fax = organization.Fax;
            var mobile = organization.Mobile;
            var website = organization.WebSite;
            var facebook = organization.Facebook;
            bool isMatchWebsite = true;
            bool isMatchFacebook = true;

            var websitePattern = @"^(http|https|ftp|)\://|[a-zA-Z0-9\-\.]+\.[a-zA-Z](:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$";
            if (website != null)
            {
                isMatchWebsite = Regex.IsMatch(website, websitePattern);
            }
            if (facebook != null)
            {
                isMatchFacebook = Regex.IsMatch(facebook, websitePattern); 
            }

            int i = 0;

            bool phoneIsNumeric = int.TryParse(phone, out i);
            bool faxIsNumeric = int.TryParse(fax, out i);
            bool mobileIsNumeric = int.TryParse(mobile, out i);

            if (string.IsNullOrEmpty(organization.Name))
            {
                _message = OrganizationChartLocalizationHelper.GetResource(OrganizationChartLocalizationHelper.MsgOrganizationNameIsRequired);
                return false;
            }

            if (locationId == 0)
            {
                _message = OrganizationChartLocalizationHelper.GetResource(OrganizationChartLocalizationHelper.MsgLocationIsRequired);
                return false;
            }

            if (sizeId == 0)
            {
                _message = OrganizationChartLocalizationHelper.GetResource(OrganizationChartLocalizationHelper.MsgSizeIsRequired);
                return false;
            }

            if (organization.NumberOfEmployees == 0)
            {
                _message = OrganizationChartLocalizationHelper.GetResource(OrganizationChartLocalizationHelper.MsgNumberOfEmployeesIsRequired);
                return false;
            }

            if (string.IsNullOrEmpty(organization.Phone))
            {
                _message = OrganizationChartLocalizationHelper.GetResource(OrganizationChartLocalizationHelper.MsgPhoneIsRequired);
                return false;
            }

            if (string.IsNullOrEmpty(organization.Fax))
            {
                _message = OrganizationChartLocalizationHelper.GetResource(OrganizationChartLocalizationHelper.MsgFaxIsRequired);
                return false;
            }

            if (string.IsNullOrEmpty(organization.Mobile))
            {
                _message = OrganizationChartLocalizationHelper.GetResource(OrganizationChartLocalizationHelper.MsgMobileIsRequired);
                return false;
            }

            if (string.IsNullOrEmpty(organization.Address))
            {
                _message = OrganizationChartLocalizationHelper.GetResource(OrganizationChartLocalizationHelper.MsgAddressIsRequired);
                return false;
            }

            if (!phoneIsNumeric)
            {
                _message = OrganizationChartLocalizationHelper.GetResource(OrganizationChartLocalizationHelper.MsgPhoneIsNotNumeric);
                return false;
            }

            if (!faxIsNumeric)
            {
                _message = OrganizationChartLocalizationHelper.GetResource(OrganizationChartLocalizationHelper.MsgFaxIsNotNumeric);
                return false;
            }

            if (!mobileIsNumeric)
            {
                _message = OrganizationChartLocalizationHelper.GetResource(OrganizationChartLocalizationHelper.MsgMobileIsNotNumeric);
                return false;
            }

            if (!isMatchWebsite)
            {
                _message = OrganizationChartLocalizationHelper.GetResource(OrganizationChartLocalizationHelper.MsgIsNotMatchWebsite);
                return false;
            }

            if (!isMatchFacebook)
            {
                _message = OrganizationChartLocalizationHelper.GetResource(OrganizationChartLocalizationHelper.MsgIsNotMatchFacebook);
                return false;
            }

            return true;
        }
        

        private Organization GetOrganization()
        {
            var organizationRepository = GetOrganizationRepository();
            var organization = organizationRepository.GetAll().FirstOrDefault();

            return organization;
        }

        private NHibernateRepository<Organization> GetOrganizationRepository()
        {
            return new NHibernateRepository<Organization>();
        }

       
        private void InitialzeDefaultValues()
        {
            _isSuccess = false;
            _message = Helpers.GlobalResource.FailMessage;
        }

        private void CommitTransaction(IDbContext dbContext)
        {
            using (dbContext.BeginTransaction())
            {
                dbContext.CommitTransaction();
            }
        }

        private void RollbackTransaction(IDbContext dbContext)
        {
            using (dbContext.BeginTransaction())
            {
                dbContext.RollbackTransaction();
            }
        }
    }
}
