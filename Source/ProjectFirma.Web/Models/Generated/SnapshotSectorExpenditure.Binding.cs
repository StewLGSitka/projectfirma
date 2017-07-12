//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[SnapshotSectorExpenditure]
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
    [Table("[dbo].[SnapshotSectorExpenditure]")]
    public partial class SnapshotSectorExpenditure : IHavePrimaryKey, IHaveATenantID
    {
        /// <summary>
        /// Default Constructor; only used by EF
        /// </summary>
        protected SnapshotSectorExpenditure()
        {

            this.TenantID = HttpRequestStorage.Tenant.TenantID;
        }

        /// <summary>
        /// Constructor for building a new object with MaximalConstructor required fields in preparation for insert into database
        /// </summary>
        public SnapshotSectorExpenditure(int snapshotSectorExpenditureID, int snapshotID, int calendarYear, decimal expenditureAmount, int organizationTypeID) : this()
        {
            this.SnapshotSectorExpenditureID = snapshotSectorExpenditureID;
            this.SnapshotID = snapshotID;
            this.CalendarYear = calendarYear;
            this.ExpenditureAmount = expenditureAmount;
            this.OrganizationTypeID = organizationTypeID;
        }

        /// <summary>
        /// Constructor for building a new object with MinimalConstructor required fields in preparation for insert into database
        /// </summary>
        public SnapshotSectorExpenditure(int snapshotID, int calendarYear, decimal expenditureAmount, int organizationTypeID) : this()
        {
            // Mark this as a new object by setting primary key with special value
            this.SnapshotSectorExpenditureID = ModelObjectHelpers.MakeNextUnsavedPrimaryKeyValue();
            
            this.SnapshotID = snapshotID;
            this.CalendarYear = calendarYear;
            this.ExpenditureAmount = expenditureAmount;
            this.OrganizationTypeID = organizationTypeID;
        }

        /// <summary>
        /// Constructor for building a new object with MinimalConstructor required fields, using objects whenever possible
        /// </summary>
        public SnapshotSectorExpenditure(Snapshot snapshot, int calendarYear, decimal expenditureAmount, OrganizationType organizationType) : this()
        {
            // Mark this as a new object by setting primary key with special value
            this.SnapshotSectorExpenditureID = ModelObjectHelpers.MakeNextUnsavedPrimaryKeyValue();
            this.SnapshotID = snapshot.SnapshotID;
            this.Snapshot = snapshot;
            snapshot.SnapshotSectorExpenditures.Add(this);
            this.CalendarYear = calendarYear;
            this.ExpenditureAmount = expenditureAmount;
            this.OrganizationTypeID = organizationType.OrganizationTypeID;
            this.OrganizationType = organizationType;
            organizationType.SnapshotSectorExpenditures.Add(this);
        }

        /// <summary>
        /// Creates a "blank" object of this type and populates primitives with defaults
        /// </summary>
        public static SnapshotSectorExpenditure CreateNewBlank(Snapshot snapshot, OrganizationType organizationType)
        {
            return new SnapshotSectorExpenditure(snapshot, default(int), default(decimal), organizationType);
        }

        /// <summary>
        /// Does this object have any dependent objects? (If it does have dependent objects, these would need to be deleted before this object could be deleted.)
        /// </summary>
        /// <returns></returns>
        public bool HasDependentObjects()
        {
            return false;
        }

        /// <summary>
        /// Dependent type names of this entity
        /// </summary>
        public static readonly List<string> DependentEntityTypeNames = new List<string> {typeof(SnapshotSectorExpenditure).Name};

        [Key]
        public int SnapshotSectorExpenditureID { get; set; }
        public int TenantID { get; private set; }
        public int SnapshotID { get; set; }
        public int CalendarYear { get; set; }
        public decimal ExpenditureAmount { get; set; }
        public int OrganizationTypeID { get; set; }
        public int PrimaryKey { get { return SnapshotSectorExpenditureID; } set { SnapshotSectorExpenditureID = value; } }

        public Tenant Tenant { get { return Tenant.AllLookupDictionary[TenantID]; } }
        public virtual Snapshot Snapshot { get; set; }
        public virtual OrganizationType OrganizationType { get; set; }

        public static class FieldLengths
        {

        }
    }
}