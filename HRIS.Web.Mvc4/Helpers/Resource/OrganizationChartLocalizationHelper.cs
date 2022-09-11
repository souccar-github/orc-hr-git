using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Core.Extensions;

namespace Project.Web.Mvc4.Helpers.Resource
{
    public class OrganizationChartLocalizationHelper
    {
        public const string ResourceGroupName = "OrganizationChartModule";

        public const string ModuleName = "ModuleName";
        public const string OrganizationCaption = "OrganizationCaption";
        public const string PrivateOrganizationCaption = "PrivateOrganizationCaption";
        public const string MainInformationCaption = "MainInformationCaption";
        public const string ContactInformationCaption = "ContactInformationCaption";
        public const string NodesCaption = "NodesCaption";
        public const string MergeNodesCaption = "MergeNodesCaption";
        public const string NodeSeparationCaption = "NodeSeparationCaption";
        public const string ReorderNodesCaption = "ReorderNodesCaption";
        public const string SeperateNode = "SeperateNode";
        public const string OrganizationNameCaption = "OrganizationNameCaption";
        public const string LocationCaption = "LocationCaption";
        public const string SizeCaption = "SizeCaption";
        public const string NumberOfEmployeesCaption = "NumberOfEmployeesCaption";
        public const string PhoneCaption = "PhoneCaption";
        public const string FaxCaption = "FaxCaption";
        public const string MobileCaption = "MobileCaption";
        public const string AddressCaption = "AddressCaption";
        public const string POBoxCaption = "POBoxCaption";
        public const string WebSiteCaption = "WebSiteCaption";
        public const string FacebookCaption = "FacebookCaption";

        public const string OrganizationNameHint = "OrganizationNameHint";
        public const string MergeTwoNodes = "MergeTwoNodes";
        public const string OrganizationTreeCaption = "OrganizationTreeCaption";
        public const string ExpandOneLevelCaption = "ExpandOneLevelCaption";
        public const string ExpandCaption = "ExpandCaption";
        public const string CollapseCaption = "CollapseCaption";
        public const string AddRootCaption = "AddRootCaption";
        public const string AddNodeCaption = "AddNodeCaption";
        public const string EditNodeCaption = "EditNodeCaption";
        public const string DeleteNodeCaption = "DeleteNodeCaption";
        public const string ParentCaption = "ParentCaption";
        public const string NodeNameCaption = "NodeNameCaption";
        public const string NodeCodeCaption = "NodeCodeCaption";
        public const string NodeTypeCaption = "NodeTypeCaption";
        public const string RootNameCaption = "NodeNameCaption";
        public const string RootCodeCaption = "NodeCodeCaption";
        public const string RootTypeCaption = "NodeTypeCaption";

        public const string ThisNodeIsAlreadyHere = "ThisNodeIsAlreadyHere";
        public const string ThisNodeNameIsAlreadyExist = "ThisNodeNameIsAlreadyExist";
        public const string ThisNodeNameIsAlreadyExistAndTheNodeIsDeleted = "ThisNodeNameIsAlreadyExistAndTheNodeIsDeleted";

        public const string FirstNodeCaption = "FirstNodeCaption";
        public const string SecondNodeCaption = "SecondNodeCaption";
        public const string MergeCaption = "MergeCaption";
        public const string ParentOfTheNewNodeCaption = "ParentOfTheNewNodeCaption";

        public const string PickNodeToSplitCaption = "PickNodeToSplitCaption";
        public const string SplitJobDescriptionsCaption = "SplitJobDescriptionsCaption";
        public const string SplitChildNodesCaption = "SplitChildNodesCaption";
        public const string JobDescriptionsIncludedInTheFirstNodeCaption = "JobDescriptionsIncludedInTheFirstNodeCaption";
        public const string JobDescriptionsIncludedInTheSecondNodeCaption = "JobDescriptionsIncludedInTheSecondNodeCaption";
        public const string ChildNodesIncludedInTheFirstNodeCaption = "ChildNodesIncludedInTheFirstNodeCaption";
        public const string ChildNodesIncludedInTheSecondNodeCaption = "ChildNodesIncludedInTheSecondNodeCaption";

