﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectFirma.Web.Security;
using ProjectFirma.Web.Common;
using ProjectFirma.Web.Models;
using ProjectFirma.Web.Security.Shared;
using ProjectFirma.Web.Views.Indicator;
using ProjectFirma.Web.Views.Shared.TextControls;
using LtInfo.Common;
using LtInfo.Common.Mvc;
using LtInfo.Common.MvcResults;
using Edit = ProjectFirma.Web.Views.Indicator.Edit;
using EditViewData = ProjectFirma.Web.Views.Indicator.EditViewData;
using EditViewModel = ProjectFirma.Web.Views.Indicator.EditViewModel;
using Summary = ProjectFirma.Web.Views.Indicator.Summary;
using SummaryViewData = ProjectFirma.Web.Views.Indicator.SummaryViewData;

namespace ProjectFirma.Web.Controllers
{
    public class IndicatorController : FirmaBaseController
    {
        [IndicatorManageFeature]
        public ViewResult Manage()
        {
            return IndexImpl();
        }


        [PerformanceMeasureViewFeature]
        public ViewResult Index()
        {
            return IndexImpl();
        }

        private ViewResult IndexImpl()
        {
            var firmaPage = FirmaPage.GetFirmaPageByPageType(FirmaPageType.WatershedsList);
            var viewData = new IndexViewData(CurrentPerson, firmaPage);
            return RazorView<Index, IndexViewData>(viewData);
        }

        [PerformanceMeasureViewFeature]
        public GridJsonNetJObjectResult<Indicator> IndicatorGridJsonData()
        {
            IndicatorGridSpec gridSpec;
            var indicators = GetIndicatorsAndGridSpec(out gridSpec);
            var gridJsonNetJObjectResult = new GridJsonNetJObjectResult<Indicator>(indicators, gridSpec);
            return gridJsonNetJObjectResult;
        }

        private static List<Indicator> GetIndicatorsAndGridSpec(out IndicatorGridSpec gridSpec)
        {
            gridSpec = new IndicatorGridSpec();
            return HttpRequestStorage.DatabaseEntities.Indicators.OrderBy(x => x.IndicatorID).ToList();
        }

        [PerformanceMeasureViewFeature]
        public ViewResult Summary(string indicatorName, SummaryViewData.IndicatorSummaryTab? indicatorSummaryTab)
        {
            var indicator = HttpRequestStorage.DatabaseEntities.Indicators.GetIndicatorByIndicatorName(indicatorName);
            var activeTab = indicatorSummaryTab ?? SummaryViewData.IndicatorSummaryTab.Overview;
            var userHasIndicatorManagePermissions = new IndicatorManageFeature().HasPermissionByPerson(CurrentPerson);
            var indicatorChartViewData = new IndicatorChartViewData(indicator, false, userHasIndicatorManagePermissions ? ChartViewMode.ManagementMode : ChartViewMode.Small, null);
            var entityNotesViewData = new EntityNotesViewData(EntityNote.CreateFromEntityNote(new List<IEntityNote>(indicator.IndicatorNotes)),
                SitkaRoute<IndicatorNoteController>.BuildUrlFromExpression(c => c.New(indicator.PrimaryKey)),
                indicator.IndicatorDisplayName,
                userHasIndicatorManagePermissions);

            var viewData = new SummaryViewData(CurrentPerson,
                indicator,
                activeTab,
                indicatorChartViewData,
                entityNotesViewData,
                userHasIndicatorManagePermissions);
            return RazorView<Summary, SummaryViewData>(viewData);
        }

        [HttpGet]
        [IndicatorManageFeature]
        public PartialViewResult Edit(IndicatorPrimaryKey indicatorPrimaryKey)
        {
            var indicator = indicatorPrimaryKey.EntityObject;
            var viewModel = new EditViewModel(indicator);
            return ViewEdit(viewModel, indicator);
        }

