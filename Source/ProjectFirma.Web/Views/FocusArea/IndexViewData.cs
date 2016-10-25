﻿using ProjectFirma.Web.Controllers;
using ProjectFirma.Web.Security;
using ProjectFirma.Web.Models;
using LtInfo.Common;
using LtInfo.Common.ModalDialog;

namespace ProjectFirma.Web.Views.FocusArea
{
    public class IndexViewData : FirmaViewData
    {
        public readonly IndexGridSpec GridSpec;
        public readonly string GridName;
        public readonly string GridDataUrl;

        public IndexViewData(Person currentPerson, Models.FirmaPage firmaPage) : base(currentPerson, firmaPage)
        {
            PageTitle = "Focus Areas";

            var hasFocusAreaManagePermissions = new FocusAreaManageFeature().HasPermissionByPerson(currentPerson);
            GridSpec = new IndexGridSpec(hasFocusAreaManagePermissions) {ObjectNameSingular = "Focus Area", ObjectNamePlural = "Focus Areas", SaveFiltersInCookie = true};
            if (hasFocusAreaManagePermissions)
            {
                GridSpec.CreateEntityModalDialogForm = new ModalDialogForm(SitkaRoute<FocusAreaController>.BuildUrlFromExpression(t => t.New()), "Create a new Focus Area");
            }

            GridName = "focusAreasGrid";
            GridDataUrl = SitkaRoute<FocusAreaController>.BuildUrlFromExpression(tc => tc.IndexGridJsonData());
        }
    }
}