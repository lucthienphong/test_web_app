USE [SweetSoft_APEM]
GO
/****** Object:  StoredProcedure [dbo].[SearchPurchaseOrderByCustomer]    Script Date: 12/26/2014 08:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER proc [dbo].[SearchPurchaseOrderByCustomer]
@CustomerID int,
@JobNumber nvarchar(50),
@PurchaseOrder nvarchar(50),
@OrderDate datetime,
@DeliveryDate datetime,
@PageIndex int,
@PageSize int,
@SortColumn varchar(2),
@SortType varchar(1)
AS
BEGIN
	;WITH T AS(
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN  p.OrderNumber END ASC,
															CASE @SortColumn WHEN N'1' THEN p.OrderDate END ASC,
															CASE @SortColumn WHEN N'2' THEN p.RequiredDeliveryDate END ASC,
															CASE @SortColumn WHEN N'3' THEN p.TotalNumberOfCylinders END ASC,
															CASE @SortColumn WHEN N'4' THEN j.JobNumber END ASC) 
															as RowIndex, 
															p.OrderNumber,j.JobNumber,p.TotalNumberOfCylinders, 
															CONVERT(nvarchar(10), p.OrderDate, 103) as OrderDate,
															CONVERT(nvarchar(10), p.RequiredDeliveryDate, 103) as RequiredDeliveryDate,
															p.IsUrgent,
															p.PurchaseOrderID
				
				from tblJob j ,
				tblCustomer c,
				tblPurchaseOrder p
			where
				(c.CustomerID = j.CustomerID and j.JobID = p.JobID)
				and (
						(c.CustomerID = @CustomerID or @CustomerID is Null)
						and (j.JobNumber like '%' + @JobNumber + '%')
						and (p.OrderNumber like '%' + @PurchaseOrder + '%')
					)
				and (DATEDIFF(D, p.OrderDate, @OrderDate) =0 or @OrderDate IS NULL)
				and (DATEDIFF(D, p.RequiredDeliveryDate, @DeliveryDate) =0 or @DeliveryDate IS NULL)
				and	@SortType = 'A'
		union all
		
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN  p.OrderNumber END DESC,
															CASE @SortColumn WHEN N'1' THEN p.OrderDate END DESC,
															CASE @SortColumn WHEN N'2' THEN p.RequiredDeliveryDate END DESC,
															CASE @SortColumn WHEN N'3' THEN p.TotalNumberOfCylinders END DESC,
															CASE @SortColumn WHEN N'4' THEN j.JobNumber END DESC) 
															as RowIndex, 
															p.OrderNumber,j.JobNumber,p.TotalNumberOfCylinders, 
															CONVERT(nvarchar(10), p.OrderDate, 103) as OrderDate,
															CONVERT(nvarchar(10), p.RequiredDeliveryDate, 103) as RequiredDeliveryDate,
															p.IsUrgent,
															p.PurchaseOrderID
															
			from tblJob j ,
				tblCustomer c,
				tblPurchaseOrder p
			where
				(c.CustomerID = j.CustomerID and j.JobID = p.JobID)
				and (
						(c.CustomerID = @CustomerID or @CustomerID is Null)
						and (j.JobNumber like '%' + @JobNumber + '%')
						and (p.OrderNumber like '%' + @PurchaseOrder + '%')
					)
				and (DATEDIFF(D, p.OrderDate, @OrderDate) =0 or @OrderDate IS NULL)
				and (DATEDIFF(D, p.RequiredDeliveryDate, @DeliveryDate) =0 or @DeliveryDate IS NULL)
				and @SortType = 'D'
				
		)
		select top (@PageSize) *, (select COUNT(RowIndex) from T) as RowsCount
		from T
		where RowIndex > (@PageIndex*@PageSize)
	
END
--exec [SearchPurchaseOrderByCustomer] null,'1412/00001','APE14600012', null, null, 0, 10, '0', 'A'
-- select * from tblCustomer
