﻿BEGIN TRANSACTION
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
ALTER TABLE dbo.tblEngraving ADD
	JobTicket nvarchar(256) NULL
GO
ALTER TABLE dbo.tblEngraving SET (LOCK_ESCALATION = TABLE)
GO

ALTER TABLE dbo.tblCredit ADD
	TaxID int NULL
GO
ALTER TABLE dbo.tblCredit SET (LOCK_ESCALATION = TABLE)
GO

COMMIT