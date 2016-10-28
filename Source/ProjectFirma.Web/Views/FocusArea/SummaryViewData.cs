﻿using ProjectFirma.Web.Controllers;
using ProjectFirma.Web.Security;
using ProjectFirma.Web.Models;
using ProjectFirma.Web.Views.Map;
using ProjectFirma.Web.Views.Project;
using ProjectFirma.Web.Views.Shared.ProjectControls;
using ProjectFirma.Web.Views.Shared.ProjectLocationControls;
using LtInfo.Common;

namespace ProjectFirma.Web.Views.FocusArea
{
    public class SummaryViewData : FirmaViewData
    {
        public readonly Models.FocusArea FocusArea;
        public readonly bool UserHasFocusAreaManagePermissions;
        public readonly bool UserHasProjectFocusAreaExpenditureManagePermissions;
        public readonly string EditFocusAreaUrl;

        public readonly string BackUrl;
        public readonly BasicProjectInfoGridSpec BasicProjectInfoGridSpec;
        public readonly string FiveYearListProjectGridName;
        public readonly string FiveYearListProjectGridDataUrl;
        public readonly string EditDescriptionUrl;

        public readonly ProjectLocationsMapInitJson ProjectLocationsMapInitJson;
        public readonly ProjectLocationsMapViewData ProjectLocationsMapViewData;
        public readonly string ProjectMapFilteredUrl;

        public readonly ProjectTaxonomyViewData ProjectTaxonomyViewData;

        public SummaryViewData(Person currentPerson,
            Models.FocusArea focusArea,
            ProjectLocationsMapInitJson projectLocationsMapInitJson,
            ProjectLocationsMapViewData projectLocationsMapViewData) : base(currentPerson)
        {
            FocusArea = focusArea;

            ProjectLocationsMapInitJson = projectLocationsMapInitJson;
            ProjectLocationsMapViewData = projectLocationsMapViewData;

            ProjectMapFilteredUrl = ProjectLocationsMapInitJson.ProjectMapCustomization.GetCustomizedUrl();

            PageTitle = focusArea.DisplayName;
            EntityName = "Focus Area";
            BackUrl = SitkaRoute<FocusAreaController>.BuildUrlFromExpression(c => c.Index());

            UserHasFocusAreaManagePermissions = new FocusAreaManageFeature().HasPermissionByPerson(CurrentPerson);
            UserHasProjectFocusAreaExpenditureManagePermissions = new FocusAreaManageFeature().HasPermissionByPerson(currentPerson);
            EditFocusAreaUrl = SitkaRoute<FocusAreaController>.BuildUrlFromExpression(c => c.Edit(focusArea.FocusAreaID));

            FiveYearListProjectGridName = "focusAreaProjectListGrid";
            BasicProjectInfoGridSpec = new BasicProjectInfoGridSpec(CurrentPerson, true)
            {
                ObjectNameSingular = "Project with this Focus Area",
                ObjectNamePlural = "Projects with this Focus Area",
                SaveFiltersInCookie = true
            };

            FiveYearListProjectGridDataUrl = SitkaRoute<FocusAreaController>.BuildUrlFromExpression(tc => tc.ProjectsGridJsonData(focusArea));
            ProjectTaxonomyViewData = new ProjectTaxonomyViewData(focusArea);
            EditDescriptionUrl = SitkaRoute<FocusAreaController>.BuildUrlFromExpression(tc => tc.EditDescription(focusArea));
        }
    }
}