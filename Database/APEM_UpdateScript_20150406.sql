GO
alter proc tblProductionSchedule_Engraving
@DeliveryBegin datetime,
@DeliveryEnd datetime,
@EngravingBegin datetime,
@EngravingEnd datetime,
@ReproStatusID int
as
begin
	select distinct '!' + LEFT(DATENAME(DW, pr.EngravingDate),3) + ' ' + REPLACE(LEFT(CONVERT(varchar, pr.EngravingDate, 13), 6), ' ', '-') EngravingDate,
			REPLACE(LEFT(CONVERT(varchar, pr.DeliveryDate, 13), 6), ' ', '-') DeliveryDate,
			c.Code, j.JobNumber, j.RevNumber, j.JobName, j.Design, LEFT(Sale.FirstName, 2) as S, LEFT(Coord.FirstName, 2) as C, 
			ISNULL(EMGPr.Qty, 0) as EMGQty, ISNULL(DLSPr.Qty, 0) as DLSQty, 
			ISNULL(EtchingPr.Qty, 0) as EtchingQty, ISNULL(Matching.Qty, 0) as MatchingQty, js.Circumference, js.FaceWidth,
			REPLACE(LEFT(CONVERT(varchar, pr.ReproDate, 13), 6), ' ', '-') ReproDate, prRS.ReproStatusName, 
			REPLACE(LEFT(CONVERT(varchar, pr.CylinderDate, 13), 6), ' ', '-') CylinderDate, prCS.CylinderStatusName,
			'' as Note, DATEPART(WK, pr.EngravingDate) as EngrWeek, YEAR(pr.EngravingDate) as EngrYear, 
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
		where j.IsClosed = 0 and j.IsOutsource = 0
			and cylSt.Action = 'Repro' and cyl.Protocol in ('EMG', 'DLS', 'Etching', 'Matching')
			and (pr.ReproStatusID = @ReproStatusID or @ReproStatusID = 0)
			and (DATEDIFF(D, pr.DeliveryDate, @DeliveryBegin) <= 0 or @DeliveryBegin is NULL)
				and (DATEDIFF(D, pr.DeliveryDate, @DeliveryEnd) >= 0 or @DeliveryEnd is NULL)
			and (DATEDIFF(D, ISNULL(pr.EngravingDate, @EngravingBegin), @EngravingBegin) <= 0 or @EngravingBegin is NULL)
				and (DATEDIFF(D, ISNULL(pr.EngravingDate, @EngravingEnd), @EngravingEnd) >= 0 or @EngravingEnd is NULL)
		order by '!' + LEFT(DATENAME(DW, pr.EngravingDate),3) + ' ' + REPLACE(LEFT(CONVERT(varchar, pr.EngravingDate, 13), 6), ' ', '-'), j.JobNumber
		--exec tblProductionSchedule_Engraving null, null, null, null, 0
end		


GO
alter proc tblProductionSchedule_Embossing
@DeliveryBegin datetime,
@DeliveryEnd datetime,
@EngravingBegin datetime,
@EngravingEnd datetime,
@ReproStatusID int
as
begin
	select distinct '!' + LEFT(DATENAME(DW, pr.EngravingDate),3) + ' ' + REPLACE(LEFT(CONVERT(varchar, pr.EngravingDate, 13), 6), ' ', '-') EngravingDate,
			REPLACE(LEFT(CONVERT(varchar, pr.DeliveryDate, 13), 6), ' ', '-') DeliveryDate,
			c.Code, j.JobNumber, j.RevNumber, j.JobName, j.Design, LEFT(Sale.FirstName, 2) as S, LEFT(Coord.FirstName, 2) as C, 
			ISNULL(CNCPr.Qty, 0) as CNCQty, ISNULL(DigilasPr.Qty, 0) as DigilasQty, js.Circumference, js.FaceWidth,
			REPLACE(LEFT(CONVERT(varchar, pr.ReproDate, 13), 6), ' ', '-') ReproDate, prRS.ReproStatusName, 
			REPLACE(LEFT(CONVERT(varchar, pr.CylinderDate, 13), 6), ' ', '-') CylinderDate, prCS.CylinderStatusName,
			'' as Note, DATEPART(WK, pr.EngravingDate) as EngrWeek, YEAR(pr.EngravingDate) as EngrYear, 
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
					where sc.Protocol = 'CNC' and sCS.Action = 'Embossing'
					group by JobID) as CNCPr ON CNCPr.JobID = j.JobID LEFT OUTER JOIN
			(Select JobID, COUNT(CylinderID) as Qty 
					from tblCylinder sC INNER JOIN
						tblCylinderStatus sCS ON sc.CylinderStatusID = sCS.CylinderStatusID 
					where sc.Protocol = 'Digilas' and sCS.Action = 'Embossing'
					group by JobID) as DigilasPr ON DigilasPr.JobID = j.JobID LEFT OUTER JOIN
			tblProgress pr ON j.JobID = pr.JobID LEFT OUTER JOIN
			tblProgressReproStatus prRS ON pr.ReproStatusID = prRS.ReproStatusID LEFT OUTER JOIN
			tblProgressCylinderStatus prCS ON pr.CylinderStatusID = prCS.CylinderStatusID
		where j.IsClosed = 0 and j.IsOutsource = 0
			and cylSt.Action = 'Repro' and cyl.Protocol in ('CNC', 'Digilas')
			and (pr.ReproStatusID = @ReproStatusID or @ReproStatusID = 0)
			and (DATEDIFF(D, pr.DeliveryDate, @DeliveryBegin) <= 0 or @DeliveryBegin is NULL)
				and (DATEDIFF(D, pr.DeliveryDate, @DeliveryEnd) >= 0 or @DeliveryEnd is NULL)
			and (DATEDIFF(D, pr.EngravingDate, @EngravingBegin) <= 0 or @EngravingBegin is NULL)
				and (DATEDIFF(D, pr.EngravingDate, @EngravingEnd) >= 0 or @EngravingEnd is NULL)
		order by '!' + LEFT(DATENAME(DW, pr.EngravingDate),3) + ' ' + REPLACE(LEFT(CONVERT(varchar, pr.EngravingDate, 13), 6), ' ', '-'), j.JobNumber
		--exec tblProductionSchedule_Embossing null, null, null, null, 0
