﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ProjectFirma.Web.Common;
using ProjectFirma.Web.Models;
using LtInfo.Common.DhtmlWrappers;
using LtInfo.Common.Models;

namespace ProjectFirma.Web.Service.ServiceModels
{
    [DataContract]
    public class WebServiceProject : SimpleModelObject
    {
        public WebServiceProject(Project project)
        {
            ProjectNumber = project.ProjectNumberString;
            ProjectName = project.ProjectName;
            FocusArea = project.ActionPriority.Program.FocusArea.FocusAreaName;
            Program = project.ActionPriority.Program.ProgramName;
            ActionPriority = project.ActionPriority.ActionPriorityName;
            ProjectDescription = project.ProjectDescription;
            LeadImplementer = project.LeadImplementerName;

            PlanningStartDate = project.PlanningDesignStartYear;
            ImplementationStartDate = project.ImplementationStartYear;
            EndDate = project.CompletionYear;
            Stage = project.ProjectStage.ProjectStageDisplayName;

            Latitude = project.ProjectLocationPointLatitude;
            Longitude = project.ProjectLocationPointLongitude;

            Datum = "WGS84";
            
            ProjectRegion = project.ProjectLocationTypeDisplay;
            ProjectState = project.ProjectLocationStateProvince;
            ProjectJurisdiction = project.ProjectLocationJurisdiction;
            ProjectWatershed = project.ProjectLocationWatershed;

            ProjectSummaryUrl = project.GetSummaryUrl();
            ProjectFactSheetUrl = project.GetFactSheetUrl();
        }

        [DataMember] public string ProjectNumber { get; set; }
        [DataMember] public string ProjectName { get; set; }
        [DataMember] public string FocusArea { get; set; }
        [DataMember] public string Program { get; set; }
        [DataMember] public string ActionPriority { get; set; }
        [DataMember] public string ProjectDescription { get; set; }
        [DataMember] public string LeadImplementer { get; set; }

        [DataMember] public int? PlanningStartDate { get; set; }
        [DataMember] public int? ImplementationStartDate { get; set; }
        [DataMember] public int? EndDate { get; set; }        
        [DataMember] public string Stage { get; set; }

        [DataMember] public double? Latitude { get; set; }
        [DataMember] public double? Longitude { get; set; }
        [DataMember] public string Datum { get; set; }
        [DataMember] public string ProjectRegion { get; set; }
        [DataMember] public string ProjectState { get; set; }
        [DataMember] public string ProjectJurisdiction { get; set; }
        [DataMember] public string ProjectWatershed { get; set; }

        [DataMember] public string ProjectSummaryUrl { get; set; }
        [DataMember] public string ProjectFactSheetUrl { get; set; }

        public static List<WebServiceProject> GetProject(string projectNumber)
        {
            var project = HttpRequestStorage.DatabaseEntities.Projects.GetProjectByProjectNumber(projectNumber);
            return new List<WebServiceProject> {new WebServiceProject(project)};
        }

        public static List<WebServiceProject> GetProjects()
        {
            var projects =
                HttpRequestStorage.DatabaseEntities.Projects
                    .Where(x => x.ProjectStageID != ProjectStage.Terminated.ProjectStageID && x.ProjectStageID != ProjectStage.Deferred.ProjectStageID)
                    .ToList();                    
            return
                projects
                    .Select(x => new WebServiceProject(x))
                    .OrderBy(x => x.ProjectNumber)
                    .ToList();
        }

        public static List<WebServiceProject> GetProjectsByOrganization(int organizationID)
        {
            var projectsByImplementingOrganization = HttpRequestStorage.DatabaseEntities.ProjectImplementingOrganizations.Where(x => x.OrganizationID == organizationID).Select(x => x.ProjectID);
            var projectsByFundingOrganization = HttpRequestStorage.DatabaseEntities.ProjectFundingOrganizations.Where(x => x.OrganizationID == organizationID).Select(x => x.ProjectID);

            var projectIDs = projectsByImplementingOrganization.Union(projectsByFundingOrganization).ToList();
            var projects = HttpRequestStorage.DatabaseEntities.Projects.Where(x => projectIDs.Contains(x.ProjectID)).ToList();

            return projects
                .Select(x => new WebServiceProject(x))
                    .OrderBy(x => x.ProjectNumber)
                    .ToList();
        }
    }

    public class WebServiceProjectGridSpec : GridSpec<WebServiceProject>
    {
        public WebServiceProjectGridSpec()
        {
            Add("ProjectNumber", x => x.ProjectNumber, 0);
            Add("ProjectName", x => x.ProjectName, 0);
            Add("FocusArea", x => x.FocusArea, 0);
            Add("Program", x => x.Program, 0);
            Add("ActionPriority", x => x.ActionPriority, 0);
            Add("Stage", x => x.Stage, 0);
            Add("ProjectDescription", x => x.ProjectDescription, 0);
            Add("LeadImplementer", x => x.LeadImplementer, 0);
            Add("PlanningStartDate", x => x.PlanningStartDate, 0);
            Add("ImplementationStartDate", x => x.ImplementationStartDate, 0);
            Add("EndDate", x => x.EndDate, 0);
            Add("Latitude", x => x.Latitude, 0);
            Add("Longitude", x => x.Longitude, 0);
            Add("Datum", x => x.Datum, 0);
            Add("ProjectRegion", x => x.ProjectRegion, 0);
            Add("ProjectState", x => x.ProjectState, 0);
            Add("ProjectJurisdiction", x => x.ProjectJurisdiction, 0);
            Add("ProjectWatershed", x => x.ProjectWatershed, 0);
            Add("ProjectSummaryUrl", x => x.ProjectSummaryUrl, 0);
            Add("ProjectFactSheetUrl", x => x.ProjectFactSheetUrl, 0);
        }
    }
}
