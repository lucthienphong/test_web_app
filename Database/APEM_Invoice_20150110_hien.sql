USE [SweetSoft_APEM]
GO

--ALTER TABLE tblInvoice
--ADD TotalPrice decimal(18,3)
--ALTER TABLE tblInvoice
--ADD NetTotal decimal(18,3)

/****** Object:  StoredProcedure [dbo].[TblConfirmOrder_SelectAll]    Script Date: 01/10/2015 09:52:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

alter proc [dbo].[TblInvoice_SelectAll]
@CustomerID int,
@InvoiceNo nvarchar(100),
@InvoiceDate datetime,
@PageIndex int,
@PageSize int,
@SortColumn varchar(2),
@SortType varchar(1)
AS
BEGIN
	;WITH T AS(
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN  i.InvoiceNo END ASC,
															CASE @SortColumn WHEN N'1' THEN  c.Name END ASC,
															CASE @SortColumn WHEN N'2' THEN  i.InvoiceDate END ASC,
															CASE @SortColumn WHEN N'3' THEN  i.TotalPrice END ASC,
															CASE @SortColumn WHEN N'4' THEN  i.NetTotal END ASC)
															as RowIndex,
															i.InvoiceNo,CONVERT(nvarchar(10), i.InvoiceDate, 103) as InvoiceDate,i.InvoiceID,
															c.Name as CustomerName,i.TotalPrice,i.NetTotal
															
															
			from tblInvoice i ,tblCustomer c
			where (c.CustomerID = i.CustomerID)
				and (c.CustomerID = @CustomerID or @CustomerID is Null)
				and (i.InvoiceNo = @InvoiceNo or @InvoiceNo is Null or @InvoiceNo ='')
				and (DATEDIFF(D, i.InvoiceDate, @InvoiceDate) =0 or @InvoiceDate IS NULL)
				and @SortType = 'A'
				
		union all
		
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN  i.InvoiceNo END DESC,
															CASE @SortColumn WHEN N'1' THEN  c.Name END DESC,
															CASE @SortColumn WHEN N'2' THEN  i.InvoiceDate END DESC,
															CASE @SortColumn WHEN N'3' THEN  i.TotalPrice END DESC,
															CASE @SortColumn WHEN N'4' THEN  i.NetTotal END DESC)
															as RowIndex,
															i.InvoiceNo,CONVERT(nvarchar(10), i.InvoiceDate, 103) as InvoiceDate,i.InvoiceID,
															c.Name as CustomerName,i.TotalPrice,i.NetTotal
															
			from tblInvoice i ,tblCustomer c
			where (c.CustomerID = i.CustomerID)
				and (c.CustomerID = @CustomerID or @CustomerID is Null)
				and (i.InvoiceNo = @InvoiceNo or @InvoiceNo is Null or @InvoiceNo ='')
				and (DATEDIFF(D, i.InvoiceDate, @InvoiceDate) =0 or @InvoiceDate IS NULL)
				and @SortType = 'D'
				
		)
		select top (@PageSize) *, (select COUNT(RowIndex) from T) as RowsCount
		from T
		where RowIndex > (@PageIndex*@PageSize)
	
END
--exec [TblConfirmOrder_SelectAll] ,null, null, null, 0, 10, '1', 'D'


SET ANSI_NULLS ON

