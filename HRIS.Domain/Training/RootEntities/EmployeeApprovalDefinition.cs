////-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
////*******company name: souccar for electronic industries*******//
////project manager:
////supervisor:
////author: Yaseen Alrefaee
////description:
////start date:
////end date:
////last update:
////update by: Ammar Alziebak
////-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//using System;
//using System.Collections.Generic;
//using FluentNHibernate.Data;
//using HRIS.Domain.JobDesc.Indexes;
//using HRIS.Domain.OrgChart.Indexes;
//using HRIS.Domain.Training.Entities;
//using HRIS.Domain.Training.Enums;
//using HRIS.Domain.Training.Indexes;
//using Souccar.Core.CustomAttribute;
//using HRIS.Domain.Global.Constant;
//using Souccar.Domain.Attachment;
//using Souccar.Domain.Attachment.Entities;
//using Souccar.Domain.DomainModel;
//using HRIS.Domain.Personnel.Indexes;
//using Entity = Souccar.Domain.DomainModel.Entity;
//using HRIS.Domain.PMS.Helpers;
//using HRIS.Domain.EmployeeRelationServices.Indexes;

//namespace HRIS.Domain.Training.RootEntities
//{
//    /// <summary>
//    /// Author: Ammar Alziebak
//    /// </summary>
//    [Module(ModulesNames.Training)]
//    public class EmployeeApprovalDefinition : Entity, IAggregateRoot
//    {
//        public EmployeeApprovalDefinition()
//        {
//            DocumentDate = DateTime.Today;
//        }

//        //نوع الموافقة 
//        [UserInterfaceParameter(Order = 1, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.CourseInformation)]
//        public virtual DocumentType DocumentType { get; set; }
//        //رقم الموافقة
//        [UserInterfaceParameter(Order = 2, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.CourseInformation)]
//        public virtual int DocumentNumber { get; set; }
//        //تاريخ  الموافقة
//        [UserInterfaceParameter(Order = 3, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.CourseInformation)]
//        public virtual DateTime DocumentDate { get; set; }
//        //توصيف
//        [UserInterfaceParameter(Order = 4, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.CourseInformation)]
//        public virtual string Description { get; set; }

//    }
//}
