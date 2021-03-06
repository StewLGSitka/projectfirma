﻿/*-----------------------------------------------------------------------
<copyright file="EditBasicsViewModel.cs" company="Tahoe Regional Planning Agency and Sitka Technology Group">
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
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using LtInfo.Common;
using LtInfo.Common.Models;
using LtInfo.Common.Mvc;
using ProjectFirma.Web.Common;
using ProjectFirma.Web.Models;
using ProjectFirmaModels.Models;
using ProjectFirma.Web.Security;

namespace ProjectFirma.Web.Views.Tenant
{
    public class EditBasicsViewModel : FormViewModel, IValidatableObject
    {
        [Required]
        public int? TenantID { get; set; }

        [Required]
        [DisplayName("Tenant Display Name")]
        public string TenantDisplayName { get; set; }

        [Required]
        [DisplayName("Tool Display Name")]
        public string ToolDisplayName { get; set; }

        [DisplayName("Primary Contact")]
        public int? PrimaryContactPersonID { get; set; }

        [DisplayName("Minimum Year")]
        [Required(ErrorMessage = "Must specify a Minimum Year")]
        public int? MinimumYear { get; set; }

        [DisplayName("Number Of Taxonomy Tiers To Use")]
        [Required(ErrorMessage = "Must specify a Number Of Taxonomy Tiers To Use")]
        public int? TaxonomyLevelID { get; set; }

        [DisplayName("Associate Performance Measures at Taxonomy Tier")]
        public int? AssociatePerfomanceMeasureTaxonomyLevelID { get; set; }

        [DisplayName("Tenant Style Sheet")]
        [SitkaFileExtensions("css")]
        public HttpPostedFileBase TenantStyleSheetFileResourceData { get; set; }

        [DisplayName("Tenant Banner Logo")]
        [SitkaFileExtensions("jpg|jpeg|gif|png")]
        public HttpPostedFileBase TenantBannerLogoFileResourceData { get; set; }

        [DisplayName("Square Logo")]
        [SitkaFileExtensions("jpg|jpeg|gif|png")]
        public HttpPostedFileBase TenantSquareLogoFileResourceData { get; set; }

        [DisplayName("Map Service Url")]
        [Url]
        public string MapServiceUrl { get; set; }

        [DisplayName("GeospatialArea Layer Name")]
        public string GeospatialAreaLayerName { get; set; }

        [DisplayName("External Data Source Enabled")]
        [Required]
        public bool? ProjectExternalDataSourceEnabled { get; set; }

        [FieldDefinitionDisplay(FieldDefinitionEnum.ShowProposalsToThePublic)]
        [Required]
        public bool? ShowProposalsToThePublic { get; set; }

        [FieldDefinitionDisplay(FieldDefinitionEnum.ShowLeadImplementerLogoOnFactSheet)]
        public bool ShowLeadImplementerLogoOnFactSheet { get; set; }

        [DisplayName("Enable Accomplishments Dashboard")]
        public bool EnableAccomplishmentsDashboard { get; set; }

        /// <summary>
        /// Needed by ModelBinder
        /// </summary>
        public EditBasicsViewModel()
        {
        }

        public EditBasicsViewModel(ProjectFirmaModels.Models.Tenant tenant, TenantAttribute tenantAttribute)
        {
            TenantID = tenant.TenantID;
            TenantDisplayName = tenantAttribute.TenantDisplayName;
            ToolDisplayName = tenantAttribute.ToolDisplayName;
            PrimaryContactPersonID = tenantAttribute.PrimaryContactPersonID;
            TaxonomyLevelID = tenantAttribute.TaxonomyLevelID;
            AssociatePerfomanceMeasureTaxonomyLevelID = tenantAttribute.AssociatePerfomanceMeasureTaxonomyLevelID;
            MinimumYear = tenantAttribute.MinimumYear;
            ProjectExternalDataSourceEnabled = tenantAttribute.ProjectExternalDataSourceEnabled;
            ShowProposalsToThePublic = tenantAttribute.ShowProposalsToThePublic;
            ShowLeadImplementerLogoOnFactSheet = tenantAttribute.ShowLeadImplementerLogoOnFactSheet;
            EnableAccomplishmentsDashboard = tenantAttribute.EnableAccomplishmentsDashboard;
        }

        public void UpdateModel(TenantAttribute attribute, Person currentPerson)
        {
            attribute.TenantDisplayName = TenantDisplayName;
            attribute.ToolDisplayName = ToolDisplayName;
            attribute.ShowProposalsToThePublic = ShowProposalsToThePublic.GetValueOrDefault();
            attribute.ShowLeadImplementerLogoOnFactSheet = ShowLeadImplementerLogoOnFactSheet;
            attribute.EnableAccomplishmentsDashboard = EnableAccomplishmentsDashboard;

            Person primaryContactPerson = null;
            if (PrimaryContactPersonID != null)
            {
                primaryContactPerson = HttpRequestStorage.DatabaseEntities.People.GetPerson(PrimaryContactPersonID.Value);
            }
            attribute.PrimaryContactPerson = primaryContactPerson;
            attribute.TaxonomyLevelID = TaxonomyLevelID ?? ModelObjectHelpers.NotYetAssignedID;
            attribute.AssociatePerfomanceMeasureTaxonomyLevelID = AssociatePerfomanceMeasureTaxonomyLevelID ?? ModelObjectHelpers.NotYetAssignedID;
            attribute.MinimumYear = MinimumYear ?? 0;

            attribute.ProjectExternalDataSourceEnabled = ProjectExternalDataSourceEnabled ?? false;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();

            if (PrimaryContactPersonID != null)
            {
                var primaryContact = HttpRequestStorage.DatabaseEntities.People.GetPerson(PrimaryContactPersonID.Value);
                if (!new FirmaAdminFeature().HasPermissionByPerson(primaryContact))
                {
                    errors.Add(new SitkaValidationResult<EditBasicsViewModel, int?>($"{FieldDefinitionEnum.OrganizationPrimaryContact.ToType().GetFieldDefinitionLabel()} must be an admin.", m => m.PrimaryContactPersonID));
                }
            }

            if (TaxonomyLevelID.Value < AssociatePerfomanceMeasureTaxonomyLevelID.Value)
            {
                errors.Add(new SitkaValidationResult<EditBasicsViewModel, int?>("Cannot choose a Taxonomy Tier that does not exist!", m => m.AssociatePerfomanceMeasureTaxonomyLevelID));
            }

            return errors;
        }
    }
}
