﻿/*-----------------------------------------------------------------------
<copyright file="FeaturedListViewData.cs" company="Tahoe Regional Planning Agency">
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
using System.Linq;
using ProjectFirma.Web.Controllers;
using ProjectFirma.Web.Models;
using LtInfo.Common;
using LtInfo.Common.ModalDialog;
using ProjectFirma.Web.Common;
using ProjectFirma.Web.Views.Shared;

namespace ProjectFirma.Web.Views.Project
{
    public class FeaturedListViewData : FirmaViewData
    {
        public readonly FeaturesListProjectGridSpec GridSpec;
        public readonly string GridName;
        public readonly string GridDataUrl;

        public FeaturedListViewData(Person currentPerson, Models.FirmaPage firmaPage) : base(currentPerson, firmaPage, false)
        {
            PageTitle = "Featured Projects";
            GridName = "featuredListGrid";
            GridSpec = new FeaturesListProjectGridSpec(currentPerson)
            {
                ObjectNameSingular = "Featured Project",
                ObjectNamePlural = "Featured Projects",
                SaveFiltersInCookie = true,
                CreateEntityModalDialogForm = new ModalDialogForm(SitkaRoute<ProjectController>.BuildUrlFromExpression(tc => tc.EditFeaturedProjects()), "Edit Featured Projects"),
                CreateEntityActionPhrase = "Add/Remove Featured Projects"
            };
            GridDataUrl = SitkaRoute<ProjectController>.BuildUrlFromExpression(tc => tc.FeaturedListGridJsonData());
        }
    }

    public class FeaturesListProjectGridSpec : BasicProjectInfoGridSpec
    {
        public FeaturesListProjectGridSpec(Person currentPerson)
            : base(currentPerson, true)
        {
            Add("# of Photos", x => x.ProjectImages.Count, 100);
            Add(string.Format("Reported {0}", MultiTenantHelpers.GetPerformanceMeasureNamePluralized()), x => string.Join(", ", x.PerformanceMeasureActuals.Select(pm => pm.PerformanceMeasureID).Distinct().OrderBy(pmID => pmID)), 100);
        }
    }
}
