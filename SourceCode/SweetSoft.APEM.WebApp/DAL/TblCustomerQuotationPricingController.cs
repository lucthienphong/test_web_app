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
    /// Controller class for tblCustomerQuotation_Pricing
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TblCustomerQuotationPricingController
    {
        // Preload our schema..
        TblCustomerQuotationPricing thisSchemaLoad = new TblCustomerQuotationPricing();
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
        public TblCustomerQuotationPricingCollection FetchAll()
        {
            TblCustomerQuotationPricingCollection coll = new TblCustomerQuotationPricingCollection();
            Query qry = new Query(TblCustomerQuotationPricing.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblCustomerQuotationPricingCollection FetchByID(object CustomerID)
        {
            TblCustomerQuotationPricingCollection coll = new TblCustomerQuotationPricingCollection().Where("CustomerID", CustomerID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblCustomerQuotationPricingCollection FetchByQuery(Query qry)
        {
            TblCustomerQuotationPricingCollection coll = new TblCustomerQuotationPricingCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object CustomerID)
        {
            return (TblCustomerQuotationPricing.Delete(CustomerID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object CustomerID)
        {
            return (TblCustomerQuotationPricing.Destroy(CustomerID) == 1);
        }
        
        
        
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(int CustomerID,short PricingID)
        {
            Query qry = new Query(TblCustomerQuotationPricing.Schema);
            qry.QueryType = QueryType.Delete;
            qry.AddWhere("CustomerID", CustomerID).AND("PricingID", PricingID);
            qry.Execute();
            return (true);
        }        
       
    	
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int CustomerID,short PricingID,decimal OldSteelBasePrice,decimal NewSteelBasePrice)
	    {
		    TblCustomerQuotationPricing item = new TblCustomerQuotationPricing();
		    
            item.CustomerID = CustomerID;
            
            item.PricingID = PricingID;
            
            item.OldSteelBasePrice = OldSteelBasePrice;
            
            item.NewSteelBasePrice = NewSteelBasePrice;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int CustomerID,short PricingID,decimal OldSteelBasePrice,decimal NewSteelBasePrice)
	    {
		    TblCustomerQuotationPricing item = new TblCustomerQuotationPricing();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.CustomerID = CustomerID;
				
			item.PricingID = PricingID;
				
			item.OldSteelBasePrice = OldSteelBasePrice;
				
			item.NewSteelBasePrice = NewSteelBasePrice;
				
	        item.Save(UserName);
	    }
    }
}
