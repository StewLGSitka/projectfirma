//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[TaxonomyTierOne]
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
    [Table("[dbo].[TaxonomyTierOne]")]
    public partial class TaxonomyTierOne : IHavePrimaryKey
    {
        /// <summary>
        /// Default Constructor; only used by EF
        /// </summary>
        protected TaxonomyTierOne()
        {
            this.Projects = new HashSet<Project>();
            this.ProposedProjects = new HashSet<ProposedProject>();
            this.TaxonomyTierOneImages = new HashSet<TaxonomyTierOneImage>();
        }

        /// <summary>
        /// Constructor for building a new object with MaximalConstructor required fields in preparation for insert into database
        /// </summary>
        public TaxonomyTierOne(int taxonomyTierOneID, int taxonomyTierTwoID, string taxonomyTierOneName, string taxonomyTierOneDescription) : this()
        {
            this.TaxonomyTierOneID = taxonomyTierOneID;
            this.TaxonomyTierTwoID = taxonomyTierTwoID;
            this.TaxonomyTierOneName = taxonomyTierOneName;
            this.TaxonomyTierOneDescription = taxonomyTierOneDescription;
        }

        /// <summary>
        /// Constructor for building a new object with MinimalConstructor required fields in preparation for insert into database
        /// </summary>
        public TaxonomyTierOne(int taxonomyTierTwoID, string taxonomyTierOneName) : this()
        {
            // Mark this as a new object by setting primary key with special value
            TaxonomyTierOneID = ModelObjectHelpers.MakeNextUnsavedPrimaryKeyValue();
            
            this.TaxonomyTierTwoID = taxonomyTierTwoID;
            this.TaxonomyTierOneName = taxonomyTierOneName;
        }

        /// <summary>
        /// Constructor for building a new object with MinimalConstructor required fields, using objects whenever possible
        /// </summary>
        public TaxonomyTierOne(TaxonomyTierTwo taxonomyTierTwo, string taxonomyTierOneName) : this()
        {
            // Mark this as a new object by setting primary key with special value
            this.TaxonomyTierOneID = ModelObjectHelpers.MakeNextUnsavedPrimaryKeyValue();
            this.TaxonomyTierTwoID = taxonomyTierTwo.TaxonomyTierTwoID;
            this.TaxonomyTierTwo = taxonomyTierTwo;
            taxonomyTierTwo.TaxonomyTierOnes.Add(this);
            this.TaxonomyTierOneName = taxonomyTierOneName;
        }

        /// <summary>
        /// Creates a "blank" object of this type and populates primitives with defaults
        /// </summary>
        public static TaxonomyTierOne CreateNewBlank(TaxonomyTierTwo taxonomyTierTwo)
        {
            return new TaxonomyTierOne(taxonomyTierTwo, default(string));
        }

        /// <summary>
        /// Does this object have any dependent objects? (If it does have dependent objects, these would need to be deleted before this object could be deleted.)
        /// </summary>
        /// <returns></returns>
        public bool HasDependentObjects()
        {
            return Projects.Any() || ProposedProjects.Any() || TaxonomyTierOneImages.Any();
        }

        /// <summary>
        /// Dependent type names of this entity
        /// </summary>
        public static readonly List<string> DependentEntityTypeNames = new List<string> {typeof(TaxonomyTierOne).Name, typeof(Project).Name, typeof(ProposedProject).Name, typeof(TaxonomyTierOneImage).Name};

        [Key]
        public int TaxonomyTierOneID { get; set; }
        public int TaxonomyTierTwoID { get; set; }
        public string TaxonomyTierOneName { get; set; }
        [NotMapped]
        private string TaxonomyTierOneDescription { get; set; }
        public HtmlString TaxonomyTierOneDescriptionHtmlString
        { 
            get { return TaxonomyTierOneDescription == null ? null : new HtmlString(TaxonomyTierOneDescription); }
            set { TaxonomyTierOneDescription = value == null ? null : value.ToString(); }
        }
        public int PrimaryKey { get { return TaxonomyTierOneID; } set { TaxonomyTierOneID = value; } }

        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<ProposedProject> ProposedProjects { get; set; }
        public virtual ICollection<TaxonomyTierOneImage> TaxonomyTierOneImages { get; set; }
        public virtual TaxonomyTierTwo TaxonomyTierTwo { get; set; }

        public static class FieldLengths
        {
            public const int TaxonomyTierOneName = 100;
        }
    }
}