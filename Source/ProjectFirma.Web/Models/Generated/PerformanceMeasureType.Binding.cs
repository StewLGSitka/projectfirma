//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[PerformanceMeasureType]
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Web;
using LtInfo.Common.DesignByContract;
using LtInfo.Common.Models;
using ProjectFirma.Web.Common;

namespace ProjectFirma.Web.Models
{
    public abstract partial class PerformanceMeasureType : IHavePrimaryKey
    {
        public static readonly PerformanceMeasureTypeAction Action = PerformanceMeasureTypeAction.Instance;
        public static readonly PerformanceMeasureTypeOutcome Outcome = PerformanceMeasureTypeOutcome.Instance;
        public static readonly PerformanceMeasureTypeNotApplicable NotApplicable = PerformanceMeasureTypeNotApplicable.Instance;

        public static readonly List<PerformanceMeasureType> All;
        public static readonly ReadOnlyDictionary<int, PerformanceMeasureType> AllLookupDictionary;

        /// <summary>
        /// Static type constructor to coordinate static initialization order
        /// </summary>
        static PerformanceMeasureType()
        {
            All = new List<PerformanceMeasureType> { Action, Outcome, NotApplicable };
            AllLookupDictionary = new ReadOnlyDictionary<int, PerformanceMeasureType>(All.ToDictionary(x => x.PerformanceMeasureTypeID));
        }

        /// <summary>
        /// Protected constructor only for use in instantiating the set of static lookup values that match database
        /// </summary>
        protected PerformanceMeasureType(int performanceMeasureTypeID, string performanceMeasureTypeName, string performanceMeasureTypeDisplayName, bool isUserSelectable)
        {
            PerformanceMeasureTypeID = performanceMeasureTypeID;
            PerformanceMeasureTypeName = performanceMeasureTypeName;
            PerformanceMeasureTypeDisplayName = performanceMeasureTypeDisplayName;
            IsUserSelectable = isUserSelectable;
        }

        [Key]
        public int PerformanceMeasureTypeID { get; private set; }
        public string PerformanceMeasureTypeName { get; private set; }
        public string PerformanceMeasureTypeDisplayName { get; private set; }
        public bool IsUserSelectable { get; private set; }
        [NotMapped]
        public int PrimaryKey { get { return PerformanceMeasureTypeID; } }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public bool Equals(PerformanceMeasureType other)
        {
            if (other == null)
            {
                return false;
            }
            return other.PerformanceMeasureTypeID == PerformanceMeasureTypeID;
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(obj as PerformanceMeasureType);
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override int GetHashCode()
        {
            return PerformanceMeasureTypeID;
        }

        public static bool operator ==(PerformanceMeasureType left, PerformanceMeasureType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PerformanceMeasureType left, PerformanceMeasureType right)
        {
            return !Equals(left, right);
        }

        public PerformanceMeasureTypeEnum ToEnum { get { return (PerformanceMeasureTypeEnum)GetHashCode(); } }

        public static PerformanceMeasureType ToType(int enumValue)
        {
            return ToType((PerformanceMeasureTypeEnum)enumValue);
        }

        public static PerformanceMeasureType ToType(PerformanceMeasureTypeEnum enumValue)
        {
            switch (enumValue)
            {
                case PerformanceMeasureTypeEnum.Action:
                    return Action;
                case PerformanceMeasureTypeEnum.NotApplicable:
                    return NotApplicable;
                case PerformanceMeasureTypeEnum.Outcome:
                    return Outcome;
                default:
                    throw new ArgumentException(string.Format("Unable to map Enum: {0}", enumValue));
            }
        }
    }

    public enum PerformanceMeasureTypeEnum
    {
        Action = 1,
        Outcome = 2,
        NotApplicable = 3
    }

    public partial class PerformanceMeasureTypeAction : PerformanceMeasureType
    {
        private PerformanceMeasureTypeAction(int performanceMeasureTypeID, string performanceMeasureTypeName, string performanceMeasureTypeDisplayName, bool isUserSelectable) : base(performanceMeasureTypeID, performanceMeasureTypeName, performanceMeasureTypeDisplayName, isUserSelectable) {}
        public static readonly PerformanceMeasureTypeAction Instance = new PerformanceMeasureTypeAction(1, @"Action", @"Action", true);
    }

    public partial class PerformanceMeasureTypeOutcome : PerformanceMeasureType
    {
        private PerformanceMeasureTypeOutcome(int performanceMeasureTypeID, string performanceMeasureTypeName, string performanceMeasureTypeDisplayName, bool isUserSelectable) : base(performanceMeasureTypeID, performanceMeasureTypeName, performanceMeasureTypeDisplayName, isUserSelectable) {}
        public static readonly PerformanceMeasureTypeOutcome Instance = new PerformanceMeasureTypeOutcome(2, @"Outcome", @"Outcome", true);
    }

    public partial class PerformanceMeasureTypeNotApplicable : PerformanceMeasureType
    {
        private PerformanceMeasureTypeNotApplicable(int performanceMeasureTypeID, string performanceMeasureTypeName, string performanceMeasureTypeDisplayName, bool isUserSelectable) : base(performanceMeasureTypeID, performanceMeasureTypeName, performanceMeasureTypeDisplayName, isUserSelectable) {}
        public static readonly PerformanceMeasureTypeNotApplicable Instance = new PerformanceMeasureTypeNotApplicable(3, @"Not Applicable", @"Not Applicable", false);
    }
}