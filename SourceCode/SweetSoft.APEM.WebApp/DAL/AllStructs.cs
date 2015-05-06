using System; 
using System.Text; 
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration; 
using System.Xml; 
using System.Xml.Serialization;
using SubSonic; 
using SubSonic.Utilities;
// <auto-generated />
namespace SweetSoft.APEM.DataAccess
{
	#region Tables Struct
	public partial struct Tables
	{
		
		public static readonly string AspnetApplication = @"aspnet_Applications";
        
		public static readonly string AspnetMembership = @"aspnet_Membership";
        
		public static readonly string AspnetPath = @"aspnet_Paths";
        
		public static readonly string AspnetPersonalizationAllUser = @"aspnet_PersonalizationAllUsers";
        
		public static readonly string AspnetPersonalizationPerUser = @"aspnet_PersonalizationPerUser";
        
		public static readonly string AspnetProfile = @"aspnet_Profile";
        
		public static readonly string AspnetRole = @"aspnet_Roles";
        
		public static readonly string AspnetSchemaVersion = @"aspnet_SchemaVersions";
        
		public static readonly string AspnetUser = @"aspnet_Users";
        
		public static readonly string AspnetUsersInRole = @"aspnet_UsersInRoles";
        
		public static readonly string AspnetWebEventEvent = @"aspnet_WebEvent_Events";
        
		public static readonly string TblBacking = @"tblBacking";
        
		public static readonly string TblCodeDictionary = @"tblCodeDictionary";
        
		public static readonly string TblContact = @"tblContact";
        
		public static readonly string TblCredit = @"tblCredit";
        
		public static readonly string TblCreditDetail = @"tblCreditDetail";
        
		public static readonly string TblCurrency = @"tblCurrency";
        
		public static readonly string TblCurrencyChangedLog = @"TblCurrencyChangedLog";
        
		public static readonly string TblCustomer = @"tblCustomer";
        
		public static readonly string TblCustomerQuotation = @"tblCustomerQuotation";
        
		public static readonly string TblCustomerQuotationAdditionalService = @"tblCustomerQuotation_AdditionalService";
        
		public static readonly string TblCustomerQuotationOtherCharge = @"tblCustomerQuotation_OtherCharges";
        
		public static readonly string TblCustomerQuotationPricing = @"tblCustomerQuotation_Pricing";
        
		public static readonly string TblCustomerQuotationDetail = @"tblCustomerQuotationDetail";
        
		public static readonly string TblCylinder = @"tblCylinder";
        
		public static readonly string TblCylinderProcessing = @"tblCylinderProcessing";
        
		public static readonly string TblCylinderStatus = @"tblCylinderStatus";
        
		public static readonly string TblDebit = @"tblDebit";
        
		public static readonly string TblDebitDetail = @"tblDebitDetail";
        
		public static readonly string TblDeliveryOrder = @"tblDeliveryOrder";
        
		public static readonly string TblDeliveryOrderPackingDimension = @"tblDeliveryOrder_PackingDimension";
        
		public static readonly string TblDepartment = @"tblDepartment";
        
		public static readonly string TblEngraving = @"tblEngraving";
        
		public static readonly string TblEngravingScreenAngle = @"tblEngraving_ScreenAngle";
        
		public static readonly string TblEngravingStylu = @"tblEngraving_Stylus";
        
		public static readonly string TblEngravingDetail = @"tblEngravingDetail";
        
		public static readonly string TblEngravingEtching = @"tblEngravingEtching";
        
		public static readonly string TblEngravingTobacco = @"tblEngravingTobacco";
        
		public static readonly string TblFunction = @"tblFunction";
        
		public static readonly string TblInvoice = @"tblInvoice";
        
		public static readonly string TblInvoiceDetail = @"tblInvoiceDetail";
        
		public static readonly string TblInvoiceLockStatus = @"tblInvoiceLockStatus";
        
		public static readonly string TblJob = @"tblJob";
        
		public static readonly string TblJobProcess = @"tblJobProcess";
        
