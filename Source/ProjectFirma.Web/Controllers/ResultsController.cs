using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using ProjectFirma.Web.Security;
using ProjectFirma.Web.Common;
using ProjectFirma.Web.Models;
using ProjectFirma.Web.Views.Map;
using ProjectFirma.Web.Views.Project;
using ProjectFirma.Web.Views.Results;
using ProjectFirma.Web.Views.Shared.ProjectLocationControls;
using ProjectFirma.Web.Security.Shared;
using ProjectFirma.Web.Views.Indicator;
using ProjectFirma.Web.Views.Shared;
using LtInfo.Common;
using LtInfo.Common.ExcelWorkbookUtilities;
using LtInfo.Common.Models;
using LtInfo.Common.Mvc;
using LtInfo.Common.MvcResults;

namespace ProjectFirma.Web.Controllers
{
    public class ResultsController : FirmaBaseController
    {
        [InvestmentByFundingSourceViewFeature]
        public ViewResult InvestmentByFundingSector(int? calendarYear)
        {
            var projectFundingSourceExpenditures = GetProjectExpendituresByFundingSector(null, null);
            var projectCount = GetProjectCountForInvestmentByFundingSectorReport(calendarYear);
            var fundingSectorExpenditures = GetFundingSectorExpendituresForInvestmentByFundingSectorReport(calendarYear, projectFundingSourceExpenditures);
            var calendarYears = GetCalendarYearsDropdownForInvestmentByFundingSectorAndSpendingBySectorAndFocusAreaReports(projectFundingSourceExpenditures);
            var reportingYearRangeTitle = YearDisplayName(calendarYear);
            var firmaPage = FirmaPage.GetFirmaPageByPageType(FirmaPageType.InvestmentByFundingSector);
            var viewData = new InvestmentByFundingSectorViewData(CurrentPerson, firmaPage, fundingSectorExpenditures, calendarYear, calendarYears, projectCount, reportingYearRangeTitle);
            var viewModel = new InvestmentByFundingSectorViewModel(calendarYear);
            return RazorView<InvestmentByFundingSector, InvestmentByFundingSectorViewData, InvestmentByFundingSectorViewModel>(viewData, viewModel);
        }

        private static string YearDisplayName(int? year)
        {
            var currentYearToUseForReporting = FirmaDateUtilities.CalculateCurrentYearToUseForReporting();
            if (!year.HasValue)
            {
                return String.Format("Recent Years ({0} - {1})", FirmaDateUtilities.GetMinimumYearForReportingExpenditures(), currentYearToUseForReporting);
            }
            if (year.Value == FirmaDateUtilities.GetYearUsedToRepresentPreReportingYears())
            {
                return String.Format("Early Years ({0} - {1})", FirmaDateUtilities.MinimumYear, FirmaDateUtilities.GetYearUsedToRepresentPreReportingYears());
            }
            if (year.Value == FirmaDateUtilities.MinimumYear)
            {
                return String.Format("All Years ({0} - {1})", FirmaDateUtilities.MinimumYear, currentYearToUseForReporting);
            }
            return year.Value.ToString(CultureInfo.InvariantCulture);
        }

        private static IEnumerable<SelectListItem> GetCalendarYearsDropdownForInvestmentByFundingSectorAndSpendingBySectorAndFocusAreaReports(
            IEnumerable<ProjectFundingSourceExpenditure> projectFundingSourceExpenditures)
        {
            var allYearsWithValues = projectFundingSourceExpenditures.Select(x => x.CalendarYear).Distinct().ToList();
            allYearsWithValues.AddRange(new[] { FirmaDateUtilities.GetYearUsedToRepresentPreReportingYears(), FirmaDateUtilities.MinimumYear });
            var calendarYears =
                allYearsWithValues.OrderByDescending(x => x).ToSelectListWithEmptyFirstRow(x => x.ToString(CultureInfo.InvariantCulture), x => YearDisplayName(x), YearDisplayName(null)).ToList();
            return calendarYears;
        }

