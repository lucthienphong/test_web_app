USE [SweetSoft_APEM]
GO
/****** Object:  StoredProcedure [dbo].[tblProductionSchedule_Engraving]    Script Date: 06/15/2015 07:53:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblProductionSchedule_Engraving]
@DeliveryBegin datetime,
@DeliveryEnd datetime,
@EngravingBegin datetime,
@EngravingEnd datetime,
@ReproStatusID int
as
begin
	select distinct pr.EngravingDate as A, '!' + LEFT(DATENAME(DW, pr.EngravingDate),3) + ' ' + REPLACE(LEFT(CONVERT(varchar, pr.EngravingDate, 13), 6), ' ', '-') EngravingDate,
			REPLACE(LEFT(CONVERT(varchar, pr.DeliveryDate, 13), 6), ' ', '-') DeliveryDate,
			c.Code, j.JobNumber, j.RevNumber, j.JobName, j.Design, LEFT(Sale.FirstName, 2) as S, LEFT(Coord.FirstName, 2) as C, 
			ISNULL(EMGPr.Qty, 0) as EMGQty, ISNULL(DLSPr.Qty, 0) as DLSQty, 
			ISNULL(EtchingPr.Qty, 0) as EtchingQty, ISNULL(Matching.Qty, 0) as MatchingQty, js.Circumference, js.FaceWidth,
			REPLACE(LEFT(CONVERT(varchar, pr.ReproDate, 13), 6), ' ', '-') ReproDate, prRS.ReproStatusName, 
			REPLACE(LEFT(CONVERT(varchar, pr.CylinderDate, 13), 6), ' ', '-') CylinderDate, prCS.CylinderStatusName,
			pr.Note as Note, DATEPART(WK, pr.EngravingDate) as EngrWeek, YEAR(pr.EngravingDate) as EngrYear, 
			CASE WHEN pr.EngravingDate IS NULL THEN 0 ELSE 1 END AS HasEngravingDate			
		from tblJob j INNER JOIN
			tblJobSheet js ON j.JobID = js.JobID INNER JOIN
			tblCylinder cyl ON j.JobID = cyl.JobID INNER JOIN
			tblCylinderStatus cylSt ON cyl.CylinderStatusID = cylSt.CylinderStatusID INNER JOIN
			tblCustomer c ON j.CustomerID = c.CustomerID LEFT OUTER JOIN
			tblStaff Sale ON j.SalesRepID = Sale.StaffID LEFT OUTER JOIN
			tblStaff Coord ON j.CoordinatorID = Coord.StaffID LEFT OUTER JOIN
			(Select JobID, COUNT(CylinderID) as Qty 
					from tblCylinder sC INNER JOIN
						tblCylinderStatus sCS ON sc.CylinderStatusID = sCS.CylinderStatusID 
					where sc.Protocol = 'EMG' and sCS.Action = 'Repro'
					group by JobID) as EMGPr ON EMGPr.JobID = j.JobID LEFT OUTER JOIN
			(Select JobID, COUNT(CylinderID) as Qty 
					from tblCylinder sC INNER JOIN
						tblCylinderStatus sCS ON sc.CylinderStatusID = sCS.CylinderStatusID 
					where sc.Protocol = 'DLS' and sCS.Action = 'Repro'
					group by JobID) as DLSPr ON DLSPr.JobID = j.JobID LEFT OUTER JOIN
			(Select JobID, COUNT(CylinderID) as Qty 
					from tblCylinder sC INNER JOIN
						tblCylinderStatus sCS ON sc.CylinderStatusID = sCS.CylinderStatusID 
					where sc.Protocol = 'Etching' and sCS.Action = 'Repro'
					group by JobID) as EtchingPr ON EtchingPr.JobID = j.JobID LEFT OUTER JOIN
			(Select JobID, COUNT(CylinderID) as Qty 
					from tblCylinder sC INNER JOIN
						tblCylinderStatus sCS ON  sC.CylinderStatusID = sCS.CylinderStatusID
					where sc.Protocol = 'Matching' and sCS.Action = 'Repro'
					group by JobID) as Matching ON Matching.JobID = j.JobID LEFT OUTER JOIN
			tblProgress pr ON j.JobID = pr.JobID LEFT OUTER JOIN
			tblProgressReproStatus prRS ON pr.ReproStatusID = prRS.ReproStatusID LEFT OUTER JOIN
			tblProgressCylinderStatus prCS ON pr.CylinderStatusID = prCS.CylinderStatusID
		where j.IsClosed = 0 and j.IsOutsource = 0 and prRS.GoBackToRepro = 0
			and cylSt.Action = 'Repro' and cyl.Protocol in ('EMG', 'DLS', 'Etching', 'Matching')
			and (pr.ReproStatusID = @ReproStatusID or @ReproStatusID = 0)
			and (DATEDIFF(D, pr.DeliveryDate, @DeliveryBegin) <= 0 or @DeliveryBegin is NULL)
				and (DATEDIFF(D, pr.DeliveryDate, @DeliveryEnd) >= 0 or @DeliveryEnd is NULL)
			and (DATEDIFF(D, ISNULL(pr.EngravingDate, @EngravingBegin), @EngravingBegin) <= 0 or @EngravingBegin is NULL)
				and (DATEDIFF(D, ISNULL(pr.EngravingDate, @EngravingEnd), @EngravingEnd) >= 0 or @EngravingEnd is NULL)
				and j.Status <> N'Delivered'
				and j.Status <> N'Canceled'
		order by pr.EngravingDate, '!' + LEFT(DATENAME(DW, pr.EngravingDate),3) + ' ' + REPLACE(LEFT(CONVERT(varchar, pr.EngravingDate, 13), 6), ' ', '-') ,c.Code, j.JobNumber
		--exec tblProductionSchedule_Engraving null, null, null, null, 0
end		

GO

insert into tblReferences(Code,Name,[Type], IsObsolete) values('CC','CC',8,0)