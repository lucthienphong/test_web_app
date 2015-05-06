/****** Object:  Table [dbo].[tblObjectLocking]    Script Date: 04/25/2015 10:34:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tblObjectLocking](
	[ID] [int] NOT NULL,
	[Type] [varchar](50) NOT NULL,
	[Locking] [bit] NOT NULL,
 CONSTRAINT [PK_tblOrderLocking] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[Type] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'JOB - Job
OC - OrderConfirm
DO - DeliveryOrder
INVOICE - Invoice' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblObjectLocking', @level2type=N'COLUMN',@level2name=N'Locking'
GO

ALTER TABLE [dbo].[tblObjectLocking] ADD  CONSTRAINT [DF_tblObjectLocking_Locking]  DEFAULT ((0)) FOR [Locking]
GO

ALTER TABLE dbo.tblRolePermission ADD
	AllowLockUnlock bit NOT NULL CONSTRAINT DF_tblRolePermission_AllowLock DEFAULT 0
GO

ALTER TABLE dbo.tblRolePermission SET (LOCK_ESCALATION = TABLE)
GO

ALTER proc [dbo].[tblRolePermissions_SelectByRoleID] 
@RoleID int 
AS BEGIN ;

WITH T AS
  ( SELECT f.FunctionID AS FunctionID, 
		   f.ParentID, 
		   f.Title, 
		   f.DisplayOrder, 
		   CASE WHEN   rp.AllowAdd = 1
				   AND rp.AllowEdit = 1
				   AND rp.AllowDelete = 1
				   AND rp.AllowOther = 1
				   AND rp.AllowUpdateStatus = 1 
				   AND rp.AllowLockUnlock = 1
				THEN 
					CAST(1 AS bit) 
				ELSE CAST(0 AS bit) 
		   END AS CheckAll, 
		   ISNULL(rp.AllowAdd, 0) AS AllowAdd, 
		   ISNULL(rp.AllowEdit, 0) AS AllowEdit, 
		   ISNULL(rp.AllowDelete, 0) AS AllowDelete, 
		   ISNULL(rp.AllowUpdateStatus, 0) AS AllowUpdateStatus, 
		   ISNULL(rp.AllowOther, 0) AS AllowOther,
		   ISNULL(rp.AllowLockUnlock, 0) AS AllowLockUnlock
   FROM tblFunction f
   INNER JOIN tblRolePermission rp ON f.FunctionID = rp.FunctionID
   WHERE rp.RoleID = @RoleID
     OR rp.RoleID IS NULL
   UNION ALL 
   SELECT f.FunctionID AS FunctionID, 
          f.ParentID, 
          f.Title, 
          f.DisplayOrder, 
          CAST(0 AS bit) AS CheckAll, 
          CAST(0 AS bit) AS AllowAdd, 
          CAST(0 AS bit) AS AllowEdit, 
          CAST(0 AS bit) AS AllowDelete, 
          CAST(0 AS bit) AS AllowViewPrice, 
          CAST(0 AS bit) AS AllowOther,
          CAST(0 AS bit) AS AllowLockUnlock
   FROM tblFunction f
   WHERE f.FunctionID NOT IN
       (SELECT FunctionID
        FROM tblRolePermission rp
        WHERE rp.RoleID = @RoleID) )
SELECT *
FROM T
ORDER BY DisplayOrder END 

--exec tblRolePermissions_SelectByRoleID 1

Go

ALTER proc [dbo].[SearchDeliveryOrderByCustomer]
@Customer nvarchar(200),
@DONumber nvarchar(50),
@JobNumber nvarchar(50),
@FromDate datetime,
@ToDate datetime,
@PageIndex int,
@PageSize int,
@SortColumn varchar(2),
@SortType varchar(1)
AS
BEGIN
	;WITH T AS(
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY CASE @SortColumn WHEN N'0' THEN  d.DONumber END ASC,
															CASE @SortColumn WHEN N'1' THEN d.OrderDate END ASC,
															CASE @SortColumn WHEN N'2' THEN c.Code END ASC,
															CASE @SortColumn WHEN N'3' THEN c.Name END ASC,
															CASE @SortColumn WHEN N'4' THEN j.JobNumber END ASC,
															CASE @SortColumn WHEN N'5' THEN j.JobName END ASC) as RowIndex, 
				d.JobID, d.DONumber, CONVERT(nvarchar(10), d.OrderDate, 103) as OrderDate,
				c.Name, c.Code, j.JobName, j.JobNumber, ol.Locking AS Lock
			from tblJob j inner join
				tblCustomer c on c.CustomerID = j.CustomerID inner join
				tblDeliveryOrder d on j.JobID = d.JobID left outer join
				tblObjectLocking ol on j.JobID = ol.ID and ol.[Type] = N'DO'
			where (c.Code like '%' + @Customer + '%' or c.Name like '%' + @Customer + '%')
				and (j.JobNumber like '%' + @JobNumber + '%')
				and (d.DONumber like '%' + @DONumber + '%')
				and (DATEDIFF(D, d.OrderDate, @FromDate) <=0 or @FromDate IS NULL)
				and (DATEDIFF(D, d.OrderDate, @ToDate) >=0 or @ToDate IS NULL)
				and	@SortType = 'A'
		union all
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY CASE @SortColumn WHEN N'0' THEN  d.DONumber END DESC,
															CASE @SortColumn WHEN N'1' THEN d.OrderDate END DESC,
															CASE @SortColumn WHEN N'2' THEN c.Code END DESC,
															CASE @SortColumn WHEN N'3' THEN c.Name END DESC,
															CASE @SortColumn WHEN N'4' THEN j.JobNumber END DESC,
															CASE @SortColumn WHEN N'5' THEN j.JobName END DESC) as RowIndex, 
				d.JobID, d.DONumber, CONVERT(nvarchar(10), d.OrderDate, 103) as OrderDate,
				c.Name, c.Code, j.JobName, j.JobNumber, ol.Locking AS Lock
			from tblJob j inner join
				tblCustomer c on c.CustomerID = j.CustomerID inner join
				tblDeliveryOrder d on j.JobID = d.JobID left outer join
				tblObjectLocking ol on j.JobID = ol.ID and ol.[Type] = N'DO'
			where (c.Code like '%' + @Customer + '%' or c.Name like '%' + @Customer + '%')
				and (j.JobNumber like '%' + @JobNumber + '%')
				and (d.DONumber like '%' + @DONumber + '%')
				and (DATEDIFF(D, d.OrderDate, @FromDate) <=0 or @FromDate IS NULL)
				and (DATEDIFF(D, d.OrderDate, @ToDate) >=0 or @ToDate IS NULL)
				and	@SortType = 'D'				
		)
		select top (@PageSize) *, (select COUNT(RowIndex) from T) as RowsCount
		from T
		where RowIndex > (@PageIndex*@PageSize)
	
END
--exec [SearchDeliveryOrderByCustomer] '', '','', null, null, 0, 10, '1', 'A'

GO

ALTER proc [dbo].[TblInvoice_SelectAll]
@Customer nvarchar(200),
@InvoiceNo nvarchar(100),
@Job nvarchar(100),
@FromDate datetime,
@ToDate datetime,
@PageIndex int,
@PageSize int,
@SortColumn varchar(2),
@SortType varchar(1)
AS
BEGIN
 ;WITH T AS(
	select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY CASE @SortColumn WHEN N'0' THEN  i.InvoiceNo END ASC,
													   CASE @SortColumn WHEN N'1' THEN  i.InvoiceDate END ASC,
													   CASE @SortColumn WHEN N'2' THEN  c.Code END ASC,
													   CASE @SortColumn WHEN N'3' THEN  c.Name END ASC,
													   CASE @SortColumn WHEN N'4' THEN  j.JobNumber END ASC,
													   CASE @SortColumn WHEN N'5' THEN  j.JobName END ASC) as RowIndex,
               i.InvoiceID, i.InvoiceNo, CONVERT(nvarchar(10), i.InvoiceDate, 103) as InvoiceDate, 
               c.Code, c.Name, j.JobNumber, j.JobName, ol.Locking AS Lock
		from tblInvoice i INNER JOIN
			tblCustomer c ON i.CustomerID = c.CustomerID LEFT OUTER JOIN
			tblInvoiceDetail jd ON i.InvoiceID = jd.InvoiceID LEFT OUTER JOIN
			tblJob j ON jd.JobID = j.JobID LEFT OUTER JOIN
			tblObjectLocking ol ON ol.ID = i.InvoiceID AND ol.[Type] = N'INVOICE' 
		where (c.Code like '%' + @Customer + '%' or c.Name like '%' + @Customer + '%')
			and (i.InvoiceNo like '%' + @InvoiceNo + '%')
			and (ISNULL(j.JobNumber,'') like '%' + @Job + '%')    
			and (DATEDIFF(D, i.InvoiceDate, @FromDate) <=0 or @FromDate IS NULL)
			and (DATEDIFF(D, i.InvoiceDate, @ToDate) >=0 or @ToDate IS NULL)
		    and @SortType = 'A'
		group by i.InvoiceNo, i.InvoiceDate, i.InvoiceID, c.Code, c.Name, j.JobNumber, j.JobName, ol.Locking
	union all
	select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY CASE @SortColumn WHEN N'0' THEN  i.InvoiceNo END DESC,
													   CASE @SortColumn WHEN N'1' THEN  i.InvoiceDate END DESC,
													   CASE @SortColumn WHEN N'2' THEN  c.Code END DESC,
													   CASE @SortColumn WHEN N'3' THEN  c.Name END DESC,
													   CASE @SortColumn WHEN N'4' THEN  j.JobNumber END DESC,
													   CASE @SortColumn WHEN N'5' THEN  j.JobName END DESC) as RowIndex,
               i.InvoiceID, i.InvoiceNo, CONVERT(nvarchar(10), i.InvoiceDate, 103) as InvoiceDate, 
               c.Code, c.Name, j.JobNumber, j.JobName, ol.Locking AS Lock
		from tblInvoice i INNER JOIN
			tblCustomer c ON i.CustomerID = c.CustomerID LEFT OUTER JOIN
			tblInvoiceDetail jd ON i.InvoiceID = jd.InvoiceID LEFT OUTER JOIN
			tblJob j ON jd.JobID = j.JobID LEFT OUTER JOIN
			tblObjectLocking ol ON ol.ID = i.InvoiceID AND ol.[Type] = N'INVOICE' 
		where (c.Code like '%' + @Customer + '%' or c.Name like '%' + @Customer + '%')
			and (i.InvoiceNo like '%' + @InvoiceNo + '%')
			and (ISNULL(j.JobNumber,'') like '%' + @Job + '%')    
			and (DATEDIFF(D, i.InvoiceDate, @FromDate) <=0 or @FromDate IS NULL)
			and (DATEDIFF(D, i.InvoiceDate, @ToDate) >=0 or @ToDate IS NULL)
		    and @SortType = 'D'
		group by i.InvoiceNo, i.InvoiceDate, i.InvoiceID, c.Code, c.Name, j.JobNumber, j.JobName, ol.Locking    
  )
  select top (@PageSize) *, (select COUNT(RowIndex) from T) as RowsCount
  from T
  where RowIndex > (@PageIndex*@PageSize)
END
--exec [TblInvoice_SelectAll] '', '', '', null, null, 0, 10, '1', 'D'

GO

ALTER proc [dbo].[tblJob_SelectAll]
@Customer nvarchar(200),
@JobBarcode nvarchar(255),
@JobNumber nvarchar(50),
@JobInfo nvarchar(200),
@CusCylID nvarchar(50),
@SaleRepID int,
@FromDate datetime,
@ToDate datetime,
@HasOC int,-- 0:All; 1:Not yet; 2:Completed
@HasDO int,-- 0:All; 1:Not yet; 2:Completed
@HasInvoice int,-- 0:All; 1:Not yet; 2:Completed
@IsServiceJob bit,
@PageIndex int,
@PageSize int,
@SortColumn varchar(2),
@SortType varchar(1)
AS
BEGIN
	;WITH T AS(
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN  c.Code END ASC,
															--CASE @SortColumn WHEN N'1' THEN c.Name END ASC,
															--CASE @SortColumn WHEN N'1' THEN j.JobBarcode END ASC,
															CASE @SortColumn WHEN N'1' THEN j.JobNumber END ASC,
															CASE @SortColumn WHEN N'1' THEN j.RevNumber END ASC,
															CASE @SortColumn WHEN N'2' THEN j.JobNumber END ASC,
															CASE @SortColumn WHEN N'2' THEN j.RevNumber END ASC,
															CASE @SortColumn WHEN N'3' THEN j.JobName END ASC,
															CASE @SortColumn WHEN N'4' THEN j.Design END ASC,
															CASE @SortColumn WHEN N'5' THEN j.CreatedOn END ASC) as RowIndex, 
				j.JobID, c.Code, c.Name, j.JobBarcode, j.JobNumber, j.RevNumber, j.JobName, j.Design, 
				CONVERT(nvarchar(10), j.CreatedOn, 103) as CreatedOn, 
				s.FirstName + ' ' + s.LastName as SaleDep, ol.Locking AS Lock
			from tblJob j INNER JOIN
				tblCustomer c ON j.CustomerID = c.CustomerID INNER JOIN
				tblStaff s ON j.SalesRepID = s.StaffID LEFT OUTER JOIN
				tblCylinder cyl ON j.JobID = cyl.JobID LEFT OUTER JOIN
				tblOrderConfirmation oc ON j.JobID = oc.JobID LEFT OUTER JOIN
				tblDeliveryOrder do ON j.JobID = do.JobID LEFT OUTER JOIN
				tblInvoiceDetail id ON j.JobID = id.JobID LEFT OUTER JOIN
				tblObjectLocking ol ON j.JobID = ol.ID AND ol.Type = N'JOB'
			where (c.Name like '%' + @Customer + '%' or c.Code like '%' + @Customer + '%')
				and j.JobBarcode like '%' + @JobBarcode + '%'
				and j.JobNumber like '%' + @JobNumber + '%'
				and (j.JobName like '%' + @JobInfo + '%' or j.Design like '%' + @JobInfo + '%' or j.DrawingNumber like '%' + @JobInfo + '%')
				and (j.SalesRepID = @SaleRepID or @SaleRepID = 0)
				and (DATEDIFF(D, j.CreatedOn, @FromDate) <=0 or @FromDate IS NULL)
				and (DATEDIFF(D, j.CreatedOn, @ToDate) >=0 or @ToDate IS NULL)
				and (@HasOC = 0 or (@HasOC = 1 and oc.JobID IS NULL) or (@HasOC = 2 and oc.JobID IS NOT NULL))
				and (@HasDO = 0 or (@HasDO = 1 and do.JobID IS NULL) or (@HasDO = 2 and do.JobID IS NOT NULL))
				and (@HasInvoice = 0 or (@HasInvoice = 1 and id.JobID IS NULL) or (@HasInvoice = 2 and id.JobID IS NOT NULL))
				and j.IsServiceJob =@IsServiceJob
				and ISNULL(cyl.CusCylinderID, '') like '%' + @CusCylID + '%'
				--and j.IsClosed = 0
				and @SortType = 'A'
		group by j.JobID, c.Code, c.Name, j.JobBarcode, j.JobNumber, j.JobName, j.Design, j.RevNumber, 
				j.CreatedOn, s.FirstName, s.LastName, ol.Locking
		union all
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN  c.Code END DESC,
															--CASE @SortColumn WHEN N'1' THEN c.Name END DESC,
															--CASE @SortColumn WHEN N'1' THEN j.JobBarcode END DESC,
															CASE @SortColumn WHEN N'1' THEN j.JobNumber END DESC,
															CASE @SortColumn WHEN N'1' THEN j.RevNumber END DESC,
															CASE @SortColumn WHEN N'2' THEN j.JobNumber END DESC,
															CASE @SortColumn WHEN N'2' THEN j.RevNumber END DESC,
															CASE @SortColumn WHEN N'3' THEN j.JobName END DESC,
															CASE @SortColumn WHEN N'4' THEN j.Design END DESC,
															CASE @SortColumn WHEN N'5' THEN j.CreatedOn END DESC) as RowIndex, 
				j.JobID, c.Code, c.Name, j.JobBarcode, j.JobNumber, j.RevNumber, j.JobName, j.Design, 
				CONVERT(nvarchar(10), j.CreatedOn, 103) as CreatedOn, 
				s.FirstName + ' ' + s.LastName as SaleDep, ol.Locking AS Lock
			from tblJob j INNER JOIN
				tblCustomer c ON j.CustomerID = c.CustomerID INNER JOIN
				tblStaff s ON j.SalesRepID = s.StaffID LEFT OUTER JOIN
				tblCylinder cyl ON j.JobID = cyl.JobID LEFT OUTER JOIN
				tblOrderConfirmation oc ON j.JobID = oc.JobID LEFT OUTER JOIN
				tblDeliveryOrder do ON j.JobID = do.JobID LEFT OUTER JOIN
				tblInvoiceDetail id ON j.JobID = id.JobID LEFT OUTER JOIN
				tblObjectLocking ol ON j.JobID = ol.ID AND ol.Type = N'JOB'
			where (c.Name like '%' + @Customer + '%' or c.Code like '%' + @Customer + '%')
				and j.JobBarcode like '%' + @JobBarcode + '%'
				and j.JobNumber like '%' + @JobNumber + '%'
				and (j.JobName like '%' + @JobInfo + '%' or j.Design like '%' + @JobInfo + '%' or j.DrawingNumber like '%' + @JobInfo + '%')
				and (j.SalesRepID = @SaleRepID or @SaleRepID = 0)
				and (DATEDIFF(D, j.CreatedOn, @FromDate) <=0 or @FromDate IS NULL)
				and (DATEDIFF(D, j.CreatedOn, @ToDate) >=0 or @ToDate IS NULL)
				and (@HasOC = 0 or (@HasOC = 1 and oc.JobID IS NULL) or (@HasOC = 2 and oc.JobID IS NOT NULL))
				and (@HasDO = 0 or (@HasDO = 1 and do.JobID IS NULL) or (@HasDO = 2 and do.JobID IS NOT NULL))
				and (@HasInvoice = 0 or (@HasInvoice = 1 and id.JobID IS NULL) or (@HasInvoice = 2 and id.JobID IS NOT NULL))
				and j.IsServiceJob =@IsServiceJob
				and ISNULL(cyl.CusCylinderID, '') like '%' + @CusCylID + '%'
				--and j.IsClosed = 0
				and @SortType = 'D'
		group by j.JobID, c.Code, c.Name, j.JobBarcode, j.JobNumber, j.JobName, j.Design, j.RevNumber, 
				j.CreatedOn, s.FirstName, s.LastName, ol.Locking
		)
		select top (@PageSize) *, (select COUNT(RowIndex) from T) as RowsCount
			from T
			where RowIndex > (@PageIndex*@PageSize)
	--exec tblJob_SelectAll '', '', '', '', '', 0, null, null, 0, 0, 0, 0, 0, 10, '0', 'A'
END

GO

ALTER proc [dbo].[TblConfirmOrder_SelectAll]
@Customer nvarchar(200),
@Job nvarchar(100),
@OCNumber nvarchar(100),
@FromDate datetime,
@ToDate datetime,
@PageIndex int,
@PageSize int,
@SortColumn varchar(2),
@SortType varchar(1)
AS
BEGIN
	;WITH T AS(
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY CASE @SortColumn WHEN N'0' THEN  o.OCNumber END ASC,
															CASE @SortColumn WHEN N'1' THEN  o.OrderDate END ASC,
															CASE @SortColumn WHEN N'2' THEN  c.Code END ASC,
															CASE @SortColumn WHEN N'3' THEN  c.Name END ASC,
															CASE @SortColumn WHEN N'4' THEN  j.JobNumber END ASC,
															CASE @SortColumn WHEN N'5' THEN  j.JobName END ASC) as RowIndex,
			o.JobID, o.OCNumber, CONVERT(nvarchar(10), o.OrderDate, 103) as OrderDate, 
			c.Code, c.Name, j.JobNumber, j.JobName, ol.Locking AS Lock
		from  tblOrderConfirmation o INNER JOIN
				tblJob j ON o.JobID = j.JobID INNER JOIN 
				tblCustomer c ON j.CustomerID = c.CustomerID LEFT OUTER JOIN
				tblObjectLocking ol ON ol.ID = j.JobID AND ol.[Type] = N'OC'
		where (c.Name like '%' + @Customer + '%' or c.Code like '%' + @Customer + '%')
				and (j.JobNumber like '%' + @Job + '%')
				and (o.OCNumber like '%' + @OCNumber + '%')
				and (DATEDIFF(D, o.OrderDate, @FromDate) <= 0 or @FromDate IS NULL)
				and (DATEDIFF(D, o.OrderDate, @ToDate) >= 0 or @ToDate IS NULL)
				and @SortType = 'A'
		union all
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY CASE @SortColumn WHEN N'0' THEN  o.OCNumber END DESC,
															CASE @SortColumn WHEN N'1' THEN  o.OrderDate END DESC,
															CASE @SortColumn WHEN N'2' THEN  c.Code END DESC,
															CASE @SortColumn WHEN N'3' THEN  c.Name END DESC,
															CASE @SortColumn WHEN N'4' THEN  j.JobNumber END DESC,
															CASE @SortColumn WHEN N'5' THEN  j.JobName END DESC) as RowIndex,
			o.JobID, o.OCNumber, CONVERT(nvarchar(10), o.OrderDate, 103) as OrderDate, 
			c.Code, c.Name, j.JobNumber, j.JobName, ol.Locking AS Lock
		from  tblOrderConfirmation o INNER JOIN
				tblJob j ON o.JobID = j.JobID INNER JOIN 
				tblCustomer c ON j.CustomerID = c.CustomerID LEFT OUTER JOIN
				tblObjectLocking ol ON ol.ID = j.JobID AND ol.[Type] = N'OC'
		where (c.Name like '%' + @Customer + '%' or c.Code like '%' + @Customer + '%')
				and (j.JobNumber like '%' + @Job + '%')
				and (o.OCNumber like '%' + @OCNumber + '%')
				and (DATEDIFF(D, o.OrderDate, @FromDate) <= 0 or @FromDate IS NULL)
				and (DATEDIFF(D, o.OrderDate, @ToDate) >= 0 or @ToDate IS NULL)
				and @SortType = 'D'				
		)
		select top (@PageSize) *, (select COUNT(RowIndex) from T) as RowsCount
		from T
		where RowIndex > (@PageIndex*@PageSize)
	
END
--exec [TblConfirmOrder_SelectAll] '', '', '', null, null, 0, 10, '0', 'A'

GO
