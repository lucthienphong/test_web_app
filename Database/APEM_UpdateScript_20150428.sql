GO
/****** Object:  StoredProcedure [dbo].[Report_SalesReport]    Script Date: 29/04/2015 08:06:54 AM ******/
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
			i.PaymentTern, c.CustomerID, c.Name as CustomerName, j.ProductTypeID, prType.Name as ProductTypeName, j.SalesRepID, s.FirstName
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
			i.PaymentTern, oc.TaxPercentage, oc.RMValue, c.CustomerID, c.Name, j.ProductTypeID, prType.Name, j.SalesRepID, s.FirstName
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


GO
/****** Object:  StoredProcedure [dbo].[tblInvocie_SelectSummary]    Script Date: 28/04/2015 23:14:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblInvocie_SelectSummary]
@InvoiceID int
as
begin
	;WITH T AS(
		select i.InvoiceID, SUM(ROUND(o.TotalPrice * (1 - o.Discount/100),2)) as SubTotal, 
			SUM(ROUND(CASE WHEN o.TaxPercentage != 0 THEN (o.TotalPrice * (1 - (o.Discount/100))) * o.TaxPercentage/100 
					ELSE 0 END, 2)) TotalTax, o.TaxPercentage as TotalZeroTax, COUNT(o.JobID) as QtyOfJob,
			SUM(ROUND((o.TotalPrice * (1 - (o.Discount/100))) * (1 + o.TaxPercentage/100), 2)) as TotalAmount
			from tblInvoice i INNER JOIN
				tblInvoiceDetail id ON i.InvoiceID = id.InvoiceID INNER JOIN
				tblOrderConfirmation o ON id.JobID = o.JobID	
			group by i.InvoiceID, o.TaxPercentage			
	)
	
	select c.Name as CustomerName, c.Address as AddressLine1, c.PostCode + ', ' + c.City as AddressLine2, rf.Name as AddressLine3,
		c.SAPCode, c.TaxCode, sp.Name as ShipName, sp.Address as ShipAddress1, sp.PostCode + ', ' + sp.City as ShipAddress2, sp.CountryName as ShipAddress3,
		i.InvoiceNo + ' / ' + CONVERT(nvarchar(10), i.InvoiceDate, 104) as InvoiceNo, ct.ContactName,
		CONVERT(nvarchar(10), i.InvoiceDate, 104) as InvoiceDate, cr.CurrencyName, i.RMValue, i.PONumber, i.PaymentTern,
		i.Remark, T.QtyOfJob, T.SubTotal, T.TotalZeroTax, T.TotalTax, T.TotalAmount, c.TIN
		from tblInvoice i INNER JOIN
			tblCustomer c ON i.CustomerID = c.CustomerID INNER JOIN
			tblReferences rf ON c.CountryID = rf.ReferencesID LEFT OUTER JOIN
			tblContact ct ON i.ContactID = ct.ContactID LEFT OUTER JOIN
			tblCurrency cr ON i.CurrencyID = cr.CurrencyID LEFT OUTER JOIN		
			(select top 1 jc.Name, jc.Address, jc.PostCode, jc.City, country.Name as CountryName, jid.InvoiceID 
				from tblJob j INNER JOIN
					tblCustomer jc ON j.ShipToParty = jc.CustomerID INNER JOIN
					tblReferences country ON jc.CountryID = country.ReferencesID INNER JOIN 
					tblInvoiceDetail jid ON jid.JobID = j.JobID
				where jid.InvoiceID = @InvoiceID) as sp ON i.InvoiceID = sp.InvoiceID LEFT OUTER JOIN
			T ON i.InvoiceID = T.InvoiceID
		where i.InvoiceID = @InvoiceID
--exec tblInvocie_SelectSummary 13
end