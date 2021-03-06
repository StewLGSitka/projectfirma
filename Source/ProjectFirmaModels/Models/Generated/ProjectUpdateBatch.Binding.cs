//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ProjectUpdateBatch]
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;
using LtInfo.Common;
using LtInfo.Common.DesignByContract;
using LtInfo.Common.Models;

namespace ProjectFirmaModels.Models
{
    // Table [dbo].[ProjectUpdateBatch] is multi-tenant, so is attributed as IHaveATenantID
    [Table("[dbo].[ProjectUpdateBatch]")]
    public partial class ProjectUpdateBatch : IHavePrimaryKey, IHaveATenantID
    {
        /// <summary>
        /// Default Constructor; only used by EF
        /// </summary>
        protected ProjectUpdateBatch()
        {
            this.PerformanceMeasureActualUpdates = new HashSet<PerformanceMeasureActualUpdate>();
            this.ProjectCustomAttributeUpdates = new HashSet<ProjectCustomAttributeUpdate>();
            this.ProjectDocumentUpdates = new HashSet<ProjectDocumentUpdate>();
            this.ProjectExemptReportingYearUpdates = new HashSet<ProjectExemptReportingYearUpdate>();
            this.ProjectExternalLinkUpdates = new HashSet<ProjectExternalLinkUpdate>();
            this.ProjectFundingSourceExpenditureUpdates = new HashSet<ProjectFundingSourceExpenditureUpdate>();
            this.ProjectFundingSourceRequestUpdates = new HashSet<ProjectFundingSourceRequestUpdate>();
            this.ProjectGeospatialAreaTypeNoteUpdates = new HashSet<ProjectGeospatialAreaTypeNoteUpdate>();
            this.ProjectGeospatialAreaUpdates = new HashSet<ProjectGeospatialAreaUpdate>();
            this.ProjectImageUpdates = new HashSet<ProjectImageUpdate>();
            this.ProjectLocationStagingUpdates = new HashSet<ProjectLocationStagingUpdate>();
            this.ProjectLocationUpdates = new HashSet<ProjectLocationUpdate>();
            this.ProjectNoteUpdates = new HashSet<ProjectNoteUpdate>();
            this.ProjectOrganizationUpdates = new HashSet<ProjectOrganizationUpdate>();
            this.ProjectUpdates = new HashSet<ProjectUpdate>();
            this.ProjectUpdateHistories = new HashSet<ProjectUpdateHistory>();
        }

        /// <summary>
        /// Constructor for building a new object with MaximalConstructor required fields in preparation for insert into database
        /// </summary>
        public ProjectUpdateBatch(int projectUpdateBatchID, int projectID, DateTime lastUpdateDate, string performanceMeasureActualYearsExemptionExplanation, int lastUpdatePersonID, string basicsComment, string expendituresComment, string performanceMeasuresComment, string locationSimpleComment, string locationDetailedComment, string budgetsComment, int projectUpdateStateID, bool isPhotosUpdated, string basicsDiffLog, string performanceMeasureDiffLog, string expendituresDiffLog, string budgetsDiffLog, string externalLinksDiffLog, string notesDiffLog, string geospatialAreaComment, string expectedFundingComment, string expectedFundingDiffLog, string organizationsComment, string organizationsDiffLog, string noExpendituresToReportExplanation) : this()
        {
            this.ProjectUpdateBatchID = projectUpdateBatchID;
            this.ProjectID = projectID;
            this.LastUpdateDate = lastUpdateDate;
            this.PerformanceMeasureActualYearsExemptionExplanation = performanceMeasureActualYearsExemptionExplanation;
            this.LastUpdatePersonID = lastUpdatePersonID;
            this.BasicsComment = basicsComment;
            this.ExpendituresComment = expendituresComment;
            this.PerformanceMeasuresComment = performanceMeasuresComment;
            this.LocationSimpleComment = locationSimpleComment;
            this.LocationDetailedComment = locationDetailedComment;
            this.BudgetsComment = budgetsComment;
            this.ProjectUpdateStateID = projectUpdateStateID;
            this.IsPhotosUpdated = isPhotosUpdated;
            this.BasicsDiffLog = basicsDiffLog;
            this.PerformanceMeasureDiffLog = performanceMeasureDiffLog;
            this.ExpendituresDiffLog = expendituresDiffLog;
            this.BudgetsDiffLog = budgetsDiffLog;
            this.ExternalLinksDiffLog = externalLinksDiffLog;
            this.NotesDiffLog = notesDiffLog;
            this.GeospatialAreaComment = geospatialAreaComment;
            this.ExpectedFundingComment = expectedFundingComment;
            this.ExpectedFundingDiffLog = expectedFundingDiffLog;
            this.OrganizationsComment = organizationsComment;
            this.OrganizationsDiffLog = organizationsDiffLog;
            this.NoExpendituresToReportExplanation = noExpendituresToReportExplanation;
        }

