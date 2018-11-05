using System.Collections.Generic;
using System.Linq;
using ProjectFirma.Web.Common;

namespace ProjectFirma.Web.Models
{
    public partial class PerformanceMeasureDataSourceTypeTechnicalAssistanceValue
    {
        public override List<PerformanceMeasureReportedValue> GetReportedPerformanceMeasureValues(PerformanceMeasure performanceMeasure, List<Project> projects)
        {
            // the source for this PM is "Technical Assistance Provided to Conservation Districts"
            var technicalAssistanceHours = HttpRequestStorage.DatabaseEntities.PerformanceMeasures.SingleOrDefault(x=>x.PerformanceMeasureID == TechnicalAssistanceProvidedPMID);
            if (technicalAssistanceHours == null)
            {
                // should not be calling this from a context where that PM doesn't exist anyway, but let's not die if we do.
                return new List<PerformanceMeasureReportedValue>();
            }

            var performanceMeasureReportedValues = Project.GetReportedPerformanceMeasureValues(technicalAssistanceHours, projects).Where(x =>
            {
                var performanceMeasureValueSubcategoryOptionIDs = x.PerformanceMeasureActualSubcategoryOptions.Select(y => y.PerformanceMeasureSubcategoryOptionID).ToList();
                return performanceMeasureValueSubcategoryOptionIDs
                           .Contains(ProvidedSubcategoryOptionID) && performanceMeasureValueSubcategoryOptionIDs
                           .Contains(ProvidedToConservationDistrictionsSubcategoryOptionID);
            });  // Should only be counting "Provided" to "Conservation District" Technical Assistance Hours

            // get these now to prepare for the main calculation
            var technicalAssistanceParameters = HttpRequestStorage.DatabaseEntities.TechnicalAssistanceParameters.ToList();
            var performanceMeasureActualSubcategoryOptions = new List<IPerformanceMeasureValueSubcategoryOption>
            {
                new VirtualPerformanceMeasureValueSubcategoryOption(performanceMeasure.PerformanceMeasureSubcategories
                    .Single())
            };

            return performanceMeasureReportedValues.Select(performanceMeasureReportedValue =>
            {
                var year = performanceMeasureReportedValue.CalendarYear;
                var project = performanceMeasureReportedValue.Project;
                var technicalAssistanceParameter = technicalAssistanceParameters.SingleOrDefault(y => y.Year == year);
                var engineeringHourlyCost = technicalAssistanceParameter?.EngineeringHourlyCost;
                var otherAssistanceHourlyCost = technicalAssistanceParameter?.OtherAssistanceHourlyCost;

                var technicalAssistanceValueInYear = performanceMeasureReportedValue
                    .PerformanceMeasureActualSubcategoryOptions
                    .Select(y => y.PerformanceMeasureSubcategoryOptionID).Contains(EngineeringAssistanceSubcategoryOptionID)  // "Engineering Assistance" is treated differently from other kinds of technical assistance.
                    ? performanceMeasureReportedValue.ReportedValue.GetValueOrDefault() *
                      (double)engineeringHourlyCost.GetValueOrDefault()
                    : performanceMeasureReportedValue.ReportedValue.GetValueOrDefault() *
                      (double)otherAssistanceHourlyCost.GetValueOrDefault();
                return new PerformanceMeasureReportedValue(performanceMeasure, project,year, technicalAssistanceValueInYear)
                {
                    PerformanceMeasureActualSubcategoryOptions = performanceMeasureActualSubcategoryOptions
                };
            }).ToList();
        }
    }
}