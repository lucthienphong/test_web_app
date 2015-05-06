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
    /// Controller class for tblTax
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TblTaxController
    {
        // Preload our schema..
        TblTax thisSchemaLoad = new TblTax();
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
        public TblTaxCollection FetchAll()
        {
            TblTaxCollection coll = new TblTaxCollection();
            Query qry = new Query(TblTax.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblTaxCollection FetchByID(object TaxID)
        {
            TblTaxCollection coll = new TblTaxCollection().Where("TaxID", TaxID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblTaxCollection FetchByQuery(Query qry)
        {
            TblTaxCollection coll = new TblTaxCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object TaxID)
        {
            return (TblTax.Delete(TaxID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object TaxID)
        {
            return (TblTax.Destroy(TaxID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string TaxCode,string TaxName,double TaxPercentage,bool IsObsolete)
	    {
		    TblTax item = new TblTax();
		    
            item.TaxCode = TaxCode;
            
            item.TaxName = TaxName;
            
            item.TaxPercentage = TaxPercentage;
            
            item.IsObsolete = IsObsolete;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(short TaxID,string TaxCode,string TaxName,double TaxPercentage,bool IsObsolete)
	    {
		    TblTax item = new TblTax();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.TaxID = TaxID;
				
			item.TaxCode = TaxCode;
				
			item.TaxName = TaxName;
				
			item.TaxPercentage = TaxPercentage;
				
			item.IsObsolete = IsObsolete;
				
	        item.Save(UserName);
	    }
    }
}