﻿/*-----------------------------------------------------------------------
<copyright file="ManageViewData.cs" company="Tahoe Regional Planning Agency and Sitka Technology Group">
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
using ProjectFirma.Web.Controllers;
using ProjectFirma.Web.Security;
using ProjectFirma.Web.Models;
using LtInfo.Common;

namespace ProjectFirma.Web.Views.Assessment
{
    public class ManageViewData : FirmaViewData
    {
        public readonly bool HasEditPermissions;
        public readonly List<AssessmentGoal> AssessmentGoals;
        public readonly string AddQuestionUrl;
        public readonly string DownloadAllAssessmentsUrl;

        public ManageViewData(Person currentPerson, List<AssessmentGoal> assessmentGoals)
            : base(currentPerson)
        {
            PageTitle = "Manage  Assessment";
            HasEditPermissions = new AssessmentManageFeature().HasPermissionByPerson(CurrentPerson);
            AssessmentGoals = assessmentGoals;
            AddQuestionUrl = SitkaRoute<AssessmentController>.BuildUrlFromExpression(c => c.NewQuestion());
            DownloadAllAssessmentsUrl = SitkaRoute<AssessmentController>.BuildUrlFromExpression(c => c.IndexExcelDownload());
        }
    }
}
