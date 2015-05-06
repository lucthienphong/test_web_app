USE [SweetSoft_APEM]

--ALTER TABLE TblJob
GO
ALTER TABLE tblJob
ADD JobBarcodeImage ntext NULL

GO
ALTER TABLE tblJob
ADD ShipToParty nvarchar(2000) NULL

GO
ALTER TABLE tblJob
ADD RevisionFromJob int NULL

GO
ALTER TABLE tblJob
ADD RevisionRootNumber int NULL

GO
EXEC sp_RENAME 'tblJob.IsInternal' , 'InternalExternal', 'COLUMN'

GO
ALTER TABLE tblJob
ALTER COLUMN InternalExternal varchar(20) NULL

GO
ALTER TABLE tblJob
ADD IsClosed bit NULL

--ALTER TABLE TblJobSheet
GO
ALTER TABLE tblJobSheet
ADD Diameter float NOT NULL

GO
ALTER TABLE tblJobSheet
ADD HasIrisProof bit NULL

--ALTER TABLE TblCylinder
GO
ALTER TABLE TblCylinder
ADD Dirameter float NOT NULL

--ALTER TABLE TblWorkflow
alter table tblDepartment
ALTER COLUMN IsObsolete bit NULL

alter table tblDepartment
ADD ShowInWorkFlow bit NULL DEFAULT(1)
GO

alter table tblDepartment
ADD IsTimeline bit NULL DEFAULT(1)
GO

alter table tblDepartment
ADD TimelineOrder tinyint NULL DEFAULT(1)
GO

alter table tblWorkFlow
ADD IdParent int NULL
GO

alter table tblWorkFlow
drop column listFromConnection
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblWorkFlow_tblMachinaryProduceType]') AND parent_object_id = OBJECT_ID(N'[dbo].[tblWorkFlow]'))
ALTER TABLE [dbo].[tblWorkFlow] DROP CONSTRAINT [FK_tblWorkFlow_tblMachinaryProduceType]
GO



IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblWorkFlowNode_tblDepartment]') AND parent_object_id = OBJECT_ID(N'[dbo].[tblWorkFlowNode]'))
ALTER TABLE [dbo].[tblWorkFlowNode] DROP CONSTRAINT [FK_tblWorkFlowNode_tblDepartment]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblWorkFlowNode_tblMachinaryProduceType]') AND parent_object_id = OBJECT_ID(N'[dbo].[tblWorkFlowNode]'))
ALTER TABLE [dbo].[tblWorkFlowNode] DROP CONSTRAINT [FK_tblWorkFlowNode_tblMachinaryProduceType]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblWorkFlowNode_tblWorkTask]') AND parent_object_id = OBJECT_ID(N'[dbo].[tblWorkFlowNode]'))
ALTER TABLE [dbo].[tblWorkFlowNode] DROP CONSTRAINT [FK_tblWorkFlowNode_tblWorkTask]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblWorkFlowNode_tblWorkTaskInNode]') AND parent_object_id = OBJECT_ID(N'[dbo].[tblWorkFlowNode]'))
ALTER TABLE [dbo].[tblWorkFlowNode] DROP CONSTRAINT [FK_tblWorkFlowNode_tblWorkTaskInNode]
GO


IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblWorkFlowLine_tblMachinaryProduceType]') AND parent_object_id = OBJECT_ID(N'[dbo].[tblWorkFlowLine]'))
ALTER TABLE [dbo].[tblWorkFlowLine] DROP CONSTRAINT [FK_tblWorkFlowLine_tblMachinaryProduceType]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblWorkFlowLine_tblWorkFlowNode]') AND parent_object_id = OBJECT_ID(N'[dbo].[tblWorkFlowLine]'))
ALTER TABLE [dbo].[tblWorkFlowLine] DROP CONSTRAINT [FK_tblWorkFlowLine_tblWorkFlowNode]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblWorkFlowLine_tblWorkFlowNode1]') AND parent_object_id = OBJECT_ID(N'[dbo].[tblWorkFlowLine]'))
ALTER TABLE [dbo].[tblWorkFlowLine] DROP CONSTRAINT [FK_tblWorkFlowLine_tblWorkFlowNode1]
GO

