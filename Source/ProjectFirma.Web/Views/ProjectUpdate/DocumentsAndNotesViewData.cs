﻿/*-----------------------------------------------------------------------
<copyright file="NotesViewData.cs" company="Tahoe Regional Planning Agency and Sitka Technology Group">
Copyright (c) Tahoe Regional Planning Agency and Sitka Technology Group. All rights reserved.
<author>Sitka Technology Group</author>
</copyright>

<license>
This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License <http://www.gnu.org/licenses/> for more details.

Source code is available upon request via <support@sitkatech.com>.
</license>
-----------------------------------------------------------------------*/
using System.Collections.Generic;
using LtInfo.Common.Mvc;
using ProjectFirma.Web.Controllers;
using ProjectFirmaModels.Models;
using ProjectFirma.Web.Views.Shared.TextControls;
using ProjectFirma.Web.Common;
using ProjectFirma.Web.Views.Shared.ProjectDocument;

namespace ProjectFirma.Web.Views.ProjectUpdate
{
    public class DocumentsAndNotesViewData : ProjectUpdateViewData
    {
        public EntityNotesViewData EntityNotesViewData { get; }
        public string RefreshUrl { get; }
        public string DiffUrl { get; }
        public ProjectDocumentsDetailViewData ProjectDocumentsViewData { get; }

        public DocumentsAndNotesViewData(Person currentPerson, ProjectUpdateBatch projectUpdateBatch, ProjectUpdateStatus projectUpdateStatus, string diffUrl) : base(currentPerson, projectUpdateBatch, projectUpdateStatus, new List<string>(), ProjectUpdateSection.NotesAndDocuments.ProjectUpdateSectionDisplayName)
        {
            EntityNotesViewData = new EntityNotesViewData(EntityNote.CreateFromEntityNote(new List<IEntityNote>(projectUpdateBatch.ProjectNoteUpdates)),
                SitkaRoute<ProjectNoteUpdateController>.BuildUrlFromExpression(x => x.New(projectUpdateBatch)),
                projectUpdateBatch.Project.GetDisplayName(),
                IsEditable);
            ProjectDocumentsViewData = new ProjectDocumentsDetailViewData(EntityDocument.CreateFromEntityDocument(new List<IEntityDocument>(projectUpdateBatch.ProjectDocumentUpdates)),
                SitkaRoute<ProjectDocumentUpdateController>.BuildUrlFromExpression(x => x.New(projectUpdateBatch)),
                projectUpdateBatch.Project.GetDisplayName(),
                IsEditable);
            RefreshUrl = SitkaRoute<ProjectUpdateController>.BuildUrlFromExpression(x => x.RefreshNotesAndDocuments(projectUpdateBatch.Project));
            DiffUrl = diffUrl;
        }

        //public EntityDocumentsViewData EntityDocumentsViewData { get; set; }
    }
}
