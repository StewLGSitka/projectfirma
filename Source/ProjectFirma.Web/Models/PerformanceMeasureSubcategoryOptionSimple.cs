﻿/*-----------------------------------------------------------------------
<copyright file="PerformanceMeasureSubcategoryOptionSimple.cs" company="Tahoe Regional Planning Agency">
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
namespace ProjectFirma.Web.Models
{
    public class PerformanceMeasureSubcategoryOptionSimple
    {
        /// <summary>
        /// Needed by ModelBinder
        /// </summary>
        public PerformanceMeasureSubcategoryOptionSimple()
        {
        }

        /// <summary>
        /// Constructor for building a new object with MaximalConstructor required fields in preparation for insert into database
        /// </summary>
        public PerformanceMeasureSubcategoryOptionSimple(int performanceMeasureSubcategoryOptionID, int performanceMeasureSubcategoryID, string performanceMeasureSubcategoryOptionName, int? sortOrder, string shortName)
            : this()
        {
            PerformanceMeasureSubcategoryOptionID = performanceMeasureSubcategoryOptionID;
            PerformanceMeasureSubcategoryID = performanceMeasureSubcategoryID;
            PerformanceMeasureSubcategoryOptionName = performanceMeasureSubcategoryOptionName;
            SortOrder = sortOrder;
            ShortName = shortName;
            HasAssociatedActuals = false;
        }

        /// <summary>
        /// Constructor for building a new simple object with the POCO class
        /// </summary>
        public PerformanceMeasureSubcategoryOptionSimple(PerformanceMeasureSubcategoryOption performanceMeasureSubcategoryOption)
            : this()
        {
            PerformanceMeasureSubcategoryOptionID = performanceMeasureSubcategoryOption.PerformanceMeasureSubcategoryOptionID;
            PerformanceMeasureSubcategoryID = performanceMeasureSubcategoryOption.PerformanceMeasureSubcategoryID;
            PerformanceMeasureSubcategoryOptionName = performanceMeasureSubcategoryOption.PerformanceMeasureSubcategoryOptionName;
            SortOrder = performanceMeasureSubcategoryOption.SortOrder;
            ShortName = performanceMeasureSubcategoryOption.ShortName;
            HasAssociatedActuals = performanceMeasureSubcategoryOption.HasDependentObjects();
        }

        public int PerformanceMeasureSubcategoryOptionID { get; set; }
        public int PerformanceMeasureSubcategoryID { get; set; }
        public string PerformanceMeasureSubcategoryOptionName { get; set; }
        public int? SortOrder { get; set; }
        public string ShortName { get; set; }
        public bool HasAssociatedActuals { get; set; }
    }
}
