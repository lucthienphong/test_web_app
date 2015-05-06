USE [SweetSoft_APEM]

CREATE TABLE [dbo].[tblMachine](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Code] [nvarchar](max) NULL,
	[Performance] [nvarchar](50) NULL,
	[Maintenance] [nvarchar](50) NULL,
	[Manufacturer] [nvarchar](max) NULL,
	[DepartmentID] [smallint] NULL,
	[IsObsolete] [tinyint] NULL,
	[ProduceYear] [int] NULL,
 CONSTRAINT [PK_tblMachine] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


