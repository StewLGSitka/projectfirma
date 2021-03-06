﻿using System.Collections.Generic;
using System.Linq;
using ProjectFirma.Web.Common;

namespace ProjectFirmaModels.Models
{
    public static class ProjectFundingSourceExpenditureUpdateModelExtensions
    {
        public static void CreateFromProject(ProjectUpdateBatch projectUpdateBatch)
        {
            var project = projectUpdateBatch.Project;
            projectUpdateBatch.ProjectFundingSourceExpenditureUpdates =
                project.ProjectFundingSourceExpenditures.Select(
                    projectFundingSourceExpenditure =>
                        new ProjectFundingSourceExpenditureUpdate(projectUpdateBatch,
                            projectFundingSourceExpenditure.FundingSource,
                            projectFundingSourceExpenditure.CalendarYear,
                            projectFundingSourceExpenditure.ExpenditureAmount)).ToList();
        }

        public static void CommitChangesToProject(ProjectUpdateBatch projectUpdateBatch, IList<ProjectFundingSourceExpenditure> allProjectFundingSourceExpenditures)
        {
            var project = projectUpdateBatch.Project;
            var projectFundingSourceExpendituresFromProjectUpdate =
                projectUpdateBatch.ProjectFundingSourceExpenditureUpdates.Select(
                    x => new ProjectFundingSourceExpenditure(project.ProjectID, x.FundingSource.FundingSourceID, x.CalendarYear, x.ExpenditureAmount)).ToList();
            project.ProjectFundingSourceExpenditures.Merge(projectFundingSourceExpendituresFromProjectUpdate,
                allProjectFundingSourceExpenditures,
                (x, y) => x.ProjectID == y.ProjectID && x.CalendarYear == y.CalendarYear && x.FundingSourceID == y.FundingSourceID,
                (x, y) => x.ExpenditureAmount = y.ExpenditureAmount, HttpRequestStorage.DatabaseEntities);
        }

        public static List<int> CalculateCalendarYearRangeForExpenditures(this IList<ProjectFundingSourceExpenditureUpdate> projectFundingSourceExpenditureUpdates, ProjectUpdate projectUpdate)
        {
            if (projectUpdate.CompletionYear < projectUpdate.ImplementationStartYear) return new List<int>();
            if (projectUpdate.CompletionYear < projectUpdate.PlanningDesignStartYear) return new List<int>();

            var existingYears = projectFundingSourceExpenditureUpdates.Select(x => x.CalendarYear).ToList();
            return FirmaDateUtilities.CalculateCalendarYearRangeForExpendituresAccountingForExistingYears(existingYears, projectUpdate, FirmaDateUtilities.CalculateCurrentYearToUseForUpToAllowableInputInReporting());
        }
    }
}