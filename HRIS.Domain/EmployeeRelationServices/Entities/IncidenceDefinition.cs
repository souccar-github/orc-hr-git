using System;
using System.Collections.Generic;
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.EmployeeRelationServices.Indexes;
using HRIS.Domain.JobDesc.Entities;
using HRIS.Domain.JobDesc.RootEntities;
using HRIS.Domain.OrgChart.Entities;
using HRIS.Domain.OrgChart.Indexes;
using HRIS.Domain.OrgChart.RootEntities;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Recruitment.Enums;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using HRIS.Domain.Personnel.Entities;
using Souccar.Domain.Security;
using Souccar.Infrastructure.Core;

namespace HRIS.Domain.EmployeeRelationServices.Entities
{

    public class IncidenceDefinition : Entity, IAggregateRoot
    {

        public IncidenceDefinition()
        {
            Employees = new List<Employee>();
            FirstDate = DateTime.Today;
            this.CreationDate = DateTime.Today;
            this.ApprovedDateByCBS = DateTime.Today;
            this.StartDate = DateTime.Today;
            this.DocumentDate = DateTime.Today;

        }

        #region Basic Info

        #region DocumentInformation

        /// <summary>
        /// نوع الوقوعة
        /// </summary>
        [UserInterfaceParameter(Order = 1, ReferenceReadUrl = "EmployeeRelationServices/Reference/ReadIncidenceType", Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceType)]
        public virtual IncidenceType Type { get; set; }
        [UserInterfaceParameter(Order = 2, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.DocumentInformation)]
        public virtual AdditionalIncidenceType AdditionalType { get; set; }
        /// <summary>
        /// تاريخ إدخال الواقعة 
        /// </summary>
        [UserInterfaceParameter(Order = 3, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.DocumentInformation)]
        public virtual DateTime CreationDate { get; set; }
        /// <summary>
        /// اسم مدخل الواقعة 
        /// </summary>
        [UserInterfaceParameter(Order = 4, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.DocumentInformation)]
        public virtual Employee Entrance { get; set; }
        /// <summary>
        /// نوع المستند 
        /// </summary>
        [UserInterfaceParameter(Order = 5, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.DocumentInformation)]
        public virtual DocumentType DocumentType { get; set; }
        /// <summary>
        /// رقم المستند
        /// </summary>
        [UserInterfaceParameter(Order = 6, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.DocumentInformation)]
        public virtual string DocumentNumber { get; set; }
        /// <summary>
        /// تاريخ المستند 
        /// </summary>
        [UserInterfaceParameter(Order = 7, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.DocumentInformation)]
        public virtual DateTime? DocumentDate { get; set; }
        /// <summary>
        /// الجهة الصادر عنها 
        /// </summary>
        [UserInterfaceParameter(Order = 8, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.DocumentInformation)]
        public virtual WorkSideFrom IssuedBy { get; set; }
        /// <summary>
        /// الجهة المرسل إليها 
        /// </summary>
        [UserInterfaceParameter(Order = 9, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.DocumentInformation)]
        public virtual WorkSideTo IssuedTo { get; set; }
        [UserInterfaceParameter(Order = 10, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual ContractType ContractType { get; set; }
        /// <summary>
        /// نوع النقل
        /// </summary>
        [UserInterfaceParameter(Order = 11, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual TransportType TransportType { get; set; }
        /// <summary>
        /// إعادة للعمل لموظف
        /// </summary>
        [UserInterfaceParameter(Order = 12, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual bool AfterUnpaidLeave { get; set; }
        /// <summary>
        /// رقم موافقة الجهاز المركزي
        /// </summary>
        [UserInterfaceParameter(Order = 13, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual string ApprovedNoByCBS { get; set; }
        /// <summary>
        /// تاريخ موافقة الجهاز المركزي
        /// </summary>
        [UserInterfaceParameter(Order = 14, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual DateTime? ApprovedDateByCBS { get; set; }

        /// <summary>
        /// تاريخ المباشرة
        /// </summary>
        [UserInterfaceParameter(Order = 15, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual DateTime? StartDate { get; set; }
        /// <summary>
        /// تاريخ الانفكاك 
        /// </summary>
        [UserInterfaceParameter(Order = 16, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual DateTime FirstDate { get; set; }
        /// <summary>
        /// تاريخ المباشرة 
        /// </summary>
        [UserInterfaceParameter(Order = 17, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual DateTime? SecondDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [UserInterfaceParameter(Order = 18, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual int FirstInt { get; set; }
        /// <summary>
        /// الرقم الذاتي  
        /// </summary>
        [UserInterfaceParameter(Order = 19, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual string CardNo { get; set; }
        /// <summary>
        /// الحقوق والواجبات   
        /// الاسم الجديد
        /// </summary>
        [UserInterfaceParameter(Order = 20, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual string FirstString { get; set; }
        /// <summary>
        /// الكنية   
        /// </summary>
        [UserInterfaceParameter(Order = 21, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual string SecondString { get; set; }
        /// <summary>
        /// نوع المكافأة 
        /// </summary>
        [UserInterfaceParameter(Order = 22, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual RewardType RewardType { get; set; }
        /// <summary>
        /// تصنيف العقوبة 
        /// </summary>
        [UserInterfaceParameter(Order = 23, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual RatingPenalty RatingPenalty { get; set; }
        /// <summary>
        /// نوع التعيين  /نوع العقد
        /// </summary>
        [UserInterfaceParameter(Order = 24, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual RecruitmentType RecruitmentType { get; set; }
        /// <summary>
        /// الجهة المنقول إليها 
        /// الجهة المندب إليها
        /// الجهة المندب منها
        /// الجهة المعار إليها 
        /// الجهة المعار منها
        /// </summary>
        [UserInterfaceParameter(Order = 25, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual WorkSide WorkSide { get; set; }
        /// <summary>
        /// نوع الندب
        /// </summary>
        [UserInterfaceParameter(Order = 26, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual AssignmentType AssignmentType { get; set; }
        /// <summary>
        /// سبب العلاوة 
        /// </summary>
        [UserInterfaceParameter(Order = 27, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual CausePremium CausePremium { get; set; }
        /// <summary>
        /// نوع نهاية الخدمة 
        /// </summary>
        [UserInterfaceParameter(Order = 30, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual HRIS.Domain.EmployeeRelationServices.Enums.ServiceEndType ServiceEndType { get; set; }
        /// <summary>
        /// تكرار منح المكافأة  
        /// </summary>
        [UserInterfaceParameter(Order = 31, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual Recurrence Recurrence { get; set; }
        /// <summary>
        /// حالة خدمة العلم   
        /// </summary>
        [UserInterfaceParameter(Order = 32, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual MilitaryStatus MilitaryStatus { get; set; }
        /// <summary>
        /// عقوبات الغياب    
        /// </summary>
        [UserInterfaceParameter(Order = 33, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual AbsencePenalty AbsencePenalty { get; set; }
        /// <summary>
        /// عقوبات شديدة    
        /// </summary>
        [UserInterfaceParameter(Order = 34, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual StrongPenalty StrongPenalty { get; set; }
        /// <summary>
        /// عقوبات خفيفة    
        /// </summary>
        [UserInterfaceParameter(Order = 35, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual LightPenalty LightPenalty { get; set; }
        [UserInterfaceParameter(IsReference = true, Order = 36, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual Education Education { get; set; }
        /// <summary>
        /// العقدة
        /// </summary>
        [UserInterfaceParameter(IsReference = true, ReferenceReadUrl = "EmployeeRelationServices/Reference/ReadNodeToList/", Order = 37, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual Node Node { get; set; }
        /// <summary>
        /// المستوى الوظيفي 
        /// </summary>
        [UserInterfaceParameter(Order = 38, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual OrganizationalLevel OrganizationalLevel { get; set; }
        /// <summary>
        /// الفئات 
        /// </summary>
        [UserInterfaceParameter(IsReference = true, Order = 39, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual Grade Grade { get; set; }
        /// <summary>
        /// المسمى الوظيفي 
        /// </summary>
        [UserInterfaceParameter(IsReference = true, Order = 40, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual JobTitle JobTitle { get; set; }
        /// <summary>
        /// المسمى الوظيفي خارجي
        /// </summary>
        [UserInterfaceParameter(Order = 45, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual GlobalJobTitle GlobalJobTitle { get; set; }
        /// <summary>
        /// الوصف الوظيفي 
        /// </summary>
        [UserInterfaceParameter(IsReference = true, Order = 41, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual JobDescription JobDescription { get; set; }
        /// <summary>
        /// الموقع الوظيفي
        /// </summary>
        [UserInterfaceParameter(IsReference = true, Order = 42, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual Position Position { get; set; }
        /// <summary>
        /// مركز الكلفة 
        /// </summary>
        [UserInterfaceParameter(Order = 45, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual CostCenter CostCenter { get; set; }

        /// <summary>
        /// هل أريد إدخال الراتب أو نسبة من الراتب
        /// </summary>
        [UserInterfaceParameter(Order = 50, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual bool IsSalary { get; set; }
        /// <summary>
        /// الراتب  
        /// الأجر المقطوع قبل العلاوة
        /// </summary>
        [UserInterfaceParameter(Order = 51, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual double FirstDouble { get; set; }
        /// <summary>
        /// راتب التعويضات
        /// الأجر بعد العلاوة
        /// </summary>
        [UserInterfaceParameter(Order = 52, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual double SecondDouble { get; set; }
        /// <summary>
        /// وحدة العملة    
        /// </summary>
        [UserInterfaceParameter(Order = 53, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual CurrencyType CurrencyType { get; set; }
        /// <summary>
        /// نسبة العلاوة 
        /// </summary>
        [UserInterfaceParameter(Order = 54, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual float Percentage { get; set; }
        /// <summary>
        /// ملاحظات
        /// </summary>
        [UserInterfaceParameter(Order = 55, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual string Cause { get; set; }
        /// <summary>
        /// ملاحظات
        /// </summary>
        [UserInterfaceParameter(Order = 56/*11*/, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual string Note { get; set; }

        [UserInterfaceParameter(Order = 57, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual string FootnoteFile { get; set; }

        /// <summary>
        /// تحديد اذا كان يجب عكس الأثر على البطاقة المالية الأساسية 
        /// </summary>
        [UserInterfaceParameter(Order = 58, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual bool IsAffectOnPrimaryCard { get; set; }

        [UserInterfaceParameter(IsReference = true, IsNonEditable = true)]
        public virtual User Creator { get; set; }
        #endregion

        //[UserInterfaceParameter(IsHidden = true)]
        [UserInterfaceParameter(Order = 59, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition, IsFile = true, AcceptExtension = ".rar,.zip,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.jpg,.png,.txt,.pdf", FileSize = 5000000)]
        public virtual string AttachmentFileName { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual bool IsTransferToPayroll { get; set; }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual bool IsPopupShowed { get; set; }
        [UserInterfaceParameter(Order = 65, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.IncidenceDefinition)]
        public virtual bool IsEnteredInPMS { get; set; }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown {
            get
            {
                var employeeName = "";
                var incidenceType = ServiceFactory.LocalizationService.GetResource("HRIS.Domain.EmployeeRelationServices.Enums.IncidenceType." + Enum.GetName(typeof(IncidenceType), Type));
                var creationDate = CreationDate.ToString("yyyy/MM/dd");
                if (this.Employees.Count == 1)
                {
                    employeeName = this.Employees[0].FullName;
                }
                var valueReturned = string.IsNullOrEmpty(employeeName) ? incidenceType + "-" + creationDate : employeeName + "-" + incidenceType + "-" + creationDate;
                return valueReturned;
            }
        }
        #region Employees
        public virtual IList<Employee> Employees { get; set; }
        public virtual void AddEmployee(Employee employee)
        {
            this.Employees.Add(employee);
            employee.IncidenceDefinitions.Add(this);
        }
        #endregion

        #endregion

    }
}
