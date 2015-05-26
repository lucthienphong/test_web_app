
/****** Object:  StoredProcedure [dbo].[tblCylinder_SelectForOrderConfirmation]    Script Date: 05/26/2015 09:47:46 ******/
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
		qd.PricingName, c.Circumference, c.FaceWidth, c.UnitPrice, c.Color,
		CASE cs.Physical WHEN 1 THEN isnull(c.Quantity,1) ELSE 0 END as Quantity, 
		case qd.UnitOfMeasure when 'cm2' then (c.FaceWidth*c.Circumference/100)*ISNULL(c.UnitPrice,0) 
									else ISNULL(c.UnitPrice,0) end as TotalPrice
	from tblCylinder c INNER JOIN
			tblCylinderStatus cs ON c.CylinderStatusID = cs.CylinderStatusID LEFT OUTER JOIN
			tblCustomerQuotationDetail qd ON c.PricingID = qd.ID LEFT JOIN 
			tblJob j on j.JobID = c.JobID
	where c.JobID = @JobID 
	and j.TypeOfOrder <> N'BarrelProof'
  )
  select *, (select sum(TotalPrice) from T) as Total from T
	order by T.Sequence
--exec tblCylinder_SelectForOrderConfirmation 155
end

/****** Object:  StoredProcedure [dbo].[tblCylinder_SelectForDeliveryOrder]    Script Date: 05/26/2015 09:49:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblCylinder_SelectForDeliveryOrder]
@JobID int
as
begin
	;WITH T AS(
		SELECT c.CylinderID,
			   c.Sequence,
			   c.SteelBase,
			   CASE c.SteelBase
				   WHEN 1 THEN 'New'
				   ELSE 'Old'
			   END AS SteelBaseName,
			   c.CylinderNo,
			   c.Color + ' ' + cs.CylinderStatusName AS CylDescription,
			   c.CylinderBarcode AS CylBarcode,
			   c.CusCylinderID AS CusCylID,
			   c.CusSteelBaseID,
			   qd.PricingName,
			   c.Circumference,
			   c.FaceWidth,
			   c.UnitPrice,
			   c.Color,
			   CASE cs.Physical
				   WHEN 1 THEN isnull(c.Quantity,1)
				   ELSE 0
			   END AS Quantity,
			   CASE qd.UnitOfMeasure
				   WHEN 'cm2' THEN (c.FaceWidth*c.Circumference/100)*ISNULL(c.UnitPrice,0)
				   ELSE ISNULL(c.UnitPrice,0)
			   END AS TotalPrice
		FROM tblCylinder c
		INNER JOIN tblCylinderStatus cs ON c.CylinderStatusID = cs.CylinderStatusID
		LEFT OUTER JOIN tblCustomerQuotationDetail qd ON c.PricingID = qd.ID
		LEFT OUTER JOIN tblReferences r ON qd.ProductTypeID = r.ReferencesID
		LEFT JOIN tblJob j on j.JobID = c.JobID
		WHERE c.JobID = @JobID 
		and j.TypeOfOrder <> N'BarrelProof'
			--and cs.Physical = 1
	)
	SELECT *,
	  (SELECT sum(TotalPrice)
	   FROM T) AS Total
	FROM T
	ORDER BY T.Sequence
--exec tblCylinder_SelectForDeliveryOrder 159
end		