        [HttpPost]
        [IndicatorManageFeature]
        [AutomaticallyCallEntityFrameworkSaveChangesWhenModelValid]
        public ActionResult Edit(IndicatorPrimaryKey indicatorPrimaryKey, EditViewModel viewModel)
        {
            var indicator = indicatorPrimaryKey.EntityObject;
            if (!ModelState.IsValid)
            {
                return ViewEdit(viewModel, indicator);
            }
            viewModel.UpdateModel(indicator, CurrentPerson);
            return new ModalDialogFormJsonResult(indicator.GetSummaryUrl());
        }

        private PartialViewResult ViewEdit(EditViewModel viewModel, Indicator indicator)
        {
            var measurementUnitTypesAsSelectListItems = MeasurementUnitType.All.ToSelectList(x => x.MeasurementUnitTypeID.ToString(CultureInfo.InvariantCulture),
                x => x.MeasurementUnitTypeDisplayName);
            var indicatorTypesAsSelectListItems = IndicatorType.All.OrderBy(x => x.IndicatorTypeDisplayName).ToSelectList(x => x.IndicatorTypeID.ToString(CultureInfo.InvariantCulture),
                x => x.IndicatorTypeDisplayName);
            var viewData = new EditViewData(measurementUnitTypesAsSelectListItems, indicatorTypesAsSelectListItems);
            return RazorPartialView<Edit, EditViewData, EditViewModel>(viewData, viewModel);
        }

        [HttpGet]
        [IndicatorManageFeature]
        public PartialViewResult EditAccomplishmentsMetadata(IndicatorPrimaryKey indicatorPrimaryKey)
        {
            var indicator = indicatorPrimaryKey.EntityObject;
            var viewModel = new EditAccomplishmentsMetadataViewModel(indicator);
            return ViewEditAccomplishmentsMetadata(viewModel, indicator);
        }

        [HttpPost]
        [IndicatorManageFeature]
        [AutomaticallyCallEntityFrameworkSaveChangesWhenModelValid]
        public ActionResult EditAccomplishmentsMetadata(IndicatorPrimaryKey indicatorPrimaryKey, EditAccomplishmentsMetadataViewModel viewModel)
        {
            var indicator = indicatorPrimaryKey.EntityObject;

            if (!ModelState.IsValid)
            {
                return ViewEditAccomplishmentsMetadata(viewModel, indicator);
            }
            viewModel.UpdateModel(indicator, CurrentPerson);
            return new ModalDialogFormJsonResult();
        }

        private PartialViewResult ViewEditAccomplishmentsMetadata(EditAccomplishmentsMetadataViewModel viewModel, Indicator indicator)
        {
            var viewData = new EditAccomplishmentsMetadataViewData();
            return RazorPartialView<EditAccomplishmentsMetadata, EditAccomplishmentsMetadataViewData, EditAccomplishmentsMetadataViewModel>(viewData, viewModel);
        }

        [HttpGet]
        [IndicatorManageFeature]
        public PartialViewResult EditIndicatorRichText(IndicatorPrimaryKey indicatorPrimaryKey, EditRtfContent.IndicatorRichTextType indicatorRichTextType)
        {
            var indicator = indicatorPrimaryKey.EntityObject;
            HtmlString rtfContent;
            switch (indicatorRichTextType)
            {
                case EditRtfContent.IndicatorRichTextType.SimpleDescription:
                    rtfContent = indicator.IndicatorPublicDescriptionHtmlString;
                    break;
                case EditRtfContent.IndicatorRichTextType.AssociatedPrograms:
                    rtfContent = indicator.AssociatedProgramsHtmlString;
                    break;
                case EditRtfContent.IndicatorRichTextType.CriticalDefinitions:
                    rtfContent = indicator.PerformanceMeasure.CriticalDefinitionsHtmlString;
                    break;
                case EditRtfContent.IndicatorRichTextType.AccountingPeriodAndScale:
                    rtfContent = indicator.PerformanceMeasure.AccountingPeriodAndScaleHtmlString;
                    break;
                case EditRtfContent.IndicatorRichTextType.ProjectReporting:
                    rtfContent = indicator.PerformanceMeasure.ProjectReportingHtmlString;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(string.Format("Invalid Indicator Rich Text Content Type: '{0}'", indicatorRichTextType));
            }
            var viewModel = new EditRtfContentViewModel(rtfContent);
            return ViewEditGuidance(viewModel, indicatorRichTextType);
        }

