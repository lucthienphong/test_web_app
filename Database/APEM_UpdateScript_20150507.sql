GO
/****** Object:  StoredProcedure [dbo].[Report_SalesReport]    Script Date: 05/06/2015 10:09:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[Report_SalesReport]
@ProductTypeID int,
@SaleID int,-- StaffID
@CustomerID int,
@Type int,-- 0 - All, 1 - No Invoice yet, 2 - Invoice Completed
@BaseCurrencyID int,
@FromDate datetime,
@ToDate datetime,
@FromDateInvoice datetime,
@ToDateInvoice datetime,
@PageIndex int,
@PageSize int,
@SortColumn varchar(2),
@SortType varchar(1)
as
begin
	WITH T AS(
	select ROW_NUMBER() OVER(ORDER BY j.JobNumber) as RowIndex, j.JobNumber, c.SAPCode, r.Code, j.JobName, j.Design, 
			bo.Name as BrandOwner, j.ItemCode, j.TypeOfOrder, j.DrawingNumber, CONVERT(nvarchar(10), oc.OrderDate, 103) as OCDate, js.Circumference, js.FaceWidth,
			curr.CurrencyName, ISNULL(i.RMValue, oc.RMValue) as RMValue, ISNULL(dereCyl.DeReQty, 0) as DeReQty, ISNULL(dereCyl.DeReTotalPrice, 0) as DeReTotalPrice, 
			cyl.OldQty, cyl.NewQty, cyl.TotalQty, cyl.TotalPrice, 
			ISNULL(sjd.ServiceJobs, 0) as ServiceJobs, ISNULL(othC.OtherCharges, 0) as OtherCharges, oc.Discount,
			oc.TotalPrice * (1 - oc.Discount/100) as SubTotal, i.InvoiceNo, CONVERT(nvarchar(10), i.InvoiceDate, 103) as InvoiceDate, 
			CONVERT(nvarchar(10), i.CreatedOn, 103) as InvoicePostingDate, tax.TaxCode, tax.TaxPercentage,
			--iTotal.TotalOverseas as TotalOverseas, iTotal.TotalRM as TotalMY, 
			CASE WHEN j.CurrencyID <>  @BaseCurrencyID THEN ROUND((oc.TotalPrice * (1 - oc.Discount/100)) * (1 + oc.TaxPercentage/100),2)
					ELSE 0 END as TotalOverseas,
			ROUND((oc.TotalPrice * (1 - oc.Discount/100)) * (1 + oc.TaxPercentage/100) * ISNULL(i.RMValue, oc.RMValue),2) as TotalRM,
			i.PaymentTern, c.CustomerID, c.Name as CustomerName, j.ProductTypeID, prType.Name as ProductTypeName, j.SalesRepID, s.FirstName, j.RevNumber as RevJobNumber
		from tblJob j INNER JOIN
			tblCurrency curr ON j.CurrencyID = curr.CurrencyID INNER JOIN
			tblStaff s ON j.SalesRepID = s.StaffID INNER JOIN
			tblCustomer c ON c.CustomerID = j.CustomerID INNER JOIN 
			tblReferences r ON c.CountryID = r.ReferencesID LEFT OUTER JOIN			
			tblJobSheet js ON j.JobID = js.JobID LEFT OUTER JOIN
			tblReferences prType ON j.ProductTypeID = prType.ReferencesID LEFT OUTER JOIN
			tblOrderConfirmation oc ON j.JobID = oc.JobID LEFT OUTER JOIN
			tblInvoiceDetail id ON id.JobID = oc.JobID LEFT OUTER JOIN
			tblInvoice i ON id.InvoiceID = i.InvoiceID LEFT OUTER JOIN
			tblTax tax ON  oc.TaxID = tax.TaxID LEFT OUTER JOIN
			tblReferences bo ON j.BrandOwner = bo.ReferencesID LEFT OUTER JOIN
			--Lấy tổng tiền DeReChrome
			(select cyl.JobID, SUM(CASE cylSt.Physical WHEN 1 THEN isnull(cyl.Quantity,1) ELSE 0 END) as DeReQty, 
					SUM(ROUND((case qd.UnitOfMeasure when 'cm2' then (cyl.FaceWidth*cyl.Circumference/100)*ISNULL(cyl.UnitPrice,0) 
											else ISNULL(cyl.UnitPrice,0) end),2)) as DeReTotalPrice
				from tblCylinder cyl INNER JOIN
					tblCylinderStatus cylSt ON cyl.CylinderStatusID = cylSt.CylinderStatusID INNER JOIN
					tblCustomerQuotationDetail qd ON cyl.PricingID = qd.ID
				where cylSt.Action = 'DeReChrome'
				group by JobID) as dereCyl ON dereCyl.JobID = j.JobID LEFT OUTER JOIN
			--Lấy tổng tiền DeReChrome
			(select cyl.JobID, 
					SUM(CASE WHEN cylSt.Physical = 1 and cyl.SteelBase = 0 THEN isnull(cyl.Quantity,1) ELSE 0 END) as OldQty,
					SUM(CASE WHEN cylSt.Physical = 1 and cyl.SteelBase = 1 THEN isnull(cyl.Quantity,1) ELSE 0 END) as NewQty,
					SUM(CASE WHEN cylSt.Physical = 1 THEN isnull(cyl.Quantity,1) ELSE 0 END) as TotalQty,
					SUM(case qd.UnitOfMeasure when 'cm2' then (cyl.FaceWidth*cyl.Circumference/100)*ISNULL(cyl.UnitPrice,0) 
											else ISNULL(cyl.UnitPrice,0) end) as TotalPrice
				from tblCylinder cyl INNER JOIN
					tblCylinderStatus cylSt ON cyl.CylinderStatusID = cylSt.CylinderStatusID INNER JOIN
					tblCustomerQuotationDetail qd ON cyl.PricingID = qd.ID
				where cylSt.Action = 'Repro'
				group by JobID) as cyl ON cyl.JobID = j.JobID  LEFT OUTER JOIN
			--Tổng tiền Service Job
			(select sjd.JobID, SUM(sjd.WorkOrderValues) as ServiceJobs
				from tblServiceJobDetail sjd
				group by sjd.JobID) as sjd ON j.JobID = sjd.JobID LEFT OUTER JOIN
			--Tổng tiền Other Charges
			(select othC.JobID, SUM(othC.Quantity * othC.Charge) as OtherCharges
				from tblOtherCharges othC
				group by othC.JobID) as othC ON j.JobID = othC.JobID --LEFT OUTER JOIN
			----Tổng tiền hóa đơn
			--(select sIdl.InvoiceID, SUM(ROUND((sO.TotalPrice * (1 - sO.Discount/100)) * (1 + sO.TaxPercentage/100),2)) as TotalOverseas,
			--		SUM(ROUND((sO.TotalPrice * (1 - sO.Discount/100)) * (1 + sO.TaxPercentage/100) * sO.RMValue, 2)) as TotalRM
			--	from tblOrderConfirmation sO INNER JOIN
			--		tblInvoiceDetail sIdl ON sO.JobID = sIdl.JobID
			--	group by sIdl.InvoiceID) as iTotal ON i.InvoiceID = iTotal.InvoiceID
		where (j.ProductTypeID = @ProductTypeID or @ProductTypeID = 0)
				and (j.CustomerID = @CustomerID or @CustomerID = 0)
				and (j.SalesRepID = @SaleID or @SaleID = 0)
				and ((@Type = 0) or (@Type = 1 and i.InvoiceID IS NULL) or (@Type = 2 and i.InvoiceID IS NOT NULL))
				and (DATEDIFF(D, j.CreatedOn, @FromDate) <= 0 or @FromDate is NULL)
				and (DATEDIFF(D, j.CreatedOn, @ToDate) >= 0 or @ToDate is NULL)
				and (DATEDIFF(D, i.InvoiceDate, @FromDateInvoice) <= 0 or @FromDateInvoice is NULL)
				and (DATEDIFF(D, i.InvoiceDate, @ToDateInvoice) >= 0 or @ToDateInvoice is NULL)
		group by j.JobNumber, s.FirstName, c.SAPCode, c.Name, r.Code, j.JobName, j.Design, 
			bo.Name, j.ItemCode, j.DrawingNumber, j.TypeOfOrder, oc.OrderDate,
			js.Circumference, js.FaceWidth, j.CurrencyID,
			curr.CurrencyName, i.RMValue, dereCyl.DeReQty, dereCyl.DeReTotalPrice, cyl.OldQty, cyl.NewQty, cyl.TotalQty, cyl.TotalPrice,
			sjd.ServiceJobs, othC.OtherCharges, oc.TotalPrice, oc.Discount, i.InvoiceNo, i.InvoiceDate, i.CreatedOn, tax.TaxCode, tax.TaxPercentage,
			--iTotal.TotalOverseas, iTotal.TotalRM, 
			i.PaymentTern, oc.TaxPercentage, oc.RMValue, c.CustomerID, c.Name, j.ProductTypeID, prType.Name, j.SalesRepID, s.FirstName, j.RevNumber
	)
	select top (@PageSize) *, (select COUNT(RowIndex) from T) as RowsCount
			from T
			where RowIndex > (@PageIndex*@PageSize) and @PageSize > 0
	union all
	select *, (select COUNT(RowIndex) from T) as RowsCount
			from T
			where @PageSize = 0
--exec Report_SalesReport 0, 0, 0, 0, 4, null, null, null, null, 0, 1000, '0', 'A'
end


/****** Object:  StoredProcedure [dbo].[tblJob_ProgressForDeReChrome]    Script Date: 05/07/2015 15:40:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblJob_ProgressForDeReChrome]
@DeliveryBegin datetime,
@DeliveryEnd datetime,
@DeReDateBegin datetime,
@DeReDateEnd datetime,
@CylinderDateBegin datetime,
@CylinderDateEnd datetime,
@PageIndex int,
@PageSize int,
@SortColumn varchar(2),
@SortType varchar(1),
@JobNumber varchar(10)
as
begin
	;WITH T AS(
	select  TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN  c.Code END ASC,
															CASE @SortColumn WHEN N'1' THEN j.JobNumber END ASC,
															CASE @SortColumn WHEN N'2' THEN j.RevNumber END ASC,
															CASE @SortColumn WHEN N'3' THEN j.JobName END ASC,
															CASE @SortColumn WHEN N'4' THEN j.Design END ASC,
															CASE @SortColumn WHEN N'5' THEN ISNULL(COUNT(cyl.CylinderID),0) END ASC,
															CASE @SortColumn WHEN N'6' THEN p.DeliveryDate END ASC,
															CASE @SortColumn WHEN N'7' THEN p.DeReDate END ASC,
															CASE @SortColumn WHEN N'8' THEN ISNULL(p.CylinderDate, js.CylinderDate) END ASC,
															CASE @SortColumn WHEN N'9' THEN pc.CylinderStatusName END ASC, 
															CASE @SortColumn WHEN N'10' THEN p.Note END ASC) as RowIndex,
			j.JobID, c.Code, j.JobNumber, j.RevNumber, j.JobName, j.Design, ISNULL(COUNT(cyl.CylinderID),0) as Qty, 
			ISNULL(CONVERT(nvarchar(10), p.DeliveryDate, 103),'') as DeliveryDate,
			ISNULL(CONVERT(nvarchar(10), p.DeReDate, 103), '') as DeReChromeDate,
			CONVERT(nvarchar(10), ISNULL(p.CylinderDate, js.CylinderDate), 103) as CylinderDate, pc.CylinderStatusName, p.Note
		from tblJob j INNER JOIN
			tblJobSheet js ON j.JobID = js.JobID INNER JOIN
			tblCustomer c ON j.CustomerID = c.CustomerID LEFT OUTER JOIN
			tblCylinder cyl ON j.JobID = cyl.JobID LEFT OUTER JOIN
			tblCylinderStatus cs ON cyl.CylinderStatusID = cs.CylinderStatusID LEFT OUTER JOIN
			tblProgress p ON j.JobID = p.JobID LEFT OUTER JOIN
			tblProgressReproStatus pr ON p.ReproStatusID = pr.ReproStatusID LEFT OUTER JOIN
			tblProgressCylinderStatus pc ON p.CylinderStatusID = pc.CylinderStatusID
		where j.IsClosed = 0 and j.IsOutsource = 0
			and cs.Action = 'DeReChrome'
			and (DATEDIFF(D, p.DeliveryDate, @DeliveryBegin) <= 0 or @DeliveryBegin is NULL)
				and (DATEDIFF(D, p.DeliveryDate, @DeliveryEnd) >= 0 or @DeliveryEnd is NULL)
			and (DATEDIFF(D, p.DeReDate, @DeReDateBegin) <=0 or @DeReDateBegin IS NULL)
				and (DATEDIFF(D, p.DeReDate, @DeReDateEnd) >=0 or @DeReDateEnd IS NULL)
			and (DATEDIFF(D, ISNULL(p.CylinderDate, js.CylinderDate), @CylinderDateBegin) <=0 or @CylinderDateBegin IS NULL)
				and (DATEDIFF(D, ISNULL(p.CylinderDate, js.CylinderDate), @CylinderDateEnd) >=0 or @CylinderDateEnd IS NULL)
			and (j.JobNumber like '%' + @JobNumber + '%' or @JobNumber is null)
			and @SortType = 'A'
		group by j.JobID, c.Code, j.JobNumber, j.RevNumber, j.JobName, j.Design, j.CreatedOn, p.ReproDate, js.ReproDate, pr.ReproStatusName,
			p.CylinderDate, js.CylinderDate, pc.CylinderStatusName, p.Note, p.DeliveryDate, p.DeReDate
	union all
	select  TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN  c.Code END DESC,
															CASE @SortColumn WHEN N'1' THEN j.JobNumber END DESC,
															CASE @SortColumn WHEN N'2' THEN j.RevNumber END DESC,
															CASE @SortColumn WHEN N'3' THEN j.JobName END DESC,
															CASE @SortColumn WHEN N'4' THEN j.Design END DESC,
															CASE @SortColumn WHEN N'5' THEN ISNULL(COUNT(cyl.CylinderID),0) END DESC,
															CASE @SortColumn WHEN N'6' THEN p.DeliveryDate END DESC,
															CASE @SortColumn WHEN N'7' THEN p.DeReDate END DESC,
															CASE @SortColumn WHEN N'8' THEN ISNULL(p.CylinderDate, js.CylinderDate) END DESC,
															CASE @SortColumn WHEN N'9' THEN pc.CylinderStatusName END DESC, 
															CASE @SortColumn WHEN N'10' THEN p.Note END DESC) as RowIndex,
			j.JobID, c.Code, j.JobNumber, j.RevNumber, j.JobName, j.Design, ISNULL(COUNT(cyl.CylinderID),0) as Qty, 
			ISNULL(CONVERT(nvarchar(10), p.DeliveryDate, 103),'') as DeliveryDate,
			ISNULL(CONVERT(nvarchar(10), p.DeReDate, 103), '') as DeReChromeDate,
			CONVERT(nvarchar(10), ISNULL(p.CylinderDate, js.CylinderDate), 103) as CylinderDate, pc.CylinderStatusName, p.Note
		from tblJob j INNER JOIN
			tblJobSheet js ON j.JobID = js.JobID INNER JOIN
			tblCustomer c ON j.CustomerID = c.CustomerID LEFT OUTER JOIN
			tblCylinder cyl ON j.JobID = cyl.JobID LEFT OUTER JOIN
			tblCylinderStatus cs ON cyl.CylinderStatusID = cs.CylinderStatusID LEFT OUTER JOIN
			tblProgress p ON j.JobID = p.JobID LEFT OUTER JOIN
			tblProgressReproStatus pr ON p.ReproStatusID = pr.ReproStatusID LEFT OUTER JOIN
			tblProgressCylinderStatus pc ON p.CylinderStatusID = pc.CylinderStatusID
		where j.IsClosed = 0 and j.IsOutsource = 0
			and cs.Action = 'DeReChrome'
			and (DATEDIFF(D, p.DeliveryDate, @DeliveryBegin) <= 0 or @DeliveryBegin is NULL)
				and (DATEDIFF(D, p.DeliveryDate, @DeliveryEnd) >= 0 or @DeliveryEnd is NULL)
			and (DATEDIFF(D, p.DeReDate, @DeReDateBegin) <=0 or @DeReDateBegin IS NULL)
				and (DATEDIFF(D, p.DeReDate, @DeReDateEnd) >=0 or @DeReDateEnd IS NULL)
			and (DATEDIFF(D, ISNULL(p.CylinderDate, js.CylinderDate), @CylinderDateBegin) <=0 or @CylinderDateBegin IS NULL)
				and (DATEDIFF(D, ISNULL(p.CylinderDate, js.CylinderDate), @CylinderDateEnd) >=0 or @CylinderDateEnd IS NULL)
			and (j.JobNumber like '%' + @JobNumber + '%' or @JobNumber is null)
			and @SortType = 'D'
		group by j.JobID, c.Code, j.JobNumber, j.RevNumber, j.JobName, j.Design, j.CreatedOn, p.ReproDate, js.ReproDate, pr.ReproStatusName,
			p.CylinderDate, js.CylinderDate, pc.CylinderStatusName, p.Note, p.DeliveryDate, p.DeReDate
	)
	select top (@PageSize)  *, (select COUNT(RowIndex) from T) as RowsCount
		from T
		where @PageSize <> 0 AND RowIndex > (@PageIndex*@PageSize)
	union all
	select *, (select COUNT(RowIndex) from T) as RowsCount
		from T
		where @PageSize = 0
--exec tblJob_ProgressForDeReChrome null, null, null, null, null, null, 0, 0, '0', 'A'	
end				

GO

/****** Object:  StoredProcedure [dbo].[tblJob_ProgressForEngraving]    Script Date: 05/07/2015 15:39:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblJob_ProgressForEngraving]
@DeliveryBegin datetime,
@DeliveryEnd datetime,
@EngravingBegin datetime,
@EngravingEnd datetime,
@ReproStatusID int,
@PageIndex int,
@PageSize int,
@SortColumn varchar(2),
@SortType varchar(1),
@JobNumber varchar(10)
as
begin
	;WITH T AS(
	select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN  c.Code END ASC,
															CASE @SortColumn WHEN N'1' THEN j.JobNumber END ASC,
															CASE @SortColumn WHEN N'2' THEN j.RevNumber END ASC,
															CASE @SortColumn WHEN N'3' THEN j.JobName END ASC,
															CASE @SortColumn WHEN N'4' THEN j.Design END ASC,
															CASE @SortColumn WHEN N'5' THEN ISNULL(COUNT(cyl.CylinderID),0) END ASC,
															CASE @SortColumn WHEN N'6' THEN p.DeliveryDate END ASC,
															CASE @SortColumn WHEN N'7' THEN p.EngravingDate END ASC,
															CASE @SortColumn WHEN N'8' THEN ISNULL(p.ReproDate, js.ReproDate) END ASC,
															CASE @SortColumn WHEN N'9' THEN pr.ReproStatusName END ASC,
															CASE @SortColumn WHEN N'10' THEN ISNULL(p.CylinderDate, js.CylinderDate) END ASC,
															CASE @SortColumn WHEN N'11' THEN pc.CylinderStatusName END ASC, 
															CASE @SortColumn WHEN N'12' THEN p.Note END ASC) as RowIndex, 
			j.JobID, c.Code, j.JobNumber, j.RevNumber, j.JobName, j.Design, ISNULL(COUNT(cyl.CylinderID),0) as Qty,
			ISNULL(CONVERT(nvarchar(10), p.DeliveryDate, 103),'') as DeliveryDate,
			ISNULL(CONVERT(nvarchar(10), p.EngravingDate, 103),'') as EngravingDate,
			CONVERT(nvarchar(10), ISNULL(p.ReproDate, js.ReproDate), 103) as ReproDate, pr.ReproStatusName,
			CONVERT(nvarchar(10), ISNULL(p.CylinderDate, js.CylinderDate), 103) as CylinderDate, pc.CylinderStatusName, p.Note
		from tblJob j INNER JOIN
			tblJobSheet js ON j.JobID = js.JobID INNER JOIN
			tblCustomer c ON j.CustomerID = c.CustomerID LEFT OUTER JOIN
			tblCylinder cyl ON j.JobID = cyl.JobID LEFT OUTER JOIN
			tblCylinderStatus cs ON cyl.CylinderStatusID = cs.CylinderStatusID LEFT OUTER JOIN
			tblProgress p ON j.JobID = p.JobID LEFT OUTER JOIN
			tblProgressReproStatus pr ON p.ReproStatusID = pr.ReproStatusID LEFT OUTER JOIN
			tblProgressCylinderStatus pc ON p.CylinderStatusID = pc.CylinderStatusID
		where j.IsClosed = 0 and j.IsOutsource = 0
			and (cs.Action = 'Repro' or cs.Action = 'Embossing')
			and (ISNULL(pr.GoBackToRepro, 0) = 0 and p.JobID IS NOT NULL and p.ReproStatusID IS NOT NULL)
			and (DATEDIFF(D, p.DeliveryDate, @DeliveryBegin) <= 0 or @DeliveryBegin is NULL)
				and (DATEDIFF(D, p.DeliveryDate, @DeliveryEnd) >= 0 or @DeliveryEnd is NULL)
			and (DATEDIFF(D, p.EngravingDate, @EngravingBegin) <= 0 or @EngravingBegin is NULL)
				and (DATEDIFF(D, p.EngravingDate, @EngravingEnd) >= 0 or @EngravingEnd is NULL)
			and (p.ReproStatusID = @ReproStatusID or @ReproStatusID = 0)
			and (j.JobNumber like '%' + @JobNumber + '%' or @JobNumber is null)
			and @SortType = 'A'
		group by j.JobID, c.Code, j.JobNumber, j.RevNumber, j.JobName, j.Design, j.CreatedOn, p.ReproDate, js.ReproDate, pr.ReproStatusName,
			p.CylinderDate, js.CylinderDate, pc.CylinderStatusName, p.Note, p.DeliveryDate, p.EngravingDate
			
	union all
	select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN  c.Code END DESC,
															CASE @SortColumn WHEN N'1' THEN j.JobNumber END DESC,
															CASE @SortColumn WHEN N'2' THEN j.RevNumber END DESC,
															CASE @SortColumn WHEN N'3' THEN j.JobName END DESC,
															CASE @SortColumn WHEN N'4' THEN j.Design END DESC,
															CASE @SortColumn WHEN N'5' THEN ISNULL(COUNT(cyl.CylinderID),0) END DESC,
															CASE @SortColumn WHEN N'6' THEN p.DeliveryDate END DESC,
															CASE @SortColumn WHEN N'7' THEN p.EngravingDate END DESC,
															CASE @SortColumn WHEN N'8' THEN ISNULL(p.ReproDate, js.ReproDate) END DESC,
															CASE @SortColumn WHEN N'9' THEN pr.ReproStatusName END DESC,
															CASE @SortColumn WHEN N'10' THEN ISNULL(p.CylinderDate, js.CylinderDate) END DESC,
															CASE @SortColumn WHEN N'11' THEN pc.CylinderStatusName END DESC, 
															CASE @SortColumn WHEN N'12' THEN p.Note END DESC) as RowIndex, 
			j.JobID, c.Code, j.JobNumber, j.RevNumber, j.JobName, j.Design, ISNULL(COUNT(cyl.CylinderID),0) as Qty,
			ISNULL(CONVERT(nvarchar(10), p.DeliveryDate, 103),'') as DeliveryDate,
			ISNULL(CONVERT(nvarchar(10), p.EngravingDate, 103),'') as EngravingDate,
			CONVERT(nvarchar(10), ISNULL(p.ReproDate, js.ReproDate), 103) as ReproDate, pr.ReproStatusName,
			CONVERT(nvarchar(10), ISNULL(p.CylinderDate, js.CylinderDate), 103) as CylinderDate, pc.CylinderStatusName, p.Note
		from tblJob j INNER JOIN
			tblJobSheet js ON j.JobID = js.JobID INNER JOIN
			tblCustomer c ON j.CustomerID = c.CustomerID LEFT OUTER JOIN
			tblCylinder cyl ON j.JobID = cyl.JobID LEFT OUTER JOIN
			tblCylinderStatus cs ON cyl.CylinderStatusID = cs.CylinderStatusID LEFT OUTER JOIN
			tblProgress p ON j.JobID = p.JobID LEFT OUTER JOIN
			tblProgressReproStatus pr ON p.ReproStatusID = pr.ReproStatusID LEFT OUTER JOIN
			tblProgressCylinderStatus pc ON p.CylinderStatusID = pc.CylinderStatusID
		where j.IsClosed = 0 and j.IsOutsource = 0
			and (cs.Action = 'Repro' or cs.Action = 'Embossing')
			and (ISNULL(pr.GoBackToRepro, 0) = 0 and p.JobID IS NOT NULL and p.ReproStatusID IS NOT NULL)
			and (DATEDIFF(D, p.DeliveryDate, @DeliveryBegin) <= 0 or @DeliveryBegin is NULL)
				and (DATEDIFF(D, p.DeliveryDate, @DeliveryEnd) >= 0 or @DeliveryEnd is NULL)
			and (DATEDIFF(D, p.EngravingDate, @EngravingBegin) <= 0 or @EngravingBegin is NULL)
				and (DATEDIFF(D, p.EngravingDate, @EngravingEnd) >= 0 or @EngravingEnd is NULL)
			and (p.ReproStatusID = @ReproStatusID or @ReproStatusID = 0)
			and (j.JobNumber like '%' + @JobNumber + '%' or @JobNumber is null)
			and @SortType = 'D'
		group by j.JobID, c.Code, j.JobNumber, j.RevNumber, j.JobName, j.Design, j.CreatedOn, p.ReproDate, js.ReproDate, pr.ReproStatusName,
			p.CylinderDate, js.CylinderDate, pc.CylinderStatusName, p.Note, p.DeliveryDate, p.EngravingDate
	)
	select top (@PageSize)  *, (select COUNT(RowIndex) from T) as RowsCount
		from T
		where @PageSize <> 0 AND RowIndex > (@PageIndex*@PageSize)
	union all
	select *, (select COUNT(RowIndex) from T) as RowsCount
		from T
		where @PageSize = 0		
--exec tblJob_ProgressForEngraving null, null, null, null, 0, 0, 0, '0', 'A'
end

GO

/****** Object:  StoredProcedure [dbo].[tblJob_ProgressForRepro]    Script Date: 05/07/2015 15:17:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblJob_ProgressForRepro]
@OrderDateBegin datetime,
@OrderDateEnd datetime,
@ProofBegin datetime,
@ProofEnd datetime,
@ReproDateBegin datetime,
@ReproDateEnd datetime,
@CylinderDateBegin datetime,
@CylinderDateEnd datetime,
@ReproStatusID int,
@PageIndex int,
@PageSize int,
@SortColumn varchar(2),
@SortType varchar(1),
@JobNumber varchar(10)
as
begin
	;WITH T AS(
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN  c.Code END ASC,
															CASE @SortColumn WHEN N'1' THEN j.JobNumber END ASC,
															CASE @SortColumn WHEN N'2' THEN j.RevNumber END ASC,
															CASE @SortColumn WHEN N'3' THEN j.JobName END ASC,
															CASE @SortColumn WHEN N'4' THEN j.Design END ASC,
															CASE @SortColumn WHEN N'5' THEN ISNULL(COUNT(cyl.CylinderID),0) END ASC,
															CASE @SortColumn WHEN N'6' THEN j.CreatedOn END ASC,
															--CASE @SortColumn WHEN N'7' THEN p.ProofDate END ASC,
															CASE @SortColumn WHEN N'7' THEN ISNULL(p.ReproDate, js.ReproDate) END ASC,
															CASE @SortColumn WHEN N'8' THEN pr.ReproStatusName END ASC,
															CASE @SortColumn WHEN N'9' THEN ISNULL(p.CylinderDate, js.CylinderDate) END ASC,
															CASE @SortColumn WHEN N'10' THEN pc.CylinderStatusName END ASC, 
															CASE @SortColumn WHEN N'11' THEN p.Note END ASC) as RowIndex, 
				j.JobID, c.Code, j.JobNumber, j.RevNumber, j.JobName, j.Design, ISNULL(COUNT(cyl.CylinderID),0) as Qty, 
				CONVERT(nvarchar(10), j.CreatedOn, 103) as CreateOn, 
				ISNULL(CONVERT(nvarchar(10), p.ProofDate, 103),'') as ProofDate,
				CONVERT(nvarchar(10), ISNULL(p.ReproDate, js.ReproDate), 103) as ReproDate, pr.ReproStatusName,
				CONVERT(nvarchar(10), ISNULL(p.CylinderDate, js.CylinderDate), 103) as CylinderDate, pc.CylinderStatusName, p.Note
			from tblJob j INNER JOIN
				tblJobSheet js ON j.JobID = js.JobID INNER JOIN
				tblCustomer c ON j.CustomerID = c.CustomerID LEFT OUTER JOIN
				tblCylinder cyl ON j.JobID = cyl.JobID INNER JOIN
				tblCylinderStatus cst ON cyl.CylinderStatusID = cst.CylinderStatusID LEFT OUTER JOIN
				tblProgress p ON j.JobID = p.JobID LEFT OUTER JOIN
				tblProgressReproStatus pr ON p.ReproStatusID = pr.ReproStatusID LEFT OUTER JOIN
				tblProgressCylinderStatus pc ON p.CylinderStatusID = pc.CylinderStatusID
			where j.IsClosed = 0 and j.IsOutsource = 0 and j.Status <> 'Delivered'
				and (cst.Action = 'Repro' or cst.Action = 'Embossing' or cst.Action = 'DeReChrome')
				and (pr.GoBackToRepro = 1 or p.JobID IS NULL or (p.JobID is NOT NULL and p.ReproStatusID IS NULL))
				and (DATEDIFF(D, j.CreatedOn, @OrderDateBegin) <=0 or @OrderDateBegin IS NULL)
					and (DATEDIFF(D, j.CreatedOn, @OrderDateEnd) >=0 or @OrderDateEnd IS NULL)
				and (DATEDIFF(D, p.ProofDate, @ProofBegin) <=0 or @ProofBegin IS NULL)
					and (DATEDIFF(D, p.ProofDate, @ProofEnd) >=0 or @ProofEnd IS NULL)
				and (DATEDIFF(D, ISNULL(p.ReproDate, js.ReproDate), @ReproDateBegin) <=0 or @ReproDateBegin IS NULL)
					and (DATEDIFF(D, ISNULL(p.ReproDate, js.ReproDate), @ReproDateEnd) >=0 or @ReproDateEnd IS NULL)
				and (DATEDIFF(D, ISNULL(p.CylinderDate, js.CylinderDate), @CylinderDateBegin) <=0 or @CylinderDateBegin IS NULL)
					and (DATEDIFF(D, ISNULL(p.CylinderDate, js.CylinderDate), @CylinderDateEnd) >=0 or @CylinderDateEnd IS NULL)
				and (p.ReproStatusID = @ReproStatusID or @ReproStatusID = 0)
				and (j.JobNumber like '%' + @JobNumber + '%' or @JobNumber is null)
				and @SortType = 'A'
			group by j.JobID, c.Code, j.JobNumber, j.RevNumber, j.JobName, j.Design, j.CreatedOn, p.ReproDate, js.ReproDate, pr.ReproStatusName,
				p.CylinderDate, js.CylinderDate, pc.CylinderStatusName, p.Note, p.ProofDate
		union all
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN  c.Code END DESC,
															CASE @SortColumn WHEN N'1' THEN j.JobNumber END DESC,
															CASE @SortColumn WHEN N'2' THEN j.RevNumber END DESC,
															CASE @SortColumn WHEN N'3' THEN j.JobName END DESC,
															CASE @SortColumn WHEN N'4' THEN j.Design END DESC,
															CASE @SortColumn WHEN N'5' THEN ISNULL(COUNT(cyl.CylinderID),0) END DESC,
															CASE @SortColumn WHEN N'6' THEN j.CreatedOn END DESC,
															--CASE @SortColumn WHEN N'7' THEN p.ProofDate END DESC,
															CASE @SortColumn WHEN N'7' THEN ISNULL(p.ReproDate, js.ReproDate) END DESC,
															CASE @SortColumn WHEN N'8' THEN pr.ReproStatusName END ASC,
															CASE @SortColumn WHEN N'9' THEN ISNULL(p.CylinderDate, js.CylinderDate) END DESC,
															CASE @SortColumn WHEN N'10' THEN pc.CylinderStatusName END DESC, 
															CASE @SortColumn WHEN N'11' THEN p.Note END DESC) as RowIndex, 
				j.JobID, c.Code, j.JobNumber, j.RevNumber, j.JobName, j.Design, ISNULL(COUNT(cyl.CylinderID),0) as Qty, 
				CONVERT(nvarchar(10), j.CreatedOn, 103) as CreateOn, 
				ISNULL(CONVERT(nvarchar(10), p.ProofDate, 103),'') as ProofDate,
				CONVERT(nvarchar(10), ISNULL(p.ReproDate, js.ReproDate), 103) as ReproDate, pr.ReproStatusName,
				CONVERT(nvarchar(10), ISNULL(p.CylinderDate, js.CylinderDate), 103) as CylinderDate, pc.CylinderStatusName, p.Note
			from tblJob j INNER JOIN
				tblJobSheet js ON j.JobID = js.JobID INNER JOIN
				tblCustomer c ON j.CustomerID = c.CustomerID LEFT OUTER JOIN
				tblCylinder cyl ON j.JobID = cyl.JobID INNER JOIN
				tblCylinderStatus cst ON cyl.CylinderStatusID = cst.CylinderStatusID LEFT OUTER JOIN
				tblProgress p ON j.JobID = p.JobID LEFT OUTER JOIN
				tblProgressReproStatus pr ON p.ReproStatusID = pr.ReproStatusID LEFT OUTER JOIN
				tblProgressCylinderStatus pc ON p.CylinderStatusID = pc.CylinderStatusID
			where j.IsClosed = 0 and j.IsOutsource = 0 and j.Status <> 'Delivered'
				and (cst.Action = 'Repro' or cst.Action = 'Embossing' or cst.Action = 'DeReChrome')
				and (pr.GoBackToRepro = 1 or p.JobID IS NULL or (p.JobID is NOT NULL and p.ReproStatusID IS NULL))
				and (DATEDIFF(D, j.CreatedOn, @OrderDateBegin) <=0 or @OrderDateBegin IS NULL)
					and (DATEDIFF(D, j.CreatedOn, @OrderDateEnd) >=0 or @OrderDateEnd IS NULL)
				and (DATEDIFF(D, p.ProofDate, @ProofBegin) <=0 or @ProofBegin IS NULL)
					and (DATEDIFF(D, p.ProofDate, @ProofEnd) >=0 or @ProofEnd IS NULL)
				and (DATEDIFF(D, ISNULL(p.ReproDate, js.ReproDate), @ReproDateBegin) <=0 or @ReproDateBegin IS NULL)
					and (DATEDIFF(D, ISNULL(p.ReproDate, js.ReproDate), @ReproDateEnd) >=0 or @ReproDateEnd IS NULL)
				and (DATEDIFF(D, ISNULL(p.CylinderDate, js.CylinderDate), @CylinderDateBegin) <=0 or @CylinderDateBegin IS NULL)
					and (DATEDIFF(D, ISNULL(p.CylinderDate, js.CylinderDate), @CylinderDateEnd) >=0 or @CylinderDateEnd IS NULL)
				and (p.ReproStatusID = @ReproStatusID or @ReproStatusID = 0)
				and (j.JobNumber like '%' + @JobNumber + '%' or @JobNumber is null)
				and @SortType = 'D'
			group by j.JobID, c.Code, j.JobNumber, j.RevNumber, j.JobName, j.Design, j.CreatedOn, p.ReproDate, js.ReproDate, pr.ReproStatusName,
				p.CylinderDate, js.CylinderDate, pc.CylinderStatusName, p.Note, p.ProofDate
	)
	select top (@PageSize)  *, (select COUNT(RowIndex) from T) as RowsCount
		from T
		where @PageSize <> 0 AND RowIndex > (@PageIndex*@PageSize)
	union all
	select *, (select COUNT(RowIndex) from T) as RowsCount
		from T
		where @PageSize = 0
--exec tblJob_ProgressForRepro null, null, null, null, null, null, null, null, 0, 0, 1000, '0', 'A'
end

GO
/****** Object:  StoredProcedure [dbo].[Report_RemakeReport]    Script Date: 07/05/2015 21:09:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[Report_RemakeReport]
@FromDate datetime,
@ToDate datetime
as
begin
	select ROW_NUMBER() OVER(ORDER BY j.JobID) as RowNumber,
			CONVERT(nvarchar(10), j.CreatedOn, 103) as Date, LEFT(s.FirstName, 2) as S, LEFT(coor.FirstName, 2) as C, 
			j.JobNumber + ' R ' + CAST(j.RevNumber as varchar(5)) as JobNumber, c.Code, j.JobName, j.Design,  		
			LEFT(j.InternalExternal,1) as IE, ISNULL(CBS.Qty, 0) as CByS, ISNULL(CBR.Qty, 0) CByR, 
			ISNULL(CBP.Qty, 0) as CByP, ISNULL(CBO.Qty, 0) as CByO, j.RevisionDetail
		from tblJob j INNER JOIN
			tblCustomer c ON j.CustomerID = c.CustomerID LEFT OUTER JOIN
			tblStaff s ON j.SalesRepID = s.StaffID LEFT OUTER JOIN
			tblStaff coor ON j.CoordinatorID = coor.StaffID LEFT OUTER JOIN
			(select sc.JobID, ISNULL(COUNT(sc.CylinderID),0) as Qty 
				from tblJob sj INNER JOIN
						tblCylinder sc ON sj.JobID = sc.JobID
				where sc.Dept = 'S'
						and (DATEDIFF(D, sj.CreatedOn, @FromDate) <= 0 or @FromDate is NULL) 
						and (DATEDIFF(D, sj.CreatedOn, @ToDate) >= 0 or @ToDate is NULL) 
						and sj.RevNumber != 0
				group by sc.JobID) as CBS ON j.JobID = CBS.JobID  LEFT OUTER JOIN
			(select sc.JobID, ISNULL(COUNT(sc.CylinderID),0) as Qty 
				from tblJob sj INNER JOIN
						tblCylinder sc ON sj.JobID = sc.JobID
				where sc.Dept = 'R'
						and (DATEDIFF(D, sj.CreatedOn, @FromDate) <= 0 or @FromDate is NULL) 
						and (DATEDIFF(D, sj.CreatedOn, @ToDate) >= 0 or @ToDate is NULL) 
						and sj.RevNumber != 0
				group by sc.JobID) as CBR ON j.JobID = CBR.JobID LEFT OUTER JOIN
			(select sc.JobID, ISNULL(COUNT(sc.CylinderID),0) as Qty 
				from tblJob sj INNER JOIN
						tblCylinder sc ON sj.JobID = sc.JobID
				where sc.Dept = 'P'
						and (DATEDIFF(D, sj.CreatedOn, @FromDate) <= 0 or @FromDate is NULL) 
						and (DATEDIFF(D, sj.CreatedOn, @ToDate) >= 0 or @ToDate is NULL) 
						and sj.RevNumber != 0
				group by sc.JobID) as CBP ON j.JobID = CBP.JobID LEFT OUTER JOIN
			(select sc.JobID, ISNULL(COUNT(sc.CylinderID),0) as Qty 
				from tblJob sj INNER JOIN
						tblCylinder sc ON sj.JobID = sc.JobID
				where sc.Dept  not in ('S', 'R', 'P') and sc.Dept <> '.'
						and (DATEDIFF(D, sj.CreatedOn, @FromDate) <= 0 or @FromDate is NULL) 
						and (DATEDIFF(D, sj.CreatedOn, @ToDate) >= 0 or @ToDate is NULL) 
						and sj.RevNumber != 0
				group by sc.JobID) as CBO ON j.JobID = CBO.JobID
		where (DATEDIFF(D, j.CreatedOn, @FromDate) <= 0 or @FromDate is NULL) 
				and (DATEDIFF(D, j.CreatedOn, @ToDate) >= 0 or @ToDate is NULL) 
				and j.RevNumber != 0
		order by j.CreatedOn
--exec Report_RemakeReport null, null
end

GO
/****** Object:  StoredProcedure [dbo].[TblConfirmOrder_SelectAll]    Script Date: 07/05/2015 22:03:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER proc [dbo].[TblConfirmOrder_SelectAll]
@Customer nvarchar(200),
@Job nvarchar(100),
@OCNumber nvarchar(100),
@FromDate datetime,
@ToDate datetime,
@PageIndex int,
@PageSize int,
@SortColumn varchar(2),
@SortType varchar(1)
AS
BEGIN
	;WITH T AS(
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY CASE @SortColumn WHEN N'0' THEN  o.OCNumber END ASC,
															CASE @SortColumn WHEN N'1' THEN  o.OrderDate END ASC,
															CASE @SortColumn WHEN N'2' THEN  c.Code END ASC,
															CASE @SortColumn WHEN N'3' THEN  c.Name END ASC,
															CASE @SortColumn WHEN N'4' THEN  j.JobNumber END ASC,
															CASE @SortColumn WHEN N'5' THEN  j.JobName END ASC) as RowIndex,
			o.JobID, o.OCNumber, CONVERT(nvarchar(10), o.OrderDate, 103) as OrderDate, 
			c.Code, c.Name, j.JobNumber, j.RevNumber, j.JobName, ol.Locking AS Lock
		from  tblOrderConfirmation o INNER JOIN
				tblJob j ON o.JobID = j.JobID INNER JOIN 
				tblCustomer c ON j.CustomerID = c.CustomerID LEFT OUTER JOIN
				tblObjectLocking ol ON ol.ID = j.JobID AND ol.[Type] = N'OC'
		where (c.Name like '%' + @Customer + '%' or c.Code like '%' + @Customer + '%')
				and (j.JobNumber like '%' + @Job + '%')
				and (o.OCNumber like '%' + @OCNumber + '%')
				and (DATEDIFF(D, o.OrderDate, @FromDate) <= 0 or @FromDate IS NULL)
				and (DATEDIFF(D, o.OrderDate, @ToDate) >= 0 or @ToDate IS NULL)
				and @SortType = 'A'
		union all
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY CASE @SortColumn WHEN N'0' THEN  o.OCNumber END DESC,
															CASE @SortColumn WHEN N'1' THEN  o.OrderDate END DESC,
															CASE @SortColumn WHEN N'2' THEN  c.Code END DESC,
															CASE @SortColumn WHEN N'3' THEN  c.Name END DESC,
															CASE @SortColumn WHEN N'4' THEN  j.JobNumber END DESC,
															CASE @SortColumn WHEN N'5' THEN  j.JobName END DESC) as RowIndex,
			o.JobID, o.OCNumber, CONVERT(nvarchar(10), o.OrderDate, 103) as OrderDate, 
			c.Code, c.Name, j.JobNumber, j.RevNumber, j.JobName, ol.Locking AS Lock
		from  tblOrderConfirmation o INNER JOIN
				tblJob j ON o.JobID = j.JobID INNER JOIN 
				tblCustomer c ON j.CustomerID = c.CustomerID LEFT OUTER JOIN
				tblObjectLocking ol ON ol.ID = j.JobID AND ol.[Type] = N'OC'
		where (c.Name like '%' + @Customer + '%' or c.Code like '%' + @Customer + '%')
				and (j.JobNumber like '%' + @Job + '%')
				and (o.OCNumber like '%' + @OCNumber + '%')
				and (DATEDIFF(D, o.OrderDate, @FromDate) <= 0 or @FromDate IS NULL)
				and (DATEDIFF(D, o.OrderDate, @ToDate) >= 0 or @ToDate IS NULL)
				and @SortType = 'D'				
		)
		select top (@PageSize) *, (select COUNT(RowIndex) from T) as RowsCount
		from T
		where RowIndex > (@PageIndex*@PageSize)	
--exec [TblConfirmOrder_SelectAll] '', '', '', null, null, 0, 10, '0', 'A'
END

GO
/****** Object:  StoredProcedure [dbo].[SearchDeliveryOrderByCustomer]    Script Date: 07/05/2015 22:05:48 PM ******/
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
				c.Name, c.Code, j.JobName, j.JobNumber, j.RevNumber, ol.Locking AS Lock
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
				c.Name, c.Code, j.JobName, j.JobNumber, j.RevNumber, ol.Locking AS Lock
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

