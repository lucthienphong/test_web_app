GO
IF COL_LENGTH('tblEngraving','MotherSet') IS NOT NULL
BEGIN
	ALTER Table tblEngraving
		DROP COLUMN MotherSet
END

GO
IF COL_LENGTH('tblEngraving','EngravingWidth') IS NOT NULL
BEGIN
	ALTER Table tblEngraving
		DROP COLUMN EngravingWidth
END

GO
IF COL_LENGTH('tblEngraving','UnitSizeV') IS NOT NULL
BEGIN
	ALTER Table tblEngraving
		DROP COLUMN UnitSizeV
END

GO
IF COL_LENGTH('tblEngraving','UnitSizeH') IS NOT NULL
BEGIN
	ALTER Table tblEngraving
		DROP COLUMN UnitSizeH
END

GO
IF COL_LENGTH('tblEngraving','SRRemark') IS NOT NULL
BEGIN
	ALTER Table tblEngraving
		DROP COLUMN SRRemark
END

GO
IF COL_LENGTH('tblEngraving','FileSizeHEMG') IS NULL
BEGIN
	ALTER Table tblEngraving
		ADD FileSizeHEMG float NULL
END

GO
IF COL_LENGTH('tblEngraving','FileSizeVEMG') IS NULL
BEGIN
	ALTER Table tblEngraving
		ADD FileSizeVEMG float NULL
END

GO
IF COL_LENGTH('tblEngraving','FileSizeHDLS') IS NULL
BEGIN
	ALTER Table tblEngraving
		ADD FileSizeHDLS float NULL
END

GO
IF COL_LENGTH('tblEngraving','FileSizeVDLS') IS NULL
BEGIN
	ALTER Table tblEngraving
		ADD FileSizeVDLS float NULL
END

GO
IF COL_LENGTH('tblEngraving','FileSizeHEtching') IS NULL
BEGIN
	ALTER Table tblEngraving
		ADD FileSizeHEtching float NULL
END

GO
IF COL_LENGTH('tblEngraving','FileSizeVEtching') IS NULL
BEGIN
	ALTER Table tblEngraving
		ADD FileSizeVEtching float NULL
END

GO
IF COL_LENGTH('tblEngraving','SRRemarkEMG') IS NULL
BEGIN
	ALTER Table tblEngraving
		ADD SRRemarkEMG nvarchar(1000) NULL
END

GO
IF COL_LENGTH('tblEngraving','SRRemarkDLS') IS NULL
BEGIN
	ALTER Table tblEngraving
		ADD SRRemarkDLS nvarchar(1000) NULL
END

GO
IF COL_LENGTH('tblEngraving','SRRemarkEtching') IS NULL
BEGIN
	ALTER Table tblEngraving
		ADD SRRemarkEtching nvarchar(1000) NULL
END

GO
IF COL_LENGTH('tblEngraving','TobaccoType') IS NULL
BEGIN
	ALTER Table tblEngraving
		ADD TobaccoType nvarchar(50) NULL
END

---------------------------------------------------------------------------------------------------------------------------
GO
/****** Object:  StoredProcedure [dbo].[tblCylinder_SelectForOrderConfirmation]    Script Date: 03/27/2015 23:40:35 ******/
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
		c.CylinderNo,  c.Color + ' '  + cs.CylinderStatusName as CylDescription,
		c.CylinderBarcode as CylBarcode, c.CusCylinderID as CusCylID, c.CusSteelBaseID,
		 qd.PricingName, 
		c.Circumference, c.FaceWidth, c.UnitPrice, isnull(c.Quantity,1) as Quantity, c.Color,
		case qd.UnitOfMeasure when 'cm2' then (c.FaceWidth*c.Circumference/100)*ISNULL(c.UnitPrice,0) 
									else ISNULL(c.UnitPrice,0) end as TotalPrice
	from tblCylinder c INNER JOIN
			tblCylinderStatus cs ON c.CylinderStatusID = cs.CylinderStatusID LEFT OUTER JOIN
			tblCustomerQuotationDetail qd ON c.PricingID = qd.ID
	where c.JobID = @JobID	
  )
  select *, (select sum(TotalPrice) from T) as Total from T
	order by T.Sequence
--exec tblCylinder_SelectForOrderConfirmation 159
end

GO
/****** Object:  StoredProcedure [dbo].[tblCylinder_SelectForDeliveryOrder]    Script Date: 03/28/2015 00:01:03 ******/
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
Update tblFunction set DisplayOrder = DisplayOrder + 1 where DisplayOrder > 40
GO
Insert into tblFunction values ('job_category_manager', 'fSystemConfig', 'Job category manager', '', 41)