--CREATE CODE DICTIONARY
GO
/****** Object:  Table [dbo].[tblCodeDictionary]    Script Date: 12/11/2014 14:52:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblCodeDictionary](
	[CodeID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nchar](10) NOT NULL,
	[IsUsed] [bit] NOT NULL,
 CONSTRAINT [PK_tblCodeDictionary] PRIMARY KEY CLUSTERED 
(
	[CodeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

GO

/****** Object:  Table [dbo].[tblJobProcessing]    Script Date: 12/11/2014 16:08:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tblJobProcessing](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[JobID] [int] NULL,
	[Description] [varchar](1000) NULL,
	[CreatedBy] [varchar](100) NULL,
	[CreatedOn] [datetime] NULL,
	[FinishedBy] [varchar](100) NULL,
	[FinishedOn] [datetime] NULL,
	[DepartmentID] [int] NULL,
 CONSTRAINT [PK__tblOrder__3214EC071B48FEF0] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[tblJobProcessing] ADD  CONSTRAINT [DF__tblOrderF__IdOrd__1D314762]  DEFAULT (NULL) FOR [JobID]
GO

ALTER TABLE [dbo].[tblJobProcessing] ADD  CONSTRAINT [DF__tblOrderF__descr__1E256B9B]  DEFAULT (NULL) FOR [Description]
GO

ALTER TABLE [dbo].[tblJobProcessing] ADD  CONSTRAINT [DF__tblOrderF__Creat__1F198FD4]  DEFAULT (NULL) FOR [CreatedBy]
GO

ALTER TABLE [dbo].[tblJobProcessing] ADD  CONSTRAINT [DF__tblOrderF__Creat__200DB40D]  DEFAULT (NULL) FOR [CreatedOn]
GO

--GO
--declare @code varchar(6),
--		@n numeric
--set @n = 0
--while(@n < 999999)
--begin
--	set @n = @n + 1;
--	set @code = CAST(@n as varchar(6))
--	while(LEN(@code) < 6)
--		set @code = '0' + @code
--	insert into tblCodeDictionary values(@code, 0)
--end

--PROCEDURE
GO
/****** Object:  StoredProcedure [dbo].[tblJob_SelectRevisionHistory]    Script Date: 12/11/2014 14:47:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[tblJob_SelectRevisionHistory]
@JobID int
as
begin
	select j.JobID, j.RevNumber, ISNULL(j.RevisionRootNumber,0) as RootRevNumber, j.IsClosed,
		s.FirstName + ' ' + s.LastName as CreatedBy, CONVERT(NVARCHAR(10), j.CreatedOn, 103) as CreatedOn, 
		ISNULL(j.InternalExternal, '') as InternalExternal, ISNULL(j.RevisionDetail, '') as RevisionDetail
		from tblJob j INNER JOIN
			tblUser u On j.CreatedBy = u.UserName INNER JOIN
			tblStaff s ON u.UserID = s.StaffID INNER JOIN
			(select ISNULL(RevisionFromJob, JobID) as JobID from tblJob where JobID = @JobID) as rj ON rj.JobID = j.JobID or rj.JobID = j.RevisionFromJob
		order by RevNumber DESC, CreatedOn DESC 
end
--exec tblJob_SelectRevisionHistory 10

GO

/****** Object:  StoredProcedure [dbo].[tblJob_SelectAll]    Script Date: 12/11/2014 14:48:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[tblJob_SelectAll]
@Customer nvarchar(200),
@JobBarcode nvarchar(255),
@JobNumber nvarchar(10),
@JobInfo nvarchar(200),
@SaleRepID int,
@FromDate datetime,
@ToDate datetime,
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
															CASE @SortColumn WHEN N'2' THEN j.JobName END ASC,
															CASE @SortColumn WHEN N'3' THEN j.Design END ASC,
															CASE @SortColumn WHEN N'4' THEN j.CreatedOn END ASC) as RowIndex, 
				j.JobID, c.Code, c.Name, j.JobBarcode, j.JobNumber, j.JobName, j.Design, j.RevNumber, 
				CONVERT(nvarchar(10), j.CreatedOn, 103) as CreatedOn, 
				s.FirstName + ' ' + s.LastName as SaleDep
			from tblJob j INNER JOIN
				tblCustomer c ON j.CustomerID = c.CustomerID INNER JOIN
				tblStaff s ON j.SalesRepID = s.StaffID
			where j.IsClosed = 0
				and (c.Name like '%' + @Customer + '%' or c.Code like '%' + @Customer + '%')
				and j.JobBarcode like '%' + @JobBarcode + '%'
				and j.JobNumber like '%' + @JobNumber + '%'
				and (j.JobName like '%' + @JobInfo + '%' or j.Design like '%' + @JobInfo + '%')
				and (j.SalesRepID = @SaleRepID or @SaleRepID = 0)
				and (DATEDIFF(D, j.CreatedOn, @FromDate) <=0 or @FromDate IS NULL)
				and (DATEDIFF(D, j.CreatedOn, @ToDate) >=0 or @ToDate IS NULL)
				and @SortType = 'A'
		union all
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN  c.Code END DESC,
															--CASE @SortColumn WHEN N'1' THEN c.Name END DESC,
															--CASE @SortColumn WHEN N'2' THEN j.JobBarcode END DESC,
															CASE @SortColumn WHEN N'1' THEN j.JobNumber END DESC,
															CASE @SortColumn WHEN N'2' THEN j.JobName END DESC,
															CASE @SortColumn WHEN N'3' THEN j.Design END DESC,
															CASE @SortColumn WHEN N'4' THEN j.CreatedOn END DESC) as RowIndex, 
				j.JobID, c.Code, c.Name, j.JobBarcode, j.JobNumber, j.JobName, j.Design, j.RevNumber, 
				CONVERT(nvarchar(10), j.CreatedOn, 103) as CreatedOn,
				s.FirstName + ' ' + s.LastName as SaleDep
			from tblJob j INNER JOIN
				tblCustomer c ON j.CustomerID = c.CustomerID INNER JOIN
				tblStaff s ON j.SalesRepID = s.StaffID
			where j.IsClosed = 0
				and (c.Name like '%' + @Customer + '%' or c.Code like '%' + @Customer + '%')
				and j.JobBarcode like '%' + @JobBarcode + '%'
				and j.JobNumber like '%' + @JobNumber + '%'
				and (j.JobName like '%' + @JobInfo + '%' or j.Design like '%' + @JobInfo + '%')
				and (j.SalesRepID = @SaleRepID or @SaleRepID = 0)
				and (DATEDIFF(D, j.CreatedOn, @FromDate) <=0 or @FromDate IS NULL)
				and (DATEDIFF(D, j.CreatedOn, @ToDate) >=0 or @ToDate IS NULL)
				and @SortType = 'D'
		)
		select top (@PageSize) *, (select COUNT(RowIndex) from T) as RowsCount
			from T
			where RowIndex > (@PageIndex*@PageSize)
	--exec tblJob_SelectAll '', '', '', '', 0, '2014/11/28', null, 0, 10, '4', 'D'
END
GO

GO
/****** Object:  StoredProcedure [dbo].[tblCylinder_SelectAll]    Script Date: 12/11/2014 14:49:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[tblCylinder_SelectAll]
@JobID int
as
begin
	select c.CylinderID, c.Sequence, c.SteelBase, c.CylinderNo, c.Color, c.PricingID, p.PricingName, 
			c.CylinderStatusID, cs.CylinderStatusName, c.Dirameter, c.Dept, c.IsPivotCylinder
		from tblCylinder c INNER JOIN
			tblCylinderStatus cs ON c.CylinderStatusID = cs.CylinderStatusID INNER JOIN
			tblPricing p ON c.PricingID = p.PricingID
		where c.JobID = @JobID
		order by c.Sequence
end
--exec tblCylinder_SelectAll 5
GO

GO
/****** Object:  StoredProcedure [dbo].[tblJob_SelectForReport]    Script Date: 12/11/2014 14:50:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[tblJob_SelectForReport]
@Customer nvarchar(200),
@JobBarcode nvarchar(255),
@JobNumber nvarchar(10),
@JobInfo nvarchar(200),
@SaleRepID int,
@FromDate datetime,
@ToDate datetime,
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
															CASE @SortColumn WHEN N'2' THEN j.JobName END ASC,
															CASE @SortColumn WHEN N'3' THEN j.Design END ASC,
															CASE @SortColumn WHEN N'4' THEN j.CreatedOn END ASC) as RowIndex, 
				j.JobID, c.Code, c.Name, j.JobBarcode, j.JobNumber, j.JobName, j.Design, j.RevNumber, j.JobBarcodeImage, 
				CONVERT(nvarchar(10), j.CreatedOn, 103) as CreatedOn, 
				s.FirstName + ' ' + s.LastName as SaleDep
			from tblJob j INNER JOIN
				tblCustomer c ON j.CustomerID = c.CustomerID INNER JOIN
				tblStaff s ON j.SalesRepID = s.StaffID
			where j.IsClosed = 0
				and (c.Name like '%' + @Customer + '%' or c.Code like '%' + @Customer + '%')
				and j.JobBarcode like '%' + @JobBarcode + '%'
				and j.JobNumber like '%' + @JobNumber + '%'
				and (j.JobName like '%' + @JobInfo + '%' or j.Design like '%' + @JobInfo + '%')
				and (j.SalesRepID = @SaleRepID or @SaleRepID = 0)
				and (DATEDIFF(D, j.CreatedOn, @FromDate) <=0 or @FromDate IS NULL)
				and (DATEDIFF(D, j.CreatedOn, @ToDate) >=0 or @ToDate IS NULL)
				and @SortType = 'A'
		union all
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN  c.Code END DESC,
															--CASE @SortColumn WHEN N'1' THEN c.Name END DESC,
															--CASE @SortColumn WHEN N'2' THEN j.JobBarcode END DESC,
															CASE @SortColumn WHEN N'1' THEN j.JobNumber END DESC,
															CASE @SortColumn WHEN N'2' THEN j.JobName END DESC,
															CASE @SortColumn WHEN N'3' THEN j.Design END DESC,
															CASE @SortColumn WHEN N'4' THEN j.CreatedOn END DESC) as RowIndex, 
				j.JobID, c.Code, c.Name, j.JobBarcode, j.JobNumber, j.JobName, j.Design, j.RevNumber, j.JobBarcodeImage,
				CONVERT(nvarchar(10), j.CreatedOn, 103) as CreatedOn,
				s.FirstName + ' ' + s.LastName as SaleDep
			from tblJob j INNER JOIN
				tblCustomer c ON j.CustomerID = c.CustomerID INNER JOIN
				tblStaff s ON j.SalesRepID = s.StaffID
			where j.IsClosed = 0
				and (c.Name like '%' + @Customer + '%' or c.Code like '%' + @Customer + '%')
				and j.JobBarcode like '%' + @JobBarcode + '%'
				and j.JobNumber like '%' + @JobNumber + '%'
				and (j.JobName like '%' + @JobInfo + '%' or j.Design like '%' + @JobInfo + '%')
				and (j.SalesRepID = @SaleRepID or @SaleRepID = 0)
				and (DATEDIFF(D, j.CreatedOn, @FromDate) <=0 or @FromDate IS NULL)
				and (DATEDIFF(D, j.CreatedOn, @ToDate) >=0 or @ToDate IS NULL)
				and @SortType = 'D'
		)
		select T.*, (select COUNT(RowIndex) from T) as RowsCount
			from T
	--exec tblJob_SelectForReport '', '', '', '', 0, '2014/11/28', null, 0, 10, '4', 'D'
END
GO

GO
/****** Object:  StoredProcedure [dbo].[tblJob_CreateJobBarcode]    Script Date: 12/11/2014 14:51:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[tblJob_CreateJobBarcode]
@NewBarcode nvarchar(50) out
as
begin
	declare @today nvarchar(10);
	set @today = CONVERT(nvarchar, GETDATE(), 102);
	set @today = REPLACE(SUBSTRING(@today, 3, 5), '.', '');
	select @NewBarcode = ISNULL(MAX(LEFT(JobBarcode,5)),0) + 1
		from tblJob
		where RIGHT(JobBarcode,4) = @today
	while(LEN(@NewBarcode) < 5)
		set @NewBarcode = '0' + @NewBarcode;
	set @NewBarcode = @NewBarcode + '-' + @today
--print @NewBarcode
end
--declare @n nvarchar(50)
--exec tblJob_CreateJobBarcode @n out
--	print @n
GO

GO
/****** Object:  StoredProcedure [dbo].[tblDepartment_SelectAll]    Script Date: 12/11/2014 17:00:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[tblDepartment_SelectAll]
@KeyWord nvarchar(100),
@IsObsolete bit,
@PageIndex int,
@PageSize int,
@SortColumn varchar(1),
@SortType varchar(1)
as
begin
	WITH T AS(
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN d.DepartmentName END ASC,
															CASE @SortColumn WHEN N'1' THEN d.ShowInWorkFlow END ASC,
															CASE @SortColumn WHEN N'2' THEN d.IsTimeline END ASC,
															CASE @SortColumn WHEN N'3' THEN ISNULL(TimelineOrder, 0) END ASC,
															CASE @SortColumn WHEN N'4' THEN d.IsObsolete END ASC) as RowIndex, 
				DepartmentID, DepartmentName, ShowInWorkFlow, IsTimeline, ISNULL(TimelineOrder, 0) TimelineOrder, IsObsolete
			from tblDepartment d
			where d.DepartmentName like N'%' + @KeyWord + '%'
				AND (d.IsObsolete = @IsObsolete or @IsObsolete is null)
				AND @SortType = 'A'
		UNION ALL
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN d.DepartmentName END DESC,
															CASE @SortColumn WHEN N'1' THEN d.ShowInWorkFlow END DESC,
															CASE @SortColumn WHEN N'2' THEN d.IsTimeline END DESC,
															CASE @SortColumn WHEN N'3' THEN ISNULL(TimelineOrder, 0) END ASC,
															CASE @SortColumn WHEN N'4' THEN d.IsObsolete END ASC) as RowIndex, 
				DepartmentID, DepartmentName, ShowInWorkFlow, IsTimeline, ISNULL(TimelineOrder, 0) TimelineOrder, IsObsolete
			from tblDepartment d
			where d.DepartmentName like N'%' + @KeyWord + '%'
				AND (d.IsObsolete = @IsObsolete or @IsObsolete is null)
				AND @SortType = 'D'
	)
	select top (@PageSize) *, (select COUNT(RowIndex) from T) as RowsCount
		from T
		where RowIndex > (@PageIndex*@PageSize)
end
--exec tblDepartment_SelectAll N'', NULL, 0, 10, '0', 'A'

--UPDATE tblRolePermission
GO
delete tblRolePermission
GO
delete tblFunction
GO
/****** Object:  Table [dbo].[tblFunction]    Script Date: 12/11/2014 17:44:06 ******/
INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'backing_manager', N'fReference', N'Backing manager', N'', 9)
INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'currency_exchange_manager', N'fReference', N'Currency exchange manager', N'', 10)
INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'customer_manager', N'fCustomer', N'Customer manager', N'', 2)
INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'customer_quotation_manager', N'fCustomer', N'Customer quotation', N'', 3)
INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'cylinder_status_manager', N'fReference', N'Cylinder status manager', N'', 11)
INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'department_manager', N'fUser', N'Department manager', N'', 12)
INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'fCustomer', N'', N'Customer management', N'', 1)
INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'fJob', N'', N'Job management', N'', 4)
INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'fReference', N'', N'Reference', N'', 8)
INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'fSystemConfig', N'', N'System configuration management', N'', 17)
INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'job_manager', N'fJob', N'Job manager', N'', 5)
INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'pricing_manager', N'fCustomer', N'Pricing manager', N'', 13)

INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'role_manager', N'fSystemConfig', N'Role manager', N'', 19)
INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'role_permission_manager', N'fSystemConfig', N'Role permission manager', N'', 20)
INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'supplier_manager', N'fReference', N'Supplier manager', N'', 14)
INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'supply_manager', N'fReference', N'Supply manager', N'', 15)
INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'system_configuration_manager', N'fSystemConfig', N'System configuration manager', N'', 21)
INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'tax_manager', N'fReference', N'Tax manager', N'', 16)
INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'timeline_manager', N'fJob', N'Job timeline manager', N'', 7)
INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'user_manager', N'fSystemConfig', N'User manager', N'', 18)
INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'workflow_manager', N'fJob', N'Workflow manager', N'', 6)


GO
insert into tblRolePermission
select 1, FunctionID, 1, 1, 1, 1, 1 from tblFunction
