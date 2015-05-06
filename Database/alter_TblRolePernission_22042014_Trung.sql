/*
   Wednesday, April 22, 20154:03:43 PM
   User: sweetdev
   Server: SWEET-LAPTOP-2\MSSQL2008
   Database: SweetSoft_APEM
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.tblRolePermission ADD
	AllowLock bit NOT NULL CONSTRAINT DF_tblRolePermission_AllowLock DEFAULT 0,
	AllowUnlock bit NOT NULL CONSTRAINT DF_tblRolePermission_AllowUnlock DEFAULT 0,
	AllowEditUnlock bit NOT NULL CONSTRAINT DF_tblRolePermission_AllowEditUnlock DEFAULT 0
GO
ALTER TABLE dbo.tblRolePermission SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
