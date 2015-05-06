USE [SweetSoft_APEM]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblPurchaseOrder_Cylinder_tblCylinder]') AND parent_object_id = OBJECT_ID(N'[dbo].[tblPurchaseOrder_Cylinder]'))
ALTER TABLE [dbo].[tblPurchaseOrder_Cylinder] DROP CONSTRAINT [FK_tblPurchaseOrder_Cylinder_tblCylinder]

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblPurchaseOrder_Cylinder_tblPurchaseOrder]') AND parent_object_id = OBJECT_ID(N'[dbo].[tblPurchaseOrder_Cylinder]'))
ALTER TABLE [dbo].[tblPurchaseOrder_Cylinder] DROP CONSTRAINT [FK_tblPurchaseOrder_Cylinder_tblPurchaseOrder]

GO
/****** Object:  Index [PK_tblPurchaseOrder_Cylinder]    Script Date: 03/25/2015 22:33:56 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tblPurchaseOrder_Cylinder]') AND name = N'PK_tblPurchaseOrder_Cylinder')
ALTER TABLE [dbo].[tblPurchaseOrder_Cylinder] DROP CONSTRAINT [PK_tblPurchaseOrder_Cylinder]

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblJobQuotation_tblJob]') AND parent_object_id = OBJECT_ID(N'[dbo].[tblJobQuotation]'))
ALTER TABLE [dbo].[tblJobQuotation] DROP CONSTRAINT [FK_tblJobQuotation_tblJob]


GO
/****** Object:  Table [dbo].[tblJobQuotation]    Script Date: 03/26/2015 00:54:12 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblJobQuotation]') AND type in (N'U'))
DROP TABLE [dbo].[tblJobQuotation]

GO
/****** Object:  Table [dbo].[tblJobQuotation]    Script Date: 03/26/2015 00:54:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblJobQuotation](
	[QuotationID] [int] IDENTITY(1,1) NOT NULL,
	[JobID] [int] NOT NULL,
	[RevNumber] [int] NOT NULL,
	[QuotationNo] [nvarchar](50) NULL,
	[QuotationDate] [datetime] NULL,
	[QuotationText] [nvarchar](1000) NULL,
	[CreatedBy] [nvarchar](100) NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedOn] [datetime] NULL,
 CONSTRAINT [PK_tblJobQuotation] PRIMARY KEY CLUSTERED 
(
	[QuotationID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


GO
/****** Object:  Table [dbo].[tblJobQuotationPricing]    Script Date: 03/26/2015 00:55:37 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblJobQuotationPricing]') AND type in (N'U'))
DROP TABLE [dbo].[tblJobQuotationPricing]

GO
/****** Object:  Table [dbo].[tblJobQuotationPricing]    Script Date: 03/26/2015 00:55:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblJobQuotationPricing](
	[QuotationID] [int] NOT NULL,
	[PricingID] [int] NOT NULL,
	[OldSteelBasePrice] [decimal](18, 3) NOT NULL,
	[NewSteelBasePrice] [decimal](18, 3) NOT NULL,
 CONSTRAINT [PK_tbltblJobQuotationPricing] PRIMARY KEY CLUSTERED 
(
	[QuotationID] ASC,
	[PricingID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
IF COL_LENGTH('tblPurchaseOrder_Cylinder','ID') IS NULL
BEGIN
	ALTER Table tblPurchaseOrder_Cylinder
		ADD ID int Identity(1, 1)
END

GO
IF COL_LENGTH('tblPurchaseOrder_Cylinder','CylinderID') IS  NOT NULL
BEGIN
	ALTER Table tblPurchaseOrder_Cylinder
		ALTER Column CylinderID int NULL
END

GO
IF COL_LENGTH('tblPurchaseOrder_Cylinder','ID') IS NOT NULL
BEGIN
	ALTER Table tblPurchaseOrder_Cylinder
		ADD CONSTRAINT pk_tblPurchaseOrder_Cylinder PRIMARY KEY (ID)
END

GO
IF COL_LENGTH('tblPurchaseOrder_Cylinder','CylinderNo') IS NULL
BEGIN
	ALTER Table tblPurchaseOrder_Cylinder
	ADD CylinderNo nvarchar(50) NULL
END

GO
IF COL_LENGTH('tblPurchaseOrder_Cylinder','Circumference') IS NULL
BEGIN
	ALTER Table tblPurchaseOrder_Cylinder
	ADD Circumference float NULL
END

GO
IF COL_LENGTH('tblPurchaseOrder_Cylinder','FaceWidth') IS NULL
BEGIN
	ALTER Table tblPurchaseOrder_Cylinder
	ADD FaceWidth float NULL
END

GO
IF COL_LENGTH('tblPurchaseOrder_Cylinder', 'JobID') IS NULL
BEGIN
	ALTER Table tblPurchaseOrder_Cylinder
	ADD JobID int NULL
END

GO
IF COL_LENGTH('tblPurchaseOrder_Cylinder', 'Unit') IS NULL
BEGIN
	ALTER Table tblPurchaseOrder_Cylinder
	ADD Unit nvarchar(50) NULL
END

GO
IF COL_LENGTH('tblPurchaseOrder_Cylinder', 'Status') IS NULL
BEGIN
	ALTER Table tblPurchaseOrder_Cylinder
	ADD Status nvarchar(50) NULL
END

GO
IF COL_LENGTH('tblPurchaseOrder_Cylinder', 'DONumber') IS NULL
BEGIN
	ALTER Table tblPurchaseOrder_Cylinder
	ADD DONumber nvarchar(50) NULL
END

GO
IF COL_LENGTH('tblPurchaseOrder_Cylinder', 'DeliveryDate') IS NULL
BEGIN
	ALTER Table tblPurchaseOrder_Cylinder
	ADD DeliveryDate datetime NULL
END

GO
IF COL_LENGTH('tblJob', 'ProductTypeID') IS NULL
BEGIN
	ALTER Table tblJob
	ADD ProductTypeID int NULL
END

GO
IF COL_LENGTH('tblServiceJobDetail', 'PricingID') IS NULL
BEGIN
	ALTER Table tblServiceJobDetail
	ADD PricingID int NULL
END

GO
IF COL_LENGTH('tblCylinder', 'CusSteelBaseID') IS NULL
BEGIN
	ALTER Table tblCylinder
	ADD CusSteelBaseID nvarchar(50) NULL
END

GO
IF COL_LENGTH('tblCylinder', 'CylinderNo') IS NULL
BEGIN
	ALTER Table tblCylinder
	ALTER column CylinderNo nvarchar(50) NULL
END

GO
IF COL_LENGTH('tblOtherCharges', 'PricingID') IS NULL
BEGIN
	ALTER Table tblOtherCharges
	ADD PricingID int NULL
END

GO
IF COL_LENGTH('tblCustomerQuotation_AdditionalService', 'Category') IS NULL
BEGIN
	ALTER Table tblCustomerQuotation_AdditionalService
	ADD Category nvarchar(50) NULL
END

GO
/****** Object:  StoredProcedure [dbo].[tblCylinder_SelectAll]    Script Date: 03/23/2015 13:36:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblCylinder_SelectAll]
@JobID int
as
begin
	select c.CylinderID, c.Sequence, c.CylinderNo, c.CylinderBarcode, c.CusCylinderID, c.SteelBase, c.Color, c.Protocol, c.PricingID, cq.PricingName, 
			c.ProductTypeID, c.ProcessTypeID, prd.Code + '-' + prc.Code as CylType, c.Quantity, c.Dirameter, c.Dept, c.IsPivotCylinder,
			c.UnitPrice, st.CylinderStatusName, c.CylinderStatusID, c.CusSteelBaseID
		from tblCylinder c INNER JOIN
				tblCustomerQuotationDetail cq ON c.PricingID = cq.ID INNER JOIN
				tblReferences prd ON c.ProductTypeID = prd.ReferencesID INNER JOIN
				tblReferences prc ON c.ProcessTypeID = prc.ReferencesID LEFT OUTER JOIN
				tblCylinderStatus st ON c.CylinderStatusID = st.CylinderStatusID
		where c.JobID = @JobID
		order by c.Sequence
--exec tblCylinder_SelectAll 77
end

GO
/****** Object:  StoredProcedure [dbo].[tblJob_SelectAll]    Script Date: 03/23/2015 15:42:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblJob_SelectAll]
@Customer nvarchar(200),
@JobBarcode nvarchar(255),
@JobNumber nvarchar(50),
@JobInfo nvarchar(200),
@CusCylID nvarchar(50),
@SaleRepID int,
@FromDate datetime,
@ToDate datetime,
@IsServiceJob bit,
@PageIndex int,
@PageSize int,
@SortColumn varchar(2),
@SortType varchar(1)
AS
BEGIN
	;WITH T AS(
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN  c.Code END ASC,
															--CASE @SortColumn WHEN N'1' THEN c.Name END ASC,
															--CASE @SortColumn WHEN N'1' THEN j.JobBarcode END ASC,
															CASE @SortColumn WHEN N'1' THEN j.JobNumber END ASC,
															CASE @SortColumn WHEN N'2' THEN j.JobName END ASC,
															CASE @SortColumn WHEN N'3' THEN j.Design END ASC,
															CASE @SortColumn WHEN N'4' THEN j.CreatedOn END ASC) as RowIndex, 
				j.JobID, c.Code, c.Name, j.JobBarcode, j.JobNumber, j.JobName, j.Design, j.RevNumber, 
				CONVERT(nvarchar(10), j.CreatedOn, 103) as CreatedOn, 
				s.FirstName + ' ' + s.LastName as SaleDep
			from tblJob j INNER JOIN
				tblCustomer c ON j.CustomerID = c.CustomerID INNER JOIN
				tblStaff s ON j.SalesRepID = s.StaffID LEFT OUTER JOIN
				tblCylinder cyl ON j.JobID = cyl.JobID
			where j.IsClosed = 0
				and (c.Name like '%' + @Customer + '%' or c.Code like '%' + @Customer + '%')
				and j.JobBarcode like '%' + @JobBarcode + '%'
				and j.JobNumber like '%' + @JobNumber + '%'
				and (j.JobName like '%' + @JobInfo + '%' or j.Design like '%' + @JobInfo + '%')
				and (j.SalesRepID = @SaleRepID or @SaleRepID = 0)
				and (DATEDIFF(D, j.CreatedOn, @FromDate) <=0 or @FromDate IS NULL)
				and (DATEDIFF(D, j.CreatedOn, @ToDate) >=0 or @ToDate IS NULL)
				and j.IsServiceJob =@IsServiceJob
				and ISNULL(cyl.CusCylinderID, '') like '%' + @CusCylID + '%'
				and @SortType = 'A'
		group by j.JobID, c.Code, c.Name, j.JobBarcode, j.JobNumber, j.JobName, j.Design, j.RevNumber, 
				j.CreatedOn, s.FirstName, s.LastName
		union all
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN  c.Code END DESC,
															--CASE @SortColumn WHEN N'1' THEN c.Name END DESC,
															--CASE @SortColumn WHEN N'2' THEN j.JobBarcode END DESC,
															CASE @SortColumn WHEN N'1' THEN j.JobNumber END DESC,
															CASE @SortColumn WHEN N'2' THEN j.JobName END DESC,
															CASE @SortColumn WHEN N'3' THEN j.Design END DESC,
															CASE @SortColumn WHEN N'4' THEN j.CreatedOn END DESC) as RowIndex, 
				j.JobID, c.Code, c.Name, j.JobBarcode, j.JobNumber, j.JobName, j.Design, j.RevNumber, 
				CONVERT(nvarchar(10), j.CreatedOn, 103) as CreatedOn,
				s.FirstName + ' ' + s.LastName as SaleDep
			from tblJob j INNER JOIN
				tblCustomer c ON j.CustomerID = c.CustomerID INNER JOIN
				tblStaff s ON j.SalesRepID = s.StaffID LEFT OUTER JOIN
				tblCylinder cyl ON j.JobID = cyl.JobID
			where j.IsClosed = 0
				and (c.Name like '%' + @Customer + '%' or c.Code like '%' + @Customer + '%')
				and j.JobBarcode like '%' + @JobBarcode + '%'
				and j.JobNumber like '%' + @JobNumber + '%'
				and (j.JobName like '%' + @JobInfo + '%' or j.Design like '%' + @JobInfo + '%')
				and (j.SalesRepID = @SaleRepID or @SaleRepID = 0)
				and (DATEDIFF(D, j.CreatedOn, @FromDate) <=0 or @FromDate IS NULL)
				and (DATEDIFF(D, j.CreatedOn, @ToDate) >=0 or @ToDate IS NULL)
				and j.IsServiceJob =@IsServiceJob
				and ISNULL(cyl.CusCylinderID, '') like '%' + @CusCylID + '%'
				and @SortType = 'D'
		group by j.JobID, c.Code, c.Name, j.JobBarcode, j.JobNumber, j.JobName, j.Design, j.RevNumber, 
				j.CreatedOn, s.FirstName, s.LastName
		)
		select top (@PageSize) *, (select COUNT(RowIndex) from T) as RowsCount
			from T
			where RowIndex > (@PageIndex*@PageSize)
	--exec tblJob_SelectAll '', '', '', '', 'a', 0, null, null, 0, 0, 10, '4', 'D'
END

GO
/****** Object:  StoredProcedure [dbo].[tblCylinder_SelectAll]    Script Date: 03/25/2015 08:52:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblCylinder_SelectAll]
@JobID int
as
begin
	select c.CylinderID, c.Sequence, c.CylinderNo, c.CylinderBarcode, c.CusCylinderID, c.SteelBase, c.Color, c.Protocol, c.PricingID, cq.PricingName, 
			c.ProductTypeID, c.ProcessTypeID, prd.Code + '-' + prc.Code as CylType, c.Quantity, c.Dirameter, c.Dept, c.IsPivotCylinder,
			c.UnitPrice, st.CylinderStatusName, c.CylinderStatusID, c.CusSteelBaseID
		from tblCylinder c LEFT OUTER JOIN
				tblCustomerQuotationDetail cq ON c.PricingID = cq.ID LEFT OUTER JOIN
				tblReferences prd ON c.ProductTypeID = prd.ReferencesID LEFT OUTER JOIN
				tblReferences prc ON c.ProcessTypeID = prc.ReferencesID LEFT OUTER JOIN
				tblCylinderStatus st ON c.CylinderStatusID = st.CylinderStatusID
		where c.JobID = @JobID
		order by c.Sequence
--exec tblCylinder_SelectAll 77
end

GO
/****** Object:  StoredProcedure [dbo].[tblCylinder_SelectForOrderConfirmation]    Script Date: 03/24/2015 16:05:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblCylinder_SelectForOrderConfirmation]
@JobID int
as
begin
with T as (
 select c.CylinderID, c.Sequence, c.SteelBase, case c.SteelBase when 1 then 'New' else 'Old' end as SteelBaseName,
		c.CylinderNo,  c.Color + ' '  + r.Name as CylDescription,
		c.CylinderBarcode as CylBarcode, c.CusCylinderID as CusCylID,
		 qd.PricingName, 
		c.Circumference, c.FaceWidth, c.UnitPrice, isnull(c.Quantity,1) as Quantity, c.Color,
		case qd.UnitOfMeasure when 'cm2' then (c.FaceWidth*c.Circumference/100)*ISNULL(c.UnitPrice,0) 
									else ISNULL(c.UnitPrice,0) end as TotalPrice
	from tblCylinder c INNER JOIN
			tblCustomerQuotationDetail qd ON c.PricingID = qd.ID INNER JOIN
			tblReferences r ON qd.ProductTypeID = r.ReferencesID
	where c.JobID = @JobID
  )
  select *, (select sum(TotalPrice) from T) as Total from T
	order by T.Sequence
--exec tblCylinder_SelectForOrderConfirmation 77
end

GO
ALTER proc [dbo].[tblCylinder_SelectForDeliveryOrder]
@JobID int
as
begin
	;WITH T AS(
		select c.CylinderID, c.Sequence, c.SteelBase, case c.SteelBase when 1 then 'New' else 'Old' end as SteelBaseName,
			c.CylinderNo,  c.Color + ' '  + cs.CylinderStatusName as CylDescription,
			c.CylinderBarcode as CylBarcode, c.CusCylinderID as CusCylID,
			 qd.PricingName, 
			c.Circumference, c.FaceWidth, c.UnitPrice, isnull(c.Quantity,1) as Quantity, c.Color,
			case qd.UnitOfMeasure when 'cm2' then (c.FaceWidth*c.Circumference/100)*ISNULL(c.UnitPrice,0) 
										else ISNULL(c.UnitPrice,0) end as TotalPrice
		from tblCylinder c INNER JOIN
				tblCylinderStatus cs ON c.CylinderStatusID = cs.CylinderStatusID LEFT OUTER JOIN
				tblCustomerQuotationDetail qd ON c.PricingID = qd.ID LEFT OUTER JOIN
				tblReferences r ON qd.ProductTypeID = r.ReferencesID
		where c.JobID = @JobID
			and cs.Physical = 1
	)
	select *, (select sum(TotalPrice) from T) as Total 
		from T
		order by T.Sequence
--exec tblCylinder_SelectForDeliveryOrder 77
end		

GO
/****** Object:  StoredProcedure [dbo].[tblEngravingDetail_SelectAll]    Script Date: 03/25/2015 11:02:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblEngravingDetail_SelectAll]
@JobID int
as
begin
	select ISNULL(ed.EngravingID, -1) as EngravingID, ISNULL(ed.Sequence, c.Sequence) Sequence, c.CylinderNo, 
			c.Color, ed.Stylus, ed.Screen, ed.Angle, ed.Wall, ed.Gamma, ed.Sh, ed.Hl, ed.Ch, ed.Mt, 
			ISNULL(ed.IsCopy, 0) as IsCopy, ed.CellDepth, ed.CopperSh, ed.CopperCh, ed.ChromeSh, ed.ChromeCh,
			c.CylinderID, r.Name as CylinderStatusName 
	from tblCylinder c LEFT OUTER JOIN
			tblReferences r ON c.ProcessTypeID = r.ReferencesID LEFT OUTER JOIN
			tblEngravingDetail ed ON c.CylinderID = ed.CylinderID
	where c.JobID = @JobID and c.Protocol = 'EMG'
	order by ISNULL(ed.Sequence, c.Sequence),ed.IsCopy
	--exec tblEngravingDetail_SelectAll 100
end

GO
/****** Object:  StoredProcedure [dbo].[tblEngravingTobacco_SelectAll]    Script Date: 03/25/2015 11:03:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblEngravingTobacco_SelectAll]
@JobID int
as
begin
	select ISNULL(ed.EngravingID, -1) as EngravingID, ISNULL(ed.Sequence, c.Sequence) Sequence, c.CylinderNo, 
			c.CylinderID, r.Name as CylinderStatusName, c.Color, ISNULL(ed.IsCopy,0) as IsCopy,
			ed.Screen, ISNULL(ed.MasterScreen, 0) as MasterScreen, ed.Angle, ed.Elongation, ed.Distotion, 
			ed.Resolution, ed.Hexagonal, hexa.Name as HexaName, ISNULL(ed.ImageSmoothness, 0) as ImageSmoothness, 
			ed.UnsharpMasking, ed.Antialiasing, ed.LineworkWidening,ed.EngravingStart, ed.EngravingWidth, 
			ed.CellShape, CellShape.Name as CellShapeName, ed.Gradation, gra.Name as GraName, ed.Gamma, 
			ISNULL(ed.LaserA, 0) as LaserA, ISNULL(ed.LaserB, 0) as LaserB, 
			ed.CellWidth, ed.ChannelWidth, ed.CellDepth, ed.EngravingTime, ed.Beam, ed.Threshold, ed.CheckedBy, ed.CheckedOn
	from tblCylinder c LEFT OUTER JOIN
			tblReferences r ON c.ProcessTypeID = r.ReferencesID LEFT OUTER JOIN
			tblEngravingTobacco ed ON c.CylinderID = ed.CylinderID LEFT OUTER JOIN
			tblReferences hexa ON ed.Hexagonal = hexa.ReferencesID LEFT OUTER JOIN
			tblReferences cellshape ON ed.CellShape = cellshape.ReferencesID LEFT OUTER JOIN
			tblReferences gra ON ed.Gradation = gra.ReferencesID
	where c.JobID = @JobID and c.Protocol = 'DLS'
	order by ISNULL(ed.Sequence, c.Sequence),ed.IsCopy
	--exec tblEngravingTobacco_SelectAll 100
end

GO
/****** Object:  StoredProcedure [dbo].[tblInvoice_SelectDetailForExport]    Script Date: 03/25/2015 21:46:37 ******/
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
				ISNULL(t.TaxCode, '') AS TaxCode, ct.Name + ': ' + i.InvoiceNo as [Description], ct.InternalOrderNo as JobNumber
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
				ISNULL(t.TaxCode, '') AS TaxCode, ct.Name + ': ' + i.InvoiceNo as [Description], ct.InternalOrderNo as JobNumber
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
				ISNULL(t.TaxCode, '') AS TaxCode, ct.Name + ': ' + i.InvoiceNo as [Description], ct.InternalOrderNo as JobNumber
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
	--exec tblInvoice_SelectDetailForExport '-7-'
END

GO
/****** Object:  StoredProcedure [dbo].[tblInvoice_SelectForExport]    Script Date: 03/25/2015 21:40:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblInvoice_SelectForExport]
@InvoiceID nvarchar(MAX)
AS
BEGIN
	SELECT i.InvoiceNo, c.SAPCode, CONVERT(nvarchar(8), i.InvoiceDate, 112) as InvoiceDate, curr.CurrencyName, 
			CASE WHEN t.TaxID IS NOT NULL THEN 'X' ELSE '' END AS CalcTax, t.TaxCode, i.NetTotal as TotalPrice, curr.RMValue
		FROM tblInvoice i INNER JOIN
			tblCustomer c ON i.CustomerID = c.CustomerID INNER JOIN
			tblCurrency curr ON i.CurrencyID = curr.CurrencyID LEFT OUTER JOIN
			tblTax t ON i.TaxID = t.TaxID
		WHERE @InvoiceID like '%-' + CAST(i.InvoiceID as nvarchar(5)) + '-%'
	--exec tblInvoice_SelectForExport '-7-'
END