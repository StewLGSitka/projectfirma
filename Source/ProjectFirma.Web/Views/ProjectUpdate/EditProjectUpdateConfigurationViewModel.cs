﻿/*-----------------------------------------------------------------------
<copyright file = "EditProjectUpdateConfigurationViewModel.cs" company="Tahoe Regional Planning Agency and Sitka Technology Group">
Copyright(c) Tahoe Regional Planning Agency and Sitka Technology Group.All rights reserved.
<author>Sitka Technology Group</author>
</copyright>

<license>
This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
GNU Affero General Public License<http://www.gnu.org/licenses/> for more details.

Source code is available upon request via<support@sitkatech.com>.
</license>
-----------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using LtInfo.Common.Models;
using LtInfo.Common.Mvc;
using ProjectFirma.Web.Models;

namespace ProjectFirma.Web.Views.ProjectUpdate
{
    public class EditProjectUpdateConfigurationViewModel : FormViewModel, IValidatableObject
    {
        [DisplayName("Kick-Off Date")] public DateTime? ProjectUpdateKickOffDate { get; set; }

        [DisplayName("Close-Out Date")] public DateTime? ProjectUpdateCloseOutDate { get; set; }

        [DisplayName("Reminder Interval (days)")]
        public int? ProjectUpdateReminderInterval { get; set; }

        [Required]
        [DisplayName("Enable Project Update Reminders?")]
        public bool? EnableProjectUpdateReminders { get; set; }

        [Required]
        [DisplayName("Send Periodic Reminders?")]
        public bool? SendPeriodicReminders { get; set; }

        [Required]
        [DisplayName("Send Close-Out Notification?")]
        public bool? SendCloseOutNotification { get; set; }

        [DisplayName("Project Update Kick-Off Email Content")]
        public string ProjectUpdateKickOffIntroContent { get; set; }

        [DisplayName("Project Update Reminder Email Content")]
        public string ProjectUpdateReminderIntroContent { get; set; }

        [DisplayName("Project Update Close-Out Email Content")]
        public string ProjectUpdateCloseOutIntroContent { get; set; }

        /// <summary>
        /// Needed by ModelBinder
        /// </summary>
        public EditProjectUpdateConfigurationViewModel()
        {
        }

        public EditProjectUpdateConfigurationViewModel(ProjectUpdateConfiguration projectUpdateConfiguration)
        {
            ProjectUpdateKickOffDate = projectUpdateConfiguration.ProjectUpdateKickOffDate;
            ProjectUpdateCloseOutDate = projectUpdateConfiguration.ProjectUpdateCloseOutDate;
            ProjectUpdateReminderInterval = projectUpdateConfiguration.ProjectUpdateReminderInterval;
            EnableProjectUpdateReminders = projectUpdateConfiguration.EnableProjectUpdateReminders;
            SendPeriodicReminders = projectUpdateConfiguration.SendPeriodicReminders;
            SendCloseOutNotification = projectUpdateConfiguration.SendCloseOutNotification;
            ProjectUpdateKickOffIntroContent = projectUpdateConfiguration.ProjectUpdateKickOffIntroContent;
            ProjectUpdateReminderIntroContent = projectUpdateConfiguration.ProjectUpdateReminderIntroContent;
            ProjectUpdateCloseOutIntroContent = projectUpdateConfiguration.ProjectUpdateCloseOutIntroContent;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // these bools will never be null due to RequiredAttribute
            if (EnableProjectUpdateReminders.GetValueOrDefault())
            {
                if (string.IsNullOrWhiteSpace(ProjectUpdateKickOffIntroContent))
                    yield return new ValidationResult(
                        "You must provide Project Update Kick-Off Email Content if Project Update Reminders are enabled.");
                if (!ProjectUpdateKickOffDate.HasValue)
                    yield return new ValidationResult(
                        "You must provide a Project Update Kick-Off Date if Project Update Remidners are enabled");
            }

            if (SendPeriodicReminders.GetValueOrDefault())
            {
                if (string.IsNullOrWhiteSpace(ProjectUpdateReminderIntroContent))
                    yield return new ValidationResult(
                        "You must provide Project Update Reminder Email Content if Periodic Reminders are enabled.");
                if (!ProjectUpdateReminderInterval.HasValue)
                    yield return new ValidationResult(
                        "You must provide a Project Update Reminder Interval if Periodic Remidners are enabled");
            }

            if (SendCloseOutNotification.GetValueOrDefault())
            {
                if (string.IsNullOrWhiteSpace(ProjectUpdateReminderIntroContent))
                    yield return new ValidationResult(
                        "You must provide Project Update Close-Out Email Content if Project Update Close-Out Notifications are enabled.");
                if (!ProjectUpdateCloseOutDate.HasValue)
                    yield return new ValidationResult(
                        "You must provide a Project Update Close-Out Date if Project Update Close-Out Notifications are enabled");
            }
        }
    }

    public class EditProjectUpdateConfigurationViewData : FirmaViewData
    {
        public EditProjectUpdateConfigurationViewData(Person currentPerson) : base(currentPerson)
        {
        }
    }

    public abstract class EditProjectUpdateConfiguration : TypedWebPartialViewPage<
        EditProjectUpdateConfigurationViewData,
        EditProjectUpdateConfigurationViewModel>
    {
    }
}