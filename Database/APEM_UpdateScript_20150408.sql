
GO
/****** Object:  StoredProcedure [dbo].[Report_RemakeReport]    Script Date: 04/08/2015 21:26:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[Report_RemakeReport]
@FromDate datetime,
@ToDate datetime
as
begin
	select ROW_NUMBER() OVER(ORDER BY j.JobID) as RowNumber,
			CONVERT(nvarchar(10), j.CreatedOn, 103) as Date, LEFT(s.FirstName, 2) as S, LEFT(coor.FirstName, 2) as C, 
			j.JobNumber + ' R ' + CAST(j.RevNumber as varchar(5)) as JobNumber, c.Code, j.JobName, j.Design,  		
			LEFT(j.InternalExternal,1) as IE, ISNULL(CBS.Qty, 0) as CByS, ISNULL(CBR.Qty, 0) CByR, 
			ISNULL(CBP.Qty, 0) as CByP, ISNULL(CBO.Qty, 0) as CByO, j.RevisionDetail
		from tblJob j INNER JOIN
			tblCustomer c ON j.CustomerID = c.CustomerID LEFT OUTER JOIN
			tblStaff s ON j.SalesRepID = s.StaffID LEFT OUTER JOIN
			tblStaff coor ON j.CoordinatorID = coor.StaffID LEFT OUTER JOIN
			(select sc.JobID, ISNULL(COUNT(sc.CylinderID),0) as Qty 
				from tblJob sj INNER JOIN
						tblCylinder sc ON sj.JobID = sc.JobID
				where sc.Dept = 'S'
						and (DATEDIFF(D, sj.CreatedOn, @FromDate) <= 0 or @FromDate is NULL) 
						and (DATEDIFF(D, sj.CreatedOn, @ToDate) >= 0 or @ToDate is NULL) 
						and sj.RevNumber != 0
				group by sc.JobID) as CBS ON j.JobID = CBS.JobID  LEFT OUTER JOIN
			(select sc.JobID, ISNULL(COUNT(sc.CylinderID),0) as Qty 
				from tblJob sj INNER JOIN
						tblCylinder sc ON sj.JobID = sc.JobID
				where sc.Dept = 'R'
						and (DATEDIFF(D, sj.CreatedOn, @FromDate) <= 0 or @FromDate is NULL) 
						and (DATEDIFF(D, sj.CreatedOn, @ToDate) >= 0 or @ToDate is NULL) 
						and sj.RevNumber != 0
				group by sc.JobID) as CBR ON j.JobID = CBR.JobID LEFT OUTER JOIN
			(select sc.JobID, ISNULL(COUNT(sc.CylinderID),0) as Qty 
				from tblJob sj INNER JOIN
						tblCylinder sc ON sj.JobID = sc.JobID
				where sc.Dept = 'P'
						and (DATEDIFF(D, sj.CreatedOn, @FromDate) <= 0 or @FromDate is NULL) 
						and (DATEDIFF(D, sj.CreatedOn, @ToDate) >= 0 or @ToDate is NULL) 
						and sj.RevNumber != 0
				group by sc.JobID) as CBP ON j.JobID = CBP.JobID LEFT OUTER JOIN
			(select sc.JobID, ISNULL(COUNT(sc.CylinderID),0) as Qty 
				from tblJob sj INNER JOIN
						tblCylinder sc ON sj.JobID = sc.JobID
				where sc.Dept  not in ('S', 'R', 'P') and LEN(sc.Dept) != 0
						and (DATEDIFF(D, sj.CreatedOn, @FromDate) <= 0 or @FromDate is NULL) 
						and (DATEDIFF(D, sj.CreatedOn, @ToDate) >= 0 or @ToDate is NULL) 
						and sj.RevNumber != 0
				group by sc.JobID) as CBO ON j.JobID = CBO.JobID
		where (DATEDIFF(D, j.CreatedOn, @FromDate) <= 0 or @FromDate is NULL) 
				and (DATEDIFF(D, j.CreatedOn, @ToDate) >= 0 or @ToDate is NULL) 
				and j.RevNumber != 0
		order by j.CreatedOn
--exec Report_RemakeReport null, null
end

GO
if not exists(select top 1 * from tblFunction where FunctionID = 'remake_report_manager')
begin
	update tblFunction set DisplayOrder = DisplayOrder + 1 where DisplayOrder > 20
	insert into tblFunction values('remake_report_manager', 'fProgress', 'Remake Report Manager', '', 21)
end