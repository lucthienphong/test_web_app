GO
alter proc tblInvocie_SelectSummary
@InvoiceID int
as
begin
	;WITH T AS(
		select i.InvoiceID, SUM(o.TotalPrice) as SubTotal, 
			SUM(CASE WHEN o.TaxPercentage != 0 THEN (o.TotalPrice * (1 - (o.Discount/100))) * o.TaxPercentage/100 
					ELSE 0 END) TotalTax, 0 as TotalZeroTax, COUNT(o.JobID) as QtyOfJob,
			SUM((o.TotalPrice * (1 - (o.Discount/100))) * (1 + o.TaxPercentage/100)) as TotalAmount
			from tblInvoice i INNER JOIN
				tblInvoiceDetail id ON i.InvoiceID = id.InvoiceID INNER JOIN
				tblOrderConfirmation o ON id.JobID = o.JobID	
			group by i.InvoiceID			
	)
	
	select c.Name as CustomerName, c.Address as AddressLine1, c.PostCode + ', ' + c.City as AddressLine2, rf.Name as AddressLine3,
		c.SAPCode, c.TaxCode, sp.Name as ShipName, sp.Address as ShipAddress1, sp.PostCode + ', ' + sp.City as ShipAddress2, sp.CountryName as ShipAddress3,
		i.InvoiceNo + ' / ' + CONVERT(nvarchar(10), i.InvoiceDate, 104) as InvoiceNo, ct.ContactName,
		CONVERT(nvarchar(10), i.InvoiceDate, 104) as InvoiceDate, cr.CurrencyName, i.RMValue, i.PONumber, i.PaymentTern,
		i.Remark, T.QtyOfJob, T.SubTotal, T.TotalZeroTax, T.TotalTax, T.TotalAmount
		from tblInvoice i INNER JOIN
			tblCustomer c ON i.CustomerID = c.CustomerID INNER JOIN
			tblReferences rf ON c.CountryID = rf.ReferencesID LEFT OUTER JOIN
			tblContact ct ON i.ContactID = ct.ContactID LEFT OUTER JOIN
			tblCurrency cr ON i.CurrencyID = cr.CurrencyID LEFT OUTER JOIN		
			(select top 1 jc.Name, jc.Address, jc.PostCode, jc.City, country.Name as CountryName, jid.InvoiceID 
				from tblJob j INNER JOIN
					tblCustomer jc ON j.ShipToParty = jc.CustomerID INNER JOIN
					tblReferences country ON jc.CountryID = country.ReferencesID INNER JOIN 
					tblInvoiceDetail jid ON jid.JobID = j.JobID) as sp ON i.InvoiceID = sp.InvoiceID LEFT OUTER JOIN
			T ON i.InvoiceID = T.InvoiceID
		where i.InvoiceID = @InvoiceID
--exec tblInvocie_SelectSummary 10
end

GO
alter proc tblInvoice_SelectJobSummary
@JobID int
as
begin
	select j.JobName, j.Design, d.DONumber + ' / ' + CONVERT(nvarchar(10), d.OrderDate, 104) as DONumber, d.OrderDate, 
			j.JobNumber + ' / ' + CONVERT(nvarchar(10), j.CreatedOn, 104) as JobNumber, j.CreatedOn,
			CASE WHEN LEN(d.CustomerPO1) <> 1 and LEN(d.CustomerPO2) <> 0 THEN d.CustomerPO1 + ', ' + d.CustomerPO2
				WHEN LEN(d.CustomerPO1) <> 1 and LEN(d.CustomerPO2) = 0 THEN d.CustomerPO1
				WHEN LEN(d.CustomerPO1) = 0 and LEN(d.CustomerPO2) <> 0 THEN d.CustomerPO2
				ELSE '' END as YourReference, o.TaxPercentage as TaxRate, o.Discount as DiscountRate,
				o.TotalPrice as SubTotal, o.TotalPrice * o.Discount / 100 as Discount, 
				o.TotalPrice * (1 - (o.Discount/100)) as SubTotalBeforGST,
				(o.TotalPrice * (1 - (o.Discount/100))) * o.TaxPercentage / 100 as Tax,
				(o.TotalPrice * (1 - (o.Discount/100))) * (1 + (o.TaxPercentage/100)) as Total
		from tblJob j INNER JOIN
			tblOrderConfirmation o ON j.JobID = o.JobID INNER JOIN
			tblDeliveryOrder d ON j.JobID = d.JobID
		where j.JobID = @JobID
--exec tblInvoice_SelectJobSummary 173	
end

GO
alter proc tblInvoice_SelectJobOtherCharges
@JobID int
as
begin
	select ROW_NUMBER() OVER(ORDER BY OtherChargesID) as Sequence,
			Description, Quantity, Charge, Quantity * Charge as Total
		from tblOtherCharges
		where JobID = @JobID
end
