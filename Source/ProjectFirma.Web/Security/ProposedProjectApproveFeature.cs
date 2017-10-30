﻿/*-----------------------------------------------------------------------
<copyright file="ProposedProjectApproveFeature.cs" company="Tahoe Regional Planning Agency and Sitka Technology Group">
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
using ProjectFirma.Web.Models;

namespace ProjectFirma.Web.Security
{
    [SecurityFeatureDescription("Approve {0}", FieldDefinitionEnum.ProposedProject)]
    public class ProposedProjectApproveFeature : FirmaFeatureWithContext, IFirmaBaseFeatureWithContext<ProposedProject>
    {
        private readonly FirmaFeatureWithContextImpl<ProposedProject> _firmaFeatureWithContextImpl;

        public ProposedProjectApproveFeature()
            : base(new List<Role> { Role.SitkaAdmin, Role.Admin, Role.ProjectSteward })
        {
            _firmaFeatureWithContextImpl = new FirmaFeatureWithContextImpl<ProposedProject>(this);
            ActionFilter = _firmaFeatureWithContextImpl;
        }

        public PermissionCheckResult HasPermission(Person person, ProposedProject contextModelObject)
        {
            var proposedProjectLabel = FieldDefinition.ProposedProject.GetFieldDefinitionLabel();
            if (!HasPermissionByPerson(person))
            {
                return new PermissionCheckResult($"You do not have permission to approve this {proposedProjectLabel}.");
            }

            if (person.Role.RoleID == Role.ProjectSteward.RoleID &&
                !person.CanApproveProposedProjectByOrganizationRelationship(contextModelObject))
            {
                var organizationLabel = FieldDefinition.Organization.GetFieldDefinitionLabel();
                return new PermissionCheckResult($"You do not have permission to approve this {proposedProjectLabel} based on your relationship to the {proposedProjectLabel}'s {organizationLabel}.");
            }

            return new PermissionCheckResult();
        }

        public void DemandPermission(Person person, ProposedProject contextModelObject)
        {
            _firmaFeatureWithContextImpl.DemandPermission(person, contextModelObject);
        }
    }
}
