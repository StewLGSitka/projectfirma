//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[PerformanceMeasureSubcategory]
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
    [Table("[dbo].[PerformanceMeasureSubcategory]")]
    public partial class PerformanceMeasureSubcategory : IHavePrimaryKey, IHaveATenantID
    {
        /// <summary>
        /// Default Constructor; only used by EF
        /// </summary>
        protected PerformanceMeasureSubcategory()
        {
            this.PerformanceMeasureActualSubcategoryOptions = new HashSet<PerformanceMeasureActualSubcategoryOption>();
            this.PerformanceMeasureActualSubcategoryOptionUpdates = new HashSet<PerformanceMeasureActualSubcategoryOptionUpdate>();
            this.PerformanceMeasureExpectedSubcategoryOptions = new HashSet<PerformanceMeasureExpectedSubcategoryOption>();
            this.PerformanceMeasureSubcategoryOptions = new HashSet<PerformanceMeasureSubcategoryOption>();
            this.SnapshotPerformanceMeasureSubcategoryOptions = new HashSet<SnapshotPerformanceMeasureSubcategoryOption>();
            this.TenantID = HttpRequestStorage.Tenant.TenantID;
        }

        /// <summary>
        /// Constructor for building a new object with MaximalConstructor required fields in preparation for insert into database
        /// </summary>
        public PerformanceMeasureSubcategory(int performanceMeasureSubcategoryID, int performanceMeasureID, string performanceMeasureSubcategoryDisplayName, string chartConfigurationJson, int? googleChartTypeID) : this()
        {
            this.PerformanceMeasureSubcategoryID = performanceMeasureSubcategoryID;
            this.PerformanceMeasureID = performanceMeasureID;
            this.PerformanceMeasureSubcategoryDisplayName = performanceMeasureSubcategoryDisplayName;
            this.ChartConfigurationJson = chartConfigurationJson;
            this.GoogleChartTypeID = googleChartTypeID;
        }

        /// <summary>
        /// Constructor for building a new object with MinimalConstructor required fields in preparation for insert into database
        /// </summary>
        public PerformanceMeasureSubcategory(int performanceMeasureID, string performanceMeasureSubcategoryDisplayName) : this()
        {
            // Mark this as a new object by setting primary key with special value
            this.PerformanceMeasureSubcategoryID = ModelObjectHelpers.MakeNextUnsavedPrimaryKeyValue();
            
            this.PerformanceMeasureID = performanceMeasureID;
            this.PerformanceMeasureSubcategoryDisplayName = performanceMeasureSubcategoryDisplayName;
        }

        /// <summary>
        /// Constructor for building a new object with MinimalConstructor required fields, using objects whenever possible
        /// </summary>
        public PerformanceMeasureSubcategory(PerformanceMeasure performanceMeasure, string performanceMeasureSubcategoryDisplayName) : this()
        {
            // Mark this as a new object by setting primary key with special value
            this.PerformanceMeasureSubcategoryID = ModelObjectHelpers.MakeNextUnsavedPrimaryKeyValue();
            this.PerformanceMeasureID = performanceMeasure.PerformanceMeasureID;
            this.PerformanceMeasure = performanceMeasure;
            performanceMeasure.PerformanceMeasureSubcategories.Add(this);
            this.PerformanceMeasureSubcategoryDisplayName = performanceMeasureSubcategoryDisplayName;
        }

        /// <summary>
        /// Creates a "blank" object of this type and populates primitives with defaults
        /// </summary>
        public static PerformanceMeasureSubcategory CreateNewBlank(PerformanceMeasure performanceMeasure)
        {
            return new PerformanceMeasureSubcategory(performanceMeasure, default(string));
        }

        /// <summary>
        /// Does this object have any dependent objects? (If it does have dependent objects, these would need to be deleted before this object could be deleted.)
        /// </summary>
        /// <returns></returns>
        public bool HasDependentObjects()
        {
            return PerformanceMeasureActualSubcategoryOptions.Any() || PerformanceMeasureActualSubcategoryOptionUpdates.Any() || PerformanceMeasureExpectedSubcategoryOptions.Any() || PerformanceMeasureSubcategoryOptions.Any() || SnapshotPerformanceMeasureSubcategoryOptions.Any();
        }

        /// <summary>
        /// Dependent type names of this entity
        /// </summary>
        public static readonly List<string> DependentEntityTypeNames = new List<string> {typeof(PerformanceMeasureSubcategory).Name, typeof(PerformanceMeasureActualSubcategoryOption).Name, typeof(PerformanceMeasureActualSubcategoryOptionUpdate).Name, typeof(PerformanceMeasureExpectedSubcategoryOption).Name, typeof(PerformanceMeasureSubcategoryOption).Name, typeof(SnapshotPerformanceMeasureSubcategoryOption).Name};


        /// <summary>
        /// Dependent type names of this entity
        /// </summary>
        public void DeleteFull()
        {
            DeleteFull(HttpRequestStorage.DatabaseEntities);
        }

        /// <summary>
        /// Dependent type names of this entity
        /// </summary>
        public void DeleteFull(DatabaseEntities dbContext)
        {

            foreach(var x in PerformanceMeasureActualSubcategoryOptions.ToList())
            {
                x.DeleteFull(dbContext);
            }

            foreach(var x in PerformanceMeasureActualSubcategoryOptionUpdates.ToList())
            {
                x.DeleteFull(dbContext);
            }

            foreach(var x in PerformanceMeasureExpectedSubcategoryOptions.ToList())
            {
                x.DeleteFull(dbContext);
            }

            foreach(var x in PerformanceMeasureSubcategoryOptions.ToList())
            {
                x.DeleteFull(dbContext);
            }

            foreach(var x in SnapshotPerformanceMeasureSubcategoryOptions.ToList())
            {
                x.DeleteFull(dbContext);
            }
            dbContext.AllPerformanceMeasureSubcategories.Remove(this);
        }

        [Key]
        public int PerformanceMeasureSubcategoryID { get; set; }
        public int TenantID { get; private set; }
        public int PerformanceMeasureID { get; set; }
        public string PerformanceMeasureSubcategoryDisplayName { get; set; }
        public string ChartConfigurationJson { get; set; }
        public int? GoogleChartTypeID { get; set; }
        [NotMapped]
        public int PrimaryKey { get { return PerformanceMeasureSubcategoryID; } set { PerformanceMeasureSubcategoryID = value; } }

        public virtual ICollection<PerformanceMeasureActualSubcategoryOption> PerformanceMeasureActualSubcategoryOptions { get; set; }
        public virtual ICollection<PerformanceMeasureActualSubcategoryOptionUpdate> PerformanceMeasureActualSubcategoryOptionUpdates { get; set; }
        public virtual ICollection<PerformanceMeasureExpectedSubcategoryOption> PerformanceMeasureExpectedSubcategoryOptions { get; set; }
        public virtual ICollection<PerformanceMeasureSubcategoryOption> PerformanceMeasureSubcategoryOptions { get; set; }
        public virtual ICollection<SnapshotPerformanceMeasureSubcategoryOption> SnapshotPerformanceMeasureSubcategoryOptions { get; set; }
        public Tenant Tenant { get { return Tenant.AllLookupDictionary[TenantID]; } }
        public virtual PerformanceMeasure PerformanceMeasure { get; set; }
        public GoogleChartType GoogleChartType { get { return GoogleChartTypeID.HasValue ? GoogleChartType.AllLookupDictionary[GoogleChartTypeID.Value] : null; } }

        public static class FieldLengths
        {
            public const int PerformanceMeasureSubcategoryDisplayName = 200;
        }
    }
}