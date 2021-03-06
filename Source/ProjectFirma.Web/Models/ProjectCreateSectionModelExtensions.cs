﻿using System;
using System.Linq;
using ProjectFirma.Web.Common;
using ProjectFirma.Web.Controllers;
using ProjectFirma.Web.Views.ProjectCreate;
using ProjectFirmaModels.Models;

namespace ProjectFirma.Web.Models
{
    public static class ProjectCreateSectionModelExtensions
    {
        public static bool IsComplete(this ProjectCreateSection projectCreateSection, Project project)
        {
            if (project == null)
            {
                return false;
            }
            switch (projectCreateSection.ToEnum)
            {
                case ProjectCreateSectionEnum.Basics:
                    return !new BasicsViewModel(project).GetValidationResults().Any();
                case ProjectCreateSectionEnum.LocationSimple:
                    var locationSimpleValidationResults = new LocationSimpleViewModel(project).GetValidationResults();
                    return !locationSimpleValidationResults.Any();
                case ProjectCreateSectionEnum.Organizations:
                    return !new OrganizationsViewModel(project, null).GetValidationResults().ToList().Any();
                case ProjectCreateSectionEnum.LocationDetailed:
                    return true;
                case ProjectCreateSectionEnum.ExpectedPerformanceMeasures:
                    return !new ExpectedPerformanceMeasureValuesViewModel(project).GetValidationResults().Any();
                case ProjectCreateSectionEnum.ReportedPerformanceMeasures:
                    var pmValidationResults = new PerformanceMeasuresViewModel(
                        project.PerformanceMeasureActuals.Select(x => new PerformanceMeasureActualSimple(x)).ToList(),
                        project.PerformanceMeasureActualYearsExemptionExplanation,
                        project.GetPerformanceMeasuresExemptReportingYears().Select(x => new ProjectExemptReportingYearSimple(x)).ToList())
                    {
                        ProjectID = project.ProjectID
                    }.GetValidationResults();
                    return !pmValidationResults.Any();
                case ProjectCreateSectionEnum.ExpectedFunding:
                    // todo: more complicated than that.
                    return ProjectCreateSection.Basics.IsComplete(project);
                case ProjectCreateSectionEnum.ReportedExpenditures:
                    var projectFundingSourceExpenditures = project.ProjectFundingSourceExpenditures.ToList();
                    var validationResults = new ExpendituresViewModel(projectFundingSourceExpenditures,
                                projectFundingSourceExpenditures.CalculateCalendarYearRangeForExpenditures(project), project,
                                project.GetExpendituresExemptReportingYears().Select(x => new ProjectExemptReportingYearSimple(x)).ToList())
                            { ProjectID = project.ProjectID }
                        .GetValidationResults();
                    return !validationResults.Any();
                case ProjectCreateSectionEnum.Classifications:
                    var projectClassificationSimples = ProjectCreateController.GetProjectClassificationSimples(project);
                    var classificationValidationResults = new EditProposalClassificationsViewModel(projectClassificationSimples).GetValidationResults();
                    return !classificationValidationResults.Any();
                case ProjectCreateSectionEnum.Assessment:
                    return !new EditAssessmentViewModel(project.ProjectAssessmentQuestions.Select(x => new ProjectAssessmentQuestionSimple(x)).ToList()).GetValidationResults().Any();
                case ProjectCreateSectionEnum.Photos:
                    return ProjectCreateSection.Basics.IsComplete(project);
                case ProjectCreateSectionEnum.NotesAndDocuments:
                    return ProjectCreateSection.Basics.IsComplete(project);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public static string GetSectionUrl(this ProjectCreateSection projectCreateSection, Project project)
        {
            if (project == null)
            {
                return null;
            }
            switch (projectCreateSection.ToEnum)
            {
                case ProjectCreateSectionEnum.Basics:
                    return SitkaRoute<ProjectCreateController>.BuildUrlFromExpression(x => x.EditBasics(project.ProjectID));
                case ProjectCreateSectionEnum.LocationSimple:
                    return ProjectCreateSection.Basics.IsComplete(project) ? SitkaRoute<ProjectCreateController>.BuildUrlFromExpression(x => x.EditLocationSimple(project.ProjectID)) : null;
                case ProjectCreateSectionEnum.Organizations:
                    return ProjectCreateSection.Basics.IsComplete(project) ? SitkaRoute<ProjectCreateController>.BuildUrlFromExpression(x => x.Organizations(project.ProjectID)) : null;
                case ProjectCreateSectionEnum.LocationDetailed:
                    return ProjectCreateSection.Basics.IsComplete(project) ? SitkaRoute<ProjectCreateController>.BuildUrlFromExpression(x => x.EditLocationDetailed(project.ProjectID)) : null;
                case ProjectCreateSectionEnum.ExpectedPerformanceMeasures:
                    return ProjectCreateSection.Basics.IsComplete(project) ? SitkaRoute<ProjectCreateController>.BuildUrlFromExpression(x => x.EditExpectedPerformanceMeasureValues(project.ProjectID)) : null;
                case ProjectCreateSectionEnum.ReportedPerformanceMeasures:
                    return ProjectCreateSection.Basics.IsComplete(project) ? SitkaRoute<ProjectCreateController>.BuildUrlFromExpression(x => x.PerformanceMeasures(project.ProjectID)) : null;
                case ProjectCreateSectionEnum.ExpectedFunding:
                    return ProjectCreateSection.Basics.IsComplete(project) ? SitkaRoute<ProjectCreateController>.BuildUrlFromExpression(x => x.ExpectedFunding(project.ProjectID)) : null;
                case ProjectCreateSectionEnum.ReportedExpenditures:
                    return ProjectCreateSection.Basics.IsComplete(project) ? SitkaRoute<ProjectCreateController>.BuildUrlFromExpression(x => x.Expenditures(project.ProjectID)) : null;
                case ProjectCreateSectionEnum.Classifications:
                    return ProjectCreateSection.Basics.IsComplete(project) ? SitkaRoute<ProjectCreateController>.BuildUrlFromExpression(x => x.EditClassifications(project.ProjectID)) : null;
                case ProjectCreateSectionEnum.Assessment:
                    return ProjectCreateSection.Basics.IsComplete(project) ? SitkaRoute<ProjectCreateController>.BuildUrlFromExpression(x => x.EditAssessment(project.ProjectID)) : null;
                case ProjectCreateSectionEnum.Photos:
                    return ProjectCreateSection.Basics.IsComplete(project) ? SitkaRoute<ProjectCreateController>.BuildUrlFromExpression(x => x.Photos(project.ProjectID)) : null;
                case ProjectCreateSectionEnum.NotesAndDocuments:
                    return ProjectCreateSection.Basics.IsComplete(project) ? SitkaRoute<ProjectCreateController>.BuildUrlFromExpression(x => x.DocumentsAndNotes(project.ProjectID)) : null;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}