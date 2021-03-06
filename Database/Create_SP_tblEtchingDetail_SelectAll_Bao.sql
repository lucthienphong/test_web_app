USE [SweetSoft_APEM]
GO
/****** Object:  StoredProcedure [dbo].[tblEngravingDetail_SelectAll]    Script Date: 02/03/2015 14:35:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[tblEtchingDetail_SelectAll]
@JobID int
as
begin
	select ISNULL(ed.EngravingID, -1) as EngravingID, c.Sequence, c.CylinderNo, c.Color,
			c.Color, ed.Stylus, ed.Screen, ed.Angle, ed.Wall, ed.Gamma, ed.Sh, ed.Hl, ed.Ch, ed.Mt, 
			ISNULL(ed.IsCopy,0) as IsCopy, ed.CellDepth, c.CylinderID,cs.CylinderStatusName
	from tblCylinder c LEFT OUTER JOIN
			tblEngravingDetail ed ON c.CylinderID = ed.CylinderID inner join tblCylinderStatus cs on c. CylinderStatusID = cs.CylinderStatusID
	where c.JobID = @JobID
	order by c.Sequence,ed.IsCopy
	--exec tblEngravingDetail_SelectAll 80
end
--select * from tblEngravingDetail
-- select * from tblJob
--exec tblEtchingDetail_SelectAll 80