        /// <summary>
        /// Constructor for building a new object with MinimalConstructor required fields in preparation for insert into database
        /// </summary>
        public ProjectUpdateBatch(int projectID, DateTime lastUpdateDate, int lastUpdatePersonID, int projectUpdateStateID, bool isPhotosUpdated) : this()
        {
            // Mark this as a new object by setting primary key with special value
            this.ProjectUpdateBatchID = ModelObjectHelpers.MakeNextUnsavedPrimaryKeyValue();
            
            this.ProjectID = projectID;
            this.LastUpdateDate = lastUpdateDate;
            this.LastUpdatePersonID = lastUpdatePersonID;
            this.ProjectUpdateStateID = projectUpdateStateID;
            this.IsPhotosUpdated = isPhotosUpdated;
        }

        /// <summary>
        /// Constructor for building a new object with MinimalConstructor required fields, using objects whenever possible
        /// </summary>
        public ProjectUpdateBatch(Project project, DateTime lastUpdateDate, Person lastUpdatePerson, ProjectUpdateState projectUpdateState, bool isPhotosUpdated) : this()
        {
            // Mark this as a new object by setting primary key with special value
            this.ProjectUpdateBatchID = ModelObjectHelpers.MakeNextUnsavedPrimaryKeyValue();
            this.ProjectID = project.ProjectID;
            this.Project = project;
            project.ProjectUpdateBatches.Add(this);
            this.LastUpdateDate = lastUpdateDate;
            this.LastUpdatePersonID = lastUpdatePerson.PersonID;
            this.LastUpdatePerson = lastUpdatePerson;
            lastUpdatePerson.ProjectUpdateBatchesWhereYouAreTheLastUpdatePerson.Add(this);
            this.ProjectUpdateStateID = projectUpdateState.ProjectUpdateStateID;
            this.IsPhotosUpdated = isPhotosUpdated;
        }

        /// <summary>
        /// Creates a "blank" object of this type and populates primitives with defaults
        /// </summary>
        public static ProjectUpdateBatch CreateNewBlank(Project project, Person lastUpdatePerson, ProjectUpdateState projectUpdateState)
        {
            return new ProjectUpdateBatch(project, default(DateTime), lastUpdatePerson, projectUpdateState, default(bool));
        }

        /// <summary>
        /// Does this object have any dependent objects? (If it does have dependent objects, these would need to be deleted before this object could be deleted.)
        /// </summary>
        /// <returns></returns>
        public bool HasDependentObjects()
        {
            return PerformanceMeasureActualUpdates.Any() || ProjectCustomAttributeUpdates.Any() || ProjectDocumentUpdates.Any() || ProjectExemptReportingYearUpdates.Any() || ProjectExternalLinkUpdates.Any() || ProjectFundingSourceExpenditureUpdates.Any() || ProjectFundingSourceRequestUpdates.Any() || ProjectGeospatialAreaTypeNoteUpdates.Any() || ProjectGeospatialAreaUpdates.Any() || ProjectImageUpdates.Any() || ProjectLocationStagingUpdates.Any() || ProjectLocationUpdates.Any() || ProjectNoteUpdates.Any() || ProjectOrganizationUpdates.Any() || (ProjectUpdate != null) || ProjectUpdateHistories.Any();
        }

        /// <summary>
        /// Dependent type names of this entity
        /// </summary>
        public static readonly List<string> DependentEntityTypeNames = new List<string> {typeof(ProjectUpdateBatch).Name, typeof(PerformanceMeasureActualUpdate).Name, typeof(ProjectCustomAttributeUpdate).Name, typeof(ProjectDocumentUpdate).Name, typeof(ProjectExemptReportingYearUpdate).Name, typeof(ProjectExternalLinkUpdate).Name, typeof(ProjectFundingSourceExpenditureUpdate).Name, typeof(ProjectFundingSourceRequestUpdate).Name, typeof(ProjectGeospatialAreaTypeNoteUpdate).Name, typeof(ProjectGeospatialAreaUpdate).Name, typeof(ProjectImageUpdate).Name, typeof(ProjectLocationStagingUpdate).Name, typeof(ProjectLocationUpdate).Name, typeof(ProjectNoteUpdate).Name, typeof(ProjectOrganizationUpdate).Name, typeof(ProjectUpdate).Name, typeof(ProjectUpdateHistory).Name};


