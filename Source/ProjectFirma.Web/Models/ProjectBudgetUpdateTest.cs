﻿/*-----------------------------------------------------------------------
<copyright file="ProjectBudgetUpdateTest.cs" company="Tahoe Regional Planning Agency and Sitka Technology Group">
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
using System.Linq;
using ProjectFirma.Web.UnitTestCommon;
using NUnit.Framework;

namespace ProjectFirma.Web.Models
{
    [TestFixture]
    public class ProjectBudgetUpdateTest
    {
        [Test]
        public void CloneProjectBudgetTest()
        {
            // Arrange
            var project = TestFramework.TestProject.Create(-777, "Project-777");
            var projectUpdateBatch = TestFramework.TestProjectUpdateBatch.Create(project);
            var fundingSource1 = TestFramework.TestFundingSource.Create();

            // Arrange: test cloning a PerformanceMeasureActual
            var projectBudget1 = TestFramework.TestProjectBudget.Create(project, fundingSource1, 2011, 555);

            const int newCalendarYear = 2017;

            // Act && Assert
            var newProjectBudgetUpdate = ProjectBudgetUpdate.CloneProjectBudget(projectUpdateBatch, projectBudget1, newCalendarYear, projectBudget1.GetMonetaryAmount());
            AssertCloneProjectBudgetSuccessful(newProjectBudgetUpdate, projectUpdateBatch, projectBudget1, newCalendarYear, projectBudget1.GetMonetaryAmount());
        }

        [Test]
        public void CreateFromProjectTestStartYearAndCompletionYearTheSameAsExistingBudgetRecords()
        {
            // Arrange
            var project = TestFramework.TestProject.Create(-777, "Project-777");
            var projectUpdateBatch = TestFramework.TestProjectUpdateBatch.Create(project);
            var projectUpdate = TestFramework.TestProjectUpdate.Create(projectUpdateBatch);

            var fundingSource1 = TestFramework.TestFundingSource.Create();
            var fundingSource2 = TestFramework.TestFundingSource.Create();

            // Arrange: test cloning a PerformanceMeasureActual
            TestFramework.TestProjectBudget.Create(project, fundingSource1, 2011, 555);
            TestFramework.TestProjectBudget.Create(project, fundingSource1, 2012, 666);
            TestFramework.TestProjectBudget.Create(project, fundingSource2, 2013, 777);
            TestFramework.TestProjectBudget.Create(project, fundingSource2, 2014, 888);

            projectUpdate.PlanningDesignStartYear = project.ProjectBudgets.Min(x => x.CalendarYear);
            projectUpdate.CompletionYear = project.ProjectBudgets.Max(x => x.CalendarYear);

            // Act
            ProjectBudgetUpdate.CreateFromProject(projectUpdateBatch);

            // Assert
            AssertThatNoProjectBudgetRecordsLeftBehind(project, projectUpdateBatch);

            Assert.That(project.ProjectBudgets.Count, Is.EqualTo(projectUpdateBatch.ProjectBudgetUpdates.Count), $"Should not have any extra records, should be a match with existing {Models.FieldDefinition.Project.GetFieldDefinitionLabel()} record");
        }

        private static void AssertThatNoProjectBudgetRecordsLeftBehind(Project project, ProjectUpdateBatch projectUpdateBatch)
        {
            // Assert - all existing rows from the project record should be there
            foreach (var ProjectBudget in project.ProjectBudgets)
            {
                var projectBudgetUpdate =
                    projectUpdateBatch.ProjectBudgetUpdates.SingleOrDefault(
                        x =>
                            x.ProjectCostTypeID == ProjectBudget.ProjectCostTypeID && x.CalendarYear == ProjectBudget.CalendarYear &&
                            x.FundingSourceID == ProjectBudget.FundingSourceID && x.GetMonetaryAmount() == ProjectBudget.GetMonetaryAmount());
                AssertCloneProjectBudgetSuccessful(projectBudgetUpdate, projectUpdateBatch, ProjectBudget);
            }
        }

        private static void AssertCloneProjectBudgetSuccessful(ProjectBudgetUpdate newProjectBudgetUpdate, ProjectUpdateBatch projectUpdateBatch, IProjectBudgetAmount ProjectBudgetAmount)
        {
            Assert.That(newProjectBudgetUpdate.ProjectUpdateBatchID, Is.EqualTo(projectUpdateBatch.ProjectUpdateBatchID), $"Should have assigned it to this {Models.FieldDefinition.Project.GetFieldDefinitionLabel()} update batch");
            Assert.That(newProjectBudgetUpdate.ProjectCostTypeID, Is.EqualTo(ProjectBudgetAmount.ProjectCostTypeID), $"Should have cloned transporation {Models.FieldDefinition.Project.GetFieldDefinitionLabel()} cost type correctly");
            Assert.That(newProjectBudgetUpdate.FundingSourceID, Is.EqualTo(ProjectBudgetAmount.FundingSourceID), "Should have cloned funding source correctly");
            Assert.That(newProjectBudgetUpdate.CalendarYear, Is.EqualTo(ProjectBudgetAmount.CalendarYear), "Should be set to the new calendar year");
            Assert.That(newProjectBudgetUpdate.GetMonetaryAmount(), Is.EqualTo(ProjectBudgetAmount.GetMonetaryAmount()), "Should be set to the new budget amount");
        }

        private static void AssertCloneProjectBudgetSuccessful(ProjectBudgetUpdate newProjectBudgetUpdate, ProjectUpdateBatch projectUpdateBatch, IProjectBudgetAmount ProjectBudgetAmount, int newCalendarYear, decimal? budgetedAmount)
        {
            Assert.That(newProjectBudgetUpdate.ProjectUpdateBatchID, Is.EqualTo(projectUpdateBatch.ProjectUpdateBatchID), $"Should have assigned it to this {Models.FieldDefinition.Project.GetFieldDefinitionLabel()} update batch");
            Assert.That(newProjectBudgetUpdate.ProjectCostTypeID, Is.EqualTo(ProjectBudgetAmount.ProjectCostTypeID), $"Should have cloned transporation {Models.FieldDefinition.Project.GetFieldDefinitionLabel()} cost type correctly");
            Assert.That(newProjectBudgetUpdate.FundingSourceID, Is.EqualTo(ProjectBudgetAmount.FundingSourceID), "Should have cloned funding source correctly");
            Assert.That(newProjectBudgetUpdate.CalendarYear, Is.EqualTo(newCalendarYear), "Should be set to the new calendar year");
            Assert.That(newProjectBudgetUpdate.GetMonetaryAmount(), Is.EqualTo(budgetedAmount), "Should be set to the new budget amount");
        }
    }
}
