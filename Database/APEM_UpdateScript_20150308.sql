USE [SweetSoft_APEM]
--ALTER TABLE
GO
ALTER TABLE tblJobQuotationPricing
ALTER COLUMN PricingID int

GO
ALTER TABLE tblJobQuotation
ADD QuotationDate datetime NULL
GO
Update tblJobQuotation set QuotationDate = GETDATE();



-------------------------------------------------------------------------------------------------------------
--DROP TABLE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblEngravingDetail_tblCylinder]') AND parent_object_id = OBJECT_ID(N'[dbo].[tblEngravingDetail]'))
ALTER TABLE [dbo].[tblEngravingDetail] DROP CONSTRAINT [FK_tblEngravingDetail_tblCylinder]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblEngravingDetail_tblEngraving]') AND parent_object_id = OBJECT_ID(N'[dbo].[tblEngravingDetail]'))
ALTER TABLE [dbo].[tblEngravingDetail] DROP CONSTRAINT [FK_tblEngravingDetail_tblEngraving]
GO

USE [SweetSoft_APEM]
GO

/****** Object:  Table [dbo].[tblEngravingDetail]    Script Date: 03/09/2015 03:19:37 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblEngravingDetail]') AND type in (N'U'))
DROP TABLE [dbo].[tblEngravingDetail]
GO

-------------------------------------------------------------------------------------------------------------
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblEngraving_tblJob]') AND parent_object_id = OBJECT_ID(N'[dbo].[tblEngraving]'))
ALTER TABLE [dbo].[tblEngraving] DROP CONSTRAINT [FK_tblEngraving_tblJob]
GO

USE [SweetSoft_APEM]
GO

/****** Object:  Table [dbo].[tblEngraving]    Script Date: 03/09/2015 03:19:49 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblEngraving]') AND type in (N'U'))
DROP TABLE [dbo].[tblEngraving]
GO




-------------------------------------------------------------------------------------------------------------
--CREATE TABLE

GO

/****** Object:  Table [dbo].[tblEngraving]    Script Date: 03/09/2015 03:16:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblEngraving](
	[JobID] [int] NOT NULL,
	[EngravingStart] [float] NULL,
	[EngravingWidth] [float] NULL,
	[JobCoOrd] [nvarchar](100) NULL,
	[EngravingOnNut] [tinyint] NULL,
	[EngravingOnBoader] [tinyint] NULL,
	[ChromeThickness] [nvarchar](20) NULL,
	[Roughness] [nvarchar](50) NULL,
	[MotherSet] [nvarchar](20) NULL,
	[LaserStart] [nvarchar](50) NULL,
	[UnitSizeV] [float] NULL,
	[UnitSizeH] [float] NULL,
	[LaserOperator] [nvarchar](200) NULL,
	[FinalControl] [nvarchar](2000) NULL,
	[SRRemark] [nvarchar](1000) NULL,
	[CreatedBy] [nvarchar](100) NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedOn] [datetime] NULL,
 CONSTRAINT [PK_tblEngraving_1] PRIMARY KEY CLUSTERED 
(
	[JobID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tblEngraving]  WITH CHECK ADD  CONSTRAINT [FK_tblEngraving_tblJob] FOREIGN KEY([JobID])
REFERENCES [dbo].[tblJob] ([JobID])
GO

ALTER TABLE [dbo].[tblEngraving] CHECK CONSTRAINT [FK_tblEngraving_tblJob]
GO
-------------------------------------------------------------------------------------------------------------
GO

/****** Object:  Table [dbo].[tblEngravingDetail]    Script Date: 03/09/2015 03:17:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblEngravingDetail](
	[EngravingID] [int] IDENTITY(1,1) NOT NULL,
	[CylinderID] [int] NOT NULL,
	[Sequence] [int] NOT NULL,
	[JobID] [int] NOT NULL,
	[Stylus] [int] NULL,
	[Screen] [nvarchar](20) NULL,
	[Angle] [int] NULL,
	[Wall] [int] NULL,
	[Gamma] [int] NULL,
	[Sh] [int] NULL,
	[Hl] [int] NULL,
	[Ch] [int] NULL,
	[Mt] [int] NULL,
	[CopperSh] [int] NULL,
	[ChromeSh] [int] NULL,
	[CopperCh] [int] NULL,
	[ChromeCh] [int] NULL,
	[CellDepth] [float] NULL,
	[IsCopy] [tinyint] NOT NULL,
 CONSTRAINT [PK_tblEngraving] PRIMARY KEY CLUSTERED 
(
	[EngravingID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tblEngravingDetail]  WITH CHECK ADD  CONSTRAINT [FK_tblEngravingDetail_tblCylinder] FOREIGN KEY([CylinderID])
REFERENCES [dbo].[tblCylinder] ([CylinderID])
GO

ALTER TABLE [dbo].[tblEngravingDetail] CHECK CONSTRAINT [FK_tblEngravingDetail_tblCylinder]
GO

ALTER TABLE [dbo].[tblEngravingDetail]  WITH CHECK ADD  CONSTRAINT [FK_tblEngravingDetail_tblEngraving] FOREIGN KEY([JobID])
REFERENCES [dbo].[tblEngraving] ([JobID])
GO

ALTER TABLE [dbo].[tblEngravingDetail] CHECK CONSTRAINT [FK_tblEngravingDetail_tblEngraving]
GO
-------------------------------------------------------------------------------------------------------------
GO

/****** Object:  Table [dbo].[tblEngravingTobacco]    Script Date: 03/09/2015 03:18:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblEngravingTobacco](
	[EngravingID] [int] IDENTITY(1,1) NOT NULL,
	[CylinderID] [int] NOT NULL,
	[Sequence] [int] NOT NULL,
	[JobID] [int] NOT NULL,
	[Stylus] [int] NULL,
	[Screen] [nvarchar](20) NULL,
	[MasterScreen] [tinyint] NULL,
	[Angle] [nvarchar](20) NULL,
	[Elongation] [nvarchar](20) NULL,
	[Distotion] [nvarchar](20) NULL,
	[Resolution] [nvarchar](20) NULL,
	[Hexagonal] [int] NULL,
	[ImageSmoothness] [tinyint] NULL,
	[UnsharpMasking] [nvarchar](20) NULL,
	[Antialiasing] [nvarchar](20) NULL,
	[LineworkWidening] [nvarchar](20) NULL,
	[EngravingStart] [nvarchar](20) NULL,
	[EngravingWidth] [nvarchar](20) NULL,
	[CellShape] [int] NULL,
	[Gradation] [int] NULL,
	[Gamma] [nvarchar](20) NULL,
	[Laser] [tinyint] NULL,
	[CellWidth] [nvarchar](20) NULL,
	[ChannelWidth] [nvarchar](20) NULL,
	[CellDepth] [nvarchar](20) NULL,
	[Beam] [nvarchar](20) NULL,
	[Threshold] [nvarchar](20) NULL,
	[CheckedBy] [nvarchar](200) NULL,
	[CheckedOn] [datetime] NULL,
	[IsCopy] [tinyint] NOT NULL,
 CONSTRAINT [PK_tblEngravingTobacco] PRIMARY KEY CLUSTERED 
(
	[EngravingID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
-------------------------------------------------------------------------------------------------------------
GO

/****** Object:  Table [dbo].[tblEngravingEtching]    Script Date: 03/09/2015 03:18:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblEngravingEtching](
	[EngravingID] [int] IDENTITY(1,1) NOT NULL,
	[CylinderID] [int] NOT NULL,
	[Sequence] [int] NOT NULL,
	[JobID] [int] NOT NULL,
	[ScreenLpi] [nvarchar](20) NULL,
	[CellType] [nvarchar](20) NULL,
	[Stylus] [int] NULL,
	[Screen] [nvarchar](20) NULL,
	[Angle] [float] NULL,
	[Gamma] [nvarchar](20) NULL,
	[TargetCellSize] [float] NULL,
	[TargetCellWall] [float] NULL,
	[TargetCellDepth] [float] NULL,
	[DevelopingTime] [int] NULL,
	[EtchingTime] [int] NULL,
	[ChromeCellSize] [float] NULL,
	[ChromeCellWall] [float] NULL,
	[ChromeCellDepth] [float] NULL,
	[IsCopy] [tinyint] NOT NULL,
 CONSTRAINT [PK_tblEngravingEtching] PRIMARY KEY CLUSTERED 
(
	[EngravingID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
-------------------------------------------------------------------------------------------------------------



-------------------------------------------------------------------------------------------------------------
--PROCEDURE
GO
/****** Object:  StoredProcedure [dbo].[tblEngravingDetail_SelectAll]    Script Date: 03/09/2015 00:20:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblEngravingDetail_SelectAll]
@JobID int
as
begin
	select ISNULL(ed.EngravingID, -1) as EngravingID, ISNULL(ed.Sequence, c.Sequence) Sequence, c.CylinderNo, 
			c.Color, ed.Stylus, ed.Screen, ed.Angle, ed.Wall, ed.Gamma, ed.Sh, ed.Hl, ed.Ch, ed.Mt, 
			ISNULL(ed.IsCopy, 0) as IsCopy, ed.CellDepth, ed.CopperSh, ed.CopperCh, ed.ChromeSh, ed.ChromeCh,
			c.CylinderID, r.Name as CylinderStatusName 
	from tblCylinder c LEFT OUTER JOIN
			tblReferences r ON c.ProcessTypeID = r.ReferencesID LEFT OUTER JOIN
			tblEngravingDetail ed ON c.CylinderID = ed.CylinderID
	where c.JobID = @JobID
	order by ISNULL(ed.Sequence, c.Sequence),ed.IsCopy
	--exec tblEngravingDetail_SelectAll 100
end

