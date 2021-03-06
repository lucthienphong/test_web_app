USE [SweetSoft_APEM]
GO
/****** Object:  StoredProcedure [dbo].[tblEngravingDetail_SelectAll]    Script Date: 12/29/2014 10:17:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblEngravingDetail_SelectAll]
@JobID int
as
begin
	select ISNULL(ed.EngravingID, -1) as EngravingID, c.Sequence, c.CylinderNo, c.Color, ed.Stylus, ed.Screen, 
			ed.Angle, ed.Wall, ed.Gamma, ed.Sh, ed.Hl, ed.Ch, ed.Mt, ed.IsCopy, ed.CellDepth, c.CylinderID,cs.CylinderStatusName
		from tblCylinder c LEFT OUTER JOIN
			tblEngravingDetail ed ON c.CylinderID = ed.CylinderID inner join tblCylinderStatus cs on c.	CylinderStatusID = cs.CylinderStatusID
		where c.JobID = @JobID
		order by ed.IsCopy ,c.Sequence
	--exec tblEngravingDetail_SelectAll 18
end
