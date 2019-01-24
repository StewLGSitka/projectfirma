﻿/*-----------------------------------------------------------------------
<copyright file="EditProjectLocationSimpleViewData.cs" company="Tahoe Regional Planning Agency and Sitka Technology Group">
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
using System.Linq;
using GeoJSON.Net.Feature;
using ProjectFirma.Web.Models;
using ProjectFirmaModels.Models;

namespace ProjectFirma.Web.Views.Shared.ProjectLocationControls
{
    public class ProjectLocationSimpleViewData : FirmaViewData
    {
        public readonly ProjectLocationSimpleViewDataForAngular ViewDataForAngular;
        public readonly string MapFormID;
        public readonly string MapPostUrl;

        public ProjectLocationSimpleViewData(Person currentPerson, MapInitJson mapInitJson, List<GeospatialAreaType> geospatialAreaTypes, Feature currentFeature, string mapPostUrl, string mapFormID)
            : base(currentPerson)
        {
            ViewDataForAngular = new ProjectLocationSimpleViewDataForAngular(mapInitJson, geospatialAreaTypes, currentFeature);
            MapPostUrl = mapPostUrl;
            MapFormID = mapFormID;
        }
    }

    public class ProjectLocationSimpleViewDataForAngular
    {
        public readonly MapInitJson MapInitJson;
        public readonly string TypeAheadInputId;
        public readonly string ProjectLocationFieldDefinitionLabel;
        public readonly List<string> GeospatialAreaMapSericeLayerNames;
        public readonly string MapServiceUrl;
        public readonly Feature CurrentFeature;

        public ProjectLocationSimpleViewDataForAngular(MapInitJson mapInitJson, List<GeospatialAreaType> geospatialAreaTypes, Feature currentFeature)
        {
            MapInitJson = mapInitJson;
            TypeAheadInputId = "projectLocationSearch";
            ProjectLocationFieldDefinitionLabel = FieldDefinitionEnum.ProjectLocation.ToType().GetFieldDefinitionLabel();
            GeospatialAreaMapSericeLayerNames = geospatialAreaTypes.Select(x => x.GeospatialAreaLayerName).ToList();
            MapServiceUrl = geospatialAreaTypes.FirstOrDefault()?.MapServiceUrl;
            CurrentFeature = currentFeature;
        }
    }
}