delete from dbo.PerformanceMeasureType

insert into dbo.PerformanceMeasureType(PerformanceMeasureTypeID, PerformanceMeasureTypeName, PerformanceMeasureTypeDisplayName, IsUserSelectable) values 
(1, 'Action', 'Action', 1),
(2, 'Outcome', 'Outcome', 1),
(3, 'Not Applicable', 'Not Applicable', 0)