        private static List<FundingSectorExpenditure> GetFundingSectorExpendituresForInvestmentByFundingSectorReport(int? calendarYear,
            IEnumerable<ProjectFundingSourceExpenditure> projectFundingSourceExpenditures)
        {
            List<FundingSectorExpenditure> fundingSectorExpenditures;
            if (!calendarYear.HasValue)
            {
                fundingSectorExpenditures =
                    Sector.All.Select(sector => new FundingSectorExpenditure(sector, projectFundingSourceExpenditures.Where(y => y.FundingSource.Organization.Sector == sector).ToList(), null))
                        .ToList();
            }
            else
            {
                if (calendarYear.Value == FirmaDateUtilities.MinimumYear)
                {
                    fundingSectorExpenditures = Sector.All.Select(sector =>
                    {
                        var fundingSourceExpendituresForThisSector = projectFundingSourceExpenditures.Where(y => y.FundingSource.Organization.Sector == sector).ToList();
                        return new FundingSectorExpenditure(sector,
                            fundingSourceExpendituresForThisSector.Sum(y => y.ExpenditureAmount) + sector.GetPreReportingYearExpenditures(),
                            fundingSourceExpendituresForThisSector.Select(y => y.FundingSourceID).Distinct().Count(),
                            fundingSourceExpendituresForThisSector.Select(y => y.FundingSource.OrganizationID).Distinct().Count(),
                            null);
                    }).ToList();
                }
                else if (calendarYear.Value == FirmaDateUtilities.GetYearUsedToRepresentPreReportingYears())
                {
                    fundingSectorExpenditures =
                        Sector.All.Select(x => new FundingSectorExpenditure(x, x.GetPreReportingYearExpenditures(), 0, 0, FirmaDateUtilities.GetYearUsedToRepresentPreReportingYears())).ToList();
                }
                else
                {
                    fundingSectorExpenditures =
                        Sector.All.Select(
                            sector =>
                                new FundingSectorExpenditure(sector,
                                    projectFundingSourceExpenditures.Where(y => y.FundingSource.Organization.Sector == sector && y.CalendarYear == calendarYear.Value).ToList(),
                                    calendarYear)).ToList();
                }
            }
            return fundingSectorExpenditures;
        }

        private static int GetProjectCountForInvestmentByFundingSectorReport(int? calendarYear)
        {
            var performanceMeasure34 =
                HttpRequestStorage.DatabaseEntities.PerformanceMeasures.Single(x => x.PerformanceMeasureTypeID == PerformanceMeasureType.PerformanceMeasure34.PerformanceMeasureTypeID);
            var projectCount =
                Convert.ToInt32(
                    performanceMeasure34.PerformanceMeasureType.CalculatePerformanceMeasureReportedValues(performanceMeasure34, null)
                        .Where(x => !calendarYear.HasValue || x.CalendarYear == calendarYear.Value)
                        .Sum(x => x.ReportedValue));

            if (calendarYear.HasValue)
            {
                if (calendarYear.Value == FirmaDateUtilities.MinimumYear)
                {
                    projectCount += Common.FirmaWebConfiguration.Pre2007ProjectCount;
                }
                else if (calendarYear.Value == FirmaDateUtilities.GetYearUsedToRepresentPreReportingYears())
                {
                    projectCount = Common.FirmaWebConfiguration.Pre2007ProjectCount;
                }
            }
            return projectCount;
        }

        [InvestmentByFundingSourceViewFeature]
        public PartialViewResult ProjectFundingSourceExpendituresBySector(int? sectorID, int? calendarYear)
        {
            var viewData = new ProjectFundingSourceExpendituresBySectorViewData(sectorID, calendarYear);
            return RazorPartialView<ProjectFundingSourceExpendituresBySector, ProjectFundingSourceExpendituresBySectorViewData>(viewData);
        }

        [InvestmentByFundingSourceViewFeature]
        public GridJsonNetJObjectResult<ProjectFundingSourceSectorExpenditure> ProjectExpendituresByFundingSectorGridJsonData(int? sectorID, int? calendarYear)
        {
            ProjectFundingSourceExpendituresBySectorGridSpec gridSpec;
            var projectFundingSourceSectorExpenditures = GetProjectExpendituresByFundingSectorAndGridSpec(out gridSpec, sectorID, calendarYear);
            var gridJsonNetJObjectResult = new GridJsonNetJObjectResult<ProjectFundingSourceSectorExpenditure>(projectFundingSourceSectorExpenditures, gridSpec);
            return gridJsonNetJObjectResult;
        }

