USE [SweetSoft_APEM]
GO

/****** Object:  Table [dbo].[tblReferences]    Script Date: 02/07/2015 08:47:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblReferences](
	[ReferencesID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](10) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Type] [tinyint] NOT NULL,
	[IsObsolete] [tinyint] NOT NULL,
 CONSTRAINT [PK_tblReferences] PRIMARY KEY CLUSTERED 
(
	[ReferencesID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  StoredProcedure [dbo].[tblReference_SelectAll]    Script Date: 02/07/2015 09:48:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblReference_SelectAll]
@KeyWord nvarchar(100),
@Type smallint,
@IsObsolete smallint,
@PageIndex int,
@PageSize int,
@SortColumn varchar(1),
@SortType varchar(1)
as
begin
	WITH T AS(
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN r.Code END ASC,
															CASE @SortColumn WHEN N'1' THEN r.Name END ASC,
															CASE @SortColumn WHEN N'2' THEN r.IsObsolete END ASC) as RowIndex, 
				r.ReferencesID,r.Code,r.Name,r.Type,r.IsObsolete
			from tblReferences r
			where (r.Name like N'%' + @KeyWord + '%' or r.Code like N'%' + @KeyWord + '%')
				AND (r.IsObsolete = @IsObsolete or @IsObsolete = -1)
				AND @SortType = 'A'
				and r.Type = @Type
		UNION ALL
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN r.Code END DESC,
															CASE @SortColumn WHEN N'1' THEN r.Name END DESC,
															CASE @SortColumn WHEN N'2' THEN r.IsObsolete END DESC) as RowIndex, 
				r.ReferencesID,r.Code,r.Name,r.Type,r.IsObsolete
			from tblReferences r
			where (r.Name like N'%' + @KeyWord + '%' or r.Code like N'%' + @KeyWord + '%')
				AND (r.IsObsolete = @IsObsolete or @IsObsolete = -1)
				AND @SortType = 'D'
				and r.Type = @Type
	)
	select top (@PageSize) *, (select COUNT(RowIndex) from T) as RowsCount
		from T
		where RowIndex > (@PageIndex*@PageSize)
end


go
insert into tblFunction values('country_manager','fReference','Country manager','Country manager',40)
insert into tblFunction values('product_type_manager','fReference','Product type manager','Product type manager',41)
insert into tblFunction values('progress_type_manager','fReference','Progress type manager','Progress type manager',42)
insert into tblFunction values('customer_group_manager','fCustomer','Customer group manager','Customer group manager',43)

go
ALTER TABLE tblContact ADD Tel nvarchar(20)
ALTER TABLE tblContact ADD Email nvarchar(200)


ALTER TABLE tblCustomer ADD CountryID int NOT NULL DEFAULT 1
ALTER TABLE tblCustomer ADD TaxCode nvarchar(15)
ALTER TABLE tblCustomer ADD TIN nvarchar(25)
ALTER TABLE tblCustomer ADD Email nvarchar(200)
ALTER TABLE tblCustomer ADD ShortName nvarchar(65)
ALTER TABLE tblCustomer ADD GroupID int 
ALTER TABLE tblCustomer ADD InternalOrderNo  nvarchar(10) 
ALTER TABLE tblCustomer DROP COLUMN Country
ALTER TABLE tblCustomer ADD IsBrand tinyint not null default 0
ALTER TABLE tblCustomer ALTER COLUMN Code varchar(5)





