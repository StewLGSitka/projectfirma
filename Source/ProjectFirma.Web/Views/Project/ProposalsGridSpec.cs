﻿/*-----------------------------------------------------------------------
<copyright file="ProposalsGridSpec.cs" company="Tahoe Regional Planning Agency and Sitka Technology Group">
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

using LtInfo.Common;
using LtInfo.Common.DhtmlWrappers;
using LtInfo.Common.HtmlHelperExtensions;
using LtInfo.Common.Views;
using ProjectFirma.Web.Common;
using ProjectFirma.Web.Models;
using ProjectFirma.Web.Security;

namespace ProjectFirma.Web.Views.Project
{
    public class ProposalsGridSpec : GridSpec<Models.Project>
    {
        public ProposalsGridSpec(Person currentPerson)
        {
            Add(string.Empty, x =>
            {
                var userHasDeletePermission = new ProjectDeleteProposalFeature().HasPermission(currentPerson, x).HasPermission;
                return DhtmlxGridHtmlHelpers.MakeDeleteIconAndLinkBootstrap(x.GetDeleteProposalUrl(),
                        userHasDeletePermission, true);
            }, 30, DhtmlxGridColumnFilterType.None);
            Add(string.Empty, x => DhtmlxGridHtmlHelpers.MakeEditIconAsHyperlinkBootstrap(x.GetProjectCreateUrl(), new ProjectCreateFeature().HasPermission(currentPerson, x).HasPermission), 30, DhtmlxGridColumnFilterType.None);
            Add(Models.FieldDefinition.ProjectName.ToGridHeaderString(), x => UrlTemplate.MakeHrefString(x.GetDetailUrl(), x.ProjectName), 300, DhtmlxGridColumnFilterType.Html);
            Add("Submittal Status", a => a.ProjectApprovalStatus.ProjectApprovalStatusDisplayName, 110, DhtmlxGridColumnFilterType.SelectFilterStrict);
            if (MultiTenantHelpers.HasCanStewardProjectsOrganizationRelationship())
            {
                Add(Models.FieldDefinition.ProjectsStewardOrganizationRelationshipToProject.ToGridHeaderString(), x => x.GetCanStewardProjectsOrganization().GetShortNameAsUrl(), 150,
                    DhtmlxGridColumnFilterType.Html);
            }
            Add(Models.FieldDefinition.IsPrimaryContactOrganization.ToGridHeaderString(), x => x.GetPrimaryContactOrganization().GetShortNameAsUrl(), 150, DhtmlxGridColumnFilterType.Html);
            Add(Models.FieldDefinition.TaxonomyLeaf.ToGridHeaderString(), x => x.TaxonomyLeaf == null ? string.Empty : x.TaxonomyLeaf.GetDisplayName(), 300, DhtmlxGridColumnFilterType.Html);
            Add(Models.FieldDefinition.PlanningDesignStartYear.ToGridHeaderString(), x => Models.ProjectModelExtensions.GetPlanningDesignStartYear(x), 90, DhtmlxGridColumnFilterType.SelectFilterStrict);
            Add(Models.FieldDefinition.ImplementationStartYear.ToGridHeaderString(), x => Models.ProjectModelExtensions.GetImplementationStartYear(x), 115, DhtmlxGridColumnFilterType.SelectFilterStrict);
            Add(Models.FieldDefinition.CompletionYear.ToGridHeaderString(), x => Models.ProjectModelExtensions.GetCompletionYear(x), 90, DhtmlxGridColumnFilterType.SelectFilterStrict);
            Add(Models.FieldDefinition.EstimatedTotalCost.ToGridHeaderString(), x => x.EstimatedTotalCost, 100, DhtmlxGridColumnFormatType.Currency, DhtmlxGridColumnAggregationType.Total);
            Add(Models.FieldDefinition.SecuredFunding.ToGridHeaderString(), x => x.GetSecuredFunding(), 100, DhtmlxGridColumnFormatType.Currency, DhtmlxGridColumnAggregationType.Total);
            Add(Models.FieldDefinition.UnfundedNeed.ToGridHeaderString(), x => x.UnfundedNeed(), 100, DhtmlxGridColumnFormatType.Currency, DhtmlxGridColumnAggregationType.Total);
            Add("Proposed By", a => a.ProposingPerson.GetFullNameFirstLastAndOrgShortNameAsUrl(), 200);
            Add("Proposed Date", a => a.ProposingDate, 120);
            Add("Submitted Date", a => a.SubmissionDate, 120);
            Add("Last Updated", a => a.GetLastUpdateDate(), 120);
            Add(Models.FieldDefinition.ProjectDescription.ToGridHeaderString(), x => x.ProjectDescription, 300);
        }
    }
}
