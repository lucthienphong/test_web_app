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
    /// Controller class for tblServiceJobInvoice
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TblServiceJobInvoiceController
    {
        // Preload our schema..
        TblServiceJobInvoice thisSchemaLoad = new TblServiceJobInvoice();
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
        public TblServiceJobInvoiceCollection FetchAll()
        {
            TblServiceJobInvoiceCollection coll = new TblServiceJobInvoiceCollection();
            Query qry = new Query(TblServiceJobInvoice.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblServiceJobInvoiceCollection FetchByID(object ServiceJobID)
        {
            TblServiceJobInvoiceCollection coll = new TblServiceJobInvoiceCollection().Where("ServiceJobID", ServiceJobID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblServiceJobInvoiceCollection FetchByQuery(Query qry)
        {
            TblServiceJobInvoiceCollection coll = new TblServiceJobInvoiceCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object ServiceJobID)
        {
            return (TblServiceJobInvoice.Delete(ServiceJobID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object ServiceJobID)
        {
            return (TblServiceJobInvoice.Destroy(ServiceJobID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int ServiceJobID,string InvoiceNumber,DateTime? InvoiceDate,string CustomerPONumber,string CreatedBy,DateTime? CreatedOn,string ModifiedBy,DateTime? ModifiedOn)
	    {
		    TblServiceJobInvoice item = new TblServiceJobInvoice();
		    
            item.ServiceJobID = ServiceJobID;
            
            item.InvoiceNumber = InvoiceNumber;
            
            item.InvoiceDate = InvoiceDate;
            
            item.CustomerPONumber = CustomerPONumber;
            
            item.CreatedBy = CreatedBy;
            
            item.CreatedOn = CreatedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.ModifiedOn = ModifiedOn;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int ServiceJobID,string InvoiceNumber,DateTime? InvoiceDate,string CustomerPONumber,string CreatedBy,DateTime? CreatedOn,string ModifiedBy,DateTime? ModifiedOn)
	    {
		    TblServiceJobInvoice item = new TblServiceJobInvoice();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.ServiceJobID = ServiceJobID;
				
			item.InvoiceNumber = InvoiceNumber;
				
			item.InvoiceDate = InvoiceDate;
				
			item.CustomerPONumber = CustomerPONumber;
				
			item.CreatedBy = CreatedBy;
				
			item.CreatedOn = CreatedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.ModifiedOn = ModifiedOn;
				
	        item.Save(UserName);
	    }
    }
}
