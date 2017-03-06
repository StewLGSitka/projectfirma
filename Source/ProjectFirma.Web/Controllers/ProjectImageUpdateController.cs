﻿/*-----------------------------------------------------------------------
<copyright file="ProjectImageUpdateController.cs" company="Tahoe Regional Planning Agency">
Copyright (c) Tahoe Regional Planning Agency. All rights reserved.
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
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using ProjectFirma.Web.Security;
using ProjectFirma.Web.Models;
using ProjectFirma.Web.Views.ProjectImageUpdate;
using ProjectFirma.Web.Controllers;
using ProjectFirma.Web.Views.Shared;
using LtInfo.Common;
using LtInfo.Common.Mvc;
using LtInfo.Common.MvcResults;

namespace ProjectFirma.Web.Controllers
{
    public class ProjectImageUpdateController : FirmaBaseController
    {
        [HttpGet]
        [ProjectImageUpdateNewFeature]
        public PartialViewResult New(ProjectUpdateBatchPrimaryKey projectUpdateBatchPrimaryKey)
        {
            var projectUpdateBatch = projectUpdateBatchPrimaryKey.EntityObject;
            var viewModel = new NewViewModel();
            return ViewNew(projectUpdateBatch, viewModel);
        }

        private PartialViewResult ViewNew(ProjectUpdateBatch projectUpdateBatch, NewViewModel viewModel)
        {
            var projectImageTimings = ProjectImageTiming.All.ToSelectListWithEmptyFirstRow(x => x.ProjectImageTimingID.ToString(CultureInfo.InvariantCulture), x => x.ProjectImageTimingDisplayName);
            var viewData = new NewViewData(projectUpdateBatch, projectImageTimings);
            return RazorPartialView<New, NewViewData, NewViewModel>(viewData, viewModel);
        }

        [HttpPost]
        [ProjectImageUpdateNewFeature]
        [AutomaticallyCallEntityFrameworkSaveChangesWhenModelValid]
        public ActionResult New(ProjectUpdateBatchPrimaryKey projectUpdateBatchPrimaryKey, NewViewModel viewModel)
        {
            var projectUpdateBatch = projectUpdateBatchPrimaryKey.EntityObject;
            if (!ModelState.IsValid)
            {
                return ViewNew(projectUpdateBatch, viewModel);
            }
            var projectImageUpdate = new ProjectImageUpdate(projectUpdateBatch, true);
            viewModel.UpdateModel(projectImageUpdate, CurrentPerson);
            projectUpdateBatch.ProjectImageUpdates.Add(projectImageUpdate);
            projectUpdateBatch.IsPhotosUpdated = true;
            projectUpdateBatch.TickleLastUpdateDate(CurrentPerson);
            return new ModalDialogFormJsonResult();
        }

        [HttpGet]
        [ProjectImageUpdateEditFeature]
        public PartialViewResult Edit(ProjectImageUpdatePrimaryKey projectImageUpdatePrimaryKey)
        {
            var projectImageUpdate = projectImageUpdatePrimaryKey.EntityObject;
            var viewModel = new EditViewModel(projectImageUpdate);
            return ViewEdit(projectImageUpdate, viewModel);
        }

        private PartialViewResult ViewEdit(ProjectImageUpdate projectImageUpdate, EditViewModel viewModel)
        {
            var projectImageTimings = ProjectImageTiming.All.ToSelectListWithEmptyFirstRow(x => x.ProjectImageTimingID.ToString(CultureInfo.InvariantCulture), x => x.ProjectImageTimingDisplayName);
            var viewData = new EditViewData(projectImageUpdate, projectImageTimings);
            return RazorPartialView<Edit, EditViewData, EditViewModel>(viewData, viewModel);
        }

        [HttpPost]
        [ProjectImageUpdateEditFeature]
        [AutomaticallyCallEntityFrameworkSaveChangesWhenModelValid]
        public ActionResult Edit(ProjectImageUpdatePrimaryKey projectImageUpdatePrimaryKey, EditViewModel viewModel)
        {
            var projectImageUpdate = projectImageUpdatePrimaryKey.EntityObject;
            if (!ModelState.IsValid)
            {
                return ViewEdit(projectImageUpdate, viewModel);
            }
            viewModel.UpdateModel(projectImageUpdate, CurrentPerson);
            var projectUpdateBatch = projectImageUpdate.ProjectUpdateBatch;
            projectUpdateBatch.IsPhotosUpdated = true;
            projectUpdateBatch.TickleLastUpdateDate(CurrentPerson);
            return new ModalDialogFormJsonResult();
        }

        [HttpGet]
        [ProjectImageUpdateEditFeature]
        public PartialViewResult DeleteProjectImageUpdate(ProjectImageUpdatePrimaryKey projectImageUpdatePrimaryKey)
        {
            var projectImageUpdate = projectImageUpdatePrimaryKey.EntityObject;
            var viewModel = new ConfirmDialogFormViewModel(projectImageUpdate.ProjectImageUpdateID);
            return ViewDeleteProjectImageUpdate(projectImageUpdate, viewModel);
        }

        private PartialViewResult ViewDeleteProjectImageUpdate(ProjectImageUpdate projectImageUpdate, ConfirmDialogFormViewModel viewModel)
        {
            var canDelete = !projectImageUpdate.HasDependentObjects();
            var confirmMessage = canDelete
                ? string.Format("Are you sure you want to flag this photo for deletion from Project '{0}'? ({1})", projectImageUpdate.ProjectUpdateBatch.Project.DisplayName, projectImageUpdate.Caption)
                : ConfirmDialogFormViewData.GetStandardCannotDeleteMessage("Project Image");

            var viewData = new ConfirmDialogFormViewData(confirmMessage, canDelete);
            return RazorPartialView<ConfirmDialogForm, ConfirmDialogFormViewData, ConfirmDialogFormViewModel>(viewData, viewModel);
        }

        [HttpPost]
        [ProjectImageUpdateEditFeature]
        [AutomaticallyCallEntityFrameworkSaveChangesWhenModelValid]
        public ActionResult DeleteProjectImageUpdate(ProjectImageUpdatePrimaryKey projectImageUpdatePrimaryKey, ConfirmDialogFormViewModel viewModel)
        {
            var projectImageUpdate = projectImageUpdatePrimaryKey.EntityObject;
            if (!ModelState.IsValid)
            {
                return ViewDeleteProjectImageUpdate(projectImageUpdate, viewModel);
            }
            ProjectUpdateBatch.DeleteProjectImageUpdates(new[] { projectImageUpdate });
            var projectUpdateBatch = projectImageUpdate.ProjectUpdateBatch;
            // reset key photo if needed
            if (projectImageUpdate.IsKeyPhoto)
            {
                var firstNonKeyPhoto = projectUpdateBatch.ProjectImageUpdates.FirstOrDefault(x => !x.IsKeyPhoto && x.ProjectImageUpdateID != projectImageUpdate.ProjectImageUpdateID);
                if (firstNonKeyPhoto != null)
                {
                    firstNonKeyPhoto.SetAsKeyPhoto(projectUpdateBatch.ProjectImageUpdates.Except(new[] { firstNonKeyPhoto, projectImageUpdate }).ToList());
                }
            }
            projectUpdateBatch.IsPhotosUpdated = true;
            projectUpdateBatch.TickleLastUpdateDate(CurrentPerson);
            return new ModalDialogFormJsonResult();
        }

        [HttpPost]
        [ProjectImageUpdateEditFeature]
        [AutomaticallyCallEntityFrameworkSaveChangesWhenModelValid]
        public ActionResult SetKeyPhoto(ProjectImageUpdatePrimaryKey projectImageUpdatePrimaryKey)
        {
            var projectImageUpdate = projectImageUpdatePrimaryKey.EntityObject;
            projectImageUpdate.SetAsKeyPhoto();
            projectImageUpdate.ProjectUpdateBatch.IsPhotosUpdated = true;
            projectImageUpdate.ProjectUpdateBatch.TickleLastUpdateDate(CurrentPerson);
            return new ModalDialogFormJsonResult();
        }
    }
}
