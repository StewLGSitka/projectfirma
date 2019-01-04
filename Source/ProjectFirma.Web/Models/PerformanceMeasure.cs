﻿/*-----------------------------------------------------------------------
<copyright file="PerformanceMeasure.cs" company="Tahoe Regional Planning Agency and Sitka Technology Group">
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
using System;
using System.Collections.Generic;
using System.Linq;
using ProjectFirma.Web.Common;
using ProjectFirma.Web.Views.Project;
using ProjectFirma.Web.Views.Shared;

namespace ProjectFirma.Web.Models
{
    public partial class PerformanceMeasure : IAuditableEntity, IHaveASortOrder
    {
        public List<Project> GetAssociatedProjectsWithReportedValues(Person person)
        {
            return PerformanceMeasureActuals.Select(ptc => ptc.Project).ToList().GetActiveProjectsAndProposals(person.CanViewProposals);
        }

        public List<Project> GetAssociatedProjectsWithExpectedValues(Person person)
        {
            return PerformanceMeasureExpecteds.Select(ptc => ptc.Project).ToList().GetActiveProjectsAndProposals(person.CanViewProposals);
        }     

        public int ReportedProjectsCount(Person currentPerson)
        {
           return GetAssociatedProjectsWithReportedValues(currentPerson).Distinct().Count(); 
        }

        public ITaxonomyTier GetPrimaryTaxonomyTier()
        {
            var taxonomyBranchPerformanceMeasureGroupedByLevel = GetTaxonomyTiers();
            return taxonomyBranchPerformanceMeasureGroupedByLevel
                .Where(group => group.Any(x => x.IsPrimaryTaxonomyLeaf))
                .Select(group => group.Key).FirstOrDefault();
        }

        public IEnumerable<IGrouping<ITaxonomyTier, TaxonomyLeafPerformanceMeasure>> GetTaxonomyTiers()
        {
            Func<TaxonomyLeafPerformanceMeasure, ITaxonomyTier> groupingFunc;
            switch (MultiTenantHelpers.GetAssociatePerformanceMeasureTaxonomyLevel().ToEnum)
            {
                case TaxonomyLevelEnum.Trunk:
                    groupingFunc = x => x.TaxonomyLeaf.TaxonomyBranch.TaxonomyTrunk;
                    break;
                case TaxonomyLevelEnum.Branch:
                    groupingFunc = x => x.TaxonomyLeaf.TaxonomyBranch;
                    break;
                case TaxonomyLevelEnum.Leaf:
                    groupingFunc = x => x.TaxonomyLeaf;
                    break;
                default:
                    throw new ArgumentException();
            }

            var taxonomyBranchPerformanceMeasureGroupedByLevel = TaxonomyLeafPerformanceMeasures.GroupBy(groupingFunc);
            return taxonomyBranchPerformanceMeasureGroupedByLevel;
        }

        public List<PerformanceMeasureSubcategory> GetPerformanceMeasureSubcategories()
        {
            return PerformanceMeasureSubcategories.Any()
                ? PerformanceMeasureSubcategories.Select(x => x).OrderBy(x => x.PerformanceMeasureSubcategoryDisplayName).ToList()
                : new List<PerformanceMeasureSubcategory>();
        }

        public IEnumerable<PerformanceMeasureSubcategoryOption> GetPerformanceMeasureSubcategoryOptions()
        {
            return GetPerformanceMeasureSubcategories()
                .SelectMany(x => x.PerformanceMeasureSubcategoryOptions).OrderBy(x=>x.PerformanceMeasureSubcategory.PerformanceMeasureSubcategoryDisplayName).ThenBy(x=>x.SortOrder);
        }

        public List<PerformanceMeasureReportedValue> GetReportedPerformanceMeasureValues(Person currentPerson)
        {
            var projects = GetAssociatedProjectsWithReportedValues(currentPerson);
            return GetReportedPerformanceMeasureValues(projects);
        }

        public List<PerformanceMeasureReportedValue> GetReportedPerformanceMeasureValues(Project project)
        {
            return GetReportedPerformanceMeasureValues(new List<Project> {project});
        }
       
        public List<PerformanceMeasureReportedValue> GetReportedPerformanceMeasureValues(List<Project> projects)
        {
            return PerformanceMeasureDataSourceType.GetReportedPerformanceMeasureValues(this, projects);
        }

        public decimal? TotalExpenditure(Person currentPerson)
        {
            return SubcategoriesTotalReportedValues(currentPerson).Sum(x => x.CalculateWeightedTotalExpenditure());
        }

        public decimal? TotalExpenditurePerPerformanceMeasureUnit(Person currentPerson)
        {
            var totalReportedValues = SubcategoriesTotalReportedValues(currentPerson).Where(x => x.CalculateWeightedTotalExpenditure() > 0).Sum(x => x.TotalReportedValue ?? 0);
            if (Math.Abs(totalReportedValues) < Double.Epsilon)
            {
                return null;
            }
            return TotalExpenditure(currentPerson) / (decimal)totalReportedValues;
        }

        public List<PerformanceMeasureSubcategoriesTotalReportedValue> SubcategoriesTotalReportedValues(Person currentPerson)
        {
            var groupByProjectAndSubcategory =
                GetReportedPerformanceMeasureValues(currentPerson)
                    .Where(x => FirmaDateUtilities.DateIsInReportingRange(x.CalendarYear))
                    .GroupBy(x => new {x.Project, x.PerformanceMeasureSubcategoriesAsString})
                    .ToList();

            return
                groupByProjectAndSubcategory.Select(
                    reportedValuesGroup =>
                        new PerformanceMeasureSubcategoriesTotalReportedValue(reportedValuesGroup.Key.Project,
                            reportedValuesGroup.First().PerformanceMeasureSubcategoryOptions,
                            this,
                            reportedValuesGroup.Sum(x => x.ReportedValue))).ToList();
        }

        public string AuditDescriptionString => PerformanceMeasureDisplayName;

        public bool HasRealSubcategories
        {
            get
            {
                return PerformanceMeasureSubcategories.Any(x => x.PerformanceMeasureSubcategoryOptions.Count > 1);
            }
        }

        public List<PerformanceMeasureSubcategory> GetSubcategoriesForPerformanceMeasureChart()
        {
            return PerformanceMeasureSubcategories.Where(x => x.PerformanceMeasureSubcategoryOptions.Count > 1 && x.ShowOnChart).ToList();
        }

        public int GetRealSubcategoryCount()
        {
            return HasRealSubcategories ? PerformanceMeasureSubcategories.Count : 0;
        }

        public List<GoogleChartJson> GetGoogleChartJsonDictionary(List<Project> projects)
        {
            var reportedValues = GetProjectPerformanceMeasureSubcategoryOptionReportedValues(this, projects);
            var googleChartJsons = PerformanceMeasureSubcategory.MakeGoogleChartJsons(this, reportedValues);
            return googleChartJsons;
        }

        public List<ProjectPerformanceMeasureReportingPeriodValue> GetProjectPerformanceMeasureSubcategoryOptionReportedValues(PerformanceMeasure performanceMeasure, List<Project> projects)
        {
            var performanceMeasureValues = performanceMeasure.PerformanceMeasureDataSourceType.GetReportedPerformanceMeasureValues(
                    performanceMeasure,
                    projects);

            var performanceMeasureActualsFiltered =
                projects?.Any() == true ? performanceMeasureValues.Where(pmav => projects.Contains(pmav.Project)).ToList() : performanceMeasureValues;

            var groupByProjectAndSubcategory = performanceMeasureActualsFiltered.GroupBy(pirv => new { pirv.Project, pirv.PerformanceMeasureSubcategoriesAsString, pirv.CalendarYear }).OrderBy(x => x.Key.PerformanceMeasureSubcategoriesAsString).ToList();

            var projectPerformanceMeasureReportingPeriodValues = groupByProjectAndSubcategory.Select(reportedValuesGroup =>
            {
                var project = reportedValuesGroup.Key.Project;
                var performanceMeasureReportingPeriodSubcategoryOptionReportedValues = reportedValuesGroup.First().PerformanceMeasureSubcategoryOptions.OrderBy(y =>
                    y.PerformanceMeasureSubcategory.PerformanceMeasureSubcategoryDisplayName).Select(
                    (y, index) =>
                    {
                        if (y.PerformanceMeasureSubcategoryOption != null)
                        {
                            return new
                                PerformanceMeasureReportingPeriodSubcategoryOptionReportedValue(
                                    reportedValuesGroup.Key.CalendarYear, y.PerformanceMeasureSubcategoryOption,
                                    reportedValuesGroup.Sum(x => x.ReportedValue ?? 0));
                        }
                        else
                        {
                            return new
                                PerformanceMeasureReportingPeriodSubcategoryOptionReportedValue(
                                    reportedValuesGroup.Key.CalendarYear,
                                    reportedValuesGroup.Sum(x => x.ReportedValue ?? 0), y.PerformanceMeasureSubcategory, y.PerformanceMeasureSubcategoryOptionName);
                        }
                    }).ToList();

                return new ProjectPerformanceMeasureReportingPeriodValue(project, performanceMeasureReportingPeriodSubcategoryOptionReportedValues);
            }).ToList();
           
            return projectPerformanceMeasureReportingPeriodValues.OrderByDescending(pma => pma.CalendarYear).ThenBy(pma => pma.Project.ProjectName).ToList();
        }

        public string DisplayName => PerformanceMeasureDisplayName;
        public int? SortOrder { get => PerformanceMeasureSortOrder; set => PerformanceMeasureSortOrder = value; }
        public int ID => PerformanceMeasureID;

        public double? TotalReportedValueWithNonZeroExpenditures(Person currentPerson)
        {
            return SubcategoriesTotalReportedValues(currentPerson).Where(x => x.CalculateWeightedTotalExpenditure() > 0).Sum(x => x.TotalReportedValue);
        }

        public bool IsExternallySourcedPerformanceMeasure()
        {
            return !string.IsNullOrWhiteSpace(ExternalDataSourceUrl);
        }
    }
}
