//namespace Reporting.JobDesc
//{
//    partial class JobDescTemplate
//    {
//        /// <summary>
//        /// Required designer variable.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        /// <summary> 
//        /// Clean up any resources being used.
//        /// </summary>
//        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        #region Designer generated code

//        /// <summary>
//        /// Required method for Designer support - do not modify
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            this.components = new System.ComponentModel.Container();
//            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JobDescTemplate));
//            this.BasicInfoBand = new DevExpress.XtraReports.UI.DetailBand();
//            this.valueBasicInfoJobSummary = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblBasicInfoJobSummary = new DevExpress.XtraReports.UI.XRLabel();
//            this.valueBasicInfoNodeName = new DevExpress.XtraReports.UI.XRLabel();
//            this.valueBasicInfoNodeType = new DevExpress.XtraReports.UI.XRLabel();
//            this.valueBasicInfoReportingTo = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblBasicInfoReportingTo = new DevExpress.XtraReports.UI.XRLabel();
//            this.valueBasicInfoJobTilte = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblBasicInfoJobTilte = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblBasicInfoJobDescribtionTemplate = new DevExpress.XtraReports.UI.XRLabel();
//            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
//            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
//            this.bindingSourceJobDescriptionTemplateDTO = new System.Windows.Forms.BindingSource(this.components);
//            this.RolesReportBand = new DevExpress.XtraReports.UI.DetailReportBand();
//            this.RolesDetailBand = new DevExpress.XtraReports.UI.DetailBand();
//            this.valueRolesJobTitleAndWeight = new DevExpress.XtraReports.UI.XRLabel();
//            this.ResponsibilitiesReportBand = new DevExpress.XtraReports.UI.DetailReportBand();
//            this.ResponsibilitiesDetailBand = new DevExpress.XtraReports.UI.DetailBand();
//            this.valueResponsibilitiesKpi = new DevExpress.XtraReports.UI.XRLabel();
//            this.valueResponsibilitiesWeight = new DevExpress.XtraReports.UI.XRLabel();
//            this.valueResponsibilitiesDescription = new DevExpress.XtraReports.UI.XRLabel();
//            this.ResponsibilitiesHeaderBand = new DevExpress.XtraReports.UI.GroupHeaderBand();
//            this.lblResponsibilitiesKpi = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblResponsibilitiesWeight = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblResponsibilitiesDescription = new DevExpress.XtraReports.UI.XRLabel();
//            this.RolesHeaderBand = new DevExpress.XtraReports.UI.GroupHeaderBand();
//            this.lblRolesAndResponsibilities = new DevExpress.XtraReports.UI.XRLabel();
//            this.AuthoritiesReportBand = new DevExpress.XtraReports.UI.DetailReportBand();
//            this.AuthoritiesDetailBand = new DevExpress.XtraReports.UI.DetailBand();
//            this.lblAuthoritiesTitle = new DevExpress.XtraReports.UI.XRLabel();
//            this.AuthoritiesHeaderBand = new DevExpress.XtraReports.UI.GroupHeaderBand();
//            this.lblAuthoritiesAuthorityLevel = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblAuthoritiesOperatingBudget = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblAuthoritiesDimensions = new DevExpress.XtraReports.UI.XRLabel();
//            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
//            this.JobRelationshipsReportBand = new DevExpress.XtraReports.UI.DetailReportBand();
//            this.JobRelationshipsDetailBand = new DevExpress.XtraReports.UI.DetailBand();
//            this.valueJobRelationshipsExternal = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblJobRelationshipsExternal = new DevExpress.XtraReports.UI.XRLabel();
//            this.valueJobRelationshipsInternal = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblJobRelationshipsInternal = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblJobRelationships = new DevExpress.XtraReports.UI.XRLabel();
//            this.QualificationsReportBand = new DevExpress.XtraReports.UI.DetailReportBand();
//            this.QualificationsDetailBand = new DevExpress.XtraReports.UI.DetailBand();
//            this.EducationReportBand = new DevExpress.XtraReports.UI.DetailReportBand();
//            this.EducationDetailBand = new DevExpress.XtraReports.UI.DetailBand();
//            this.valueEducationType = new DevExpress.XtraReports.UI.XRLabel();
//            this.valueEducationMajor = new DevExpress.XtraReports.UI.XRLabel();
//            this.EducationHeaderBand = new DevExpress.XtraReports.UI.GroupHeaderBand();
//            this.lblEducationType = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblEducationMajor = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblEducation = new DevExpress.XtraReports.UI.XRLabel();
//            this.ExperienceReportBand = new DevExpress.XtraReports.UI.DetailReportBand();
//            this.ExperienceDetailBand = new DevExpress.XtraReports.UI.DetailBand();
//            this.valueExperienceCareerLevel = new DevExpress.XtraReports.UI.XRLabel();
//            this.valueExperienceIndustry = new DevExpress.XtraReports.UI.XRLabel();
//            this.ExperienceHeaserBand = new DevExpress.XtraReports.UI.GroupHeaderBand();
//            this.lblExperience = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblExperienceIndustry = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblExperienceCareerLevel = new DevExpress.XtraReports.UI.XRLabel();
//            this.LanguagesReportBand = new DevExpress.XtraReports.UI.DetailReportBand();
//            this.LanguagesDetailBand = new DevExpress.XtraReports.UI.DetailBand();
//            this.valueLanguageReading = new DevExpress.XtraReports.UI.XRLabel();
//            this.valueLanguageSpeaking = new DevExpress.XtraReports.UI.XRLabel();
//            this.valueLanguageWriting = new DevExpress.XtraReports.UI.XRLabel();
//            this.valueLanguageName = new DevExpress.XtraReports.UI.XRLabel();
//            this.valueLanguageListening = new DevExpress.XtraReports.UI.XRLabel();
//            this.LanguagesHeaderBand = new DevExpress.XtraReports.UI.GroupHeaderBand();
//            this.lblLanguageListening = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblLanguageReading = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblLanguageWriting = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblLanguageSpeaking = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblLanguageName = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblLanguage = new DevExpress.XtraReports.UI.XRLabel();
//            this.PCSkillsReportBand = new DevExpress.XtraReports.UI.DetailReportBand();
//            this.PCSkillsDetailBand = new DevExpress.XtraReports.UI.DetailBand();
//            this.valuePCSkillsType = new DevExpress.XtraReports.UI.XRLabel();
//            this.valuePCSkillsLevel = new DevExpress.XtraReports.UI.XRLabel();
//            this.PCSkillsHeaderBand = new DevExpress.XtraReports.UI.GroupHeaderBand();
//            this.lblPCSkillsLevel = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblPCSkillsType = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblPCSkills = new DevExpress.XtraReports.UI.XRLabel();
//            this.OtherSkillsReportBand = new DevExpress.XtraReports.UI.DetailReportBand();
//            this.OtherSkillsDetailBand = new DevExpress.XtraReports.UI.DetailBand();
//            this.valueOtherSkillsLevel = new DevExpress.XtraReports.UI.XRLabel();
//            this.valueOtherSkillsType = new DevExpress.XtraReports.UI.XRLabel();
//            this.OtherSkillsHeaderBand = new DevExpress.XtraReports.UI.GroupHeaderBand();
//            this.lblOtherSkillsLevel = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblOtherSkillsType = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblOtherSkills = new DevExpress.XtraReports.UI.XRLabel();
//            this.QualificationsHeaderBand = new DevExpress.XtraReports.UI.GroupHeaderBand();
//            this.lblQualifications = new DevExpress.XtraReports.UI.XRLabel();
//            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
//            this.CompetenciesReportBand = new DevExpress.XtraReports.UI.DetailReportBand();
//            this.CompetenciesDetailBand = new DevExpress.XtraReports.UI.DetailBand();
//            this.ValueCompetenciesDescription = new DevExpress.XtraReports.UI.XRLabel();
//            this.ValueCompetenciesName = new DevExpress.XtraReports.UI.XRLabel();
//            this.valueCompetenciesLevel = new DevExpress.XtraReports.UI.XRLabel();
//            this.valueCompetenciesWeight = new DevExpress.XtraReports.UI.XRLabel();
//            this.CompetenciesTypeGroupHeaderBand = new DevExpress.XtraReports.UI.GroupHeaderBand();
//            this.valueCompetenciesTypeGroup = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblCompetenciesWeight = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblCompetenciesLevel = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblCompetenciesDescription = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblCompetenciesName = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblCompetenciesTypeGroup = new DevExpress.XtraReports.UI.XRLabel();
//            this.CompetenciesHeaderBand = new DevExpress.XtraReports.UI.GroupHeaderBand();
//            this.lblCompetencies = new DevExpress.XtraReports.UI.XRLabel();
//            this.ApprovalsReportBand = new DevExpress.XtraReports.UI.DetailReportBand();
//            this.ApprovalsDetailBand = new DevExpress.XtraReports.UI.DetailBand();
//            this.lblApprovalsEmptyCell3Row3 = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblApprovalsEmptyCell2Row3 = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblApprovalsEmptyCell1Row3 = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblApprovalsHeadOfDepartment = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblApprovalsDirectLineManager = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblApprovalsEmptyCell2Row2 = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblApprovalsEmptyCell1Row2 = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblApprovalsEmptyCell3Row2 = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblApprovalsEmptyCell3Row1 = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblApprovalsEmptyCell2Row1 = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblApprovalsEmptyCell1Row1 = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblApprovalsEmployee = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblApprovalsDate = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblApprovalsSignature = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblApprovalsName = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblApprovalsEmptyCellCorner = new DevExpress.XtraReports.UI.XRLabel();
//            this.lblApprovals = new DevExpress.XtraReports.UI.XRLabel();
//            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceJobDescriptionTemplateDTO)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
//            // 
//            // BasicInfoBand
//            // 
//            this.BasicInfoBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
//            this.valueBasicInfoJobSummary,
//            this.lblBasicInfoJobSummary,
//            this.valueBasicInfoNodeName,
//            this.valueBasicInfoNodeType,
//            this.valueBasicInfoReportingTo,
//            this.lblBasicInfoReportingTo,
//            this.valueBasicInfoJobTilte,
//            this.lblBasicInfoJobTilte,
//            this.lblBasicInfoJobDescribtionTemplate});
//            resources.ApplyResources(this.BasicInfoBand, "BasicInfoBand");
//            this.BasicInfoBand.Name = "BasicInfoBand";
//            this.BasicInfoBand.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
//            // 
//            // valueBasicInfoJobSummary
//            // 
//            this.valueBasicInfoJobSummary.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
//            new DevExpress.XtraReports.UI.XRBinding("Text", null, "JobSummary")});
//            resources.ApplyResources(this.valueBasicInfoJobSummary, "valueBasicInfoJobSummary");
//            this.valueBasicInfoJobSummary.Name = "valueBasicInfoJobSummary";
//            this.valueBasicInfoJobSummary.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.valueBasicInfoJobSummary.StyleName = "DataField";
//            // 
//            // lblBasicInfoJobSummary
//            // 
//            resources.ApplyResources(this.lblBasicInfoJobSummary, "lblBasicInfoJobSummary");
//            this.lblBasicInfoJobSummary.Name = "lblBasicInfoJobSummary";
//            this.lblBasicInfoJobSummary.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblBasicInfoJobSummary.StyleName = "TitleLevel2";
//            // 
//            // valueBasicInfoNodeName
//            // 
//            this.valueBasicInfoNodeName.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
//            new DevExpress.XtraReports.UI.XRBinding("Text", null, "NodeName")});
//            resources.ApplyResources(this.valueBasicInfoNodeName, "valueBasicInfoNodeName");
//            this.valueBasicInfoNodeName.Name = "valueBasicInfoNodeName";
//            this.valueBasicInfoNodeName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.valueBasicInfoNodeName.StyleName = "DataField";
//            // 
//            // valueBasicInfoNodeType
//            // 
//            this.valueBasicInfoNodeType.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
//            new DevExpress.XtraReports.UI.XRBinding("Text", null, "NodeType")});
//            resources.ApplyResources(this.valueBasicInfoNodeType, "valueBasicInfoNodeType");
//            this.valueBasicInfoNodeType.Name = "valueBasicInfoNodeType";
//            this.valueBasicInfoNodeType.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.valueBasicInfoNodeType.StyleName = "TitleLevel2";
//            // 
//            // valueBasicInfoReportingTo
//            // 
//            this.valueBasicInfoReportingTo.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
//            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ReportingTo")});
//            resources.ApplyResources(this.valueBasicInfoReportingTo, "valueBasicInfoReportingTo");
//            this.valueBasicInfoReportingTo.Name = "valueBasicInfoReportingTo";
//            this.valueBasicInfoReportingTo.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.valueBasicInfoReportingTo.StyleName = "DataField";
//            // 
//            // lblBasicInfoReportingTo
//            // 
//            resources.ApplyResources(this.lblBasicInfoReportingTo, "lblBasicInfoReportingTo");
//            this.lblBasicInfoReportingTo.Name = "lblBasicInfoReportingTo";
//            this.lblBasicInfoReportingTo.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblBasicInfoReportingTo.StyleName = "TitleLevel2";
//            // 
//            // valueBasicInfoJobTilte
//            // 
//            this.valueBasicInfoJobTilte.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
//            new DevExpress.XtraReports.UI.XRBinding("Text", null, "JobTitle")});
//            resources.ApplyResources(this.valueBasicInfoJobTilte, "valueBasicInfoJobTilte");
//            this.valueBasicInfoJobTilte.Name = "valueBasicInfoJobTilte";
//            this.valueBasicInfoJobTilte.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.valueBasicInfoJobTilte.StyleName = "DataField";
//            // 
//            // lblBasicInfoJobTilte
//            // 
//            resources.ApplyResources(this.lblBasicInfoJobTilte, "lblBasicInfoJobTilte");
//            this.lblBasicInfoJobTilte.Name = "lblBasicInfoJobTilte";
//            this.lblBasicInfoJobTilte.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblBasicInfoJobTilte.StyleName = "TitleLevel2";
//            // 
//            // lblBasicInfoJobDescribtionTemplate
//            // 
//            resources.ApplyResources(this.lblBasicInfoJobDescribtionTemplate, "lblBasicInfoJobDescribtionTemplate");
//            this.lblBasicInfoJobDescribtionTemplate.Name = "lblBasicInfoJobDescribtionTemplate";
//            this.lblBasicInfoJobDescribtionTemplate.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblBasicInfoJobDescribtionTemplate.StyleName = "TitleLevel1";
//            this.lblBasicInfoJobDescribtionTemplate.StylePriority.UseTextAlignment = false;
//            // 
//            // TopMargin
//            // 
//            resources.ApplyResources(this.TopMargin, "TopMargin");
//            this.TopMargin.Name = "TopMargin";
//            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
//            // 
//            // BottomMargin
//            // 
//            resources.ApplyResources(this.BottomMargin, "BottomMargin");
//            this.BottomMargin.Name = "BottomMargin";
//            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
//            // 
//            // bindingSourceJobDescriptionTemplateDTO
//            // 
//            this.bindingSourceJobDescriptionTemplateDTO.DataSource = typeof(Service.DTO.JobDesc.JobDescriptionTemplate);
//            // 
//            // RolesReportBand
//            // 
//            this.RolesReportBand.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
//            this.RolesDetailBand,
//            this.ResponsibilitiesReportBand,
//            this.RolesHeaderBand});
//            this.RolesReportBand.DataMember = "Roles";
//            this.RolesReportBand.DataSource = this.bindingSourceJobDescriptionTemplateDTO;
//            resources.ApplyResources(this.RolesReportBand, "RolesReportBand");
//            this.RolesReportBand.Level = 4;
//            this.RolesReportBand.Name = "RolesReportBand";
//            // 
//            // RolesDetailBand
//            // 
//            this.RolesDetailBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
//            this.valueRolesJobTitleAndWeight});
//            resources.ApplyResources(this.RolesDetailBand, "RolesDetailBand");
//            this.RolesDetailBand.Name = "RolesDetailBand";
//            // 
//            // valueRolesJobTitleAndWeight
//            // 
//            resources.ApplyResources(this.valueRolesJobTitleAndWeight, "valueRolesJobTitleAndWeight");
//            this.valueRolesJobTitleAndWeight.Name = "valueRolesJobTitleAndWeight";
//            this.valueRolesJobTitleAndWeight.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.valueRolesJobTitleAndWeight.StyleName = "TitleLevel2";
//            // 
//            // ResponsibilitiesReportBand
//            // 
//            this.ResponsibilitiesReportBand.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
//            this.ResponsibilitiesDetailBand,
//            this.ResponsibilitiesHeaderBand});
//            this.ResponsibilitiesReportBand.DataMember = "Roles.Responsibilities";
//            this.ResponsibilitiesReportBand.DataSource = this.bindingSourceJobDescriptionTemplateDTO;
//            resources.ApplyResources(this.ResponsibilitiesReportBand, "ResponsibilitiesReportBand");
//            this.ResponsibilitiesReportBand.Level = 0;
//            this.ResponsibilitiesReportBand.Name = "ResponsibilitiesReportBand";
//            // 
//            // ResponsibilitiesDetailBand
//            // 
//            this.ResponsibilitiesDetailBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
//            this.valueResponsibilitiesKpi,
//            this.valueResponsibilitiesWeight,
//            this.valueResponsibilitiesDescription});
//            resources.ApplyResources(this.ResponsibilitiesDetailBand, "ResponsibilitiesDetailBand");
//            this.ResponsibilitiesDetailBand.Name = "ResponsibilitiesDetailBand";
//            // 
//            // valueResponsibilitiesKpi
//            // 
//            this.valueResponsibilitiesKpi.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
//            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Roles.Responsibilities.ResponsibilityKpis")});
//            resources.ApplyResources(this.valueResponsibilitiesKpi, "valueResponsibilitiesKpi");
//            this.valueResponsibilitiesKpi.Name = "valueResponsibilitiesKpi";
//            this.valueResponsibilitiesKpi.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.valueResponsibilitiesKpi.StyleName = "DataField";
//            // 
//            // valueResponsibilitiesWeight
//            // 
//            this.valueResponsibilitiesWeight.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
//            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Roles.Responsibilities.Weight")});
//            resources.ApplyResources(this.valueResponsibilitiesWeight, "valueResponsibilitiesWeight");
//            this.valueResponsibilitiesWeight.Name = "valueResponsibilitiesWeight";
//            this.valueResponsibilitiesWeight.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.valueResponsibilitiesWeight.StyleName = "DataField";
//            // 
//            // valueResponsibilitiesDescription
//            // 
//            this.valueResponsibilitiesDescription.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
//            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Roles.Responsibilities.Description")});
//            resources.ApplyResources(this.valueResponsibilitiesDescription, "valueResponsibilitiesDescription");
//            this.valueResponsibilitiesDescription.Name = "valueResponsibilitiesDescription";
//            this.valueResponsibilitiesDescription.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.valueResponsibilitiesDescription.StyleName = "DataField";
//            // 
//            // ResponsibilitiesHeaderBand
//            // 
//            this.ResponsibilitiesHeaderBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
//            this.lblResponsibilitiesKpi,
//            this.lblResponsibilitiesWeight,
//            this.lblResponsibilitiesDescription});
//            resources.ApplyResources(this.ResponsibilitiesHeaderBand, "ResponsibilitiesHeaderBand");
//            this.ResponsibilitiesHeaderBand.Name = "ResponsibilitiesHeaderBand";
//            // 
//            // lblResponsibilitiesKpi
//            // 
//            resources.ApplyResources(this.lblResponsibilitiesKpi, "lblResponsibilitiesKpi");
//            this.lblResponsibilitiesKpi.Name = "lblResponsibilitiesKpi";
//            this.lblResponsibilitiesKpi.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblResponsibilitiesKpi.StyleName = "TitleLevel3";
//            // 
//            // lblResponsibilitiesWeight
//            // 
//            resources.ApplyResources(this.lblResponsibilitiesWeight, "lblResponsibilitiesWeight");
//            this.lblResponsibilitiesWeight.Name = "lblResponsibilitiesWeight";
//            this.lblResponsibilitiesWeight.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblResponsibilitiesWeight.StyleName = "TitleLevel3";
//            // 
//            // lblResponsibilitiesDescription
//            // 
//            resources.ApplyResources(this.lblResponsibilitiesDescription, "lblResponsibilitiesDescription");
//            this.lblResponsibilitiesDescription.Name = "lblResponsibilitiesDescription";
//            this.lblResponsibilitiesDescription.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblResponsibilitiesDescription.StyleName = "TitleLevel3";
//            // 
//            // RolesHeaderBand
//            // 
//            this.RolesHeaderBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
//            this.lblRolesAndResponsibilities});
//            resources.ApplyResources(this.RolesHeaderBand, "RolesHeaderBand");
//            this.RolesHeaderBand.Name = "RolesHeaderBand";
//            // 
//            // lblRolesAndResponsibilities
//            // 
//            resources.ApplyResources(this.lblRolesAndResponsibilities, "lblRolesAndResponsibilities");
//            this.lblRolesAndResponsibilities.Name = "lblRolesAndResponsibilities";
//            this.lblRolesAndResponsibilities.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblRolesAndResponsibilities.StyleName = "TitleLevel1";
//            // 
//            // AuthoritiesReportBand
//            // 
//            this.AuthoritiesReportBand.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
//            this.AuthoritiesDetailBand,
//            this.AuthoritiesHeaderBand});
//            this.AuthoritiesReportBand.DataMember = "Authorities";
//            this.AuthoritiesReportBand.DataSource = this.bindingSourceJobDescriptionTemplateDTO;
//            resources.ApplyResources(this.AuthoritiesReportBand, "AuthoritiesReportBand");
//            this.AuthoritiesReportBand.Level = 3;
//            this.AuthoritiesReportBand.Name = "AuthoritiesReportBand";
//            // 
//            // AuthoritiesDetailBand
//            // 
//            this.AuthoritiesDetailBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
//            this.lblAuthoritiesTitle});
//            resources.ApplyResources(this.AuthoritiesDetailBand, "AuthoritiesDetailBand");
//            this.AuthoritiesDetailBand.Name = "AuthoritiesDetailBand";
//            // 
//            // lblAuthoritiesTitle
//            // 
//            this.lblAuthoritiesTitle.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
//            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Authorities.Title")});
//            resources.ApplyResources(this.lblAuthoritiesTitle, "lblAuthoritiesTitle");
//            this.lblAuthoritiesTitle.Name = "lblAuthoritiesTitle";
//            this.lblAuthoritiesTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblAuthoritiesTitle.StyleName = "DataField";
//            // 
//            // AuthoritiesHeaderBand
//            // 
//            this.AuthoritiesHeaderBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
//            this.lblAuthoritiesAuthorityLevel,
//            this.lblAuthoritiesOperatingBudget,
//            this.lblAuthoritiesDimensions,
//            this.xrLabel1});
//            resources.ApplyResources(this.AuthoritiesHeaderBand, "AuthoritiesHeaderBand");
//            this.AuthoritiesHeaderBand.KeepTogether = true;
//            this.AuthoritiesHeaderBand.Name = "AuthoritiesHeaderBand";
//            // 
//            // lblAuthoritiesAuthorityLevel
//            // 
//            resources.ApplyResources(this.lblAuthoritiesAuthorityLevel, "lblAuthoritiesAuthorityLevel");
//            this.lblAuthoritiesAuthorityLevel.Name = "lblAuthoritiesAuthorityLevel";
//            this.lblAuthoritiesAuthorityLevel.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblAuthoritiesAuthorityLevel.StyleName = "TitleLevel3";
//            // 
//            // lblAuthoritiesOperatingBudget
//            // 
//            resources.ApplyResources(this.lblAuthoritiesOperatingBudget, "lblAuthoritiesOperatingBudget");
//            this.lblAuthoritiesOperatingBudget.Name = "lblAuthoritiesOperatingBudget";
//            this.lblAuthoritiesOperatingBudget.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblAuthoritiesOperatingBudget.StyleName = "TitleLevel2";
//            // 
//            // lblAuthoritiesDimensions
//            // 
//            resources.ApplyResources(this.lblAuthoritiesDimensions, "lblAuthoritiesDimensions");
//            this.lblAuthoritiesDimensions.Name = "lblAuthoritiesDimensions";
//            this.lblAuthoritiesDimensions.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblAuthoritiesDimensions.StyleName = "TitleLevel1";
//            // 
//            // xrLabel1
//            // 
//            this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
//            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OperatingBudget")});
//            resources.ApplyResources(this.xrLabel1, "xrLabel1");
//            this.xrLabel1.Name = "xrLabel1";
//            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.xrLabel1.StyleName = "DataField";
//            // 
//            // JobRelationshipsReportBand
//            // 
//            this.JobRelationshipsReportBand.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
//            this.JobRelationshipsDetailBand});
//            resources.ApplyResources(this.JobRelationshipsReportBand, "JobRelationshipsReportBand");
//            this.JobRelationshipsReportBand.Level = 2;
//            this.JobRelationshipsReportBand.Name = "JobRelationshipsReportBand";
//            // 
//            // JobRelationshipsDetailBand
//            // 
//            this.JobRelationshipsDetailBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
//            this.valueJobRelationshipsExternal,
//            this.lblJobRelationshipsExternal,
//            this.valueJobRelationshipsInternal,
//            this.lblJobRelationshipsInternal,
//            this.lblJobRelationships});
//            resources.ApplyResources(this.JobRelationshipsDetailBand, "JobRelationshipsDetailBand");
//            this.JobRelationshipsDetailBand.KeepTogether = true;
//            this.JobRelationshipsDetailBand.Name = "JobRelationshipsDetailBand";
//            // 
//            // valueJobRelationshipsExternal
//            // 
//            this.valueJobRelationshipsExternal.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
//            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ExternalRelationship")});
//            resources.ApplyResources(this.valueJobRelationshipsExternal, "valueJobRelationshipsExternal");
//            this.valueJobRelationshipsExternal.Name = "valueJobRelationshipsExternal";
//            this.valueJobRelationshipsExternal.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.valueJobRelationshipsExternal.StyleName = "DataField";
//            // 
//            // lblJobRelationshipsExternal
//            // 
//            resources.ApplyResources(this.lblJobRelationshipsExternal, "lblJobRelationshipsExternal");
//            this.lblJobRelationshipsExternal.Name = "lblJobRelationshipsExternal";
//            this.lblJobRelationshipsExternal.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblJobRelationshipsExternal.StyleName = "TitleLevel2";
//            // 
//            // valueJobRelationshipsInternal
//            // 
//            this.valueJobRelationshipsInternal.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
//            new DevExpress.XtraReports.UI.XRBinding("Text", null, "InternalRelationship")});
//            resources.ApplyResources(this.valueJobRelationshipsInternal, "valueJobRelationshipsInternal");
//            this.valueJobRelationshipsInternal.Name = "valueJobRelationshipsInternal";
//            this.valueJobRelationshipsInternal.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.valueJobRelationshipsInternal.StyleName = "DataField";
//            // 
//            // lblJobRelationshipsInternal
//            // 
//            resources.ApplyResources(this.lblJobRelationshipsInternal, "lblJobRelationshipsInternal");
//            this.lblJobRelationshipsInternal.Name = "lblJobRelationshipsInternal";
//            this.lblJobRelationshipsInternal.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblJobRelationshipsInternal.StyleName = "TitleLevel2";
//            // 
//            // lblJobRelationships
//            // 
//            resources.ApplyResources(this.lblJobRelationships, "lblJobRelationships");
//            this.lblJobRelationships.Name = "lblJobRelationships";
//            this.lblJobRelationships.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblJobRelationships.StyleName = "TitleLevel1";
//            // 
//            // QualificationsReportBand
//            // 
//            this.QualificationsReportBand.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
//            this.QualificationsDetailBand,
//            this.EducationReportBand,
//            this.ExperienceReportBand,
//            this.LanguagesReportBand,
//            this.PCSkillsReportBand,
//            this.OtherSkillsReportBand,
//            this.QualificationsHeaderBand});
//            resources.ApplyResources(this.QualificationsReportBand, "QualificationsReportBand");
//            this.QualificationsReportBand.Level = 1;
//            this.QualificationsReportBand.Name = "QualificationsReportBand";
//            // 
//            // QualificationsDetailBand
//            // 
//            resources.ApplyResources(this.QualificationsDetailBand, "QualificationsDetailBand");
//            this.QualificationsDetailBand.Name = "QualificationsDetailBand";
//            // 
//            // EducationReportBand
//            // 
//            this.EducationReportBand.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
//            this.EducationDetailBand,
//            this.EducationHeaderBand});
//            this.EducationReportBand.DataMember = "Educations";
//            resources.ApplyResources(this.EducationReportBand, "EducationReportBand");
//            this.EducationReportBand.Level = 0;
//            this.EducationReportBand.Name = "EducationReportBand";
//            // 
//            // EducationDetailBand
//            // 
//            this.EducationDetailBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
//            this.valueEducationType,
//            this.valueEducationMajor});
//            resources.ApplyResources(this.EducationDetailBand, "EducationDetailBand");
//            this.EducationDetailBand.Name = "EducationDetailBand";
//            // 
//            // valueEducationType
//            // 
//            resources.ApplyResources(this.valueEducationType, "valueEducationType");
//            this.valueEducationType.Name = "valueEducationType";
//            this.valueEducationType.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.valueEducationType.StyleName = "DataField";
//            // 
//            // valueEducationMajor
//            // 
//            this.valueEducationMajor.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
//            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Educations.Major.Name")});
//            resources.ApplyResources(this.valueEducationMajor, "valueEducationMajor");
//            this.valueEducationMajor.Name = "valueEducationMajor";
//            this.valueEducationMajor.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.valueEducationMajor.StyleName = "DataField";
//            // 
//            // EducationHeaderBand
//            // 
//            this.EducationHeaderBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
//            this.lblEducationType,
//            this.lblEducationMajor,
//            this.lblEducation});
//            resources.ApplyResources(this.EducationHeaderBand, "EducationHeaderBand");
//            this.EducationHeaderBand.KeepTogether = true;
//            this.EducationHeaderBand.Name = "EducationHeaderBand";
//            // 
//            // lblEducationType
//            // 
//            resources.ApplyResources(this.lblEducationType, "lblEducationType");
//            this.lblEducationType.Name = "lblEducationType";
//            this.lblEducationType.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblEducationType.StyleName = "TitleLevel2";
//            // 
//            // lblEducationMajor
//            // 
//            resources.ApplyResources(this.lblEducationMajor, "lblEducationMajor");
//            this.lblEducationMajor.Name = "lblEducationMajor";
//            this.lblEducationMajor.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblEducationMajor.StyleName = "TitleLevel2";
//            // 
//            // lblEducation
//            // 
//            resources.ApplyResources(this.lblEducation, "lblEducation");
//            this.lblEducation.Name = "lblEducation";
//            this.lblEducation.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblEducation.StyleName = "TitleLevel1";
//            // 
//            // ExperienceReportBand
//            // 
//            this.ExperienceReportBand.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
//            this.ExperienceDetailBand,
//            this.ExperienceHeaserBand});
//            this.ExperienceReportBand.DataMember = "Experiences";
//            resources.ApplyResources(this.ExperienceReportBand, "ExperienceReportBand");
//            this.ExperienceReportBand.Level = 1;
//            this.ExperienceReportBand.Name = "ExperienceReportBand";
//            // 
//            // ExperienceDetailBand
//            // 
//            this.ExperienceDetailBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
//            this.valueExperienceCareerLevel,
//            this.valueExperienceIndustry});
//            resources.ApplyResources(this.ExperienceDetailBand, "ExperienceDetailBand");
//            this.ExperienceDetailBand.Name = "ExperienceDetailBand";
//            // 
//            // valueExperienceCareerLevel
//            // 
//            resources.ApplyResources(this.valueExperienceCareerLevel, "valueExperienceCareerLevel");
//            this.valueExperienceCareerLevel.Name = "valueExperienceCareerLevel";
//            this.valueExperienceCareerLevel.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.valueExperienceCareerLevel.StyleName = "DataField";
//            // 
//            // valueExperienceIndustry
//            // 
//            resources.ApplyResources(this.valueExperienceIndustry, "valueExperienceIndustry");
//            this.valueExperienceIndustry.Name = "valueExperienceIndustry";
//            this.valueExperienceIndustry.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.valueExperienceIndustry.StyleName = "DataField";
//            // 
//            // ExperienceHeaserBand
//            // 
//            this.ExperienceHeaserBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
//            this.lblExperience,
//            this.lblExperienceIndustry,
//            this.lblExperienceCareerLevel});
//            resources.ApplyResources(this.ExperienceHeaserBand, "ExperienceHeaserBand");
//            this.ExperienceHeaserBand.KeepTogether = true;
//            this.ExperienceHeaserBand.Name = "ExperienceHeaserBand";
//            // 
//            // lblExperience
//            // 
//            resources.ApplyResources(this.lblExperience, "lblExperience");
//            this.lblExperience.Name = "lblExperience";
//            this.lblExperience.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblExperience.StyleName = "TitleLevel1";
//            // 
//            // lblExperienceIndustry
//            // 
//            resources.ApplyResources(this.lblExperienceIndustry, "lblExperienceIndustry");
//            this.lblExperienceIndustry.Name = "lblExperienceIndustry";
//            this.lblExperienceIndustry.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblExperienceIndustry.StyleName = "TitleLevel2";
//            // 
//            // lblExperienceCareerLevel
//            // 
//            resources.ApplyResources(this.lblExperienceCareerLevel, "lblExperienceCareerLevel");
//            this.lblExperienceCareerLevel.Name = "lblExperienceCareerLevel";
//            this.lblExperienceCareerLevel.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblExperienceCareerLevel.StyleName = "TitleLevel2";
//            // 
//            // LanguagesReportBand
//            // 
//            this.LanguagesReportBand.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
//            this.LanguagesDetailBand,
//            this.LanguagesHeaderBand});
//            this.LanguagesReportBand.DataMember = "Languages";
//            resources.ApplyResources(this.LanguagesReportBand, "LanguagesReportBand");
//            this.LanguagesReportBand.Level = 2;
//            this.LanguagesReportBand.Name = "LanguagesReportBand";
//            // 
//            // LanguagesDetailBand
//            // 
//            this.LanguagesDetailBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
//            this.valueLanguageReading,
//            this.valueLanguageSpeaking,
//            this.valueLanguageWriting,
//            this.valueLanguageName,
//            this.valueLanguageListening});
//            resources.ApplyResources(this.LanguagesDetailBand, "LanguagesDetailBand");
//            this.LanguagesDetailBand.Name = "LanguagesDetailBand";
//            // 
//            // valueLanguageReading
//            // 
//            resources.ApplyResources(this.valueLanguageReading, "valueLanguageReading");
//            this.valueLanguageReading.Name = "valueLanguageReading";
//            this.valueLanguageReading.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.valueLanguageReading.StyleName = "DataField";
//            // 
//            // valueLanguageSpeaking
//            // 
//            resources.ApplyResources(this.valueLanguageSpeaking, "valueLanguageSpeaking");
//            this.valueLanguageSpeaking.Name = "valueLanguageSpeaking";
//            this.valueLanguageSpeaking.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.valueLanguageSpeaking.StyleName = "DataField";
//            // 
//            // valueLanguageWriting
//            // 
//            resources.ApplyResources(this.valueLanguageWriting, "valueLanguageWriting");
//            this.valueLanguageWriting.Name = "valueLanguageWriting";
//            this.valueLanguageWriting.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.valueLanguageWriting.StyleName = "DataField";
//            // 
//            // valueLanguageName
//            // 
//            resources.ApplyResources(this.valueLanguageName, "valueLanguageName");
//            this.valueLanguageName.Name = "valueLanguageName";
//            this.valueLanguageName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.valueLanguageName.StyleName = "DataField";
//            // 
//            // valueLanguageListening
//            // 
//            resources.ApplyResources(this.valueLanguageListening, "valueLanguageListening");
//            this.valueLanguageListening.Name = "valueLanguageListening";
//            this.valueLanguageListening.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.valueLanguageListening.StyleName = "DataField";
//            // 
//            // LanguagesHeaderBand
//            // 
//            this.LanguagesHeaderBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
//            this.lblLanguageListening,
//            this.lblLanguageReading,
//            this.lblLanguageWriting,
//            this.lblLanguageSpeaking,
//            this.lblLanguageName,
//            this.lblLanguage});
//            resources.ApplyResources(this.LanguagesHeaderBand, "LanguagesHeaderBand");
//            this.LanguagesHeaderBand.KeepTogether = true;
//            this.LanguagesHeaderBand.Name = "LanguagesHeaderBand";
//            // 
//            // lblLanguageListening
//            // 
//            resources.ApplyResources(this.lblLanguageListening, "lblLanguageListening");
//            this.lblLanguageListening.Name = "lblLanguageListening";
//            this.lblLanguageListening.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblLanguageListening.StyleName = "TitleLevel2";
//            // 
//            // lblLanguageReading
//            // 
//            resources.ApplyResources(this.lblLanguageReading, "lblLanguageReading");
//            this.lblLanguageReading.Name = "lblLanguageReading";
//            this.lblLanguageReading.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblLanguageReading.StyleName = "TitleLevel2";
//            // 
//            // lblLanguageWriting
//            // 
//            resources.ApplyResources(this.lblLanguageWriting, "lblLanguageWriting");
//            this.lblLanguageWriting.Name = "lblLanguageWriting";
//            this.lblLanguageWriting.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblLanguageWriting.StyleName = "TitleLevel2";
//            // 
//            // lblLanguageSpeaking
//            // 
//            resources.ApplyResources(this.lblLanguageSpeaking, "lblLanguageSpeaking");
//            this.lblLanguageSpeaking.Name = "lblLanguageSpeaking";
//            this.lblLanguageSpeaking.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblLanguageSpeaking.StyleName = "TitleLevel2";
//            // 
//            // lblLanguageName
//            // 
//            resources.ApplyResources(this.lblLanguageName, "lblLanguageName");
//            this.lblLanguageName.Name = "lblLanguageName";
//            this.lblLanguageName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblLanguageName.StyleName = "TitleLevel2";
//            // 
//            // lblLanguage
//            // 
//            resources.ApplyResources(this.lblLanguage, "lblLanguage");
//            this.lblLanguage.Name = "lblLanguage";
//            this.lblLanguage.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblLanguage.StyleName = "TitleLevel1";
//            // 
//            // PCSkillsReportBand
//            // 
//            this.PCSkillsReportBand.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
//            this.PCSkillsDetailBand,
//            this.PCSkillsHeaderBand});
//            this.PCSkillsReportBand.DataMember = "PCSkills";
//            resources.ApplyResources(this.PCSkillsReportBand, "PCSkillsReportBand");
//            this.PCSkillsReportBand.Level = 3;
//            this.PCSkillsReportBand.Name = "PCSkillsReportBand";
//            // 
//            // PCSkillsDetailBand
//            // 
//            this.PCSkillsDetailBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
//            this.valuePCSkillsType,
//            this.valuePCSkillsLevel});
//            resources.ApplyResources(this.PCSkillsDetailBand, "PCSkillsDetailBand");
//            this.PCSkillsDetailBand.Name = "PCSkillsDetailBand";
//            // 
//            // valuePCSkillsType
//            // 
//            resources.ApplyResources(this.valuePCSkillsType, "valuePCSkillsType");
//            this.valuePCSkillsType.Name = "valuePCSkillsType";
//            this.valuePCSkillsType.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.valuePCSkillsType.StyleName = "DataField";
//            // 
//            // valuePCSkillsLevel
//            // 
//            resources.ApplyResources(this.valuePCSkillsLevel, "valuePCSkillsLevel");
//            this.valuePCSkillsLevel.Name = "valuePCSkillsLevel";
//            this.valuePCSkillsLevel.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.valuePCSkillsLevel.StyleName = "DataField";
//            // 
//            // PCSkillsHeaderBand
//            // 
//            this.PCSkillsHeaderBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
//            this.lblPCSkillsLevel,
//            this.lblPCSkillsType,
//            this.lblPCSkills});
//            resources.ApplyResources(this.PCSkillsHeaderBand, "PCSkillsHeaderBand");
//            this.PCSkillsHeaderBand.KeepTogether = true;
//            this.PCSkillsHeaderBand.Name = "PCSkillsHeaderBand";
//            // 
//            // lblPCSkillsLevel
//            // 
//            resources.ApplyResources(this.lblPCSkillsLevel, "lblPCSkillsLevel");
//            this.lblPCSkillsLevel.Name = "lblPCSkillsLevel";
//            this.lblPCSkillsLevel.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblPCSkillsLevel.StyleName = "TitleLevel2";
//            // 
//            // lblPCSkillsType
//            // 
//            resources.ApplyResources(this.lblPCSkillsType, "lblPCSkillsType");
//            this.lblPCSkillsType.Name = "lblPCSkillsType";
//            this.lblPCSkillsType.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblPCSkillsType.StyleName = "TitleLevel2";
//            // 
//            // lblPCSkills
//            // 
//            resources.ApplyResources(this.lblPCSkills, "lblPCSkills");
//            this.lblPCSkills.Name = "lblPCSkills";
//            this.lblPCSkills.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblPCSkills.StyleName = "TitleLevel1";
//            // 
//            // OtherSkillsReportBand
//            // 
//            this.OtherSkillsReportBand.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
//            this.OtherSkillsDetailBand,
//            this.OtherSkillsHeaderBand});
//            this.OtherSkillsReportBand.DataMember = "OtherSkills";
//            resources.ApplyResources(this.OtherSkillsReportBand, "OtherSkillsReportBand");
//            this.OtherSkillsReportBand.Level = 4;
//            this.OtherSkillsReportBand.Name = "OtherSkillsReportBand";
//            // 
//            // OtherSkillsDetailBand
//            // 
//            this.OtherSkillsDetailBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
//            this.valueOtherSkillsLevel,
//            this.valueOtherSkillsType});
//            resources.ApplyResources(this.OtherSkillsDetailBand, "OtherSkillsDetailBand");
//            this.OtherSkillsDetailBand.Name = "OtherSkillsDetailBand";
//            // 
//            // valueOtherSkillsLevel
//            // 
//            resources.ApplyResources(this.valueOtherSkillsLevel, "valueOtherSkillsLevel");
//            this.valueOtherSkillsLevel.Name = "valueOtherSkillsLevel";
//            this.valueOtherSkillsLevel.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.valueOtherSkillsLevel.StyleName = "DataField";
//            // 
//            // valueOtherSkillsType
//            // 
//            resources.ApplyResources(this.valueOtherSkillsType, "valueOtherSkillsType");
//            this.valueOtherSkillsType.Name = "valueOtherSkillsType";
//            this.valueOtherSkillsType.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.valueOtherSkillsType.StyleName = "DataField";
//            // 
//            // OtherSkillsHeaderBand
//            // 
//            this.OtherSkillsHeaderBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
//            this.lblOtherSkillsLevel,
//            this.lblOtherSkillsType,
//            this.lblOtherSkills});
//            resources.ApplyResources(this.OtherSkillsHeaderBand, "OtherSkillsHeaderBand");
//            this.OtherSkillsHeaderBand.KeepTogether = true;
//            this.OtherSkillsHeaderBand.Name = "OtherSkillsHeaderBand";
//            // 
//            // lblOtherSkillsLevel
//            // 
//            resources.ApplyResources(this.lblOtherSkillsLevel, "lblOtherSkillsLevel");
//            this.lblOtherSkillsLevel.Name = "lblOtherSkillsLevel";
//            this.lblOtherSkillsLevel.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblOtherSkillsLevel.StyleName = "TitleLevel2";
//            // 
//            // lblOtherSkillsType
//            // 
//            resources.ApplyResources(this.lblOtherSkillsType, "lblOtherSkillsType");
//            this.lblOtherSkillsType.Name = "lblOtherSkillsType";
//            this.lblOtherSkillsType.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblOtherSkillsType.StyleName = "TitleLevel2";
//            // 
//            // lblOtherSkills
//            // 
//            resources.ApplyResources(this.lblOtherSkills, "lblOtherSkills");
//            this.lblOtherSkills.Name = "lblOtherSkills";
//            this.lblOtherSkills.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblOtherSkills.StyleName = "TitleLevel1";
//            // 
//            // QualificationsHeaderBand
//            // 
//            this.QualificationsHeaderBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
//            this.lblQualifications});
//            resources.ApplyResources(this.QualificationsHeaderBand, "QualificationsHeaderBand");
//            this.QualificationsHeaderBand.Name = "QualificationsHeaderBand";
//            // 
//            // lblQualifications
//            // 
//            resources.ApplyResources(this.lblQualifications, "lblQualifications");
//            this.lblQualifications.Name = "lblQualifications";
//            this.lblQualifications.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblQualifications.StyleName = "TitleLevel1";
//            // 
//            // Detail
//            // 
//            resources.ApplyResources(this.Detail, "Detail");
//            this.Detail.KeepTogether = true;
//            this.Detail.Name = "Detail";
//            // 
//            // CompetenciesReportBand
//            // 
//            this.CompetenciesReportBand.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
//            this.CompetenciesDetailBand,
//            this.CompetenciesTypeGroupHeaderBand,
//            this.CompetenciesHeaderBand});
//            this.CompetenciesReportBand.DataMember = "Comptencies";
//            this.CompetenciesReportBand.DataSource = this.bindingSourceJobDescriptionTemplateDTO;
//            resources.ApplyResources(this.CompetenciesReportBand, "CompetenciesReportBand");
//            this.CompetenciesReportBand.Level = 0;
//            this.CompetenciesReportBand.Name = "CompetenciesReportBand";
//            // 
//            // CompetenciesDetailBand
//            // 
//            this.CompetenciesDetailBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
//            this.ValueCompetenciesDescription,
//            this.ValueCompetenciesName,
//            this.valueCompetenciesLevel,
//            this.valueCompetenciesWeight});
//            resources.ApplyResources(this.CompetenciesDetailBand, "CompetenciesDetailBand");
//            this.CompetenciesDetailBand.Name = "CompetenciesDetailBand";
//            // 
//            // ValueCompetenciesDescription
//            // 
//            resources.ApplyResources(this.ValueCompetenciesDescription, "ValueCompetenciesDescription");
//            this.ValueCompetenciesDescription.Name = "ValueCompetenciesDescription";
//            this.ValueCompetenciesDescription.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.ValueCompetenciesDescription.StyleName = "DataField";
//            // 
//            // ValueCompetenciesName
//            // 
//            resources.ApplyResources(this.ValueCompetenciesName, "ValueCompetenciesName");
//            this.ValueCompetenciesName.Name = "ValueCompetenciesName";
//            this.ValueCompetenciesName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.ValueCompetenciesName.StyleName = "DataField";
//            // 
//            // valueCompetenciesLevel
//            // 
//            resources.ApplyResources(this.valueCompetenciesLevel, "valueCompetenciesLevel");
//            this.valueCompetenciesLevel.Name = "valueCompetenciesLevel";
//            this.valueCompetenciesLevel.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.valueCompetenciesLevel.StyleName = "DataField";
//            // 
//            // valueCompetenciesWeight
//            // 
//            resources.ApplyResources(this.valueCompetenciesWeight, "valueCompetenciesWeight");
//            this.valueCompetenciesWeight.Name = "valueCompetenciesWeight";
//            this.valueCompetenciesWeight.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.valueCompetenciesWeight.StyleName = "DataField";
//            // 
//            // CompetenciesTypeGroupHeaderBand
//            // 
//            this.CompetenciesTypeGroupHeaderBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
//            this.valueCompetenciesTypeGroup,
//            this.lblCompetenciesWeight,
//            this.lblCompetenciesLevel,
//            this.lblCompetenciesDescription,
//            this.lblCompetenciesName,
//            this.lblCompetenciesTypeGroup});
//            resources.ApplyResources(this.CompetenciesTypeGroupHeaderBand, "CompetenciesTypeGroupHeaderBand");
//            this.CompetenciesTypeGroupHeaderBand.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
//            new DevExpress.XtraReports.UI.GroupField("Type.Name", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
//            this.CompetenciesTypeGroupHeaderBand.KeepTogether = true;
//            this.CompetenciesTypeGroupHeaderBand.Name = "CompetenciesTypeGroupHeaderBand";
//            // 
//            // valueCompetenciesTypeGroup
//            // 
//            resources.ApplyResources(this.valueCompetenciesTypeGroup, "valueCompetenciesTypeGroup");
//            this.valueCompetenciesTypeGroup.Name = "valueCompetenciesTypeGroup";
//            this.valueCompetenciesTypeGroup.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.valueCompetenciesTypeGroup.StyleName = "TitleLevel2";
//            // 
//            // lblCompetenciesWeight
//            // 
//            resources.ApplyResources(this.lblCompetenciesWeight, "lblCompetenciesWeight");
//            this.lblCompetenciesWeight.Name = "lblCompetenciesWeight";
//            this.lblCompetenciesWeight.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblCompetenciesWeight.StyleName = "TitleLevel3";
//            // 
//            // lblCompetenciesLevel
//            // 
//            resources.ApplyResources(this.lblCompetenciesLevel, "lblCompetenciesLevel");
//            this.lblCompetenciesLevel.Name = "lblCompetenciesLevel";
//            this.lblCompetenciesLevel.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblCompetenciesLevel.StyleName = "TitleLevel3";
//            // 
//            // lblCompetenciesDescription
//            // 
//            resources.ApplyResources(this.lblCompetenciesDescription, "lblCompetenciesDescription");
//            this.lblCompetenciesDescription.Name = "lblCompetenciesDescription";
//            this.lblCompetenciesDescription.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblCompetenciesDescription.StyleName = "TitleLevel3";
//            // 
//            // lblCompetenciesName
//            // 
//            resources.ApplyResources(this.lblCompetenciesName, "lblCompetenciesName");
//            this.lblCompetenciesName.Name = "lblCompetenciesName";
//            this.lblCompetenciesName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblCompetenciesName.StyleName = "TitleLevel3";
//            // 
//            // lblCompetenciesTypeGroup
//            // 
//            resources.ApplyResources(this.lblCompetenciesTypeGroup, "lblCompetenciesTypeGroup");
//            this.lblCompetenciesTypeGroup.Name = "lblCompetenciesTypeGroup";
//            this.lblCompetenciesTypeGroup.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblCompetenciesTypeGroup.StyleName = "TitleLevel2";
//            // 
//            // CompetenciesHeaderBand
//            // 
//            this.CompetenciesHeaderBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
//            this.lblCompetencies});
//            resources.ApplyResources(this.CompetenciesHeaderBand, "CompetenciesHeaderBand");
//            this.CompetenciesHeaderBand.Level = 1;
//            this.CompetenciesHeaderBand.Name = "CompetenciesHeaderBand";
//            // 
//            // lblCompetencies
//            // 
//            resources.ApplyResources(this.lblCompetencies, "lblCompetencies");
//            this.lblCompetencies.Name = "lblCompetencies";
//            this.lblCompetencies.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblCompetencies.StyleName = "TitleLevel1";
//            // 
//            // ApprovalsReportBand
//            // 
//            this.ApprovalsReportBand.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
//            this.ApprovalsDetailBand});
//            resources.ApplyResources(this.ApprovalsReportBand, "ApprovalsReportBand");
//            this.ApprovalsReportBand.Level = 5;
//            this.ApprovalsReportBand.Name = "ApprovalsReportBand";
//            // 
//            // ApprovalsDetailBand
//            // 
//            this.ApprovalsDetailBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
//            this.lblApprovalsEmptyCell3Row3,
//            this.lblApprovalsEmptyCell2Row3,
//            this.lblApprovalsEmptyCell1Row3,
//            this.lblApprovalsHeadOfDepartment,
//            this.lblApprovalsDirectLineManager,
//            this.lblApprovalsEmptyCell2Row2,
//            this.lblApprovalsEmptyCell1Row2,
//            this.lblApprovalsEmptyCell3Row2,
//            this.lblApprovalsEmptyCell3Row1,
//            this.lblApprovalsEmptyCell2Row1,
//            this.lblApprovalsEmptyCell1Row1,
//            this.lblApprovalsEmployee,
//            this.lblApprovalsDate,
//            this.lblApprovalsSignature,
//            this.lblApprovalsName,
//            this.lblApprovalsEmptyCellCorner,
//            this.lblApprovals});
//            resources.ApplyResources(this.ApprovalsDetailBand, "ApprovalsDetailBand");
//            this.ApprovalsDetailBand.KeepTogether = true;
//            this.ApprovalsDetailBand.Name = "ApprovalsDetailBand";
//            // 
//            // lblApprovalsEmptyCell3Row3
//            // 
//            resources.ApplyResources(this.lblApprovalsEmptyCell3Row3, "lblApprovalsEmptyCell3Row3");
//            this.lblApprovalsEmptyCell3Row3.Name = "lblApprovalsEmptyCell3Row3";
//            this.lblApprovalsEmptyCell3Row3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblApprovalsEmptyCell3Row3.StyleName = "DataField";
//            // 
//            // lblApprovalsEmptyCell2Row3
//            // 
//            resources.ApplyResources(this.lblApprovalsEmptyCell2Row3, "lblApprovalsEmptyCell2Row3");
//            this.lblApprovalsEmptyCell2Row3.Name = "lblApprovalsEmptyCell2Row3";
//            this.lblApprovalsEmptyCell2Row3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblApprovalsEmptyCell2Row3.StyleName = "DataField";
//            // 
//            // lblApprovalsEmptyCell1Row3
//            // 
//            resources.ApplyResources(this.lblApprovalsEmptyCell1Row3, "lblApprovalsEmptyCell1Row3");
//            this.lblApprovalsEmptyCell1Row3.Name = "lblApprovalsEmptyCell1Row3";
//            this.lblApprovalsEmptyCell1Row3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblApprovalsEmptyCell1Row3.StyleName = "DataField";
//            // 
//            // lblApprovalsHeadOfDepartment
//            // 
//            resources.ApplyResources(this.lblApprovalsHeadOfDepartment, "lblApprovalsHeadOfDepartment");
//            this.lblApprovalsHeadOfDepartment.Name = "lblApprovalsHeadOfDepartment";
//            this.lblApprovalsHeadOfDepartment.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblApprovalsHeadOfDepartment.StyleName = "TitleLevel2";
//            // 
//            // lblApprovalsDirectLineManager
//            // 
//            resources.ApplyResources(this.lblApprovalsDirectLineManager, "lblApprovalsDirectLineManager");
//            this.lblApprovalsDirectLineManager.Name = "lblApprovalsDirectLineManager";
//            this.lblApprovalsDirectLineManager.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblApprovalsDirectLineManager.StyleName = "TitleLevel2";
//            // 
//            // lblApprovalsEmptyCell2Row2
//            // 
//            resources.ApplyResources(this.lblApprovalsEmptyCell2Row2, "lblApprovalsEmptyCell2Row2");
//            this.lblApprovalsEmptyCell2Row2.Name = "lblApprovalsEmptyCell2Row2";
//            this.lblApprovalsEmptyCell2Row2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblApprovalsEmptyCell2Row2.StyleName = "DataField";
//            // 
//            // lblApprovalsEmptyCell1Row2
//            // 
//            resources.ApplyResources(this.lblApprovalsEmptyCell1Row2, "lblApprovalsEmptyCell1Row2");
//            this.lblApprovalsEmptyCell1Row2.Name = "lblApprovalsEmptyCell1Row2";
//            this.lblApprovalsEmptyCell1Row2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblApprovalsEmptyCell1Row2.StyleName = "DataField";
//            // 
//            // lblApprovalsEmptyCell3Row2
//            // 
//            resources.ApplyResources(this.lblApprovalsEmptyCell3Row2, "lblApprovalsEmptyCell3Row2");
//            this.lblApprovalsEmptyCell3Row2.Name = "lblApprovalsEmptyCell3Row2";
//            this.lblApprovalsEmptyCell3Row2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblApprovalsEmptyCell3Row2.StyleName = "DataField";
//            // 
//            // lblApprovalsEmptyCell3Row1
//            // 
//            resources.ApplyResources(this.lblApprovalsEmptyCell3Row1, "lblApprovalsEmptyCell3Row1");
//            this.lblApprovalsEmptyCell3Row1.Name = "lblApprovalsEmptyCell3Row1";
//            this.lblApprovalsEmptyCell3Row1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblApprovalsEmptyCell3Row1.StyleName = "DataField";
//            // 
//            // lblApprovalsEmptyCell2Row1
//            // 
//            resources.ApplyResources(this.lblApprovalsEmptyCell2Row1, "lblApprovalsEmptyCell2Row1");
//            this.lblApprovalsEmptyCell2Row1.Name = "lblApprovalsEmptyCell2Row1";
//            this.lblApprovalsEmptyCell2Row1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblApprovalsEmptyCell2Row1.StyleName = "DataField";
//            // 
//            // lblApprovalsEmptyCell1Row1
//            // 
//            resources.ApplyResources(this.lblApprovalsEmptyCell1Row1, "lblApprovalsEmptyCell1Row1");
//            this.lblApprovalsEmptyCell1Row1.Name = "lblApprovalsEmptyCell1Row1";
//            this.lblApprovalsEmptyCell1Row1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblApprovalsEmptyCell1Row1.StyleName = "DataField";
//            // 
//            // lblApprovalsEmployee
//            // 
//            resources.ApplyResources(this.lblApprovalsEmployee, "lblApprovalsEmployee");
//            this.lblApprovalsEmployee.Name = "lblApprovalsEmployee";
//            this.lblApprovalsEmployee.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblApprovalsEmployee.StyleName = "TitleLevel2";
//            // 
//            // lblApprovalsDate
//            // 
//            resources.ApplyResources(this.lblApprovalsDate, "lblApprovalsDate");
//            this.lblApprovalsDate.Name = "lblApprovalsDate";
//            this.lblApprovalsDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblApprovalsDate.StyleName = "TitleLevel2";
//            // 
//            // lblApprovalsSignature
//            // 
//            resources.ApplyResources(this.lblApprovalsSignature, "lblApprovalsSignature");
//            this.lblApprovalsSignature.Name = "lblApprovalsSignature";
//            this.lblApprovalsSignature.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblApprovalsSignature.StyleName = "TitleLevel2";
//            // 
//            // lblApprovalsName
//            // 
//            resources.ApplyResources(this.lblApprovalsName, "lblApprovalsName");
//            this.lblApprovalsName.Name = "lblApprovalsName";
//            this.lblApprovalsName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblApprovalsName.StyleName = "TitleLevel2";
//            // 
//            // lblApprovalsEmptyCellCorner
//            // 
//            resources.ApplyResources(this.lblApprovalsEmptyCellCorner, "lblApprovalsEmptyCellCorner");
//            this.lblApprovalsEmptyCellCorner.Name = "lblApprovalsEmptyCellCorner";
//            this.lblApprovalsEmptyCellCorner.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblApprovalsEmptyCellCorner.StyleName = "DataField";
//            // 
//            // lblApprovals
//            // 
//            resources.ApplyResources(this.lblApprovals, "lblApprovals");
//            this.lblApprovals.Name = "lblApprovals";
//            this.lblApprovals.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
//            this.lblApprovals.StyleName = "TitleLevel1";
//            // 
//            // JobDescTemplate
//            // 
//            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
//            this.BasicInfoBand,
//            this.TopMargin,
//            this.BottomMargin,
//            this.RolesReportBand,
//            this.AuthoritiesReportBand,
//            this.JobRelationshipsReportBand,
//            this.QualificationsReportBand,
//            this.CompetenciesReportBand,
//            this.ApprovalsReportBand});
//            this.DataSource = this.bindingSourceJobDescriptionTemplateDTO;
//            resources.ApplyResources(this, "$this");
//            this.Margins = new System.Drawing.Printing.Margins(127, 127, 67, 75);
//            this.PageHeight = 2969;
//            this.PageWidth = 2101;
//            this.PaperKind = System.Drawing.Printing.PaperKind.A4;
//            this.Version = "11.2";
//            this.DataSourceDemanded += new System.EventHandler<System.EventArgs>(this.JobDescTemplate_DataSourceDemanded);            
//            this.Controls.SetChildIndex(this.ApprovalsReportBand, 0);
//            this.Controls.SetChildIndex(this.CompetenciesReportBand, 0);
//            this.Controls.SetChildIndex(this.QualificationsReportBand, 0);
//            this.Controls.SetChildIndex(this.JobRelationshipsReportBand, 0);
//            this.Controls.SetChildIndex(this.AuthoritiesReportBand, 0);
//            this.Controls.SetChildIndex(this.RolesReportBand, 0);
//            this.Controls.SetChildIndex(this.BottomMargin, 0);
//            this.Controls.SetChildIndex(this.TopMargin, 0);
//            this.Controls.SetChildIndex(this.BasicInfoBand, 0);
//            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceJobDescriptionTemplateDTO)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

