/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.tblDebit ADD
	TaxID int NULL
GO
ALTER TABLE dbo.tblDebit SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

/****** Object:  StoredProcedure [dbo].[tblDebit_SelectAll]    Script Date: 05/22/2015 15:18:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblDebit_SelectAll]
@DebitNo nvarchar(50),
@Customer nvarchar(200),
@FromDate datetime,
@ToDate datetime,
@PageIndex int,
@PageSize int,
@SortColumn varchar(2),
@SortType varchar(1)
as
begin
;WITH T AS(
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN  cd.DebitNo END ASC,
															CASE @SortColumn WHEN N'1' THEN cd.DebitDate END ASC,
															CASE @SortColumn WHEN N'2' THEN c.Code END ASC,
															CASE @SortColumn WHEN N'3' THEN c.Name END ASC,
															CASE @SortColumn WHEN N'4' THEN cd.Total END ASC) as RowIndex, 
				cd.DebitID, c.Code, c.Name, cd.DebitNo, CONVERT(nvarchar(10), cd.DebitDate, 103) as DebitDate,
				ISNULL(ISNULL(cd.Total, 0)  * (1 + t.TaxPercentage /100), ISNULL(cd.Total, 0)) as Total, cr.CurrencyName
			from tblDebit cd INNER JOIN
				tblCustomer c ON cd.CustomerID = c.CustomerID INNER JOIN
				tblCurrency cr ON cd.CurrencyID = cr.CurrencyID LEFT OUTER JOIN
				tblTax t on t.TaxID = cd.TaxID
			where (cd.DebitNo like '%' + @DebitNo + '%')
				and (c.Name like '%' + @Customer + '%' or c.Code like '%' + @Customer + '%')
				and (DATEDIFF(D, cd.DebitDate, @FromDate) <=0 or @FromDate IS NULL)
				and (DATEDIFF(D, cd.DebitDate, @ToDate) >=0 or @ToDate IS NULL)
				and @SortType = 'A'
		union all
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN  cd.DebitNo END DESC,
															CASE @SortColumn WHEN N'1' THEN cd.DebitDate END DESC,
															CASE @SortColumn WHEN N'2' THEN c.Code END DESC,
															CASE @SortColumn WHEN N'3' THEN c.Name END DESC,
															CASE @SortColumn WHEN N'4' THEN cd.Total END DESC) as RowIndex, 
				cd.DebitID, c.Code, c.Name, cd.DebitNo, CONVERT(nvarchar(10), cd.DebitDate, 103) as DebitDate,
				ISNULL(ISNULL(cd.Total, 0)  * (1 + t.TaxPercentage /100), ISNULL(cd.Total, 0)) as Total, cr.CurrencyName
			from tblDebit cd INNER JOIN
				tblCustomer c ON cd.CustomerID = c.CustomerID INNER JOIN
				tblCurrency cr ON cd.CurrencyID = cr.CurrencyID LEFT OUTER JOIN
				tblTax t on t.TaxID = cd.TaxID
			where (cd.DebitNo like '%' + @DebitNo + '%')
				and (c.Name like '%' + @Customer + '%' or c.Code like '%' + @Customer + '%')
				and (DATEDIFF(D, cd.DebitDate, @FromDate) <=0 or @FromDate IS NULL)
				and (DATEDIFF(D, cd.DebitDate, @ToDate) >=0 or @ToDate IS NULL)
				and @SortType = 'D'
		)
		select top (@PageSize) *, (select COUNT(RowIndex) from T) as RowsCount
			from T
			where RowIndex > (@PageIndex*@PageSize)
	--exec tblDebit_SelectAll '', '', null, null, 0, 10, '0', 'A'
end

go

ALTER TABLE tblPurchaseOrder
DROP CONSTRAINT [FK_tblPurchaseOrder_tblSupplier]