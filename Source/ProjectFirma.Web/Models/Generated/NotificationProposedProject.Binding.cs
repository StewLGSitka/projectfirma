//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[NotificationProposedProject]
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Net.Mail;
using System.Web;
using LtInfo.Common;
using LtInfo.Common.DesignByContract;
using LtInfo.Common.Models;
using ProjectFirma.Web.Common;
using ProjectFirma.Web.Controllers;

namespace ProjectFirma.Web.Models
{
    [Table("[dbo].[NotificationProposedProject]")]
    public partial class NotificationProposedProject : IHavePrimaryKey, IHaveATenantID
    {
        /// <summary>
        /// Default Constructor; only used by EF
        /// </summary>
        protected NotificationProposedProject()
        {

            this.TenantID = HttpRequestStorage.Tenant.TenantID;
        }

        /// <summary>
        /// Constructor for building a new object with MaximalConstructor required fields in preparation for insert into database
        /// </summary>
        public NotificationProposedProject(int notificationProposedProjectID, int notificationID, int proposedProjectID) : this()
        {
            this.NotificationProposedProjectID = notificationProposedProjectID;
            this.NotificationID = notificationID;
            this.ProposedProjectID = proposedProjectID;
        }

        /// <summary>
        /// Constructor for building a new object with MinimalConstructor required fields in preparation for insert into database
        /// </summary>
        public NotificationProposedProject(int notificationID, int proposedProjectID) : this()
        {
            // Mark this as a new object by setting primary key with special value
            this.NotificationProposedProjectID = ModelObjectHelpers.MakeNextUnsavedPrimaryKeyValue();
            
            this.NotificationID = notificationID;
            this.ProposedProjectID = proposedProjectID;
        }

        /// <summary>
        /// Constructor for building a new object with MinimalConstructor required fields, using objects whenever possible
        /// </summary>
        public NotificationProposedProject(Notification notification, ProposedProject proposedProject) : this()
        {
            // Mark this as a new object by setting primary key with special value
            this.NotificationProposedProjectID = ModelObjectHelpers.MakeNextUnsavedPrimaryKeyValue();
            this.NotificationID = notification.NotificationID;
            this.Notification = notification;
            notification.NotificationProposedProjects.Add(this);
            this.ProposedProjectID = proposedProject.ProposedProjectID;
            this.ProposedProject = proposedProject;
            proposedProject.NotificationProposedProjects.Add(this);
        }

        /// <summary>
        /// Creates a "blank" object of this type and populates primitives with defaults
        /// </summary>
        public static NotificationProposedProject CreateNewBlank(Notification notification, ProposedProject proposedProject)
        {
            return new NotificationProposedProject(notification, proposedProject);
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
        public static readonly List<string> DependentEntityTypeNames = new List<string> {typeof(NotificationProposedProject).Name};

        [Key]
        public int NotificationProposedProjectID { get; set; }
        public int TenantID { get; private set; }
        public int NotificationID { get; set; }
        public int ProposedProjectID { get; set; }
        public int PrimaryKey { get { return NotificationProposedProjectID; } set { NotificationProposedProjectID = value; } }

        public Tenant Tenant { get { return Tenant.AllLookupDictionary[TenantID]; } }
        public virtual Notification Notification { get; set; }
        public virtual ProposedProject ProposedProject { get; set; }

        public static class FieldLengths
        {

        }
    }
}