		public static readonly string TblJobQuotation = @"tblJobQuotation";
        
		public static readonly string TblJobQuotationPricing = @"tblJobQuotationPricing";
        
		public static readonly string TblJobSheet = @"tblJobSheet";
        
		public static readonly string TblLogging = @"tblLogging";
        
		public static readonly string TblMachinaryProduceType = @"tblMachinaryProduceType";
        
		public static readonly string TblMachine = @"tblMachine";
        
		public static readonly string TblNotification = @"tblNotification";
        
		public static readonly string TblNotificationSetting = @"tblNotificationSetting";
        
		public static readonly string TblObjectLocking = @"tblObjectLocking";
        
		public static readonly string TblOrderConfirmation = @"tblOrderConfirmation";
        
		public static readonly string TblOtherCharge = @"tblOtherCharges";
        
		public static readonly string TblPricing = @"tblPricing";
        
		public static readonly string TblProgress = @"tblProgress";
        
		public static readonly string TblProgressCylinderStatus = @"tblProgressCylinderStatus";
        
		public static readonly string TblProgressReproStatus = @"tblProgressReproStatus";
        
		public static readonly string TblPurchaseOrder = @"tblPurchaseOrder";
        
		public static readonly string TblPurchaseOrderCylinder = @"tblPurchaseOrder_Cylinder";
        
		public static readonly string TblReference = @"tblReferences";
        
		public static readonly string TblRole = @"tblRole";
        
		public static readonly string TblRolePermission = @"tblRolePermission";
        
		public static readonly string TblServiceJobDetail = @"tblServiceJobDetail";
        
		public static readonly string TblStaff = @"tblStaff";
        
		public static readonly string TblSupplier = @"tblSupplier";
        
		public static readonly string TblSupply = @"tblSupply";
        
		public static readonly string TblSystemSetting = @"tblSystemSetting";
        
		public static readonly string TblTax = @"tblTax";
        
		public static readonly string TblUser = @"tblUser";
        
		public static readonly string TblUserRole = @"tblUserRole";
        
		public static readonly string TblWorkFlow = @"tblWorkFlow";
        
		public static readonly string TblWorkFlowLine = @"tblWorkFlowLine";
        
		public static readonly string TblWorkFlowNode = @"tblWorkFlowNode";
        
		public static readonly string TblWorkTask = @"tblWorkTask";
        
		public static readonly string TblWorkTaskInNode = @"tblWorkTaskInNode";
        
	}
	#endregion
    #region Schemas
    public partial class Schemas {
		
