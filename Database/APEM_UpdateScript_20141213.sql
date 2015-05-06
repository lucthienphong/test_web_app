USE [SweetSoft_APEM]
GO
IF (EXISTS (SELECT *  FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'TheSchema' 
                 AND  TABLE_NAME = 'tblOrder'))
BEGIN    
	DROP table tblOrder
END

GO

/****** Object:  Table [dbo].[tblDeliveryOrder]    Script Date: 12/13/2014 09:34:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tblDeliveryOrder](
	[JobID] [int] NOT NULL,
	[DONumber] [varchar](50) NOT NULL,
	[CustomerPO1] [nvarchar](50) NULL,
	[CustomerPO2] [nvarchar](50) NULL,
	[ContactPersonID] [int] NOT NULL,
	[OrderDate] [datetime] NOT NULL,
	[OtherItem] [nvarchar](1000) NULL,
 CONSTRAINT [PK_tblDeliveryOrder] PRIMARY KEY CLUSTERED 
(
	[JobID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[tblDeliveryOrder]  WITH CHECK ADD  CONSTRAINT [FK_tblDeliveryOrder_tblJob] FOREIGN KEY([JobID])
REFERENCES [dbo].[tblJob] ([JobID])
GO

ALTER TABLE [dbo].[tblDeliveryOrder] CHECK CONSTRAINT [FK_tblDeliveryOrder_tblJob]
GO


GO

/****** Object:  Table [dbo].[tblOrderConfirmation]    Script Date: 12/13/2014 09:35:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tblOrderConfirmation](
	[JobID] [int] NOT NULL,
	[OCNumber] [varchar](50) NOT NULL,
	[CustomerPO1] [nvarchar](50) NULL,
	[CustomerPO2] [nvarchar](50) NULL,
	[ContactPersonID] [int] NOT NULL,
	[OrderDate] [datetime] NOT NULL,
	[TaxID] [smallint] NULL,
	[TaxPercentage] [float] NULL,
	[CurrencyID] [smallint] NOT NULL,
	[RMValue] [decimal](18, 3) NOT NULL,
	[CurrencyValue] [decimal](18, 3) NOT NULL,
	[Discount] [float] NULL,
	[Remark] [nvarchar](2000) NULL,
	[RemarkScreen] [nvarchar](2000) NULL,
	[DeliveryTerm] [nvarchar](100) NULL,
	[PaymentTerm] [nvarchar](100) NULL,
 CONSTRAINT [PK_tblOrderConfirmation] PRIMARY KEY CLUSTERED 
(
	[JobID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[tblOrderConfirmation]  WITH CHECK ADD  CONSTRAINT [FK_tblOrderConfirmation_tblCurrency] FOREIGN KEY([CurrencyID])
REFERENCES [dbo].[tblCurrency] ([CurrencyID])
GO

ALTER TABLE [dbo].[tblOrderConfirmation] CHECK CONSTRAINT [FK_tblOrderConfirmation_tblCurrency]
GO

ALTER TABLE [dbo].[tblOrderConfirmation]  WITH CHECK ADD  CONSTRAINT [FK_tblOrderConfirmation_tblJob] FOREIGN KEY([JobID])
REFERENCES [dbo].[tblJob] ([JobID])
GO

ALTER TABLE [dbo].[tblOrderConfirmation] CHECK CONSTRAINT [FK_tblOrderConfirmation_tblJob]
GO


GO

/****** Object:  Table [dbo].[tblOtherCharges]    Script Date: 12/13/2014 09:35:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblOtherCharges](
	[OtherChargesID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](200) NULL,
	[Charge] [decimal](18, 3) NULL,
	[JobID] [int] NULL,
 CONSTRAINT [PK_tblOtherCharges] PRIMARY KEY CLUSTERED 
(
	[OtherChargesID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tblOtherCharges]  WITH CHECK ADD  CONSTRAINT [FK_tblOtherCharges_tblOrderConfirmation] FOREIGN KEY([JobID])
REFERENCES [dbo].[tblOrderConfirmation] ([JobID])
GO

ALTER TABLE [dbo].[tblOtherCharges] CHECK CONSTRAINT [FK_tblOtherCharges_tblOrderConfirmation]
GO
