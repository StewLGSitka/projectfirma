﻿/*-----------------------------------------------------------------------
<copyright file="PerformanceMeasureActualSimple.cs" company="Tahoe Regional Planning Agency">
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

namespace ProjectFirma.Web.Models
{
    public class PerformanceMeasureActualSimple
    {
        public int? PerformanceMeasureActualID { get; set; }        
        [Required]
        public int? ProjectID { get; set; }
        [Required]
        public int? PerformanceMeasureID { get; set; }
        [Required]
        [DisplayName("Calendar Year")]
        public int? CalendarYear { get; set; }
        [Required]
        [DisplayName("Reported Value")]
        public double? ActualValue { get; set; }
        public List<PerformanceMeasureActualSubcategoryOptionSimple> PerformanceMeasureActualSubcategoryOptions { get; set; }

        /// <summary>
        /// Needed by ModelBinder
        /// </summary>
        public PerformanceMeasureActualSimple()
        {
        }

        /// <summary>
        /// Constructor for building a new object with MaximalConstructor required fields in preparation for insert into database
        /// </summary>
        public PerformanceMeasureActualSimple(int performanceMeasureActualID, int projectID, int performanceMeasureID, int calendarYear, double actualValue)
            : this()
        {
            PerformanceMeasureActualID = performanceMeasureActualID;
            ProjectID = projectID;
            PerformanceMeasureID = performanceMeasureID;
            CalendarYear = calendarYear;
            ActualValue = actualValue;
        }

        /// <summary>
        /// Constructor for building a new simple object with the POCO class
        /// </summary>
        public PerformanceMeasureActualSimple(PerformanceMeasureActual performanceMeasureActual)
            : this()
        {
            PerformanceMeasureActualID = performanceMeasureActual.PerformanceMeasureActualID;
            ProjectID = performanceMeasureActual.ProjectID;
            PerformanceMeasureID = performanceMeasureActual.PerformanceMeasureID;
            CalendarYear = performanceMeasureActual.CalendarYear;
            ActualValue = performanceMeasureActual.ActualValue;
            PerformanceMeasureActualSubcategoryOptions = PerformanceMeasureValueSubcategoryOption.GetAllPossibleSubcategoryOptions(performanceMeasureActual);
        }        
    }
}