        private static List<ProjectFundingSourceSectorExpenditure> GetProjectExpendituresByFundingSectorAndGridSpec(out ProjectFundingSourceExpendituresBySectorGridSpec gridSpec,
            int? sectorID,
            int? calendarYear)
        {
            gridSpec = new ProjectFundingSourceExpendituresBySectorGridSpec(calendarYear);
            var projectFundingSourceExpenditures = GetProjectExpendituresByFundingSector(sectorID, calendarYear);
            var projectFundingSourceSectorExpenditures = ProjectFundingSourceSectorExpenditure.MakeFromProjectFundingSourceExpenditures(projectFundingSourceExpenditures);
            return projectFundingSourceSectorExpenditures;
        }

        private static List<ProjectFundingSourceExpenditure> GetProjectExpendituresByFundingSector(int? sectorID, int? calendarYear)
        {
            var currentYearToUseForReporting = FirmaDateUtilities.CalculateCurrentYearToUseForReporting();
            return HttpRequestStorage.DatabaseEntities.ProjectFundingSourceExpenditures.GetExpendituresFromMininumYearForReportingOnward()
                    .Where(x => (!calendarYear.HasValue && x.CalendarYear <= currentYearToUseForReporting) || x.CalendarYear == calendarYear)
                    .ToList()
                    .Where(x => !sectorID.HasValue || (x.FundingSource.Organization.Sector == Sector.AllLookupDictionary[sectorID.Value]))
                    .OrderBy(x => x.Project.ProjectNumberString)
                    .ToList();
        }

        [SpendingBySectorByFocusAreaByProgramViewFeature]
        public ViewResult SpendingBySectorByFocusAreaByProgram(int? calendarYear)
        {
            var projectFundingSourceExpenditures = GetProjectExpendituresByFundingSector(null, null);
            var programSectorExpenditures = GetProgramSectorExpenditures(calendarYear, projectFundingSourceExpenditures);
            var firmaPage = FirmaPage.GetFirmaPageByPageType(FirmaPageType.SpendingBySectorByFocusAreaByProgram);
            var calendarYears = GetCalendarYearsDropdownForInvestmentByFundingSectorAndSpendingBySectorAndFocusAreaReports(projectFundingSourceExpenditures);
            var viewData = new SpendingBySectorByFocusAreaByProgramViewData(CurrentPerson, firmaPage, programSectorExpenditures, Sector.All, calendarYear, calendarYears);
            var viewModel = new SpendingBySectorByFocusAreaByProgramViewModel(calendarYear);
            return RazorView<SpendingBySectorByFocusAreaByProgram, SpendingBySectorByFocusAreaByProgramViewData, SpendingBySectorByFocusAreaByProgramViewModel>(viewData, viewModel);
        }

        private static List<ProgramSectorExpenditure> GetProgramSectorExpenditures(int? calendarYear, IEnumerable<ProjectFundingSourceExpenditure> projectFundingSourceExpenditures)
        {
            List<ProgramSectorExpenditure> programSectorExpenditures;
            if (!calendarYear.HasValue)
            {
                programSectorExpenditures =
                    projectFundingSourceExpenditures.GroupBy(y => new {y.Project.ActionPriority.Program, y.FundingSource.Organization.Sector})
                        .Select(x => new ProgramSectorExpenditure(x.Key.Sector, x.Key.Program, x.Sum(y => y.ExpenditureAmount)))
                        .ToList();
            }
            else
            {
                if (calendarYear.Value == FirmaDateUtilities.MinimumYear)
                {
                    programSectorExpenditures =
                        projectFundingSourceExpenditures.GroupBy(y => new {y.Project.ActionPriority.Program, y.FundingSource.Organization.Sector})
                            .Select(x => new ProgramSectorExpenditure(x.Key.Sector, x.Key.Program, x.Sum(y => y.ExpenditureAmount)))
                            .ToList();
                    programSectorExpenditures.AddRange(Sector.All.Select(x => new ProgramSectorExpenditure(x, "Unknown", "Unknown", x.GetPreReportingYearExpenditures())).ToList());
                }
                else if (calendarYear.Value == FirmaDateUtilities.GetYearUsedToRepresentPreReportingYears())
                {
                    programSectorExpenditures = Sector.All.Select(x => new ProgramSectorExpenditure(x, "Unknown", "Unknown", x.GetPreReportingYearExpenditures())).ToList();
                }
                else
                {
                    programSectorExpenditures =
                        projectFundingSourceExpenditures.Where(x => x.CalendarYear == calendarYear.Value)
                            .GroupBy(y => new {y.Project.ActionPriority.Program, y.FundingSource.Organization.Sector})
                            .Select(x => new ProgramSectorExpenditure(x.Key.Sector, x.Key.Program, x.Sum(y => y.ExpenditureAmount)))
                            .ToList();
                }
            }
            return programSectorExpenditures;
        }

