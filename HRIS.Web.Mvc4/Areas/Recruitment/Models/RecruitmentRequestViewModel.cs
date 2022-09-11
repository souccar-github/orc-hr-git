using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Recruitment.Enums;
using HRIS.Domain.Recruitment.RootEntities;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Core.Fasterflect;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Areas.Recruitment.Models
{
    public class RecruitmentRequestViewModel : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(RecruitmentRequestViewModel).FullName;
            model.Views[0].EditHandler = "recruitmentRequestEditHandler";
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null,
            Entity parententity = null)
        {
            

        }

        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var recruitmentRequest = entity as RecruitmentRequest;

            if (recruitmentRequest != null)
            {
                recruitmentRequest.RequestStatus = RequestStatus.Initiated;
                recruitmentRequest.Requester = UserExtensions.CurrentUser;
                var currentEmployee = EmployeeExtensions.CurrentEmployee;
                if (currentEmployee != null)
                {
                    recruitmentRequest.RequesterPosition = currentEmployee.PrimaryPosition();

                }
            }
        }
    }
}