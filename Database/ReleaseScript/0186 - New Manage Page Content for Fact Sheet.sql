
INSERT INTO dbo.FirmaPageType (FirmaPageTypeID, FirmaPageTypeName, FirmaPageTypeDisplayName, FirmaPageRenderTypeID)
VALUES (54, 'FactSheetCustomText', 'Fact Sheet Custom Text', 2);



INSERT INTO dbo.FirmaPage (TenantID, FirmaPageTypeID, FirmaPageContent)
VALUES 
(1, 54, NULL),
(2, 54, NULL),
(3, 54, NULL),
(4, 54, NULL),
(5, 54, NULL),
(6, 54, NULL),
(7, 54, NULL),
(8, 54, NULL),
(9, 54, NULL);



