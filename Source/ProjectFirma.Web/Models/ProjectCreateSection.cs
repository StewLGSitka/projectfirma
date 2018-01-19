﻿using System.Collections.Generic;
using System.Linq;
using ProjectFirma.Web.Controllers;
using ProjectFirma.Web.Views.ProjectCreate;
using ProjectFirma.Web.Common;

namespace ProjectFirma.Web.Models
{
    public partial class ProjectCreateSection
    {
        public abstract bool IsComplete(Project project);
        public abstract string GetSectionUrl(Project project);

        public static List<ProjectCreateSection> ConditionalSections =>
            new List<ProjectCreateSection> {ExpectedFunding, Assessment, ReportedPerformanceMeasures, ReportedExpenditures};
    }

    public partial class ProjectCreateSectionInstructions
    {
        public override bool IsComplete(Project project)
        {
            return true;
        }

        public override string GetSectionUrl(Project project)
        {
            return SitkaRoute<ProjectCreateController>.BuildUrlFromExpression(x => x.InstructionsProposal(project != null ? project.ProjectID : (int?) null));
        }
    }

    public partial class ProjectCreateSectionBasics
    {
        public override bool IsComplete(Project project)
        {
            if (project == null)
            {
                return false;
            }
            var basicsResults = new BasicsViewModel(project).GetValidationResults();
            return !basicsResults.Any();
        }

        public override string GetSectionUrl(Project project)
        {
            return project != null ? SitkaRoute<ProjectCreateController>.BuildUrlFromExpression(x => x.EditBasics(project.ProjectID)) : null;
        }
    }

    public partial class ProjectCreateSectionLocationSimple
    {
        public override bool IsComplete(Project project)
        {
            var locationSimpleValidationResults = new LocationSimpleViewModel(project).GetValidationResults();
            return !locationSimpleValidationResults.Any();            
        }

        public override string GetSectionUrl(Project project)
        {
            return Basics.IsComplete(project) ? SitkaRoute<ProjectCreateController>.BuildUrlFromExpression(x => x.EditLocationSimple(project.ProjectID)) : null;
        }
    }

    public partial class ProjectCreateSectionLocationDetailed
    {
        public override bool IsComplete(Project project)
        {
            return true;
        }

        public override string GetSectionUrl(Project project)
        {
            return Basics.IsComplete(project) ? SitkaRoute<ProjectCreateController>.BuildUrlFromExpression(x => x.EditLocationDetailed(project.ProjectID)) : null;
        }
    }

    public partial class ProjectCreateSectionWatershed
    {
        public override bool IsComplete(Project project)
        {
            var watershedValidationResults = new WatershedViewModel(project).GetValidationResults();
            return !watershedValidationResults.Any();
        }

        public override string GetSectionUrl(Project project)
        {
            return Basics.IsComplete(project) ? SitkaRoute<ProjectCreateController>.BuildUrlFromExpression(x => x.EditWatershed(project.ProjectID)) : null;
        }
    }

    public partial class ProjectCreateSectionExpectedPerformanceMeasures
    {
        public override bool IsComplete(Project project)
        {
            var pmValidationResults = new ExpectedPerformanceMeasureValuesViewModel(project).GetValidationResults();
            return !pmValidationResults.Any();
        }

        public override string GetSectionUrl(Project project)
        {
            return Basics.IsComplete(project) ? SitkaRoute<ProjectCreateController>.BuildUrlFromExpression(x => x.EditExpectedPerformanceMeasureValues(project.ProjectID)) : null;
        }
    }

    public partial class ProjectCreateSectionReportedPerformanceMeasures
    {
        public override bool IsComplete(Project project)
        {
            var pmValidationResults = new PerformanceMeasuresViewModel(project.PerformanceMeasureActuals.Select(x=>new PerformanceMeasureActualSimple(x)).ToList(), project.PerformanceMeasureActualYearsExemptionExplanation,project.ProjectExemptReportingYears.Select(x=>new ProjectExemptReportingYearSimple(x)).ToList()){ProjectID=project.ProjectID}.GetValidationResults();
            return !pmValidationResults.Any();
        }

        public override string GetSectionUrl(Project project)
        {
            //todo
            return Basics.IsComplete(project) ? SitkaRoute<ProjectCreateController>.BuildUrlFromExpression(x => x.PerformanceMeasures(project.ProjectID)) : null;
        }
    }

    public partial class ProjectCreateSectionAssessment
    {
        public override bool IsComplete(Project project)
        {
            var assessmentValidationResults = new EditAssessmentViewModel(project.ProjectAssessmentQuestions.Select(x => new ProjectAssessmentQuestionSimple(x)).ToList()).GetValidationResults();
            return !assessmentValidationResults.Any();
        }

        public override string GetSectionUrl(Project project)
        {
            return Basics.IsComplete(project) ? SitkaRoute<ProjectCreateController>.BuildUrlFromExpression(x => x.EditAssessment(project.ProjectID)) : null;
        }
    }

    public partial class ProjectCreateSectionClassifications
    {
        public override bool IsComplete(Project project)
        {
            var projectClassificationSimples = project.ProjectClassifications
                .Select(x => new ProjectClassificationSimple(x.ProjectClassificationID,project.ProjectID, x.ClassificationID, x.ProjectClassificationNotes, true)).ToList();

            var classificationValidationResults = new EditProposalClassificationsViewModel(projectClassificationSimples).GetValidationResults();
            return !classificationValidationResults.Any();
        }

        public override string GetSectionUrl(Project project)
        {
            return Basics.IsComplete(project) ? SitkaRoute<ProjectCreateController>.BuildUrlFromExpression(x => x.EditClassifications(project.ProjectID)) : null;
        }
    }

    public partial class ProjectCreateSectionPhotos
    {
        public override bool IsComplete(Project project)
        {
            return Basics.IsComplete(project);
        }

        public override string GetSectionUrl(Project project)
        {
            return Basics.IsComplete(project) ? SitkaRoute<ProjectCreateController>.BuildUrlFromExpression(x => x.Photos(project.ProjectID)) : null;
        }
    }

    public partial class ProjectCreateSectionNotes
    {
        public override bool IsComplete(Project project)
        {
            return Basics.IsComplete(project);
        }

        public override string GetSectionUrl(Project project)
        {
            return Basics.IsComplete(project) ? SitkaRoute<ProjectCreateController>.BuildUrlFromExpression(x => x.Notes(project.ProjectID)) : null;
        }
    }

    public partial class ProjectCreateSectionExpectedFunding
    {
        public override bool IsComplete(Project project)
        {
            // todo: more complicated than that.
            return Basics.IsComplete(project);
        }

        public override string GetSectionUrl(Project project)
        {
            return Basics.IsComplete(project) ? SitkaRoute<ProjectCreateController>.BuildUrlFromExpression(x => x.ExpectedFunding(project.ProjectID)) : null;
        }
    }

    public partial class ProjectCreateSectionReportedExpenditures
    {
        public override bool IsComplete(Project project)
        {
            var projectFundingSourceExpenditures = project.ProjectFundingSourceExpenditures.ToList();
            var validationResults = new ExpendituresViewModel(projectFundingSourceExpenditures, projectFundingSourceExpenditures.CalculateCalendarYearRangeForExpenditures(project)){ProjectID=project.ProjectID}.GetValidationResults();
            return !validationResults.Any();
        }

        public override string GetSectionUrl(Project project)
        {
            return Basics.IsComplete(project) ? SitkaRoute<ProjectCreateController>.BuildUrlFromExpression(x => x.Expenditures(project.ProjectID)) : null;
        }
    }
}
