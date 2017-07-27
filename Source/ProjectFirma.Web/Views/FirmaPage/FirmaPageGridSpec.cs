﻿/*-----------------------------------------------------------------------
<copyright file="FirmaPageGridSpec.cs" company="Tahoe Regional Planning Agency">
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
using ProjectFirma.Web.Controllers;
using LtInfo.Common;
using LtInfo.Common.DhtmlWrappers;
using LtInfo.Common.ModalDialog;
using LtInfo.Common.Views;

namespace ProjectFirma.Web.Views.FirmaPage
{
    public class FirmaPageGridSpec : GridSpec<Models.FirmaPage>
    {
        public FirmaPageGridSpec(bool hasManagePermissions)
        {            
            if (hasManagePermissions)
            {
                Add(string.Empty, a => DhtmlxGridHtmlHelpers.MakeLtInfoEditIconAsModalDialogLinkBootstrap(new ModalDialogForm(SitkaRoute<FirmaPageController>.BuildUrlFromExpression(t => t.EditInDialog(a)),
                        $"Edit Intro Content for '{a.FirmaPageType.FirmaPageTypeDisplayName}'")),
                    30);
            }
            Add("Page Name", a => UrlTemplate.MakeHrefString(a.FirmaPageType.GetViewUrl(), a.FirmaPageType.FirmaPageTypeDisplayName), 180, DhtmlxGridColumnFilterType.Text);
            Add("Has Content", a => a.HasFirmaPageContent.ToYesNo(), 85, DhtmlxGridColumnFilterType.SelectFilterStrict);
            Add("Type", a => a.FirmaPageType.FirmaPageRenderType.FirmaPageRenderTypeDisplayName, 110, DhtmlxGridColumnFilterType.SelectFilterStrict);
            Add("FirmaPageID", a => a.FirmaPageID, 0);
        }
    }
}
