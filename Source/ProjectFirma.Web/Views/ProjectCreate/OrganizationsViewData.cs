﻿/*-----------------------------------------------------------------------
<copyright file="OrganizationsViewData.cs" company="Tahoe Regional Planning Agency and Sitka Technology Group">
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

using ProjectFirmaModels.Models;
using ProjectFirma.Web.Views.Shared.ProjectOrganization;

namespace ProjectFirma.Web.Views.ProjectCreate
{
    public class OrganizationsViewData : ProjectCreateViewData
    {
        public readonly EditOrganizationsViewData EditOrganizationsViewData;

        public OrganizationsViewData(Person currentPerson,
            ProjectFirmaModels.Models.Project project,
            ProposalSectionsStatus proposalSectionsStatus, EditOrganizationsViewData editOrganizationsViewData) : base(currentPerson, project, ProjectCreateSection.Organizations.ProjectCreateSectionDisplayName, proposalSectionsStatus)
        {
            EditOrganizationsViewData = editOrganizationsViewData;
        }
    }
}
