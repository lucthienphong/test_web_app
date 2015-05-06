USE [SweetSoft_APEM]
GO
ALTER TABLE tblOtherCharges
DROP CONSTRAINT FK_tblOtherCharges_tblOrderConfirmation


GO
/****** Object:  StoredProcedure [dbo].[tblCylinder_SelectForDeliveryOrder]    Script Date: 03/30/2015 22:47:05 ******/
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
			--and cs.Physical = 1
	)
	select *, (select sum(TotalPrice) from T) as Total 
		from T
		order by T.Sequence
--exec tblCylinder_SelectForDeliveryOrder 159
end		