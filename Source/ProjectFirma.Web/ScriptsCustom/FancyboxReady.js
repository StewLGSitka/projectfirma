﻿jQuery(document).ready(function () {
    jQuery(".gallery").fancybox({
        type: "image",
        beforeShow: function () {
            if (!Sitka.Methods.isUndefinedNullOrEmpty(jQuery(this.element).data("caption")))
            {
                this.title = jQuery(this.element).data("caption");
            }
        }
    });
});