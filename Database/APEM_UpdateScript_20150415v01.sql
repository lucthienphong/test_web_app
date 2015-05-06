GO
IF COL_LENGTH('tblEngravingDetail','Color') IS  NULL
BEGIN
	ALTER table tblEngravingDetail
		ADD Color nvarchar(100) null
END

GO
IF COL_LENGTH('tblEngravingEtching','Color') IS  NULL
BEGIN
	ALTER table tblEngravingEtching
		ADD Color nvarchar(100) null
END

GO
IF COL_LENGTH('tblEngravingTobacco','Color') IS  NULL
BEGIN
	ALTER table tblEngravingTobacco
		ADD Color nvarchar(100) null
END


GO
/****** Object:  StoredProcedure [dbo].[tblEngravingEtching_SelectAllForPrint]    Script Date: 15/04/2015 19:17:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER proc [dbo].[tblEngravingEtching_SelectAllForPrint]
@JobID int
as
begin
	select ISNULL(ed.EngravingID, -1) as EngravingID, ISNULL(ed.Sequence, c.Sequence) Sequence, c.CylinderNo, 
			c.CylinderID, r.Name as CylinderStatusName, c.Color, ISNULL(ed.IsCopy,0) as IsCopy,
			ed.ScreenLpi, ed.CellType, ed.Angle, ed.Gamma, ed.TargetCellSize, ed.TargetCellWall, ed.TargetCellDepth,
			ed.DevelopingTime, ed.EtchingTime, ed.ChromeCellSize, ed.ChromeCellWall, ed.ChromeCellDepth			
	from tblCylinder c LEFT OUTER JOIN
			tblReferences r ON c.ProcessTypeID = r.ReferencesID LEFT OUTER JOIN
			tblEngravingEtching ed ON c.CylinderID = ed.CylinderID
	where c.JobID = @JobID
	union all
	select ISNULL(ed.EngravingID, -1) as EngravingID, ed.Sequence, '' as CylinderNo, 
			ed.CylinderID, '' as CylinderStatusName, ed.Color as Color, ISNULL(ed.IsCopy,0) as IsCopy,
			ed.ScreenLpi, ed.CellType, ed.Angle, ed.Gamma, ed.TargetCellSize, ed.TargetCellWall, ed.TargetCellDepth,
			ed.DevelopingTime, ed.EtchingTime, ed.ChromeCellSize, ed.ChromeCellWall, ed.ChromeCellDepth			
	from tblEngravingEtching ed
	where ed.JobID = @JobID and ed.CylinderID = -1
	order by Sequence, IsCopy
	--exec tblEngravingEtching_SelectAllForPrint 178
end


GO
/****** Object:  StoredProcedure [dbo].[tblEngravingDetail_SelectAllForPrint]    Script Date: 17/04/2015 00:03:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[tblEngravingDetail_SelectAllForPrint]
@JobID int
as
begin
	select ISNULL(ed.EngravingID, -1) as EngravingID, ISNULL(ed.Sequence, c.Sequence) Sequence, c.CylinderNo, 
			ISNULL(ed.Color, c.Color) as Color, ed.Stylus, ed.Screen, ed.Angle, ed.Wall, ed.Gamma, ed.Sh, ed.Hl, ed.Ch, ed.Mt, 
			ISNULL(ed.IsCopy, 0) as IsCopy, ed.CellDepth, ed.CopperSh, ed.CopperCh, ed.ChromeSh, ed.ChromeCh,
			c.CylinderID, r.Name as CylinderStatusName 
	from tblCylinder c LEFT OUTER JOIN
			tblReferences r ON c.ProcessTypeID = r.ReferencesID LEFT OUTER JOIN
			tblEngravingDetail ed ON c.CylinderID = ed.CylinderID
	where c.JobID = @JobID
	order by ISNULL(ed.Sequence, c.Sequence),ed.IsCopy
	--exec tblEngravingDetail_SelectAllForPrint 100
end



GO
/****** Object:  StoredProcedure [dbo].[tblEngravingDetail_SelectAllForPrint]    Script Date: 15/04/2015 19:18:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[tblEngravingTobacco_SelectAllForPrint]
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
	where c.JobID = @JobID
	UNION ALL
	select ISNULL(ed.EngravingID, -1) as EngravingID, ed.Sequence, '' CylinderNo, 
			ed.CylinderID, '' as CylinderStatusName, ed.Color, ISNULL(ed.IsCopy,0) as IsCopy,
			ed.Screen, ISNULL(ed.MasterScreen, 0) as MasterScreen, ed.Angle, ed.Elongation, ed.Distotion, 
			ed.Resolution, ed.Hexagonal, hexa.Name as HexaName, ISNULL(ed.ImageSmoothness, 0) as ImageSmoothness, 
			ed.UnsharpMasking, ed.Antialiasing, ed.LineworkWidening,ed.EngravingStart, ed.EngravingWidth, 
			ed.CellShape, CellShape.Name as CellShapeName, ed.Gradation, gra.Name as GraName, ed.Gamma, 
			ISNULL(ed.LaserA, 0) as LaserA, ISNULL(ed.LaserB, 0) as LaserB, 
			ed.CellWidth, ed.ChannelWidth, ed.CellDepth, ed.EngravingTime, ed.Beam, ed.Threshold, ed.CheckedBy, ed.CheckedOn
	from tblEngravingTobacco ed LEFT OUTER JOIN
			tblReferences hexa ON ed.Hexagonal = hexa.ReferencesID LEFT OUTER JOIN
			tblReferences cellshape ON ed.CellShape = cellshape.ReferencesID LEFT OUTER JOIN
			tblReferences gra ON ed.Gradation = gra.ReferencesID
	where ed.JobID = @JobID and ed.CylinderID = -1
	order by Sequence, IsCopy
	--exec tblEngravingTobacco_SelectAllForPrint 178
end

GO
/****** Object:  StoredProcedure [dbo].[tblEngravingTobacco_SelectAll]    Script Date: 15/04/2015 19:35:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblEngravingTobacco_SelectAll]
@JobID int
as
begin
	select ISNULL(ed.EngravingID, -1) as EngravingID, ISNULL(ed.Sequence, c.Sequence) Sequence, c.CylinderNo, 
			c.CylinderID, r.Name as CylinderStatusName, ISNULL(ed.Color, c.Color) as Color, ISNULL(ed.IsCopy,0) as IsCopy,
			ed.Screen, ISNULL(ed.MasterScreen, 0) as MasterScreen, ed.Angle, ed.Elongation, ed.Distotion, 
			ed.Resolution, ed.Hexagonal, hexa.Name as HexaName, ISNULL(ed.ImageSmoothness, 0) as ImageSmoothness, 
			ed.UnsharpMasking, ed.Antialiasing, ed.LineworkWidening,ed.EngravingStart, ed.EngravingWidth, 
			ed.CellShape, CellShape.Name as CellShapeName, ed.Gradation, gra.Name as GraName, ed.Gamma, 
			ISNULL(ed.LaserA, 0) as LaserA, ISNULL(ed.LaserB, 0) as LaserB, 
			ed.CellWidth, ed.ChannelWidth, ed.CellDepth, ed.EngravingTime, ed.Beam, ed.Threshold, ed.CheckedBy, ed.CheckedOn
	from tblCylinder c FULL OUTER JOIN
			tblEngravingTobacco ed ON c.CylinderID = ed.CylinderID LEFT OUTER JOIN
			tblReferences r ON c.ProcessTypeID = r.ReferencesID LEFT OUTER JOIN
			tblReferences hexa ON ed.Hexagonal = hexa.ReferencesID LEFT OUTER JOIN
			tblReferences cellshape ON ed.CellShape = cellshape.ReferencesID LEFT OUTER JOIN
			tblReferences gra ON ed.Gradation = gra.ReferencesID
	where c.JobID = @JobID and c.Protocol = 'DLS'
	UNION ALL
	select ISNULL(ed.EngravingID, -1) as EngravingID, ed.Sequence, '' CylinderNo, 
			ed.CylinderID, '' as CylinderStatusName, ed.Color, ISNULL(ed.IsCopy,0) as IsCopy,
			ed.Screen, ISNULL(ed.MasterScreen, 0) as MasterScreen, ed.Angle, ed.Elongation, ed.Distotion, 
			ed.Resolution, ed.Hexagonal, hexa.Name as HexaName, ISNULL(ed.ImageSmoothness, 0) as ImageSmoothness, 
			ed.UnsharpMasking, ed.Antialiasing, ed.LineworkWidening,ed.EngravingStart, ed.EngravingWidth, 
			ed.CellShape, CellShape.Name as CellShapeName, ed.Gradation, gra.Name as GraName, ed.Gamma, 
			ISNULL(ed.LaserA, 0) as LaserA, ISNULL(ed.LaserB, 0) as LaserB, 
			ed.CellWidth, ed.ChannelWidth, ed.CellDepth, ed.EngravingTime, ed.Beam, ed.Threshold, ed.CheckedBy, ed.CheckedOn
	from tblEngravingTobacco ed LEFT OUTER JOIN
			tblReferences hexa ON ed.Hexagonal = hexa.ReferencesID LEFT OUTER JOIN
			tblReferences cellshape ON ed.CellShape = cellshape.ReferencesID LEFT OUTER JOIN
			tblReferences gra ON ed.Gradation = gra.ReferencesID
	where ed.JobID = @JobID and ed.CylinderID = -1
	order by Sequence, IsCopy
	--exec tblEngravingTobacco_SelectAll 178
end

GO
/****** Object:  StoredProcedure [dbo].[tblEngravingEtching_SelectAll]    Script Date: 16/04/2015 23:36:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblEngravingEtching_SelectAll]
@JobID int
as
begin
	select ISNULL(ed.EngravingID, -1) as EngravingID, ISNULL(ed.Sequence, c.Sequence) Sequence, c.CylinderNo, 
			c.CylinderID, r.Name as CylinderStatusName, c.Color, ISNULL(ed.IsCopy,0) as IsCopy,
			ed.ScreenLpi, ed.CellType, ed.Angle, ed.Gamma, ed.TargetCellSize, ed.TargetCellWall, ed.TargetCellDepth,
			ed.DevelopingTime, ed.EtchingTime, ed.ChromeCellSize, ed.ChromeCellWall, ed.ChromeCellDepth			
	from tblCylinder c LEFT OUTER JOIN
			tblReferences r ON c.ProcessTypeID = r.ReferencesID LEFT OUTER JOIN
			tblEngravingEtching ed ON c.CylinderID = ed.CylinderID
	where c.JobID = @JobID and c.Protocol = 'Etching'
	union all
	select ISNULL(ed.EngravingID, -1) as EngravingID, ed.Sequence, '' as CylinderNo, 
			ed.CylinderID, '' as CylinderStatusName, ed.Color as Color, ISNULL(ed.IsCopy,0) as IsCopy,
			ed.ScreenLpi, ed.CellType, ed.Angle, ed.Gamma, ed.TargetCellSize, ed.TargetCellWall, ed.TargetCellDepth,
			ed.DevelopingTime, ed.EtchingTime, ed.ChromeCellSize, ed.ChromeCellWall, ed.ChromeCellDepth			
	from tblEngravingEtching ed
	where ed.JobID = @JobID and ed.CylinderID = -1
	order by Sequence, IsCopy

	--exec tblEngravingEtching_SelectAll 178
end

GO
/****** Object:  StoredProcedure [dbo].[tblEngravingDetail_SelectAll]    Script Date: 17/04/2015 00:02:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblEngravingDetail_SelectAll]
@JobID int
as
begin
	select ISNULL(ed.EngravingID, -1) as EngravingID, ISNULL(ed.Sequence, c.Sequence) Sequence, c.CylinderNo, 
			ISNULL(ed.Color, c.Color) as Color, ed.Stylus, ed.Screen, ed.Angle, ed.Wall, ed.Gamma, ed.Sh, ed.Hl, ed.Ch, ed.Mt, 
			ISNULL(ed.IsCopy, 0) as IsCopy, ed.CellDepth, ed.CopperSh, ed.CopperCh, ed.ChromeSh, ed.ChromeCh,
			c.CylinderID, r.Name as CylinderStatusName 
	from tblCylinder c LEFT OUTER JOIN
			tblReferences r ON c.ProcessTypeID = r.ReferencesID LEFT OUTER JOIN
			tblEngravingDetail ed ON c.CylinderID = ed.CylinderID
	where c.JobID = @JobID and c.Protocol = 'EMG'
	order by ISNULL(ed.Sequence, c.Sequence),ed.IsCopy
	--exec tblEngravingDetail_SelectAll 178
end