        [HttpPost]
        [IndicatorManageFeature]
        [AutomaticallyCallEntityFrameworkSaveChangesWhenModelValid]
        public ActionResult EditIndicatorRichText(IndicatorPrimaryKey indicatorPrimaryKey, EditRtfContent.IndicatorRichTextType indicatorRichTextType, EditRtfContentViewModel viewModel)
        {
            var indicator = indicatorPrimaryKey.EntityObject;
            if (!ModelState.IsValid)
            {
                return ViewEditGuidance(viewModel, indicatorRichTextType);
            }
            viewModel.UpdateModel(indicator, indicatorRichTextType);
            return new ModalDialogFormJsonResult();
        }

        private PartialViewResult ViewEditGuidance(EditRtfContentViewModel viewModel, EditRtfContent.IndicatorRichTextType indicatorRichTextType)
        {
            EditRtfContentViewData viewData;
            switch (indicatorRichTextType)
            {
                case EditRtfContent.IndicatorRichTextType.SimpleDescription:
                case EditRtfContent.IndicatorRichTextType.AssociatedPrograms:
                case EditRtfContent.IndicatorRichTextType.CriticalDefinitions:
                case EditRtfContent.IndicatorRichTextType.AccountingPeriodAndScale:
                case EditRtfContent.IndicatorRichTextType.ProjectReporting:
                    viewData = new EditRtfContentViewData(CkEditorExtension.CkEditorToolbar.Minimal, null);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(string.Format("Unknown GuidanceType: {0}", indicatorRichTextType));
            }
            return RazorPartialView<EditRtfContent, EditRtfContentViewData, EditRtfContentViewModel>(viewData, viewModel);
        }

        [HttpGet]
        [IndicatorManageFeature]
        public ContentResult SaveChartConfiguration(IndicatorPrimaryKey indicatorPrimaryKey, int indicatorSubcategoryID)
        {
            return new ContentResult();
        }

        [HttpPost]
        [IndicatorManageFeature]
        [AutomaticallyCallEntityFrameworkSaveChangesWhenModelValid]
        public ActionResult SaveChartConfiguration(IndicatorPrimaryKey indicatorPrimaryKey, int indicatorSubcategoryID, GoogleChartConfigurationViewModel viewModel)
        {
            var indicator = indicatorPrimaryKey.EntityObject;

            if (!ModelState.IsValid)
            {
                SetErrorForDisplay("Unable to save chart configuration: Unsupported options.");
            }
            else
            {
                viewModel.UpdateModel(indicator, indicatorSubcategoryID);
            }
            return RedirectToAction(new SitkaRoute<IndicatorController>(x => x.Summary(indicator.IndicatorName, null)));
        }


        [PerformanceMeasureViewFeature]
        [CrossAreaRoute]
        public PartialViewResult DefinitionAndGuidance(IndicatorPrimaryKey indicatorPrimaryKey)
        {
            var indicator = indicatorPrimaryKey.EntityObject;
            var viewData = new DefinitionAndGuidanceViewData(indicator);
            return RazorPartialView<DefinitionAndGuidance, DefinitionAndGuidanceViewData>(viewData);
        }

        [HttpGet]
        [AnonymousUnclassifiedFeature]
        [CrossAreaRoute]
        public PartialViewResult IndicatorChartPopup(IndicatorPrimaryKey indicatorPrimaryKey)
        {
            var indicator = indicatorPrimaryKey.EntityObject;
            var accomplishmentsChartViewData = new IndicatorChartViewData(indicator, 1080 + 20, 630 + 20, false, ChartViewMode.Large, null);
            return RazorPartialView<IndicatorChartPopup, IndicatorChartViewData>(accomplishmentsChartViewData);
        }

    }
}