//        }

//        #endregion

//        private DevExpress.XtraReports.UI.DetailBand BasicInfoBand;
//        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
//        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
//        private System.Windows.Forms.BindingSource bindingSourceJobDescriptionTemplateDTO;
//        private DevExpress.XtraReports.UI.DetailReportBand RolesReportBand;
//        private DevExpress.XtraReports.UI.DetailBand RolesDetailBand;
//        private DevExpress.XtraReports.UI.DetailReportBand ResponsibilitiesReportBand;
//        private DevExpress.XtraReports.UI.DetailBand ResponsibilitiesDetailBand;
//        private DevExpress.XtraReports.UI.GroupHeaderBand ResponsibilitiesHeaderBand;
//        private DevExpress.XtraReports.UI.GroupHeaderBand RolesHeaderBand;
//        private DevExpress.XtraReports.UI.DetailReportBand AuthoritiesReportBand;
//        private DevExpress.XtraReports.UI.DetailBand AuthoritiesDetailBand;
//        private DevExpress.XtraReports.UI.XRLabel lblAuthoritiesTitle;
//        private DevExpress.XtraReports.UI.GroupHeaderBand AuthoritiesHeaderBand;
//        private DevExpress.XtraReports.UI.DetailReportBand JobRelationshipsReportBand;
//        private DevExpress.XtraReports.UI.DetailBand JobRelationshipsDetailBand;
//        private DevExpress.XtraReports.UI.DetailReportBand QualificationsReportBand;
//        private DevExpress.XtraReports.UI.DetailBand QualificationsDetailBand;
//        private DevExpress.XtraReports.UI.DetailReportBand EducationReportBand;
//        private DevExpress.XtraReports.UI.DetailBand EducationDetailBand;
//        private DevExpress.XtraReports.UI.GroupHeaderBand EducationHeaderBand;
//        private DevExpress.XtraReports.UI.DetailReportBand ExperienceReportBand;
//        private DevExpress.XtraReports.UI.DetailBand ExperienceDetailBand;
//        private DevExpress.XtraReports.UI.GroupHeaderBand ExperienceHeaserBand;
//        private DevExpress.XtraReports.UI.DetailReportBand LanguagesReportBand;
//        private DevExpress.XtraReports.UI.DetailBand LanguagesDetailBand;
//        private DevExpress.XtraReports.UI.GroupHeaderBand LanguagesHeaderBand;
//        private DevExpress.XtraReports.UI.DetailReportBand PCSkillsReportBand;
//        private DevExpress.XtraReports.UI.DetailBand PCSkillsDetailBand;
//        private DevExpress.XtraReports.UI.GroupHeaderBand PCSkillsHeaderBand;
//        private DevExpress.XtraReports.UI.DetailReportBand OtherSkillsReportBand;
//        private DevExpress.XtraReports.UI.DetailBand OtherSkillsDetailBand;
//        private DevExpress.XtraReports.UI.GroupHeaderBand OtherSkillsHeaderBand;
//        private DevExpress.XtraReports.UI.GroupHeaderBand QualificationsHeaderBand;
//        private DevExpress.XtraReports.UI.XRLabel lblQualifications;
//        private DevExpress.XtraReports.UI.DetailBand Detail;
//        private DevExpress.XtraReports.UI.DetailReportBand CompetenciesReportBand;
//        private DevExpress.XtraReports.UI.DetailBand CompetenciesDetailBand;
//        private DevExpress.XtraReports.UI.GroupHeaderBand CompetenciesTypeGroupHeaderBand;
//        private DevExpress.XtraReports.UI.GroupHeaderBand CompetenciesHeaderBand;
//        private DevExpress.XtraReports.UI.XRLabel lblCompetencies;
//        private DevExpress.XtraReports.UI.XRLabel lblCompetenciesTypeGroup;
//        private DevExpress.XtraReports.UI.XRLabel valueBasicInfoJobSummary;
//        private DevExpress.XtraReports.UI.XRLabel lblBasicInfoJobSummary;
//        private DevExpress.XtraReports.UI.XRLabel valueBasicInfoNodeName;
//        private DevExpress.XtraReports.UI.XRLabel valueBasicInfoNodeType;
//        private DevExpress.XtraReports.UI.XRLabel valueBasicInfoReportingTo;
//        private DevExpress.XtraReports.UI.XRLabel lblBasicInfoReportingTo;
//        private DevExpress.XtraReports.UI.XRLabel valueBasicInfoJobTilte;
//        private DevExpress.XtraReports.UI.XRLabel lblBasicInfoJobTilte;
//        private DevExpress.XtraReports.UI.XRLabel lblBasicInfoJobDescribtionTemplate;
//        private DevExpress.XtraReports.UI.XRLabel lblRolesAndResponsibilities;
//        private DevExpress.XtraReports.UI.XRLabel valueRolesJobTitleAndWeight;
//        private DevExpress.XtraReports.UI.XRLabel valueResponsibilitiesKpi;
//        private DevExpress.XtraReports.UI.XRLabel valueResponsibilitiesWeight;
//        private DevExpress.XtraReports.UI.XRLabel valueResponsibilitiesDescription;
//        private DevExpress.XtraReports.UI.XRLabel lblResponsibilitiesKpi;
//        private DevExpress.XtraReports.UI.XRLabel lblResponsibilitiesWeight;
//        private DevExpress.XtraReports.UI.XRLabel lblResponsibilitiesDescription;
//        private DevExpress.XtraReports.UI.XRLabel lblAuthoritiesAuthorityLevel;
//        private DevExpress.XtraReports.UI.XRLabel lblAuthoritiesOperatingBudget;
//        private DevExpress.XtraReports.UI.XRLabel lblAuthoritiesDimensions;
//        private DevExpress.XtraReports.UI.XRLabel valueJobRelationshipsExternal;
//        private DevExpress.XtraReports.UI.XRLabel lblJobRelationshipsExternal;
//        private DevExpress.XtraReports.UI.XRLabel valueJobRelationshipsInternal;
//        private DevExpress.XtraReports.UI.XRLabel lblJobRelationshipsInternal;
//        private DevExpress.XtraReports.UI.XRLabel lblJobRelationships;
//        private DevExpress.XtraReports.UI.XRLabel valueEducationType;
//        private DevExpress.XtraReports.UI.XRLabel valueEducationMajor;
//        private DevExpress.XtraReports.UI.XRLabel lblEducationType;
//        private DevExpress.XtraReports.UI.XRLabel lblEducationMajor;
//        private DevExpress.XtraReports.UI.XRLabel lblEducation;
//        private DevExpress.XtraReports.UI.XRLabel valueExperienceCareerLevel;
//        private DevExpress.XtraReports.UI.XRLabel valueExperienceIndustry;
//        private DevExpress.XtraReports.UI.XRLabel lblExperience;
//        private DevExpress.XtraReports.UI.XRLabel lblExperienceIndustry;
//        private DevExpress.XtraReports.UI.XRLabel lblExperienceCareerLevel;
//        private DevExpress.XtraReports.UI.XRLabel valueLanguageReading;
//        private DevExpress.XtraReports.UI.XRLabel valueLanguageSpeaking;
//        private DevExpress.XtraReports.UI.XRLabel valueLanguageWriting;
//        private DevExpress.XtraReports.UI.XRLabel valueLanguageName;
//        private DevExpress.XtraReports.UI.XRLabel valueLanguageListening;
//        private DevExpress.XtraReports.UI.XRLabel lblLanguageListening;
//        private DevExpress.XtraReports.UI.XRLabel lblLanguageReading;
//        private DevExpress.XtraReports.UI.XRLabel lblLanguageWriting;
//        private DevExpress.XtraReports.UI.XRLabel lblLanguageSpeaking;
//        private DevExpress.XtraReports.UI.XRLabel lblLanguageName;
//        private DevExpress.XtraReports.UI.XRLabel lblLanguage;
//        private DevExpress.XtraReports.UI.XRLabel valuePCSkillsType;
//        private DevExpress.XtraReports.UI.XRLabel valuePCSkillsLevel;
//        private DevExpress.XtraReports.UI.XRLabel lblPCSkillsLevel;
//        private DevExpress.XtraReports.UI.XRLabel lblPCSkillsType;
//        private DevExpress.XtraReports.UI.XRLabel lblPCSkills;
//        private DevExpress.XtraReports.UI.XRLabel valueOtherSkillsLevel;
//        private DevExpress.XtraReports.UI.XRLabel valueOtherSkillsType;
//        private DevExpress.XtraReports.UI.XRLabel lblOtherSkillsLevel;
//        private DevExpress.XtraReports.UI.XRLabel lblOtherSkillsType;
//        private DevExpress.XtraReports.UI.XRLabel lblOtherSkills;
        