        [ProjectLocationsViewFeature]
        public ViewResult ProjectMap()
        {
            List<int> filterValues;
            ProjectLocationFilterType projectLocationFilterType;
            ProjectColorByType colorByValue;

            if (!String.IsNullOrEmpty(Request.QueryString[ProjectMapCustomization.FilterByQueryStringParameter]))
            {
                projectLocationFilterType = ProjectLocationFilterType.ToType(Request.QueryString[ProjectMapCustomization.FilterByQueryStringParameter].ParseAsEnum<ProjectLocationFilterTypeEnum>());
            }
            else
            {
                projectLocationFilterType = ProjectLocationFilterType.Program;
            }

            if (!String.IsNullOrEmpty(Request.QueryString[ProjectMapCustomization.FilterValuesQueryStringParameter]))
            {
                var filterValuesAsString = Request.QueryString[ProjectMapCustomization.FilterValuesQueryStringParameter].Split(',');
                filterValues = filterValuesAsString.Select(Int32.Parse).ToList();
            }
            else
            {
                filterValues = HttpRequestStorage.DatabaseEntities.Programs.Select(x => x.ProgramID).OrderBy(x => x).ToList();
            }

            if (!String.IsNullOrEmpty(Request.QueryString[ProjectMapCustomization.ColorByQueryStringParameter]))
            {
                colorByValue = ProjectColorByType.ToType(Request.QueryString[ProjectMapCustomization.ColorByQueryStringParameter].ParseAsEnum<ProjectColorByTypeEnum>());
            }
            else
            {
                colorByValue = ProjectColorByType.FocusArea;
            }

            var firmaPage = FirmaPage.GetFirmaPageByPageType(FirmaPageType.ProjectMap);

            var allProjects = HttpRequestStorage.DatabaseEntities.Projects.ToList();
            var projects = allProjects.Where(p => p.IsVisibleToThisPerson(CurrentPerson)).ToList();

            var initialCustomization = new ProjectMapCustomization(projectLocationFilterType, filterValues, colorByValue);
            var projectLocationsLayerGeoJson = new LayerGeoJson("Project Locations", Project.MappedPointsToGeoJsonFeatureCollection(projects, true), "red", 1, LayerInitialVisibility.Show);
            var namedAreasAsPointsLayerGeoJson = new LayerGeoJson("Named Areas", Project.NamedAreasToPointGeoJsonFeatureCollection(projects, true), "red", 1, LayerInitialVisibility.Hide);
            var projectLocationsMapInitJson = new ProjectLocationsMapInitJson(projectLocationsLayerGeoJson, namedAreasAsPointsLayerGeoJson, initialCustomization, "ProjectLocationsMap");

            var projectStages = (ProjectStage.All.Where(x => x.ShouldShowOnMap())).OrderBy(x => x.SortOrder).ToList();
            var focusAreas = HttpRequestStorage.DatabaseEntities.FocusAreas.ToList();
            var projectLocationsMapViewData = new ProjectLocationsMapViewData(projectLocationsMapInitJson.MapDivID, colorByValue.ProjectColorByTypeDisplayName);

            var projectLocationFilterTypesAndValues = CreateProjectLocationFilterTypesAndValuesDictionary(focusAreas, projects, projectStages);
            var projectLocationsUrl = SitkaRoute<ResultsController>.BuildAbsoluteUrlHttpsFromExpression(x => x.ProjectMap(), SitkaWebConfiguration.CanonicalHostName);
            var filteredProjectsWithLocationAreasUrl = SitkaRoute<ResultsController>.BuildUrlFromExpression(x => x.FilteredProjectsWithLocationAreas(null));

            var viewData = new ProjectMapViewData(CurrentPerson,
                firmaPage,
                projectLocationsMapInitJson,
                projectLocationsMapViewData,
                projectLocationFilterTypesAndValues,
                projectLocationsUrl, filteredProjectsWithLocationAreasUrl);
            return RazorView<ProjectMap, ProjectMapViewData>(viewData);
        }

