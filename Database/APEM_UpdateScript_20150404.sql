USE [SweetSoft_APEM]
GO
IF COL_LENGTH('tblEngraving','EngrStartEtching') IS NULL
BEGIN
	ALTER Table tblEngraving
		ADD EngrStartEtching float NULL
END

GO
IF COL_LENGTH('tblEngraving','EngrWidthEtching') IS NULL
BEGIN
	ALTER Table tblEngraving
		ADD EngrWidthEtching float NULL
END

GO
IF COL_LENGTH('tblJobSheet','ActualPrintingMaterial') IS NULL
BEGIN
	ALTER Table tblJobSheet
		ADD ActualPrintingMaterial nvarchar(100) NULL
END

GO
IF COL_LENGTH('tblJobSheet','MaterialWidth') IS NULL
BEGIN
	ALTER Table tblJobSheet
		ADD MaterialWidth nvarchar(100) NULL
END

GO
IF COL_LENGTH('tblEngravingTobacco','Gamma') IS NOT NULL
BEGIN
	ALTER Table tblEngravingTobacco
		ALTER column Gamma nvarchar(100) NULL
END

GO
IF COL_LENGTH('TblCurrencyChangedLog','NewValue') IS NOT NULL
BEGIN
	ALTER table TblCurrencyChangedLog
		alter column NewValue decimal(18, 4)
END

GO
IF COL_LENGTH('TblCurrencyChangedLog','NewRMValue') IS NOT NULL
BEGIN
	ALTER table TblCurrencyChangedLog
		alter column NewRMValue decimal(18, 4)
END

GO
IF COL_LENGTH('tblOrderConfirmation','RMValue') IS NOT NULL
BEGIN
	ALTER table tblOrderConfirmation
		alter column RMValue decimal(18, 4)
END

GO
IF COL_LENGTH('tblOrderConfirmation','CurrencyValue') IS NOT NULL
BEGIN
	ALTER table tblOrderConfirmation
		alter column CurrencyValue decimal(18, 4)
END

GO
IF COL_LENGTH('tblInvoice','RMValue') IS NOT NULL
BEGIN
	ALTER table tblInvoice
		alter column RMValue decimal(18, 4)
END

GO
IF COL_LENGTH('tblInvoice','CurrencyValue') IS NOT NULL
BEGIN
	ALTER table tblInvoice
		alter column CurrencyValue decimal(18, 4)
END

GO
IF NOT EXISTS(select top 1 * from tblFunction where FunctionID = 'job_engraving_manager')
begin
	Insert into tblFunction values('job_engraving_manager', 'fJob', 'Engraving Manager', '', 6)
end

GO
/****** Object:  StoredProcedure [dbo].[tblJob_ProgressForRepro]    Script Date: 04/04/2015 10:54:21 ******/
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
@SortType varchar(1)
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
				tblCylinder cyl ON j.JobID = cyl.JobID LEFT OUTER JOIN
				tblProgress p ON j.JobID = p.JobID LEFT OUTER JOIN
				tblProgressReproStatus pr ON p.ReproStatusID = pr.ReproStatusID LEFT OUTER JOIN
				tblProgressCylinderStatus pc ON p.CylinderStatusID = pc.CylinderStatusID
			where j.IsClosed = 0 and j.IsOutsource = 0
				and cyl.ProcessTypeID <> (Select ISNULL(CAST(CAST(SettingValue as nvarchar(4)) as int),-1) from tblSystemSetting 
					where SettingName = 'SweetSoft.APEM.Settings.DeReChromeSetting')
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
				tblCylinder cyl ON j.JobID = cyl.JobID LEFT OUTER JOIN
				tblCylinderStatus cst ON cyl.CylinderStatusID = cst.CylinderStatusID LEFT OUTER JOIN
				tblProgress p ON j.JobID = p.JobID LEFT OUTER JOIN
				tblProgressReproStatus pr ON p.ReproStatusID = pr.ReproStatusID LEFT OUTER JOIN
				tblProgressCylinderStatus pc ON p.CylinderStatusID = pc.CylinderStatusID
			where j.IsClosed = 0 and j.IsOutsource = 0
				and cyl.ProcessTypeID <> (Select ISNULL(CAST(CAST(SettingValue as nvarchar(4)) as int),-1) from tblSystemSetting 
					where SettingName = 'SweetSoft.APEM.Settings.DeReChromeSetting')
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
--exec tblJob_ProgressForRepro null, null, null, null, null, null, null, null, 0, 0, 0, '0', 'D'
end		