end


GO
alter proc tblProductionSchedule_DeReChrome
@DeliveryBegin datetime,
@DeliveryEnd datetime,
@DeReBegin datetime,
@DeReEnd datetime
as
begin
	select '!' + LEFT(DATENAME(DW, pr.DeReDate),3) + ' ' + REPLACE(LEFT(CONVERT(varchar, pr.DeReDate, 13), 6), ' ', '-') DeReDate,
			REPLACE(LEFT(CONVERT(varchar, pr.DeliveryDate, 13), 6), ' ', '-') DeliveryDate,
			c.Code, j.JobNumber, j.RevNumber, j.JobName, j.Design, LEFT(Sale.FirstName, 2) as S, LEFT(Coord.FirstName, 2) as C, 
			ISNULL(DeRe.Qty, 0) as DeReQty, js.Circumference, js.FaceWidth,
			REPLACE(LEFT(CONVERT(varchar, pr.CylinderDate, 13), 6), ' ', '-') CylinderDate, prCS.CylinderStatusName,
			'' as Note, DATEPART(WK, pr.DeReDate) as EngrWeek, YEAR(pr.DeReDate) as EngrYear, 
			CASE WHEN pr.DeReDate IS NULL THEN 0 ELSE 1 END AS HasDeReDate			
		from tblJob j INNER JOIN
			tblJobSheet js ON j.JobID = js.JobID INNER JOIN
			tblCylinder cyl ON j.JobID = cyl.JobID INNER JOIN
			tblCylinderStatus cylSt ON cyl.CylinderStatusID = cylSt.CylinderStatusID INNER JOIN
			tblCustomer c ON j.CustomerID = c.CustomerID LEFT OUTER JOIN
			tblStaff Sale ON j.SalesRepID = Sale.StaffID LEFT OUTER JOIN
			tblStaff Coord ON j.CoordinatorID = Coord.StaffID LEFT OUTER JOIN
			(Select JobID, COUNT(CylinderID) as Qty 
					from tblCylinder sC INNER JOIN
						tblCylinderStatus sCS ON  sC.CylinderStatusID = sCS.CylinderStatusID
					where sCS.Action = 'DeReChrome'
					group by JobID) as DeRe ON DeRe.JobID = j.JobID LEFT OUTER JOIN
			tblProgress pr ON j.JobID = pr.JobID LEFT OUTER JOIN
			tblProgressCylinderStatus prCS ON pr.CylinderStatusID = prCS.CylinderStatusID
		where j.IsClosed = 0 and j.IsOutsource = 0
			and cylSt.Action = 'DeReChrome'
			and (DATEDIFF(D, pr.DeliveryDate, @DeliveryBegin) <= 0 or @DeliveryBegin is NULL)
				and (DATEDIFF(D, pr.DeliveryDate, @DeliveryEnd) >= 0 or @DeliveryEnd is NULL)
			and (DATEDIFF(D, pr.DeReDate, @DeReBegin) <= 0 or @DeReBegin is NULL)
				and (DATEDIFF(D, pr.EngravingDate, @DeReEnd) >= 0 or @DeReEnd is NULL)
		order by '!' + LEFT(DATENAME(DW, pr.DeReDate),3) + ' ' + REPLACE(LEFT(CONVERT(varchar, pr.DeReDate, 13), 6), ' ', '-'), j.JobNumber
		--exec tblProductionSchedule_DeReChrome null, null, null, null
