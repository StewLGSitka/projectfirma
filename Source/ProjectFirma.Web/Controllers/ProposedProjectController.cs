﻿/*-----------------------------------------------------------------------
<copyright file="ProposedProjectController.cs" company="Tahoe Regional Planning Agency">
Copyright (c) Tahoe Regional Planning Agency. All rights reserved.
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
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web.Mvc;
using ProjectFirma.Web.Security;
using ProjectFirma.Web.Common;
using ProjectFirma.Web.Models;
using ProjectFirma.Web.Views.Map;
using ProjectFirma.Web.Views.ProposedProject;
using ProjectFirma.Web.Views.Shared.ProjectControls;
using ProjectFirma.Web.Views.Shared.ProjectLocationControls;
using ProjectFirma.Web.Views.Shared;
using ProjectFirma.Web.Views.Shared.TextControls;
using LtInfo.Common;
using LtInfo.Common.DbSpatial;
using LtInfo.Common.DesignByContract;
using LtInfo.Common.Models;
using LtInfo.Common.MvcResults;
using ProjectFirma.Web.Views.Shared.PerformanceMeasureControls;
using ProjectFirma.Web.Views.Shared.ProjectWatershedControls;

namespace ProjectFirma.Web.Controllers
{
    public class ProposedProjectController : FirmaBaseController
    {
        [ProposedProjectViewFeature]
        public ViewResult Detail(ProposedProjectPrimaryKey proposedProjectPrimaryKey)
        {
            var proposedProject = proposedProjectPrimaryKey.EntityObject;
            var mapDivID = $"proposedProject_{proposedProject.ProposedProjectID}_Map";
            var projectLocationSummaryMapInitJson = new ProjectLocationSummaryMapInitJson(proposedProject, mapDivID, true);
            var projectLocationSummaryViewData = new ProjectLocationSummaryViewData(proposedProject, projectLocationSummaryMapInitJson);
            var mapFormID = GenerateEditProjectLocationSimpleFormID(proposedProject);
            var performanceMeasureExpectedsSummaryViewData =
                new PerformanceMeasureExpectedSummaryViewData(new List<IPerformanceMeasureValue>(proposedProject.PerformanceMeasureExpectedProposeds));
            var entityNotesViewData = new EntityNotesViewData(EntityNote.CreateFromEntityNote(new List<IEntityNote>(proposedProject.ProposedProjectNotes)),
                SitkaRoute<ProposedProjectController>.BuildUrlFromExpression(x => x.NewNote(proposedProject)),
                proposedProject.DisplayName,
                true);

            var galleryName = $"ProjectImage{proposedProject.ProposedProjectID}";
            var galleryImages = proposedProject.ProposedProjectImages.ToList();
            var imageGalleryViewData = new ImageGalleryViewData(CurrentPerson, galleryName, galleryImages, false, string.Empty, string.Empty, true, x => x.CaptionOnFullView, "Photo");

            var assessmentGoals = HttpRequestStorage.DatabaseEntities.AssessmentGoals.ToList();
            var goalsAsFancyTreeNodes = assessmentGoals.Select(x => x.ToFancyTreeNode(new List<IQuestionAnswer>(proposedProject.ProposedProjectAssessmentQuestions.ToList()))).ToList();
            var assessmentTreeViewData = new AssessmentTreeViewData(goalsAsFancyTreeNodes);


            var viewData = new DetailViewData(CurrentPerson,
                proposedProject,
                projectLocationSummaryViewData,
                performanceMeasureExpectedsSummaryViewData,
                imageGalleryViewData,
                entityNotesViewData,
                mapFormID,
                assessmentTreeViewData,
                HttpRequestStorage.Tenant);
            return RazorView<Detail, DetailViewData>(viewData);
        }

        [ProposedProjectsViewListFeature]
        public ViewResult Index()
        {
            var firmaPage = FirmaPage.GetFirmaPageByPageType(FirmaPageType.ProposedProjects);
            var viewData = new IndexViewData(CurrentPerson, firmaPage);
            return RazorView<Index, IndexViewData>(viewData);
        }

        [ProposedProjectsViewListFeature]
        public GridJsonNetJObjectResult<ProposedProject> IndexGridJsonData()
        {
            var gridSpec = new ProposedProjectGridSpec(CurrentPerson);
            var watersheds = HttpRequestStorage.DatabaseEntities.Watersheds.GetWatershedsWithGeospatialFeatures();
            var stateProvinces = HttpRequestStorage.DatabaseEntities.StateProvinces.ToList();
            var proposedProjects = HttpRequestStorage.DatabaseEntities.ProposedProjects.GetProposedProjectsWithGeoSpatialProperties(watersheds,
                stateProvinces,
                x => x.IsEditableToThisPerson(CurrentPerson)).Where(x1 => x1.ProposedProjectState != ProposedProjectState.Approved && x1.ProposedProjectState != ProposedProjectState.Rejected).ToList();
            var gridJsonNetJObjectResult = new GridJsonNetJObjectResult<ProposedProject>(proposedProjects, gridSpec);
            return gridJsonNetJObjectResult;
        }

        [ProposedProjectCreateNewFeature]
        public ViewResult Instructions(int? proposedProjectID)
        {
            var firmaPageType = FirmaPageType.ToType(FirmaPageTypeEnum.ProposeProjectInstructions);
            var firmaPage = FirmaPage.GetFirmaPageByPageType(firmaPageType);

            if (proposedProjectID.HasValue)
            {
                var proposedProject = HttpRequestStorage.DatabaseEntities.ProposedProjects.GetProposedProject(proposedProjectID.Value);

                var proposalSectionsStatus = new ProposalSectionsStatus(proposedProject);
                var viewData = new InstructionsViewData(CurrentPerson, proposedProject, proposalSectionsStatus, firmaPage);

                return RazorView<Instructions, InstructionsViewData>(viewData);
            }
            else
            {
                var viewData = new InstructionsViewData(CurrentPerson, firmaPage);
                return RazorView<Instructions, InstructionsViewData>(viewData);
            }
        }

        [HttpGet]
        [ProposedProjectCreateNewFeature]
        public ViewResult CreateAndEditBasics()
        {
            return ViewCreateAndEditBasics(new BasicsViewModel(CurrentPerson.Organization), null);
        }

        private ViewResult ViewCreateAndEditBasics(BasicsViewModel viewModel, ProposedProject proposedProject)
        {
            var taxonomyTierOnes = HttpRequestStorage.DatabaseEntities.TaxonomyTierOnes;
            var organizations = HttpRequestStorage.DatabaseEntities.Organizations.GetActiveOrganizations();
            var primaryContactPeople = HttpRequestStorage.DatabaseEntities.People.GetActivePeople();
            var defaultPrimaryContactPerson = proposedProject?.GetPrimaryContact() ?? CurrentPerson.Organization.PrimaryContactPerson;
            var viewData = new BasicsViewData(CurrentPerson, organizations, primaryContactPeople, defaultPrimaryContactPerson, FundingType.All, taxonomyTierOnes, MultiTenantHelpers.GetCanApproveProjectsOrganizationRelationship(), MultiTenantHelpers.GetIsPrimaryContactOrganizationRelationship());

            return RazorView<Basics, BasicsViewData, BasicsViewModel>(viewData, viewModel);
        }

        [HttpGet]
        [ProposedProjectEditFeature]
        public ViewResult EditBasics(ProposedProjectPrimaryKey proposedProjectPrimaryKey)
        {
            var proposedProject = proposedProjectPrimaryKey.EntityObject;
            var viewModel = new BasicsViewModel(proposedProject);
            return ViewEditBasics(proposedProjectPrimaryKey, viewModel);
        }

        private ViewResult ViewEditBasics(ProposedProjectPrimaryKey proposedProjectPrimaryKey, BasicsViewModel viewModel)
        {
            
            var proposedProject = proposedProjectPrimaryKey.EntityObject;
            var proposalSectionsStatus = new ProposalSectionsStatus(proposedProject);
            proposalSectionsStatus.IsBasicsSectionComplete = ModelState.IsValid && proposalSectionsStatus.IsBasicsSectionComplete;
            
            var taxonomyTierOnes = HttpRequestStorage.DatabaseEntities.TaxonomyTierOnes;
            var organizations = HttpRequestStorage.DatabaseEntities.Organizations.GetActiveOrganizations();
            var primaryContacts = HttpRequestStorage.DatabaseEntities.People.GetActivePeople();
            var defaultPrimaryContactPerson = proposedProject?.GetPrimaryContact() ?? CurrentPerson.Organization.PrimaryContactPerson;
            var viewData = new BasicsViewData(CurrentPerson, proposedProject, proposalSectionsStatus, taxonomyTierOnes, organizations, primaryContacts, defaultPrimaryContactPerson, FundingType.All, MultiTenantHelpers.GetCanApproveProjectsOrganizationRelationship(), MultiTenantHelpers.GetIsPrimaryContactOrganizationRelationship());

            return RazorView<Basics, BasicsViewData, BasicsViewModel>(viewData, viewModel);
        }

        [HttpPost]
        [ProposedProjectCreateNewFeature]
        [AutomaticallyCallEntityFrameworkSaveChangesWhenModelValid]
        public ActionResult CreateAndEditBasics(BasicsViewModel viewModel)
        {
            var proposedProject = new ProposedProject(viewModel.ProjectName,
                viewModel.ProjectDescription,
                CurrentPerson.PersonID,
                DateTime.Now,
                (int) ProjectLocationSimpleType.None.ToEnum,
                viewModel.FundingTypeID,
                (int) ProposedProjectState.Draft.ToEnum);

            return SaveProposedProjectAndCreateAuditEntry(proposedProject, viewModel);
        }

        [HttpPost]
        [ProposedProjectEditFeature]
        [AutomaticallyCallEntityFrameworkSaveChangesWhenModelValid]
        public ActionResult EditBasics(ProposedProjectPrimaryKey proposedProjectPrimaryKey, BasicsViewModel viewModel)
        {
            var proposedProject = proposedProjectPrimaryKey.EntityObject;
            return SaveProposedProjectAndCreateAuditEntry(proposedProject, viewModel);
        }

        private ActionResult SaveProposedProjectAndCreateAuditEntry(ProposedProject proposedProject, BasicsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                SetErrorForDisplay($"Could not save {FieldDefinition.ProposedProject.GetFieldDefinitionLabel()}: Please fix validation errors to proceed.");
                return ModelObjectHelpers.IsRealPrimaryKeyValue(proposedProject.PrimaryKey) ? ViewEditBasics(proposedProject, viewModel) : ViewCreateAndEditBasics(viewModel, proposedProject);
            }

            if (!ModelObjectHelpers.IsRealPrimaryKeyValue(proposedProject.PrimaryKey))
            {
                HttpRequestStorage.DatabaseEntities.AllProposedProjects.Add(proposedProject);
            }

            viewModel.UpdateModel(proposedProject, CurrentPerson);

            SetProposedProjectOrganizationForRelationshipType(proposedProject, viewModel.PrimaryContactOrganizationID, MultiTenantHelpers.GetIsPrimaryContactOrganizationRelationship());
            SetProposedProjectOrganizationForRelationshipType(proposedProject, viewModel.ApprovingProjectsOrganizationID, MultiTenantHelpers.GetCanApproveProjectsOrganizationRelationship());

            HttpRequestStorage.DatabaseEntities.SaveChanges();

            var auditLog = new AuditLog(CurrentPerson,
                DateTime.Now,
                AuditLogEventType.Added,
                "ProposedProject",
                proposedProject.ProposedProjectID,
                "ProposedProjectID",
                proposedProject.ProposedProjectID.ToString())
            {
                ProposedProjectID = proposedProject.ProposedProjectID,
                AuditDescription = $"ProposedProject: created {proposedProject.DisplayName}"
            };
            HttpRequestStorage.DatabaseEntities.AllAuditLogs.Add(auditLog);
            SetMessageForDisplay($"{FieldDefinition.ProposedProject.GetFieldDefinitionLabel()} succesfully saved.");

            return RedirectToAction(viewModel.AutoAdvance ? new SitkaRoute<ProposedProjectController>(x => x.EditLocationSimple(proposedProject.PrimaryKey)) : new SitkaRoute<ProposedProjectController>(x => x.EditBasics(proposedProject.PrimaryKey)));
        }

        private static void SetProposedProjectOrganizationForRelationshipType(ProposedProject proposedProject, int? organizationID, RelationshipType relationshipType)
        {
            if (relationshipType != null && organizationID.HasValue)
            {
                var organization = HttpRequestStorage.DatabaseEntities.Organizations.GetOrganization(organizationID.Value);

                var proposedProjectPrimaryContactOrganization =
                    proposedProject.ProposedProjectOrganizations.SingleOrDefault(x =>
                        x.RelationshipTypeID == relationshipType.RelationshipTypeID);
                if (relationshipType.OrganizationTypeRelationshipTypes.Any(x =>
                    x.OrganizationTypeID == organization.OrganizationTypeID))
                {
                    if (proposedProjectPrimaryContactOrganization == null)
                    {
                        proposedProject.ProposedProjectOrganizations.Add(new ProposedProjectOrganization(
                            proposedProject,
                            organization, relationshipType));
                    }
                    else
                    {
                        proposedProjectPrimaryContactOrganization.OrganizationID =
                            organizationID.Value;
                    }
                }
                else
                {
                    proposedProjectPrimaryContactOrganization?.DeleteProposedProjectOrganization();
                }
            }
        }

        [HttpGet]
        [PerformanceMeasureExpectedProposedFeature]
        public ViewResult EditExpectedPerformanceMeasureValues(ProposedProjectPrimaryKey projectPrimaryKey)
        {
            var proposedProject = projectPrimaryKey.EntityObject;
            var viewModel = new ExpectedPerformanceMeasureValuesViewModel(proposedProject);
            return ViewEditExpectedPerformanceMeasureValues(proposedProject, viewModel);
        }

        private ViewResult ViewEditExpectedPerformanceMeasureValues(ProposedProject proposedProject, ExpectedPerformanceMeasureValuesViewModel viewModel)
        {
            var performanceMeasures = HttpRequestStorage.DatabaseEntities.PerformanceMeasures.ToList();
            var proposalSectionsStatus = new ProposalSectionsStatus(proposedProject);
            proposalSectionsStatus.IsPerformanceMeasureSectionComplete = ModelState.IsValid && proposalSectionsStatus.IsPerformanceMeasureSectionComplete;

            var editPerformanceMeasureExpectedsViewData = new EditPerformanceMeasureExpectedViewData(proposedProject, performanceMeasures);
            var viewData = new ExpectedPerformanceMeasureValuesViewData(CurrentPerson, proposedProject, proposalSectionsStatus, editPerformanceMeasureExpectedsViewData);
            return RazorView<ExpectedPerformanceMeasureValues, ExpectedPerformanceMeasureValuesViewData, ExpectedPerformanceMeasureValuesViewModel>(viewData, viewModel);
        }

        [HttpPost]
        [PerformanceMeasureExpectedProposedFeature]
        [AutomaticallyCallEntityFrameworkSaveChangesWhenModelValid]
        public ActionResult EditExpectedPerformanceMeasureValues(ProposedProjectPrimaryKey projectPrimaryKey, ExpectedPerformanceMeasureValuesViewModel viewModel)
        {
            var proposedProject = projectPrimaryKey.EntityObject;
            if (!ModelState.IsValid)
            {
                ShowValidationErrors(viewModel.GetValidationResults().ToList());
                return ViewEditExpectedPerformanceMeasureValues(proposedProject, viewModel);
            }
            var currentPerformanceMeasureExpectedProposeds = proposedProject.PerformanceMeasureExpectedProposeds.ToList();

            HttpRequestStorage.DatabaseEntities.PerformanceMeasureExpectedProposeds.Load();
            var allPerformanceMeasureExpectedProposeds = HttpRequestStorage.DatabaseEntities.AllPerformanceMeasureExpectedProposeds.Local;

            HttpRequestStorage.DatabaseEntities.PerformanceMeasureExpectedSubcategoryOptionProposeds.Load();
            var allPerformanceMeasureExpectedSubcategoryOptionProposeds = HttpRequestStorage.DatabaseEntities.AllPerformanceMeasureExpectedSubcategoryOptionProposeds.Local;

            viewModel.UpdateModel(currentPerformanceMeasureExpectedProposeds, allPerformanceMeasureExpectedProposeds, allPerformanceMeasureExpectedSubcategoryOptionProposeds, proposedProject);
            HttpRequestStorage.DatabaseEntities.SaveChanges();

            SetMessageForDisplay($"{FieldDefinition.ProposedProject.GetFieldDefinitionLabel()} {MultiTenantHelpers.GetPerformanceMeasureNamePluralized()} succesfully saved.");
            return RedirectToAction(viewModel.AutoAdvance ? new SitkaRoute<ProposedProjectController>(x => x.EditClassifications(proposedProject.PrimaryKey)) : new SitkaRoute<ProposedProjectController>(x => x.EditExpectedPerformanceMeasureValues(proposedProject.PrimaryKey)));
        }

        [HttpGet]
        [ProposedProjectClassificationEditFeature]
        public ViewResult EditClassifications(ProposedProjectPrimaryKey proposedProjectPrimaryKey)
        {
            var proposedProject = proposedProjectPrimaryKey.EntityObject;
            var proposedProjectClassificationSimples = GetProposedProjectClassificationSimples(proposedProject);

            var viewModel = new EditProposedProjectClassificationsViewModel(proposedProjectClassificationSimples);
            return ViewEditClassifications(proposedProject, viewModel);
        }

        public static List<ProposedProjectClassificationSimple> GetProposedProjectClassificationSimples(ProposedProject proposedProject)
        {
            var selectedProposedProjectClassifications = proposedProject.ProposedProjectClassifications;

            //JHB 2/28/17: This is really brittle. The ViewModel relies on the ViewData also being ordered by DisplayName. 
            var proposedProjectClassificationSimples =
                HttpRequestStorage.DatabaseEntities.Classifications.OrderBy(x => x.DisplayName).Select(x => new ProposedProjectClassificationSimple { ClassificationID = x.ClassificationID }).ToList();
 
            foreach (var selectedClassification in selectedProposedProjectClassifications)
            {
                var selectedSimple = proposedProjectClassificationSimples.Single(x => x.ClassificationID == selectedClassification.ClassificationID);
                selectedSimple.Selected = true;
                selectedSimple.ProposedProjectClassificationNotes = selectedClassification.ProposedProjectClassificationNotes;
            }

            return proposedProjectClassificationSimples;
        }

        private ViewResult ViewEditClassifications(ProposedProject proposedProject, EditProposedProjectClassificationsViewModel viewModel)
        {
            var allClassifications = HttpRequestStorage.DatabaseEntities.Classifications.OrderBy(p => p.DisplayName).ToList();
            var proposalSectionsStatus = new ProposalSectionsStatus(proposedProject);
            proposalSectionsStatus.IsClassificationsComplete = ModelState.IsValid && proposalSectionsStatus.IsClassificationsComplete;

            var viewData = new EditProposedProjectClassificationsViewData(CurrentPerson, proposedProject, allClassifications, ProposedProjectSectionEnum.Classifications, proposalSectionsStatus);
            return RazorView<EditProposedProjectClassifications, EditProposedProjectClassificationsViewData, EditProposedProjectClassificationsViewModel>(viewData, viewModel);
        }

        [HttpPost]
        [ProposedProjectClassificationEditFeature]
        [AutomaticallyCallEntityFrameworkSaveChangesWhenModelValid]
        public ActionResult EditClassifications(ProposedProjectPrimaryKey proposedProjectPrimaryKey, EditProposedProjectClassificationsViewModel viewModel)
        {
            var proposedProject = proposedProjectPrimaryKey.EntityObject;
            if (!ModelState.IsValid)
            {
                ShowValidationErrors(viewModel.GetValidationResults().ToList());
                return ViewEditClassifications(proposedProject, viewModel);
            }
            var currentProposedProjectClassifications = viewModel.ProposedProjectClassificationSimples;
            HttpRequestStorage.DatabaseEntities.ProposedProjectClassifications.Load();
            viewModel.UpdateModel(proposedProject, currentProposedProjectClassifications);

            SetMessageForDisplay($"{FieldDefinition.ProposedProject.GetFieldDefinitionLabel()} {FieldDefinition.Classification.GetFieldDefinitionLabelPluralized()} succesfully saved.");
            return RedirectToAction(viewModel.AutoAdvance ? new SitkaRoute<ProposedProjectController>(x => x.Photos(proposedProject.PrimaryKey)) : new SitkaRoute<ProposedProjectController>(x => x.EditClassifications(proposedProject.PrimaryKey)));
        }

        [HttpGet]
        [ProposedProjectEditFeature]
        public ViewResult EditAssessment(ProposedProjectPrimaryKey proposedProjectPrimaryKey)
        {
            var proposedProject = proposedProjectPrimaryKey.EntityObject;
            var proposedProjectAssessmentQuestionSimples = GetProposedProjectAssessmentQuestionSimples(proposedProject);

            var viewModel = new EditAssessmentViewModel(proposedProjectAssessmentQuestionSimples);
            return ViewEditAssessment(proposedProject, viewModel);
        }

        public static List<ProposedProjectAssessmentQuestionSimple> GetProposedProjectAssessmentQuestionSimples(ProposedProject proposedProject)
        {           
            var allQuestionsAsSimples =
                HttpRequestStorage.DatabaseEntities.AssessmentQuestions.Where(x => !x.ArchiveDate.HasValue).ToList().Select(x => new ProposedProjectAssessmentQuestionSimple(x, proposedProject)).ToList();

            var answeredQuestions = proposedProject.ProposedProjectAssessmentQuestions.ToList();

            foreach (var question in allQuestionsAsSimples)
            {
                var matchedQuestionOrNull = answeredQuestions.SingleOrDefault(answeredQuestion => answeredQuestion.AssessmentQuestionID == question.AssessmentQuestionID);                
                question.Answer = matchedQuestionOrNull?.Answer;
            }
            return allQuestionsAsSimples;
        }


        [HttpPost]
        [AutomaticallyCallEntityFrameworkSaveChangesWhenModelValid]
        [ProposedProjectEditFeature]
        public ActionResult EditAssessment(ProposedProjectPrimaryKey proposedProjectPrimaryKey, EditAssessmentViewModel viewModel)
        {
            var proposedProject = proposedProjectPrimaryKey.EntityObject;
            if (!ModelState.IsValid)
            {
                ShowValidationErrors(viewModel.GetValidationResults().ToList());
                return ViewEditAssessment(proposedProject, viewModel);
            }

            viewModel.UpdateModel(proposedProject);
            SetMessageForDisplay("Assessment succesfully saved.");
            return RedirectToAction(viewModel.AutoAdvance ? new SitkaRoute<ProposedProjectController>(x => x.Photos(proposedProject.PrimaryKey)) : new SitkaRoute<ProposedProjectController>(x => x.EditAssessment(proposedProject.PrimaryKey)));
        }

        private ViewResult ViewEditAssessment(ProposedProject proposedProject, EditAssessmentViewModel viewModel)
        {
            var proposalSectionsStatus = new ProposalSectionsStatus(proposedProject);
            proposalSectionsStatus.IsAssessmentComplete = ModelState.IsValid && proposalSectionsStatus.IsAssessmentComplete;
            var assessmentGoals = HttpRequestStorage.DatabaseEntities.AssessmentGoals.ToList();
            var viewData = new EditAssessmentViewData(CurrentPerson, proposedProject, assessmentGoals, ProposedProjectSectionEnum.Assessment, proposalSectionsStatus);
            return RazorView<EditAssessment, EditAssessmentViewData, EditAssessmentViewModel>(viewData, viewModel);
        }

        [HttpGet]
        [ProposedProjectEditFeature]
        public ViewResult EditLocationSimple(ProposedProjectPrimaryKey proposedProjectPrimaryKey)
        {
            var proposedProject = proposedProjectPrimaryKey.EntityObject;
            var viewModel = new LocationSimpleViewModel(proposedProject);
            return ViewEditLocationSimple(proposedProject, viewModel);
        }

        private ViewResult ViewEditLocationSimple(ProposedProject proposedProject, LocationSimpleViewModel viewModel)
        {
            var layerGeoJsons = MapInitJson.GetAllWatershedMapLayers(LayerInitialVisibility.Hide);
            var mapInitJson = new MapInitJson($"proposedProject_{proposedProject.ProposedProjectID}_EditMap", 10, layerGeoJsons, BoundingBox.MakeNewDefaultBoundingBox(), false) {AllowFullScreen = false};
            
            var tenantAttribute = HttpRequestStorage.Tenant.GetTenantAttribute();
            var mapPostUrl = SitkaRoute<ProposedProjectController>.BuildUrlFromExpression(c => c.EditLocationSimple(proposedProject, null));
            var mapFormID = GenerateEditProjectLocationSimpleFormID(proposedProject);

            var editProjectLocationViewData = new ProjectLocationSimpleViewData(CurrentPerson, mapInitJson, tenantAttribute, null, mapPostUrl, mapFormID);

            var proposalSectionsStatus = new ProposalSectionsStatus(proposedProject);
            proposalSectionsStatus.IsProjectLocationSimpleSectionComplete = ModelState.IsValid && proposalSectionsStatus.IsProjectLocationSimpleSectionComplete;
            var viewData = new LocationSimpleViewData(CurrentPerson, proposedProject, proposalSectionsStatus, editProjectLocationViewData);

            return RazorView<LocationSimple, LocationSimpleViewData, LocationSimpleViewModel>(viewData, viewModel);
        }

        [HttpPost]
        [ProposedProjectEditFeature]
        [AutomaticallyCallEntityFrameworkSaveChangesWhenModelValid]
        public ActionResult EditLocationSimple(ProposedProjectPrimaryKey proposedProjectPrimaryKey, LocationSimpleViewModel viewModel)
        {
            var proposedProject = proposedProjectPrimaryKey.EntityObject;
            if (!ModelState.IsValid)
            {
                ShowValidationErrors(viewModel.GetValidationResults().ToList());
                return ViewEditLocationSimple(proposedProject, viewModel);
            }

            viewModel.UpdateModel(proposedProject);
            SetMessageForDisplay($"{FieldDefinition.ProposedProject.GetFieldDefinitionLabel()} Location succesfully saved.");
            return RedirectToAction(viewModel.AutoAdvance ? new SitkaRoute<ProposedProjectController>(x => x.EditLocationDetailed(proposedProject.PrimaryKey)) : new SitkaRoute<ProposedProjectController>(x => x.EditLocationSimple(proposedProject.PrimaryKey)));
        }


        private static string GenerateEditProjectLocationSimpleFormID(ProposedProject proposedProject)
        {
            return $"editMapForProposedProject{proposedProject.ProposedProjectID}";
        }

        [HttpGet]
        [ProposedProjectEditFeature]
        public ViewResult EditLocationDetailed(ProposedProjectPrimaryKey proposedProjectPrimaryKey)
        {
            var proposedProject = proposedProjectPrimaryKey.EntityObject;
            var viewModel = new LocationDetailedViewModel();
            return ViewEditLocationDetailed(proposedProject, viewModel);
        }

        private ViewResult ViewEditLocationDetailed(ProposedProject proposedProject, LocationDetailedViewModel viewModel)
        {
            var mapDivID = $"proposedProject_{proposedProject.EntityID}_EditDetailedMap";
            var detailedLocationGeoJsonFeatureCollection = proposedProject.DetailedLocationToGeoJsonFeatureCollection();
            var editableLayerGeoJson = new LayerGeoJson($"Proposed {FieldDefinition.ProjectLocation.GetFieldDefinitionLabel()}- Detail", detailedLocationGeoJsonFeatureCollection, "red", 1, LayerInitialVisibility.Show);

            var boundingBox = ProjectLocationSummaryMapInitJson.GetProjectBoundingBox(proposedProject);
            var layers = MapInitJson.GetAllWatershedMapLayers(LayerInitialVisibility.Show);
            layers.AddRange(MapInitJson.GetProjectLocationSimpleMapLayer(proposedProject));
            var mapInitJson = new MapInitJson(mapDivID, 10, layers, boundingBox) { AllowFullScreen = false };

            var mapFormID = GenerateEditProposedProjectLocationFormID(proposedProject.ProposedProjectID);
            var uploadGisFileUrl = SitkaRoute<ProposedProjectController>.BuildUrlFromExpression(c => c.ImportGdbFile(proposedProject.EntityID));
            var saveFeatureCollectionUrl = SitkaRoute<ProposedProjectController>.BuildUrlFromExpression(x => x.EditLocationDetailed(proposedProject, null));

            var hasSimpleLocationPoint = proposedProject.ProjectLocationPoint != null;

            var proposalSectionsStatus = new ProposalSectionsStatus(proposedProject);
            var projectLocationDetailViewData = new ProjectLocationDetailViewData(proposedProject.ProposedProjectID, mapInitJson, editableLayerGeoJson, uploadGisFileUrl, mapFormID, saveFeatureCollectionUrl, ProjectLocation.FieldLengths.Annotation, hasSimpleLocationPoint);
            var viewData = new LocationDetailedViewData(CurrentPerson, proposedProject, proposalSectionsStatus, projectLocationDetailViewData);
            return RazorView<LocationDetailed, LocationDetailedViewData, LocationDetailedViewModel>(viewData, viewModel);
        }

        [HttpPost]
        [ProposedProjectEditFeature]
        [AutomaticallyCallEntityFrameworkSaveChangesWhenModelValid]
        public ActionResult EditLocationDetailed(ProposedProjectPrimaryKey proposedProjectPrimaryKey, LocationDetailedViewModel viewModel)
        {
            var proposedProject = proposedProjectPrimaryKey.EntityObject;
            if (!ModelState.IsValid)
            {
                return ViewEditLocationDetailed(proposedProject, viewModel);
            }
            SaveDetailedLocations(viewModel, proposedProject);
            return RedirectToAction(viewModel.AutoAdvance ? new SitkaRoute<ProposedProjectController>(x => x.EditWatershed(proposedProject.PrimaryKey)) : new SitkaRoute<ProposedProjectController>(x => x.EditLocationDetailed(proposedProject.PrimaryKey)));
        }

        [HttpGet]
        [ProposedProjectEditFeature]
        public PartialViewResult ImportGdbFile(ProposedProjectPrimaryKey proposedProjectPrimaryKey)
        {
            var proposedProject = proposedProjectPrimaryKey.EntityObject;
            var viewModel = new ImportGdbFileViewModel();
            return ViewImportGdbFile(viewModel, proposedProject.ProposedProjectID);
        }

        private PartialViewResult ViewImportGdbFile(ImportGdbFileViewModel viewModel, int proposedProjectID)
        {
            var mapFormID = GenerateEditProposedProjectLocationFormID(proposedProjectID);
            var newGisUploadUrl = SitkaRoute<ProposedProjectController>.BuildUrlFromExpression(x => x.ImportGdbFile(proposedProjectID, null));
            var approveGisUploadUrl = SitkaRoute<ProposedProjectController>.BuildUrlFromExpression(x => x.ApproveGisUpload(proposedProjectID, null));
            var viewData = new ImportGdbFileViewData(mapFormID, newGisUploadUrl, approveGisUploadUrl);
            return RazorPartialView<ImportGdbFile, ImportGdbFileViewData, ImportGdbFileViewModel>(viewData, viewModel);
        }

        [HttpPost]
        [ProposedProjectEditFeature]
        [AutomaticallyCallEntityFrameworkSaveChangesWhenModelValid]
        public ActionResult ImportGdbFile(ProposedProjectPrimaryKey proposedProjectPrimaryKey, ImportGdbFileViewModel viewModel)
        {
            var proposedProject = proposedProjectPrimaryKey.EntityObject;
            if (!ModelState.IsValid)
            {
                return ViewImportGdbFile(viewModel, proposedProject.ProposedProjectID);
            }

            var httpPostedFileBase = viewModel.FileResourceData;
            var fileEnding = ".gdb.zip";
            using (var disposableTempFile = DisposableTempFile.MakeDisposableTempFileEndingIn(fileEnding))
            {
                var gdbFile = disposableTempFile.FileInfo;
                httpPostedFileBase.SaveAs(gdbFile.FullName);
                proposedProject.ProposedProjectLocationStagings.ToList().DeleteProposedProjectLocationStaging();
                proposedProject.ProposedProjectLocationStagings.Clear();
                ProposedProjectLocationStaging.CreateProposedProjectLocationStagingListFromGdb(gdbFile, proposedProject, CurrentPerson);
            }
            return ApproveGisUpload(proposedProject);
        }

        [HttpGet]
        [ProposedProjectEditFeature]
        public PartialViewResult ApproveGisUpload(ProposedProjectPrimaryKey proposedProjectPrimaryKey)
        {
            var proposedProject = proposedProjectPrimaryKey.EntityObject;
            var viewModel = new ProjectLocationDetailViewModel();
            return ViewApproveGisUpload(proposedProject, viewModel);
        }

        private PartialViewResult ViewApproveGisUpload(ProposedProject proposedProject, ProjectLocationDetailViewModel viewModel)
        {
            var projectLocationStagings = proposedProject.ProposedProjectLocationStagings.ToList();
            var layerGeoJsons =
                projectLocationStagings.Select(
                    (projectLocationStaging, i) =>
                        new LayerGeoJson(projectLocationStaging.FeatureClassName,
                            projectLocationStaging.ToGeoJsonFeatureCollection(),
                            FirmaHelpers.DefaultColorRange[i],
                            1,
                            LayerInitialVisibility.Show)).ToList();

            var boundingBox = BoundingBox.MakeBoundingBoxFromLayerGeoJsonList(layerGeoJsons);

            var mapInitJson = new MapInitJson($"proposedProject_{proposedProject.ProposedProjectID}_PreviewMap", 10, layerGeoJsons, boundingBox, false) {AllowFullScreen = false};
            var mapFormID = GenerateEditProposedProjectLocationFormID(proposedProject.ProposedProjectID);
            var approveGisUploadUrl = SitkaRoute<ProposedProjectController>.BuildUrlFromExpression(x => x.ApproveGisUpload(proposedProject, null));

            var viewData = new ApproveGisUploadViewData(new List<IProjectLocationStaging>(projectLocationStagings), mapInitJson, mapFormID, approveGisUploadUrl);
            return RazorPartialView<ApproveGisUpload, ApproveGisUploadViewData, ProjectLocationDetailViewModel>(viewData, viewModel);
        }

        [HttpPost]
        [ProposedProjectEditFeature]
        [AutomaticallyCallEntityFrameworkSaveChangesWhenModelValid]
        public ActionResult ApproveGisUpload(ProposedProjectPrimaryKey proposedProjectPrimaryKey, ProjectLocationDetailViewModel viewModel)
        {
            var proposedProject = proposedProjectPrimaryKey.EntityObject;
            if (!ModelState.IsValid)
            {
                return ViewApproveGisUpload(proposedProject, viewModel);
            }
            SaveDetailedLocations(viewModel, proposedProject);
            DbSpatialHelper.Reduce(new List<IHaveSqlGeometry>(proposedProject.ProposedProjectLocations.ToList()));
            return new ModalDialogFormJsonResult();
        }

        private static void SaveDetailedLocations(ProjectLocationDetailViewModel viewModel, ProposedProject proposedProject)
        {
            var proposedProjectLocations = proposedProject.ProposedProjectLocations.ToList();
            proposedProjectLocations.DeleteProposedProjectLocation();
            proposedProject.ProposedProjectLocations.Clear();
            if (viewModel.WktAndAnnotations != null)
            {
                foreach (var wktAndAnnotation in viewModel.WktAndAnnotations)
                {
                    proposedProject.ProposedProjectLocations.Add(new ProposedProjectLocation(proposedProject, DbGeometry.FromText(wktAndAnnotation.Wkt), wktAndAnnotation.Annotation));
                }
            }
        }

        public static string GenerateEditProposedProjectLocationFormID(int proposedProjectID)
        {
            return $"editMapForProposedProject{proposedProjectID}";
        }

        [HttpGet]
        [ProposedProjectEditFeature]
        public ViewResult EditWatershed(ProposedProjectPrimaryKey proposedProjectPrimaryKey)
        {
            var proposedProject = proposedProjectPrimaryKey.EntityObject;            
            var viewModel = new WatershedViewModel(proposedProject);
            return ViewEditWatershed(proposedProject, viewModel);
        }

        private ViewResult ViewEditWatershed(ProposedProject proposedProject, WatershedViewModel viewModel)
        {
            var boundingBox = ProjectLocationSummaryMapInitJson.GetProjectBoundingBox(proposedProject);
            var layers = MapInitJson.GetAllWatershedMapLayers(LayerInitialVisibility.Show);
            layers.AddRange(MapInitJson.GetProjectLocationSimpleAndDetailedMapLayers(proposedProject));
            var mapInitJson = new MapInitJson("projectWatershedMap", 0, layers, boundingBox) { AllowFullScreen = false };
            var watershedIDs = viewModel.WatershedIDs ?? new List<int>();
            var watershedsInViewModel = HttpRequestStorage.DatabaseEntities.Watersheds.Where(x => watershedIDs.Contains(x.WatershedID)).ToList();
            var tenantAttribute = HttpRequestStorage.Tenant.GetTenantAttribute();
            var editProjectWatershedsPostUrl = SitkaRoute<ProposedProjectController>.BuildUrlFromExpression(c => c.EditWatershed(proposedProject, null));
            var editProjectWatershedsFormId = GenerateEditProjectWatershedFormID(proposedProject);

            var editProjectLocationViewData = new EditProjectWatershedsViewData(CurrentPerson, mapInitJson, watershedsInViewModel, tenantAttribute, editProjectWatershedsPostUrl, editProjectWatershedsFormId, proposedProject.HasProjectLocationPoint, proposedProject.HasProjectLocationDetail);

            var proposalSectionsStatus = new ProposalSectionsStatus(proposedProject);
            proposalSectionsStatus.IsWatershedSectionComplete = ModelState.IsValid && proposalSectionsStatus.IsWatershedSectionComplete;
            var viewData = new WatershedViewData(CurrentPerson, proposedProject, proposalSectionsStatus, editProjectLocationViewData);

            return RazorView<Views.ProposedProject.Watershed, WatershedViewData, WatershedViewModel>(viewData, viewModel);
        }

        [HttpPost]
        [ProposedProjectEditFeature]
        [AutomaticallyCallEntityFrameworkSaveChangesWhenModelValid]
        public ActionResult EditWatershed(ProposedProjectPrimaryKey proposedProjectPrimaryKey, WatershedViewModel viewModel)
        {
            var proposedProject = proposedProjectPrimaryKey.EntityObject;
            if (!ModelState.IsValid)
            {
                ShowValidationErrors(viewModel.GetValidationResults().ToList());
                return ViewEditWatershed(proposedProject, viewModel);
            }
            var currentProposedProjectWatersheds = proposedProject.ProposedProjectWatersheds.ToList();
            var allProposedProjectWatersheds = HttpRequestStorage.DatabaseEntities.AllProposedProjectWatersheds.Local;
            viewModel.UpdateModel(proposedProject, currentProposedProjectWatersheds, allProposedProjectWatersheds);
            SetMessageForDisplay($"{FieldDefinition.ProposedProject.GetFieldDefinitionLabel()} Watersheds succesfully saved.");
            return RedirectToAction(viewModel.AutoAdvance ? new SitkaRoute<ProposedProjectController>(x => x.EditExpectedPerformanceMeasureValues(proposedProject.PrimaryKey)) : new SitkaRoute<ProposedProjectController>(x => x.EditWatershed(proposedProject.PrimaryKey)));
        }

        private static string GenerateEditProjectWatershedFormID(ProposedProject proposedProject)
        {
            return $"editMapForProposedProject{proposedProject.ProposedProjectID}";
        }

        [ProposedProjectEditFeature]
        public ViewResult Notes(ProposedProjectPrimaryKey proposedProjectPrimaryKey)
        {
            var proposedProject = proposedProjectPrimaryKey.EntityObject;
            var entityNotes = new List<IEntityNote>(proposedProject.ProposedProjectNotes);
            var addNoteUrl = SitkaRoute<ProposedProjectController>.BuildUrlFromExpression(x => x.NewNote(proposedProject));
            var canEditNotes = new ProposedProjectNoteManageFeature().HasPermissionByPerson(CurrentPerson);
            var entityNotesViewData = new EntityNotesViewData(EntityNote.CreateFromEntityNote(entityNotes), addNoteUrl, $"{FieldDefinition.ProposedProject.GetFieldDefinitionLabel()}", canEditNotes);

            var proposalSectionsStatus = new ProposalSectionsStatus(proposedProject);
            var viewData = new NotesViewData(CurrentPerson, proposedProject, proposalSectionsStatus, entityNotesViewData);
            return RazorView<Notes, NotesViewData>(viewData);
        }

        [HttpGet]
        [ProposedProjectNoteCreateFeature]
        public PartialViewResult NewNote(ProposedProjectPrimaryKey proposedProjectPrimaryKey)
        {
            var viewModel = new EditNoteViewModel();
            return ViewEditNote(viewModel);
        }

        [HttpPost]
        [ProposedProjectNoteCreateFeature]
        [AutomaticallyCallEntityFrameworkSaveChangesWhenModelValid]
        public ActionResult NewNote(ProposedProjectPrimaryKey proposedProjectPrimaryKey, EditNoteViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return ViewEditNote(viewModel);
            }
            var proposedProject = proposedProjectPrimaryKey.EntityObject;
            var proposedProjectNote = ProposedProjectNote.CreateNewBlank(proposedProject);
            viewModel.UpdateModel(proposedProjectNote, CurrentPerson);
            HttpRequestStorage.DatabaseEntities.AllProposedProjectNotes.Add(proposedProjectNote);
            return new ModalDialogFormJsonResult();
        }

        [HttpGet]
        [ProposedProjectNoteManageFeature]
        public PartialViewResult EditNote(ProposedProjectNotePrimaryKey proposedProjectNotePrimaryKey)
        {
            var proposedProjectNote = proposedProjectNotePrimaryKey.EntityObject;
            var viewModel = new EditNoteViewModel(proposedProjectNote.Note);
            return ViewEditNote(viewModel);
        }

        [HttpPost]
        [ProposedProjectNoteManageFeature]
        [AutomaticallyCallEntityFrameworkSaveChangesWhenModelValid]
        public ActionResult EditNote(ProposedProjectNotePrimaryKey proposedProjectNotePrimaryKey, EditNoteViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return ViewEditNote(viewModel);
            }
            var proposedProjectNote = proposedProjectNotePrimaryKey.EntityObject;
            viewModel.UpdateModel(proposedProjectNote, CurrentPerson);
            return new ModalDialogFormJsonResult();
        }

        private PartialViewResult ViewEditNote(EditNoteViewModel viewModel)
        {
            var viewData = new EditNoteViewData();
            return RazorPartialView<EditNote, EditNoteViewData, EditNoteViewModel>(viewData, viewModel);
        }

        [HttpGet]
        [ProposedProjectNoteManageFeature]
        public PartialViewResult DeleteNote(ProposedProjectNotePrimaryKey proposedProjectNotePrimaryKey)
        {
            var proposedProjectNote = proposedProjectNotePrimaryKey.EntityObject;
            var viewModel = new ConfirmDialogFormViewModel(proposedProjectNote.ProposedProjectNoteID);
            return ViewDeleteNote(proposedProjectNote, viewModel);
        }

        private PartialViewResult ViewDeleteNote(ProposedProjectNote proposedProjectNote, ConfirmDialogFormViewModel viewModel)
        {
            var canDelete = !proposedProjectNote.HasDependentObjects();
            var confirmMessage = canDelete
                ? $"Are you sure you want to delete this note for proposedProject '{proposedProjectNote.ProposedProject.DisplayName}'?"
                : ConfirmDialogFormViewData.GetStandardCannotDeleteMessage($"Proposed {FieldDefinition.ProjectNote.GetFieldDefinitionLabel()}");

            var viewData = new ConfirmDialogFormViewData(confirmMessage, canDelete);

            return RazorPartialView<ConfirmDialogForm, ConfirmDialogFormViewData, ConfirmDialogFormViewModel>(viewData, viewModel);
        }

        [HttpPost]
        [ProposedProjectNoteManageFeature]
        [AutomaticallyCallEntityFrameworkSaveChangesWhenModelValid]
        public ActionResult DeleteNote(ProposedProjectNotePrimaryKey proposedProjectNotePrimaryKey, ConfirmDialogFormViewModel viewModel)
        {
            var proposedProjectNote = proposedProjectNotePrimaryKey.EntityObject;
            if (!ModelState.IsValid)
            {
                return ViewDeleteNote(proposedProjectNote, viewModel);
            }
            proposedProjectNote.DeleteProposedProjectNote();
            return new ModalDialogFormJsonResult();
        }

        [ProposedProjectEditFeature]
        public ViewResult Photos(ProposedProjectPrimaryKey proposedProjectPrimaryKey)
        {
            var proposedProject = proposedProjectPrimaryKey.EntityObject;
            var viewData = BuildImageGalleryViewData(proposedProject, CurrentPerson);
            return RazorView<Photos, PhotoViewData>(viewData);
        }

        private static PhotoViewData BuildImageGalleryViewData(ProposedProject proposedProject, Person currentPerson)
        {
            var proposalSectionsStatus = new ProposalSectionsStatus(proposedProject);
            var newPhotoForProjectUrl = SitkaRoute<ProposedProjectImageController>.BuildUrlFromExpression(x => x.New(proposedProject));
            var galleryName = $"ProjectImage{proposedProject.ProposedProjectID}";
            var projectImages = proposedProject.ProposedProjectImages.ToList();
            var imageGalleryViewData = new PhotoViewData(currentPerson, galleryName, projectImages, newPhotoForProjectUrl, x => x.CaptionOnFullView, proposedProject, proposalSectionsStatus);
            return imageGalleryViewData;
        }

        [HttpGet]
        [ProposedProjectEditFeature]
        public PartialViewResult DeleteProposedProject(ProposedProjectPrimaryKey proposedProjectPrimaryKey)
        {
            var viewModel = new ConfirmDialogFormViewModel(proposedProjectPrimaryKey.PrimaryKeyValue);
            return ViewDeleteProposedProject(proposedProjectPrimaryKey.EntityObject, viewModel);
        }

        private PartialViewResult ViewDeleteProposedProject(ProposedProject proposedProject, ConfirmDialogFormViewModel viewModel)
        {
            var canDelete = proposedProject.CanDelete().HasPermission;
            var confirmMessage = canDelete
                ? $"Are you sure you want to delete {FieldDefinition.ProposedProject.GetFieldDefinitionLabel()} \"{proposedProject.DisplayName}\"?"
                : ConfirmDialogFormViewData.GetStandardCannotDeleteMessage("ProposedProject", SitkaRoute<ProposedProjectController>.BuildLinkFromExpression(x => x.Detail(proposedProject), "here"));

            var viewData = new ConfirmDialogFormViewData(confirmMessage, canDelete);
            return RazorPartialView<ConfirmDialogForm, ConfirmDialogFormViewData, ConfirmDialogFormViewModel>(viewData, viewModel);
        }

        [HttpPost]
        [ProposedProjectEditFeature]
        [AutomaticallyCallEntityFrameworkSaveChangesWhenModelValid]
        public ActionResult DeleteProposedProject(ProposedProjectPrimaryKey proposedProjectPrimaryKey, ConfirmDialogFormViewModel viewModel)
        {
            var proposedProject = proposedProjectPrimaryKey.EntityObject;
            var permissionCheckResult = proposedProject.CanDelete();
            if (!permissionCheckResult.HasPermission)
            {
                throw new SitkaRecordNotAuthorizedException(permissionCheckResult.PermissionDeniedMessage);
            }
            if (!ModelState.IsValid)
            {
                return ViewDeleteProposedProject(proposedProject, viewModel);
            }
            DeleteProposedProject(proposedProject);
            SetMessageForDisplay($"Proposed Project {proposedProject.DisplayName} successfully deleted.");
            return new ModalDialogFormJsonResult();
        }

        private static void DeleteProposedProject(ProposedProject proposedProject)
        {
            proposedProject.ProposedProjectOrganizations.DeleteProposedProjectOrganization();
            proposedProject.ProposedProjectImages.DeleteProposedProjectImage();
            proposedProject.ProposedProjectLocations.DeleteProposedProjectLocation();
            proposedProject.ProposedProjectLocationStagings.DeleteProposedProjectLocationStaging();

            proposedProject.ProposedProjectNotes.DeleteProposedProjectNote();
            proposedProject.ProposedProjectClassifications.DeleteProposedProjectClassification();

            var proposedProjectPerformanceMeasureExpecteds = proposedProject.PerformanceMeasureExpectedProposeds.ToList();
            proposedProjectPerformanceMeasureExpecteds.SelectMany(x => x.PerformanceMeasureExpectedSubcategoryOptionProposeds).ToList().DeletePerformanceMeasureExpectedSubcategoryOptionProposed();
            proposedProjectPerformanceMeasureExpecteds.DeletePerformanceMeasureExpectedProposed();

            var notifications = proposedProject.NotificationProposedProjects.Select(x => x.Notification).ToList();
            proposedProject.NotificationProposedProjects.DeleteNotificationProposedProject();
            notifications.DeleteNotification();
            proposedProject.DeleteProposedProject();
        }

        [HttpGet]
        [ProposedProjectEditFeature]
        public PartialViewResult Submit(ProposedProjectPrimaryKey proposedProjectPrimaryKey)
        {
            var proposedProject = proposedProjectPrimaryKey.EntityObject;
            var viewModel = new ConfirmDialogFormViewModel(proposedProject.ProposedProjectID);
            //TODO: Change "reviewer" to specific reviewer as determined by tentant review 
            var viewData = new ConfirmDialogFormViewData($"Are you sure you want to submit {FieldDefinition.ProposedProject.GetFieldDefinitionLabel()} \"{proposedProject.DisplayName}\" to the reviewer?");
            return RazorPartialView<ConfirmDialogForm, ConfirmDialogFormViewData, ConfirmDialogFormViewModel>(viewData, viewModel);
        }

        [HttpPost]
        [ProposedProjectEditFeature]
        [AutomaticallyCallEntityFrameworkSaveChangesWhenModelValid]
        public ActionResult Submit(ProposedProjectPrimaryKey proposedProjectPrimaryKey, ConfirmDialogFormViewModel viewModel)
        {
            var proposedProject = proposedProjectPrimaryKey.EntityObject;
            proposedProject.ProposedProjectStateID = (int)ProposedProjectStateEnum.Submitted;
            proposedProject.SubmissionDate = DateTime.Now;
            NotificationProposedProject.SendSubmittedMessage(proposedProject);
            SetMessageForDisplay($"{FieldDefinition.ProposedProject.GetFieldDefinitionLabel()} succesfully submitted for review.");
            return new ModalDialogFormJsonResult(proposedProject.GetDetailUrl());
        }

        [HttpGet]
        [ProposedProjectApproveFeature]
        public PartialViewResult Approve(ProposedProjectPrimaryKey proposedProjectPrimaryKey)
        {
            var proposedProject = proposedProjectPrimaryKey.EntityObject;
            var viewModel = new ConfirmDialogFormViewModel(proposedProject.ProposedProjectID);
            return ViewApprove(viewModel);
        }

        [HttpPost]
        [ProposedProjectApproveFeature]
        [AutomaticallyCallEntityFrameworkSaveChangesWhenModelValid]
        public ActionResult Approve(ProposedProjectPrimaryKey proposedProjectPrimaryKey, ConfirmDialogFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return ViewApprove(viewModel);
            }
            var proposedProject = proposedProjectPrimaryKey.EntityObject;
            Check.Assert(proposedProject.ProposedProjectState == ProposedProjectState.Submitted,
                $"{FieldDefinition.Project.GetFieldDefinitionLabel()} is not in Submitted state. Actual state is: " + proposedProject.ProposedProjectState.ProposedProjectStateDisplayName);

            Check.Assert(new ProposalSectionsStatus(proposedProject).AreAllSectionsValid, "Proposal is not ready for submittal.");

            var project = proposedProject.PromoteToProject(proposedProject, CurrentPerson);
            HttpRequestStorage.DatabaseEntities.AllProjects.Add(project);
            HttpRequestStorage.DatabaseEntities.SaveChanges();
            proposedProject.ProposedProjectStateID = (int)ProposedProjectStateEnum.Approved;
            proposedProject.ProjectID = project.ProjectID;
            proposedProject.ApprovalDate = DateTime.Now;
            proposedProject.ReviewedByPerson = CurrentPerson;
            HttpRequestStorage.DatabaseEntities.SaveChanges();
            GenerateApprovalAuditLogEntries(project, proposedProject);

            NotificationProposedProject.SendApprovalMessage(proposedProject);

            SetMessageForDisplay($"{FieldDefinition.ProposedProject.GetFieldDefinitionLabel()} \"{UrlTemplate.MakeHrefString(project.GetDetailUrl(), project.DisplayName)}\" succesfully approved as an actual {FieldDefinition.Project.GetFieldDefinitionLabel()} in the Planning/Design stage.");

            return new ModalDialogFormJsonResult(project.GetDetailUrl());
        }

        private void GenerateApprovalAuditLogEntries(Project project, ProposedProject proposedProject)
        {
            var auditLog = new AuditLog(CurrentPerson, DateTime.Now, AuditLogEventType.Added, "Project", project.ProjectID, "ProjectID", project.ProjectID.ToString())
            {
                ProposedProjectID = proposedProject.ProposedProjectID,
                ProjectID = project.ProjectID,
                AuditDescription = $"Project: created via approval of Proposed Project {proposedProject.DisplayName}"
            };
            HttpRequestStorage.DatabaseEntities.AllAuditLogs.Add(auditLog);
        }

        private PartialViewResult ViewApprove(ConfirmDialogFormViewModel viewModel)
        {
            var viewData = new ConfirmDialogFormViewData($"Are you sure you want to approve this {FieldDefinition.Project.GetFieldDefinitionLabel()}?");
            return RazorPartialView<ConfirmDialogForm, ConfirmDialogFormViewData, ConfirmDialogFormViewModel>(viewData, viewModel);
        }

        [HttpGet]
        [ProposedProjectEditFeature]
        public PartialViewResult Withdraw(ProposedProjectPrimaryKey proposedProjectPrimaryKey)
        {
            var proposedProject = proposedProjectPrimaryKey.EntityObject;
            var viewModel = new ConfirmDialogFormViewModel(proposedProject.ProposedProjectID);
            //TODO: Change "reviewer" to specific reviewer as determined by tentant review 
            var viewData = new ConfirmDialogFormViewData($"Are you sure you want to withdraw {FieldDefinition.ProposedProject.GetFieldDefinitionLabel()} \"{proposedProject.DisplayName}\" from review?");
            return RazorPartialView<ConfirmDialogForm, ConfirmDialogFormViewData, ConfirmDialogFormViewModel>(viewData, viewModel);
        }

        [HttpPost]
        [ProposedProjectEditFeature]
        [AutomaticallyCallEntityFrameworkSaveChangesWhenModelValid]
        public ActionResult Withdraw(ProposedProjectPrimaryKey proposedProjectPrimaryKey, ConfirmDialogFormViewModel viewModel)
        {
            var proposedProject = proposedProjectPrimaryKey.EntityObject;
            proposedProject.ProposedProjectStateID = (int)ProposedProjectStateEnum.Draft;
            //TODO: Change "reviewer" to specific reviewer as determined by tentant review 
            SetMessageForDisplay($"{FieldDefinition.ProposedProject.GetFieldDefinitionLabel()} withdrawn from review.");
            return new ModalDialogFormJsonResult(proposedProject.GetDetailUrl());
        }

        [HttpGet]
        [ProposedProjectApproveFeature]
        public PartialViewResult Return(ProposedProjectPrimaryKey proposedProjectPrimaryKey)
        {
            var proposedProject = proposedProjectPrimaryKey.EntityObject;
            var viewModel = new ConfirmDialogFormViewModel(proposedProject.ProposedProjectID);
            var viewData = new ConfirmDialogFormViewData($"Are you sure you want to return {FieldDefinition.ProposedProject.GetFieldDefinitionLabel()} \"{proposedProject.DisplayName}\" to Submitter?");
            return RazorPartialView<ConfirmDialogForm, ConfirmDialogFormViewData, ConfirmDialogFormViewModel>(viewData, viewModel);
        }

        [HttpPost]
        [ProposedProjectApproveFeature]
        [AutomaticallyCallEntityFrameworkSaveChangesWhenModelValid]
        public ActionResult Return(ProposedProjectPrimaryKey proposedProjectPrimaryKey, ConfirmDialogFormViewModel viewModel)
        {
            var proposedProject = proposedProjectPrimaryKey.EntityObject;
            proposedProject.ProposedProjectStateID = (int)ProposedProjectStateEnum.Draft;
            proposedProject.ReviewedByPerson = CurrentPerson;
            NotificationProposedProject.SendReturnedMessage(proposedProject);
            SetMessageForDisplay($"{FieldDefinition.ProposedProject.GetFieldDefinitionLabel()} returned to Submitter for additional clarifactions/corrections.");
            return new ModalDialogFormJsonResult(proposedProject.GetDetailUrl());
        }

        [HttpGet]
        [ProposedProjectApproveFeature]
        public PartialViewResult Reject(ProposedProjectPrimaryKey proposedProjectPrimaryKey)
        {
            var proposedProject = proposedProjectPrimaryKey.EntityObject;
            var viewModel = new ConfirmDialogFormViewModel(proposedProject.ProposedProjectID);
            var viewData = new ConfirmDialogFormViewData($"Are you sure you want to reject {FieldDefinition.ProposedProject.GetFieldDefinitionLabel()} \"{proposedProject.DisplayName}\"?");
            return RazorPartialView<ConfirmDialogForm, ConfirmDialogFormViewData, ConfirmDialogFormViewModel>(viewData, viewModel);
        }

        [HttpPost]
        [ProposedProjectApproveFeature]
        [AutomaticallyCallEntityFrameworkSaveChangesWhenModelValid]
        public ActionResult Reject(ProposedProjectPrimaryKey proposedProjectPrimaryKey, ConfirmDialogFormViewModel viewModel)
        {
            var proposedProject = proposedProjectPrimaryKey.EntityObject;
            proposedProject.ProposedProjectStateID = (int)ProposedProjectStateEnum.Rejected;
            SetMessageForDisplay($"{FieldDefinition.ProposedProject.GetFieldDefinitionLabel()} was rejected.");
            return new ModalDialogFormJsonResult(proposedProject.GetDetailUrl());
        }

        [ProposedProjectViewFeature]
        public GridJsonNetJObjectResult<AuditLog> AuditLogsGridJsonData(ProposedProjectPrimaryKey proposedProjectPrimaryKey)
        {
            var gridSpec = new AuditLogsGridSpec();
            var auditLogs1 = HttpRequestStorage.DatabaseEntities.AuditLogs.GetAuditLogEntriesForProposedProject(proposedProjectPrimaryKey.EntityObject);
            var auditLogs = auditLogs1;
            var gridJsonNetJObjectResult = new GridJsonNetJObjectResult<AuditLog>(auditLogs, gridSpec);
            return gridJsonNetJObjectResult;
        }

        private void ShowValidationErrors(List<ValidationResult> validationResults)
        {
            var validationErrorMessages = string.Empty;
            if (validationResults.Any())
            {
                validationErrorMessages =$" Please fix these errors: <ul>{string.Join(Environment.NewLine, validationResults.Select(x => $"<li>{x.ErrorMessage}</li>"))}</ul>";
            }
            SetErrorForDisplay($"Could not save {FieldDefinition.ProposedProject.GetFieldDefinitionLabel()}.{validationErrorMessages}");
        }

        [CrossAreaRoute]
        [ProposedProjectViewFeature]
        public PartialViewResult ProposedProjectMapPopup(ProposedProjectPrimaryKey primaryKeyProposedProject)
        {
            var proposedProject = primaryKeyProposedProject.EntityObject;
            return RazorPartialView<ProjectMapPopup, ProjectMapPopupViewData>(new ProjectMapPopupViewData(proposedProject));
        }
    }
}
