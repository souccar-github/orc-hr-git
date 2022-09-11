using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Infrastructure.Entities;
using HRIS.Domain.OrgChart.ValueObjects;
using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.Personnel.ValueObjects;
using Souccar.Domain.DomainModel;
using UI.Areas.OrganizationChart.Helpers;
using UI.Areas.PMSComprehensive.DTO.ViewModels;

namespace UI.Areas.Services.DTO.ViewModels
{
    public class ContactViewModel : Entity, IAggregateRoot
    {
        private Contact contact;
        public Contact Contact
        {
            get { return contact; }
            set { contact = value; }
        }

        private Employee employee;
        public Employee Employee
        {
            get { return employee; }
            set { employee = value; }
        }

    }
}