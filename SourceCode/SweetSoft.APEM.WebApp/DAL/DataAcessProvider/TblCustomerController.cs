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
    /// <summary>
    /// Controller class for tblCustomer
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TblCustomerController
    {
        // Preload our schema..
        TblCustomer thisSchemaLoad = new TblCustomer();
        private string userName = String.Empty;
        protected string UserName
        {
            get
            {
				if (userName.Length == 0) 
				{
    				if (System.Web.HttpContext.Current != null)
    				{
						userName=System.Web.HttpContext.Current.User.Identity.Name;
					}
					else
					{
						userName=System.Threading.Thread.CurrentPrincipal.Identity.Name;
					}
				}
				return userName;
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public TblCustomerCollection FetchAll()
        {
            TblCustomerCollection coll = new TblCustomerCollection();
            Query qry = new Query(TblCustomer.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblCustomerCollection FetchByID(object CustomerID)
        {
            TblCustomerCollection coll = new TblCustomerCollection().Where("CustomerID", CustomerID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblCustomerCollection FetchByQuery(Query qry)
        {
            TblCustomerCollection coll = new TblCustomerCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object CustomerID)
        {
            return (TblCustomer.Delete(CustomerID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object CustomerID)
        {
            return (TblCustomer.Destroy(CustomerID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string Code,string Name,string Address,string City,string Tel,string Fax,string PostCode,string DeliveryNote,int? DeliveryID,string PaymentNote,int? PaymentID,int? SaleRepID,string SaleRecords,string PackagingRequirement,string TechData,string ForwarderName,string ForwarderAddress,bool IsObsolete,string CreatedBy,DateTime? CreatedOn,string ModifiedBy,DateTime? ModifiedOn,int CountryID,string TaxCode,string Tin,string Email,string ShortName,int? GroupID,string SAPCode,string InternalOrderNo,byte IsBrand)
	    {
		    TblCustomer item = new TblCustomer();
		    
            item.Code = Code;
            
            item.Name = Name;
            
            item.Address = Address;
            
            item.City = City;
            
            item.Tel = Tel;
            
            item.Fax = Fax;
            
            item.PostCode = PostCode;
            
            item.DeliveryNote = DeliveryNote;
            
            item.DeliveryID = DeliveryID;
            
            item.PaymentNote = PaymentNote;
            
            item.PaymentID = PaymentID;
            
            item.SaleRepID = SaleRepID;
            
            item.SaleRecords = SaleRecords;
            
            item.PackagingRequirement = PackagingRequirement;
            
            item.TechData = TechData;
            
            item.ForwarderName = ForwarderName;
            
            item.ForwarderAddress = ForwarderAddress;
            
            item.IsObsolete = IsObsolete;
            
            item.CreatedBy = CreatedBy;
            
            item.CreatedOn = CreatedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.CountryID = CountryID;
            
            item.TaxCode = TaxCode;
            
            item.Tin = Tin;
            
            item.Email = Email;
            
            item.ShortName = ShortName;
            
            item.GroupID = GroupID;
            
            item.SAPCode = SAPCode;
            
            item.InternalOrderNo = InternalOrderNo;
            
            item.IsBrand = IsBrand;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int CustomerID,string Code,string Name,string Address,string City,string Tel,string Fax,string PostCode,string DeliveryNote,int? DeliveryID,string PaymentNote,int? PaymentID,int? SaleRepID,string SaleRecords,string PackagingRequirement,string TechData,string ForwarderName,string ForwarderAddress,bool IsObsolete,string CreatedBy,DateTime? CreatedOn,string ModifiedBy,DateTime? ModifiedOn,int CountryID,string TaxCode,string Tin,string Email,string ShortName,int? GroupID,string SAPCode,string InternalOrderNo,byte IsBrand)
	    {
		    TblCustomer item = new TblCustomer();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.CustomerID = CustomerID;
				
			item.Code = Code;
				
			item.Name = Name;
				
			item.Address = Address;
				
			item.City = City;
				
			item.Tel = Tel;
				
			item.Fax = Fax;
				
			item.PostCode = PostCode;
				
			item.DeliveryNote = DeliveryNote;
				
			item.DeliveryID = DeliveryID;
				
			item.PaymentNote = PaymentNote;
				
			item.PaymentID = PaymentID;
				
			item.SaleRepID = SaleRepID;
				
			item.SaleRecords = SaleRecords;
				
			item.PackagingRequirement = PackagingRequirement;
				
			item.TechData = TechData;
				
			item.ForwarderName = ForwarderName;
				
			item.ForwarderAddress = ForwarderAddress;
				
			item.IsObsolete = IsObsolete;
				
			item.CreatedBy = CreatedBy;
				
			item.CreatedOn = CreatedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.CountryID = CountryID;
				
			item.TaxCode = TaxCode;
				
			item.Tin = Tin;
				
			item.Email = Email;
				
			item.ShortName = ShortName;
				
			item.GroupID = GroupID;
				
			item.SAPCode = SAPCode;
				
			item.InternalOrderNo = InternalOrderNo;
				
			item.IsBrand = IsBrand;
				
	        item.Save(UserName);
	    }
    }
}
