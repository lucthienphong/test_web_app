USE [SweetSoft_APEM]
GO
/****** Object:  StoredProcedure [dbo].[tblTax_SelectAll]    Script Date: 12/22/2014 08:06:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblTax_SelectAll]
@KeyWord nvarchar(100),
@IsObsolete bit,
@PageIndex int,
@PageSize int,
@SortColumn varchar(1),
@SortType varchar(1)
as
begin
	WITH T AS(
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN d.TaxName END ASC,
															CASE @SortColumn WHEN N'1' THEN d.TaxPercentage END ASC,
															CASE @SortColumn WHEN N'2' THEN d.IsObsolete END ASC) as RowIndex, 
				TaxID, TaxName,TaxCode, TaxPercentage, IsObsolete
			from tblTax d
			where d.TaxName like N'%' + @KeyWord + '%'
				AND (d.IsObsolete = @IsObsolete or @IsObsolete is null)
				AND @SortType = 'A'
		UNION ALL
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN d.TaxName END DESC,
															CASE @SortColumn WHEN N'1' THEN d.TaxPercentage END DESC,
															CASE @SortColumn WHEN N'2' THEN d.IsObsolete END DESC) as RowIndex, 
				TaxID, TaxName,TaxCode,TaxPercentage, IsObsolete
			from tblTax d
			where d.TaxName like N'%' + @KeyWord + '%'
				AND (d.IsObsolete = @IsObsolete or @IsObsolete is null)
				AND @SortType = 'D'
	)
	select top (@PageSize) *, (select COUNT(RowIndex) from T) as RowsCount
		from T
		where RowIndex > (@PageIndex*@PageSize)
end
--exec tblTax_SelectAll N'', NULL, 0, 10, '0', 'A'

drop table tblServiceJobDetail
drop table tblServiceJobInvoice
drop table tblServiceJob

/****** Object:  Table [dbo].[tblServiceJobDetail]    Script Date: 12/22/2014 09:42:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblServiceJobDetail](
	[ServiceJobID] [int] IDENTITY(1,1) NOT NULL,
	[JobID] [int] NOT NULL,
	[WorkOrderNumber] [nvarchar](100) NOT NULL,
	[ProductID] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[WorkOrderValues] [decimal](18, 3) NOT NULL,
 CONSTRAINT [PK_tblServiceJobDetail] PRIMARY KEY CLUSTERED 
(
	[ServiceJobID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tblServiceJobDetail]  WITH CHECK ADD  CONSTRAINT [FK_tblServiceJobDetail_tblJob] FOREIGN KEY([JobID])
REFERENCES [dbo].[tblJob] ([JobID])
GO

ALTER TABLE [dbo].[tblServiceJobDetail] CHECK CONSTRAINT [FK_tblServiceJobDetail_tblJob]
GO

ALTER TABLE tblJob
ADD IsServiceJob bit not null default 0

ALTER TABLE tblJob
ADD PaymentTerms nvarchar(50) 

/****** Object:  StoredProcedure [dbo].[tblJob_SelectAll]    Script Date: 12/23/2014 15:07:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblJob_SelectAll]
@Customer nvarchar(200),
@JobBarcode nvarchar(255),
@JobNumber nvarchar(10),
@JobInfo nvarchar(200),
@SaleRepID int,
@FromDate datetime,
@ToDate datetime,
@IsServiceJob bit,
@PageIndex int,
@PageSize int,
@SortColumn varchar(2),
@SortType varchar(1)
AS
BEGIN
	;WITH T AS(
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN  c.Code END ASC,
															--CASE @SortColumn WHEN N'1' THEN c.Name END ASC,
															--CASE @SortColumn WHEN N'1' THEN j.JobBarcode END ASC,
															CASE @SortColumn WHEN N'1' THEN j.JobNumber END ASC,
															CASE @SortColumn WHEN N'2' THEN j.JobName END ASC,
															CASE @SortColumn WHEN N'3' THEN j.Design END ASC,
															CASE @SortColumn WHEN N'4' THEN j.CreatedOn END ASC) as RowIndex, 
				j.JobID, c.Code, c.Name, j.JobBarcode, j.JobNumber, j.JobName, j.Design, j.RevNumber, 
				CONVERT(nvarchar(10), j.CreatedOn, 103) as CreatedOn, 
				s.FirstName + ' ' + s.LastName as SaleDep
			from tblJob j INNER JOIN
				tblCustomer c ON j.CustomerID = c.CustomerID INNER JOIN
				tblStaff s ON j.SalesRepID = s.StaffID
			where j.IsClosed = 0
				and (c.Name like '%' + @Customer + '%' or c.Code like '%' + @Customer + '%')
				and j.JobBarcode like '%' + @JobBarcode + '%'
				and j.JobNumber like '%' + @JobNumber + '%'
				and (j.JobName like '%' + @JobInfo + '%' or j.Design like '%' + @JobInfo + '%')
				and (j.SalesRepID = @SaleRepID or @SaleRepID = 0)
				and (DATEDIFF(D, j.CreatedOn, @FromDate) <=0 or @FromDate IS NULL)
				and (DATEDIFF(D, j.CreatedOn, @ToDate) >=0 or @ToDate IS NULL)
				and j.IsServiceJob =@IsServiceJob
				and @SortType = 'A'
		union all
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN  c.Code END DESC,
															--CASE @SortColumn WHEN N'1' THEN c.Name END DESC,
															--CASE @SortColumn WHEN N'2' THEN j.JobBarcode END DESC,
															CASE @SortColumn WHEN N'1' THEN j.JobNumber END DESC,
															CASE @SortColumn WHEN N'2' THEN j.JobName END DESC,
															CASE @SortColumn WHEN N'3' THEN j.Design END DESC,
															CASE @SortColumn WHEN N'4' THEN j.CreatedOn END DESC) as RowIndex, 
				j.JobID, c.Code, c.Name, j.JobBarcode, j.JobNumber, j.JobName, j.Design, j.RevNumber, 
				CONVERT(nvarchar(10), j.CreatedOn, 103) as CreatedOn,
				s.FirstName + ' ' + s.LastName as SaleDep
			from tblJob j INNER JOIN
				tblCustomer c ON j.CustomerID = c.CustomerID INNER JOIN
				tblStaff s ON j.SalesRepID = s.StaffID
			where j.IsClosed = 0
				and (c.Name like '%' + @Customer + '%' or c.Code like '%' + @Customer + '%')
				and j.JobBarcode like '%' + @JobBarcode + '%'
				and j.JobNumber like '%' + @JobNumber + '%'
				and (j.JobName like '%' + @JobInfo + '%' or j.Design like '%' + @JobInfo + '%')
				and (j.SalesRepID = @SaleRepID or @SaleRepID = 0)
				and (DATEDIFF(D, j.CreatedOn, @FromDate) <=0 or @FromDate IS NULL)
				and (DATEDIFF(D, j.CreatedOn, @ToDate) >=0 or @ToDate IS NULL)
				and j.IsServiceJob =@IsServiceJob
				and @SortType = 'D'
		)
		select top (@PageSize) *, (select COUNT(RowIndex) from T) as RowsCount
			from T
			where RowIndex > (@PageIndex*@PageSize)
	--exec tblJob_SelectAll '', '', '', '', 0, '2014/11/28', null, 0, 10, '4', 'D'
END