end

GO
/****** Object:  StoredProcedure [dbo].[tblJob_ProgressForRepro]    Script Date: 04/07/2015 09:23:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblJob_ProgressForRepro]
@OrderDateBegin datetime,
@OrderDateEnd datetime,
@ProofBegin datetime,
@ProofEnd datetime,
@ReproDateBegin datetime,
@ReproDateEnd datetime,
@CylinderDateBegin datetime,
@CylinderDateEnd datetime,
@ReproStatusID int,
@PageIndex int,
@PageSize int,
@SortColumn varchar(2),
@SortType varchar(1)
as
begin
	;WITH T AS(
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN  c.Code END ASC,
															CASE @SortColumn WHEN N'1' THEN j.JobNumber END ASC,
															CASE @SortColumn WHEN N'2' THEN j.RevNumber END ASC,
															CASE @SortColumn WHEN N'3' THEN j.JobName END ASC,
															CASE @SortColumn WHEN N'4' THEN j.Design END ASC,
															CASE @SortColumn WHEN N'5' THEN ISNULL(COUNT(cyl.CylinderID),0) END ASC,
															CASE @SortColumn WHEN N'6' THEN j.CreatedOn END ASC,
															--CASE @SortColumn WHEN N'7' THEN p.ProofDate END ASC,
															CASE @SortColumn WHEN N'7' THEN ISNULL(p.ReproDate, js.ReproDate) END ASC,
															CASE @SortColumn WHEN N'8' THEN pr.ReproStatusName END ASC,
															CASE @SortColumn WHEN N'9' THEN ISNULL(p.CylinderDate, js.CylinderDate) END ASC,
															CASE @SortColumn WHEN N'10' THEN pc.CylinderStatusName END ASC, 
															CASE @SortColumn WHEN N'11' THEN p.Note END ASC) as RowIndex, 
				j.JobID, c.Code, j.JobNumber, j.RevNumber, j.JobName, j.Design, ISNULL(COUNT(cyl.CylinderID),0) as Qty, 
				CONVERT(nvarchar(10), j.CreatedOn, 103) as CreateOn, 
				ISNULL(CONVERT(nvarchar(10), p.ProofDate, 103),'') as ProofDate,
				CONVERT(nvarchar(10), ISNULL(p.ReproDate, js.ReproDate), 103) as ReproDate, pr.ReproStatusName,
				CONVERT(nvarchar(10), ISNULL(p.CylinderDate, js.CylinderDate), 103) as CylinderDate, pc.CylinderStatusName, p.Note
			from tblJob j INNER JOIN
				tblJobSheet js ON j.JobID = js.JobID INNER JOIN
				tblCustomer c ON j.CustomerID = c.CustomerID LEFT OUTER JOIN
				tblCylinder cyl ON j.JobID = cyl.JobID INNER JOIN
				tblCylinderStatus cst ON cyl.CylinderStatusID = cst.CylinderStatusID LEFT OUTER JOIN
				tblProgress p ON j.JobID = p.JobID LEFT OUTER JOIN
				tblProgressReproStatus pr ON p.ReproStatusID = pr.ReproStatusID LEFT OUTER JOIN
				tblProgressCylinderStatus pc ON p.CylinderStatusID = pc.CylinderStatusID
			where j.IsClosed = 0 and j.IsOutsource = 0
				and cst.Action = 'Repro'
				and (pr.GoBackToRepro = 1 or p.JobID IS NULL or (p.JobID is NOT NULL and p.ReproStatusID IS NULL))
				and (DATEDIFF(D, j.CreatedOn, @OrderDateBegin) <=0 or @OrderDateBegin IS NULL)
					and (DATEDIFF(D, j.CreatedOn, @OrderDateEnd) >=0 or @OrderDateEnd IS NULL)
				and (DATEDIFF(D, p.ProofDate, @ProofBegin) <=0 or @ProofBegin IS NULL)
					and (DATEDIFF(D, p.ProofDate, @ProofEnd) >=0 or @ProofEnd IS NULL)
				and (DATEDIFF(D, ISNULL(p.ReproDate, js.ReproDate), @ReproDateBegin) <=0 or @ReproDateBegin IS NULL)
					and (DATEDIFF(D, ISNULL(p.ReproDate, js.ReproDate), @ReproDateEnd) >=0 or @ReproDateEnd IS NULL)
				and (DATEDIFF(D, ISNULL(p.CylinderDate, js.CylinderDate), @CylinderDateBegin) <=0 or @CylinderDateBegin IS NULL)
					and (DATEDIFF(D, ISNULL(p.CylinderDate, js.CylinderDate), @CylinderDateEnd) >=0 or @CylinderDateEnd IS NULL)
				and (p.ReproStatusID = @ReproStatusID or @ReproStatusID = 0)
				and @SortType = 'A'
			group by j.JobID, c.Code, j.JobNumber, j.RevNumber, j.JobName, j.Design, j.CreatedOn, p.ReproDate, js.ReproDate, pr.ReproStatusName,
				p.CylinderDate, js.CylinderDate, pc.CylinderStatusName, p.Note, p.ProofDate
		union all
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN  c.Code END DESC,
															CASE @SortColumn WHEN N'1' THEN j.JobNumber END DESC,
															CASE @SortColumn WHEN N'2' THEN j.RevNumber END DESC,
															CASE @SortColumn WHEN N'3' THEN j.JobName END DESC,
															CASE @SortColumn WHEN N'4' THEN j.Design END DESC,
															CASE @SortColumn WHEN N'5' THEN ISNULL(COUNT(cyl.CylinderID),0) END DESC,
															CASE @SortColumn WHEN N'6' THEN j.CreatedOn END DESC,
															--CASE @SortColumn WHEN N'7' THEN p.ProofDate END DESC,
															CASE @SortColumn WHEN N'7' THEN ISNULL(p.ReproDate, js.ReproDate) END DESC,
															CASE @SortColumn WHEN N'8' THEN pr.ReproStatusName END ASC,
															CASE @SortColumn WHEN N'9' THEN ISNULL(p.CylinderDate, js.CylinderDate) END DESC,
															CASE @SortColumn WHEN N'10' THEN pc.CylinderStatusName END DESC, 
															CASE @SortColumn WHEN N'11' THEN p.Note END DESC) as RowIndex, 
				j.JobID, c.Code, j.JobNumber, j.RevNumber, j.JobName, j.Design, ISNULL(COUNT(cyl.CylinderID),0) as Qty, 
				CONVERT(nvarchar(10), j.CreatedOn, 103) as CreateOn, 
				ISNULL(CONVERT(nvarchar(10), p.ProofDate, 103),'') as ProofDate,
				CONVERT(nvarchar(10), ISNULL(p.ReproDate, js.ReproDate), 103) as ReproDate, pr.ReproStatusName,
				CONVERT(nvarchar(10), ISNULL(p.CylinderDate, js.CylinderDate), 103) as CylinderDate, pc.CylinderStatusName, p.Note
			from tblJob j INNER JOIN
				tblJobSheet js ON j.JobID = js.JobID INNER JOIN
				tblCustomer c ON j.CustomerID = c.CustomerID LEFT OUTER JOIN
				tblCylinder cyl ON j.JobID = cyl.JobID INNER JOIN
				tblCylinderStatus cst ON cyl.CylinderStatusID = cst.CylinderStatusID LEFT OUTER JOIN
				tblProgress p ON j.JobID = p.JobID LEFT OUTER JOIN
				tblProgressReproStatus pr ON p.ReproStatusID = pr.ReproStatusID LEFT OUTER JOIN
				tblProgressCylinderStatus pc ON p.CylinderStatusID = pc.CylinderStatusID
			where j.IsClosed = 0 and j.IsOutsource = 0
				and cst.Action = 'Repro'
				and (pr.GoBackToRepro = 1 or p.JobID IS NULL or (p.JobID is NOT NULL and p.ReproStatusID IS NULL))
				and (DATEDIFF(D, j.CreatedOn, @OrderDateBegin) <=0 or @OrderDateBegin IS NULL)
					and (DATEDIFF(D, j.CreatedOn, @OrderDateEnd) >=0 or @OrderDateEnd IS NULL)
				and (DATEDIFF(D, p.ProofDate, @ProofBegin) <=0 or @ProofBegin IS NULL)
					and (DATEDIFF(D, p.ProofDate, @ProofEnd) >=0 or @ProofEnd IS NULL)
				and (DATEDIFF(D, ISNULL(p.ReproDate, js.ReproDate), @ReproDateBegin) <=0 or @ReproDateBegin IS NULL)
					and (DATEDIFF(D, ISNULL(p.ReproDate, js.ReproDate), @ReproDateEnd) >=0 or @ReproDateEnd IS NULL)
				and (DATEDIFF(D, ISNULL(p.CylinderDate, js.CylinderDate), @CylinderDateBegin) <=0 or @CylinderDateBegin IS NULL)
					and (DATEDIFF(D, ISNULL(p.CylinderDate, js.CylinderDate), @CylinderDateEnd) >=0 or @CylinderDateEnd IS NULL)
				and (p.ReproStatusID = @ReproStatusID or @ReproStatusID = 0)
				and @SortType = 'D'
			group by j.JobID, c.Code, j.JobNumber, j.RevNumber, j.JobName, j.Design, j.CreatedOn, p.ReproDate, js.ReproDate, pr.ReproStatusName,
				p.CylinderDate, js.CylinderDate, pc.CylinderStatusName, p.Note, p.ProofDate
	)
	select top (@PageSize)  *, (select COUNT(RowIndex) from T) as RowsCount
		from T
		where @PageSize <> 0 AND RowIndex > (@PageIndex*@PageSize)
	union all
	select *, (select COUNT(RowIndex) from T) as RowsCount
		from T
		where @PageSize = 0
--exec tblJob_ProgressForRepro null, null, null, null, null, null, null, null, 0, 0, 0, '0', 'D'
end

GO
/****** Object:  StoredProcedure [dbo].[tblJob_ProgressForEngraving]    Script Date: 04/07/2015 09:29:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblJob_ProgressForEngraving]
@DeliveryBegin datetime,
@DeliveryEnd datetime,
@EngravingBegin datetime,
@EngravingEnd datetime,
@ReproStatusID int,
@PageIndex int,
@PageSize int,
@SortColumn varchar(2),
@SortType varchar(1)
as
begin
	;WITH T AS(
	select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN  c.Code END ASC,
															CASE @SortColumn WHEN N'1' THEN j.JobNumber END ASC,
															CASE @SortColumn WHEN N'2' THEN j.RevNumber END ASC,
															CASE @SortColumn WHEN N'3' THEN j.JobName END ASC,
															CASE @SortColumn WHEN N'4' THEN j.Design END ASC,
															CASE @SortColumn WHEN N'5' THEN ISNULL(COUNT(cyl.CylinderID),0) END ASC,
															CASE @SortColumn WHEN N'6' THEN p.DeliveryDate END ASC,
															CASE @SortColumn WHEN N'7' THEN p.EngravingDate END ASC,
															CASE @SortColumn WHEN N'8' THEN ISNULL(p.ReproDate, js.ReproDate) END ASC,
															CASE @SortColumn WHEN N'9' THEN pr.ReproStatusName END ASC,
															CASE @SortColumn WHEN N'10' THEN ISNULL(p.CylinderDate, js.CylinderDate) END ASC,
															CASE @SortColumn WHEN N'11' THEN pc.CylinderStatusName END ASC, 
															CASE @SortColumn WHEN N'12' THEN p.Note END ASC) as RowIndex, 
			j.JobID, c.Code, j.JobNumber, j.RevNumber, j.JobName, j.Design, ISNULL(COUNT(cyl.CylinderID),0) as Qty,
			ISNULL(CONVERT(nvarchar(10), p.DeliveryDate, 103),'') as DeliveryDate,
			ISNULL(CONVERT(nvarchar(10), p.EngravingDate, 103),'') as EngravingDate,
			CONVERT(nvarchar(10), ISNULL(p.ReproDate, js.ReproDate), 103) as ReproDate, pr.ReproStatusName,
			CONVERT(nvarchar(10), ISNULL(p.CylinderDate, js.CylinderDate), 103) as CylinderDate, pc.CylinderStatusName, p.Note
		from tblJob j INNER JOIN
			tblJobSheet js ON j.JobID = js.JobID INNER JOIN
			tblCustomer c ON j.CustomerID = c.CustomerID LEFT OUTER JOIN
			tblCylinder cyl ON j.JobID = cyl.JobID LEFT OUTER JOIN
			tblCylinderStatus cs ON cyl.CylinderStatusID = cs.CylinderStatusID LEFT OUTER JOIN
			tblProgress p ON j.JobID = p.JobID LEFT OUTER JOIN
			tblProgressReproStatus pr ON p.ReproStatusID = pr.ReproStatusID LEFT OUTER JOIN
			tblProgressCylinderStatus pc ON p.CylinderStatusID = pc.CylinderStatusID
		where j.IsClosed = 0 and j.IsOutsource = 0
			and cs.Action = 'Repro'
			and (ISNULL(pr.GoBackToRepro, 0) = 0 and p.JobID IS NOT NULL and p.ReproStatusID IS NOT NULL)
			and (DATEDIFF(D, p.DeliveryDate, @DeliveryBegin) <= 0 or @DeliveryBegin is NULL)
				and (DATEDIFF(D, p.DeliveryDate, @DeliveryEnd) >= 0 or @DeliveryEnd is NULL)
			and (DATEDIFF(D, p.EngravingDate, @EngravingBegin) <= 0 or @EngravingBegin is NULL)
				and (DATEDIFF(D, p.EngravingDate, @EngravingEnd) >= 0 or @EngravingEnd is NULL)
			and (p.ReproStatusID = @ReproStatusID or @ReproStatusID = 0)
			and @SortType = 'A'
		group by j.JobID, c.Code, j.JobNumber, j.RevNumber, j.JobName, j.Design, j.CreatedOn, p.ReproDate, js.ReproDate, pr.ReproStatusName,
			p.CylinderDate, js.CylinderDate, pc.CylinderStatusName, p.Note, p.DeliveryDate, p.EngravingDate
			
	union all
	select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN  c.Code END DESC,
															CASE @SortColumn WHEN N'1' THEN j.JobNumber END DESC,
															CASE @SortColumn WHEN N'2' THEN j.RevNumber END DESC,
															CASE @SortColumn WHEN N'3' THEN j.JobName END DESC,
															CASE @SortColumn WHEN N'4' THEN j.Design END DESC,
															CASE @SortColumn WHEN N'5' THEN ISNULL(COUNT(cyl.CylinderID),0) END DESC,
															CASE @SortColumn WHEN N'6' THEN p.DeliveryDate END DESC,
															CASE @SortColumn WHEN N'7' THEN p.EngravingDate END DESC,
															CASE @SortColumn WHEN N'8' THEN ISNULL(p.ReproDate, js.ReproDate) END DESC,
															CASE @SortColumn WHEN N'9' THEN pr.ReproStatusName END DESC,
															CASE @SortColumn WHEN N'10' THEN ISNULL(p.CylinderDate, js.CylinderDate) END DESC,
															CASE @SortColumn WHEN N'11' THEN pc.CylinderStatusName END DESC, 
															CASE @SortColumn WHEN N'12' THEN p.Note END DESC) as RowIndex, 
			j.JobID, c.Code, j.JobNumber, j.RevNumber, j.JobName, j.Design, ISNULL(COUNT(cyl.CylinderID),0) as Qty,
			ISNULL(CONVERT(nvarchar(10), p.DeliveryDate, 103),'') as DeliveryDate,
			ISNULL(CONVERT(nvarchar(10), p.EngravingDate, 103),'') as EngravingDate,
			CONVERT(nvarchar(10), ISNULL(p.ReproDate, js.ReproDate), 103) as ReproDate, pr.ReproStatusName,
			CONVERT(nvarchar(10), ISNULL(p.CylinderDate, js.CylinderDate), 103) as CylinderDate, pc.CylinderStatusName, p.Note
		from tblJob j INNER JOIN
			tblJobSheet js ON j.JobID = js.JobID INNER JOIN
			tblCustomer c ON j.CustomerID = c.CustomerID LEFT OUTER JOIN
			tblCylinder cyl ON j.JobID = cyl.JobID LEFT OUTER JOIN
			tblCylinderStatus cs ON cyl.CylinderStatusID = cs.CylinderStatusID LEFT OUTER JOIN
			tblProgress p ON j.JobID = p.JobID LEFT OUTER JOIN
			tblProgressReproStatus pr ON p.ReproStatusID = pr.ReproStatusID LEFT OUTER JOIN
			tblProgressCylinderStatus pc ON p.CylinderStatusID = pc.CylinderStatusID
		where j.IsClosed = 0 and j.IsOutsource = 0
			and cs.Action = 'Repro'
			and (ISNULL(pr.GoBackToRepro, 0) = 0 and p.JobID IS NOT NULL and p.ReproStatusID IS NOT NULL)
			and (DATEDIFF(D, p.DeliveryDate, @DeliveryBegin) <= 0 or @DeliveryBegin is NULL)
				and (DATEDIFF(D, p.DeliveryDate, @DeliveryEnd) >= 0 or @DeliveryEnd is NULL)
			and (DATEDIFF(D, p.EngravingDate, @EngravingBegin) <= 0 or @EngravingBegin is NULL)
				and (DATEDIFF(D, p.EngravingDate, @EngravingEnd) >= 0 or @EngravingEnd is NULL)
			and (p.ReproStatusID = @ReproStatusID or @ReproStatusID = 0)
			and @SortType = 'D'
		group by j.JobID, c.Code, j.JobNumber, j.RevNumber, j.JobName, j.Design, j.CreatedOn, p.ReproDate, js.ReproDate, pr.ReproStatusName,
			p.CylinderDate, js.CylinderDate, pc.CylinderStatusName, p.Note, p.DeliveryDate, p.EngravingDate
	)
	select top (@PageSize)  *, (select COUNT(RowIndex) from T) as RowsCount
		from T
		where @PageSize <> 0 AND RowIndex > (@PageIndex*@PageSize)
	union all
	select *, (select COUNT(RowIndex) from T) as RowsCount
		from T
		where @PageSize = 0		
--exec tblJob_ProgressForEngraving null, null, null, null, 0, 0, 0, '0', 'A'
end

GO
/****** Object:  StoredProcedure [dbo].[tblJob_ProgressForDeReChrome]    Script Date: 04/07/2015 09:31:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblJob_ProgressForDeReChrome]
@DeliveryBegin datetime,
@DeliveryEnd datetime,
@DeReDateBegin datetime,
@DeReDateEnd datetime,
@CylinderDateBegin datetime,
@CylinderDateEnd datetime,
@PageIndex int,
@PageSize int,
@SortColumn varchar(2),
@SortType varchar(1)
as
begin
	;WITH T AS(
	select  TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN  c.Code END ASC,
															CASE @SortColumn WHEN N'1' THEN j.JobNumber END ASC,
															CASE @SortColumn WHEN N'2' THEN j.RevNumber END ASC,
															CASE @SortColumn WHEN N'3' THEN j.JobName END ASC,
															CASE @SortColumn WHEN N'4' THEN j.Design END ASC,
															CASE @SortColumn WHEN N'5' THEN ISNULL(COUNT(cyl.CylinderID),0) END ASC,
															CASE @SortColumn WHEN N'6' THEN p.DeliveryDate END ASC,
															CASE @SortColumn WHEN N'7' THEN p.DeReDate END ASC,
															CASE @SortColumn WHEN N'8' THEN ISNULL(p.CylinderDate, js.CylinderDate) END ASC,
															CASE @SortColumn WHEN N'9' THEN pc.CylinderStatusName END ASC, 
															CASE @SortColumn WHEN N'10' THEN p.Note END ASC) as RowIndex,
			j.JobID, c.Code, j.JobNumber, j.RevNumber, j.JobName, j.Design, ISNULL(COUNT(cyl.CylinderID),0) as Qty, 
			ISNULL(CONVERT(nvarchar(10), p.DeliveryDate, 103),'') as DeliveryDate,
			ISNULL(CONVERT(nvarchar(10), p.DeReDate, 103), '') as DeReChromeDate,
			CONVERT(nvarchar(10), ISNULL(p.CylinderDate, js.CylinderDate), 103) as CylinderDate, pc.CylinderStatusName, p.Note
		from tblJob j INNER JOIN
			tblJobSheet js ON j.JobID = js.JobID INNER JOIN
			tblCustomer c ON j.CustomerID = c.CustomerID LEFT OUTER JOIN
			tblCylinder cyl ON j.JobID = cyl.JobID LEFT OUTER JOIN
			tblCylinderStatus cs ON cyl.CylinderStatusID = cs.CylinderStatusID LEFT OUTER JOIN
			tblProgress p ON j.JobID = p.JobID LEFT OUTER JOIN
			tblProgressReproStatus pr ON p.ReproStatusID = pr.ReproStatusID LEFT OUTER JOIN
			tblProgressCylinderStatus pc ON p.CylinderStatusID = pc.CylinderStatusID
		where j.IsClosed = 0 and j.IsOutsource = 0
			and cs.Action = 'DeReChrome'
			and (DATEDIFF(D, p.DeliveryDate, @DeliveryBegin) <= 0 or @DeliveryBegin is NULL)
				and (DATEDIFF(D, p.DeliveryDate, @DeliveryEnd) >= 0 or @DeliveryEnd is NULL)
			and (DATEDIFF(D, p.DeReDate, @DeReDateBegin) <=0 or @DeReDateBegin IS NULL)
				and (DATEDIFF(D, p.DeReDate, @DeReDateEnd) >=0 or @DeReDateEnd IS NULL)
			and (DATEDIFF(D, ISNULL(p.CylinderDate, js.CylinderDate), @CylinderDateBegin) <=0 or @CylinderDateBegin IS NULL)
				and (DATEDIFF(D, ISNULL(p.CylinderDate, js.CylinderDate), @CylinderDateEnd) >=0 or @CylinderDateEnd IS NULL)
			and @SortType = 'A'
		group by j.JobID, c.Code, j.JobNumber, j.RevNumber, j.JobName, j.Design, j.CreatedOn, p.ReproDate, js.ReproDate, pr.ReproStatusName,
			p.CylinderDate, js.CylinderDate, pc.CylinderStatusName, p.Note, p.DeliveryDate, p.DeReDate
	union all
	select  TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN  c.Code END DESC,
															CASE @SortColumn WHEN N'1' THEN j.JobNumber END DESC,
															CASE @SortColumn WHEN N'2' THEN j.RevNumber END DESC,
															CASE @SortColumn WHEN N'3' THEN j.JobName END DESC,
															CASE @SortColumn WHEN N'4' THEN j.Design END DESC,
															CASE @SortColumn WHEN N'5' THEN ISNULL(COUNT(cyl.CylinderID),0) END DESC,
															CASE @SortColumn WHEN N'6' THEN p.DeliveryDate END DESC,
															CASE @SortColumn WHEN N'7' THEN p.DeReDate END DESC,
															CASE @SortColumn WHEN N'8' THEN ISNULL(p.CylinderDate, js.CylinderDate) END DESC,
															CASE @SortColumn WHEN N'9' THEN pc.CylinderStatusName END DESC, 
															CASE @SortColumn WHEN N'10' THEN p.Note END DESC) as RowIndex,
			j.JobID, c.Code, j.JobNumber, j.RevNumber, j.JobName, j.Design, ISNULL(COUNT(cyl.CylinderID),0) as Qty, 
			ISNULL(CONVERT(nvarchar(10), p.DeliveryDate, 103),'') as DeliveryDate,
			ISNULL(CONVERT(nvarchar(10), p.DeReDate, 103), '') as DeReChromeDate,
			CONVERT(nvarchar(10), ISNULL(p.CylinderDate, js.CylinderDate), 103) as CylinderDate, pc.CylinderStatusName, p.Note
		from tblJob j INNER JOIN
			tblJobSheet js ON j.JobID = js.JobID INNER JOIN
			tblCustomer c ON j.CustomerID = c.CustomerID LEFT OUTER JOIN
			tblCylinder cyl ON j.JobID = cyl.JobID LEFT OUTER JOIN
			tblCylinderStatus cs ON cyl.CylinderStatusID = cs.CylinderStatusID LEFT OUTER JOIN
			tblProgress p ON j.JobID = p.JobID LEFT OUTER JOIN
			tblProgressReproStatus pr ON p.ReproStatusID = pr.ReproStatusID LEFT OUTER JOIN
			tblProgressCylinderStatus pc ON p.CylinderStatusID = pc.CylinderStatusID
		where j.IsClosed = 0 and j.IsOutsource = 0
			and cs.Action = 'DeReChrome'
			and (DATEDIFF(D, p.DeliveryDate, @DeliveryBegin) <= 0 or @DeliveryBegin is NULL)
				and (DATEDIFF(D, p.DeliveryDate, @DeliveryEnd) >= 0 or @DeliveryEnd is NULL)
			and (DATEDIFF(D, p.DeReDate, @DeReDateBegin) <=0 or @DeReDateBegin IS NULL)
				and (DATEDIFF(D, p.DeReDate, @DeReDateEnd) >=0 or @DeReDateEnd IS NULL)
			and (DATEDIFF(D, ISNULL(p.CylinderDate, js.CylinderDate), @CylinderDateBegin) <=0 or @CylinderDateBegin IS NULL)
				and (DATEDIFF(D, ISNULL(p.CylinderDate, js.CylinderDate), @CylinderDateEnd) >=0 or @CylinderDateEnd IS NULL)
			and @SortType = 'D'
		group by j.JobID, c.Code, j.JobNumber, j.RevNumber, j.JobName, j.Design, j.CreatedOn, p.ReproDate, js.ReproDate, pr.ReproStatusName,
			p.CylinderDate, js.CylinderDate, pc.CylinderStatusName, p.Note, p.DeliveryDate, p.DeReDate
	)
	select top (@PageSize)  *, (select COUNT(RowIndex) from T) as RowsCount
		from T
		where @PageSize <> 0 AND RowIndex > (@PageIndex*@PageSize)
	union all
	select *, (select COUNT(RowIndex) from T) as RowsCount
		from T
		where @PageSize = 0
--exec tblJob_ProgressForDeReChrome null, null, null, null, null, null, 0, 0, '0', 'A'	
end				