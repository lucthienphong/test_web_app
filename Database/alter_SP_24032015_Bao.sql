USE [SweetSoft_APEM]
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
end
--exec tblCylinder_SelectForOrderConfirmation 77
GO
ALTER proc [dbo].[tblCylinder_SelectForDeliveryOrder]
@JobID int
as
begin
	;WITH T AS(
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
	select *, (select sum(TotalPrice) from T) as Total 
		from T
		order by T.Sequence
end		
--exec tblCylinder_SelectForDeliveryOrder 77

