﻿/*-----------------------------------------------------------------------
<copyright file="TestProject.cs" company="Tahoe Regional Planning Agency and Sitka Technology Group">
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
using ProjectFirmaModels.Models;

namespace ProjectFirmaModels.UnitTestCommon
{
    public static partial class TestFramework
    {
        public static class TestProject
        {
            public static Project Create()
            {
                var taxonomyLeaf = TestFramework.TestTaxonomyLeaf.Create();
                var projectStage = ProjectStage.PlanningDesign;
                // TODO: Verify that "Approved" is the correct project state or use the correct value
                var project = Project.CreateNewBlank(taxonomyLeaf, projectStage, ProjectLocationSimpleType.None,
                    FundingType.Capital, ProjectApprovalStatus.Approved);
                return project;
            }

            public static Project Create(DatabaseEntities dbContext)
            {
                var taxonomyLeaf = TestFramework.TestTaxonomyLeaf.Create(dbContext);

                var projectStage = ProjectStage.PlanningDesign;
                var project = new Project(taxonomyLeaf,
                    projectStage,
                    string.Format("Test Project Name {0}", Guid.NewGuid()),
                    MakeTestName("Test Project Description"),
                    false,
                    ProjectLocationSimpleType.None,
                    FundingType.Capital,
                    // TODO: Verify that this is correct or use the correct value
                    ProjectApprovalStatus.Approved);

                dbContext.AllProjects.Add(project);
                return project;
            }

            public static Project Create(short projectID, string projectName)
            {
                var taxonomyLeaf = TestFramework.TestTaxonomyLeaf.Create();
                var projectStage = ProjectStage.Implementation;
                // TODO: Verify that "Approved" is the correct project state or use the correct value
                var project = new Project(taxonomyLeaf, projectStage, projectName, "Some description",  false, ProjectLocationSimpleType.None, FundingType.Capital, ProjectApprovalStatus.Approved)
                {
                    ProjectID = projectID
                };
                return project;
            }
        }
    }
}
