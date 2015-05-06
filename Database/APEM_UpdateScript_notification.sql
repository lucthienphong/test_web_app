USE [SweetSoft_APEM]
GO

/****** Object:  Table [dbo].[tblNotification]    Script Date: 12/31/2014 16:04:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblNotification]') AND type in (N'U'))
DROP TABLE [dbo].[tblNotification]
GO

/****** Object:  Table [dbo].[tblNotificationSetting]    Script Date: 12/31/2014 16:04:23 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblNotificationSetting]') AND type in (N'U'))
DROP TABLE [dbo].[tblNotificationSetting]
GO

USE [SweetSoft_APEM]
GO

/****** Object:  Table [dbo].[tblNotification]    Script Date: 12/31/2014 16:04:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblNotification](
	[NotificationID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) NULL,
	[Contents] [nvarchar](max) NULL,
	[IsObsolete] [bit] NOT NULL,
	[ReceiveIds] [nvarchar](max) NULL,
	[ReceiveType] [nvarchar](50) NULL,
	[Actions] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[DateDismiss] [nvarchar](max) NULL,
	[DismissBy] [nvarchar](max) NULL,
	[DismissEvent] [nvarchar](50) NULL,
	[PageId] [nvarchar](250) NULL,
	[CommandType] [nvarchar](50) NULL,
 CONSTRAINT [PK_tblNotification] PRIMARY KEY CLUSTERED 
(
	[NotificationID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [SweetSoft_APEM]
GO

/****** Object:  Table [dbo].[tblNotificationSetting]    Script Date: 12/31/2014 16:04:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblNotificationSetting](
	[SettingId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) NULL,
	[Description] [nvarchar](max) NULL,
	[PageId] [nvarchar](250) NULL,
	[Actions] [nvarchar](max) NULL,
	[TriggerButton] [nvarchar](500) NULL,
	[IsObsolete] [bit] NULL,
	[CommandType] [nvarchar](50) NULL,
	[DismissEvent] [nvarchar](50) NULL,
	[ReceiveIds] [nvarchar](max) NULL,
	[ReceiveType] [nvarchar](50) NULL,
 CONSTRAINT [PK_tblNotificationSetting] PRIMARY KEY CLUSTERED 
(
	[SettingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


insert into tblFunction values('notification_setting','fSystemConfig','Notification Setting','Notification Setting','5')
INSERT INTO [tblRolePermission]
           ([RoleID]
           ,[FunctionID]
           ,[AllowAdd]
           ,[AllowEdit]
           ,[AllowDelete]
           ,[AllowUpdateStatus]
           ,[AllowOther])
     VALUES
           (1
           ,'notification_setting'
           ,1
           ,1
           ,1
           ,1
           ,1)
GO


