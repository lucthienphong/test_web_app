/*
   Friday, December 12, 201410:19:25 AM
   User: sa
   Server: SWEET-KHOATRAN\SQL2008
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
ALTER TABLE dbo.tblJobProcessing
	DROP CONSTRAINT DF__tblOrderF__IdOrd__1D314762
GO
ALTER TABLE dbo.tblJobProcessing
	DROP CONSTRAINT DF__tblOrderF__descr__1E256B9B
GO
ALTER TABLE dbo.tblJobProcessing
	DROP CONSTRAINT DF__tblOrderF__Creat__1F198FD4
GO
ALTER TABLE dbo.tblJobProcessing
	DROP CONSTRAINT DF__tblOrderF__Creat__200DB40D
GO
CREATE TABLE dbo.Tmp_tblJobProcessing
	(
	Id int NOT NULL IDENTITY (1, 1),
	JobID int NULL,
	Description nvarchar(1000) NULL,
	CreatedBy nvarchar(100) NULL,
	CreatedOn datetime NULL,
	FinishedBy nvarchar(100) NULL,
	FinishedOn datetime NULL,
	DepartmentID int NULL,
	JobStatus nvarchar(50) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_tblJobProcessing SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_tblJobProcessing ADD CONSTRAINT
	DF__tblOrderF__IdOrd__1D314762 DEFAULT (NULL) FOR JobID
GO
ALTER TABLE dbo.Tmp_tblJobProcessing ADD CONSTRAINT
	DF__tblOrderF__descr__1E256B9B DEFAULT (NULL) FOR Description
GO
ALTER TABLE dbo.Tmp_tblJobProcessing ADD CONSTRAINT
	DF__tblOrderF__Creat__1F198FD4 DEFAULT (NULL) FOR CreatedBy
GO
ALTER TABLE dbo.Tmp_tblJobProcessing ADD CONSTRAINT
	DF__tblOrderF__Creat__200DB40D DEFAULT (NULL) FOR CreatedOn
GO
SET IDENTITY_INSERT dbo.Tmp_tblJobProcessing ON
GO
IF EXISTS(SELECT * FROM dbo.tblJobProcessing)
	 EXEC('INSERT INTO dbo.Tmp_tblJobProcessing (Id, JobID, Description, CreatedBy, CreatedOn, FinishedBy, FinishedOn, DepartmentID)
		SELECT Id, JobID, CONVERT(nvarchar(1000), Description), CONVERT(nvarchar(100), CreatedBy), CreatedOn, CONVERT(nvarchar(100), FinishedBy), FinishedOn, DepartmentID FROM dbo.tblJobProcessing WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_tblJobProcessing OFF
GO
DROP TABLE dbo.tblJobProcessing
GO
EXECUTE sp_rename N'dbo.Tmp_tblJobProcessing', N'tblJobProcessing', 'OBJECT' 
GO
ALTER TABLE dbo.tblJobProcessing ADD CONSTRAINT
	PK__tblOrder__3214EC071B48FEF0 PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
