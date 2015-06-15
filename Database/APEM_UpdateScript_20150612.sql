
GO
/****** Object:  StoredProcedure [dbo].[tblInvoice_SelectJobSummary]    Script Date: 06/12/2015 10:15:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblInvoice_SelectJobSummary]
@JobID int
as
begin
;with T as (
	select j.JobName, j.Design, d.DONumber + ' / ' + CONVERT(nvarchar(10), d.OrderDate, 104) as DONumber, d.OrderDate, 
			j.JobNumber + ' / ' + CONVERT(nvarchar(10), j.CreatedOn, 104) as JobNumber, j.CreatedOn,
			CASE WHEN LEN(d.CustomerPO1) <> 1 and LEN(d.CustomerPO2) <> 0 THEN d.CustomerPO1 + ', ' + d.CustomerPO2
				WHEN LEN(d.CustomerPO1) <> 1 and LEN(d.CustomerPO2) = 0 THEN d.CustomerPO1
				WHEN LEN(d.CustomerPO1) = 0 and LEN(d.CustomerPO2) <> 0 THEN d.CustomerPO2
				ELSE '' END as YourReference, o.TaxPercentage as TaxRate, o.Discount as DiscountRate,
				--o.TotalPrice as SubTotal, o.TotalPrice * o.Discount / 100 as Discount, 
				--o.TotalPrice * (1 - (o.Discount/100)) as SubTotalBeforGST,
				--(o.TotalPrice * (1 - (o.Discount/100))) * o.TaxPercentage / 100 as Tax,
				--ROUND(o.TotalPrice * (1 - (o.Discount/100)), 2) * (1 + (o.TaxPercentage/100)) as Total,
				case qd.UnitOfMeasure when 'cm2' then round((c.FaceWidth*c.Circumference/100)*ISNULL(c.UnitPrice,0),2) 
									else round(ISNULL(c.UnitPrice,0), 2) end as TotalPrice
		from tblJob j INNER JOIN
			tblOrderConfirmation o ON j.JobID = o.JobID INNER JOIN
			tblCylinder c on j.JobID = c.JobID INNER JOIN
			tblCustomerQuotationDetail qd ON c.PricingID = qd.ID LEFT JOIN 
			tblDeliveryOrder d ON j.JobID = d.JobID
		where j.JobID = @JobID
		)
		select distinct * , 
		(select SUM(T.TotalPrice) from T) as SubTotal ,
		(select SUM(T.TotalPrice) from T) * T.DiscountRate / 100 as Discount,
		(select SUM(T.TotalPrice) from T) * (1 - (T.DiscountRate/100)) as SubTotalBeforGST,
		(select SUM(T.TotalPrice) from T) * T.TaxRate / 100 as Tax,
		ROUND((select SUM(T.TotalPrice) from T) * (1 - (T.DiscountRate/100)), 2) * (1 + (T.TaxRate/100)) as Total
		from T
--exec tblInvoice_SelectJobSummary 894	
end


GO

/****** Object:  StoredProcedure [dbo].[tblEngravingCertificate_SelectAll]    Script Date: 06/12/2015 17:36:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[tblEngravingCertificate_SelectAll]
	@JobID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	select * from (
		select ISNULL(ed.EngravingID, -1) as EngravingID, ISNULL(ed.Sequence, c.Sequence) Sequence, c.CylinderNo, 
				ISNULL(ed.Color, c.Color) as Color, ed.Stylus, ed.Screen, ed.Angle, ed.Wall, CONVERT(nvarchar(50), ed.Gamma) as Gamma, ed.Sh, ed.Hl, ed.Ch, ed.Mt, 
				ISNULL(ed.IsCopy, 0) as IsCopy, ed.CellDepth, ed.CopperSh, ed.CopperCh, ed.ChromeSh, ed.ChromeCh,
				c.CylinderID, ISNULL(r.Name, '') as CylinderStatusName ,c.JobID, 0 as CellWidth
		from tblCylinder c LEFT OUTER JOIN
				tblReferences r ON c.ProcessTypeID = r.ReferencesID LEFT OUTER JOIN
				tblEngravingDetail ed ON c.CylinderID = ed.CylinderID
		where c.JobID = @JobID and c.Protocol = 'EMG'
		union all	
		select ISNULL(ed.EngravingID, -1) as EngravingID, ed.Sequence, '' as CylinderNo, 
				ISNULL(ed.Color, '') as Color, ed.Stylus, ed.Screen, ed.Angle, ed.Wall, CONVERT(nvarchar(50), ed.Gamma) as Gamma, ed.Sh, ed.Hl, ed.Ch, ed.Mt, 
				ISNULL(ed.IsCopy, 0) as IsCopy, ed.CellDepth, ed.CopperSh, ed.CopperCh, ed.ChromeSh, ed.ChromeCh,
				ed.CylinderID, 'EMG' as CylinderStatusName , ed.JobID, 0 as CellWidth
		from tblEngravingDetail ed
		where ed.JobID = @JobID and ed.CylinderID = -1
	) as A
	
	UNION ALL
	(
		--declare @JobID int = 165;
		select ISNULL(ed.EngravingID, -1) as EngravingID, ISNULL(ed.Sequence, c.Sequence) Sequence, c.CylinderNo, 
			ISNULL(ed.Color, c.Color) as Color, ed.Stylus, ed.Screen, ed.Angle, ed.ChannelWidth as Wall, ed.Gamma, '' as Sh, '' as Hl, '' as Ch, '' as Mt,
			ed.IsCopy, ed.CellDepth, '' as CopperSh, '' as CopperCh, '' as ChromeSh, '' as ChromeCh,
			ed.CylinderID, 'DLS' as CylinderStatusName, c.JobID, ed.CellWidth as CellWidth
		from tblCylinder c FULL OUTER JOIN
				tblEngravingTobacco ed ON c.CylinderID = ed.CylinderID LEFT OUTER JOIN
				tblReferences r ON c.ProcessTypeID = r.ReferencesID LEFT OUTER JOIN
				tblReferences hexa ON ed.Hexagonal = hexa.ReferencesID LEFT OUTER JOIN
				tblReferences cellshape ON ed.CellShape = cellshape.ReferencesID LEFT OUTER JOIN
				tblReferences gra ON ed.Gradation = gra.ReferencesID
		where c.JobID = @JobID and c.Protocol = 'DLS'
		UNION ALL
		select ISNULL(ed.EngravingID, -1) as EngravingID, ed.Sequence Sequence, '' as CylinderNo, 
			ed.Color as Color, ed.Stylus, ed.Screen, ed.Angle, ed.ChannelWidth as Wall, ed.Gamma, '' as Sh, '' as Hl, '' as Ch, '' as Mt,
			ed.IsCopy, ed.CellDepth, '' as CopperSh, '' as CopperCh, '' as ChromeSh, '' as ChromeCh,
			ed.CylinderID, 'DLS' as CylinderStatusName, ed.JobID, ed.CellWidth as CellWidth
		from tblEngravingTobacco ed LEFT OUTER JOIN
				tblReferences hexa ON ed.Hexagonal = hexa.ReferencesID LEFT OUTER JOIN
				tblReferences cellshape ON ed.CellShape = cellshape.ReferencesID LEFT OUTER JOIN
				tblReferences gra ON ed.Gradation = gra.ReferencesID
		where ed.JobID = @JobID and ed.CylinderID = -1
	)
	UNION ALL
	(
		select ISNULL(ed.EngravingID, -1) as EngravingID, ISNULL(ed.Sequence, c.Sequence) Sequence, c.CylinderNo, 
			ISNULL(ed.Color, c.Color) as Color, ed.Stylus, ed.Screen, ed.Angle, ed.ChromeCellWall as Wall, ed.Gamma, '' as Sh, '' as Hl, '' as Ch, '' as Mt,
			ed.IsCopy, ed.ChromeCellDepth as CellDepth, '' as CopperSh, '' as CopperCh, '' as ChromeSh, '' as ChromeCh,
			ed.CylinderID, 'Etching' as CylinderStatusName, c.JobID, ed.ChromeCellSize as CellWidth
		from tblCylinder c LEFT OUTER JOIN
				tblReferences r ON c.ProcessTypeID = r.ReferencesID LEFT OUTER JOIN
				tblEngravingEtching ed ON c.CylinderID = ed.CylinderID
		where c.JobID = @JobID and c.Protocol = 'Etching'
		union all
		select ISNULL(ed.EngravingID, -1) as EngravingID, ed.Sequence Sequence, '' as CylinderNo, 
			ed.Color , ed.Stylus, ed.Screen, ed.Angle, ed.ChromeCellWall as Wall, ed.Gamma, '' as Sh, '' as Hl, '' as Ch, '' as Mt,
			ed.IsCopy, ed.ChromeCellDepth as CellDepth, '' as CopperSh, '' as CopperCh, '' as ChromeSh, '' as ChromeCh,
			ed.CylinderID, 'Etching' as CylinderStatusName, ed.JobID, ed.ChromeCellSize as CellWidth			
		from tblEngravingEtching ed
		where ed.JobID = @JobID and ed.CylinderID = -1
	)
	order by Sequence, IsCopy
	
	--exec [tblEngravingCertificate_SelectAll] 165
END


GO
/****** Object:  StoredProcedure [dbo].[tblEngravingCertificate_SelectAll]    Script Date: 06/12/2015 17:37:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[tblEngravingCertificate_SelectAll]
	@JobID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	select * from (
		select ISNULL(ed.EngravingID, -1) as EngravingID, ISNULL(ed.Sequence, c.Sequence) Sequence, c.CylinderNo, 
				ISNULL(ed.Color, c.Color) as Color, ed.Stylus, ed.Screen, ed.Angle, ed.Wall, CONVERT(nvarchar(50), ed.Gamma) as Gamma, ed.Sh, ed.Hl, ed.Ch, ed.Mt, 
				ISNULL(ed.IsCopy, 0) as IsCopy, ed.CellDepth, ed.CopperSh, ed.CopperCh, ed.ChromeSh, ed.ChromeCh,
				c.CylinderID, ISNULL(r.Name, '') as CylinderStatusName ,c.JobID, 0 as CellWidth
		from tblCylinder c LEFT OUTER JOIN
				tblReferences r ON c.ProcessTypeID = r.ReferencesID LEFT OUTER JOIN
				tblEngravingDetail ed ON c.CylinderID = ed.CylinderID LEFT OUTER JOIN
				tblCylinderStatus cs on cs.CylinderStatusID = c.CylinderStatusID
		where c.JobID = @JobID and c.Protocol = 'EMG'
		and cs.CylinderStatusName not in ('Existing', 'Matching', 'Common')
		union all	
		select ISNULL(ed.EngravingID, -1) as EngravingID, ed.Sequence, '' as CylinderNo, 
				ISNULL(ed.Color, '') as Color, ed.Stylus, ed.Screen, ed.Angle, ed.Wall, CONVERT(nvarchar(50), ed.Gamma) as Gamma, ed.Sh, ed.Hl, ed.Ch, ed.Mt, 
				ISNULL(ed.IsCopy, 0) as IsCopy, ed.CellDepth, ed.CopperSh, ed.CopperCh, ed.ChromeSh, ed.ChromeCh,
				ed.CylinderID, 'EMG' as CylinderStatusName , ed.JobID, 0 as CellWidth
		from tblEngravingDetail ed
		where ed.JobID = @JobID and ed.CylinderID = -1
	) as A
	
	UNION ALL
	(
		--declare @JobID int = 165;
		select ISNULL(ed.EngravingID, -1) as EngravingID, ISNULL(ed.Sequence, c.Sequence) Sequence, c.CylinderNo, 
			ISNULL(ed.Color, c.Color) as Color, ed.Stylus, ed.Screen, ed.Angle, ed.ChannelWidth as Wall, ed.Gamma, '' as Sh, '' as Hl, '' as Ch, '' as Mt,
			ed.IsCopy, ed.CellDepth, '' as CopperSh, '' as CopperCh, '' as ChromeSh, '' as ChromeCh,
			ed.CylinderID, 'DLS' as CylinderStatusName, c.JobID, ed.CellWidth as CellWidth
		from tblCylinder c FULL OUTER JOIN
				tblEngravingTobacco ed ON c.CylinderID = ed.CylinderID LEFT OUTER JOIN
				tblReferences r ON c.ProcessTypeID = r.ReferencesID LEFT OUTER JOIN
				tblReferences hexa ON ed.Hexagonal = hexa.ReferencesID LEFT OUTER JOIN
				tblReferences cellshape ON ed.CellShape = cellshape.ReferencesID LEFT OUTER JOIN
				tblReferences gra ON ed.Gradation = gra.ReferencesID LEFT OUTER JOIN
				tblCylinderStatus cs on cs.CylinderStatusID = c.CylinderStatusID
		where c.JobID = @JobID and c.Protocol = 'DLS'
		and cs.CylinderStatusName not in ('Existing', 'Matching', 'Common')
		UNION ALL
		select ISNULL(ed.EngravingID, -1) as EngravingID, ed.Sequence Sequence, '' as CylinderNo, 
			ed.Color as Color, ed.Stylus, ed.Screen, ed.Angle, ed.ChannelWidth as Wall, ed.Gamma, '' as Sh, '' as Hl, '' as Ch, '' as Mt,
			ed.IsCopy, ed.CellDepth, '' as CopperSh, '' as CopperCh, '' as ChromeSh, '' as ChromeCh,
			ed.CylinderID, 'DLS' as CylinderStatusName, ed.JobID, ed.CellWidth as CellWidth
		from tblEngravingTobacco ed LEFT OUTER JOIN
				tblReferences hexa ON ed.Hexagonal = hexa.ReferencesID LEFT OUTER JOIN
				tblReferences cellshape ON ed.CellShape = cellshape.ReferencesID LEFT OUTER JOIN
				tblReferences gra ON ed.Gradation = gra.ReferencesID 
		where ed.JobID = @JobID and ed.CylinderID = -1
	)
	UNION ALL
	(
		select ISNULL(ed.EngravingID, -1) as EngravingID, ISNULL(ed.Sequence, c.Sequence) Sequence, c.CylinderNo, 
			ISNULL(ed.Color, c.Color) as Color, ed.Stylus, ed.Screen, ed.Angle, ed.ChromeCellWall as Wall, ed.Gamma, '' as Sh, '' as Hl, '' as Ch, '' as Mt,
			ed.IsCopy, ed.ChromeCellDepth as CellDepth, '' as CopperSh, '' as CopperCh, '' as ChromeSh, '' as ChromeCh,
			ed.CylinderID, 'Etching' as CylinderStatusName, c.JobID, ed.ChromeCellSize as CellWidth
		from tblCylinder c LEFT OUTER JOIN
				tblReferences r ON c.ProcessTypeID = r.ReferencesID LEFT OUTER JOIN
				tblEngravingEtching ed ON c.CylinderID = ed.CylinderID LEFT OUTER JOIN
				tblCylinderStatus cs on cs.CylinderStatusID = c.CylinderStatusID
		where c.JobID = @JobID and c.Protocol = 'Etching'
		and cs.CylinderStatusName not in ('Existing', 'Matching', 'Common')
		union all
		select ISNULL(ed.EngravingID, -1) as EngravingID, ed.Sequence Sequence, '' as CylinderNo, 
			ed.Color , ed.Stylus, ed.Screen, ed.Angle, ed.ChromeCellWall as Wall, ed.Gamma, '' as Sh, '' as Hl, '' as Ch, '' as Mt,
			ed.IsCopy, ed.ChromeCellDepth as CellDepth, '' as CopperSh, '' as CopperCh, '' as ChromeSh, '' as ChromeCh,
			ed.CylinderID, 'Etching' as CylinderStatusName, ed.JobID, ed.ChromeCellSize as CellWidth			
		from tblEngravingEtching ed
		where ed.JobID = @JobID and ed.CylinderID = -1
	)
	order by Sequence, IsCopy
	
	--exec [tblEngravingCertificate_SelectAll] 165
END


