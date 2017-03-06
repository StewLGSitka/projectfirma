﻿/*-----------------------------------------------------------------------
<copyright file="FirmaHelpers.cs" company="Tahoe Regional Planning Agency">
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
using System;
using System.Collections.Generic;
using System.Web;
using ProjectFirma.Web.Controllers;
using LtInfo.Common;

namespace ProjectFirma.Web.Common
{
    public static class FirmaHelpers
    {
        public static readonly List<string> DefaultColorRange = new List<string> {
            "#1f77b4",
            "#ff7f0e",
            "#aec7e8",
            "#ffbb78",
            "#2ca02c",
            "#98df8a",
            "#d62728",
            "#ff9896",
            "#9467bd",
            "#c5b0d5",
            "#022c99",
            "#507e3c",
            "#0d5875",
            "#37aba8",
            "#44cc44",
            "#afa5f2",
            "#d3ffce",
            "#070a41"
        };

        public static string GenerateLogInUrlWithReturnUrl()
        {
            var logInUrl = SitkaRoute<AccountController>.BuildUrlFromExpression(c => c.LogOn());

            var returnUrl = HttpContext.Current.Request.Url.AbsoluteUri;

            return OnErrorOrNotFoundPage(returnUrl) ? logInUrl : string.Format("{0}?returnUrl={1}", logInUrl, HttpUtility.UrlEncode(returnUrl));
        }

        public static string GenerateLogOutUrlWithReturnUrl()
        {
            var logOutUrl = SitkaRoute<AccountController>.BuildAbsoluteUrlHttpsFromExpression(c => c.LogOff());
            
            var returnUrl = HttpContext.Current.Request.Url.AbsoluteUri;

            return OnErrorOrNotFoundPage(returnUrl) ? logOutUrl : String.Format("{0}?returnUrl={1}", logOutUrl, HttpUtility.UrlEncode(returnUrl));
        }

        private static bool OnErrorOrNotFoundPage(string url)
        {
            var returnUrlPathAndQuery = new Uri(url).PathAndQuery;
            var notFoundUrlPathAndQuery = new Uri(SitkaRoute<HomeController>.BuildAbsoluteUrlHttpsFromExpression(x => x.NotFound())).PathAndQuery;
            var errorUrlPathAndQuery = new Uri(SitkaRoute<HomeController>.BuildAbsoluteUrlHttpsFromExpression(x => x.Error())).PathAndQuery;

            var onErrorOrNotFoundPage = returnUrlPathAndQuery.StartsWith(notFoundUrlPathAndQuery, StringComparison.InvariantCultureIgnoreCase) ||
                                        returnUrlPathAndQuery.StartsWith(errorUrlPathAndQuery, StringComparison.InvariantCultureIgnoreCase);
            return onErrorOrNotFoundPage;
        }
    }
}
