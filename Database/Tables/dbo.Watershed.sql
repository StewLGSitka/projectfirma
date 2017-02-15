SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Watershed](
	[WatershedID] [int] IDENTITY(1,1) NOT NULL,
	[WatershedName] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[WatershedFeature] [geometry] NULL,
	[TenantID] [int] NOT NULL,
 CONSTRAINT [PK_Watershed_WatershedID] PRIMARY KEY CLUSTERED 
(
	[WatershedID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [AK_Watershed_WatershedID_TenantID] UNIQUE NONCLUSTERED 
(
	[WatershedID] ASC,
	[TenantID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [AK_Watershed_WatershedName] UNIQUE NONCLUSTERED 
(
	[WatershedName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[Watershed]  WITH CHECK ADD  CONSTRAINT [FK_Watershed_Tenant_TenantID] FOREIGN KEY([TenantID])
REFERENCES [dbo].[Tenant] ([TenantID])
GO
ALTER TABLE [dbo].[Watershed] CHECK CONSTRAINT [FK_Watershed_Tenant_TenantID]