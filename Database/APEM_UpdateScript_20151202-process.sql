USE [SweetSoft_APEM]
GO

drop table tblJobProcessing
go

/****** Object:  Table [dbo].[tblCylinderProcessing]    Script Date: 02/12/2015 09:11:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblCylinderProcessing](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CylinderID] [int] NULL,
	[Description] [nvarchar](1000) NULL,
	[CreatedBy] [nvarchar](100) NULL,
	[CreatedOn] [datetime] NULL,
	[FinishedBy] [nvarchar](100) NULL,
	[FinishedOn] [datetime] NULL,
	[DepartmentID] [smallint] NULL,
	[CylinderStatus] [nvarchar](50) NULL,
	[MachineID] [int] NULL,
 CONSTRAINT [PK_tblCylinderProcessing] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [SweetSoft_APEM]
GO

/****** Object:  Table [dbo].[tblJobProcess]    Script Date: 02/12/2015 09:11:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblJobProcess](
	[JobID] [int] NOT NULL,
	[StartedOn] [datetime] NULL,
	[StartedBy] [nvarchar](100) NULL,
	[FinishedOn] [datetime] NULL,
	[FinishedBy] [nvarchar](100) NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tblJobProcess]  WITH CHECK ADD  CONSTRAINT [FK_tblJobProcess_tblJob] FOREIGN KEY([JobID])
REFERENCES [dbo].[tblJob] ([JobID])
GO

ALTER TABLE [dbo].[tblJobProcess] CHECK CONSTRAINT [FK_tblJobProcess_tblJob]
GO


