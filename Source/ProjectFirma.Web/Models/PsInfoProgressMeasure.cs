using System.Collections.Generic;
using System.Web;

namespace ProjectFirma.Web.Models
{
    public class PsInfoProgressMeasure
    {
        public int ProgressMeasureID { get; set; }
        public string ProgressMeasureName { get; set; }
        public string ProgressMeasureTypeName { get; set; }
        public string MeasurementUnitTypeName { get; set; }
        public string ProgressMeasureDataSourceTypeName { get; set; }
        public HtmlString DefinitionsHtmlString { get; set; }
        public HtmlString ReportingGuidanceHtmlString { get; set; }
        public string ProgressMeasureDescription { get; set; }
        public bool IsAggregatable { get; set; }


    }


}