        private static Dictionary<ProjectLocationFilterType, IEnumerable<SelectListItem>> CreateProjectLocationFilterTypesAndValuesDictionary(List<FocusArea> focusAreas,
            List<Project> projects,
            List<ProjectStage> projectStages)
        {
            var projectLocationFilterTypesAndValues = new Dictionary<ProjectLocationFilterType, IEnumerable<SelectListItem>>();

            var focusAreasAsSelectListItems = focusAreas.ToSelectList(x => x.FocusAreaID.ToString(CultureInfo.InvariantCulture), x => x.DisplayName);

            var programsAsSelectListItems = HttpRequestStorage.DatabaseEntities.Programs.ToSelectList(x => x.ProgramID.ToString(CultureInfo.InvariantCulture),
                x => x.DisplayName);
            var actionPrioritiesAsSelectListItems = HttpRequestStorage.DatabaseEntities.ActionPriorities.ToSelectList(x => x.ActionPriorityID.ToString(CultureInfo.InvariantCulture),
                x => x.DisplayName);
            var thresholdCategoriesAsSelectListItems =
                HttpRequestStorage.DatabaseEntities.ThresholdCategories.ToSelectList(x => x.ThresholdCategoryID.ToString(CultureInfo.InvariantCulture), x => x.DisplayName);
            var implementingOrganizationsAsSelectListItems =
                projects.SelectMany(x => x.ProjectImplementingOrganizations)
                    .Select(x => x.Organization)
                    .Distinct(new HavePrimaryKeyComparer<Organization>())
                    .OrderBy(x => x.DisplayName)
                    .ToSelectList(x => x.OrganizationID.ToString(CultureInfo.InvariantCulture), x => x.DisplayName);
            var fundingOrganizationsAsSelectListItems =
                projects.SelectMany(x => x.ProjectFundingOrganizations)
                    .Select(x => x.Organization)
                    .Distinct(new HavePrimaryKeyComparer<Organization>())
                    .OrderBy(x => x.DisplayName)
                    .ToSelectList(x => x.OrganizationID.ToString(CultureInfo.InvariantCulture), x => x.DisplayName);
            var projectStagesAsSelectListItems = projectStages.ToSelectList(x => x.ProjectStageID.ToString(CultureInfo.InvariantCulture), x => x.ProjectStageDisplayName);

            projectLocationFilterTypesAndValues.Add(ProjectLocationFilterType.FocusArea, focusAreasAsSelectListItems);
            projectLocationFilterTypesAndValues.Add(ProjectLocationFilterType.Program, programsAsSelectListItems);
            projectLocationFilterTypesAndValues.Add(ProjectLocationFilterType.ActionPriority, actionPrioritiesAsSelectListItems);
            projectLocationFilterTypesAndValues.Add(ProjectLocationFilterType.ThresholdCategory, thresholdCategoriesAsSelectListItems);
            projectLocationFilterTypesAndValues.Add(ProjectLocationFilterType.ImplementingOrganization, implementingOrganizationsAsSelectListItems);
            projectLocationFilterTypesAndValues.Add(ProjectLocationFilterType.FundingOrganization, fundingOrganizationsAsSelectListItems);
            projectLocationFilterTypesAndValues.Add(ProjectLocationFilterType.ProjectStage, projectStagesAsSelectListItems);

            return projectLocationFilterTypesAndValues;
        }

        [ProjectLocationsViewFeature]
        [HttpGet]
        public ContentResult FilteredProjectsWithLocationAreas()
        {
            return new ContentResult();
        }


