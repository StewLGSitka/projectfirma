﻿/*-----------------------------------------------------------------------
<copyright file="EditProjectViewModel.cs" company="Tahoe Regional Planning Agency and Sitka Technology Group">
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
using ProjectFirma.Web.Common;
using ProjectFirmaModels.Models;
using LtInfo.Common;
using LtInfo.Common.Models;
using ProjectFirma.Web.Models;

namespace ProjectFirma.Web.Views.Shared.ProjectControls
{
    public class EditProjectViewModel : FormViewModel, IValidatableObject
    {
        public int ProjectID { get; set; }

        [FieldDefinitionDisplay(FieldDefinitionEnum.ProjectName)]
        [StringLength(ProjectFirmaModels.Models.Project.FieldLengths.ProjectName)]
        [Required]
        public string ProjectName { get; set; }

        [FieldDefinitionDisplay(FieldDefinitionEnum.ProjectDescription)]
        [StringLength(ProjectFirmaModels.Models.ProjectModelExtensions.MaxLengthForProjectDescription)]
        [Required]
        public string ProjectDescription { get; set; }

        [FieldDefinitionDisplay(FieldDefinitionEnum.ProjectStage)]
        [Required]
        public int ProjectStageID { get; set; }

        [FieldDefinitionDisplay(FieldDefinitionEnum.FundingType)]
        [Required]
        public int FundingTypeID { get; set; }

        [FieldDefinitionDisplay(FieldDefinitionEnum.ImplementationStartYear)]
        public int? ImplementationStartYear { get; set; }

        [FieldDefinitionDisplay(FieldDefinitionEnum.PlanningDesignStartYear)]
        public int? PlanningDesignStartYear { get; set; }

        [FieldDefinitionDisplay(FieldDefinitionEnum.CompletionYear)]
        public int? CompletionYear { get; set; }

        [FieldDefinitionDisplay(FieldDefinitionEnum.TaxonomyLeaf)]
        [Required(ErrorMessage = "This field is required.")]
        public int? TaxonomyLeafID { get; set; }

        [FieldDefinitionDisplay(FieldDefinitionEnum.EstimatedTotalCost)]
        public Money? EstimatedTotalCost { get; set; }

        [FieldDefinitionDisplay(FieldDefinitionEnum.EstimatedAnnualOperatingCost)]
        public Money? EstimatedAnnualOperatingCost { get; set; }

        public bool HasExistingProjectUpdate { get; set; }

        public int? OldProjectStageID { get; set; }

        public ProjectCustomAttributes ProjectCustomAttributes { get; set; }


        /// <summary>
        /// Needed by the ModelBinder
        /// </summary>
        public EditProjectViewModel()
        {
        }

        public EditProjectViewModel(ProjectFirmaModels.Models.Project project, bool hasExistingProjectUpdate)
        {
            TaxonomyLeafID = project.TaxonomyLeafID;
            ProjectID = project.ProjectID;
            ProjectName = project.ProjectName;
            ProjectDescription = project.ProjectDescription;
            ProjectStageID = project.ProjectStageID;
            OldProjectStageID = project.ProjectStageID;
            FundingTypeID = project.FundingTypeID;
            ImplementationStartYear = project.ImplementationStartYear;
            PlanningDesignStartYear = project.PlanningDesignStartYear;
            CompletionYear = project.CompletionYear;
            EstimatedTotalCost = project.EstimatedTotalCost;
            EstimatedAnnualOperatingCost = project.EstimatedAnnualOperatingCost;
            HasExistingProjectUpdate = hasExistingProjectUpdate;
            ProjectCustomAttributes = new ProjectCustomAttributes(project);
        }

        public void UpdateModel(ProjectFirmaModels.Models.Project project, Person currentPerson)
        {
            project.ProjectName = ProjectName;
            project.ProjectDescription = ProjectDescription;
            project.TaxonomyLeafID = TaxonomyLeafID ?? ModelObjectHelpers.NotYetAssignedID;
            project.ProjectStageID = ProjectStageID;
            project.FundingTypeID = FundingTypeID;
            project.ImplementationStartYear = ImplementationStartYear;
            project.PlanningDesignStartYear = PlanningDesignStartYear;
            project.CompletionYear = CompletionYear;

            if (FundingTypeID == (int) FundingTypeEnum.Capital)
            {
                project.EstimatedTotalCost = EstimatedTotalCost;
                project.EstimatedAnnualOperatingCost = null;

            }
            else if (FundingTypeID == (int) FundingTypeEnum.OperationsAndMaintenance)
            {
                project.EstimatedTotalCost = null;
                project.EstimatedAnnualOperatingCost = EstimatedAnnualOperatingCost;
            }

            ProjectCustomAttributes?.UpdateModel(project, currentPerson);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var projects = HttpRequestStorage.DatabaseEntities.Projects.ToList();
            if (!ProjectFirmaModels.Models.ProjectModelExtensions.IsProjectNameUnique(projects, ProjectName, ProjectID))
            {
                yield return new SitkaValidationResult<EditProjectViewModel, string>(
                    FirmaValidationMessages.ProjectNameUnique, m => m.ProjectName);
            }

            if (ImplementationStartYear < PlanningDesignStartYear)
            {
                yield return new SitkaValidationResult<EditProjectViewModel, int?>(
                    FirmaValidationMessages.ImplementationStartYearGreaterThanPlanningDesignStartYear,
                    m => m.ImplementationStartYear);
            }

            if (CompletionYear < ImplementationStartYear)
            {
                yield return new SitkaValidationResult<EditProjectViewModel, int?>(
                    FirmaValidationMessages.CompletionYearGreaterThanEqualToImplementationStartYear,
                    m => m.CompletionYear);
            }

            if (ProjectStageID == ProjectStage.Completed.ProjectStageID && !CompletionYear.HasValue)
            {
                yield return new SitkaValidationResult<EditProjectViewModel, int?>($"Since the {FieldDefinitionEnum.Project.ToType().GetFieldDefinitionLabel()} is in the Completed stage, the Completion year is required", m => m.CompletionYear);
            }

            var isCompletedOrPostImplementation = ProjectStageID == ProjectStage.Completed.ProjectStageID || ProjectStageID == ProjectStage.PostImplementation.ProjectStageID;
            if (isCompletedOrPostImplementation && CompletionYear > DateTime.Now.Year)
            {
                var errorMessage = $"{FieldDefinitionEnum.Project.ToType().GetFieldDefinitionLabel()} is in the Completed or Post-Implementation stage: " +
                                   $"the {FieldDefinitionEnum.CompletionYear.ToType().GetFieldDefinitionLabel()} must be less than or equal to the current year";
                yield return new SitkaValidationResult<EditProjectViewModel, int?>(errorMessage, m => m.CompletionYear);
            }

            if (HasExistingProjectUpdate && OldProjectStageID != ProjectStageID)
            {
                var errorMessage = $"There are updates to this {FieldDefinitionEnum.Project.ToType().GetFieldDefinitionLabel()} that have not been submitted.<br />" +
                                   "Making this change can potentially affect that update in process.<br />" +
                                   $"Please delete the update if you want to change this {FieldDefinitionEnum.Project.ToType().GetFieldDefinitionLabel()}'s stage.";
                yield return new SitkaValidationResult<EditProjectViewModel, int>(errorMessage, m => m.ProjectStageID);
            }
        }
    }
}