GO
alter proc tblDelivery_SelectSummaryForPrint
@JobID int
as
begin
	select c.Name as CustomerName, c.Address as AddressLine1, c.PostCode + ', ' + c.City as AddressLine2, country.Name as AddressLine3,
			CASE WHEN LEN(d.CustomerPO1) <> 1 and LEN(d.CustomerPO2) <> 0 THEN d.CustomerPO1 + ', ' + d.CustomerPO2
				WHEN LEN(d.CustomerPO1) <> 1 and LEN(d.CustomerPO2) = 0 THEN d.CustomerPO1
				WHEN LEN(d.CustomerPO1) = 0 and LEN(d.CustomerPO2) <> 0 THEN d.CustomerPO2
				ELSE '' END as YourReference, 
			d.DONumber + ' / ' + CONVERT(nvarchar(10), d.OrderDate, 104) as DONumber,
			j.JobNumber + ' / ' + CONVERT(nvarchar(10), j.CreatedOn, 104) as JobNumber,
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
--exec tblDelivery_SelectSummaryForPrint 159
end


GO
/****** Object:  StoredProcedure [dbo].[tblCurrencyChangegLod_SelectAll]    Script Date: 04/06/2015 09:12:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblCurrencyChangegLod_SelectAll]
@CurrencyID int,
@SearchDate DateTime,
@PageIndex int,
@PageSize int,
@SortColumn varchar(1),
@SortType varchar(1)
as
begin
	WITH T AS(
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN c.DateChanged END ASC,
															CASE @SortColumn WHEN N'1' THEN c.[NewName] END ASC,
															CASE @SortColumn WHEN N'2' THEN c.NewValue END ASC,
															CASE @SortColumn WHEN N'3' THEN c.NewRMValue END ASC,
															CASE @SortColumn WHEN N'4' THEN u.DisplayName END ASC,
															CASE @SortColumn WHEN N'5' THEN c.[Status] END ASC
															) as RowIndex, 
				CurrencyID, [NewName], NewValue, NewRMValue, CONVERT(nvarchar(10), c.DateChanged, 103) as DateChanged,
				c.[Status], u.DisplayName as DisplayName
			from TblCurrencyChangedLog c inner join tblUser u on c.ModifiedBy = u.UserName
			where c.CurrencyID = @CurrencyID
				AND (DATEDIFF(D, c.DateChanged, @SearchDate) =0 or @SearchDate IS NULL)
				AND @SortType = 'A'
		UNION all 
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN c.DateChanged END DESC,
															CASE @SortColumn WHEN N'1' THEN c.[NewName] END DESC,
															CASE @SortColumn WHEN N'2' THEN c.NewValue END DESC,
															CASE @SortColumn WHEN N'3' THEN c.NewRMValue END DESC,
															CASE @SortColumn WHEN N'4' THEN u.DisplayName END DESC,
															CASE @SortColumn WHEN N'5' THEN c.[Status] END DESC
															) as RowIndex, 
				CurrencyID, [NewName], NewValue, NewRMValue, CONVERT(nvarchar(10), c.DateChanged, 103) as DateChanged,
				c.[Status], u.DisplayName as DisplayName
			from TblCurrencyChangedLog c inner join tblUser u on c.ModifiedBy = u.UserName
			where c.CurrencyID = @CurrencyID
				AND (DATEDIFF(D, c.DateChanged, @SearchDate) =0 or @SearchDate IS NULL)
				AND @SortType = 'D'
	)
	select top (@PageSize) *, (select COUNT(RowIndex) from T) as RowsCount
		from T
		where RowIndex > (@PageIndex*@PageSize)
end
--exec [tblCurrencyChangegLod_SelectAll] 2, NULL, 0, 20, '0', 'A'

GO
/****** Object:  StoredProcedure [dbo].[tblInvoice_SelectJobSummary]    Script Date: 04/06/2015 10:37:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblInvoice_SelectJobSummary]
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
				ROUND(o.TotalPrice * (1 - (o.Discount/100)), 2) * (1 + (o.TaxPercentage/100)) as Total
		from tblJob j INNER JOIN
			tblOrderConfirmation o ON j.JobID = o.JobID INNER JOIN
			tblDeliveryOrder d ON j.JobID = d.JobID
		where j.JobID = @JobID
--exec tblInvoice_SelectJobSummary 173	
end

GO
/****** Object:  StoredProcedure [dbo].[tblInvocie_SelectSummary]    Script Date: 04/06/2015 11:45:33 ******/
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
					tblInvoiceDetail jid ON jid.JobID = j.JobID
				where jid.InvoiceID = @InvoiceID) as sp ON i.InvoiceID = sp.InvoiceID LEFT OUTER JOIN
			T ON i.InvoiceID = T.InvoiceID
		where i.InvoiceID = @InvoiceID
--exec tblInvocie_SelectSummary 13
end