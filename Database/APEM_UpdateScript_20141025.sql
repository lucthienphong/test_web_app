USE [SweetSoft_APEM]

GO
IF OBJECTPROPERTY(OBJECT_ID('dbo.tblRole_SelectAll'), N'IsProcedure') = 1
DROP PROCEDURE [dbo].[tblRole_SelectAll]
GO
CREATE proc [dbo].[tblRole_SelectAll]
@KeyWord nvarchar(100),
@IsActive bit,
@PageIndex int,
@PageSize int,
@SortColumn varchar(1),
@SortType varchar(1)
as
begin
	WITH T AS(
	select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN r.RoleName END ASC,
														CASE @SortColumn WHEN N'1' THEN r.Description END ASC,
														CASE @SortColumn WHEN N'3' THEN r.IsObsolete END ASC) as RowIndex, 
		r.RoleID, r.RoleName, r.Description, r.IsObsolete
	from tblRole r
		WHERE (r.RoleName like '%' + @KeyWord + '%')
			AND (IsObsolete = @IsActive OR @IsActive IS NULL)
			AND @SortType = 'A'		
	UNION ALL
	select TOP 100 PERCENT ROW_NUMBER() OVER(ORDER BY	CASE @SortColumn WHEN N'0' THEN r.RoleName END DESC,
														CASE @SortColumn WHEN N'1' THEN r.Description END DESC,
														CASE @SortColumn WHEN N'3' THEN r.IsObsolete END DESC) as RowIndex, 
		r.RoleID, r.RoleName, r.Description, r.IsObsolete
	from tblRole r
		WHERE (r.RoleName like '%' + @KeyWord + '%')
			AND (IsObsolete = @IsActive OR @IsActive IS NULL)
			AND @SortType = 'D'	
	)
	select top (@PageSize) *, (select count(RowIndex) from T) as RowsCount 
		from T
		where RowIndex > (@PageIndex*@PageSize)
end
--exec tblRole_SelectAll N'', 1, 0, 10, '0', 'A'


--Delete old values
GO
delete tblRolePermission
GO
delete tblFunction
--Insert tblFunction
GO
/****** Object:  Table [dbo].[tblFunction]    Script Date: 10/25/2014 17:38:13 ******/
INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'department_manager', N'fUser', N'Department manager', N'', 5)
INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'fSystemConfig', N'', N'System configuration management', N'', 6)
INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'fUser', N'', N'User management', N'', 1)
INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'role_manager', N'fUser', N'Role manager', N'', 3)
INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'role_permission_manager', N'fUser', N'Role permission manager', N'', 4)
INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'system_configuration_manager', N'fSystemConfig', N'System configuration manager', N'', 7)
INSERT [dbo].[tblFunction] ([FunctionID], [ParentID], [Title], [Description], [DisplayOrder]) VALUES (N'user_manager', N'fUser', N'User manager', N'', 2)

--Insert tblRolePermission
GO
insert into tblRolePermission
select 1, FunctionID, 1, 1, 1, 1, 1 from tblFunction