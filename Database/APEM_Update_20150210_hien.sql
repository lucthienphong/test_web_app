USE [SweetSoft_APEM]
GO
/****** Object:  StoredProcedure [dbo].[tblCylinder_SelectForOrderConfirmation]    Script Date: 02/10/2015 14:30:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblCylinder_SelectForOrderConfirmation]
@JobID int
as
begin
with T as (
	select c.CylinderID, c.Sequence, c.SteelBase, c.CylinderNo, c.Color, c.PricingID, p.PricingName, case c.SteelBase when 1 then 'New' else 'Old' end as SteelBaseName,c.Circumference,c.FaceWidth,c.UnitPrice,isnull(c.Quantity,1) as Quantity,c.CylinderStatusID, cs.CylinderStatusName,
	c.Dirameter, c.Dept, c.IsPivotCylinder,p.ForTobaccoCustomers,t.TaxCode,c.TaxPercentage,t.TaxID,
	
	case p.ForTobaccoCustomers when 0 then (c.FaceWidth*c.Circumference/100)*ISNULL(c.UnitPrice,0) 
	else ISNULL(c.UnitPrice,0) end as TotalPrice,
	
	case p.ForTobaccoCustomers when 0 then (
			(c.FaceWidth*c.Circumference/100)*ISNULL(c.UnitPrice,0) + (c.FaceWidth*c.Circumference/100)*ISNULL(c.UnitPrice,0)*ISNULL(c.TaxPercentage/100,0)) 
	else (ISNULL(c.UnitPrice,0) + ISNULL(c.UnitPrice,0)*ISNULL(c.TaxPercentage/100,0)) end as PriceTaxed
		
		from tblCylinder c INNER JOIN
			tblCylinderStatus cs ON c.CylinderStatusID = cs.CylinderStatusID INNER JOIN
			tblPricing p ON c.PricingID = p.PricingID
			left join tblJobQuotationPricing jp on c.JobID = jp.JobID
			left outer join tblTax t on c.TaxID = t.TaxID
		where c.JobID = @JobID
		and c.PricingID = jp.PricingID
		)
		select *,(select sum(TotalPrice) from T) as Total from T
		order by T.Sequence
end