GO
/****** Object:  StoredProcedure [dbo].[tblDelivery_SelectSummaryForPrint]    Script Date: 07/05/2015 22:20:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblDelivery_SelectSummaryForPrint]
@JobID int
as
begin
	select c.Name as CustomerName, c.Address as AddressLine1, c.PostCode + ', ' + c.City as AddressLine2, country.Name as AddressLine3,
			CASE WHEN LEN(d.CustomerPO1) <> 1 and LEN(d.CustomerPO2) <> 0 THEN d.CustomerPO1 + ', ' + d.CustomerPO2
				WHEN LEN(d.CustomerPO1) <> 1 and LEN(d.CustomerPO2) = 0 THEN d.CustomerPO1
				WHEN LEN(d.CustomerPO1) = 0 and LEN(d.CustomerPO2) <> 0 THEN d.CustomerPO2
				ELSE '' END as YourReference, 
			d.DONumber + ' / ' + CONVERT(nvarchar(10), d.OrderDate, 104) as DONumber,
			j.JobNumber + (CASE WHEN j.RevNumber > 0 THEN ' (R' + CAST(j.RevNumber as nvarchar(10)) + ')' ELSE '' END) + ' / ' + CONVERT(nvarchar(10), j.CreatedOn, 104) as JobNumber,
			j.JobName, j.Design, d.OtherItem as Remark, r.Name as PackingName, 
			d.GrossWeigth as GrossWeight, d.NetWeight, s.FirstName + ' ' + s.LastName as CreatedBy
		from tblJob j INNER JOIN
			tblCustomer c ON j.ShipToParty = c.CustomerID INNER JOIN
			tblReferences country ON c.CountryID = country.ReferencesID INNER JOIN
			tblDeliveryOrder d ON j.JobID = d.JobID LEFT OUTER JOIN
			tblReferences r ON r.ReferencesID = d.PackingID LEFT OUTER JOIN
			tblUser u ON d.CreatedBy = u.UserName LEFT OUTER JOIN
			tblStaff s ON u.UserID = s.StaffID
		where d.JobID = @JobID
--exec tblDelivery_SelectSummaryForPrint 154
end
