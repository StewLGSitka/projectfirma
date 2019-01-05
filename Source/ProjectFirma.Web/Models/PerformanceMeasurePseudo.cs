using System.Collections.Generic;
using System.Linq;
using System.Web;
using LtInfo.Common;
using ProjectFirma.Web.Common;

namespace ProjectFirma.Web.Models
{
    public class PerformanceMeasurePseudo
    {
        public PerformanceMeasurePseudo(PerformanceMeasure performanceMeasure)
        {
            this.IsExternallySourcedPerformanceMeasure = false;

            this.PerformanceMeasure = performanceMeasure;
   
            //populate pseudo class with performanceMeasure
            this.TenantID = performanceMeasure.TenantID;
            this.CriticalDefinitionsHtmlString = performanceMeasure.CriticalDefinitionsHtmlString;
            this.ProjectReportingHtmlString = performanceMeasure.ProjectReportingHtmlString;
            this.PerformanceMeasureDisplayName = performanceMeasure.DisplayName;
            this.MeasurementUnitTypeName = MeasurementUnitType.AllLookupDictionary[performanceMeasure.MeasurementUnitTypeID].MeasurementUnitTypeDisplayName;
            this.PerformanceMeasureTypeName = PerformanceMeasureType.AllLookupDictionary[performanceMeasure.PerformanceMeasureTypeID].PerformanceMeasureTypeDisplayName;
            this.PerformanceMeasureDefinition = performanceMeasure.PerformanceMeasureDefinition;
            this.ExternalDataSourceUrl = performanceMeasure.ExternalDataSourceUrl;
            this.SwapChartAxes = performanceMeasure.SwapChartAxes;
            this.CanCalculateTotal = performanceMeasure.CanCalculateTotal;
            this.IsAggregatable = performanceMeasure.IsAggregatable;
            this.PerformanceMeasureDataSourceTypeName = PerformanceMeasureDataSourceType.AllLookupDictionary[performanceMeasure.PerformanceMeasureDataSourceTypeID].PerformanceMeasureDataSourceTypeDisplayName;

        }

        public PerformanceMeasurePseudo(PerformanceMeasure performanceMeasure, PsInfoProgressMeasure psInfoProgressMeasure)
        {
            this.ExternalDataSourceUrl = performanceMeasure.ExternalDataSourceUrl;
            this.TenantID = performanceMeasure.TenantID;
            this.PerformanceMeasure = performanceMeasure;
            //query external DS for field information and populate properties
            this.IsExternallySourcedPerformanceMeasure = true;

            
            this.CriticalDefinitionsHtmlString = psInfoProgressMeasure.DefinitionsHtmlString;
            this.ProjectReportingHtmlString = psInfoProgressMeasure.ReportingGuidanceHtmlString;
            this.PerformanceMeasureDisplayName = psInfoProgressMeasure.ProgressMeasureName;
            this.MeasurementUnitTypeName = psInfoProgressMeasure.MeasurementUnitTypeName;
            this.PerformanceMeasureTypeName = psInfoProgressMeasure.ProgressMeasureTypeName;
            this.PerformanceMeasureDefinition = psInfoProgressMeasure.ProgressMeasureDescription;
            this.SwapChartAxes = false;
            this.CanCalculateTotal = false;
            this.IsAggregatable = psInfoProgressMeasure.IsAggregatable;
            this.PerformanceMeasureDataSourceTypeName = psInfoProgressMeasure.ProgressMeasureDataSourceTypeName;
        }

        public PerformanceMeasure PerformanceMeasure { get; set; }
        public int PerformanceMeasureID => PerformanceMeasure.PerformanceMeasureID;
        public int TenantID { get; set; }
        public HtmlString CriticalDefinitionsHtmlString { get; set; }
        public HtmlString ProjectReportingHtmlString { get; set; }
        public string PerformanceMeasureDisplayName { get; set; }
        public string MeasurementUnitTypeName { get; set; }
        public string PerformanceMeasureTypeName { get; set; }
        public string PerformanceMeasureDefinition { get; set; }
        public string ExternalDataSourceUrl { get; set; }
        public bool SwapChartAxes { get; set; }
        public bool CanCalculateTotal { get; set; }
        public bool IsAggregatable { get; set; }
        public string PerformanceMeasureDataSourceTypeName { get; set; }

        public ICollection<PerformanceMeasureActual> PerformanceMeasureActuals { get; set; }
        public ICollection<PerformanceMeasureActualSubcategoryOption> PerformanceMeasureActualSubcategoryOptions { get; set; }
        public ICollection<PerformanceMeasureSubcategory> PerformanceMeasureSubcategories { get; set; }
        public bool IsExternallySourcedPerformanceMeasure { get; set; }
        public bool HasRealSubcategories { get; set; }
    }
}