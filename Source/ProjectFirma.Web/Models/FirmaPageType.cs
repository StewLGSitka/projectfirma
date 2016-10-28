using ProjectFirma.Web.Controllers;
using LtInfo.Common;

namespace ProjectFirma.Web.Models
{
    public partial class FirmaPageType
    {
        public abstract string GetViewUrl();
    }

    public partial class FirmaPageTypeHomePage
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<HomeController>.BuildUrlFromExpression(x => x.Index());
        }
    }

    public partial class FirmaPageTypeAboutClackamasPartnership
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<AboutController>.BuildUrlFromExpression(x => x.AboutClackamasPartnership());
        }
    }

    public partial class FirmaPageTypeMeetings
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<AboutController>.BuildUrlFromExpression(x => x.AboutClackamasPartnership());
        }
    }

    public partial class FirmaPageTypeHomeAdditionalInfo
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<HomeController>.BuildUrlFromExpression(x => x.ViewPageContent(ToEnum));
        }
    }

    public partial class FirmaPageTypeFullProjectList
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<ProjectController>.BuildUrlFromExpression(x => x.Index());
        }
    }

    public partial class FirmaPageTypeFiveYearProjectList
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<ProjectController>.BuildUrlFromExpression(x => x.FiveYearList());
        }
    }

    public partial class FirmaPageTypeCompletedProjectList
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<ProjectController>.BuildUrlFromExpression(x => x.CompletedList());
        }
    }

    public partial class FirmaPageTypeTerminatedProjectList
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<ProjectController>.BuildUrlFromExpression(x => x.TerminatedList());
        }
    }

    public partial class FirmaPageTypePerformanceMeasuresList
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<PerformanceMeasureController>.BuildUrlFromExpression(x => x.Index());
        }
    }

    public partial class FirmaPageTypeActionPrioritiesList
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<ActionPriorityController>.BuildUrlFromExpression(x => x.Index());
        }
    }

    public partial class FirmaPageTypeLocalAndRegionalPlansList
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<LocalAndRegionalPlanController>.BuildUrlFromExpression(x => x.Index());
        }
    }

    public partial class FirmaPageTypeFocusAreasList
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<FocusAreaController>.BuildUrlFromExpression(x => x.Index());
        }
    }

    public partial class FirmaPageTypeFundingSourcesList
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<FundingSourceController>.BuildUrlFromExpression(x => x.Index());
        }
    }

    public partial class FirmaPageTypeOrganizationsList
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<OrganizationController>.BuildUrlFromExpression(x => x.Index());
        }
    }

    public partial class FirmaPageTypeProgramsList
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<ProgramController>.BuildUrlFromExpression(x => x.Index());
        }
    }

    public partial class FirmaPageTypeWatershedsList
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<WatershedController>.BuildUrlFromExpression(x => x.Index());
        }
    }

    public partial class FirmaPageTypeMyProjects
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<ProjectUpdateController>.BuildUrlFromExpression(x => x.MyProjectsRequiringAnUpdate());
        }
    }

    public partial class FirmaPageTypeMyOrganizationsProjects
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<ProjectController>.BuildUrlFromExpression(x => x.MyOrganizationsProjects());
        }
    }

    public partial class FirmaPageTypePagesWithIntroTextList
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<FirmaPageController>.BuildUrlFromExpression(x => x.Index());
        }
    }

    public partial class FirmaPageTypeInvestmentByFundingSector
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<ResultsController>.BuildUrlFromExpression(x => x.InvestmentByFundingSector(null));
        }
    }

    public partial class FirmaPageTypeSpendingBySectorByFocusAreaByProgram
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<ResultsController>.BuildUrlFromExpression(x => x.SpendingBySectorByFocusAreaByProgram(null));
        }
    }

    public partial class FirmaPageTypeSpendingByPerformanceMeasureByProject
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<ResultsController>.BuildUrlFromExpression(x => x.SpendingByPerformanceMeasureByProject(null));
        }
    }

    public partial class FirmaPageTypeProjectMap
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<ResultsController>.BuildUrlFromExpression(x => x.ProjectMap());
        }
    }

    public partial class FirmaPageTypeResultsByProgram
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<ResultsController>.BuildUrlFromExpression(x => x.ResultsByProgram(null));
        }
    }

    public partial class FirmaPageTypeTaxonomy
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<ProgramInfoController>.BuildUrlFromExpression(x => x.Taxonomy());
        }
    }

    public partial class FirmaPageTypeTransportationTaxonomy
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<ProgramInfoController>.BuildUrlFromExpression(x => x.TransportationTaxonomy());
        }
    }



    public partial class FirmaPageTypeFeaturedProjectList
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<ProjectController>.BuildUrlFromExpression(x => x.FeaturedList());
        }
    }

    public partial class FirmaPageTypeManageUpdateNotifications
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<ProjectUpdateController>.BuildUrlFromExpression(x => x.Manage());
        }
    }

    public partial class FirmaPageTypeProjectUpdateStatus
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<ProjectUpdateController>.BuildUrlFromExpression(x => x.ProjectUpdateStatus());
        }
    }

    public partial class FirmaPageTypeTransportationStrategiesList
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<TransportationStrategyController>.BuildUrlFromExpression(x => x.Index());
        }
    }

    public partial class FirmaPageTypeTransportationObjectivesList
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<TransportationObjectiveController>.BuildUrlFromExpression(x => x.Index());
        }
    }

    public partial class FirmaPageTypeTransportationProjectList
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<ProjectController>.BuildUrlFromExpression(x => x.TransportationList());
        }
    }

    public partial class FirmaPageTypeTransportationCostParameterSet
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<TransportationCostParameterSetController>.BuildUrlFromExpression(x => x.Summary());
        }
    }

    public partial class FirmaPageTypeFullProjectListSimple
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<ProjectController>.BuildUrlFromExpression(x => x.FullProjectListSimple());
        }
    }

    public partial class FirmaPageTypeTagList
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<TagController>.BuildUrlFromExpression(x => x.Index());
        }
    }

    public partial class FirmaPageTypeProposedProjects
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<ProposedProjectController>.BuildUrlFromExpression(x => x.Index());
        }
    }

    public partial class FirmaPageTypeThresholdCategoriesList
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<ThresholdCategoryController>.BuildUrlFromExpression(x => x.Index());
        }
    }

    public partial class FirmaPageTypeMonitoringProgramsList
    {
        public override string GetViewUrl()
        {
            return SitkaRoute<MonitoringProgramController>.BuildUrlFromExpression(x => x.Index());
        }
    }
}