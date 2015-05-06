USE [SweetSoft_APEM]
GO


create proc [SearchPurchaseOrderByCustomer]
@CustomerID int,
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
															CASE @SortColumn WHEN N'3' THEN p.TotalNumberOfCylinders END ASC) 
															as RowIndex, 
															p.OrderNumber,p.TotalNumberOfCylinders, 
															CONVERT(nvarchar(10), p.OrderDate, 103) as OrderDate,
															CONVERT(nvarchar(10), p.RequiredDeliveryDate, 103) as RequiredDeliveryDate,
															p.IsUrgent,
															p.PurchaseOrderID
				
				from tblJob j ,
				tblCustomer c,
				tblPurchaseOrder p
			where
				(c.CustomerID = j.CustomerID and j.JobID = p.JobID)
				and (c.CustomerID = @CustomerID or @CustomerID is Null)
				and (DATEDIFF(D, p.OrderDate, @OrderDate) <=0 or @OrderDate IS NULL)
				and (DATEDIFF(D, p.RequiredDeliveryDate, @DeliveryDate) >=0 or @DeliveryDate IS NULL)
				and	@SortType = 'A'
		union all
		
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN  p.OrderNumber END DESC,
															CASE @SortColumn WHEN N'1' THEN p.OrderDate END DESC,
															CASE @SortColumn WHEN N'2' THEN p.RequiredDeliveryDate END DESC,
															CASE @SortColumn WHEN N'3' THEN p.TotalNumberOfCylinders END DESC) 
															as RowIndex, 
															p.OrderNumber,p.TotalNumberOfCylinders, 
															CONVERT(nvarchar(10), p.OrderDate, 103) as OrderDate,
															CONVERT(nvarchar(10), p.RequiredDeliveryDate, 103) as RequiredDeliveryDate,
															p.IsUrgent,
															p.PurchaseOrderID
															
			from tblJob j ,
				tblCustomer c,
				tblPurchaseOrder p
			where
				(c.CustomerID = j.CustomerID and j.JobID = p.JobID)
				and (c.CustomerID = @CustomerID or @CustomerID is Null)
				and (DATEDIFF(D, p.OrderDate, @OrderDate) <=0 or @OrderDate IS NULL)
				and (DATEDIFF(D, p.RequiredDeliveryDate, @DeliveryDate) >=0 or @DeliveryDate IS NULL)
				and @SortType = 'D'
				
		)
		select top (@PageSize) *, (select COUNT(RowIndex) from T) as RowsCount
		from T
		where RowIndex > (@PageIndex*@PageSize)
	
END
--exec [SearchPurchaseOrderByCustomer] 5, null, null, 0, 10, '1', 'D'

