﻿/*-----------------------------------------------------------------------
<copyright file="EditViewModel.cs" company="Tahoe Regional Planning Agency">
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
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ProjectFirma.Web.Models;
using LtInfo.Common.Models;

namespace ProjectFirma.Web.Views.Snapshot
{
    public class EditViewModel : FormViewModel
    {
        [Required]
        public int SnapshotID { get; set; }

        [DisplayName("Snapshot Note")]
        public string SnapshotNote { get; set; }

        public EditViewModel() {}

        public EditViewModel(Models.Snapshot snapshot)
        {
            SnapshotID = snapshot.SnapshotID;
            SnapshotNote = snapshot.SnapshotNote;
        }

        public void UpdateModel(Models.Snapshot snapshot, Person currentPerson)
        {
            snapshot.SnapshotNote = SnapshotNote;
        }
    }
}