        /// <summary>
        /// Delete just the entity 
        /// </summary>
        public void Delete(DatabaseEntities dbContext)
        {
            dbContext.AllProjectUpdateBatches.Remove(this);
        }
        
        /// <summary>
        /// Delete entity plus all children
        /// </summary>
        public void DeleteFull(DatabaseEntities dbContext)
        {
            DeleteChildren(dbContext);
            Delete(dbContext);
        }
        /// <summary>
        /// Dependent type names of this entity
        /// </summary>
        public void DeleteChildren(DatabaseEntities dbContext)
        {

            foreach(var x in PerformanceMeasureActualUpdates.ToList())
            {
                x.DeleteFull(dbContext);
            }

            foreach(var x in ProjectCustomAttributeUpdates.ToList())
            {
                x.DeleteFull(dbContext);
            }

            foreach(var x in ProjectDocumentUpdates.ToList())
            {
                x.DeleteFull(dbContext);
            }

            foreach(var x in ProjectExemptReportingYearUpdates.ToList())
            {
                x.DeleteFull(dbContext);
            }

            foreach(var x in ProjectExternalLinkUpdates.ToList())
            {
                x.DeleteFull(dbContext);
            }

            foreach(var x in ProjectFundingSourceExpenditureUpdates.ToList())
            {
                x.DeleteFull(dbContext);
            }

            foreach(var x in ProjectFundingSourceRequestUpdates.ToList())
            {
                x.DeleteFull(dbContext);
            }

            foreach(var x in ProjectGeospatialAreaTypeNoteUpdates.ToList())
            {
                x.DeleteFull(dbContext);
            }

            foreach(var x in ProjectGeospatialAreaUpdates.ToList())
            {
                x.DeleteFull(dbContext);
            }

            foreach(var x in ProjectImageUpdates.ToList())
            {
                x.DeleteFull(dbContext);
            }

            foreach(var x in ProjectLocationStagingUpdates.ToList())
            {
                x.DeleteFull(dbContext);
            }

            foreach(var x in ProjectLocationUpdates.ToList())
            {
                x.DeleteFull(dbContext);
            }

            foreach(var x in ProjectNoteUpdates.ToList())
            {
                x.DeleteFull(dbContext);
            }

            foreach(var x in ProjectOrganizationUpdates.ToList())
            {
                x.DeleteFull(dbContext);
            }

            foreach(var x in ProjectUpdates.ToList())
            {
                x.DeleteFull(dbContext);
            }

            foreach(var x in ProjectUpdateHistories.ToList())
            {
                x.DeleteFull(dbContext);
            }
        }

