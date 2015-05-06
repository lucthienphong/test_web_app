USE [SweetSoft_APEM]
GO

/****** Object:  Table [dbo].[tblJobProcessing]    Script Date: 12/11/2014 16:08:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tblJobProcessing](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[JobID] [int] NULL,
	[Description] [varchar](1000) NULL,
	[CreatedBy] [varchar](100) NULL,
	[CreatedOn] [datetime] NULL,
	[FinishedBy] [varchar](100) NULL,
	[FinishedOn] [datetime] NULL,
	[DepartmentID] [int] NULL,
 CONSTRAINT [PK__tblOrder__3214EC071B48FEF0] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[tblJobProcessing] ADD  CONSTRAINT [DF__tblOrderF__IdOrd__1D314762]  DEFAULT (NULL) FOR [JobID]
GO

ALTER TABLE [dbo].[tblJobProcessing] ADD  CONSTRAINT [DF__tblOrderF__descr__1E256B9B]  DEFAULT (NULL) FOR [Description]
GO

ALTER TABLE [dbo].[tblJobProcessing] ADD  CONSTRAINT [DF__tblOrderF__Creat__1F198FD4]  DEFAULT (NULL) FOR [CreatedBy]
GO

ALTER TABLE [dbo].[tblJobProcessing] ADD  CONSTRAINT [DF__tblOrderF__Creat__200DB40D]  DEFAULT (NULL) FOR [CreatedOn]
GO


