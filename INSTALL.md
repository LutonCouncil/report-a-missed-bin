# Create the LLPG database

EntityFramework is used to talk to SQL server and the ORM objects are based on a view called "vwFullExtract", which in
turn depends on a table called "FullExtract".

## Create FullExtract table:

```
/****** Object:  Table [dbo].[FullExtract]    Script Date: 19/05/2016 14:33:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FullExtract](
	[KEYVAL] [nvarchar](13) NOT NULL,
	[UPRN] [nvarchar](12) NOT NULL,
	[SAO_START_NO] [decimal](4, 0) NULL,
	[SAO_START_SFX] [nvarchar](1) NOT NULL,
	[SAO_END_NO] [decimal](4, 0) NULL,
	[SAO_END_SFX] [nvarchar](1) NOT NULL,
	[SAO_DESC] [nvarchar](90) NOT NULL,
	[SAON] [nvarchar](110) NOT NULL,
	[PAO_START_NO] [decimal](4, 0) NULL,
	[PAO_START_SFX] [nvarchar](1) NOT NULL,
	[PAO_END_NO] [decimal](4, 0) NULL,
	[PAO_END_SFX] [nvarchar](1) NOT NULL,
	[PAO_DESC] [nvarchar](90) NOT NULL,
	[PAON] [nvarchar](110) NOT NULL,
	[STREET_NAME] [nvarchar](100) NOT NULL,
	[LOCALITY_NAME] [nvarchar](40) NOT NULL,
	[POSTAL_TOWN] [nvarchar](40) NOT NULL,
	[POSTCODE] [nvarchar](8) NOT NULL,
	[MAP_EAST] [nvarchar](7) NOT NULL,
	[MAP_NORTH] [nvarchar](7) NOT NULL,
	[Primary] [nvarchar](1) NULL,
	[Secondary] [nvarchar](1) NULL,
	[Tertiary] [nvarchar](1) NULL,
	[ORGANISATION] [nvarchar](100) NOT NULL,
	[BLPU_LOGICAL_STATUS] [nvarchar](1) NOT NULL,
	[BLPU_STATE] [nvarchar](1) NULL,
	[LPI_LOGICAL_STATUS] [nvarchar](1) NOT NULL,
	[REL_POS_ACC] [nvarchar](7) NOT NULL,
	[BLPU_Created_Date] [datetime] NOT NULL,
	[BLPU_Updated_Date] [datetime] NOT NULL,
	[LPI_Created_Date] [datetime] NOT NULL,
	[LPI_Updated_Date] [datetime] NOT NULL
) ON [PRIMARY]

GO
```

## Create vwFullExtract view:

```
/****** Object:  View [dbo].[vwFullExtract]    Script Date: 19/05/2016 14:33:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vwFullExtract]
AS
SELECT        dbo.FullExtract.*
FROM            dbo.FullExtract

GO
```
