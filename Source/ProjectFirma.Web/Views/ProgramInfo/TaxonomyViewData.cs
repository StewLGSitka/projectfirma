using System.Collections.Generic;
using ProjectFirma.Web.Models;

namespace ProjectFirma.Web.Views.ProgramInfo
{
    public class TaxonomyViewData : FirmaViewData
    {
        public readonly List<FancyTreeNode> TaxonomyTierThreesAsFancyTreeNodes;

        public TaxonomyViewData(Person currentPerson, Models.FirmaPage firmaPage,
            List<FancyTreeNode> taxonomyTierThreesAsFancyTreeNodes) : base(currentPerson, firmaPage)
        {
            TaxonomyTierThreesAsFancyTreeNodes = taxonomyTierThreesAsFancyTreeNodes;
            PageTitle = "Project Taxonomy";
        }
    }
}