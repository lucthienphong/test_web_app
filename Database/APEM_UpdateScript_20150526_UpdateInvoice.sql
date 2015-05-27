
/****** Object:  StoredProcedure [dbo].[tblInvoice_SelectForExport]    Script Date: 05/26/2015 14:20:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblInvoice_SelectForExport]
@InvoiceID nvarchar(MAX)
AS
BEGIN
	SELECT i.InvoiceNo, c.SAPCode, CONVERT(nvarchar(8), i.InvoiceDate, 112) as InvoiceDate, curr.CurrencyName, 
			CASE WHEN t.TaxID IS NOT NULL THEN 'X' ELSE '' END AS CalcTax, t.TaxCode, i.NetTotal as TotalPrice, i.RMValue, i.CreatedOn
		FROM tblInvoice i INNER JOIN
			tblCustomer c ON i.CustomerID = c.CustomerID INNER JOIN
			tblCurrency curr ON i.CurrencyID = curr.CurrencyID LEFT OUTER JOIN
			tblTax t ON i.TaxID = t.TaxID
		WHERE @InvoiceID like '%-' + CAST(i.InvoiceID as nvarchar(5)) + '-%'
	--exec tblInvoice_SelectForExport '-22-'
END