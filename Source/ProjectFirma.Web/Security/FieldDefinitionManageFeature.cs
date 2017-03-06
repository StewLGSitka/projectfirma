﻿/*-----------------------------------------------------------------------
<copyright file="FieldDefinitionManageFeature.cs" company="Tahoe Regional Planning Agency">
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
using ProjectFirma.Web.Models;

namespace ProjectFirma.Web.Security
{
    [SecurityFeatureDescription("Manage Field Definitions")]
    public class FieldDefinitionManageFeature : FirmaFeatureWithContext, IFirmaBaseFeatureWithContext<FieldDefinition>
    {
        private readonly FirmaFeatureWithContextImpl<FieldDefinition> _firmaFeatureWithContextImpl;

        public FieldDefinitionManageFeature()
            : base(FirmaBaseFeatureHelpers.AllRolesExceptUnassigned)
        {
            _firmaFeatureWithContextImpl = new FirmaFeatureWithContextImpl<FieldDefinition>(this);
            ActionFilter = _firmaFeatureWithContextImpl;
        }

        public void DemandPermission(Person person, FieldDefinition contextModelObject)
        {
            _firmaFeatureWithContextImpl.DemandPermission(person, contextModelObject);
        }

        public PermissionCheckResult HasPermission(Person person, FieldDefinition contextModelObject)
        {
            if (HasPermissionByPerson(person))
            {
                return new PermissionCheckResult();
            }
            return new PermissionCheckResult("Does not have administration privileges");
        }
    }
}