		public static TableSchema.Table AspnetApplication
		{
            get { return DataService.GetSchema("aspnet_Applications", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table AspnetMembership
		{
            get { return DataService.GetSchema("aspnet_Membership", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table AspnetPath
		{
            get { return DataService.GetSchema("aspnet_Paths", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table AspnetPersonalizationAllUser
		{
            get { return DataService.GetSchema("aspnet_PersonalizationAllUsers", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table AspnetPersonalizationPerUser
		{
            get { return DataService.GetSchema("aspnet_PersonalizationPerUser", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table AspnetProfile
		{
            get { return DataService.GetSchema("aspnet_Profile", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table AspnetRole
		{
            get { return DataService.GetSchema("aspnet_Roles", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table AspnetSchemaVersion
		{
            get { return DataService.GetSchema("aspnet_SchemaVersions", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table AspnetUser
		{
            get { return DataService.GetSchema("aspnet_Users", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table AspnetUsersInRole
		{
            get { return DataService.GetSchema("aspnet_UsersInRoles", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table AspnetWebEventEvent
		{
            get { return DataService.GetSchema("aspnet_WebEvent_Events", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblBacking
		{
            get { return DataService.GetSchema("tblBacking", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblCodeDictionary
		{
            get { return DataService.GetSchema("tblCodeDictionary", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblContact
		{
            get { return DataService.GetSchema("tblContact", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblCredit
		{
            get { return DataService.GetSchema("tblCredit", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblCreditDetail
		{
            get { return DataService.GetSchema("tblCreditDetail", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblCurrency
		{
            get { return DataService.GetSchema("tblCurrency", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblCurrencyChangedLog
		{
            get { return DataService.GetSchema("TblCurrencyChangedLog", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblCustomer
		{
            get { return DataService.GetSchema("tblCustomer", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblCustomerQuotation
		{
            get { return DataService.GetSchema("tblCustomerQuotation", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblCustomerQuotationAdditionalService
		{
            get { return DataService.GetSchema("tblCustomerQuotation_AdditionalService", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblCustomerQuotationOtherCharge
		{
            get { return DataService.GetSchema("tblCustomerQuotation_OtherCharges", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblCustomerQuotationPricing
		{
            get { return DataService.GetSchema("tblCustomerQuotation_Pricing", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblCustomerQuotationDetail
		{
            get { return DataService.GetSchema("tblCustomerQuotationDetail", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblCylinder
		{
            get { return DataService.GetSchema("tblCylinder", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblCylinderProcessing
		{
            get { return DataService.GetSchema("tblCylinderProcessing", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblCylinderStatus
		{
            get { return DataService.GetSchema("tblCylinderStatus", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblDebit
		{
            get { return DataService.GetSchema("tblDebit", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblDebitDetail
		{
            get { return DataService.GetSchema("tblDebitDetail", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblDeliveryOrder
		{
            get { return DataService.GetSchema("tblDeliveryOrder", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblDeliveryOrderPackingDimension
		{
            get { return DataService.GetSchema("tblDeliveryOrder_PackingDimension", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblDepartment
		{
            get { return DataService.GetSchema("tblDepartment", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblEngraving
		{
            get { return DataService.GetSchema("tblEngraving", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblEngravingScreenAngle
		{
            get { return DataService.GetSchema("tblEngraving_ScreenAngle", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblEngravingStylu
		{
            get { return DataService.GetSchema("tblEngraving_Stylus", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblEngravingDetail
		{
            get { return DataService.GetSchema("tblEngravingDetail", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblEngravingEtching
		{
            get { return DataService.GetSchema("tblEngravingEtching", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblEngravingTobacco
		{
            get { return DataService.GetSchema("tblEngravingTobacco", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblFunction
		{
            get { return DataService.GetSchema("tblFunction", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblInvoice
		{
            get { return DataService.GetSchema("tblInvoice", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblInvoiceDetail
		{
            get { return DataService.GetSchema("tblInvoiceDetail", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblInvoiceLockStatus
		{
            get { return DataService.GetSchema("tblInvoiceLockStatus", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblJob
		{
            get { return DataService.GetSchema("tblJob", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblJobProcess
		{
            get { return DataService.GetSchema("tblJobProcess", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblJobQuotation
		{
            get { return DataService.GetSchema("tblJobQuotation", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblJobQuotationPricing
		{
            get { return DataService.GetSchema("tblJobQuotationPricing", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblJobSheet
		{
            get { return DataService.GetSchema("tblJobSheet", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblLogging
		{
            get { return DataService.GetSchema("tblLogging", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblMachinaryProduceType
		{
            get { return DataService.GetSchema("tblMachinaryProduceType", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblMachine
		{
            get { return DataService.GetSchema("tblMachine", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblNotification
		{
            get { return DataService.GetSchema("tblNotification", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblNotificationSetting
		{
            get { return DataService.GetSchema("tblNotificationSetting", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblObjectLocking
		{
            get { return DataService.GetSchema("tblObjectLocking", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblOrderConfirmation
		{
            get { return DataService.GetSchema("tblOrderConfirmation", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblOtherCharge
		{
            get { return DataService.GetSchema("tblOtherCharges", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblPricing
		{
            get { return DataService.GetSchema("tblPricing", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblProgress
		{
            get { return DataService.GetSchema("tblProgress", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblProgressCylinderStatus
		{
            get { return DataService.GetSchema("tblProgressCylinderStatus", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblProgressReproStatus
		{
            get { return DataService.GetSchema("tblProgressReproStatus", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblPurchaseOrder
		{
            get { return DataService.GetSchema("tblPurchaseOrder", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblPurchaseOrderCylinder
		{
            get { return DataService.GetSchema("tblPurchaseOrder_Cylinder", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblReference
		{
            get { return DataService.GetSchema("tblReferences", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblRole
		{
            get { return DataService.GetSchema("tblRole", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblRolePermission
		{
            get { return DataService.GetSchema("tblRolePermission", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblServiceJobDetail
		{
            get { return DataService.GetSchema("tblServiceJobDetail", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblStaff
		{
            get { return DataService.GetSchema("tblStaff", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblSupplier
		{
            get { return DataService.GetSchema("tblSupplier", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblSupply
		{
            get { return DataService.GetSchema("tblSupply", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblSystemSetting
		{
            get { return DataService.GetSchema("tblSystemSetting", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblTax
		{
            get { return DataService.GetSchema("tblTax", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblUser
		{
            get { return DataService.GetSchema("tblUser", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblUserRole
		{
            get { return DataService.GetSchema("tblUserRole", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblWorkFlow
		{
            get { return DataService.GetSchema("tblWorkFlow", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblWorkFlowLine
		{
            get { return DataService.GetSchema("tblWorkFlowLine", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblWorkFlowNode
		{
            get { return DataService.GetSchema("tblWorkFlowNode", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblWorkTask
		{
            get { return DataService.GetSchema("tblWorkTask", "DataAcessProvider"); }
		}
        
		public static TableSchema.Table TblWorkTaskInNode
		{
            get { return DataService.GetSchema("tblWorkTaskInNode", "DataAcessProvider"); }
		}
        
	
    }
    #endregion
    #region View Struct
    public partial struct Views 
    {
		
		public static readonly string VwAspnetApplication = @"vw_aspnet_Applications";
        
		public static readonly string VwAspnetMembershipUser = @"vw_aspnet_MembershipUsers";
        
		public static readonly string VwAspnetProfile = @"vw_aspnet_Profiles";
        
		public static readonly string VwAspnetRole = @"vw_aspnet_Roles";
        
		public static readonly string VwAspnetUser = @"vw_aspnet_Users";
        
		public static readonly string VwAspnetUsersInRole = @"vw_aspnet_UsersInRoles";
        
		public static readonly string VwAspnetWebPartStatePath = @"vw_aspnet_WebPartState_Paths";
        
		public static readonly string VwAspnetWebPartStateShared = @"vw_aspnet_WebPartState_Shared";
        
		public static readonly string VwAspnetWebPartStateUser = @"vw_aspnet_WebPartState_User";
        
    }
    #endregion
    
    #region Query Factories
	public static partial class DB
	{
        public static DataProvider _provider = DataService.Providers["DataAcessProvider"];
        static ISubSonicRepository _repository;
        public static ISubSonicRepository Repository 
        {
            get 
            {
                if (_repository == null)
                    return new SubSonicRepository(_provider);
                return _repository; 
            }
            set { _repository = value; }
        }
        public static Select SelectAllColumnsFrom<T>() where T : RecordBase<T>, new()
	    {
            return Repository.SelectAllColumnsFrom<T>();
	    }
	    public static Select Select()
	    {
            return Repository.Select();
	    }
	    
		public static Select Select(params string[] columns)
		{
            return Repository.Select(columns);
        }
	    
		public static Select Select(params Aggregate[] aggregates)
		{
            return Repository.Select(aggregates);
        }
   
	    public static Update Update<T>() where T : RecordBase<T>, new()
	    {
            return Repository.Update<T>();
	    }
	    
	    public static Insert Insert()
	    {
            return Repository.Insert();
	    }
	    
	    public static Delete Delete()
	    {
            return Repository.Delete();
	    }
	    
	    public static InlineQuery Query()
	    {
            return Repository.Query();
	    }
	    	    
	    
	}
    #endregion
    
}
#region Databases
public partial struct Databases 
{
	
	public static readonly string DataAcessProvider = @"DataAcessProvider";
    
}
#endregion