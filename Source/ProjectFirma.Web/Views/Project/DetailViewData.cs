﻿/*-----------------------------------------------------------------------
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

using System;
using System.Collections.Generic;
using ProjectFirma.Web.Controllers;
using ProjectFirma.Web.Views.ProjectUpdate;
using ProjectFirmaModels.Models;
using ProjectFirma.Web.Views.Shared.ExpenditureAndBudgetControls;
using ProjectFirma.Web.Views.Shared.ProjectControls;
using ProjectFirma.Web.Views.Shared.ProjectLocationControls;
using ProjectFirma.Web.Views.Shared;
using ProjectFirma.Web.Views.Shared.TextControls;
using LtInfo.Common;
using LtInfo.Common.Mvc;
using ProjectFirma.Web.Common;
using ProjectFirma.Web.Models;
using ProjectFirma.Web.Security;
using ProjectFirma.Web.Views.ProjectFunding;
using ProjectFirma.Web.Views.Shared.PerformanceMeasureControls;
using ProjectFirma.Web.Views.Shared.ProjectDocument;
using ProjectFirma.Web.Views.Shared.ProjectOrganization;

namespace ProjectFirma.Web.Views.Project
{
    public class DetailViewData : ProjectViewData
    {
        public bool UserHasProjectAdminPermissions { get; }
        public bool UserHasEditProjectPermissions { get; }
        public bool UserHasPerformanceMeasureActualManagePermissions { get; }

        public string EditProjectUrl { get; }
        public string EditProjectOrganizationsUrl { get; }
        public List<GeospatialAreaType> GeospatialAreaTypes { get; }
        public string EditSimpleProjectLocationUrl { get; }
        public string EditDetailedProjectLocationUrl { get; }
        public string EditProjectBoundingBoxUrl { get; }
        public string EditPerformanceMeasureExpectedsUrl { get; }
        public string EditPerformanceMeasureActualsUrl { get; }
        public string EditReportedExpendituresUrl { get; }
        public string EditExternalLinksUrl { get; }
        public string EditExpectedFundingUrl { get; }

        public ProjectBasicsViewData ProjectBasicsViewData { get; }
        public ProjectLocationSummaryViewData ProjectLocationSummaryViewData { get; }
        public PerformanceMeasureExpectedSummaryViewData PerformanceMeasureExpectedSummaryViewData { get; }
        public PerformanceMeasureReportedValuesGroupedViewData PerformanceMeasureReportedValuesGroupedViewData { get; }
        public ProjectExpendituresDetailViewData ProjectExpendituresDetailViewData { get; }
        public ImageGalleryViewData ImageGalleryViewData { get; }
        public EntityNotesViewData ProjectNotesViewData { get; }
        public EntityNotesViewData InternalNotesViewData { get; }
        public EntityExternalLinksViewData EntityExternalLinksViewData { get; }

        public ProjectBasicsTagsViewData ProjectBasicsTagsViewData { get; }

        public List<ProjectStage> ProjectStages { get; }
        public string MapFormID { get; }

        public ProjectUpdateBatchGridSpec ProjectUpdateBatchGridSpec { get; }
        public string ProjectUpdateBatchGridName { get; }
        public string ProjectUpdateBatchGridDataUrl { get; }

        public AuditLogsGridSpec AuditLogsGridSpec { get; }
        public string AuditLogsGridName { get; }
        public string AuditLogsGridDataUrl { get; }

        public ProjectNotificationGridSpec ProjectNotificationGridSpec { get; }
        public string ProjectNotificationGridName { get; }
        public string ProjectNotificationGridDataUrl { get; }

        public string EditProjectGeospatialAreaFormID { get; }
        public string EditProjectBoundingBoxFormID { get; }
        public string ProjectStewardCannotEditUrl { get; }
        public string ProjectStewardCannotEditPendingApprovalUrl { get; }
        public ProjectFundingDetailViewData ProjectFundingDetailViewData { get; }

        public string ProjectUpdateButtonText { get; }
        public bool CanLaunchProjectOrProposalWizard { get; }
        public string ProjectWizardUrl { get; }
        public string ProjectListUrl { get; }
        public string BackToProjectsText { get; }
        public List<string> ProjectAlerts { get; }
        public readonly ProjectOrganizationsDetailViewData ProjectOrganizationsDetailViewData;
        public List<ProjectFirmaModels.Models.ClassificationSystem> ClassificationSystems { get; }
        public ProjectDocumentsDetailViewData ProjectDocumentsDetailViewData { get; }
        public IEnumerable<ProjectFirmaModels.Models.ProjectCustomAttributeType> ProjectCustomAttributeTypes { get; }


        public DetailViewData(Person currentPerson, ProjectFirmaModels.Models.Project project, List<ProjectStage> projectStages,
            ProjectBasicsViewData projectBasicsViewData, ProjectLocationSummaryViewData projectLocationSummaryViewData,
            ProjectFundingDetailViewData projectFundingDetailViewData,
            PerformanceMeasureExpectedSummaryViewData performanceMeasureExpectedSummaryViewData,
            PerformanceMeasureReportedValuesGroupedViewData performanceMeasureReportedValuesGroupedViewData,
            ProjectExpendituresDetailViewData projectExpendituresDetailViewData,
            ImageGalleryViewData imageGalleryViewData, EntityNotesViewData projectNotesViewData, EntityNotesViewData internalNotesViewData,
            EntityExternalLinksViewData entityExternalLinksViewData,
            ProjectBasicsTagsViewData projectBasicsTagsViewData, bool userHasProjectAdminPermissions,
            bool userHasEditProjectPermissions, bool userHasProjectUpdatePermissions,
            bool userHasPerformanceMeasureActualManagePermissions, string mapFormID,
            string editSimpleProjectLocationUrl, string editDetailedProjectLocationUrl,
            string editProjectOrganizationsUrl, string editPerformanceMeasureExpectedsUrl,
            string editPerformanceMeasureActualsUrl, string editReportedExpendituresUrl, AuditLogsGridSpec auditLogsGridSpec, string auditLogsGridDataUrl,
            string editExternalLinksUrl, ProjectNotificationGridSpec projectNotificationGridSpec,
            string projectNotificationGridName, string projectNotificationGridDataUrl, bool userCanEditProposal,
            ProjectOrganizationsDetailViewData projectOrganizationsDetailViewData, List<ProjectFirmaModels.Models.ClassificationSystem> classificationSystems,
            string editProjectBoundingBoxFormID,
            IEnumerable<ProjectFirmaModels.Models.ProjectCustomAttributeType> projectCustomAttributeTypes, List<GeospatialAreaType> geospatialAreaTypes)
            : base(currentPerson, project)
        {
            PageTitle = project.GetDisplayName().ToEllipsifiedStringClean(110);
            BreadCrumbTitle = $"{FieldDefinitionEnum.Project.ToType().GetFieldDefinitionLabel()} Detail";

            ProjectStages = projectStages;

            EditProjectUrl = project.GetEditUrl();
            UserHasProjectAdminPermissions = userHasProjectAdminPermissions;
            UserHasEditProjectPermissions = userHasEditProjectPermissions;
            UserHasPerformanceMeasureActualManagePermissions = userHasPerformanceMeasureActualManagePermissions;

            var projectAlerts = new List<string>();
            var proposedProjectListUrl = SitkaRoute<ProjectController>.BuildUrlFromExpression(c => c.Proposed());
            var backToAllProposalsText = "Back to all Proposals";
            var pendingProjectsListUrl = SitkaRoute<ProjectController>.BuildUrlFromExpression(c => c.Pending());
            var backToAllPendingProjectsText = $"Back to all Pending {FieldDefinitionEnum.Project.ToType().GetFieldDefinitionLabelPluralized()}";

            if (project.IsRejected())
            {
                var projectApprovalStatus = project.ProjectApprovalStatus;
                ProjectUpdateButtonText =
                    projectApprovalStatus == ProjectApprovalStatus.Draft ||
                    projectApprovalStatus == ProjectApprovalStatus.Returned
                        ? $"Edit Pending {FieldDefinitionEnum.Project.ToType().GetFieldDefinitionLabel()}"
                        : $"Review Pending {FieldDefinitionEnum.Project.ToType().GetFieldDefinitionLabel()}";
                ProjectWizardUrl =
                    SitkaRoute<ProjectCreateController>.BuildUrlFromExpression(x => x.EditBasics(project.ProjectID));
                CanLaunchProjectOrProposalWizard = userCanEditProposal;
                if (project.IsProposal())
                {
                    ProjectListUrl = proposedProjectListUrl;
                    BackToProjectsText = backToAllProposalsText;
                }
                else if (project.IsPendingProject())
                {
                    ProjectListUrl = pendingProjectsListUrl;
                    BackToProjectsText = backToAllPendingProjectsText;
                }

                if (userHasProjectAdminPermissions || currentPerson.CanStewardProject(project))
                {
                    projectAlerts.Add(
                        $"This {FieldDefinitionEnum.Project.ToType().GetFieldDefinitionLabel()} was rejected and can no longer be edited. It can be deleted, or preserved for archival purposes.");
                }
            }            
            else if (project.IsProposal())
            {
                var projectApprovalStatus = project.ProjectApprovalStatus;
                ProjectUpdateButtonText =
                    projectApprovalStatus == ProjectApprovalStatus.Draft ||
                    projectApprovalStatus == ProjectApprovalStatus.Returned
                        ? "Edit Proposal"
                        : "Review Proposal";
                ProjectWizardUrl =
                    SitkaRoute<ProjectCreateController>.BuildUrlFromExpression(x => x.EditBasics(project.ProjectID));
                CanLaunchProjectOrProposalWizard = userCanEditProposal;
                ProjectListUrl = proposedProjectListUrl;
                BackToProjectsText = backToAllProposalsText;
                if (userHasProjectAdminPermissions || currentPerson.CanStewardProject(project))
                {
                    projectAlerts.Add(
                        $"This {FieldDefinitionEnum.Project.ToType().GetFieldDefinitionLabel()} is in the Proposal stage. Any edits to this {FieldDefinitionEnum.Project.ToType().GetFieldDefinitionLabel()} must be made using the Add New {FieldDefinitionEnum.Project.ToType().GetFieldDefinitionLabel()} workflow.");
                }
            }
            else if (project.IsPendingProject())
            {
                var projectApprovalStatus = project.ProjectApprovalStatus;
                ProjectUpdateButtonText =
                    projectApprovalStatus == ProjectApprovalStatus.Draft ||
                    projectApprovalStatus == ProjectApprovalStatus.Returned
                        ? $"Edit Pending {FieldDefinitionEnum.Project.ToType().GetFieldDefinitionLabel()}"
                        : $"Review Pending {FieldDefinitionEnum.Project.ToType().GetFieldDefinitionLabel()}";
                ProjectWizardUrl =
                    SitkaRoute<ProjectCreateController>.BuildUrlFromExpression(x => x.EditBasics(project.ProjectID));
                CanLaunchProjectOrProposalWizard = userCanEditProposal;
                ProjectListUrl = pendingProjectsListUrl;
                BackToProjectsText = backToAllPendingProjectsText;
                if (userHasProjectAdminPermissions || currentPerson.CanStewardProject(project))
                {
                    projectAlerts.Add(
                        $"This {FieldDefinitionEnum.Project.ToType().GetFieldDefinitionLabel()} is pending. Any edits to this {FieldDefinitionEnum.Project.ToType().GetFieldDefinitionLabel()} must be made using the Add New {FieldDefinitionEnum.Project.ToType().GetFieldDefinitionLabel()} workflow.");
                }
            }
            else
            {
                var latestUpdateState = project.GetLatestUpdateState();
                ProjectUpdateButtonText =
                    latestUpdateState == ProjectUpdateState.Submitted ||
                    latestUpdateState == ProjectUpdateState.Returned
                        ? "Review Update"
                        : $"Update {FieldDefinitionEnum.Project.ToType().GetFieldDefinitionLabel()}";
                ProjectWizardUrl = project.GetProjectUpdateUrl();
                CanLaunchProjectOrProposalWizard = userHasProjectUpdatePermissions;
                ProjectListUrl = FullProjectListUrl;
                BackToProjectsText = $"Back to all {FieldDefinitionEnum.Project.ToType().GetFieldDefinitionLabelPluralized()}";


                if (currentPerson.IsPersonAProjectOwnerWhoCanStewardProjects())
                {
                    if (currentPerson.CanStewardProject(project))
                    {
                        projectAlerts.Add(
                            $"You are a {FieldDefinitionEnum.ProjectSteward.ToType().GetFieldDefinitionLabel()} for this {FieldDefinitionEnum.Project.ToType().GetFieldDefinitionLabel()}. You may edit this {FieldDefinitionEnum.Project.ToType().GetFieldDefinitionLabel()} by using the <i class=\"glyphicon glyphicon-edit\"></i> icon on each panel.<br/>");
                    }
                    else
                    {
                        projectAlerts.Add(
                            $"You are a {FieldDefinitionEnum.ProjectSteward.ToType().GetFieldDefinitionLabel()}, but not for this {FieldDefinitionEnum.Project.ToType().GetFieldDefinitionLabel()}. You may only edit {FieldDefinitionEnum.Project.ToType().GetFieldDefinitionLabelPluralized()} that are associated with your {FieldDefinitionEnum.ProjectStewardshipArea.ToType().GetFieldDefinitionLabel()}.");
                    }
                }
            }

            
            if (ProjectModelExtensions.GetLatestNotApprovedUpdateBatch(project) != null)
            {
                if (userHasProjectAdminPermissions || currentPerson.CanStewardProject(project))
                {
                    projectAlerts.Add($"This {FieldDefinitionEnum.Project.ToType().GetFieldDefinitionLabel()} has an Update in progress. Changes made through this page will be overwritten when the Update is approved.");
                }
                else
                {
                    projectAlerts.Add($"This {FieldDefinitionEnum.Project.ToType().GetFieldDefinitionLabel()} has an Update in progress.");
                }
            }

            ProjectAlerts = projectAlerts;

            ProjectBasicsViewData = projectBasicsViewData;
            ProjectBasicsTagsViewData = projectBasicsTagsViewData;

            ProjectLocationSummaryViewData = projectLocationSummaryViewData;
            MapFormID = mapFormID;
            EditSimpleProjectLocationUrl = editSimpleProjectLocationUrl;
            EditDetailedProjectLocationUrl = editDetailedProjectLocationUrl;

            EditProjectBoundingBoxUrl = SitkaRoute<ProjectLocationController>.BuildUrlFromExpression(c => c.EditProjectBoundingBox(project));
            EditProjectBoundingBoxFormID = editProjectBoundingBoxFormID;
            ProjectCustomAttributeTypes = projectCustomAttributeTypes;

            EditProjectOrganizationsUrl = editProjectOrganizationsUrl;

            PerformanceMeasureExpectedSummaryViewData = performanceMeasureExpectedSummaryViewData;
            EditPerformanceMeasureExpectedsUrl = editPerformanceMeasureExpectedsUrl;

            PerformanceMeasureReportedValuesGroupedViewData = performanceMeasureReportedValuesGroupedViewData;
            EditPerformanceMeasureActualsUrl = editPerformanceMeasureActualsUrl;

            ProjectFundingDetailViewData = projectFundingDetailViewData;
            EditExpectedFundingUrl =
                SitkaRoute<ProjectFundingSourceRequestController>.BuildUrlFromExpression(c =>
                    c.EditProjectFundingSourceRequestsForProject(project));

            ProjectExpendituresDetailViewData = projectExpendituresDetailViewData;
            EditReportedExpendituresUrl = editReportedExpendituresUrl;
            GeospatialAreaTypes = geospatialAreaTypes;
            EditExternalLinksUrl = editExternalLinksUrl;
            ImageGalleryViewData = imageGalleryViewData;

            ProjectNotesViewData = projectNotesViewData;
            InternalNotesViewData = internalNotesViewData;

            EntityExternalLinksViewData = entityExternalLinksViewData;

            ProjectUpdateBatchGridSpec = new ProjectUpdateBatchGridSpec
            {
                ObjectNameSingular = $"{FieldDefinitionEnum.Project.ToType().GetFieldDefinitionLabel()} Update",
                ObjectNamePlural = $"{FieldDefinitionEnum.Project.ToType().GetFieldDefinitionLabel()} Updates",
                SaveFiltersInCookie = true
            };
            ProjectUpdateBatchGridName = "projectUpdateBatch";
            ProjectUpdateBatchGridDataUrl =
                SitkaRoute<ProjectController>.BuildUrlFromExpression(x =>
                    x.ProjectUpdateBatchGridJsonData(project.ProjectID));

            AuditLogsGridSpec = auditLogsGridSpec;
            AuditLogsGridName = "projectAuditLogsGrid";
            AuditLogsGridDataUrl = auditLogsGridDataUrl;

            ProjectNotificationGridSpec = projectNotificationGridSpec;
            ProjectNotificationGridName = projectNotificationGridName;
            ProjectNotificationGridDataUrl = projectNotificationGridDataUrl;
            ProjectOrganizationsDetailViewData = projectOrganizationsDetailViewData;
           
            EditProjectGeospatialAreaFormID = ProjectGeospatialAreaController.GetEditProjectGeospatialAreasFormID();

            ProjectStewardCannotEditUrl =
                SitkaRoute<ProjectController>.BuildUrlFromExpression(c => c.ProjectStewardCannotEdit());
            ProjectStewardCannotEditPendingApprovalUrl =
                SitkaRoute<ProjectController>.BuildUrlFromExpression(c =>
                    c.ProjectStewardCannotEditPendingApproval(project));

            ClassificationSystems = classificationSystems;

            //ProjectDocumentsDetailViewData = new ProjectDocumentsDetailViewData(project, currentPerson, !project.IsProposal());
            ProjectDocumentsDetailViewData = new ProjectDocumentsDetailViewData(
                EntityDocument.CreateFromEntityDocument(project.ProjectDocuments),
                SitkaRoute<ProjectDocumentController>.BuildUrlFromExpression(x => x.New(project)), project.ProjectName,
                new ProjectEditAsAdminFeature().HasPermission(currentPerson, project).HasPermission);
        }

        public string StringToDateString(string stringDate)
        {
            return DateTime.TryParse(stringDate, out var date) ? date.ToShortDateString() : null;
        }
    }
}
