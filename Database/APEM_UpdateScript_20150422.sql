/****** Object:  StoredProcedure [dbo].[tblRolePermissions_SelectByRoleIDVersion2]    Script Date: 04/24/2015 15:30:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[tblRolePermissions_SelectByRoleIDVersion2] 
	-- Add the parameters for the stored procedure here
	@RoleID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

DECLARE @ParentID varchar(128) 
DECLARE @temptable TABLE ( 
			FunctionID varchar(128),
			ParentID varchar(128),
            Title nvarchar(250),
            Description nvarchar(500),
            DisplayOrder int) 
DECLARE MY_CURSOR
CURSOR LOCAL STATIC READ_ONLY FORWARD_ONLY
FOR		SELECT f.FunctionID 
		FROM tblFunction AS f
		WHERE LEN(f.ParentID) <= 0 OR f.ParentID IS NULL

OPEN MY_CURSOR FETCH NEXT
  FROM MY_CURSOR INTO @ParentID 
  
WHILE @@FETCH_STATUS = 0 BEGIN 

  INSERT INTO @temptable
  SELECT *
  FROM
    ( 
		SELECT * FROM tblFunction as a WHERE a.FunctionID = @ParentID
		UNION ALL 
		SELECT * FROM tblFunction as b WHERE b.ParentID = @ParentID 
	) AS Result
	
   FETCH NEXT FROM MY_CURSOR INTO @ParentID 
  
END
CLOSE MY_CURSOR 
DEALLOCATE MY_CURSOR

SELECT f.FunctionID AS FunctionID,
       f.ParentID,
       f.Title,
       f.DisplayOrder,
       CASE
           WHEN
			   (SELECT COUNT(*) 
			   FROM tblRolePermission_LockLogic
			   WHERE FunctionID = f.FunctionID
			   ) = 1
           THEN CASE WHEN
                  (SELECT COUNT(*)
                   FROM tblRolePermission AS rp
                   WHERE rp.FunctionID = f.FunctionID
                     AND rp.RoleID = @RoleID
                     AND rp.AllowAdd = 1
                     AND rp.AllowEdit = 1
                     AND rp.AllowDelete = 1
                     AND rp.AllowOther = 1
                     AND rp.AllowUpdateStatus = 1
                     AND rp.AllowLock = 1
                     AND rp.AllowUnlock = 1) = 1 
			   THEN CAST(1 AS bit)
			   ELSE CAST(0 AS bit)
			   END
			ELSE 
				CASE WHEN
				(SELECT COUNT(*)
                   FROM tblRolePermission AS rp
                   WHERE rp.FunctionID = f.FunctionID
                     AND rp.RoleID = @RoleID
                     AND rp.AllowAdd = 1
                     AND rp.AllowEdit = 1
                     AND rp.AllowDelete = 1
                     AND rp.AllowOther = 1
                     AND rp.AllowUpdateStatus = 1) = 1 
			   THEN CAST(1 AS bit)
			   ELSE CAST(0 AS bit)
			   END
       END AS CheckAll,
       CASE
           WHEN
                  (SELECT COUNT(*)
                   FROM tblRolePermission AS rp
                   WHERE rp.FunctionID = f.FunctionID
                     AND rp.RoleID = @RoleID
                     AND rp.AllowAdd = 1 ) = 1 
           THEN CAST(1 AS bit)
           ELSE CAST(0 AS bit)
       END AS AllowAdd,
       CASE
           WHEN
                  (SELECT COUNT(*)
                   FROM tblRolePermission AS rp
                   WHERE rp.FunctionID = f.FunctionID
                     AND rp.RoleID = @RoleID
                     AND rp.AllowEdit = 1 ) = 1 
           THEN CAST(1 AS bit)
           ELSE CAST(0 AS bit)
       END AS AllowEdit,
       CASE
           WHEN
                  (SELECT COUNT(*)
                   FROM tblRolePermission AS rp
                   WHERE rp.FunctionID = f.FunctionID
                     AND rp.RoleID = @RoleID
                     AND rp.AllowDelete = 1 ) = 1 
           THEN CAST(1 AS bit)
           ELSE CAST(0 AS bit)
       END AS AllowDelete,
       CASE
           WHEN
                  (SELECT COUNT(*)
                   FROM tblRolePermission AS rp
                   WHERE rp.FunctionID = f.FunctionID
                     AND rp.RoleID = @RoleID
                     AND rp.AllowOther = 1 ) = 1 
           THEN CAST(1 AS bit)
           ELSE CAST(0 AS bit)
       END AS AllowOther,
       CASE
           WHEN
                  (SELECT COUNT(*)
                   FROM tblRolePermission AS rp
                   WHERE rp.FunctionID = f.FunctionID
                     AND rp.RoleID = @RoleID
                     AND rp.AllowUpdateStatus = 1 ) = 1 
           THEN CAST(1 AS bit)
           ELSE CAST(0 AS bit)
       END AS AllowUpdateStatus,
       CASE
           WHEN
			   (SELECT COUNT(*) 
			   FROM tblRolePermission_LockLogic
			   WHERE FunctionID = f.FunctionID
			   ) = 1
           THEN CASE WHEN
                  (SELECT COUNT(*)
                   FROM tblRolePermission AS rp
                   WHERE rp.FunctionID = f.FunctionID
                     AND rp.RoleID = @RoleID
                     AND rp.AllowLock = 1 ) = 1 
			   THEN CAST(1 AS bit)
			   ELSE CAST(0 AS bit) 
			   END
           ELSE NULL
       END AS AllowLock,
       CASE
           WHEN
			   (SELECT COUNT(*) 
			   FROM tblRolePermission_LockLogic
			   WHERE FunctionID = f.FunctionID
			   ) = 1
           THEN CASE WHEN
                  (SELECT COUNT(*)
                   FROM tblRolePermission AS rp
                   WHERE rp.FunctionID = f.FunctionID
                     AND rp.RoleID = @RoleID
                     AND rp.AllowUnlock = 1 ) = 1 
			   THEN CAST(1 AS bit)
			   ELSE CAST(0 AS bit) 
			   END
           ELSE NULL
       END AS AllowUnlock
FROM @temptable f
END

--exec [tblRolePermissions_SelectByRoleIDVersion2] 13

GO


/*Create table tblRolePermission_LockLogic*/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tblRolePermission_LockLogic](
	[FunctionID] [varchar](128) NOT NULL,
 CONSTRAINT [PK_tblRolePermission_LockLogic] PRIMARY KEY CLUSTERED 
(
	[FunctionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[tblRolePermission_LockLogic] ([FunctionID]) VALUES (N'delivery_order_manager')
INSERT [dbo].[tblRolePermission_LockLogic] ([FunctionID]) VALUES (N'invoice_manager')
INSERT [dbo].[tblRolePermission_LockLogic] ([FunctionID]) VALUES (N'job_manager')
INSERT [dbo].[tblRolePermission_LockLogic] ([FunctionID]) VALUES (N'order_confirmation')

/*Create table tblJobLockStatus*/

CREATE TABLE [dbo].[tblJobLockStatus](
	[JobID] [int] NOT NULL,
	[Lock] [bit] NOT NULL,
 CONSTRAINT [PK_tblJobLockStatus] PRIMARY KEY CLUSTERED 
(
	[JobID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/*Create table tblOrderLockStatus*/

CREATE TABLE [dbo].[tblOrderLockStatus](
	[JobID] [int] NOT NULL,
	[Lock] [bit] NOT NULL,
 CONSTRAINT [PK_tblOrderLockStatus] PRIMARY KEY CLUSTERED 
(
	[JobID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/*Create table tblDeliveryOrderLockStatus*/

CREATE TABLE [dbo].[tblDeliveryOrderLockStatus](
	[JobID] [int] NOT NULL,
	[Lock] [bit] NOT NULL,
 CONSTRAINT [PK_tblDeliveryOrderLockStatus] PRIMARY KEY CLUSTERED 
(
	[JobID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/*Create table tblInvoiceLockStatus*/

CREATE TABLE [dbo].[tblInvoiceLockStatus](
	[InvoiceID] [int] NOT NULL,
	[Lock] [bit] NOT NULL,
 CONSTRAINT [PK_tblInvoiceLockStatus] PRIMARY KEY CLUSTERED 
(
	[InvoiceID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/*Alter Table TblRolePermission*/

ALTER TABLE dbo.tblRolePermission ADD
	AllowLock bit NOT NULL CONSTRAINT DF_tblRolePermission_AllowLock DEFAULT 0,
	AllowUnlock bit NOT NULL CONSTRAINT DF_tblRolePermission_AllowUnlock DEFAULT 0
GO
ALTER TABLE dbo.tblRolePermission SET (LOCK_ESCALATION = TABLE)
GO

SET ANSI_PADDING OFF
GO

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
				c.Name, c.Code, j.JobName, j.JobNumber,
				CASE WHEN 
						(SELECT COUNT(*) FROM tblDeliveryOrderLockStatus as jls WHERE jls.JobID = d.JobID AND jls.Lock = 1) > 0
					THEN CAST(1 AS bit)
					ELSE 
						CASE WHEN 
							(SELECT COUNT(*) FROM tblDeliveryOrderLockStatus as jls WHERE jls.JobID = d.JobID AND jls.Lock = 0) > 0
						THEN CAST(0 AS bit)
						ELSE NULL
						END
				END AS Lock
			from tblJob j inner join
				tblCustomer c on c.CustomerID = j.CustomerID inner join
				tblDeliveryOrder d on j.JobID = d.JobID
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
				c.Name, c.Code, j.JobName, j.JobNumber,
				CASE WHEN 
						(SELECT COUNT(*) FROM tblDeliveryOrderLockStatus as jls WHERE jls.JobID = d.JobID AND jls.Lock = 1) > 0
					THEN CAST(1 AS bit)
					ELSE 
						CASE WHEN 
							(SELECT COUNT(*) FROM tblDeliveryOrderLockStatus as jls WHERE jls.JobID = d.JobID AND jls.Lock = 0) > 0
						THEN CAST(0 AS bit)
						ELSE NULL
						END
				END AS Lock
			from tblJob j inner join
				tblCustomer c on c.CustomerID = j.CustomerID inner join
				tblDeliveryOrder d on j.JobID = d.JobID
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
               c.Code, c.Name, j.JobNumber, j.JobName,
               CASE WHEN 
						(SELECT COUNT(*) FROM tblInvoiceLockStatus as jls WHERE jls.InvoiceID = i.InvoiceID AND jls.Lock = 1) > 0
					THEN CAST(1 AS bit)
					ELSE 
						CASE WHEN 
							(SELECT COUNT(*) FROM tblInvoiceLockStatus as jls WHERE jls.InvoiceID = i.InvoiceID AND jls.Lock = 0) > 0
						THEN CAST(0 AS bit)
						ELSE NULL
						END
				END AS Lock
		from tblInvoice i INNER JOIN
			tblCustomer c ON i.CustomerID = c.CustomerID LEFT OUTER JOIN
			tblInvoiceDetail jd ON i.InvoiceID = jd.InvoiceID LEFT OUTER JOIN
			tblJob j ON jd.JobID = j.JobID
		where (c.Code like '%' + @Customer + '%' or c.Name like '%' + @Customer + '%')
			and (i.InvoiceNo like '%' + @InvoiceNo + '%')
			and (ISNULL(j.JobNumber,'') like '%' + @Job + '%')    
			and (DATEDIFF(D, i.InvoiceDate, @FromDate) <=0 or @FromDate IS NULL)
			and (DATEDIFF(D, i.InvoiceDate, @ToDate) >=0 or @ToDate IS NULL)
		    and @SortType = 'A'
		group by i.InvoiceNo, i.InvoiceDate, i.InvoiceID, c.Code, c.Name, j.JobNumber, j.JobName
	union all
	select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY CASE @SortColumn WHEN N'0' THEN  i.InvoiceNo END DESC,
													   CASE @SortColumn WHEN N'1' THEN  i.InvoiceDate END DESC,
													   CASE @SortColumn WHEN N'2' THEN  c.Code END DESC,
													   CASE @SortColumn WHEN N'3' THEN  c.Name END DESC,
													   CASE @SortColumn WHEN N'4' THEN  j.JobNumber END DESC,
													   CASE @SortColumn WHEN N'5' THEN  j.JobName END DESC) as RowIndex,
               i.InvoiceID, i.InvoiceNo, CONVERT(nvarchar(10), i.InvoiceDate, 103) as InvoiceDate, 
               c.Code, c.Name, j.JobNumber, j.JobName,
               CASE WHEN 
						(SELECT COUNT(*) FROM tblInvoiceLockStatus as jls WHERE jls.InvoiceID = i.InvoiceID AND jls.Lock = 1) > 0
					THEN CAST(1 AS bit)
					ELSE 
						CASE WHEN 
							(SELECT COUNT(*) FROM tblInvoiceLockStatus as jls WHERE jls.InvoiceID = i.InvoiceID AND jls.Lock = 0) > 0
						THEN CAST(0 AS bit)
						ELSE NULL
						END
				END AS Lock
		from tblInvoice i INNER JOIN
			tblCustomer c ON i.CustomerID = c.CustomerID LEFT OUTER JOIN
			tblInvoiceDetail jd ON i.InvoiceID = jd.InvoiceID LEFT OUTER JOIN
			tblJob j ON jd.JobID = j.JobID
		where (c.Code like '%' + @Customer + '%' or c.Name like '%' + @Customer + '%')
			and (i.InvoiceNo like '%' + @InvoiceNo + '%')
			and (ISNULL(j.JobNumber,'') like '%' + @Job + '%')    
			and (DATEDIFF(D, i.InvoiceDate, @FromDate) <=0 or @FromDate IS NULL)
			and (DATEDIFF(D, i.InvoiceDate, @ToDate) >=0 or @ToDate IS NULL)
		    and @SortType = 'D'
		group by i.InvoiceNo, i.InvoiceDate, i.InvoiceID, c.Code, c.Name, j.JobNumber, j.JobName    
  )
  select top (@PageSize) *, (select COUNT(RowIndex) from T) as RowsCount
  from T
  where RowIndex > (@PageIndex*@PageSize)
END
--exec [TblConfirmOrder_SelectAll] '', '', '', null, null, 0, 10, '1', 'D'


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
				s.FirstName + ' ' + s.LastName as SaleDep,
				CASE WHEN 
					(SELECT COUNT(*) FROM tblJobLockStatus as jls WHERE jls.JobID = j.JobID AND jls.Lock = 1) > 0
				THEN CAST(1 AS bit)
				ELSE 
					CASE WHEN 
						(SELECT COUNT(*) FROM tblJobLockStatus as jls WHERE jls.JobID = j.JobID AND jls.Lock = 0) > 0
					THEN CAST(0 AS bit)
					ELSE NULL
					END
				END AS Lock
			from tblJob j INNER JOIN
				tblCustomer c ON j.CustomerID = c.CustomerID INNER JOIN
				tblStaff s ON j.SalesRepID = s.StaffID LEFT OUTER JOIN
				tblCylinder cyl ON j.JobID = cyl.JobID LEFT OUTER JOIN
				tblOrderConfirmation oc ON j.JobID = oc.JobID LEFT OUTER JOIN
				tblDeliveryOrder do ON j.JobID = do.JobID LEFT OUTER JOIN
				tblInvoiceDetail id ON j.JobID = id.JobID 
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
				j.CreatedOn, s.FirstName, s.LastName
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
				s.FirstName + ' ' + s.LastName as SaleDep,
				CASE WHEN 
					(SELECT COUNT(*) FROM tblJobLockStatus as jls WHERE jls.JobID = j.JobID AND jls.Lock = 1) > 0
				THEN CAST(1 AS bit)
				ELSE 
					CASE WHEN 
						(SELECT COUNT(*) FROM tblJobLockStatus as jls WHERE jls.JobID = j.JobID AND jls.Lock = 0) > 0
					THEN CAST(0 AS bit)
					ELSE NULL
					END
				END AS Lock
			from tblJob j INNER JOIN
				tblCustomer c ON j.CustomerID = c.CustomerID INNER JOIN
				tblStaff s ON j.SalesRepID = s.StaffID LEFT OUTER JOIN
				tblCylinder cyl ON j.JobID = cyl.JobID LEFT OUTER JOIN
				tblOrderConfirmation oc ON j.JobID = oc.JobID LEFT OUTER JOIN
				tblDeliveryOrder do ON j.JobID = do.JobID LEFT OUTER JOIN
				tblInvoiceDetail id ON j.JobID = id.JobID
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
				j.CreatedOn, s.FirstName, s.LastName
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
			c.Code, c.Name, j.JobNumber, j.JobName,
			CASE WHEN 
					(SELECT COUNT(*) FROM tblOrderLockStatus as jls WHERE jls.JobID = o.JobID AND jls.Lock = 1) > 0
				THEN CAST(1 AS bit)
				ELSE 
					CASE WHEN 
						(SELECT COUNT(*) FROM tblOrderLockStatus as jls WHERE jls.JobID = o.JobID AND jls.Lock = 0) > 0
					THEN CAST(0 AS bit)
					ELSE NULL
					END
			END AS Lock
		from  tblOrderConfirmation o INNER JOIN
				tblJob j ON o.JobID = j.JobID INNER JOIN 
				tblCustomer c ON j.CustomerID = c.CustomerID
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
			c.Code, c.Name, j.JobNumber, j.JobName,
			CASE WHEN 
					(SELECT COUNT(*) FROM tblOrderLockStatus as jls WHERE jls.JobID = o.JobID AND jls.Lock = 1) > 0
				THEN CAST(1 AS bit)
				ELSE 
					CASE WHEN 
						(SELECT COUNT(*) FROM tblOrderLockStatus as jls WHERE jls.JobID = o.JobID AND jls.Lock = 0) > 0
					THEN CAST(0 AS bit)
					ELSE NULL
					END
			END AS Lock
		from  tblOrderConfirmation o INNER JOIN
				tblJob j ON o.JobID = j.JobID INNER JOIN 
				tblCustomer c ON j.CustomerID = c.CustomerID
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