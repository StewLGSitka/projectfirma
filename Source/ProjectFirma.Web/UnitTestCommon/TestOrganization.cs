﻿/*-----------------------------------------------------------------------
<copyright file="TestOrganization.cs" company="Tahoe Regional Planning Agency">
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
using ProjectFirma.Web.Common;
using ProjectFirma.Web.Models;

namespace ProjectFirma.Web.UnitTestCommon
{
    public static partial class TestFramework
    {
        public static class TestOrganization
        {
            public static Organization Create()
            {
                var organization = Organization.CreateNewBlank(Sector.Local);
                return organization;
            }

            public static Organization Create(string organizationName)
            {
                var organization = new Organization(organizationName, Sector.Private, true);
                return organization;
            }

            public static Organization Create(DatabaseEntities dbContext)
            {
                string testOrganizationName = MakeTestName("Org Name");
                const int maxLengthOfOrganizationAbbreviation = Organization.FieldLengths.OrganizationAbbreviation - 1;
                string testOrganizationAbbreviation = TestFramework.MakeTestName(testOrganizationName, maxLengthOfOrganizationAbbreviation);
                Sector testSector = Sector.Federal;
                // Since a person contains an Org, we get into a chicken & egg recursion issue. So we put in a stubby Person to start with
                //Person testPersonPrimaryContact = TestPerson.Create();

                var testOrganization = new Organization(testOrganizationName, testSector, true);
                testOrganization.OrganizationAbbreviation = testOrganizationAbbreviation;
                //testOrganization.PrimaryContactPerson = testPersonPrimaryContact;

                // Now we sew up the Person with our org
                //testPersonPrimaryContact.Organization = testOrganization;
                //HttpRequestStorage.DatabaseEntities.People.Add(testPersonPrimaryContact);

                dbContext.AllOrganizations.Add(testOrganization);
                return testOrganization;
            }

            public static Organization Insert(DatabaseEntities dbContext)
            {
                var organization = Create(dbContext);
                HttpRequestStorage.DatabaseEntities.ChangeTracker.DetectChanges();
                HttpRequestStorage.DatabaseEntities.SaveChanges();
                return organization;
            }
        }
    }
}
