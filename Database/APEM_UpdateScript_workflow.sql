alter table tblDepartment
ADD ShowInWorkFlow bit not NULL DEFAULT(1)
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
