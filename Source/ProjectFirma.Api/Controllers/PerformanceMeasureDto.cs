﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ProjectFirmaModels.Models;

namespace ProjectFirma.Api.Controllers
{
    public class PerformanceMeasureDto
    {
        public PerformanceMeasureDto(PerformanceMeasure performanceMeasure)
        {
            PerformanceMeasureID = performanceMeasure.PerformanceMeasureID;
            PerformanceMeasureDisplayName = performanceMeasure.PerformanceMeasureDisplayName;
            PerformanceMeasureDefinition = performanceMeasure.PerformanceMeasureDefinition;
            CriticalDefinitions = performanceMeasure.CriticalDefinitions;
            ProjectReporting = performanceMeasure.ProjectReporting;
            PerformanceMeasureTypeName = performanceMeasure.PerformanceMeasureType.PerformanceMeasureTypeDisplayName;
            PerformanceMeasureDataSourceTypeName = performanceMeasure.PerformanceMeasureDataSourceType.PerformanceMeasureDataSourceTypeDisplayName;
            MeasurementUnitTypeName = performanceMeasure.MeasurementUnitType.MeasurementUnitTypeDisplayName;
            PerformanceMeasureSubcategories = performanceMeasure.PerformanceMeasureSubcategories
                .Select(x => new PerformanceMeasureSubcategoryDto(x)).ToList();

        }

        public PerformanceMeasureDto()
        {
        }

        public int PerformanceMeasureID { get; set; }
        public string CriticalDefinitions { get; set; }
        public string ProjectReporting { get; set; }

        [DisplayName("Performance Measure Display Name")]
        public string PerformanceMeasureDisplayName { get; set; }
        public string MeasurementUnitTypeName { get; set; }
        public string PerformanceMeasureTypeName { get; set; }
        public string PerformanceMeasureDefinition { get; set; }
        public string DataSourceText { get; set; }
        public string ExternalDataSourceUrl { get; set; }
        public string ChartCaption { get; set; }
        public bool SwapChartAxes { get; set; }
        public bool CanCalculateTotal { get; set; }
        public int? PerformanceMeasureSortOrder { get; set; }
        public bool IsAggregatable { get; set; }
        public string PerformanceMeasureDataSourceTypeName { get; set; }

        public List<PerformanceMeasureSubcategoryDto> PerformanceMeasureSubcategories { get; set; }

    }
}