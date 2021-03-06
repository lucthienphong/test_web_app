GO
IF COL_LENGTH('tblCurrency','RMValue') IS NULL
BEGIN
	ALTER Table tblCurrency
		ALTER column RMValue decimal(18,4) NOT NULL
END

GO
IF COL_LENGTH('tblCurrency','CurrencyValue') IS NULL
BEGIN
	ALTER Table tblCurrency
		ALTER column CurrencyValue decimal(18,4) NOT NULL
END

GO
IF COL_LENGTH('tblEngraving','EngravingWidth') IS NULL
BEGIN
ALTER table tblEngraving
	ADD EngravingWidth float NULL
END

GO
/****** Object:  StoredProcedure [dbo].[tblCylinder_SelectForDeliveryOrder]    Script Date: 03/31/2015 10:29:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblCylinder_SelectForDeliveryOrder]
@JobID int
as
begin
	;WITH T AS(
		select c.CylinderID, c.Sequence, c.SteelBase, case c.SteelBase when 1 then 'New' else 'Old' end as SteelBaseName,
			c.CylinderNo,  c.Color + ' '  + cs.CylinderStatusName as CylDescription,
			c.CylinderBarcode as CylBarcode, c.CusCylinderID as CusCylID, c.CusSteelBaseID,
			qd.PricingName, c.Circumference, c.FaceWidth, c.UnitPrice, c.Color,
			CASE cs.Physical WHEN 1 THEN isnull(c.Quantity,1) ELSE 0 END as Quantity, 
			case qd.UnitOfMeasure when 'cm2' then (c.FaceWidth*c.Circumference/100)*ISNULL(c.UnitPrice,0) 
										else ISNULL(c.UnitPrice,0) end as TotalPrice
		from tblCylinder c INNER JOIN
				tblCylinderStatus cs ON c.CylinderStatusID = cs.CylinderStatusID LEFT OUTER JOIN
				tblCustomerQuotationDetail qd ON c.PricingID = qd.ID LEFT OUTER JOIN
				tblReferences r ON qd.ProductTypeID = r.ReferencesID
		where c.JobID = @JobID
			--and cs.Physical = 1
	)
	select *, (select sum(TotalPrice) from T) as Total 
		from T
		order by T.Sequence
--exec tblCylinder_SelectForDeliveryOrder 159
end		

GO
/****** Object:  StoredProcedure [dbo].[JobPrintingDetail]    Script Date: 03/31/2015 17:11:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- up date storeprocedure

ALTER proc [dbo].[JobPrintingDetail]
@JobID int
as
begin
		select 
			c.Name,j.JobNumber, j.RevNumber,j.JobName,j.Design,jR.JobNumber as RootJobNumber,j.RootJobRevNumber, j.CommonJobNumber,
			(stSale.FirstName +' '+ stSale.LastName) as SalePerson, (st2.FirstName) as CreatedBy,
			CONVERT(nvarchar(10),j.CreatedOn,103) as CreatedDate,
			CONVERT(nvarchar(10),js.ReproDate,103) as ReproCreateDate, js.IrisProof,
			(st.FirstName + st.LastName) as CoopName, CONVERT(nvarchar(10),js.CylinderDate,103) as CylinderCreateDate,js.DeilveryNotes,
			 js.LeavingAPE, js.EMWidth,js.EMPonsition,js.PrintingDirection,
			 js.EMHeight,js.EMColor,b.BackingName,js.EMPonsition,
			js.BarcodeSize,js.BarcodeColor,js.BarcodeNo,js.BWR, s.SupplyName,
			Cast(js.UNSizeV as decimal(18,2)) as UNSizeV,Cast(js.UNSizeH as decimal(18,2)) as UNSizeH, js.PrintingDirection,
			js.Size,js.OpaqueInkRate,js.ColorTarget,js.ProofingMaterial,
			js.NumberOfRepeatV, js.NumberOfRepeatH,
			Cast(js.FaceWidth as decimal(18,2)) as FaceWidth,
			Cast(js.Circumference as decimal(18,2)) as Circumference,js.TypeOfCylinder,js.Printing,j.Remark,
			 ct.ContactName, s.SupplyName
			from tblJob j left outer join tblJobSheet js on j.JobID = js.JobID 
				inner join tblUser u on j.CreatedBy  = u.UserName
				inner join tblStaff st2 on u.UserID = st2.StaffID
				left outer join tblBacking b on js.BackingID = b.BackingID 
				-- inner join tblCylinder cl on cl.JobID = j.JobID 
				inner join tblCustomer c on j.CustomerID = c.CustomerID 
				inner join tblContact ct on j.ContactPersonID = ct.ContactID
				left outer join tblSupply s on s.SupplyID = js.SupplyID
				left outer join tblStaff st on st.StaffID = j.CoordinatorID
				left outer join tblStaff stSale on stSale.StaffID = j.SalesRepID
				left outer join (select * from tblJob) as jR on jR.JobID = j.RootJobID
			where j.JobID = @JobID
end
-- exec [JobPrintingDetail] 84

GO
/****** Object:  StoredProcedure [dbo].[JobPrintingDetail]    Script Date: 04/01/2015 15:03:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- up date storeprocedure

ALTER proc [dbo].[JobPrintingDetail]
@JobID int
as
begin
		select 
			c.Name,j.JobNumber, j.RevNumber,j.JobName,j.Design,
			ISNULL(jR.JobNumber, j.RootJobNo) as RootJobNumber, 
			j.RootJobRevNumber, j.CommonJobNumber,
			(stSale.FirstName +' '+ stSale.LastName) as SalePerson, (st2.FirstName) as CreatedBy,
			CONVERT(nvarchar(10),j.CreatedOn,103) as CreatedDate,
			CONVERT(nvarchar(10),js.ReproDate,103) as ReproCreateDate, js.IrisProof,
			(st.FirstName + st.LastName) as CoopName, CONVERT(nvarchar(10),js.CylinderDate,103) as CylinderCreateDate,js.DeilveryNotes,
			 js.LeavingAPE, js.EMWidth,js.EMPonsition,js.PrintingDirection,
			 js.EMHeight,js.EMColor,b.BackingName,js.EMPonsition,
			js.BarcodeSize,js.BarcodeColor,js.BarcodeNo,js.BWR, s.SupplyName,
			Cast(js.UNSizeV as decimal(18,2)) as UNSizeV,Cast(js.UNSizeH as decimal(18,2)) as UNSizeH, js.PrintingDirection,
			js.Size,js.OpaqueInkRate,js.ColorTarget,js.ProofingMaterial,
			js.NumberOfRepeatV, js.NumberOfRepeatH,
			Cast(js.FaceWidth as decimal(18,2)) as FaceWidth,
			Cast(js.Circumference as decimal(18,2)) as Circumference,js.TypeOfCylinder,js.Printing,j.Remark,
			 ct.ContactName, s.SupplyName
			from tblJob j left outer join tblJobSheet js on j.JobID = js.JobID 
				inner join tblUser u on j.CreatedBy  = u.UserName
				inner join tblStaff st2 on u.UserID = st2.StaffID
				left outer join tblBacking b on js.BackingID = b.BackingID 
				-- inner join tblCylinder cl on cl.JobID = j.JobID 
				inner join tblCustomer c on j.CustomerID = c.CustomerID 
				inner join tblContact ct on j.ContactPersonID = ct.ContactID
				left outer join tblSupply s on s.SupplyID = js.SupplyID
				left outer join tblStaff st on st.StaffID = j.CoordinatorID
				left outer join tblStaff stSale on stSale.StaffID = j.SalesRepID
				left outer join (select * from tblJob) as jR on jR.JobID = j.RootJobID
			where j.JobID = @JobID
-- exec [JobPrintingDetail] 84
end