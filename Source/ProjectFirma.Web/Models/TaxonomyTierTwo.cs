﻿/*-----------------------------------------------------------------------
<copyright file="TaxonomyTierTwo.cs" company="Tahoe Regional Planning Agency">
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjectFirma.Web.Controllers;
using ProjectFirma.Web.Views.Shared.ProjectLocationControls;
using LtInfo.Common;

namespace ProjectFirma.Web.Models
{
    public partial class TaxonomyTierTwo : IAuditableEntity
    {
        public string DeleteUrl
        {
            get { return SitkaRoute<TaxonomyTierTwoController>.BuildUrlFromExpression(c => c.DeleteTaxonomyTierTwo(TaxonomyTierTwoID)); }
        }

        public ICollection<Project> Projects
        {
            get { return TaxonomyTierOnes.SelectMany(x => x.Projects).ToList(); }
        }

        public string DisplayName
        {
            get
            {
                var taxonomyPrefix = string.IsNullOrWhiteSpace(TaxonomyTierTwoCode) ? string.Empty : string.Format("{0}: ", TaxonomyTierTwoCode);
                return string.Format("{0}{1}", taxonomyPrefix, TaxonomyTierTwoName);
            }
        }

        public HtmlString GetDisplayNameAsUrl()
        {
            return UrlTemplate.MakeHrefString(SummaryUrl, DisplayName);
        }

        public string SummaryUrl
        {
            get { return SitkaRoute<TaxonomyTierTwoController>.BuildUrlFromExpression(x => x.Detail(TaxonomyTierTwoID)); }
        }

        public string CustomizedMapUrl
        {
            get { return ProjectMapCustomization.BuildCustomizedUrl(ProjectLocationFilterType.TaxonomyTierTwo, TaxonomyTierTwoID.ToString(), ProjectColorByType.ProjectStage); }
        }


        public static bool IsTaxonomyTierTwoNameUnique(IEnumerable<TaxonomyTierTwo> taxonomyTierTwos, string taxonomyTierTwoName, int currentTaxonomyTierTwoID)
        {
            var taxonomyTierTwo = taxonomyTierTwos.SingleOrDefault(x => x.TaxonomyTierTwoID != currentTaxonomyTierTwoID && String.Equals(x.TaxonomyTierTwoName, taxonomyTierTwoName, StringComparison.InvariantCultureIgnoreCase));
            return taxonomyTierTwo == null;
        }

        public string AuditDescriptionString
        {
            get { return TaxonomyTierTwoName; }
        }

        public List<PerformanceMeasure> GetPerformanceMeasures()
        {
            var performanceMeasures = TaxonomyTierTwoPerformanceMeasures.Where(x => x.IsPrimaryTaxonomyTierTwo).OrderBy(x => x.PerformanceMeasure.PerformanceMeasureDisplayName).Select(x => x.PerformanceMeasure).ToList();
            return performanceMeasures;
        }

        public FancyTreeNode ToFancyTreeNode()
        {
            var fancyTreeNode = new FancyTreeNode(string.Format("{0}", UrlTemplate.MakeHrefString(SummaryUrl, DisplayName)), TaxonomyTierTwoID.ToString(), false)
            {
                ThemeColor = string.IsNullOrWhiteSpace(ThemeColor) ? TaxonomyTierThree.ThemeColor : ThemeColor,
                MapUrl = CustomizedMapUrl,
                Children = TaxonomyTierOnes.Select(x => x.ToFancyTreeNode()).ToList()
            };
            return fancyTreeNode;
        }
    }
}
