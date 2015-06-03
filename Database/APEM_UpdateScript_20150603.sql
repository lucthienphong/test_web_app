USE [SweetSoft_APEM-Production]
GO
/****** Object:  StoredProcedure [dbo].[JobPrintingDetail]    Script Date: 06/03/2015 10:45:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- up date storeprocedure

ALTER proc [dbo].[JobPrintingDetail]
@JobID int
as
begin
		select 
			c.Name,j.JobNumber, j.RevNumber,j.JobName,j.Design,
			j.RootJobNo as RootJobNumber, 
			j.RootJobRevNumber, j.CommonJobNumber,
			(stSale.FirstName +' '+ stSale.LastName) as SalePerson, (st2.FirstName) as CreatedBy,
			CONVERT(nvarchar(10),j.CreatedOn,103) as CreatedDate,
			CONVERT(nvarchar(10),js.ReproDate,103) as ReproCreateDate, js.IrisProof,
			(st.FirstName + st.LastName) as CoopName, CONVERT(nvarchar(10),js.CylinderDate,103) as CylinderCreateDate,js.DeilveryNotes,
			 js.LeavingAPE, js.EMWidth,js.EMPonsition,js.PrintingDirection,
			 js.EMHeight,js.EMColor,b.BackingName,js.EMPonsition,
			js.BarcodeSize,js.BarcodeColor,js.BarcodeNo,js.BWR, s.SupplyName,
			Cast(js.UNSizeV as decimal(18,2)) as UNSizeV,Cast(js.UNSizeH as decimal(18,2)) as UNSizeH, js.PrintingDirection,
			js.Size,js.OpaqueInkRate,js.ColorTarget,js.ProofingMaterial,
			js.NumberOfRepeatV, js.NumberOfRepeatH,
			Cast(js.FaceWidth as decimal(18,2)) as FaceWidth,
			Cast(js.Circumference as decimal(18,2)) as Circumference,js.TypeOfCylinder,js.Printing,j.Remark,
			 ct.ContactName, s.SupplyName, j.CustomerPO1, j.CustomerPO2,
			 js.EyeMark
			from tblJob j left outer join tblJobSheet js on j.JobID = js.JobID 
				inner join tblUser u on j.CreatedBy  = u.UserName
				inner join tblStaff st2 on u.UserID = st2.StaffID
				left outer join tblBacking b on js.BackingID = b.BackingID 
				-- inner join tblCylinder cl on cl.JobID = j.JobID 
				inner join tblCustomer c on j.CustomerID = c.CustomerID 
				inner join tblContact ct on j.ContactPersonID = ct.ContactID
				left outer join tblSupply s on s.SupplyID = js.SupplyID
				left outer join tblStaff st on st.StaffID = j.CoordinatorID
				left outer join tblStaff stSale on stSale.StaffID = j.SalesRepID
			where j.JobID = @JobID
-- exec [JobPrintingDetail] 932
end


/****** Object:  StoredProcedure [dbo].[tblCylinder_SelectForOrderConfirmation]    Script Date: 05/25/2015 15:09:40 ******/
SET ANSI_NULLS ON
