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
using LtInfo.Common.Models;

namespace ProjectFirma.Web.Models
{
    public partial class PerformanceMeasure : IAuditableEntity
    {
        public int ExpectedProjectsCount
        {
            get { return PerformanceMeasureExpecteds.ToList().Select(pepm => pepm.ProjectID).Distinct().Count(); }
        }

        public int ReportedProjectsCount
        {
            get { return PerformanceMeasureActuals.ToList().Select(pepm => pepm.ProjectID).Distinct().Count(); }
        }

        public TaxonomyBranch PrimaryTaxonomyBranch
        {
            get
            {
                var taxonomyBranchPerformanceMeasure = TaxonomyBranchPerformanceMeasures.SingleOrDefault(ppm => ppm.IsPrimaryTaxonomyBranch);
                return taxonomyBranchPerformanceMeasure?.TaxonomyBranch;
            }
        }

        public List<PerformanceMeasureSubcategory> GetPerformanceMeasureSubcategories()
        {
            return PerformanceMeasureSubcategories.Any()
                ? PerformanceMeasureSubcategories.Select(x => x).OrderBy(x => x.PerformanceMeasureSubcategoryDisplayName).ToList()
                : new List<PerformanceMeasureSubcategory>();
        }

        public List<PerformanceMeasureReportedValue> GetReportedPerformanceMeasureValues()
        {
            return GetReportedPerformanceMeasureValues(new List<int>());
        }

        public List<PerformanceMeasureReportedValue> GetReportedPerformanceMeasureValues(Project project)
        {
            return GetReportedPerformanceMeasureValues(new List<int> {project.ProjectID});
        }
       
        public List<PerformanceMeasureReportedValue> GetReportedPerformanceMeasureValues(List<int> projectIDs)
        {
            List<PerformanceMeasureActual> performanceMeasureActuals;
            if (projectIDs == null || !projectIDs.Any())
            {
                performanceMeasureActuals = HttpRequestStorage.DatabaseEntities.PerformanceMeasureActuals.Where(pmav => pmav.PerformanceMeasureID == PerformanceMeasureID).ToList();
            }
            else
            {
                performanceMeasureActuals =
                    HttpRequestStorage.DatabaseEntities.PerformanceMeasureActuals.Where(
                        pmav => pmav.PerformanceMeasureID == PerformanceMeasureID && projectIDs.Contains(pmav.ProjectID)).ToList();
            }
            var performanceMeasureReportedValues = PerformanceMeasureReportedValue.MakeFromList(performanceMeasureActuals);
            return performanceMeasureReportedValues.OrderByDescending(pma => pma.CalendarYear).ThenBy(pma => pma.ProjectName).ToList();
        }

        public Dictionary<TaxonomyBranch, bool> GetTaxonomyBranches()
        {
            return TaxonomyBranchPerformanceMeasures.Any()
                ? TaxonomyBranchPerformanceMeasures.ToDictionary(x => x.TaxonomyBranch, x => x.IsPrimaryTaxonomyBranch, new HavePrimaryKeyComparer<TaxonomyBranch>())
                : new Dictionary<TaxonomyBranch, bool>();
        }

        public decimal? TotalExpenditure()
        {
            return SubcategoriesTotalReportedValues().Sum(x => x.CalculateWeightedTotalExpenditure());
        }

        public decimal? TotalExpenditurePerPerformanceMeasureUnit()
        {
            var totalReportedValues = SubcategoriesTotalReportedValues().Where(x => x.CalculateWeightedTotalExpenditure() > 0).Sum(x => x.TotalReportedValue ?? 0);
            if (Math.Abs(totalReportedValues) < Double.Epsilon)
            {
                return null;
            }
            return TotalExpenditure()/(decimal) totalReportedValues;
        }

        public decimal? AnnualExpenditure(int calendarYear)
        {
            return GetReportedPerformanceMeasureValues().Where(x => x.CalendarYear == calendarYear).Sum(x => x.CalculateWeightedAnnualExpenditure());
        }

        public double? TotalReportedValueWithNonZeroExpenditures()
        {
            return SubcategoriesTotalReportedValues().Where(x => x.CalculateWeightedTotalExpenditure() > 0).Sum(x => x.TotalReportedValue);
        }

