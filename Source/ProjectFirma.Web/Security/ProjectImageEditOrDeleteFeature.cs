﻿/*-----------------------------------------------------------------------
<copyright file="ProjectImageEditOrDeleteFeature.cs" company="Tahoe Regional Planning Agency and Sitka Technology Group">
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
using ProjectFirmaModels.Models;

namespace ProjectFirma.Web.Security
{
    [SecurityFeatureDescription("Edit/Delete {0} Image", FieldDefinitionEnum.Project)]
    public class ProjectImageEditOrDeleteFeature : FirmaFeatureWithContext, IFirmaBaseFeatureWithContext<ProjectImage>
    {
        private readonly FirmaFeatureWithContextImpl<ProjectImage> _firmaFeatureWithContextImpl;

        public ProjectImageEditOrDeleteFeature()
            : base(new List<Role> { Role.Normal, Role.SitkaAdmin, Role.Admin, Role.ProjectSteward })
        {
            _firmaFeatureWithContextImpl = new FirmaFeatureWithContextImpl<ProjectImage>(this);
            ActionFilter = _firmaFeatureWithContextImpl;
        }

        public void DemandPermission(Person person, ProjectImage contextModelObject)
        {
            _firmaFeatureWithContextImpl.DemandPermission(person, contextModelObject);
        }

        public PermissionCheckResult HasPermission(Person person, ProjectImage contextModelObject)
        {
            if (contextModelObject.Project.IsPendingProject())
            {
                return new ProjectCreateFeature().HasPermission(person, contextModelObject.Project);
            }

            return new ProjectEditAsAdminFeature().HasPermission(person, contextModelObject.Project);
        }
    }
}