        [ProjectLocationsViewFeature]
        [HttpPost]
        public JsonNetJArrayResult FilteredProjectsWithLocationAreas(ProjectMapCustomization projectMapCustomization)
        {
            if (projectMapCustomization.FilterPropertyValues == null || !projectMapCustomization.FilterPropertyValues.Any())
            {
                return new JsonNetJArrayResult(new List<object>());
            }
            var projectLocationGroupsAsFancyTreeNodes = HttpRequestStorage.DatabaseEntities.ProjectLocationAreaGroups.ToList().Select(x => x.ToFancyTreeNode()).ToList();

            var projectLocationFilterTypeFromFilterPropertyName = projectMapCustomization.GetProjectLocationFilterTypeFromFilterPropertyName();
            var filterFunction = projectLocationFilterTypeFromFilterPropertyName.GetFilterFunction(projectMapCustomization.FilterPropertyValues);
            var filteredProjects =
                HttpRequestStorage.DatabaseEntities.Projects.Where(
                    filterFunction).ToList();

            var projects = IsCurrentUserAnonymous() ? filteredProjects.Where(p => p.IsVisibleToEveryone()).ToList() : filteredProjects;
            var filteredProjectsWithLocationAreas = projects.Where(x => !x.HasProjectLocationPoint && x.ProjectLocationAreaID.HasValue).ToList();

            projectLocationGroupsAsFancyTreeNodes.RemoveAll(
                typeNode =>
                    typeNode.Children.Count ==
                    typeNode.Children.RemoveAll(
                        areaNameNode =>
                            areaNameNode.Children.Count ==
                            areaNameNode.Children.RemoveAll(projectNode => !filteredProjectsWithLocationAreas.Select(project => project.ProjectID.ToString()).Contains(projectNode.Key))));

            return new JsonNetJArrayResult(projectLocationGroupsAsFancyTreeNodes);
        }

        [SpendingBySectorByOrganizationViewFeature]
        public PartialViewResult SpendingBySectorByOrganization(int sectorID, int? calendarYear)
        {
            var viewData = GetSpendingBySectorByOrganizationViewData(sectorID, calendarYear);
            return RazorPartialView<SpendingBySectorByOrganization, SpendingBySectorByOrganizationViewData>(viewData);
        }

        private static SpendingBySectorByOrganizationViewData GetSpendingBySectorByOrganizationViewData(int sectorID, int? calendarYear)
        {
            var projectFundingSourceExpenditures = GetProjectExpendituresByFundingSector(sectorID, null);
            var sector = Sector.AllLookupDictionary[sectorID];
            List<int> calendarYearRange;
            List<FundingSourceCalendarYearExpenditure> fundingSourceCalendarYearExpenditures;
            if (!calendarYear.HasValue)
            {
                calendarYearRange = FirmaDateUtilities.GetRangeOfYearsForReportingExpenditures();
                fundingSourceCalendarYearExpenditures =
                    FundingSourceCalendarYearExpenditure.CreateFromFundingSourcesAndCalendarYears(new List<IFundingSourceExpenditure>(projectFundingSourceExpenditures), calendarYearRange);
            }
            else
            {
                if (calendarYear.Value == FirmaDateUtilities.MinimumYear)
                {
                    calendarYearRange = FirmaDateUtilities.GetRangeOfYears(FirmaDateUtilities.GetYearUsedToRepresentPreReportingYears(), FirmaDateUtilities.CalculateCurrentYearToUseForReporting());
                    fundingSourceCalendarYearExpenditures =
                        FundingSourceCalendarYearExpenditure.CreateFromFundingSourcesAndCalendarYears(new List<IFundingSourceExpenditure>(projectFundingSourceExpenditures), calendarYearRange);
                    var calendarYearExpenditureForPreReportingYears = calendarYearRange.ToDictionary(x => x, x => (decimal?) null);
                    calendarYearExpenditureForPreReportingYears[FirmaDateUtilities.GetYearUsedToRepresentPreReportingYears()] = sector.GetPreReportingYearExpenditures();
                    fundingSourceCalendarYearExpenditures.Add(new FundingSourceCalendarYearExpenditure(calendarYearExpenditureForPreReportingYears));
                }
                else if (calendarYear.Value == FirmaDateUtilities.GetYearUsedToRepresentPreReportingYears())
                {
                    calendarYearRange = new List<int> {FirmaDateUtilities.GetYearUsedToRepresentPreReportingYears()};
                    fundingSourceCalendarYearExpenditures = new List<FundingSourceCalendarYearExpenditure>
                    {
                        new FundingSourceCalendarYearExpenditure(new Dictionary<int, decimal?> {{FirmaDateUtilities.GetYearUsedToRepresentPreReportingYears(), sector.GetPreReportingYearExpenditures()}})
                    };
                }
                else
                {
                    calendarYearRange = new List<int> {calendarYear.Value};
                    fundingSourceCalendarYearExpenditures =
                        FundingSourceCalendarYearExpenditure.CreateFromFundingSourcesAndCalendarYears(
                            new List<IFundingSourceExpenditure>(projectFundingSourceExpenditures.Where(x => x.CalendarYear == calendarYear.Value)),
                            calendarYearRange);
                }
            }
            var calendarYears = calendarYearRange.ToDictionary(x => x,
                year =>
                {
                    if (year < FirmaDateUtilities.GetMinimumYearForReportingExpenditures())
                    {
                        return String.Format("{0} - {1}", FirmaDateUtilities.MinimumYear, FirmaDateUtilities.GetYearUsedToRepresentPreReportingYears());
                    }
                    return year.ToString(CultureInfo.InvariantCulture);
                });
            var excelUrl = SitkaRoute<ResultsController>.BuildUrlFromExpression(x => x.SpendingBySectorByOrganizationExcelDownload(sectorID, calendarYear));
            var viewData = new SpendingBySectorByOrganizationViewData(fundingSourceCalendarYearExpenditures, calendarYears, excelUrl);
            return viewData;
        }

