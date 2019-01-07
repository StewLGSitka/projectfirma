/*-----------------------------------------------------------------------
<copyright file="PerformanceMeasureGridSpec.cs" company="Tahoe Regional Planning Agency and Sitka Technology Group">
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

using System.Web;
using ProjectFirma.Web.Models;
using LtInfo.Common;
using LtInfo.Common.DhtmlWrappers;
using LtInfo.Common.HtmlHelperExtensions;
using LtInfo.Common.Views;
using ProjectFirma.Web.Common;
using ProjectFirma.Web.Security;

namespace ProjectFirma.Web.Views.PerformanceMeasure
{
    public class PerformanceMeasurePseudoGridSpec : GridSpec<Models.PerformanceMeasurePseudo>
    {
        public PerformanceMeasurePseudoGridSpec(Person currentPerson)
        {
            var hasDeletePermission = new PerformanceMeasureManageFeature().HasPermissionByPerson(currentPerson);
            if (hasDeletePermission)
            {
                Add(string.Empty, x => x.PerformanceMeasure.PerformanceMeasureDataSourceType.IsCustomCalculation ? new HtmlString("") : DhtmlxGridHtmlHelpers.MakeDeleteIconAndLinkBootstrap(x.PerformanceMeasure.GetDeleteUrl(), true), 30, DhtmlxGridColumnFilterType.None);
            }
            Add(Models.FieldDefinition.PerformanceMeasure.ToGridHeaderString(MultiTenantHelpers.GetPerformanceMeasureName()),
                a => UrlTemplate.MakeHrefString(a.PerformanceMeasure.GetSummaryUrl(), a.PerformanceMeasureDisplayName),
                300,
                DhtmlxGridColumnFilterType.Text);
            Add(Models.FieldDefinition.MeasurementUnit.ToGridHeaderString("Unit"), a => a.MeasurementUnitTypeName, 80, DhtmlxGridColumnFilterType.SelectFilterStrict);
            Add(Models.FieldDefinition.PerformanceMeasureType.ToGridHeaderString("Type"), a => a.PerformanceMeasureTypeName, 60, DhtmlxGridColumnFilterType.SelectFilterStrict);
            Add("Description", a => a.PerformanceMeasureDefinition, 400, DhtmlxGridColumnFilterType.Html);
            Add($"# of {Models.FieldDefinition.PerformanceMeasureSubcategory.GetFieldDefinitionLabelPluralized()}", a => a.PerformanceMeasure.GetRealSubcategoryCount(), 110);
            Add($"# of {Models.FieldDefinition.Project.GetFieldDefinitionLabelPluralized()}", a => a.PerformanceMeasure.ReportedProjectsCount(currentPerson), 80);
            Add($"Primary {MultiTenantHelpers.GetAssociatePerformanceMeasureTaxonomyLevel().GetFieldDefinition().GetFieldDefinitionLabel()}", a => a.PerformanceMeasure.GetPrimaryTaxonomyTier() == null ? new HtmlString(string.Empty) : a.PerformanceMeasure.GetPrimaryTaxonomyTier().GetDisplayNameAsUrl(), 150);
        }
    }
}