        public List<PerformanceMeasureSubcategoriesTotalReportedValue> SubcategoriesTotalReportedValues()
        {
            var groupByProjectAndSubcategory =
                GetReportedPerformanceMeasureValues()
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

        public List<GoogleChartJson> GetGoogleChartJsonDictionary(List<int> projectIDs)
        {
            var reportedValues = GetProjectPerformanceMeasureSubcategoryOptionReportedValues(this, projectIDs);
            var googleChartJsons = PerformanceMeasureSubcategory.MakeGoogleChartJsons(this, reportedValues);
            return googleChartJsons;
        }

        public List<ProjectPerformanceMeasureReportingPeriodValue> GetProjectPerformanceMeasureSubcategoryOptionReportedValues(PerformanceMeasure performanceMeasure, List<int> projectIDs)
        {
            var performanceMeasureActuals = HttpRequestStorage.DatabaseEntities.PerformanceMeasureActuals.Where(pmav => pmav.PerformanceMeasureID == performanceMeasure.PerformanceMeasureID).ToList();
            var performanceMeasureActualsFiltered =
                projectIDs?.Any() == true ? performanceMeasureActuals.Where(pmav => projectIDs.Contains(pmav.ProjectID)).ToList() : performanceMeasureActuals;

            var groupByProjectAndSubcategory = performanceMeasureActualsFiltered.GroupBy(pirv => new { pirv.Project, pirv.PerformanceMeasureSubcategoriesAsString, pirv.CalendarYear }).OrderBy(x => x.Key.PerformanceMeasureSubcategoriesAsString).ToList();

            var projectPerformanceMeasureReportingPeriodValues = groupByProjectAndSubcategory.Select(reportedValuesGroup =>
            {
                var project = reportedValuesGroup.Key.Project;
                var performanceMeasureReportingPeriodSubcategoryOptionReportedValues = reportedValuesGroup.First().PerformanceMeasureActualSubcategoryOptions.OrderBy(y =>
                    y.PerformanceMeasureSubcategory.PerformanceMeasureSubcategoryDisplayName).Select(
                    (y, index) =>
                        new PerformanceMeasureReportingPeriodSubcategoryOptionReportedValue(
                            reportedValuesGroup.Key.CalendarYear, y.PerformanceMeasureSubcategoryOption,
                            reportedValuesGroup.Sum(x => x.ReportedValue ?? 0))).ToList();

                return new ProjectPerformanceMeasureReportingPeriodValue(project, performanceMeasureReportingPeriodSubcategoryOptionReportedValues);
            }).ToList();
           
            return projectPerformanceMeasureReportingPeriodValues.OrderByDescending(pma => pma.CalendarYear).ThenBy(pma => pma.Project.ProjectName).ToList();
        }

        public void DeletePerformanceMeasureAndAllRelatedData()
        {
            PerformanceMeasureActualSubcategoryOptions.ToList().ForEach(x => x.DeletePerformanceMeasureActualSubcategoryOption());
            PerformanceMeasureActuals.ToList().ForEach(x => x.DeletePerformanceMeasureActual());
            PerformanceMeasureActualSubcategoryOptionUpdates.ToList().ForEach(x => x.DeletePerformanceMeasureActualSubcategoryOptionUpdate());
            PerformanceMeasureActualUpdates.ToList().ForEach(x => x.DeletePerformanceMeasureActualUpdate());
            PerformanceMeasureExpectedSubcategoryOptions.ToList().ForEach(x => x.DeletePerformanceMeasureExpectedSubcategoryOption());
            PerformanceMeasureExpecteds.ToList().ForEach(x => x.DeletePerformanceMeasureExpected());
            PerformanceMeasureSubcategories.SelectMany(x => x.PerformanceMeasureSubcategoryOptions).ToList().ForEach(x => x.DeletePerformanceMeasureSubcategoryOption());
            PerformanceMeasureSubcategories.ToList().ForEach(x => x.DeletePerformanceMeasureSubcategory());
            PerformanceMeasureMonitoringPrograms.ToList().ForEach(x => x.DeletePerformanceMeasureMonitoringProgram());
            ClassificationPerformanceMeasures.ToList().ForEach(x => x.DeleteClassificationPerformanceMeasure());
            // TODO: We might want to consider removing the FKs to SnapshotPerformanceMeasure and SnapshotPerformanceMeasureSubcategoryOption since it's purpose it to track historical data
            SnapshotPerformanceMeasureSubcategoryOptions.ToList().ForEach(x => x.DeleteSnapshotPerformanceMeasureSubcategoryOption());
            SnapshotPerformanceMeasures.ToList().ForEach(x => x.DeleteSnapshotPerformanceMeasure());
            TaxonomyBranchPerformanceMeasures.ToList().ForEach(x => x.DeleteTaxonomyBranchPerformanceMeasure());
            PerformanceMeasureNotes.ToList().ForEach(x => x.DeletePerformanceMeasureNote());
            this.DeletePerformanceMeasure();
        }
    }
}
