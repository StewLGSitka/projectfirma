//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[PerformanceMeasureDataSourceType]
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
    public abstract partial class PerformanceMeasureDataSourceType : IHavePrimaryKey
    {
        public static readonly PerformanceMeasureDataSourceTypeProject Project = PerformanceMeasureDataSourceTypeProject.Instance;
        public static readonly PerformanceMeasureDataSourceTypeTechnicalAssistanceValue TechnicalAssistanceValue = PerformanceMeasureDataSourceTypeTechnicalAssistanceValue.Instance;
        public static readonly PerformanceMeasureDataSourceTypeNotApplicable NotApplicable = PerformanceMeasureDataSourceTypeNotApplicable.Instance;

        public static readonly List<PerformanceMeasureDataSourceType> All;
        public static readonly ReadOnlyDictionary<int, PerformanceMeasureDataSourceType> AllLookupDictionary;

        /// <summary>
        /// Static type constructor to coordinate static initialization order
        /// </summary>
        static PerformanceMeasureDataSourceType()
        {
            All = new List<PerformanceMeasureDataSourceType> { Project, TechnicalAssistanceValue, NotApplicable };
            AllLookupDictionary = new ReadOnlyDictionary<int, PerformanceMeasureDataSourceType>(All.ToDictionary(x => x.PerformanceMeasureDataSourceTypeID));
        }

        /// <summary>
        /// Protected constructor only for use in instantiating the set of static lookup values that match database
        /// </summary>
        protected PerformanceMeasureDataSourceType(int performanceMeasureDataSourceTypeID, string performanceMeasureDataSourceTypeName, string performanceMeasureDataSourceTypeDisplayName, bool isCustomCalculation, bool isUserSelectable)
        {
            PerformanceMeasureDataSourceTypeID = performanceMeasureDataSourceTypeID;
            PerformanceMeasureDataSourceTypeName = performanceMeasureDataSourceTypeName;
            PerformanceMeasureDataSourceTypeDisplayName = performanceMeasureDataSourceTypeDisplayName;
            IsCustomCalculation = isCustomCalculation;
            IsUserSelectable = isUserSelectable;
        }

        [Key]
        public int PerformanceMeasureDataSourceTypeID { get; private set; }
        public string PerformanceMeasureDataSourceTypeName { get; private set; }
        public string PerformanceMeasureDataSourceTypeDisplayName { get; private set; }
        public bool IsCustomCalculation { get; private set; }
        public bool IsUserSelectable { get; private set; }
        [NotMapped]
        public int PrimaryKey { get { return PerformanceMeasureDataSourceTypeID; } }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public bool Equals(PerformanceMeasureDataSourceType other)
        {
            if (other == null)
            {
                return false;
            }
            return other.PerformanceMeasureDataSourceTypeID == PerformanceMeasureDataSourceTypeID;
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(obj as PerformanceMeasureDataSourceType);
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override int GetHashCode()
        {
            return PerformanceMeasureDataSourceTypeID;
        }

        public static bool operator ==(PerformanceMeasureDataSourceType left, PerformanceMeasureDataSourceType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PerformanceMeasureDataSourceType left, PerformanceMeasureDataSourceType right)
        {
            return !Equals(left, right);
        }

        public PerformanceMeasureDataSourceTypeEnum ToEnum { get { return (PerformanceMeasureDataSourceTypeEnum)GetHashCode(); } }

        public static PerformanceMeasureDataSourceType ToType(int enumValue)
        {
            return ToType((PerformanceMeasureDataSourceTypeEnum)enumValue);
        }

        public static PerformanceMeasureDataSourceType ToType(PerformanceMeasureDataSourceTypeEnum enumValue)
        {
            switch (enumValue)
            {
                case PerformanceMeasureDataSourceTypeEnum.NotApplicable:
                    return NotApplicable;
                case PerformanceMeasureDataSourceTypeEnum.Project:
                    return Project;
                case PerformanceMeasureDataSourceTypeEnum.TechnicalAssistanceValue:
                    return TechnicalAssistanceValue;
                default:
                    throw new ArgumentException(string.Format("Unable to map Enum: {0}", enumValue));
            }
        }
    }

    public enum PerformanceMeasureDataSourceTypeEnum
    {
        Project = 1,
        TechnicalAssistanceValue = 2,
        NotApplicable = 3
    }

    public partial class PerformanceMeasureDataSourceTypeProject : PerformanceMeasureDataSourceType
    {
        private PerformanceMeasureDataSourceTypeProject(int performanceMeasureDataSourceTypeID, string performanceMeasureDataSourceTypeName, string performanceMeasureDataSourceTypeDisplayName, bool isCustomCalculation, bool isUserSelectable) : base(performanceMeasureDataSourceTypeID, performanceMeasureDataSourceTypeName, performanceMeasureDataSourceTypeDisplayName, isCustomCalculation, isUserSelectable) {}
        public static readonly PerformanceMeasureDataSourceTypeProject Instance = new PerformanceMeasureDataSourceTypeProject(1, @"Project", @"Project", false, true);
    }

    public partial class PerformanceMeasureDataSourceTypeTechnicalAssistanceValue : PerformanceMeasureDataSourceType
    {
        private PerformanceMeasureDataSourceTypeTechnicalAssistanceValue(int performanceMeasureDataSourceTypeID, string performanceMeasureDataSourceTypeName, string performanceMeasureDataSourceTypeDisplayName, bool isCustomCalculation, bool isUserSelectable) : base(performanceMeasureDataSourceTypeID, performanceMeasureDataSourceTypeName, performanceMeasureDataSourceTypeDisplayName, isCustomCalculation, isUserSelectable) {}
        public static readonly PerformanceMeasureDataSourceTypeTechnicalAssistanceValue Instance = new PerformanceMeasureDataSourceTypeTechnicalAssistanceValue(2, @"TechnicalAssistanceValue", @"Technical Assistance Value", true, true);
    }

    public partial class PerformanceMeasureDataSourceTypeNotApplicable : PerformanceMeasureDataSourceType
    {
        private PerformanceMeasureDataSourceTypeNotApplicable(int performanceMeasureDataSourceTypeID, string performanceMeasureDataSourceTypeName, string performanceMeasureDataSourceTypeDisplayName, bool isCustomCalculation, bool isUserSelectable) : base(performanceMeasureDataSourceTypeID, performanceMeasureDataSourceTypeName, performanceMeasureDataSourceTypeDisplayName, isCustomCalculation, isUserSelectable) {}
        public static readonly PerformanceMeasureDataSourceTypeNotApplicable Instance = new PerformanceMeasureDataSourceTypeNotApplicable(3, @"Not Applicable", @"Not Applicable", true, false);
    }
}