        public const string MsgPleaseEnterName = "MsgPleaseEnterName";
        public const string MsgPleaseSelectNodeFirst = "MsgPleaseSelectNodeFirst";
        public const string MsgPleaseSelectFirstNode = "MsgPleaseSelectFirstNode";
        public const string MsgPleaseSelectSecondNode = "MsgPleaseSelectSecondNode";
        public const string MsgPleaseSelectParent = "MsgPleaseSelectParent";
        public const string MsgYouCannotMergeParentNode = "MsgYouCannotMergeParentNode";
        public const string MsgYouCanNotSelectParentAsFirstNode = "MsgYouCanNotSelectParentAsFirstNode";
        public const string MsgYouCanNotSelectParentAsSecondNode = "MsgYouCanNotSelectParentAsSecondNode";
        public const string MsgPleaseSelectTwoDifferentNodes = "MsgPleaseSelectTwoDifferentNodes";
        public const string MsgCanNotMoveNode = "MsgCanNotMoveNode";
        public const string MsgAreYouSureYouWantMoveNode = "MsgAreYouSureYouWantMoveNode";
        //public const string MsgCanNotMoveNode = "Node {0} Level {1} Can't Be nested Under Node {2} Level {3}";
        //public const string MsgAreYouSureYouWantMoveNode = "Are You Sure You Want To Nest Node {0} Level {1} Under Node {2} lEVEL {3} ?";
        //MsgOrganizationNameIsRequired
        public const string MsgOrganizationNameIsRequired = "MsgOrganizationNameIsRequired";
        //MsgLocationIsRequired
        public const string MsgLocationIsRequired = "MsgLocationIsRequired";
        //MsgSizeIsRequired
        public const string MsgSizeIsRequired = "MsgSizeIsRequired";
        //MsgNumberOfEmployeesIsRequired
        public const string MsgNumberOfEmployeesIsRequired = "MsgNumberOfEmployeesIsRequired";
        //MsgPhoneIsRequired
        public const string MsgPhoneIsRequired = "MsgPhoneIsRequired";
        //MsgFaxIsRequired
        public const string MsgFaxIsRequired = "MsgFaxIsRequired";
        //MsgMobileIsRequired
        public const string MsgMobileIsRequired = "MsgMobileIsRequired";
        //MsgAddressIsRequired
        public const string MsgAddressIsRequired = "MsgAddressIsRequired";
        public const string OverallOrgChart = "OverallOrgChart";
        public const string OrgChartBasedOnPosition = "OrgChartBasedOnPosition";
        public const string OrgChartBasedOnGrade = "OrgChartBasedOnGrade";
        public const string OrderingParentFailerMessage = "YouCan'tParentNodeToChildNode";
        public const string OrderingFailerMessage = "YouCan'tAddOrUpdateToThisNodeTypeBecauseOfParentNodeType";
        public const string OrderingChildrenFailerMessage = "YouCan'tAddOrUpdateToThisNodeTypeBecauseOfChildrenNodeType";
        public const string TheOrderOfTypeOfFirstChildNodeMustGreaterThanTheOrderOfTheParent = "TheOrderOfTypeOfFirstChildNodeMustGreaterThanTheOrderOfTheParent";
        public const string TheOrderOfTypeOfSecondChildNodeMustGreaterThanTheOrderOfTheParent = "TheOrderOfTypeOfSecondChildNodeMustGreaterThanTheOrderOfTheParent";
        public const string MsgTheNewNodeTypeMustBeLowerThanParentNodeType = "MsgTheNewNodeTypeMustBeLowerThanParentNodeType";
        public const string MsgPhoneIsNotNumeric = "MsgPhoneIsNotNumeric";
        public const string MsgFaxIsNotNumeric = "MsgFaxIsNotNumeric";
        public const string MsgMobileIsNotNumeric = "MsgMobileIsNotNumeric";
        public const string MsgIsNotMatchWebsite = "MsgIsNotMatchWebsite";
        public const string MsgIsNotMatchFacebook = "MsgIsNotMatchFacebook";
        public const string OrganizationChart= "OrganizationChart";
        public const string DeleteNode = "DeleteNode";
        public static string GetResource(string key)
        {
            var result = ServiceFactory.LocalizationService.GetResource(ResourceGroupName + "_" + key);
            return string.IsNullOrEmpty(result) ? key.ToCapitalLetters() : result;
        }
    }
}