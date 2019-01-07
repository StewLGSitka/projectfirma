/*-----------------------------------------------------------------------
<copyright file="DetailViewData.cs" company="Tahoe Regional Planning Agency and Sitka Technology Group">
Copyright (c) Tahoe Regional Planning Agency and Sitka Technology Group. All rights reserved.
<author>Sitka Technology Group</author>
</copyright>

<license>
This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License <http://www.gnu.org/licenses/> for more details.

Source code is available upon request via <support@sitkatech.com>.
</license>
-----------------------------------------------------------------------*/

using ProjectFirma.Web.Controllers;
using ProjectFirma.Web.Security;
using ProjectFirma.Web.Models;
using ProjectFirma.Web.Views.Shared.TextControls;
using ProjectFirma.Web.Common;

namespace ProjectFirma.Web.Views.PerformanceMeasure
{
    public class DetailViewData : FirmaViewData
    {
        public Models.PerformanceMeasurePseudo PerformanceMeasure { get; }
        public PerformanceMeasureChartViewData PerformanceMeasureChartViewData { get; }
        public EntityNotesViewData EntityNotesViewData { get; }

        public bool UserHasPerformanceMeasureOverviewManagePermissions { get; }

        public string EditPerformanceMeasureUrl { get; }
        public string EditSubcategoriesAndOptionsUrl { get; }
        public string EditCriticalDefinitionsUrl { get; }
        public string EditProjectReportingUrl { get; }

        public string IndexUrl { get; }

        public string EditTaxonomyTiersUrl { get; }
        public bool UserHasTaxonomyTierPerformanceMeasureManagePermissions { get; }
        public PerformanceMeasureReportedValuesGridSpec PerformanceMeasureReportedValuesGridSpec { get; }
        public string PerformanceMeasureReportedValuesGridName { get; }
        public string PerformanceMeasureReportedValuesGridDataUrl { get; }
        public PerformanceMeasureExpectedGridSpec PerformanceMeasureExpectedGridSpec { get; }
        public string PerformanceMeasureExpectedsGridName { get; }
        public string PerformanceMeasureExpectedsGridDataUrl { get; }

        public string TaxonomyTierDisplayNamePluralized { get; }

        public DetailViewData(Person currentPerson,
            Models.PerformanceMeasurePseudo performanceMeasurePseudo,
            PerformanceMeasureChartViewData performanceMeasureChartViewData,
            EntityNotesViewData entityNotesViewData,
            bool userHasPerformanceMeasureManagePermissions) : base(currentPerson)
        {
            PageTitle = performanceMeasurePseudo.PerformanceMeasureDisplayName;
            EntityName = "PerformanceMeasure Detail";

            PerformanceMeasure = performanceMeasurePseudo;
            PerformanceMeasureChartViewData = performanceMeasureChartViewData;
            EntityNotesViewData = entityNotesViewData;
            UserHasPerformanceMeasureOverviewManagePermissions = userHasPerformanceMeasureManagePermissions;

            EditPerformanceMeasureUrl = SitkaRoute<PerformanceMeasureController>.BuildUrlFromExpression(c => c.Edit(performanceMeasurePseudo.PerformanceMeasureID));
            EditSubcategoriesAndOptionsUrl = SitkaRoute<PerformanceMeasureController>.BuildUrlFromExpression(c => c.EditSubcategoriesAndOptions(performanceMeasurePseudo.PerformanceMeasureID));
                
            EditCriticalDefinitionsUrl = SitkaRoute<PerformanceMeasureController>.BuildUrlFromExpression(c => c.EditPerformanceMeasureRichText(performanceMeasurePseudo.PerformanceMeasureID, EditRtfContent.PerformanceMeasureRichTextType.CriticalDefinitions));
            EditProjectReportingUrl = SitkaRoute<PerformanceMeasureController>.BuildUrlFromExpression(c => c.EditPerformanceMeasureRichText(performanceMeasurePseudo.PerformanceMeasureID, EditRtfContent.PerformanceMeasureRichTextType.ProjectReporting));

            IndexUrl = SitkaRoute<PerformanceMeasureController>.BuildUrlFromExpression(c => c.Index());
            var associatePerformanceMeasureTaxonomyLevel = MultiTenantHelpers.GetAssociatePerformanceMeasureTaxonomyLevel();
            TaxonomyTierDisplayNamePluralized = associatePerformanceMeasureTaxonomyLevel.GetFieldDefinition().GetFieldDefinitionLabelPluralized();
            UserHasTaxonomyTierPerformanceMeasureManagePermissions = new TaxonomyTierPerformanceMeasureManageFeature().HasPermissionByPerson(currentPerson);
            EditTaxonomyTiersUrl = SitkaRoute<TaxonomyTierPerformanceMeasureController>.BuildUrlFromExpression(c => c.Edit(performanceMeasurePseudo.PerformanceMeasureID));
            RelatedTaxonomyTiersViewData = new RelatedTaxonomyTiersViewData(performanceMeasurePseudo.PerformanceMeasure, associatePerformanceMeasureTaxonomyLevel, true);

            PerformanceMeasureReportedValuesGridSpec = new PerformanceMeasureReportedValuesGridSpec(performanceMeasurePseudo)
            {
                ObjectNameSingular = $"{Models.FieldDefinition.ReportedValue.GetFieldDefinitionLabel()} for {Models.FieldDefinition.Project.GetFieldDefinitionLabel()}",
                ObjectNamePlural = $"{Models.FieldDefinition.ReportedValue.GetFieldDefinitionLabelPluralized()} for {Models.FieldDefinition.Project.GetFieldDefinitionLabelPluralized()}",
                SaveFiltersInCookie = true
            };

            PerformanceMeasureReportedValuesGridName = "performanceMeasuresReportedValuesFromPerformanceMeasureGrid";
            PerformanceMeasureReportedValuesGridDataUrl = SitkaRoute<PerformanceMeasureController>.BuildUrlFromExpression(tc => tc.PerformanceMeasureReportedValuesGridJsonData(performanceMeasurePseudo.PerformanceMeasure));

            PerformanceMeasureExpectedGridSpec = new PerformanceMeasureExpectedGridSpec(performanceMeasurePseudo.PerformanceMeasure)
            {
                ObjectNameSingular = $"{Models.FieldDefinition.ExpectedValue.GetFieldDefinitionLabel()} for {Models.FieldDefinition.Project.GetFieldDefinitionLabel()}",
                ObjectNamePlural = $"{Models.FieldDefinition.ExpectedValue.GetFieldDefinitionLabelPluralized()} for {Models.FieldDefinition.Project.GetFieldDefinitionLabelPluralized()}",
                SaveFiltersInCookie = true
            };

            PerformanceMeasureExpectedsGridName = "performanceMeasuresExpectedValuesFromPerformanceMeasureGrid";
            PerformanceMeasureExpectedsGridDataUrl = SitkaRoute<PerformanceMeasureController>.BuildUrlFromExpression(tc => tc.PerformanceMeasureExpectedsGridJsonData(performanceMeasurePseudo.PerformanceMeasure));
        }

        public RelatedTaxonomyTiersViewData RelatedTaxonomyTiersViewData { get; }
    }
}
