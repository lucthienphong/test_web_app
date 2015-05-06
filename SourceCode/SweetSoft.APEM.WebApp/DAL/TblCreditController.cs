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
    /// Controller class for tblCredit
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TblCreditController
    {
        // Preload our schema..
        TblCredit thisSchemaLoad = new TblCredit();
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
        public TblCreditCollection FetchAll()
        {
            TblCreditCollection coll = new TblCreditCollection();
            Query qry = new Query(TblCredit.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblCreditCollection FetchByID(object CreditID)
        {
            TblCreditCollection coll = new TblCreditCollection().Where("CreditID", CreditID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblCreditCollection FetchByQuery(Query qry)
        {
            TblCreditCollection coll = new TblCreditCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object CreditID)
        {
            return (TblCredit.Delete(CreditID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object CreditID)
        {
            return (TblCredit.Destroy(CreditID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string CreditNo,DateTime CreditDate,int CustomerID,short CurrencyID,string Remark,decimal Total,string CreatedBy,DateTime? CreatedOn,string ModifiedBy,DateTime? ModifiedOn)
	    {
		    TblCredit item = new TblCredit();
		    
            item.CreditNo = CreditNo;
            
            item.CreditDate = CreditDate;
            
            item.CustomerID = CustomerID;
            
            item.CurrencyID = CurrencyID;
            
            item.Remark = Remark;
            
            item.Total = Total;
            
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
	    public void Update(int CreditID,string CreditNo,DateTime CreditDate,int CustomerID,short CurrencyID,string Remark,decimal Total,string CreatedBy,DateTime? CreatedOn,string ModifiedBy,DateTime? ModifiedOn)
	    {
		    TblCredit item = new TblCredit();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.CreditID = CreditID;
				
			item.CreditNo = CreditNo;
				
			item.CreditDate = CreditDate;
				
			item.CustomerID = CustomerID;
				
			item.CurrencyID = CurrencyID;
				
			item.Remark = Remark;
				
			item.Total = Total;
				
			item.CreatedBy = CreatedBy;
				
			item.CreatedOn = CreatedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.ModifiedOn = ModifiedOn;
				
	        item.Save(UserName);
	    }
    }
}