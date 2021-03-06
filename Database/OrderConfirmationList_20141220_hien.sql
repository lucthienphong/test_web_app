USE [SweetSoft_APEM]
GO
/****** Object:  StoredProcedure [dbo].[SearchPurchaseOrderByCustomer]    Script Date: 12/20/2014 08:09:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create proc [dbo].[TblConfirmOrder_SelectAll]
@CustomerID int,
@JobName nvarchar(100),
@OCNumber nvarchar(100),
@OrderDate datetime,
@PageIndex int,
@PageSize int,
@SortColumn varchar(2),
@SortType varchar(1)
AS
BEGIN
	;WITH T AS(
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN  o.OCNumber END ASC,
															CASE @SortColumn WHEN N'1' THEN  o.OrderDate END ASC,
															CASE @SortColumn WHEN N'2' THEN  o.Discount END ASC,
															CASE @SortColumn WHEN N'3' THEN  o.TaxPercentage END ASC,
															CASE @SortColumn WHEN N'4' THEN  o.TotalPrice END ASC,
															CASE @SortColumn WHEN N'5' THEN  (ISNULL(o.TotalPrice,0) - ISNULL(o.TotalPrice,0)*o.Discount/100) END ASC)
															as RowIndex,
															o.OCNumber,CONVERT(nvarchar(10), o.OrderDate, 103) as OrderDate,o.JobID,
															ISNULL(o.Discount,0) as Discount,
															ISNULL(o.TaxPercentage,0) as TaxPercentage,
															ISNULL(o.TotalPrice,0) as TotalPrice,
															(ISNULL(o.TotalPrice,0) - ISNULL(o.TotalPrice,0)*o.Discount/100) as NetTotal
															
			from tblJob j ,tblCustomer c,tblOrderConfirmation o 
			where 
					(c.CustomerID = j.CustomerID and j.JobID = o.JobID)
				and (c.CustomerID = @CustomerID or @CustomerID is Null)
				and ((j.JobName like N'%' + @JobName + '%') OR (j.JobNumber like N'%' + @JobName + '%') or (@JobName is null))
				and (o.OCNumber like N'%' + @OCNumber + '%' or o.OCNumber is null)
				and (DATEDIFF(D, o.OrderDate, @OrderDate) =0 or @OrderDate IS NULL)
				and @SortType = 'A'
		union all
		
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN  o.OCNumber END DESC,
															CASE @SortColumn WHEN N'1' THEN o.OrderDate END DESC,
															CASE @SortColumn WHEN N'2' THEN  o.Discount END DESC,
															CASE @SortColumn WHEN N'3' THEN  o.TaxPercentage END DESC,
															CASE @SortColumn WHEN N'4' THEN  o.TotalPrice END DESC,
															CASE @SortColumn WHEN N'5' THEN  (ISNULL(o.TotalPrice,0) - ISNULL(o.TotalPrice,0)*o.Discount/100) END DESC)
															as RowIndex, 
															o.OCNumber,CONVERT(nvarchar(10), o.OrderDate, 103) as OrderDate,o.JobID,
															ISNULL(o.Discount,0) as Discount,
															ISNULL(o.TaxPercentage,0) as TaxPercentage,
															ISNULL(o.TotalPrice,0) as TotalPrice,
															(ISNULL(o.TotalPrice,0) - ISNULL(o.TotalPrice,0)*o.Discount/100) as NetTotal
															
			from tblJob j,tblCustomer c,tblOrderConfirmation o 
			where
				(c.CustomerID = j.CustomerID and j.JobID = o.JobID)
				and (c.CustomerID = @CustomerID or @CustomerID is Null)
				and ((j.JobName like N'%' + @JobName + '%') OR (j.JobNumber like N'%' + @JobName + '%') or (@JobName is null))
				and (o.OCNumber like N'%' + @OCNumber + '%' or o.OCNumber is null)
				and (DATEDIFF(D, o.OrderDate, @OrderDate) =0 or @OrderDate IS NULL)
				and @SortType = 'D'
				
		)
		select top (@PageSize) *, (select COUNT(RowIndex) from T) as RowsCount
		from T
		where RowIndex > (@PageIndex*@PageSize)
	
END
--exec [TblConfirmOrder_SelectAll] ,null, null, null, 0, 10, '1', 'D'


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [dbo].[tblCylinder_SelectForOrderConfirmation]    Script Date: 12/22/2014 07:43:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblCylinder_SelectForOrderConfirmation]
@JobID int
as
begin
with T as (
	select c.CylinderID, c.Sequence, c.SteelBase, c.CylinderNo, c.Color, c.PricingID, p.PricingName, case c.SteelBase when 1 then 'New' else 'Old' end as SteelBaseName,c.Circumference,c.FaceWidth,c.UnitPrice,isnull(c.Quantity,1) as Quantity,c.CylinderStatusID, cs.CylinderStatusName,
	c.Dirameter, c.Dept, c.IsPivotCylinder,p.ForTobaccoCustomers,t.TaxCode,c.TaxPercentage,t.TaxID,
	
	case p.ForTobaccoCustomers when 0 then (c.FaceWidth*c.Circumference/100)*ISNULL(c.UnitPrice,0) 
	else ISNULL(c.UnitPrice,0) end as TotalPrice,
	
	case p.ForTobaccoCustomers when 0 then (
			(c.FaceWidth*c.Circumference/100)*ISNULL(c.UnitPrice,0) - (c.FaceWidth*c.Circumference/100)*ISNULL(c.UnitPrice,0)*ISNULL(c.						TaxPercentage/100,0)) 
	else (ISNULL(c.UnitPrice,0) - ISNULL(c.UnitPrice,0)*ISNULL(c.TaxPercentage/100,0)) end as PriceTaxed
		
		from tblCylinder c INNER JOIN
			tblCylinderStatus cs ON c.CylinderStatusID = cs.CylinderStatusID INNER JOIN
			tblPricing p ON c.PricingID = p.PricingID
			left join tblJobQuotationPricing jp on c.JobID = jp.JobID
			left outer join tblTax t on c.TaxID = t.TaxID
		where c.JobID = @JobID
		and c.PricingID = jp.PricingID
		)
		select *,(select sum(PriceTaxed) from T) as Total from T
		order by T.Sequence
end