delete from dbo.PerformanceMeasureDataSourceType


INSERT INTO dbo.PerformanceMeasureDataSourceType(PerformanceMeasureDataSourceTypeID, PerformanceMeasureDataSourceTypeName, PerformanceMeasureDataSourceTypeDisplayName, IsCustomCalculation, IsUserSelectable) values 
(1, 'Project', 'Project', 0, 1),
(2, 'TechnicalAssistanceValue', 'Technical Assistance Value', 1, 1),
(3, 'Not Applicable', 'Not Applicable', 1, 0)