﻿function HookupCheckIfFormIsDirty(formSelector, submitButtonSelector, submitDisableNotification, navigationNotification)
{
    // Define default values for function parameters
    formSelector = Sitka.Methods.isUndefinedNullOrEmpty(formSelector) ? "form" : formSelector;
    submitButtonSelector = Sitka.Methods
        .isUndefinedNullOrEmpty(submitButtonSelector)
        ? ":submit"
        : submitButtonSelector;
    submitDisableNotification = Sitka.Methods.isUndefinedNullOrEmpty(submitDisableNotification)
        ? "You currently have unsaved changed. Save your changes or reload the page to submit this form."
        : formSelector;
    navigationNotification = Sitka.Methods.isUndefinedNullOrEmpty(navigationNotification)
        ? "You have unsaved changes."
        : navigationNotification;

    // This function encapsulates the comparison of the current state of a form to it's initial state
    var checkIfFormIsDirty = function($form)
    {
        var currentFormSerialized = $form.find(":not(.ignoreSerialization)").serialize();
        return currentFormSerialized !== $form.data("formSerialization");
    };

    var $formToCheckIfDirty = jQuery(formSelector);

    var $submitButton = jQuery(submitButtonSelector);
    $submitButton.data("cachedOnClick", $submitButton.attr("onclick"));

    // Handle any changes to form input so that submission is disabled while the form is unclean
    _.each($formToCheckIfDirty,
        function(formIndividual)
        {
            var $formIndividual = jQuery(formIndividual);
            $formIndividual.data("formSerialization", $formIndividual.find(":not(.ignoreSerialization)").serialize());

            var handler = function()
            {
                if (checkIfFormIsDirty($formIndividual))
                {
                    // Disable any submit buttons not already currently disabled                        
                    jQuery(submitButtonSelector + ":not(:disabled)")
                        .attr("disabled", true)
                        .attr("title", submitDisableNotification)
                        .addClass("disabledByDirtyCheck")
                        .attr('onclick', 'event.preventDefault()');
                }
                else
                {
                    // Re-enable only submit buttons that have been disabled by this feature
                    var $disabledSubmitButton = jQuery(submitButtonSelector + ".disabledByDirtyCheck");
                    $disabledSubmitButton
                        .attr("disabled", null)
                        .attr("title", null)
                        .attr("onclick", $disabledSubmitButton.data("cachedOnClick"));
                }
            };

            $formIndividual.on("input", handler);
            $formIndividual.find('input[type=radio], select').on('change', handler);
        });

    // Handle the case when the user navigates from the page
    jQuery(window)
        .on("beforeunload",
            function(event)
            {
                if (_.some($formToCheckIfDirty,
                    function(formIndividual) { return checkIfFormIsDirty(jQuery(formIndividual)); }))
                {
                    return navigationNotification;
                }
                event = null;
            });

    // Turn of navigation check when the form is being submitted
    $formToCheckIfDirty.on("submit", function(event) { jQuery(window).off("beforeunload"); });
}