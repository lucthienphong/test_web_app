USE [SweetSoft_APEM]
GO

/****** Object:  Table [dbo].[tblCustomerQuotation_AdditionalService]    Script Date: 03/23/2015 07:51:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblCustomerQuotation_AdditionalService](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[GLCode] [nvarchar](10) NOT NULL,
	[Description] [nvarchar](200) NOT NULL,
	[Price] [decimal](18, 3) NOT NULL,
	[CurrencyID] [smallint] NOT NULL,
	[CustomerID] [int] NOT NULL,
 CONSTRAINT [PK_tblCustomerQuotation_AdditionalService] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


GO

/****** Object:  Table [dbo].[tblCustomerQuotation_OtherCharges]    Script Date: 03/20/2015 14:54:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblCustomerQuotation_OtherCharges](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[GLCode] [nvarchar](10) NOT NULL,
	[Description] [nvarchar](200) NOT NULL,
	[Price] [decimal](18, 3) NOT NULL,
	[CurrencyID] [smallint] NOT NULL,
	[CustomerID] [int] NOT NULL,
 CONSTRAINT [PK_tblCustomerQuotation_OtherCharges] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  StoredProcedure [dbo].[tblCustomer_SelectForReport]    Script Date: 03/21/2015 09:37:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblCustomer_SelectForReport]
@KeyWord nvarchar(100),
@IsObsolete bit,
@SortColumn varchar(1),
@SortType varchar(1)
as
begin
	WITH T AS(
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN c.Code END ASC,
															CASE @SortColumn WHEN N'1' THEN c.Name END ASC,
															CASE @SortColumn WHEN N'2' THEN c.Address END ASC,
															CASE @SortColumn WHEN N'3' THEN c.Tel END ASC,
															CASE @SortColumn WHEN N'4' THEN c.Fax END ASC,
															CASE @SortColumn WHEN N'5' THEN c.IsObsolete END ASC) as RowIndex, 
				c.CustomerID, c.Code, c.Name, c.Address, c.Tel, c.Fax, c.PostCode, c.City, r.Name as Country, 
				c.IsObsolete, s.FirstName + ' ' + s.LastName as FullName
			from tblCustomer c INNER JOIN
					tblReferences r ON c.CountryID = r.ReferencesID LEFT OUTER JOIN
					tblStaff s ON c.SaleRepID = s.StaffID
			where (c.Code like N'%' + @KeyWord + '%'
						OR	c.Name like N'%' + @KeyWord + '%'
						OR	c.Address like N'%' + @KeyWord + '%'
						OR	c.Tel like N'%' + @KeyWord + '%'
						OR	c.Fax like N'%' + @KeyWord + '%')
				AND (c.IsObsolete = @IsObsolete or @IsObsolete is null)
				AND @SortType = 'A'
		UNION ALL
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN c.Code END DESC,
															CASE @SortColumn WHEN N'1' THEN c.Name END DESC,
															CASE @SortColumn WHEN N'2' THEN c.Address END DESC,
															CASE @SortColumn WHEN N'3' THEN c.Tel END DESC,
															CASE @SortColumn WHEN N'4' THEN c.Fax END DESC,
															CASE @SortColumn WHEN N'5' THEN c.IsObsolete END DESC) as RowIndex, 
				c.CustomerID, c.Code, c.Name, c.Address, c.Tel, c.Fax, c.PostCode, c.City, r.Name as Country, 
				c.IsObsolete, s.FirstName + ' ' + s.LastName as FullName
			from tblCustomer c INNER JOIN
					tblReferences r ON c.CountryID = r.ReferencesID LEFT OUTER JOIN
					tblStaff s ON c.SaleRepID = s.StaffID
			where (c.Code like N'%' + @KeyWord + '%'
						OR	c.Name like N'%' + @KeyWord + '%'
						OR	c.Address like N'%' + @KeyWord + '%'
						OR	c.Tel like N'%' + @KeyWord + '%'
						OR	c.Fax like N'%' + @KeyWord + '%')
				AND (c.IsObsolete = @IsObsolete or @IsObsolete is null)
				AND @SortType = 'D'
	)
	select *
		from T
end
--exec tblCustomer_SelectForReport N'', NULL, '0', 'A'