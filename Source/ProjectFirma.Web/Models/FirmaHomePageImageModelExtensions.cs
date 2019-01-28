﻿using ProjectFirma.Web.Common;
using ProjectFirma.Web.Controllers;
using ProjectFirmaModels.Models;

namespace ProjectFirma.Web.Models
{
    public static class FirmaHomePageImageModelExtensions
    {
        public static string GetDeleteUrl(this FirmaHomePageImage firmaHomePageImage)
        {
            return SitkaRoute<FirmaHomePageImageController>.BuildUrlFromExpression(x =>
                x.DeleteFirmaHomePageImage(firmaHomePageImage.FirmaHomePageImageID));
        }

        public static string GetCaptionOnFullView(this FirmaHomePageImage firmaHomePageImage) => $"{firmaHomePageImage.GetCaptionOnGallery()}";

        public static string GetCaptionOnGallery(this FirmaHomePageImage firmaHomePageImage) => $"{firmaHomePageImage.Caption}\r\n{firmaHomePageImage.FileResource.GetFileResourceDataLengthString()}";

        public static string GetPhotoUrl(this FirmaHomePageImage firmaHomePageImage) => firmaHomePageImage.FileResource.GetFileResourceUrl();

        public static string GetPhotoUrlScaledThumbnail(this FirmaHomePageImage firmaHomePageImage) => firmaHomePageImage.FileResource.FileResourceUrlScaledThumbnail(150);

        public static string GetPhotoUrlScaledForPrint(this FirmaHomePageImage firmaHomePageImage) => firmaHomePageImage.FileResource.GetFileResourceUrlScaledForPrint();

        public static string GetEditUrl(this FirmaHomePageImage firmaHomePageImage)
        {
            return SitkaRoute<FirmaHomePageImageController>.BuildUrlFromExpression(x => x.Edit(firmaHomePageImage.FirmaHomePageImageID));
        }
    }
}