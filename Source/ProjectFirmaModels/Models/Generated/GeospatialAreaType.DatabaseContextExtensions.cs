//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[GeospatialAreaType]
using System.Collections.Generic;
using System.Linq;
using LtInfo.Common;
using LtInfo.Common.DesignByContract;
using LtInfo.Common.Models;

namespace ProjectFirmaModels.Models
{
    public static partial class DatabaseContextExtensions
    {
        public static GeospatialAreaType GetGeospatialAreaType(this IQueryable<GeospatialAreaType> geospatialAreaTypes, int geospatialAreaTypeID)
        {
            var geospatialAreaType = geospatialAreaTypes.SingleOrDefault(x => x.GeospatialAreaTypeID == geospatialAreaTypeID);
            Check.RequireNotNullThrowNotFound(geospatialAreaType, "GeospatialAreaType", geospatialAreaTypeID);
            return geospatialAreaType;
        }

    }
}