USE [SweetSoft_APEM]
GO

alter table tblNotificationSetting
ADD CreatedOn datetime  NULL 
alter table tblNotificationSetting
ADD CreatedBy nvarchar(100)  NULL 

alter table tblNotificationSetting
ADD ModifiedOn datetime  NULL 
alter table tblNotificationSetting
ADD ModifiedBy nvarchar(100)  NULL 