//        private DevExpress.XtraReports.UI.XRLabel ValueCompetenciesDescription;
//        private DevExpress.XtraReports.UI.XRLabel ValueCompetenciesName;
//        private DevExpress.XtraReports.UI.XRLabel valueCompetenciesLevel;
//        private DevExpress.XtraReports.UI.XRLabel valueCompetenciesWeight;
//        private DevExpress.XtraReports.UI.XRLabel lblCompetenciesWeight;
//        private DevExpress.XtraReports.UI.XRLabel lblCompetenciesLevel;
//        private DevExpress.XtraReports.UI.XRLabel lblCompetenciesDescription;
//        private DevExpress.XtraReports.UI.XRLabel lblCompetenciesName;
//        private DevExpress.XtraReports.UI.DetailReportBand ApprovalsReportBand;
//        private DevExpress.XtraReports.UI.DetailBand ApprovalsDetailBand;
//        private DevExpress.XtraReports.UI.XRLabel lblApprovalsEmptyCell3Row3;
//        private DevExpress.XtraReports.UI.XRLabel lblApprovalsEmptyCell2Row3;
//        private DevExpress.XtraReports.UI.XRLabel lblApprovalsEmptyCell1Row3;
//        private DevExpress.XtraReports.UI.XRLabel lblApprovalsHeadOfDepartment;
//        private DevExpress.XtraReports.UI.XRLabel lblApprovalsDirectLineManager;
//        private DevExpress.XtraReports.UI.XRLabel lblApprovalsEmptyCell2Row2;
//        private DevExpress.XtraReports.UI.XRLabel lblApprovalsEmptyCell1Row2;
//        private DevExpress.XtraReports.UI.XRLabel lblApprovalsEmptyCell3Row2;
//        private DevExpress.XtraReports.UI.XRLabel lblApprovalsEmptyCell3Row1;
//        private DevExpress.XtraReports.UI.XRLabel lblApprovalsEmptyCell2Row1;
//        private DevExpress.XtraReports.UI.XRLabel lblApprovalsEmptyCell1Row1;
//        private DevExpress.XtraReports.UI.XRLabel lblApprovalsEmployee;
//        private DevExpress.XtraReports.UI.XRLabel lblApprovalsDate;
//        private DevExpress.XtraReports.UI.XRLabel lblApprovalsSignature;
//        private DevExpress.XtraReports.UI.XRLabel lblApprovalsName;
//        private DevExpress.XtraReports.UI.XRLabel lblApprovalsEmptyCellCorner;
//        private DevExpress.XtraReports.UI.XRLabel lblApprovals;
//        private DevExpress.XtraReports.UI.XRLabel valueCompetenciesTypeGroup;
//        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
//    }
//}
