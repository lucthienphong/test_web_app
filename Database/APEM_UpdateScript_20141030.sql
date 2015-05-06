USE [SweetSoft_APEM]
GO
ALTER proc [dbo].[tblCustomer_SelectAll]
@KeyWord nvarchar(100),
@IsObsolete bit,
@PageIndex int,
@PageSize int,
@SortColumn varchar(1),
@SortType varchar(1)
as
begin
	WITH T AS(
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN c.Code END ASC,
															CASE @SortColumn WHEN N'1' THEN c.Name END ASC,
															CASE @SortColumn WHEN N'2' THEN c.Address END ASC,
															CASE @SortColumn WHEN N'3' THEN c.Tel END ASC,
															CASE @SortColumn WHEN N'4' THEN c.Fax END ASC,
															CASE @SortColumn WHEN N'5' THEN c.IsObsolete END ASC) as RowIndex, 
				CustomerID, Code, Name, Address, Tel, Fax, IsObsolete
			from tblCustomer c
			where (c.Code like N'%' + @KeyWord + '%'
						OR	c.Name like N'%' + @KeyWord + '%'
						OR	c.Address like N'%' + @KeyWord + '%'
						OR	c.Tel like N'%' + @KeyWord + '%'
						OR	c.Fax like N'%' + @KeyWord + '%')
				AND (c.IsObsolete = @IsObsolete or @IsObsolete is null)
				AND @SortType = 'A'
		UNION ALL
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN c.Code END DESC,
															CASE @SortColumn WHEN N'1' THEN c.Name END DESC,
															CASE @SortColumn WHEN N'2' THEN c.Address END DESC,
															CASE @SortColumn WHEN N'3' THEN c.Tel END DESC,
															CASE @SortColumn WHEN N'4' THEN c.Fax END DESC,
															CASE @SortColumn WHEN N'2' THEN c.IsObsolete END DESC) as RowIndex, 
				CustomerID, Code, Name, Address, Tel, Fax, IsObsolete
			from tblCustomer c
			where (c.Code like N'%' + @KeyWord + '%'
						OR	c.Name like N'%' + @KeyWord + '%'
						OR	c.Address like N'%' + @KeyWord + '%'
						OR	c.Tel like N'%' + @KeyWord + '%'
						OR	c.Fax like N'%' + @KeyWord + '%')
				AND (c.IsObsolete = @IsObsolete or @IsObsolete is null)
				AND @SortType = 'D'
	)
	select top (@PageSize) *, (select COUNT(RowIndex) from T) as RowsCount
		from T
		where RowIndex > (@PageIndex*@PageSize)
end
--exec tblCustomer_SelectAll N'', NULL, 0, 10, '0', 'A'


GO
ALTER proc [dbo].[tblCustomer_SelectForReport]
@KeyWord nvarchar(100),
@IsObsolete bit,
@SortColumn varchar(1),
@SortType varchar(1)
as
begin
	WITH T AS(
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN c.Code END ASC,
															CASE @SortColumn WHEN N'1' THEN c.Name END ASC,
															CASE @SortColumn WHEN N'2' THEN c.Address END ASC,
															CASE @SortColumn WHEN N'3' THEN c.Tel END ASC,
															CASE @SortColumn WHEN N'4' THEN c.Fax END ASC,
															CASE @SortColumn WHEN N'5' THEN c.IsObsolete END ASC) as RowIndex, 
				c.CustomerID, c.Code, c.Name, c.Address, c.Tel, c.Fax, c.PostCode, c.City, c.Country, 
				c.IsObsolete, s.FirstName + ' ' + s.LastName as FullName
			from tblCustomer c INNER JOIN
				tblStaff s ON c.SaleRepID = s.StaffID
			where (c.Code like N'%' + @KeyWord + '%'
						OR	c.Name like N'%' + @KeyWord + '%'
						OR	c.Address like N'%' + @KeyWord + '%'
						OR	c.Tel like N'%' + @KeyWord + '%'
						OR	c.Fax like N'%' + @KeyWord + '%')
				AND (c.IsObsolete = @IsObsolete or @IsObsolete is null)
				AND @SortType = 'A'
		UNION ALL
		select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN c.Code END DESC,
															CASE @SortColumn WHEN N'1' THEN c.Name END DESC,
															CASE @SortColumn WHEN N'2' THEN c.Address END DESC,
															CASE @SortColumn WHEN N'3' THEN c.Tel END DESC,
															CASE @SortColumn WHEN N'4' THEN c.Fax END DESC,
															CASE @SortColumn WHEN N'2' THEN c.IsObsolete END DESC) as RowIndex, 
				c.CustomerID, c.Code, c.Name, c.Address, c.Tel, c.Fax, c.PostCode, c.City, c.Country, 
				c.IsObsolete, s.FirstName + ' ' + s.LastName as FullName
			from tblCustomer c INNER JOIN
				tblStaff s ON c.SaleRepID = s.StaffID
			where (c.Code like N'%' + @KeyWord + '%'
						OR	c.Name like N'%' + @KeyWord + '%'
						OR	c.Address like N'%' + @KeyWord + '%'
						OR	c.Tel like N'%' + @KeyWord + '%'
						OR	c.Fax like N'%' + @KeyWord + '%')
				AND (c.IsObsolete = @IsObsolete or @IsObsolete is null)
				AND @SortType = 'D'
	)
	select *
		from T
end
--exec tblCustomer_SelectForReport N'', NULL, '0', 'A'


--Update tblfunction records
GO
Delete tblRolePermission
GO
Delete tblFunction
GO
INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'department_manager', N'fUser', N'Department manager', N'', 5)
INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'fSystemConfig', N'', N'System configuration management', N'', 6)
INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'fUser', N'', N'User management', N'', 1)
INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'role_manager', N'fUser', N'Role manager', N'', 3)
INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'role_permission_manager', N'fUser', N'Role permission manager', N'', 4)
INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'system_configuration_manager', N'fSystemConfig', N'System configuration manager', N'', 7)
INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'user_manager', N'fUser', N'User manager', N'', 2)

--Add data to RolePermission
GO
Insert into tblRolePermission
select 1, FunctionID, 1, 1, 1, 1, 1 from tblFunction


--Update right value for IsObsolete field
GO
update tblRole set IsObsolete = 0

GO
update tblStaff set IsObsolete = 0

GO
update tblUser set IsObsolete = 0
