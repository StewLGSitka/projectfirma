﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LtInfo.Common;
using ProjectFirma.Web.Common;
using ProjectFirma.Web.Controllers;
using ProjectFirma.Web.Views.Shared.ProjectLocationControls;
using ProjectFirma.Web.Views.Shared.SortOrder;
using ProjectFirmaModels.Models;

namespace ProjectFirma.Web.Models
{
    public static class TaxonomyBranchModelExtensions
    {
        public static string GetDeleteUrl(this TaxonomyBranch taxonomyBranch)
        {
            return SitkaRoute<TaxonomyBranchController>.BuildUrlFromExpression(c => c.DeleteTaxonomyBranch(taxonomyBranch.TaxonomyBranchID));
        }

        public static string GetDetailUrl(this TaxonomyBranch taxonomyBranch)
        {
            return SitkaRoute<TaxonomyBranchController>.BuildUrlFromExpression(x => x.Detail(taxonomyBranch.TaxonomyBranchID));
        }

        public static string GetCustomizedMapUrl(this TaxonomyBranch taxonomyBranch)
        {
            return ProjectMapCustomization.BuildCustomizedUrl(ProjectLocationFilterType.TaxonomyBranch, taxonomyBranch.TaxonomyBranchID.ToString(), ProjectColorByType.ProjectStage);
        }

        public static bool IsTaxonomyBranchNameUnique(IEnumerable<TaxonomyBranch> taxonomyBranches, string taxonomyBranchName, int currentTaxonomyBranchID)
        {
            var taxonomyBranch = taxonomyBranches.SingleOrDefault(x => x.TaxonomyBranchID != currentTaxonomyBranchID && string.Equals(x.TaxonomyBranchName, taxonomyBranchName, StringComparison.InvariantCultureIgnoreCase));
            return taxonomyBranch == null;
        }

        public static List<Project> GetAssociatedProjects(this TaxonomyBranch taxonomyBranch, Person person)
        {
            return taxonomyBranch.TaxonomyLeafs.SelectMany(y => y.Projects).ToList().GetActiveProjectsAndProposals(person.CanViewProposals());
        }


        public static FancyTreeNode ToFancyTreeNode(this TaxonomyBranch taxonomyBranch, Person currentPerson)
        {
            var fancyTreeNode = new FancyTreeNode($"{UrlTemplate.MakeHrefString(taxonomyBranch.GetDetailUrl(), taxonomyBranch.GetDisplayName())}", taxonomyBranch.TaxonomyBranchID.ToString(), false)
            {
                ThemeColor = string.IsNullOrWhiteSpace(taxonomyBranch.ThemeColor) ? taxonomyBranch.TaxonomyTrunk.ThemeColor : taxonomyBranch.ThemeColor,
                MapUrl = taxonomyBranch.GetCustomizedMapUrl(),
                Children = taxonomyBranch.TaxonomyLeafs.SortByOrderThenName().Select(x => x.ToFancyTreeNode(currentPerson)).ToList()
            };
            return fancyTreeNode;
        }

        public static HtmlString GetDisplayNameAsUrl(this TaxonomyBranch taxonomyBranch)
        {
            return UrlTemplate.MakeHrefString(taxonomyBranch.GetDetailUrl(), taxonomyBranch.GetDisplayName());
        }
    }
}