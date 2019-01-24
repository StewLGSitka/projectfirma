﻿/*-----------------------------------------------------------------------
<copyright file="TaxonomyBranch.cs" company="Tahoe Regional Planning Agency and Sitka Technology Group">
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
using System.Linq;
using System.Web;
using LtInfo.Common;

namespace ProjectFirmaModels.Models
{
    public partial class TaxonomyBranch : IAuditableEntity, ITaxonomyTier
    {
        public void SetSortOrder(int? value) => TaxonomyBranchSortOrder = value;

        public int? GetSortOrder() => TaxonomyBranchSortOrder;
        public int GetID() => TaxonomyBranchID;

        public List<Project> GetAssociatedProjects(Person person)
        {
            return TaxonomyLeafs.SelectMany(y => y.Projects).ToList().GetActiveProjectsAndProposals(person.CanViewProposals());
        }

        public int GetTaxonomyTierID() => TaxonomyBranchID;

        public string GetDisplayName()
        {
            var taxonomyPrefix = string.IsNullOrWhiteSpace(TaxonomyBranchCode)
                ? string.Empty
                : $"{TaxonomyBranchCode}: ";
            return $"{taxonomyPrefix}{TaxonomyBranchName}";
        }

        public HtmlString GetDisplayNameAsUrl()
        {
            return UrlTemplate.MakeHrefString(GetDetailUrl(), GetDisplayName());
        }

        public string GetDetailUrl()
        {
            return TaxonomyBranchModelExtensions.GetDetailUrl(this);
        }

        public string GetAuditDescriptionString() => TaxonomyBranchName;

        public List<IGrouping<PerformanceMeasure, TaxonomyLeafPerformanceMeasure>> GetTaxonomyTierPerformanceMeasures()
        {
            return TaxonomyLeafs.SelectMany(x => x.TaxonomyLeafPerformanceMeasures).GroupBy(y => y.PerformanceMeasure).ToList();
        }

        public FancyTreeNode ToFancyTreeNode(Person currentPerson)
        {
            return TaxonomyBranchModelExtensions.ToFancyTreeNode(this, currentPerson);
        }
    }
}
