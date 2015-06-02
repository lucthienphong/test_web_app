
/****** Object:  StoredProcedure [dbo].[SearchDeliveryOrderByCustomer]    Script Date: 06/01/2015 09:45:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER proc [dbo].[SearchDeliveryOrderByCustomer]
@Customer nvarchar(200),
@DONumber nvarchar(50),
@JobNumber nvarchar(50),
@FromDate datetime,
@ToDate datetime,
@PageIndex int,
@PageSize int,
@SortColumn varchar(2),
@SortType varchar(1)
AS
BEGIN
	;WITH T AS(
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY CASE @SortColumn WHEN N'0' THEN  d.DONumber END ASC,
															CASE @SortColumn WHEN N'1' THEN d.OrderDate END ASC,
															CASE @SortColumn WHEN N'2' THEN c.Code END ASC,
															CASE @SortColumn WHEN N'3' THEN c.Name END ASC,
															CASE @SortColumn WHEN N'4' THEN j.JobNumber END ASC,
															CASE @SortColumn WHEN N'5' THEN j.JobName END ASC) as RowIndex, 
				d.JobID, d.DONumber, CONVERT(nvarchar(10), d.OrderDate, 103) as OrderDate,
				c.Name, c.Code, j.JobName, j.JobNumber, j.RevNumber, ol.Locking AS Lock,
				case when (select COUNT(*) from tblInvoiceDetail as id where j.JobID = id.JobID) > 0 then 1 else 0 end as InvoiceStatus
			from tblJob j inner join
				tblCustomer c on c.CustomerID = j.CustomerID inner join
				tblDeliveryOrder d on j.JobID = d.JobID left outer join
				tblObjectLocking ol on j.JobID = ol.ID and ol.[Type] = N'DO'
			where (c.Code like '%' + @Customer + '%' or c.Name like '%' + @Customer + '%')
				and (j.JobNumber like '%' + @JobNumber + '%')
				and (d.DONumber like '%' + @DONumber + '%')
				and (DATEDIFF(D, d.OrderDate, @FromDate) <=0 or @FromDate IS NULL)
				and (DATEDIFF(D, d.OrderDate, @ToDate) >=0 or @ToDate IS NULL)
				and	@SortType = 'A'
		union all
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY CASE @SortColumn WHEN N'0' THEN  d.DONumber END DESC,
															CASE @SortColumn WHEN N'1' THEN d.OrderDate END DESC,
															CASE @SortColumn WHEN N'2' THEN c.Code END DESC,
															CASE @SortColumn WHEN N'3' THEN c.Name END DESC,
															CASE @SortColumn WHEN N'4' THEN j.JobNumber END DESC,
															CASE @SortColumn WHEN N'5' THEN j.JobName END DESC) as RowIndex, 
				d.JobID, d.DONumber, CONVERT(nvarchar(10), d.OrderDate, 103) as OrderDate,
				c.Name, c.Code, j.JobName, j.JobNumber, j.RevNumber, ol.Locking AS Lock,
				case when (select COUNT(*) from tblInvoiceDetail as id where j.JobID = id.JobID) > 0 then 1 else 0 end as InvoiceStatus
			from tblJob j inner join
				tblCustomer c on c.CustomerID = j.CustomerID inner join
				tblDeliveryOrder d on j.JobID = d.JobID left outer join
				tblObjectLocking ol on j.JobID = ol.ID and ol.[Type] = N'DO'
			where (c.Code like '%' + @Customer + '%' or c.Name like '%' + @Customer + '%')
				and (j.JobNumber like '%' + @JobNumber + '%')
				and (d.DONumber like '%' + @DONumber + '%')
				and (DATEDIFF(D, d.OrderDate, @FromDate) <=0 or @FromDate IS NULL)
				and (DATEDIFF(D, d.OrderDate, @ToDate) >=0 or @ToDate IS NULL)
				and	@SortType = 'D'				
		)
		select top (@PageSize) *, (select COUNT(RowIndex) from T) as RowsCount
		from T
		where RowIndex > (@PageIndex*@PageSize)
--exec [SearchDeliveryOrderByCustomer] '', '','', null, null, 0, 10, '1', 'A'	
END

