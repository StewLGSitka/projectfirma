//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[IndicatorSubcategory]
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;
using LtInfo.Common.DesignByContract;
using LtInfo.Common.Models;
using ProjectFirma.Web.Common;

namespace ProjectFirma.Web.Models
{
    [Table("[dbo].[IndicatorSubcategory]")]
    public partial class IndicatorSubcategory : IHavePrimaryKey
    {
        /// <summary>
        /// Default Constructor; only used by EF
        /// </summary>
        protected IndicatorSubcategory()
        {
            this.IndicatorSubcategoryOptions = new HashSet<IndicatorSubcategoryOption>();
            this.PerformanceMeasureActualSubcategoryOptions = new HashSet<PerformanceMeasureActualSubcategoryOption>();
            this.PerformanceMeasureActualSubcategoryOptionUpdates = new HashSet<PerformanceMeasureActualSubcategoryOptionUpdate>();
            this.PerformanceMeasureExpectedSubcategoryOptions = new HashSet<PerformanceMeasureExpectedSubcategoryOption>();
            this.PerformanceMeasureExpectedSubcategoryOptionProposeds = new HashSet<PerformanceMeasureExpectedSubcategoryOptionProposed>();
            this.SnapshotPerformanceMeasureSubcategoryOptions = new HashSet<SnapshotPerformanceMeasureSubcategoryOption>();
        }

        /// <summary>
        /// Constructor for building a new object with MaximalConstructor required fields in preparation for insert into database
        /// </summary>
        public IndicatorSubcategory(int indicatorSubcategoryID, int indicatorID, string indicatorSubcategoryName, string indicatorSubcategoryDisplayName, int? sortOrder, string chartConfigurationJson, string chartType, bool? swapChartAxes, int? performanceMeasureID) : this()
        {
            this.IndicatorSubcategoryID = indicatorSubcategoryID;
            this.IndicatorID = indicatorID;
            this.IndicatorSubcategoryName = indicatorSubcategoryName;
            this.IndicatorSubcategoryDisplayName = indicatorSubcategoryDisplayName;
            this.SortOrder = sortOrder;
            this.ChartConfigurationJson = chartConfigurationJson;
            this.ChartType = chartType;
            this.SwapChartAxes = swapChartAxes;
            this.PerformanceMeasureID = performanceMeasureID;
        }

        /// <summary>
        /// Constructor for building a new object with MinimalConstructor required fields in preparation for insert into database
        /// </summary>
        public IndicatorSubcategory(int indicatorID, string indicatorSubcategoryName, string indicatorSubcategoryDisplayName) : this()
        {
            // Mark this as a new object by setting primary key with special value
            IndicatorSubcategoryID = ModelObjectHelpers.MakeNextUnsavedPrimaryKeyValue();
            
            this.IndicatorID = indicatorID;
            this.IndicatorSubcategoryName = indicatorSubcategoryName;
            this.IndicatorSubcategoryDisplayName = indicatorSubcategoryDisplayName;
        }

        /// <summary>
        /// Constructor for building a new object with MinimalConstructor required fields, using objects whenever possible
        /// </summary>
        public IndicatorSubcategory(Indicator indicator, string indicatorSubcategoryName, string indicatorSubcategoryDisplayName) : this()
        {
            // Mark this as a new object by setting primary key with special value
            this.IndicatorSubcategoryID = ModelObjectHelpers.MakeNextUnsavedPrimaryKeyValue();
            this.IndicatorID = indicator.IndicatorID;
            this.Indicator = indicator;
            indicator.IndicatorSubcategories.Add(this);
            this.IndicatorSubcategoryName = indicatorSubcategoryName;
            this.IndicatorSubcategoryDisplayName = indicatorSubcategoryDisplayName;
        }

        /// <summary>
        /// Creates a "blank" object of this type and populates primitives with defaults
        /// </summary>
        public static IndicatorSubcategory CreateNewBlank(Indicator indicator)
        {
            return new IndicatorSubcategory(indicator, default(string), default(string));
        }

        /// <summary>
        /// Does this object have any dependent objects? (If it does have dependent objects, these would need to be deleted before this object could be deleted.)
        /// </summary>
        /// <returns></returns>
        public bool HasDependentObjects()
        {
            return IndicatorSubcategoryOptions.Any() || PerformanceMeasureActualSubcategoryOptions.Any() || PerformanceMeasureActualSubcategoryOptionUpdates.Any() || PerformanceMeasureExpectedSubcategoryOptions.Any() || PerformanceMeasureExpectedSubcategoryOptionProposeds.Any() || SnapshotPerformanceMeasureSubcategoryOptions.Any();
        }

        /// <summary>
        /// Dependent type names of this entity
        /// </summary>
        public static readonly List<string> DependentEntityTypeNames = new List<string> {typeof(IndicatorSubcategory).Name, typeof(IndicatorSubcategoryOption).Name, typeof(PerformanceMeasureActualSubcategoryOption).Name, typeof(PerformanceMeasureActualSubcategoryOptionUpdate).Name, typeof(PerformanceMeasureExpectedSubcategoryOption).Name, typeof(PerformanceMeasureExpectedSubcategoryOptionProposed).Name, typeof(SnapshotPerformanceMeasureSubcategoryOption).Name};

        [Key]
        public int IndicatorSubcategoryID { get; set; }
        public int IndicatorID { get; set; }
        public string IndicatorSubcategoryName { get; set; }
        public string IndicatorSubcategoryDisplayName { get; set; }
        public int? SortOrder { get; set; }
        public string ChartConfigurationJson { get; set; }
        public string ChartType { get; set; }
        public bool? SwapChartAxes { get; set; }
        public int? PerformanceMeasureID { get; set; }
        public int PrimaryKey { get { return IndicatorSubcategoryID; } set { IndicatorSubcategoryID = value; } }

        public virtual ICollection<IndicatorSubcategoryOption> IndicatorSubcategoryOptions { get; set; }
        public virtual ICollection<PerformanceMeasureActualSubcategoryOption> PerformanceMeasureActualSubcategoryOptions { get; set; }
        public virtual ICollection<PerformanceMeasureActualSubcategoryOptionUpdate> PerformanceMeasureActualSubcategoryOptionUpdates { get; set; }
        public virtual ICollection<PerformanceMeasureExpectedSubcategoryOption> PerformanceMeasureExpectedSubcategoryOptions { get; set; }
        public virtual ICollection<PerformanceMeasureExpectedSubcategoryOptionProposed> PerformanceMeasureExpectedSubcategoryOptionProposeds { get; set; }
        public virtual ICollection<SnapshotPerformanceMeasureSubcategoryOption> SnapshotPerformanceMeasureSubcategoryOptions { get; set; }
        public virtual Indicator Indicator { get; set; }
        public virtual PerformanceMeasure PerformanceMeasure { get; set; }

        public static class FieldLengths
        {
            public const int IndicatorSubcategoryName = 100;
            public const int IndicatorSubcategoryDisplayName = 200;
            public const int ChartType = 100;
        }
    }
}