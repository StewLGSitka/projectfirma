delete from dbo.ProjectUpdateSection

insert into dbo.ProjectUpdateSection (ProjectUpdateSectionID, ProjectUpdateSectionName, ProjectUpdateSectionDisplayName, SortOrder, HasCompletionStatus, ProjectWorkflowSectionGroupingID)
values
(2, 'Basics', 'Basics', 20, 1, 1),
(3, 'LocationSimple', 'Simple Location', 30, 1, 1),
(4, 'Organizations', 'Organizations', 40, 1, 1),
(5, 'LocationDetailed', 'Detailed Location', 50, 0, 2),
(6, 'PerformanceMeasures', 'Performance Measures', 60, 1, 3),
(7, 'ExpectedFunding', 'Expected Funding', 70, 0, 4),
(8, 'Expenditures', 'Expenditures', 80, 1, 4),
(9, 'Photos', 'Photos', 100, 0, 5),
(10, 'ExternalLinks', 'External Links', 125, 0, 5),
(11, 'NotesAndDocuments', 'Documents and Notes', 120, 0, 5)

