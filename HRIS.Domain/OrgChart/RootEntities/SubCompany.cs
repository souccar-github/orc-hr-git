#region

using HRIS.Domain.Global.Constant;
using HRIS.Domain.OrganizationChart.Helpers;
using HRIS.Domain.OrganizationChart.Indexes;
using HRIS.Domain.Personnel.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.OrganizationChart.RootEntities
{
    [Module(ModulesNames.OrganizationChart)]
    [Order(3)]
    public class SubCompany : Entity, IAggregateRoot
    {
        public SubCompany()
        {
            
        }


        #region General

        [UserInterfaceParameter(Group = OrgChartGoupesNames.ResourceGroupName + "_" + OrgChartGoupesNames.General, Order = 5)]
        public virtual string Name { get; set; }

        [UserInterfaceParameter(Group = OrgChartGoupesNames.ResourceGroupName + "_" + OrgChartGoupesNames.General, Order = 10)]
        public virtual Country Location { get; set; }

        [UserInterfaceParameter(Group = OrgChartGoupesNames.ResourceGroupName + "_" + OrgChartGoupesNames.General, Order = 15)]
        public virtual OrganizationSize Size { get; set; }

        [UserInterfaceParameter(Group = OrgChartGoupesNames.ResourceGroupName + "_" + OrgChartGoupesNames.General, Order = 20)]
        public virtual int NumberOfEmployees { get; set; }

        [UserInterfaceParameter(Group = OrgChartGoupesNames.ResourceGroupName + "_" + OrgChartGoupesNames.ContactInformation, Order = 25)]
        public virtual string Phone { get; set; }

        [UserInterfaceParameter(Group = OrgChartGoupesNames.ResourceGroupName + "_" + OrgChartGoupesNames.ContactInformation, Order = 30)]
        public virtual string Fax { get; set; }

        [UserInterfaceParameter(Group = OrgChartGoupesNames.ResourceGroupName + "_" + OrgChartGoupesNames.ContactInformation, Order = 35)]
        public virtual string Mobile { get; set; }

        [UserInterfaceParameter(Group = OrgChartGoupesNames.ResourceGroupName + "_" + OrgChartGoupesNames.ContactInformation, Order = 40)]
        public virtual string Address { get; set; }

        [UserInterfaceParameter(Group = OrgChartGoupesNames.ResourceGroupName + "_" + OrgChartGoupesNames.ContactInformation, Order = 45)]
        public virtual string POBox { get; set; }

        [UserInterfaceParameter(Group = OrgChartGoupesNames.ResourceGroupName + "_" + OrgChartGoupesNames.ContactInformation, Order = 50)]
        public virtual string WebSite { get; set; }

        [UserInterfaceParameter(Group = OrgChartGoupesNames.ResourceGroupName + "_" + OrgChartGoupesNames.ContactInformation, Order = 55)]
        public virtual string Facebook { get; set; }

        [UserInterfaceParameter(Group = OrgChartGoupesNames.ResourceGroupName + "_" + OrgChartGoupesNames.Additional,
            IsFile = true, AcceptExtension = ".rar,.zip,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.jpg,.png,.txt,.pdf", FileSize = 5000000, Order = 60)]
        public virtual string LogoPath { get; set; }

        [UserInterfaceParameter(IsImageColumn = true, ImageColumnPath = "Content/UploadedFiles/HRIS.Domain.OrganizationChart.RootEntities.SubCompany/LogoPath/", DefaultImageName = "DefaultSubCompany.jpg", Order = 1, Width = 60, IsHidden = false)]
        public virtual string Photo
        {
            get { return LogoPath; }
        }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual Organization Organization { get; set; }

        #endregion
        
    }
}