        [SpendingBySectorByOrganizationViewFeature]
        public ExcelResult SpendingBySectorByOrganizationExcelDownload(int sectorID, int? calendarYear)
        {
            var sector = Sector.AllLookupDictionary[sectorID];
            var viewData = GetSpendingBySectorByOrganizationViewData(sectorID, calendarYear);

            var excelSpec = new FundingSourceCalendarYearExpenditureExcelSpec(viewData.CalendarYears);
            var wsFundingSources = ExcelWorkbookSheetDescriptorFactory.MakeWorksheet("Funding Sources",
                excelSpec,
                viewData.FundingSourceCalendarYearExpenditures.OrderBy(x => x.OrganizationName).ThenBy(x => x.FundingSourceName).ToList());
            var workSheets = new List<IExcelWorkbookSheetDescriptor> {wsFundingSources};

            var wbm = new ExcelWorkbookMaker(workSheets);
            var excelWorkbook = wbm.ToXLWorkbook();
            return new ExcelResult(excelWorkbook, String.Format("Funding Source Spending for {0}", sector.SectorDisplayName));
        }

        [ResultsByProgramViewFeature]
        public ViewResult ResultsByProgram(int? programID)
        {
            var focusAreas = HttpRequestStorage.DatabaseEntities.FocusAreas.OrderBy(x => x.FocusAreaNumber).ToList();
            var selectedProgram = programID.HasValue ? HttpRequestStorage.DatabaseEntities.Programs.GetProgram(programID.Value) : focusAreas.First().Programs.First();
            var firmaPage = FirmaPage.GetFirmaPageByPageType(FirmaPageType.ResultsByProgram);
            var viewData = new ResultsByProgramViewData(CurrentPerson, firmaPage, focusAreas, selectedProgram);
            return RazorView<ResultsByProgram, ResultsByProgramViewData>(viewData);
        }

        [SpendingByPerformanceMeasureByProjectViewFeature]
        public ViewResult SpendingByPerformanceMeasureByProject(int? performanceMeasureID)
        {
            var firmaPage = FirmaPage.GetFirmaPageByPageType(FirmaPageType.SpendingByPerformanceMeasureByProject);
            var performanceMeasures = HttpRequestStorage.DatabaseEntities.PerformanceMeasures.ToList();
            var selectedPerformanceMeasure = performanceMeasureID.HasValue ? performanceMeasures.Single(x => x.PerformanceMeasureID == performanceMeasureID) : performanceMeasures.First();
            var accomplishmentsChartViewData = new IndicatorChartViewData(selectedPerformanceMeasure.Indicator, false, ChartViewMode.Small, null);

            var viewData = new SpendingByPerformanceMeasureByProjectViewData(CurrentPerson, firmaPage, performanceMeasures, selectedPerformanceMeasure, accomplishmentsChartViewData);
            var viewModel = new SpendingByPerformanceMeasureByProjectViewModel();
            return RazorView<SpendingByPerformanceMeasureByProject, SpendingByPerformanceMeasureByProjectViewData, SpendingByPerformanceMeasureByProjectViewModel>(viewData, viewModel);
        }

        [SpendingByPerformanceMeasureByProjectViewFeature]
        public GridJsonNetJObjectResult<PerformanceMeasureSubcategoriesTotalReportedValue> SpendingByPerformanceMeasureByProjectGridJsonData(PerformanceMeasurePrimaryKey performanceMeasurePrimaryKey)
        {
            SpendingByPerformanceMeasureByProjectGridSpec gridSpec;
            var performanceMeasure = performanceMeasurePrimaryKey.EntityObject;
            var performanceMeasureSubcategoriesTotalReportedValues = GetSpendingByPerformanceMeasureByProjectAndGridSpec(out gridSpec, performanceMeasure);
            var gridJsonNetJObjectResult = new GridJsonNetJObjectResult<PerformanceMeasureSubcategoriesTotalReportedValue>(performanceMeasureSubcategoriesTotalReportedValues, gridSpec);
            return gridJsonNetJObjectResult;
        }

