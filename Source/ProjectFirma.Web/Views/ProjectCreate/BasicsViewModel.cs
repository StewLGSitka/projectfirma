﻿/*-----------------------------------------------------------------------
<copyright file="BasicsViewModel.cs" company="Tahoe Regional Planning Agency and Sitka Technology Group">
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
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using LtInfo.Common;
using ProjectFirma.Web.Common;
using ProjectFirma.Web.Models;
using LtInfo.Common.Models;

namespace ProjectFirma.Web.Views.ProjectCreate
{
    public class BasicsViewModel : FormViewModel, IValidatableObject
    {
        public int? ProjectID { get; set; }

        [FieldDefinitionDisplay(FieldDefinitionEnum.TaxonomyLeaf)]
        [Required]
        public int? TaxonomyLeafID { get; set; }

        [FieldDefinitionDisplay(FieldDefinitionEnum.ProjectName)]
        [StringLength(Models.Project.FieldLengths.ProjectName)]
        [Required]
        public string ProjectName { get; set; }

        [FieldDefinitionDisplay(FieldDefinitionEnum.ProjectDescription)]
        [StringLength(Models.ProjectModelExtensions.MaxLengthForProjectDescription)]
        [Required]
        public string ProjectDescription { get; set; }

        [FieldDefinitionDisplay(FieldDefinitionEnum.ProjectStage)]
        [Required]
        public int? ProjectStageID { get; set; }

        [FieldDefinitionDisplay(FieldDefinitionEnum.PlanningDesignStartYear)]
        [Required]
        public int? PlanningDesignStartYear { get; set; }
        
        [FieldDefinitionDisplay(FieldDefinitionEnum.ImplementationStartYear)]
        [Required]
        public int? ImplementationStartYear { get; set; }

        [FieldDefinitionDisplay(FieldDefinitionEnum.CompletionYear)]
        public int? CompletionYear { get; set; }

        [FieldDefinitionDisplay(FieldDefinitionEnum.EstimatedTotalCost)]
        public Money? EstimatedTotalCost { get; set; }
        
        [FieldDefinitionDisplay(FieldDefinitionEnum.EstimatedAnnualOperatingCost)]
        public Money? EstimatedAnnualOperatingCost { get; set; }
      
        [FieldDefinitionDisplay(FieldDefinitionEnum.FundingType)]
        [Required]
        public int FundingTypeID { get; set; }

        public int? ImportExternalProjectStagingID { get; set; }

        public ProjectCustomAttributes ProjectCustomAttributes { get; set; }

        /// <summary>
        /// Needed by the ModelBinder
        /// </summary>
        public BasicsViewModel()
        {
        }

        public BasicsViewModel(Models.Project project)
        {
            TaxonomyLeafID = project.TaxonomyLeafID;
            ProjectID = project.ProjectID;
            ProjectName = project.ProjectName;
            ProjectDescription = project.ProjectDescription;
            ProjectStageID = project.ProjectStageID;            
            FundingTypeID = project.FundingTypeID;
            EstimatedTotalCost = project.EstimatedTotalCost;
            EstimatedAnnualOperatingCost = project.EstimatedAnnualOperatingCost;
            PlanningDesignStartYear = project.PlanningDesignStartYear;
            ImplementationStartYear = project.ImplementationStartYear;
            CompletionYear = project.CompletionYear;
            ProjectCustomAttributes = new ProjectCustomAttributes(project);
        }

        public void UpdateModel(Models.Project project, Person person)
        {
            if (ImportExternalProjectStagingID.HasValue)
            {
                var importExternalProjectStagingToDelete = HttpRequestStorage.DatabaseEntities.ImportExternalProjectStagings.Single(x =>
                    x.ImportExternalProjectStagingID == ImportExternalProjectStagingID);
                HttpRequestStorage.DatabaseEntities.AllImportExternalProjectStagings.Remove(importExternalProjectStagingToDelete);
            }

            project.ProposingPersonID = person.PersonID;
            project.TaxonomyLeafID = TaxonomyLeafID ?? ModelObjectHelpers.NotYetAssignedID;
            project.ProjectID = ProjectID ?? ModelObjectHelpers.NotYetAssignedID;
            project.ProjectName = ProjectName;
            project.ProjectDescription = ProjectDescription;
            project.ProjectStageID = ProjectStageID ?? ModelObjectHelpers.NotYetAssignedID;
            project.FundingTypeID = FundingTypeID;
            if (FundingTypeID == FundingType.Capital.FundingTypeID)
            {
                project.EstimatedTotalCost = EstimatedTotalCost;
                project.EstimatedAnnualOperatingCost = null;
                
            }
            else if (FundingTypeID == FundingType.OperationsAndMaintenance.FundingTypeID)
            {
                project.EstimatedTotalCost = null;
                project.EstimatedAnnualOperatingCost = EstimatedAnnualOperatingCost;
            }
            
            project.PlanningDesignStartYear = PlanningDesignStartYear;
            project.ImplementationStartYear = ImplementationStartYear;
            project.CompletionYear = CompletionYear;
            ProjectCustomAttributes?.UpdateModel(project, person);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
           return GetValidationResults();
        }

        public IEnumerable<ValidationResult> GetValidationResults()
        {
            var projects = HttpRequestStorage.DatabaseEntities.Projects.ToList();

            if (TaxonomyLeafID == -1)
            {
                yield return new SitkaValidationResult<BasicsViewModel, int?>($"{MultiTenantHelpers.GetTaxonomyLeafDisplayNameForProject()} is required.", m => m.TaxonomyLeafID);
            }

            if (!Models.ProjectModelExtensions.IsProjectNameUnique(projects, ProjectName, ProjectID))
            {
                yield return new SitkaValidationResult<BasicsViewModel, string>(FirmaValidationMessages.ProjectNameUnique, m => m.ProjectName);
            }

            if (ImplementationStartYear < PlanningDesignStartYear)
            {
                yield return new SitkaValidationResult<BasicsViewModel, int?>(FirmaValidationMessages.ImplementationStartYearGreaterThanPlanningDesignStartYear, m => m.ImplementationStartYear);
            }

            if (CompletionYear < ImplementationStartYear)
            {
                yield return new SitkaValidationResult<BasicsViewModel, int?>(FirmaValidationMessages.CompletionYearGreaterThanEqualToImplementationStartYear, m => m.CompletionYear);
            }

            if (CompletionYear < PlanningDesignStartYear)
            {
                yield return new SitkaValidationResult<BasicsViewModel, int?>(FirmaValidationMessages.CompletionYearGreaterThanEqualToPlanningDesignStartYear, m => m.CompletionYear);
            }

            var currentYear = FirmaDateUtilities.CalculateCurrentYearToUseForUpToAllowableInputInReporting();
            if (ProjectStageID == ProjectStage.Implementation.ProjectStageID)
            {
                if (ImplementationStartYear > currentYear)
                {
                    yield return new SitkaValidationResult<BasicsViewModel, int?>(
                        FirmaValidationMessages.ImplementationYearMustBePastOrPresentForImplementationProjects,
                        m => m.ImplementationStartYear);
                }
            }
            
            if (ImplementationStartYear == null && ProjectStageID != ProjectStage.Terminated.ProjectStageID && ProjectStageID != ProjectStage.Deferred.ProjectStageID)
            {
                yield return new SitkaValidationResult<BasicsViewModel, int?>(
                    $"Implementation year is required when the {Models.FieldDefinition.Project.GetFieldDefinitionLabel()} stage is not Deferred or Terminated",
                    m => m.ImplementationStartYear);
            }

            if (ProjectStageID == ProjectStage.Completed.ProjectStageID && !CompletionYear.HasValue)
            {
                yield return new SitkaValidationResult<BasicsViewModel, int?>($"Since the {Models.FieldDefinition.Project.GetFieldDefinitionLabel()} is in the Completed stage, the Completion year is required", m => m.CompletionYear);
            }

            if ((ProjectStageID == ProjectStage.Completed.ProjectStageID ||
                ProjectStageID == ProjectStage.PostImplementation.ProjectStageID) && CompletionYear > currentYear)
            {
                yield return new SitkaValidationResult<BasicsViewModel, int?>(FirmaValidationMessages.CompletionYearMustBePastOrPresentForCompletedProjects, m => m.CompletionYear);
            }

            if (ProjectStageID == ProjectStage.PlanningDesign.ProjectStageID && PlanningDesignStartYear > currentYear)
            {
                yield return new SitkaValidationResult<BasicsViewModel, int?>(
                    $"Since the {Models.FieldDefinition.Project.GetFieldDefinitionLabel()} is in the Planning / Design stage, the Planning / Design start year must be less than or equal to the current year",
                    m => m.PlanningDesignStartYear);
            }
        }
    }
}
