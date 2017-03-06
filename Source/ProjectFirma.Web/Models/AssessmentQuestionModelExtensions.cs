﻿/*-----------------------------------------------------------------------
<copyright file="AssessmentQuestionModelExtensions.cs" company="Tahoe Regional Planning Agency">
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
using System.Collections.Generic;
using System.Linq;
using LtInfo.Common;
using LtInfo.Common.Views;

namespace ProjectFirma.Web.Models
{
    public static class AssessmentQuestionModelExtensions
    {
        public static FancyTreeNode ToFancyTreeNode(this AssessmentQuestion assessmentQuestion, List<IQuestionAnswer> projectAssessmentQuestions)
        {
            var projectAssessmentQuestion = projectAssessmentQuestions != null && projectAssessmentQuestions.Any()
                ? projectAssessmentQuestions.SingleOrDefault(x => x.AssessmentQuestionID == assessmentQuestion.AssessmentQuestionID)
                : null;
            var answer = projectAssessmentQuestion != null ? projectAssessmentQuestion.Answer : null;
            var fancyTreeNode = new FancyTreeNode(assessmentQuestion.AssessmentQuestionText, assessmentQuestion.AssessmentQuestionID.ToString(), false)
            {
                Answer = answer.HasValue ? answer.ToYesNo() : ViewUtilities.NoAnswerProvided
            };
            return fancyTreeNode;
        }
    }
}