        private static List<PerformanceMeasureSubcategoriesTotalReportedValue> GetSpendingByPerformanceMeasureByProjectAndGridSpec(out SpendingByPerformanceMeasureByProjectGridSpec gridSpec,
            PerformanceMeasure performanceMeasure)
        {
            gridSpec = new SpendingByPerformanceMeasureByProjectGridSpec(performanceMeasure);
            return performanceMeasure.SubcategoriesTotalReportedValues();
        }



        [HttpGet]
        [AnonymousUnclassifiedFeature]
        public PartialViewResult InvestmentByFundingSectorGoogleChartPopup(int? selectedCalendarYear)
        {
            var projectFundingSourceExpenditures = GetProjectExpendituresByFundingSector(null, null);
            var fundingSectorExpenditures = GetFundingSectorExpendituresForInvestmentByFundingSectorReport(selectedCalendarYear, projectFundingSourceExpenditures);

            var googleChart = GetInvestmentByFundingSectorGoogleChart(fundingSectorExpenditures, selectedCalendarYear);
            var viewData = new GoogleChartPopupViewData(googleChart);
            return RazorPartialView<GoogleChartPopup, GoogleChartPopupViewData>(viewData);
        }

        public static GoogleChartJson GetInvestmentByFundingSectorGoogleChart(List<FundingSectorExpenditure> fundingSectorExpenditures, int? selectedCalendarYear)
        {
            const int chartSize = 350;
            var chartName = string.Format("InvestmentByFundingSector{0}PieChart", selectedCalendarYear);

            var googleChartDataTable = GetInvestmentByFundingSectorGoogleChartDataTable(fundingSectorExpenditures);
            var googleChartTitle = "Investment by Funding Sector for: " + YearDisplayName(selectedCalendarYear);
            var googleChartConfiguration = new GooglePieChartConfiguration(googleChartTitle, MeasurementUnitType.Dollars, chartSize, chartSize) {Background = {Color = "transparent"}};

            var enumerable = fundingSectorExpenditures.Select(x => x.LegendColor).Select((value, index) => new {value, index});
            var googlePieChartSlices = enumerable.ToDictionary(x => x.index, x => new GooglePieChartSlice(){Color = x.value});
            googleChartConfiguration.Slices = googlePieChartSlices;
            
            var chartPopupUrl = SitkaRoute<ResultsController>.BuildUrlFromExpression(x => x.InvestmentByFundingSectorGoogleChartPopup(selectedCalendarYear));
            var googleChart = new GoogleChartJson(string.Empty,
                chartName,
                googleChartConfiguration,
                GoogleChartType.PieChart,
                googleChartDataTable,
                chartPopupUrl,
                string.Empty,
                string.Empty);
            return googleChart;
        }

        public static GoogleChartDataTable GetInvestmentByFundingSectorGoogleChartDataTable(List<FundingSectorExpenditure> fundingSectorExpenditures)
        {
            var googleChartColumns = new List<GoogleChartColumn> { new GoogleChartColumn("Funding Sector", GoogleChartColumnDataType.String, GoogleChartType.PieChart), new GoogleChartColumn("Expenditures", GoogleChartColumnDataType.Number, GoogleChartType.PieChart) };

            var googleChartRowCs = fundingSectorExpenditures.Select(x =>
            {
                var sectorRowV = new GoogleChartRowV(x.SectorDisplayName);
                var formattedValue = GoogleChartJson.GetFormattedValue((double) x.CalendarYearExpenditureAmount, MeasurementUnitType.Dollars);
                var expenditureRowV = new GoogleChartRowV(x.CalendarYearExpenditureAmount, formattedValue);
                return new GoogleChartRowC(new List<GoogleChartRowV> {sectorRowV, expenditureRowV});
            }).ToList();

            var googleChartDataTable = new GoogleChartDataTable(googleChartColumns, googleChartRowCs);
            return googleChartDataTable;
        }
    }
}