        [Key]
        public int ProjectUpdateBatchID { get; set; }
        public int TenantID { get; set; }
        public int ProjectID { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string PerformanceMeasureActualYearsExemptionExplanation { get; set; }
        public int LastUpdatePersonID { get; set; }
        public string BasicsComment { get; set; }
        public string ExpendituresComment { get; set; }
        public string PerformanceMeasuresComment { get; set; }
        public string LocationSimpleComment { get; set; }
        public string LocationDetailedComment { get; set; }
        public string BudgetsComment { get; set; }
        public int ProjectUpdateStateID { get; set; }
        public bool IsPhotosUpdated { get; set; }
        public string BasicsDiffLog { get; set; }
        [NotMapped]
        public HtmlString BasicsDiffLogHtmlString
        { 
            get { return BasicsDiffLog == null ? null : new HtmlString(BasicsDiffLog); }
            set { BasicsDiffLog = value?.ToString(); }
        }
        public string PerformanceMeasureDiffLog { get; set; }
        [NotMapped]
        public HtmlString PerformanceMeasureDiffLogHtmlString
        { 
            get { return PerformanceMeasureDiffLog == null ? null : new HtmlString(PerformanceMeasureDiffLog); }
            set { PerformanceMeasureDiffLog = value?.ToString(); }
        }
        public string ExpendituresDiffLog { get; set; }
        [NotMapped]
        public HtmlString ExpendituresDiffLogHtmlString
        { 
            get { return ExpendituresDiffLog == null ? null : new HtmlString(ExpendituresDiffLog); }
            set { ExpendituresDiffLog = value?.ToString(); }
        }
        public string BudgetsDiffLog { get; set; }
        [NotMapped]
        public HtmlString BudgetsDiffLogHtmlString
        { 
            get { return BudgetsDiffLog == null ? null : new HtmlString(BudgetsDiffLog); }
            set { BudgetsDiffLog = value?.ToString(); }
        }
        public string ExternalLinksDiffLog { get; set; }
        [NotMapped]
        public HtmlString ExternalLinksDiffLogHtmlString
        { 
            get { return ExternalLinksDiffLog == null ? null : new HtmlString(ExternalLinksDiffLog); }
            set { ExternalLinksDiffLog = value?.ToString(); }
        }
        public string NotesDiffLog { get; set; }
        [NotMapped]
        public HtmlString NotesDiffLogHtmlString
        { 
            get { return NotesDiffLog == null ? null : new HtmlString(NotesDiffLog); }
            set { NotesDiffLog = value?.ToString(); }
        }
        public string GeospatialAreaComment { get; set; }
        public string ExpectedFundingComment { get; set; }
        public string ExpectedFundingDiffLog { get; set; }
        public string OrganizationsComment { get; set; }
        public string OrganizationsDiffLog { get; set; }
        public string NoExpendituresToReportExplanation { get; set; }
        [NotMapped]
        public int PrimaryKey { get { return ProjectUpdateBatchID; } set { ProjectUpdateBatchID = value; } }

        public virtual ICollection<PerformanceMeasureActualUpdate> PerformanceMeasureActualUpdates { get; set; }
        public virtual ICollection<ProjectCustomAttributeUpdate> ProjectCustomAttributeUpdates { get; set; }
        public virtual ICollection<ProjectDocumentUpdate> ProjectDocumentUpdates { get; set; }
        public virtual ICollection<ProjectExemptReportingYearUpdate> ProjectExemptReportingYearUpdates { get; set; }
        public virtual ICollection<ProjectExternalLinkUpdate> ProjectExternalLinkUpdates { get; set; }
        public virtual ICollection<ProjectFundingSourceExpenditureUpdate> ProjectFundingSourceExpenditureUpdates { get; set; }
        public virtual ICollection<ProjectFundingSourceRequestUpdate> ProjectFundingSourceRequestUpdates { get; set; }
        public virtual ICollection<ProjectGeospatialAreaTypeNoteUpdate> ProjectGeospatialAreaTypeNoteUpdates { get; set; }
        public virtual ICollection<ProjectGeospatialAreaUpdate> ProjectGeospatialAreaUpdates { get; set; }
        public virtual ICollection<ProjectImageUpdate> ProjectImageUpdates { get; set; }
        public virtual ICollection<ProjectLocationStagingUpdate> ProjectLocationStagingUpdates { get; set; }
        public virtual ICollection<ProjectLocationUpdate> ProjectLocationUpdates { get; set; }
        public virtual ICollection<ProjectNoteUpdate> ProjectNoteUpdates { get; set; }
        public virtual ICollection<ProjectOrganizationUpdate> ProjectOrganizationUpdates { get; set; }
        public virtual ICollection<ProjectUpdate> ProjectUpdates { get; set; }
        [NotMapped]
        public ProjectUpdate ProjectUpdate { get { return ProjectUpdates.SingleOrDefault(); } set { ProjectUpdates = new List<ProjectUpdate>{value};} }
        public virtual ICollection<ProjectUpdateHistory> ProjectUpdateHistories { get; set; }
        public Tenant Tenant { get { return Tenant.AllLookupDictionary[TenantID]; } }
        public virtual Project Project { get; set; }
        public virtual Person LastUpdatePerson { get; set; }
        public ProjectUpdateState ProjectUpdateState { get { return ProjectUpdateState.AllLookupDictionary[ProjectUpdateStateID]; } }

        public static class FieldLengths
        {
            public const int PerformanceMeasureActualYearsExemptionExplanation = 4000;
            public const int BasicsComment = 1000;
            public const int ExpendituresComment = 1000;
            public const int PerformanceMeasuresComment = 1000;
            public const int LocationSimpleComment = 1000;
            public const int LocationDetailedComment = 1000;
            public const int BudgetsComment = 1000;
            public const int GeospatialAreaComment = 1000;
            public const int ExpectedFundingComment = 1000;
            public const int OrganizationsComment = 1000;
            public const int OrganizationsDiffLog = 1;
        }
    }
}