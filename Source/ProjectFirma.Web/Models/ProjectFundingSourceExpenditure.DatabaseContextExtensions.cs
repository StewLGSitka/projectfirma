﻿/*-----------------------------------------------------------------------
<copyright file="ProjectFundingSourceExpenditure.DatabaseContextExtensions.cs" company="Tahoe Regional Planning Agency">
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
using ProjectFirma.Web.Common;
using ProjectFirma.Web.Views.Shared;

namespace ProjectFirma.Web.Models
{
    public static partial class DatabaseContextExtensions
    {
        public static IQueryable<ProjectFundingSourceExpenditure> GetExpendituresFromMininumYearForReportingOnward(this IQueryable<ProjectFundingSourceExpenditure> projectFundingSourceExpenditures)
        {
            var minimumYearForReportingExpenditures = FirmaDateUtilities.GetMinimumYearForReportingExpenditures();
            return projectFundingSourceExpenditures.Where(x => x.CalendarYear >= minimumYearForReportingExpenditures);
        }

        public static List<int> CalculateCalendarYearRangeForExpenditures(this IList<ProjectFundingSourceExpenditure> projectFundingSourceExpenditures, Project project)
        {
            var existingYears = projectFundingSourceExpenditures.Select(x => x.CalendarYear).ToList();
            return FirmaDateUtilities.CalculateCalendarYearRangeForExpendituresAccountingForExistingYears(existingYears, project, DateTime.Today.Year);
        }

        public static List<int> CalculateCalendarYearRangeForExpenditures(this IEnumerable<ProjectFundingSourceExpenditure> projectFundingSourceExpenditures, FundingSource fundingSource)
        {
            var existingYears = projectFundingSourceExpenditures.Select(x => x.CalendarYear).ToList();
            return FirmaDateUtilities.CalculateCalendarYearRangeAccountingForExistingYears(existingYears,
                fundingSource.ProjectsWhereYouAreTheFundingSourceMinCalendarYear,
                fundingSource.ProjectsWhereYouAreTheFundingSourceMaxCalendarYear,
                DateTime.Today.Year,
                FirmaDateUtilities.MinimumYear,
                null);
        }

        public static GoogleChartJson ToGoogleChart(this IEnumerable<ProjectFundingSourceExpenditure> projectFundingSourceExpenditures,
            Func<ProjectFundingSourceExpenditure, string> filterFunction,
            List<string> filterValues,
            Func<ProjectFundingSourceExpenditure, IComparable> sortFunction,
            string chartName,
            string chartTitle, string chartPopupUrl)
        {
            var projectFundingSourceExpenditureList = projectFundingSourceExpenditures.ToList();
            if (!projectFundingSourceExpenditureList.Any())
            {
                return null;
            }
            var beginCalendarYear = projectFundingSourceExpenditureList.Min(x => x.CalendarYear);
            var endCalendarYear = projectFundingSourceExpenditureList.Max(x => x.CalendarYear);
            var rangeOfYears = FirmaDateUtilities.GetRangeOfYears(beginCalendarYear, endCalendarYear);

            return projectFundingSourceExpenditureList.ToGoogleChart(filterFunction, filterValues, sortFunction, rangeOfYears, chartName, chartTitle, chartPopupUrl);
        }

        //TODO: The GetFullCategoryYearDictionary and GetGoogleChartDataTable functions are probably fine in this Extension class, but the ToGoogleChart functions are more about display and probably could be in a better location
        public static GoogleChartJson ToGoogleChart(this IEnumerable<ProjectFundingSourceExpenditure> projectFundingSourceExpenditures,
            Func<ProjectFundingSourceExpenditure, string> filterFunction,
            List<string> filterValues,
            Func<ProjectFundingSourceExpenditure, IComparable> sortFunction,
            List<int> rangeOfYears,
            string chartName,
            string chartTitle,
            string chartPopupUrl)
        {
            var chartType = GoogleChartType.AreaChart;

            var fullCategoryYearDictionary = GetFullCategoryYearDictionary(projectFundingSourceExpenditures, filterFunction, filterValues, sortFunction, rangeOfYears);
            var googleChartDataTable = GetGoogleChartDataTable(fullCategoryYearDictionary, rangeOfYears);
            
            var chartConfiguration = new GoogleChartConfiguration(chartTitle, $"{FieldDefinition.ReportedExpenditure.GetFieldDefinitionLabelPluralized()} ($)", MeasurementUnitType.Dollars);
            var googleChart = new GoogleChartJson(string.Empty, chartName, chartConfiguration, chartType, googleChartDataTable, chartPopupUrl, string.Empty, string.Empty);
            return googleChart;
        }

        public static Dictionary<string, Dictionary<int, decimal>> GetFullCategoryYearDictionary(this IEnumerable<ProjectFundingSourceExpenditure> projectFundingSourceExpenditures,
            Func<ProjectFundingSourceExpenditure, string> filterFunction,
            List<string> filterValues,
            Func<ProjectFundingSourceExpenditure, IComparable> sortFunction,
            List<int> rangeOfYears)
        {
            var fullCategoryYearDictionary = filterValues.ToDictionary(x => x, x => rangeOfYears.ToDictionary(y => y, y => 0m));
            projectFundingSourceExpenditures.OrderBy(sortFunction.Invoke).Where(x => rangeOfYears.Contains(x.CalendarYear)).GroupBy(x => x.CalendarYear).ToList().ForEach(x =>
            {
                filterValues.ForEach(s =>
                {
                    var expenditures = x.Where(y => filterFunction.Invoke(y) == s).ToList();
                    fullCategoryYearDictionary[s][x.Key] = expenditures.Sum(y => y.ExpenditureAmount);
                });
            });
            return fullCategoryYearDictionary;
        }

        private static GoogleChartDataTable GetGoogleChartDataTable(Dictionary<string, Dictionary<int, decimal>> fullCategoryYearDictionary, List<int> rangeOfYears)
        {
            var googleChartRowCs = new List<GoogleChartRowC>();
            var sortedYearCategoryDictionary =
                fullCategoryYearDictionary.OrderBy(x => x.Value.Sum(y => y.Value)).ThenBy(x => x.Key).ToList();

            foreach (var year in rangeOfYears.OrderBy(x => x))
            {
                var googleChartRowVs = new List<GoogleChartRowV> { new GoogleChartRowV(year, year.ToString()) };
                googleChartRowVs.AddRange(
                    sortedYearCategoryDictionary
                        .Select(x => x.Key)
                        .Select(category => fullCategoryYearDictionary[category][year])
                        .Select(value => new GoogleChartRowV(value, GoogleChartJson.GetFormattedValue((double) value, MeasurementUnitType.Dollars))));

                googleChartRowCs.Add(new GoogleChartRowC(googleChartRowVs));
            }

            var googleChartColumns = new List<GoogleChartColumn> { new GoogleChartColumn("Year", "Year", "string", GoogleChartType.LineChart, GoogleChartAxisType.Primary) };
            googleChartColumns.AddRange(
                sortedYearCategoryDictionary.Select(
                        x => new GoogleChartColumn(x.Key, x.Key, "number", GoogleChartType.LineChart, GoogleChartAxisType.Primary)));

            return new GoogleChartDataTable(googleChartColumns, googleChartRowCs);
        }

    }
}
