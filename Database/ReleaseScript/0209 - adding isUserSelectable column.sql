alter table dbo.MeasurementUnitType add IsUserSelectable bit;
alter table dbo.PerformanceMeasureType add IsUserSelectable bit;
alter table dbo.PerformanceMeasureDataSourceType add IsUserSelectable bit;
go

update dbo.MeasurementUnitType set IsUserSelectable = 1;
update dbo.PerformanceMeasureType set IsUserSelectable = 1;
update dbo.PerformanceMeasureDataSourceType set IsUserSelectable = 1;


alter table dbo.MeasurementUnitType alter column IsUserSelectable bit not null;
alter table dbo.PerformanceMeasureType alter column IsUserSelectable bit not null;
alter table dbo.PerformanceMeasureDataSourceType alter column IsUserSelectable bit not null;
GO

INSERT INTO dbo.PerformanceMeasureDataSourceType(PerformanceMeasureDataSourceTypeID, PerformanceMeasureDataSourceTypeName, PerformanceMeasureDataSourceTypeDisplayName, IsCustomCalculation, IsUserSelectable) values 
(3, 'Not Applicable', 'Not Applicable', 1, 0)


insert into dbo.PerformanceMeasureType(PerformanceMeasureTypeID, PerformanceMeasureTypeName, PerformanceMeasureTypeDisplayName, IsUserSelectable) values 
(3, 'Not Applicable', 'Not Applicable', 0)

insert into dbo.MeasurementUnitType(MeasurementUnitTypeID, MeasurementUnitTypeName, MeasurementUnitTypeDisplayName, LegendDisplayName, SingularDisplayName, NumberOfSignificantDigits, IsUserSelectable) values 
(24, 'Not Applicable', 'Not Applicable', 'Not Applicable', 'Not Applicable', 0, 0)