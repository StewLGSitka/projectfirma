﻿/*-----------------------------------------------------------------------
<copyright file="IndexViewData.cs" company="Tahoe Regional Planning Agency and Sitka Technology Group">
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
using ProjectFirma.Web.Controllers;
using ProjectFirma.Web.Security;
using ProjectFirma.Web.Models;
using LtInfo.Common;

namespace ProjectFirma.Web.Views.ProposedProject
{
    public class IndexViewData : FirmaViewData
    {
        public readonly ProposedProjectGridSpec GridSpec;
        public readonly string GridName;
        public readonly string GridDataUrl;
        public readonly bool HasProposeProjectPermissions;
        public readonly string ProposeNewProjectUrl;

        public IndexViewData(Person currentPerson, Models.FirmaPage firmaPage) : base(currentPerson, firmaPage)
        {
            PageTitle = Models.FieldDefinition.ProposedProject.GetFieldDefinitionLabelPluralized();

            HasProposeProjectPermissions = new ProposedProjectEditFeature().HasPermissionByPerson(CurrentPerson);
            ProposeNewProjectUrl = SitkaRoute<ProposedProjectController>.BuildUrlFromExpression(x => x.Instructions(null));

            GridSpec = new ProposedProjectGridSpec(currentPerson) {ObjectNameSingular = $"{Models.FieldDefinition.ProposedProject.GetFieldDefinitionLabel()}", ObjectNamePlural = $"{Models.FieldDefinition.ProposedProject.GetFieldDefinitionLabelPluralized()}", SaveFiltersInCookie = true};
            if (new ProposedProjectEditFeature().HasPermissionByPerson(CurrentPerson))
            {
                GridSpec.CreateEntityActionPhrase = "Propose a New Project";
                GridSpec.CreateEntityModalDialogForm = null;
            }
            GridName = "proposedProjectsGrid";
            GridDataUrl = SitkaRoute<ProposedProjectController>.BuildUrlFromExpression(tc => tc.IndexGridJsonData());
        }
    }
}
