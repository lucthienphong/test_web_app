GO
/****** Object:  StoredProcedure [dbo].[Report_SalesReport]    Script Date: 20/04/2015 22:01:24 PM ******/
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
	select ROW_NUMBER() OVER(ORDER BY j.JobNumber) as RowIndex,
			j.JobNumber, c.SAPCode, r.Code, j.JobName, j.Design, js.Circumference, js.FaceWidth,
			curr.CurrencyName, curr.RMValue, ISNULL(dereCyl.DeReQty, 0) as DeReQty, ISNULL(dereCyl.DeReTotalPrice, 0) as DeReTotalPrice, 
			cyl.OldQty, cyl.NewQty, cyl.TotalQty, cyl.TotalPrice, 
			ISNULL(sjd.ServiceJobs, 0) as ServiceJobs, ISNULL(othC.OtherCharges, 0) as OtherCharges, oc.Discount,
			oc.TotalPrice * (1 - oc.Discount/100) as SubTotal, i.InvoiceNo, CONVERT(nvarchar(10), i.InvoiceDate, 103) as InvoiceDate, 
			CONVERT(nvarchar(10), i.CreatedOn, 103) as InvoicePostingDate, tax.TaxCode, tax.TaxPercentage,
			--iTotal.TotalOverseas as TotalOverseas, iTotal.TotalRM as TotalMY, 
			CASE WHEN j.CurrencyID <>  @BaseCurrencyID THEN ROUND((oc.TotalPrice * (1 - oc.Discount/100)) * (1 + oc.TaxPercentage/100),2)
					ELSE 0 END as TotalOverseas,
			ROUND((oc.TotalPrice * (1 - oc.Discount/100)) * (1 + oc.TaxPercentage/100) * oc.RMValue,2) as TotalRM,
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
		group by j.JobNumber, s.FirstName, c.SAPCode, c.Name, r.Code, j.JobName, j.Design, js.Circumference, js.FaceWidth, j.CurrencyID,
			curr.CurrencyName, curr.RMValue, dereCyl.DeReQty, dereCyl.DeReTotalPrice, cyl.OldQty, cyl.NewQty, cyl.TotalQty, cyl.TotalPrice,
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
--exec Report_SalesReport 0, 0, 0, 0, 4, null, null, '2015/05/01', '2015/05/09', 0, 10, '0', 'A'
end

GO
/****** Object:  Table [dbo].[tblOrderLocking]    Script Date: 21/04/2015 00:23:40 AM ******/
DROP TABLE [dbo].[tblOrderLocking]
GO

/****** Object:  Table [dbo].[tblOrderLocking]    Script Date: 21/04/2015 00:23:40 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tblOrderLocking](
	[ID] [int] NOT NULL,
	[Type] [varchar](50) NOT NULL,
 CONSTRAINT [PK_tblOrderLocking] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[Type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


GO
/****** Object:  StoredProcedure [dbo].[tblInvoice_SelectDetailForExport]    Script Date: 21/04/2015 07:55:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblInvoice_SelectDetailForExport]
@InvoiceID nvarchar(MAX)
AS
BEGIN
	WITH T AS(
		--Cylinder
		SELECT i.InvoiceNo, cqd.GLCode,
				((CASE cqd.UnitOfMeasure WHEN 'cm2' 
						THEN (c.FaceWidth*c.Circumference/100)*ISNULL(c.UnitPrice,0) 
						ELSE ISNULL(c.UnitPrice,0) END) * (1 - oc.Discount/100)) * (1 + oc.TaxPercentage/100) AS TotalPrice,
				ISNULL(t.TaxCode, '') AS TaxCode, LEFT(ct.Name, 35) + ': ' + i.InvoiceNo as [Description], ct.InternalOrderNo as JobNumber
			FROM tblInvoice i INNER JOIN
				tblCustomer ct ON i.CustomerID = ct.CustomerID INNER JOIN
				tblInvoiceDetail id ON i.InvoiceID = id.InvoiceID INNER JOIN
				tblJob j ON id.JobID = j.JobID INNER JOIN
				tblOrderConfirmation oc ON j.JobID = oc.JobID INNER JOIN
				tblCylinder c ON id.JobID = c.JobID INNER JOIN
				tblCustomerQuotationDetail cqd ON c.PricingID = cqd.ID LEFT OUTER JOIN
				tblTax t ON oc.TaxID = t.TaxID
			WHERE @InvoiceID like '%-' + CAST(i.InvoiceID as nvarchar(5)) + '-%'
		UNION ALL
		--SERVICEJOB
		SELECT i.InvoiceNo, cqa.GLCode, (s.WorkOrderValues * (1 - oc.Discount/100)) * (1 + oc.TaxPercentage/100) AS TotalPrice,
				ISNULL(t.TaxCode, '') AS TaxCode, LEFT(ct.Name, 35) + ': ' + i.InvoiceNo as [Description], ct.InternalOrderNo as JobNumber
			FROM tblInvoice i INNER JOIN
				tblCustomer ct ON i.CustomerID = ct.CustomerID INNER JOIN
				tblInvoiceDetail id ON i.InvoiceID = id.InvoiceID INNER JOIN
				tblJob j ON id.JobID = j.JobID INNER JOIN
				tblOrderConfirmation oc ON j.JobID = oc.JobID INNER JOIN
				tblServiceJobDetail s ON id.JobID = s.JobID INNER JOIN
				tblCustomerQuotation_AdditionalService cqa ON s.PricingID = cqa.ID LEFT OUTER JOIN
				tblTax t ON oc.TaxID = t.TaxID
			WHERE @InvoiceID like '%-' + CAST(i.InvoiceID as nvarchar(5)) + '-%'
		UNION ALL
		--ORTHER CHARGES
		SELECT i.InvoiceNo, cqo.GLCode, (o.Charge * o.Quantity * (1 - oc.Discount/100)) * (1 + oc.TaxPercentage/100) AS TotalPrice,
				ISNULL(t.TaxCode, '') AS TaxCode, LEFT(ct.Name, 35) + ': ' + i.InvoiceNo as [Description], ct.InternalOrderNo as JobNumber
			FROM tblInvoice i INNER JOIN
				tblCustomer ct ON i.CustomerID = ct.CustomerID INNER JOIN
				tblInvoiceDetail id ON i.InvoiceID = id.InvoiceID INNER JOIN
				tblJob j ON id.JobID = j.JobID INNER JOIN
				tblOrderConfirmation oc ON j.JobID = oc.JobID INNER JOIN
				tblOtherCharges o ON id.JobID = o.JobID INNER JOIN
				tblCustomerQuotation_OtherCharges cqo ON o.PricingID = cqo.ID LEFT OUTER JOIN
				tblTax t ON oc.TaxID = t.TaxID
			WHERE @InvoiceID like '%-' + CAST(i.InvoiceID as nvarchar(5)) + '-%'
	)
	SELECT InvoiceNo, GLCode, SUM(TotalPrice) as Total, TaxCode, [Description], JobNumber
		FROM T
		GROUP BY InvoiceNo, GLCode, TaxCode, JobNumber, [Description]
	--exec tblInvoice_SelectDetailForExport '-17-'
END
